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
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions;

internal class CreateComponentCodeActionResolver(LanguageServerFeatureOptions languageServerFeatureOptions) : IRazorCodeActionResolver
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;

    public string Action => LanguageServerConstants.CodeActions.CreateComponentFromTag;

    public async Task<WorkspaceEdit?> ResolveAsync(DocumentContext documentContext, JsonElement data, RazorFormattingOptions options, CancellationToken cancellationToken)
    {
        var actionParams = data.Deserialize<CreateComponentCodeActionParams>();
        if (actionParams is null)
        {
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            return null;
        }

        if (!FileKinds.IsComponent(codeDocument.FileKind))
        {
            return null;
        }

        // VS Code in Windows expects path to start with '/'
        var updatedPath = _languageServerFeatureOptions.ReturnCodeActionAndRenamePathsWithPrefixedSlash && !actionParams.Path.StartsWith("/")
            ? '/' + actionParams.Path
            : actionParams.Path;
        var newComponentUri = VsLspFactory.CreateFilePathUri(updatedPath);

        using var documentChanges = new PooledArrayBuilder<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>>();
        documentChanges.Add(new CreateFile() { Uri = newComponentUri });

        TryAddNamespaceDirective(codeDocument, newComponentUri, ref documentChanges.AsRef());

        return new WorkspaceEdit()
        {
            DocumentChanges = documentChanges.ToArray(),
        };
    }

    private static void TryAddNamespaceDirective(RazorCodeDocument codeDocument, Uri newComponentUri, ref PooledArrayBuilder<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>> documentChanges)
    {
        var syntaxTree = codeDocument.GetSyntaxTree();
        var namespaceDirective = syntaxTree.Root.DescendantNodes()
            .Where(n => n.Kind == SyntaxKind.RazorDirective)
            .Cast<RazorDirectiveSyntax>()
            .FirstOrDefault(static n => n.DirectiveDescriptor == NamespaceDirective.Directive);

        if (namespaceDirective != null)
        {
            var documentIdentifier = new OptionalVersionedTextDocumentIdentifier { Uri = newComponentUri };
            documentChanges.Add(new TextDocumentEdit
            {
                TextDocument = documentIdentifier,
                Edits = [VsLspFactory.CreateTextEdit(position: (0, 0), namespaceDirective.GetContent())]
            });
        }
    }
}
