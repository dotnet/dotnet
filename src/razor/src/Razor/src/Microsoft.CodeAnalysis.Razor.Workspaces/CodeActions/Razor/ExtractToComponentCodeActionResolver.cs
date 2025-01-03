// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#if !NET
using System;
#endif
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions.Razor;

internal class ExtractToComponentCodeActionResolver(
    LanguageServerFeatureOptions languageServerFeatureOptions) : IRazorCodeActionResolver
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;

    public string Action => LanguageServerConstants.CodeActions.ExtractToNewComponentAction;

    public async Task<WorkspaceEdit?> ResolveAsync(DocumentContext documentContext, JsonElement data, RazorFormattingOptions options, CancellationToken cancellationToken)
    {
        if (data.ValueKind == JsonValueKind.Undefined)
        {
            return null;
        }

        var actionParams = JsonSerializer.Deserialize<ExtractToComponentCodeActionParams>(data.GetRawText());
        if (actionParams is null)
        {
            return null;
        }

        var componentDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (componentDocument.IsUnsupported())
        {
            return null;
        }

        var text = componentDocument.Source.Text;
        var path = FilePathNormalizer.Normalize(documentContext.Uri.GetAbsoluteOrUNCPath());
        var directoryName = Path.GetDirectoryName(path).AssumeNotNull();
        var templatePath = Path.Combine(directoryName, "Component.razor");
        var componentPath = FileUtilities.GenerateUniquePath(templatePath, ".razor");
        var componentName = Path.GetFileNameWithoutExtension(componentPath);

        // VS Code in Windows expects path to start with '/'
        componentPath = _languageServerFeatureOptions.ReturnCodeActionAndRenamePathsWithPrefixedSlash && !componentPath.StartsWith('/')
            ? '/' + componentPath
            : componentPath;

        var newComponentUri = VsLspFactory.CreateFilePathUri(componentPath);

        using var _ = StringBuilderPool.GetPooledObject(out var builder);

        var syntaxTree = componentDocument.GetSyntaxTree();

        // Right now this includes all the usings in the original document.
        // https://github.com/dotnet/razor/issues/11025 tracks reducing to only the required set.
        var usingDirectives = syntaxTree.GetUsingDirectives();
        foreach (var usingDirective in usingDirectives)
        {
            builder.AppendLine(usingDirective.ToFullString());
        }

        // If any using directives were added, add a newline before the extracted content.
        if (usingDirectives.Length > 0)
        {
            builder.AppendLine();
        }

        var span = TextSpan.FromBounds(actionParams.Start, actionParams.End);
        FormattingUtilities.NaivelyUnindentSubstring(text, span, builder);

        var removeRange = text.GetRange(actionParams.Start, actionParams.End);

        var documentChanges = new SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[]
        {
            new CreateFile { Uri = newComponentUri },
            new TextDocumentEdit
            {
                TextDocument = new OptionalVersionedTextDocumentIdentifier { Uri = documentContext.Uri },
                Edits =
                [
                    new TextEdit
                    {
                        NewText = $"<{componentName} />",
                        Range = removeRange,
                    }
                ],
            },
            new TextDocumentEdit
            {
                TextDocument = new OptionalVersionedTextDocumentIdentifier { Uri = newComponentUri },
                Edits  =
                [
                    new TextEdit
                    {
                        NewText = builder.ToString(),
                        Range = VsLspFactory.DefaultRange,
                    }
                ],
            }
        };

        return new WorkspaceEdit
        {
            DocumentChanges = documentChanges,
        };
    }
}
