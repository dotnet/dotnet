// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal class AggregateCompletionItemResolver
{
    private readonly IReadOnlyList<CompletionItemResolver> _completionItemResolvers;
    private readonly ILogger _logger;

    public AggregateCompletionItemResolver(IEnumerable<CompletionItemResolver> completionItemResolvers, ILoggerFactory loggerFactory)
    {
        _completionItemResolvers = completionItemResolvers.ToArray();
        _logger = loggerFactory.GetOrCreateLogger<AggregateCompletionItemResolver>();
    }

    public async Task<VSInternalCompletionItem?> ResolveAsync(
        VSInternalCompletionItem item,
        VSInternalCompletionList containingCompletionList,
        ICompletionResolveContext originalRequestContext,
        VSInternalClientCapabilities? clientCapabilities,
        IComponentAvailabilityService componentAvailabilityService,
        CancellationToken cancellationToken)
    {
        using var completionItemResolverTasks = new PooledArrayBuilder<Task<VSInternalCompletionItem?>>(_completionItemResolvers.Count);

        foreach (var completionItemResolver in _completionItemResolvers)
        {
            try
            {
                var task = completionItemResolver.ResolveAsync(item, containingCompletionList, originalRequestContext, clientCapabilities, componentAvailabilityService, cancellationToken);
                completionItemResolverTasks.Add(task);
            }
            catch (Exception ex) when (ex is not TaskCanceledException)
            {
                _logger.LogError(ex, $"Resolving completion item failed synchronously unexpectedly.");
            }
        }

        // We don't currently handle merging completion items because it's very rare for more than one resolution to take place.
        // Instead we'll prioritized the last completion item resolved.
        VSInternalCompletionItem? lastResolved = null;
        foreach (var completionItemResolverTask in completionItemResolverTasks)
        {
            try
            {
                var resolvedCompletionItem = await completionItemResolverTask.ConfigureAwait(false);
                if (resolvedCompletionItem is not null)
                {
                    lastResolved = resolvedCompletionItem;
                }

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, $"Resolving completion item failed unexpectedly.");
            }
        }

        return lastResolved;
    }
}
