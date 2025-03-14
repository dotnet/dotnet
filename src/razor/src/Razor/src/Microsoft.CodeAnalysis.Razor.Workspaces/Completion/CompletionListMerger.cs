// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal static class CompletionListMerger
{
    private static readonly string s_data1Key = nameof(MergedCompletionListData.Data1);
    private static readonly string s_data2Key = nameof(MergedCompletionListData.Data2);
    private static readonly object s_emptyData = new();

    [return: NotNullIfNotNull(nameof(razorCompletionList))]
    [return: NotNullIfNotNull(nameof(delegatedCompletionList))]
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

    public static bool TrySplit(object? data, out ImmutableArray<JsonElement> splitData)
    {
        if (data is null)
        {
            splitData = default;
            return false;
        }

        using var collector = new PooledArrayBuilder<JsonElement>();
        Split(data, ref collector.AsRef());

        if (collector.Count == 0)
        {
            splitData = default;
            return false;
        }

        splitData = collector.ToImmutable();
        return true;
    }

    private static void Split(object data, ref PooledArrayBuilder<JsonElement> collector)
    {
        if (data is MergedCompletionListData mergedData)
        {
            // Merged data adds an extra object wrapper around the original data, so remove
            // that to restore to the original form.
            Split(mergedData.Data1, ref collector);
            Split(mergedData.Data2, ref collector);
            return;
        }

        TrySplitJsonElement(data, ref collector);
    }

    private static void TrySplitJsonElement(object data, ref PooledArrayBuilder<JsonElement> collector)
    {
        if (data is not JsonElement jsonElement)
        {
            return;
        }

        if ((jsonElement.TryGetProperty(s_data1Key, out _) || jsonElement.TryGetProperty(s_data1Key.ToLowerInvariant(), out _)) &&
            (jsonElement.TryGetProperty(s_data2Key, out _) || jsonElement.TryGetProperty(s_data2Key.ToLowerInvariant(), out _)))
        {
            // Merged data
            var mergedCompletionListData = jsonElement.Deserialize<MergedCompletionListData>();

            if (mergedCompletionListData is null)
            {
                Debug.Fail("Merged completion list data is null, this should never happen.");
                return;
            }

            Split(mergedCompletionListData.Data1, ref collector);
            Split(mergedCompletionListData.Data2, ref collector);
        }
        else
        {
            collector.Add(jsonElement);
        }
    }

    private static void EnsureMergeableData(VSInternalCompletionList completionListA, VSInternalCompletionList completionListB)
    {
        if (completionListA.Data != completionListB.Data &&
            (completionListA.Data is null || completionListB.Data is null))
        {
            // One of the completion lists have data while the other does not, we need to ensure that any non-data centric items don't get incorrect data associated

            // The candidate completion list will be one where we populate empty data for any `null` specifying data given we'll be merging
            // two completion lists together we don't want incorrect data to be inherited down
            var candidateCompletionList = completionListA.Data is null ? completionListA : completionListB;
            for (var i = 0; i < candidateCompletionList.Items.Length; i++)
            {
                var item = candidateCompletionList.Items[i];
                item.Data ??= s_emptyData;
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
            if (inheritableCommitCharacterCompletionsA.Length >= inheritableCommitCharacterCompletionsB.Length)
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

    private static ImmutableArray<VSInternalCompletionItem> GetCompletionsThatDoNotSpecifyCommitCharacters(VSInternalCompletionList completionList)
    {
        using var inheritableCompletions = new PooledArrayBuilder<VSInternalCompletionItem>();
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

        return inheritableCompletions.ToImmutable();
    }

    private record MergedCompletionListData(object Data1, object Data2);
}
