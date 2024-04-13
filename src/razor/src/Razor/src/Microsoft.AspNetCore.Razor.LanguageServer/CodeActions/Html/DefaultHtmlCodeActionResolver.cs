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

internal sealed class DefaultHtmlCodeActionResolver : HtmlCodeActionResolver
{
    private readonly IDocumentContextFactory _documentContextFactory;
    private readonly IRazorDocumentMappingService _documentMappingService;

    public DefaultHtmlCodeActionResolver(
        IDocumentContextFactory documentContextFactory,
        IClientConnection clientConnection,
        IRazorDocumentMappingService documentMappingService)
        : base(clientConnection)
    {
        if (documentContextFactory is null)
        {
            throw new ArgumentNullException(nameof(documentContextFactory));
        }

        if (documentMappingService is null)
        {
            throw new ArgumentNullException(nameof(documentMappingService));
        }

        _documentContextFactory = documentContextFactory;
        _documentMappingService = documentMappingService;
    }

    public override string Action => LanguageServerConstants.CodeActions.Default;

    public async override Task<CodeAction> ResolveAsync(
        CodeActionResolveParams resolveParams,
        CodeAction codeAction,
        CancellationToken cancellationToken)
    {
        if (resolveParams is null)
        {
            throw new ArgumentNullException(nameof(resolveParams));
        }

        if (codeAction is null)
        {
            throw new ArgumentNullException(nameof(codeAction));
        }

        var documentContext = await _documentContextFactory.TryCreateForOpenDocumentAsync(resolveParams.RazorFileIdentifier, cancellationToken).ConfigureAwait(false);
        if (documentContext is null)
        {
            return codeAction;
        }

        var resolvedCodeAction = await ResolveCodeActionWithServerAsync(resolveParams.RazorFileIdentifier, documentContext.Version, RazorLanguageKind.Html, codeAction, cancellationToken).ConfigureAwait(false);
        if (resolvedCodeAction?.Edit?.DocumentChanges is null)
        {
            // Unable to resolve code action with server, return original code action
            return codeAction;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        await DefaultHtmlCodeActionProvider.RemapAndFixHtmlCodeActionEditAsync(_documentMappingService, codeDocument, resolvedCodeAction, cancellationToken).ConfigureAwait(false);

        return resolvedCodeAction;
    }
}
