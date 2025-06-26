// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Completion.Delegation;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Tooltip;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

internal class DelegatedCompletionItemResolver(
    IDocumentContextFactory documentContextFactory,
    IRazorFormattingService formattingService,
    IDocumentMappingService documentMappingService,
    RazorLSPOptionsMonitor optionsMonitor,
    IClientConnection clientConnection,
    ILoggerFactory loggerFactory) : CompletionItemResolver
{
    private readonly IDocumentContextFactory _documentContextFactory = documentContextFactory;
    private readonly IRazorFormattingService _formattingService = formattingService;
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;
    private readonly RazorLSPOptionsMonitor _optionsMonitor = optionsMonitor;
    private readonly IClientConnection _clientConnection = clientConnection;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<DelegatedCompletionItemResolver>();

    public override async Task<VSInternalCompletionItem?> ResolveAsync(
        VSInternalCompletionItem item,
        VSInternalCompletionList containingCompletionList,
        ICompletionResolveContext originalRequestContext,
        VSInternalClientCapabilities? clientCapabilities,
        IComponentAvailabilityService componentAvailabilityService,
        CancellationToken cancellationToken)
    {
        if (originalRequestContext is not DelegatedCompletionResolutionContext resolutionContext)
        {
            // Can't recognize the original request context, bail.
            return null;
        }

        item.Data = DelegatedCompletionHelper.GetOriginalCompletionItemData(item, containingCompletionList, resolutionContext.OriginalCompletionListData);

        var delegatedResolveParams = new DelegatedCompletionItemResolveParams(
            resolutionContext.Identifier,
            item,
            resolutionContext.ProjectedKind);
        var resolvedCompletionItem = await _clientConnection.SendRequestAsync<DelegatedCompletionItemResolveParams, VSInternalCompletionItem?>(LanguageServerConstants.RazorCompletionResolveEndpointName, delegatedResolveParams, cancellationToken).ConfigureAwait(false);

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
        if (context.ProjectedKind != RazorLanguageKind.CSharp)
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

        var identifier = context.Identifier.TextDocumentIdentifier;
        if (!_documentContextFactory.TryCreate(identifier, out var documentContext))
        {
            return resolvedCompletionItem;
        }

        var formattingOptions = await _clientConnection
            .SendRequestAsync<TextDocumentIdentifierAndVersion, FormattingOptions?>(
                LanguageServerConstants.RazorGetFormattingOptionsEndpointName,
                documentContext.GetTextDocumentIdentifierAndVersion(),
                cancellationToken)
            .ConfigureAwait(false);

        if (formattingOptions is null)
        {
            return resolvedCompletionItem;
        }

        var options = RazorFormattingOptions.From(formattingOptions, _optionsMonitor.CurrentValue.CodeBlockBraceOnNextLine);

        return await DelegatedCompletionHelper.FormatCSharpCompletionItemAsync(resolvedCompletionItem, documentContext, options, _formattingService, _documentMappingService, _logger, cancellationToken).ConfigureAwait(false);
    }
}
