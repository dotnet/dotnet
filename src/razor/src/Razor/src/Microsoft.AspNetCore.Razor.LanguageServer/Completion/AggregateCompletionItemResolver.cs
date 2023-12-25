// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal class AggregateCompletionItemResolver
{
    private readonly IReadOnlyList<CompletionItemResolver> _completionItemResolvers;
    private readonly ILogger _logger;

    public AggregateCompletionItemResolver(IEnumerable<CompletionItemResolver> completionItemResolvers, IRazorLoggerFactory loggerFactory)
    {
        _completionItemResolvers = completionItemResolvers.ToArray();
        _logger = loggerFactory.CreateLogger<AggregateCompletionItemResolver>();
    }

    public async Task<VSInternalCompletionItem?> ResolveAsync(
        VSInternalCompletionItem item,
        VSInternalCompletionList containingCompletionList,
        object? originalRequestContext,
        VSInternalClientCapabilities? clientCapabilities,
        CancellationToken cancellationToken)
    {
        var completionItemResolverTasks = new List<Task<VSInternalCompletionItem?>>(_completionItemResolvers.Count);

        foreach (var completionItemResolver in _completionItemResolvers)
        {
            try
            {
                var task = completionItemResolver.ResolveAsync(item, containingCompletionList, originalRequestContext, clientCapabilities, cancellationToken);
                completionItemResolverTasks.Add(task);
            }
            catch (Exception ex) when (ex is not TaskCanceledException)
            {
                _logger.LogError(ex, "Resolving completion item failed synchronously unexpectedly.");
            }
        }

        var resolvedCompletionItems = new Queue<VSInternalCompletionItem>();
        foreach (var completionItemResolverTask in completionItemResolverTasks)
        {
            try
            {
                var resolvedCompletionItem = await completionItemResolverTask.ConfigureAwait(false);
                if (resolvedCompletionItem is not null)
                {
                    resolvedCompletionItems.Enqueue(resolvedCompletionItem);
                }

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Resolving completion item failed unexpectedly.");
            }
        }

        if (resolvedCompletionItems.Count == 0)
        {
            return null;
        }

        // We don't currently handle merging completion items because it's very rare for more than one resolution to take place.
        // Instead we'll prioritized the last completion item resolved.
        var finalCompletionItem = resolvedCompletionItems.Last();
        return finalCompletionItem;
    }
}
