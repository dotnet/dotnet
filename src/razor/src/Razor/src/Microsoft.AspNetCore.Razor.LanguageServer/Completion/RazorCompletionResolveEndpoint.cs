// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Tooltip;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

[RazorLanguageServerEndpoint(Methods.TextDocumentCompletionResolveName)]
internal class RazorCompletionResolveEndpoint(
    AggregateCompletionItemResolver completionItemResolver,
    CompletionListCache completionListCache,
    IComponentAvailabilityService componentAvailabilityService)
    : IRazorRequestHandler<VSInternalCompletionItem, VSInternalCompletionItem>, ICapabilitiesProvider
{
    private readonly AggregateCompletionItemResolver _completionItemResolver = completionItemResolver;
    private readonly CompletionListCache _completionListCache = completionListCache;
    private readonly IComponentAvailabilityService _componentAvailabilityService = componentAvailabilityService;

    private VSInternalClientCapabilities? _clientCapabilities;

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        _clientCapabilities = clientCapabilities;
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(VSInternalCompletionItem request)
    {
        var context = RazorCompletionResolveData.Unwrap(request);
        return context.TextDocument;
    }

    public async Task<VSInternalCompletionItem> HandleRequestAsync(VSInternalCompletionItem completionItem, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var data = RazorCompletionResolveData.Unwrap(completionItem);
        completionItem.Data = data.OriginalData;

        if (!_completionListCache.TryGetOriginalRequestData(completionItem, out var containingCompletionList, out var originalRequestContext))
        {
            return completionItem;
        }

        var resolvedCompletionItem = await _completionItemResolver
            .ResolveAsync(
                completionItem,
                containingCompletionList,
                originalRequestContext,
                _clientCapabilities,
                _componentAvailabilityService,
                cancellationToken)
            .ConfigureAwait(false);

        resolvedCompletionItem ??= completionItem;

        return resolvedCompletionItem;
    }
}
