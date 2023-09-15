// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal sealed class ExtractToCodeBehindCodeActionResolver : IRazorCodeActionResolver
{
    private static readonly Workspace s_workspace = new AdhocWorkspace();

    private readonly DocumentContextFactory _documentContextFactory;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly ClientNotifierServiceBase _languageServer;

    public ExtractToCodeBehindCodeActionResolver(
        DocumentContextFactory documentContextFactory,
        LanguageServerFeatureOptions languageServerFeatureOptions,
        ClientNotifierServiceBase languageServer)
    {
        _documentContextFactory = documentContextFactory ?? throw new ArgumentNullException(nameof(documentContextFactory));
        _languageServerFeatureOptions = languageServerFeatureOptions ?? throw new ArgumentNullException(nameof(languageServerFeatureOptions));
        _languageServer = languageServer ?? throw new ArgumentNullException(nameof(languageServer));
    }

    public string Action => LanguageServerConstants.CodeActions.ExtractToCodeBehindAction;

    public async Task<WorkspaceEdit?> ResolveAsync(JObject data, CancellationToken cancellationToken)
    {
        if (data is null)
        {
            return null;
        }

        var actionParams = data.ToObject<ExtractToCodeBehindCodeActionParams>();
        if (actionParams is null)
        {
            return null;
        }

        var path = FilePathNormalizer.Normalize(actionParams.Uri.GetAbsoluteOrUNCPath());

        var documentContext = _documentContextFactory.TryCreate(actionParams.Uri);
        if (documentContext is null)
        {
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            return null;
        }

        if (!FileKinds.IsComponent(codeDocument.GetFileKind()))
        {
            return null;
        }

        var codeBehindPath = GenerateCodeBehindPath(path);

        // VS Code in Windows expects path to start with '/'
        var updatedCodeBehindPath = _languageServerFeatureOptions.ReturnCodeActionAndRenamePathsWithPrefixedSlash && !codeBehindPath.StartsWith("/")
            ? '/' + codeBehindPath
            : codeBehindPath;

        var codeBehindUri = new UriBuilder
        {
            Scheme = Uri.UriSchemeFile,
            Path = updatedCodeBehindPath,
            Host = string.Empty,
        }.Uri;

        var text = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (text is null)
        {
            return null;
        }

        var className = Path.GetFileNameWithoutExtension(path);
        var codeBlockContent = text.GetSubTextString(new CodeAnalysis.Text.TextSpan(actionParams.ExtractStart, actionParams.ExtractEnd - actionParams.ExtractStart)).Trim();
        var codeBehindContent = await GenerateCodeBehindClassAsync(documentContext.Project, codeBehindUri, className, actionParams.Namespace, codeBlockContent, codeDocument, cancellationToken).ConfigureAwait(false);

        var start = codeDocument.Source.Lines.GetLocation(actionParams.RemoveStart);
        var end = codeDocument.Source.Lines.GetLocation(actionParams.RemoveEnd);
        var removeRange = new Range
        {
            Start = new Position(start.LineIndex, start.CharacterIndex),
            End = new Position(end.LineIndex, end.CharacterIndex)
        };

        var codeDocumentIdentifier = new OptionalVersionedTextDocumentIdentifier { Uri = actionParams.Uri };
        var codeBehindDocumentIdentifier = new OptionalVersionedTextDocumentIdentifier { Uri = codeBehindUri };

        var documentChanges = new SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[]
        {
            new CreateFile { Uri = codeBehindUri },
            new TextDocumentEdit
            {
                TextDocument = codeDocumentIdentifier,
                Edits = new[]
                {
                    new TextEdit
                    {
                        NewText = string.Empty,
                        Range = removeRange,
                    }
                },
            },
            new TextDocumentEdit
            {
                TextDocument = codeBehindDocumentIdentifier,
                Edits  = new[]
                {
                    new TextEdit
                    {
                        NewText = codeBehindContent,
                        Range = new Range { Start = new Position(0, 0), End = new Position(0, 0) },
                    }
                },
            }
        };

        return new WorkspaceEdit
        {
            DocumentChanges = documentChanges,
        };
    }

    /// <summary>
    /// Generate a file path with adjacent to our input path that has the
    /// correct codebehind extension, using numbers to differentiate from
    /// any collisions.
    /// </summary>
    /// <param name="path">The origin file path.</param>
    /// <returns>A non-existent file path with the same base name and a codebehind extension.</returns>
    private static string GenerateCodeBehindPath(string path)
    {
        var n = 0;
        string codeBehindPath;
        do
        {
            var identifier = n > 0 ? n.ToString(CultureInfo.InvariantCulture) : string.Empty;  // Make it look nice
            var directoryName = Path.GetDirectoryName(path);
            Assumes.NotNull(directoryName);

            codeBehindPath = Path.Combine(
                directoryName,
                $"{Path.GetFileNameWithoutExtension(path)}{identifier}{Path.GetExtension(path)}.cs");
            n++;
        }
        while (File.Exists(codeBehindPath));

        return codeBehindPath;
    }

    private async Task<string> GenerateCodeBehindClassAsync(CodeAnalysis.Razor.ProjectSystem.IProjectSnapshot project, Uri codeBehindUri, string className, string namespaceName, string contents, RazorCodeDocument razorCodeDocument, CancellationToken cancellationToken)
    {
        using var _ = StringBuilderPool.GetPooledObject(out var builder);

        var usingDirectives = razorCodeDocument
            .GetDocumentIntermediateNode()
            .FindDescendantNodes<UsingDirectiveIntermediateNode>();
        foreach (var usingDirective in usingDirectives)
        {
            builder.Append("using ");

            var content = usingDirective.Content;
            var startIndex = content.StartsWith("global::", StringComparison.Ordinal)
                ? 8
                : 0;

            builder.Append(content, startIndex, content.Length - startIndex);
            builder.Append(';');
            builder.AppendLine();
        }

        builder.AppendLine();
        builder.Append("namespace ");
        builder.AppendLine(namespaceName);
        builder.Append('{');
        builder.AppendLine();
        builder.Append("public partial class ");
        builder.AppendLine(className);
        builder.AppendLine(contents);
        builder.Append('}');

        var newFileContent = builder.ToString();

        var parameters = new FormatNewFileParams()
        {
            Project = new TextDocumentIdentifier
            {
                Uri = new Uri(project.FilePath, UriKind.Absolute)
            },
            Document = new TextDocumentIdentifier
            {
                Uri = codeBehindUri
            },
            Contents = newFileContent
        };
        var fixedContent = await _languageServer.SendRequestAsync<FormatNewFileParams, string?>(CustomMessageNames.RazorFormatNewFileEndpointName, parameters, cancellationToken).ConfigureAwait(false);

        if (fixedContent is null)
        {
            // Sadly we can't use a "real" workspace here, because we don't have access. If we use our workspace, it wouldn't have the right settings
            // for C# formatting, only Razor formatting, and we have no access to Roslyn's real workspace, since it could be in another process.
            var node = await CSharpSyntaxTree.ParseText(newFileContent).GetRootAsync(cancellationToken).ConfigureAwait(false);
            node = Formatter.Format(node, s_workspace);

            return node.ToFullString();
        }

        return fixedContent;
    }
}
