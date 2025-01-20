// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions;

internal class HtmlCodeActionProvider(IEditMappingService editMappingService) : IHtmlCodeActionProvider
{
    private readonly IEditMappingService _editMappingService = editMappingService;

    public async Task<ImmutableArray<RazorVSInternalCodeAction>> ProvideAsync(
        RazorCodeActionContext context,
        ImmutableArray<RazorVSInternalCodeAction> codeActions,
        CancellationToken cancellationToken)
    {
        using var results = new PooledArrayBuilder<RazorVSInternalCodeAction>(codeActions.Length);
        foreach (var codeAction in codeActions)
        {
            if (codeAction.Edit is not null)
            {
                await RemapAndFixHtmlCodeActionEditAsync(_editMappingService, context.DocumentSnapshot, codeAction, cancellationToken).ConfigureAwait(false);

                results.Add(codeAction);
            }
            else
            {
                results.Add(codeAction.WrapResolvableCodeAction(context, language: RazorLanguageKind.Html));
            }
        }

        return results.ToImmutable();
    }

    public static async Task RemapAndFixHtmlCodeActionEditAsync(IEditMappingService editMappingService, IDocumentSnapshot documentSnapshot, CodeAction codeAction, CancellationToken cancellationToken)
    {
        Assumes.NotNull(codeAction.Edit);

        codeAction.Edit = await editMappingService.RemapWorkspaceEditAsync(documentSnapshot, codeAction.Edit, cancellationToken).ConfigureAwait(false);

        if (codeAction.Edit.TryGetTextDocumentEdits(out var documentEdits))
        {
            var codeDocument = await documentSnapshot.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
            var htmlSourceText = codeDocument.GetHtmlSourceText();

            foreach (var edit in documentEdits)
            {
                edit.Edits = FormattingUtilities.FixHtmlTextEdits(htmlSourceText, edit.Edits);
            }

            codeAction.Edit = new WorkspaceEdit
            {
                DocumentChanges = documentEdits
            };
        }
    }
}
