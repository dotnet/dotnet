// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal class CompletionListProvider
{
    private readonly RazorCompletionListProvider _razorCompletionListProvider;
    private readonly DelegatedCompletionListProvider _delegatedCompletionListProvider;

    public CompletionListProvider(RazorCompletionListProvider razorCompletionListProvider, DelegatedCompletionListProvider delegatedCompletionListProvider)
    {
        _razorCompletionListProvider = razorCompletionListProvider;
        _delegatedCompletionListProvider = delegatedCompletionListProvider;

        var allTriggerCharacters = razorCompletionListProvider.TriggerCharacters.Concat(delegatedCompletionListProvider.TriggerCharacters);
        var distinctTriggerCharacters = new HashSet<string>(allTriggerCharacters);
        AggregateTriggerCharacters = distinctTriggerCharacters.ToImmutableHashSet();
    }

    public ImmutableHashSet<string> AggregateTriggerCharacters { get; }

    public async Task<VSInternalCompletionList?> GetCompletionListAsync(
        int absoluteIndex,
        VSInternalCompletionContext completionContext,
        VersionedDocumentContext documentContext,
        VSInternalClientCapabilities clientCapabilities,
        CancellationToken cancellationToken)
    {
        // First we delegate to get completion items from the individual language server
        var delegatedCompletionList = IsValidTrigger(_delegatedCompletionListProvider.TriggerCharacters, completionContext)
            ? await _delegatedCompletionListProvider.GetCompletionListAsync(absoluteIndex, completionContext, documentContext, clientCapabilities, cancellationToken).ConfigureAwait(false)
            : null;

        // Extract the items we got back from the delegated server, to inform tag helper completion
        var existingItems = delegatedCompletionList?.Items != null
            ? new HashSet<string>(delegatedCompletionList.Items.Select(i => i.Label))
            : null;

        // Now we get the Razor completion list, using information from the actual language server if necessary
        var razorCompletionList = IsValidTrigger(_razorCompletionListProvider.TriggerCharacters, completionContext)
            ? await _razorCompletionListProvider.GetCompletionListAsync(absoluteIndex, completionContext, documentContext, clientCapabilities, existingItems, cancellationToken).ConfigureAwait(false)
            : null;

        var finalCompletionList = CompletionListMerger.Merge(razorCompletionList, delegatedCompletionList);

        return finalCompletionList;
    }

    private bool IsValidTrigger(ImmutableHashSet<string> triggerCharacters, VSInternalCompletionContext completionContext)
        => completionContext.TriggerKind != CompletionTriggerKind.TriggerCharacter ||
           completionContext.TriggerCharacter is null ||
           triggerCharacters.Contains(completionContext.TriggerCharacter);
}
