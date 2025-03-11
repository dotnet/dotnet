// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal class CompletionListProvider(
    RazorCompletionListProvider razorCompletionListProvider,
    DelegatedCompletionListProvider delegatedCompletionListProvider,
    CompletionTriggerAndCommitCharacters triggerAndCommitCharacters)
{
    private readonly RazorCompletionListProvider _razorCompletionListProvider = razorCompletionListProvider;
    private readonly DelegatedCompletionListProvider _delegatedCompletionListProvider = delegatedCompletionListProvider;
    private readonly CompletionTriggerAndCommitCharacters _triggerAndCommitCharacters = triggerAndCommitCharacters;

    public async Task<VSInternalCompletionList?> GetCompletionListAsync(
        int absoluteIndex,
        VSInternalCompletionContext completionContext,
        DocumentContext documentContext,
        VSInternalClientCapabilities clientCapabilities,
        RazorCompletionOptions razorCompletionOptions,
        Guid correlationId,
        CancellationToken cancellationToken)
    {
        // First we delegate to get completion items from the individual language server
        var delegatedCompletionList = _triggerAndCommitCharacters.IsValidDelegationTrigger(completionContext)
            ? await _delegatedCompletionListProvider.GetCompletionListAsync(
                absoluteIndex,
                completionContext,
                documentContext,
                clientCapabilities,
                razorCompletionOptions,
                correlationId,
                cancellationToken).ConfigureAwait(false)
            : null;

        // Extract the items we got back from the delegated server, to inform tag helper completion
        var existingItems = delegatedCompletionList?.Items != null
            ? new HashSet<string>(delegatedCompletionList.Items.Select(i => i.Label))
            : null;

        // Now we get the Razor completion list, using information from the actual language server if necessary
        var razorCompletionList = _triggerAndCommitCharacters.IsValidRazorTrigger(completionContext)
            ? await _razorCompletionListProvider.GetCompletionListAsync(
                absoluteIndex,
                completionContext,
                documentContext,
                clientCapabilities,
                existingItems,
                razorCompletionOptions,
                cancellationToken).ConfigureAwait(false)
            : null;

        var finalCompletionList = CompletionListMerger.Merge(razorCompletionList, delegatedCompletionList);

        return finalCompletionList;
    }
}
