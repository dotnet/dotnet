// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal class DefaultHtmlCodeActionProvider : HtmlCodeActionProvider
{
    private readonly RazorDocumentMappingService _documentMappingService;

    public DefaultHtmlCodeActionProvider(RazorDocumentMappingService documentMappingService)
    {
        _documentMappingService = documentMappingService;
    }

    public override async Task<IReadOnlyList<RazorVSInternalCodeAction>?> ProvideAsync(
        RazorCodeActionContext context,
        IEnumerable<RazorVSInternalCodeAction> codeActions,
        CancellationToken cancellationToken)
    {
        var results = new List<RazorVSInternalCodeAction>();

        foreach (var codeAction in codeActions)
        {
            if (codeAction.Edit is not null)
            {
                await RemapeAndFixHtmlCodeActionEditAsync(_documentMappingService, context.CodeDocument, codeAction, cancellationToken).ConfigureAwait(false);

                results.Add(codeAction);
            }
            else
            {
                results.Add(codeAction.WrapResolvableCodeAction(context, language: LanguageServerConstants.CodeActions.Languages.Html));
            }
        }

        return results;
    }

    public static async Task RemapeAndFixHtmlCodeActionEditAsync(RazorDocumentMappingService documentMappingService, RazorCodeDocument codeDocument, CodeAction codeAction, CancellationToken cancellationToken)
    {
        Assumes.NotNull(codeAction.Edit);

        codeAction.Edit = await documentMappingService.RemapWorkspaceEditAsync(codeAction.Edit, cancellationToken).ConfigureAwait(false);

        if (codeAction.Edit.TryGetDocumentChanges(out var documentEdits) == true)
        {
            var htmlSourceText = codeDocument.GetHtmlSourceText();

            foreach (var edit in documentEdits)
            {
                edit.Edits = HtmlFormatter.FixHtmlTestEdits(htmlSourceText, edit.Edits);
            }

            codeAction.Edit = new WorkspaceEdit
            {
                DocumentChanges = documentEdits
            };
        }
    }
}
