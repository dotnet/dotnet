// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal sealed class DefaultHtmlCodeActionResolver(
    IDocumentContextFactory documentContextFactory,
    IClientConnection clientConnection,
    IEditMappingService editMappingService) : HtmlCodeActionResolver(clientConnection)
{
    private readonly IDocumentContextFactory _documentContextFactory = documentContextFactory;
    private readonly IEditMappingService _editMappingService = editMappingService;

    public override string Action => LanguageServerConstants.CodeActions.Default;

    public async override Task<CodeAction> ResolveAsync(
        CodeActionResolveParams resolveParams,
        CodeAction codeAction,
        CancellationToken cancellationToken)
    {
        if (!_documentContextFactory.TryCreateForOpenDocument(resolveParams.RazorFileIdentifier, out var documentContext))
        {
            return codeAction;
        }

        var resolvedCodeAction = await ResolveCodeActionWithServerAsync(resolveParams.RazorFileIdentifier, documentContext.Version, RazorLanguageKind.Html, codeAction, cancellationToken).ConfigureAwait(false);
        if (resolvedCodeAction?.Edit?.DocumentChanges is null)
        {
            // Unable to resolve code action with server, return original code action
            return codeAction;
        }

        await DefaultHtmlCodeActionProvider.RemapAndFixHtmlCodeActionEditAsync(_editMappingService, documentContext.Snapshot, resolvedCodeAction, cancellationToken).ConfigureAwait(false);

        return resolvedCodeAction;
    }
}
