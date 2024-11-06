// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions;

internal sealed class ExtractToCodeBehindCodeActionResolver(
    LanguageServerFeatureOptions languageServerFeatureOptions,
    IRoslynCodeActionHelpers roslynCodeActionHelpers) : IRazorCodeActionResolver
{
    private static readonly Workspace s_workspace = new AdhocWorkspace();

    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly IRoslynCodeActionHelpers _roslynCodeActionHelpers = roslynCodeActionHelpers;

    public string Action => LanguageServerConstants.CodeActions.ExtractToCodeBehindAction;

    public async Task<WorkspaceEdit?> ResolveAsync(DocumentContext documentContext, JsonElement data, RazorFormattingOptions options, CancellationToken cancellationToken)
    {
        var actionParams = data.Deserialize<ExtractToCodeBehindCodeActionParams>();
        if (actionParams is null)
        {
            return null;
        }

        var path = FilePathNormalizer.Normalize(documentContext.Uri.GetAbsoluteOrUNCPath());

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            return null;
        }

        if (!FileKinds.IsComponent(codeDocument.GetFileKind()))
        {
            return null;
        }

        var codeBehindPath = FileUtilities.GenerateUniquePath(path, $"{Path.GetExtension(path)}.cs");

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

        var className = Path.GetFileNameWithoutExtension(path);
        var codeBlockContent = text.GetSubTextString(new CodeAnalysis.Text.TextSpan(actionParams.ExtractStart, actionParams.ExtractEnd - actionParams.ExtractStart)).Trim();
        var codeBehindContent = await GenerateCodeBehindClassAsync(documentContext.Project, codeBehindUri, className, actionParams.Namespace, codeBlockContent, codeDocument, cancellationToken).ConfigureAwait(false);

        var removeRange = codeDocument.Source.Text.GetRange(actionParams.RemoveStart, actionParams.RemoveEnd);

        var codeDocumentIdentifier = new OptionalVersionedTextDocumentIdentifier { Uri = documentContext.Uri };
        var codeBehindDocumentIdentifier = new OptionalVersionedTextDocumentIdentifier { Uri = codeBehindUri };

        var documentChanges = new SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[]
        {
            new CreateFile { Uri = codeBehindUri },
            new TextDocumentEdit
            {
                TextDocument = codeDocumentIdentifier,
                Edits = [VsLspFactory.CreateTextEdit(removeRange, string.Empty)]
            },
            new TextDocumentEdit
            {
                TextDocument = codeBehindDocumentIdentifier,
                Edits = [VsLspFactory.CreateTextEdit(position: (0, 0), codeBehindContent)]
            }
        };

        return new WorkspaceEdit
        {
            DocumentChanges = documentChanges,
        };
    }

    private async Task<string> GenerateCodeBehindClassAsync(IProjectSnapshot project, Uri codeBehindUri, string className, string namespaceName, string contents, RazorCodeDocument razorCodeDocument, CancellationToken cancellationToken)
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

        var fixedContent = await _roslynCodeActionHelpers.GetFormattedNewFileContentsAsync(project.FilePath, codeBehindUri, newFileContent, cancellationToken).ConfigureAwait(false);

        if (fixedContent is null)
        {
            // Sadly we can't use a "real" workspace here, because we don't have access. If we use our workspace, it wouldn't have the right settings
            // for C# formatting, only Razor formatting, and we have no access to Roslyn's real workspace, since it could be in another process.
            var node = await CSharpSyntaxTree.ParseText(newFileContent, cancellationToken: cancellationToken).GetRootAsync(cancellationToken).ConfigureAwait(false);
            node = Formatter.Format(node, s_workspace, cancellationToken: cancellationToken);

            return node.ToFullString();
        }

        return fixedContent;
    }
}
