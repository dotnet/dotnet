// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

internal class DelegatedCompletionItemResolver : CompletionItemResolver
{
    private readonly DocumentContextFactory _documentContextFactory;
    private readonly RazorFormattingService _formattingService;
    private readonly ClientNotifierServiceBase _languageServer;

    public DelegatedCompletionItemResolver(
        DocumentContextFactory documentContextFactory,
        RazorFormattingService formattingService,
        ClientNotifierServiceBase languageServer)
    {
        _documentContextFactory = documentContextFactory;
        _formattingService = formattingService;
        _languageServer = languageServer;
    }

    public override async Task<VSInternalCompletionItem?> ResolveAsync(
        VSInternalCompletionItem item,
        VSInternalCompletionList containingCompletionList,
        object? originalRequestContext,
        VSInternalClientCapabilities? clientCapabilities,
        CancellationToken cancellationToken)
    {
        if (originalRequestContext is not DelegatedCompletionResolutionContext resolutionContext)
        {
            // Can't recognize the original request context, bail.
            return null;
        }

        var labelQuery = item.Label;
        var associatedDelegatedCompletion = containingCompletionList.Items.FirstOrDefault(completion => string.Equals(labelQuery, completion.Label, StringComparison.Ordinal));
        if (associatedDelegatedCompletion is null)
        {
            return null;
        }

        item.Data = associatedDelegatedCompletion.Data ?? resolutionContext.OriginalCompletionListData;

        var delegatedParams = resolutionContext.OriginalRequestParams;
        var delegatedResolveParams = new DelegatedCompletionItemResolveParams(
            delegatedParams.HostDocument,
            item,
            delegatedParams.ProjectedKind);
        var resolvedCompletionItem = await _languageServer.SendRequestAsync<DelegatedCompletionItemResolveParams, VSInternalCompletionItem?>(Common.LanguageServerConstants.RazorCompletionResolveEndpointName, delegatedResolveParams, cancellationToken).ConfigureAwait(false);

        if (resolvedCompletionItem is not null)
        {
            resolvedCompletionItem = await PostProcessCompletionItemAsync(resolutionContext, resolvedCompletionItem, cancellationToken).ConfigureAwait(false);
        }

        return resolvedCompletionItem;
    }

    private async Task<VSInternalCompletionItem> PostProcessCompletionItemAsync(
        DelegatedCompletionResolutionContext context,
        VSInternalCompletionItem resolvedCompletionItem,
        CancellationToken cancellationToken)
    {
        if (context.OriginalRequestParams.ProjectedKind != RazorLanguageKind.CSharp)
        {
            // We currently don't do any post-processing for non-C# items.
            return resolvedCompletionItem;
        }

        if (!resolvedCompletionItem.VsResolveTextEditOnCommit)
        {
            // Resolve doesn't typically handle text edit resolution; however, in VS cases it does.
            return resolvedCompletionItem;
        }

        if (resolvedCompletionItem.TextEdit is null && resolvedCompletionItem.AdditionalTextEdits is null)
        {
            // Only post-processing work we have to do is formatting text edits on resolution.
            return resolvedCompletionItem;
        }

        var hostDocumentUri = context.OriginalRequestParams.HostDocument.Uri;
        var documentContext = await _documentContextFactory.TryCreateAsync(hostDocumentUri, cancellationToken).ConfigureAwait(false);
        if (documentContext is null)
        {
            return resolvedCompletionItem;
        }

        var formattingOptions = await _languageServer.SendRequestAsync<VersionedTextDocumentIdentifier, FormattingOptions?>(Common.LanguageServerConstants.RazorGetFormattingOptionsEndpointName, documentContext.Identifier, cancellationToken).ConfigureAwait(false);
        if (formattingOptions is null)
        {
            return resolvedCompletionItem;
        }

        if (resolvedCompletionItem.TextEdit is not null)
        {
            var formattedTextEdit = await _formattingService.FormatSnippetAsync(
                documentContext,
                RazorLanguageKind.CSharp,
                new[] { resolvedCompletionItem.TextEdit },
                formattingOptions,
                cancellationToken).ConfigureAwait(false);

            resolvedCompletionItem.TextEdit = formattedTextEdit.FirstOrDefault();
        }

        if (resolvedCompletionItem.AdditionalTextEdits is not null)
        {
            var formattedTextEdits = await _formattingService.FormatSnippetAsync(
                documentContext,
                RazorLanguageKind.CSharp,
                resolvedCompletionItem.AdditionalTextEdits,
                formattingOptions,
                cancellationToken).ConfigureAwait(false);

            resolvedCompletionItem.AdditionalTextEdits = formattedTextEdits;
        }

        return resolvedCompletionItem;
    }
}
