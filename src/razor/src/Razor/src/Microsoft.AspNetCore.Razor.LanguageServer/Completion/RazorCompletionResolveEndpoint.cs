// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal class RazorCompletionResolveEndpoint : IVSCompletionResolveEndpoint, ICapabilitiesProvider
{
    private readonly AggregateCompletionItemResolver _completionItemResolver;
    private readonly CompletionListCache _completionListCache;
    private VSInternalClientCapabilities? _clientCapabilities;

    public RazorCompletionResolveEndpoint(
        AggregateCompletionItemResolver completionItemResolver,
        CompletionListCache completionListCache)
    {
        _completionItemResolver = completionItemResolver;
        _completionListCache = completionListCache;
    }

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        _clientCapabilities = clientCapabilities;
    }

    public async Task<VSInternalCompletionItem> HandleRequestAsync(VSInternalCompletionItem completionItem, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (!completionItem.TryGetCompletionListResultIds(out var resultIds))
        {
            // Unable to lookup completion item result info
            return completionItem;
        }

        object? originalRequestContext = null;
        VSInternalCompletionList? containingCompletionList = null;
        foreach (var resultId in resultIds)
        {
            if (!_completionListCache.TryGet(resultId, out var cacheEntry))
            {
                continue;
            }

            // See if this is the right completion list for this corresponding completion item. We cross-check this based on label only given that
            // is what users interact with.
            if (cacheEntry.CompletionList.Items.Any(completion => string.Equals(completionItem.Label, completion.Label, StringComparison.Ordinal)))
            {
                originalRequestContext = cacheEntry.Context;
                containingCompletionList = cacheEntry.CompletionList;
                break;
            }
        }

        if (containingCompletionList is null)
        {
            // Couldn't find an associated completion list
            return completionItem;
        }

        var resolvedCompletionItem = await _completionItemResolver.ResolveAsync(
            completionItem,
            containingCompletionList,
            originalRequestContext,
            _clientCapabilities,
            cancellationToken).ConfigureAwait(false);
        resolvedCompletionItem ??= completionItem;

        return resolvedCompletionItem;
    }
}
