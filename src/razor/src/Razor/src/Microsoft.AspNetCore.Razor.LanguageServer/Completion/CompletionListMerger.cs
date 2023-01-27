// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal static class CompletionListMerger
{
    private static readonly string Data1Key = nameof(MergedCompletionListData.Data1);
    private static readonly string Data2Key = nameof(MergedCompletionListData.Data2);
    private static readonly object EmptyData = new object();

    public static VSInternalCompletionList? Merge(VSInternalCompletionList? razorCompletionList, VSInternalCompletionList? delegatedCompletionList)
    {
        if (razorCompletionList is null)
        {
            return delegatedCompletionList;
        }

        if (delegatedCompletionList?.Items is null)
        {
            return razorCompletionList;
        }

        EnsureMergeableCommitCharacters(razorCompletionList, delegatedCompletionList);
        EnsureMergeableData(razorCompletionList, delegatedCompletionList);

        var mergedIsIncomplete = razorCompletionList.IsIncomplete || delegatedCompletionList.IsIncomplete;
        var mergedItems = razorCompletionList.Items.Concat(delegatedCompletionList.Items).ToArray();
        var mergedData = MergeData(razorCompletionList.Data, delegatedCompletionList.Data);
        var mergedCommitCharacters = razorCompletionList.CommitCharacters ?? delegatedCompletionList.CommitCharacters;
        var mergedSuggestionMode = razorCompletionList.SuggestionMode || delegatedCompletionList.SuggestionMode;

        // We don't fully support merging continue characters currently. Razor doesn't currently use them so delegated completion lists always win.
        var mergedContinueWithCharacters = razorCompletionList.ContinueCharacters ?? delegatedCompletionList.ContinueCharacters;

        // We don't fully support merging edit ranges currently. Razor doesn't currently use them so delegated completion lists always win.
        var mergedItemDefaultsEditRange = razorCompletionList.ItemDefaults?.EditRange ?? delegatedCompletionList.ItemDefaults?.EditRange;

        var mergedCompletionList = new VSInternalCompletionList()
        {
            CommitCharacters = mergedCommitCharacters,
            Data = mergedData,
            IsIncomplete = mergedIsIncomplete,
            Items = mergedItems,
            SuggestionMode = mergedSuggestionMode,
            ContinueCharacters = mergedContinueWithCharacters,
            ItemDefaults = new CompletionListItemDefaults()
            {
                EditRange = mergedItemDefaultsEditRange,
            }
        };

        return mergedCompletionList;
    }

    public static object? MergeData(object? data1, object? data2)
    {
        if (data1 is null)
        {
            return data2;
        }

        if (data2 is null)
        {
            return data1;
        }

        return new MergedCompletionListData(data1, data2);
    }

    public static bool TrySplit(object? data, [NotNullWhen(true)] out IReadOnlyList<JObject>? splitData)
    {
        if (data is null)
        {
            splitData = null;
            return false;
        }

        var collector = new List<JObject>();
        Split(data, collector);

        if (collector.Count == 0)
        {
            splitData = null;
            return false;
        }

        splitData = collector;
        return true;
    }

    private static void Split(object data, List<JObject> collector)
    {
        if (data is not JObject jobject)
        {
            return;
        }

        if (!(jobject.ContainsKey(Data1Key) || jobject.ContainsKey(Data1Key.ToLowerInvariant())) ||
            !(jobject.ContainsKey(Data2Key) || jobject.ContainsKey(Data2Key.ToLowerInvariant())))
        {
            // Normal, non-merged data
            collector.Add(jobject);
        }
        else
        {
            // Merged data
            var mergedCompletionListData = jobject.ToObject<MergedCompletionListData>();

            if (mergedCompletionListData is null)
            {
                Debug.Fail("Merged completion list data is null, this should never happen.");
                return;
            }

            Split(mergedCompletionListData.Data1, collector);
            Split(mergedCompletionListData.Data2, collector);
        }
    }

    private static void EnsureMergeableData(VSInternalCompletionList completionListA, VSInternalCompletionList completionListB)
    {
        if (completionListA.Data != completionListB.Data &&
            completionListA.Data is null || completionListB.Data is null)
        {
            // One of the completion lists have data while the other does not, we need to ensure that any non-data centric items don't get incorrect data associated

            // The candidate completion list will be one where we populate empty data for any `null` specifying data given we'll be merging
            // two completion lists together we don't want incorrect data to be inherited down
            var candidateCompletionList = completionListA.Data is null ? completionListA : completionListB;
            for (var i = 0; i < candidateCompletionList.Items.Length; i++)
            {
                var item = candidateCompletionList.Items[i];
                if (item.Data is null)
                {
                    item.Data = EmptyData;
                }
            }
        }
    }

    private static void EnsureMergeableCommitCharacters(VSInternalCompletionList completionListA, VSInternalCompletionList completionListB)
    {
        var aInheritsCommitCharacters = completionListA.CommitCharacters is not null || completionListA.ItemDefaults?.CommitCharacters is not null;
        var bInheritsCommitCharacters = completionListB.CommitCharacters is not null || completionListB.ItemDefaults?.CommitCharacters is not null;
        if (aInheritsCommitCharacters && bInheritsCommitCharacters)
        {
            // Need to merge commit characters because both are trying to inherit

            var inheritableCommitCharacterCompletionsA = GetCompletionsThatDoNotSpecifyCommitCharacters(completionListA);
            var inheritableCommitCharacterCompletionsB = GetCompletionsThatDoNotSpecifyCommitCharacters(completionListB);
            IReadOnlyList<VSInternalCompletionItem>? completionItemsToStopInheriting;
            VSInternalCompletionList? completionListToStopInheriting;

            // Decide which completion list has more items that benefit from "inheriting" commit characters.
            if (inheritableCommitCharacterCompletionsA.Count >= inheritableCommitCharacterCompletionsB.Count)
            {
                completionListToStopInheriting = completionListB;
                completionItemsToStopInheriting = inheritableCommitCharacterCompletionsB;
            }
            else
            {
                completionListToStopInheriting = completionListA;
                completionItemsToStopInheriting = inheritableCommitCharacterCompletionsA;
            }

            for (var i = 0; i < completionItemsToStopInheriting.Count; i++)
            {
                if (completionListToStopInheriting.CommitCharacters is not null)
                {
                    completionItemsToStopInheriting[i].VsCommitCharacters = completionListToStopInheriting.CommitCharacters;
                }
                else if (completionListToStopInheriting.ItemDefaults?.CommitCharacters is not null)
                {
                    completionItemsToStopInheriting[i].VsCommitCharacters = completionListToStopInheriting.ItemDefaults?.CommitCharacters;
                }
            }

            completionListToStopInheriting.CommitCharacters = null;

            if (completionListToStopInheriting.ItemDefaults is not null)
            {
                completionListToStopInheriting.ItemDefaults.CommitCharacters = null;
            }
        }
    }

    private static IReadOnlyList<VSInternalCompletionItem> GetCompletionsThatDoNotSpecifyCommitCharacters(VSInternalCompletionList completionList)
    {
        var inheritableCompletions = new List<VSInternalCompletionItem>();

        for (var i = 0; i < completionList.Items.Length; i++)
        {
            var completionItem = completionList.Items[i] as VSInternalCompletionItem;
            if (completionItem is null ||
                completionItem.CommitCharacters is not null ||
                completionItem.VsCommitCharacters is not null)
            {
                // Completion item wasn't the right type or already specifies commit characters
                continue;
            }

            inheritableCompletions.Add(completionItem);
        }

        return inheritableCompletions;
    }

    private record MergedCompletionListData(object Data1, object Data2);
}
