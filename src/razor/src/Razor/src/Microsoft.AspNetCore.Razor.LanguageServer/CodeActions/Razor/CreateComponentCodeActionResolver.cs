// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal sealed class CreateComponentCodeActionResolver(IDocumentContextFactory documentContextFactory, LanguageServerFeatureOptions languageServerFeatureOptions) : IRazorCodeActionResolver
{
    private readonly IDocumentContextFactory _documentContextFactory = documentContextFactory;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;

    public string Action => LanguageServerConstants.CodeActions.CreateComponentFromTag;

    public async Task<WorkspaceEdit?> ResolveAsync(JsonElement data, CancellationToken cancellationToken)
    {
        var actionParams = data.Deserialize<CreateComponentCodeActionParams>();
        if (actionParams is null)
        {
            return null;
        }

        if (!_documentContextFactory.TryCreate(actionParams.Uri, out var documentContext))
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

        // VS Code in Windows expects path to start with '/'
        var updatedPath = _languageServerFeatureOptions.ReturnCodeActionAndRenamePathsWithPrefixedSlash && !actionParams.Path.StartsWith("/")
            ? '/' + actionParams.Path
            : actionParams.Path;
        var newComponentUri = new UriBuilder()
        {
            Scheme = Uri.UriSchemeFile,
            Path = updatedPath,
            Host = string.Empty,
        }.Uri;

        var documentChanges = new List<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>>
        {
            new CreateFile() { Uri = newComponentUri },
        };

        TryAddNamespaceDirective(codeDocument, newComponentUri, documentChanges);

        return new WorkspaceEdit()
        {
            DocumentChanges = documentChanges.ToArray(),
        };
    }

    private static void TryAddNamespaceDirective(RazorCodeDocument codeDocument, Uri newComponentUri, List<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>> documentChanges)
    {
        var syntaxTree = codeDocument.GetSyntaxTree();
        var namespaceDirective = syntaxTree.Root.DescendantNodes()
            .Where(n => n.Kind == SyntaxKind.RazorDirective)
            .Cast<RazorDirectiveSyntax>()
            .Where(n => n.DirectiveDescriptor == NamespaceDirective.Directive)
            .FirstOrDefault();

        if (namespaceDirective != null)
        {
            var documentIdentifier = new OptionalVersionedTextDocumentIdentifier { Uri = newComponentUri };
            documentChanges.Add(new TextDocumentEdit
            {
                TextDocument = documentIdentifier,
                Edits =
                [
                    new TextEdit()
                    {
                        NewText = namespaceDirective.GetContent(),
                        Range = new Range{ Start = new Position(0, 0), End = new Position(0, 0) },
                    }
                ]
            });
        }
    }
}
