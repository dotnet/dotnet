// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

[RazorLanguageServerEndpoint(Methods.TextDocumentCompletionResolveName)]
internal class RazorCompletionResolveEndpoint(
    AggregateCompletionItemResolver completionItemResolver,
    CompletionListCache completionListCache,
    IComponentAvailabilityService componentAvailabilityService)
    : IRazorDocumentlessRequestHandler<VSInternalCompletionItem, VSInternalCompletionItem>, ICapabilitiesProvider
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

    public async Task<VSInternalCompletionItem> HandleRequestAsync(VSInternalCompletionItem completionItem, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (!TryGetOriginalRequestData(completionItem, out var containingCompletionList, out var originalRequestContext))
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

    private bool TryGetOriginalRequestData(VSInternalCompletionItem completionItem, [NotNullWhen(true)] out VSInternalCompletionList? completionList, [NotNullWhen(true)] out ICompletionResolveContext? context)
    {
        context = null;
        completionList = null;

        if (!completionItem.TryGetCompletionListResultIds(out var resultIds))
        {
            // Unable to lookup completion item result info
            return false;
        }

        foreach (var resultId in resultIds)
        {
            // See if this is the right completion list for this corresponding completion item. We cross-check this based on label only given that
            // is what users interact with.
            if (_completionListCache.TryGet(resultId, out completionList, out context) &&
                completionList.Items.Any(
                    completion =>
                        completionItem.Label == completion.Label &&
                        // Check the Kind as well, e.g. we may have a Razor snippet and a C# keyword with the same label, etc.
                        completionItem.Kind == completion.Kind))
            {
                return true;
            }
        }

        return false;
    }
}
