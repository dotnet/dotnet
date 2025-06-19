// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions;

/// <summary>
/// Resolves and remaps the code action, without running formatting passes.
/// </summary>
internal class UnformattedRemappingCSharpCodeActionResolver(IDocumentMappingService documentMappingService) : ICSharpCodeActionResolver
{
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;

    public string Action => LanguageServerConstants.CodeActions.UnformattedRemap;

    public async Task<CodeAction> ResolveAsync(
        DocumentContext documentContext,
        CodeAction codeAction,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (codeAction.Edit?.DocumentChanges is null)
        {
            // Unable to resolve code action with server, return original code action
            return codeAction;
        }

        if (codeAction.Edit.DocumentChanges.Value.Count() != 1)
        {
            // We don't yet support multi-document code actions, return original code action
            Debug.Fail($"Encountered an unsupported multi-document code action edit with ${codeAction.Title}.");
            return codeAction;
        }

        var documentChanged = codeAction.Edit.DocumentChanges.Value.First();
        if (!documentChanged.TryGetFirst(out var textDocumentEdit))
        {
            // Only Text Document Edit changes are supported currently, return original code action
            return codeAction;
        }

        var textEdit = (TextEdit)textDocumentEdit.Edits.FirstOrDefault();
        if (textEdit is null)
        {
            // No text edit available
            return codeAction;
        }

        var codeDocument = await documentContext.Snapshot.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);

        if (!_documentMappingService.TryMapToHostDocumentRange(codeDocument.GetRequiredCSharpDocument(), textEdit.Range, MappingBehavior.Inclusive, out var originalRange))
        {
            // Text edit failed to map
            return codeAction;
        }

        textEdit.Range = originalRange;

        var codeDocumentIdentifier = new OptionalVersionedTextDocumentIdentifier()
        {
            Uri = documentContext.Uri,
        };
        codeAction.Edit = new WorkspaceEdit()
        {
            DocumentChanges = new[] {
                new TextDocumentEdit()
                {
                    TextDocument = codeDocumentIdentifier,
                    Edits = [textEdit],
                }
            },
        };

        return codeAction;
    }
}
