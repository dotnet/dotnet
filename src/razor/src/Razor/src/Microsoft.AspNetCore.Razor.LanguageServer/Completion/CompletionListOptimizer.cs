// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using AliasedVSCommitCharacters = Microsoft.VisualStudio.LanguageServer.Protocol.SumType<string[], Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalCommitCharacter[]>;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal static class CompletionListOptimizer
{
    public static VSInternalCompletionList Optimize(VSInternalCompletionList completionList, VSInternalCompletionSetting? completionCapability)
    {
        if (completionCapability is not null)
        {
            completionList = OptimizeCommitCharacters(completionList, completionCapability);
        }

        // We wrap the pre-existing completion list with an optimized completion list to better control serialization/deserialization
        var optimizedCompletionList = new OptimizedVSCompletionList(completionList);
        return optimizedCompletionList;
    }

    private static VSInternalCompletionList OptimizeCommitCharacters(VSInternalCompletionList completionList, VSInternalCompletionSetting completionCapability)
    {
        var completionListCapability = completionCapability.CompletionList;
        if (completionListCapability?.CommitCharacters != true)
        {
            return completionList;
        }

        // The commit characters capability is a VS capability with how we utilize it, therefore we want to promote onto the VS list.
        completionList = PromoteVSCommonCommitCharactersOntoList(completionList);
        return completionList;
    }

    private static VSInternalCompletionList PromoteVSCommonCommitCharactersOntoList(VSInternalCompletionList completionList)
    {
        (AliasedVSCommitCharacters VsCommitCharacters, List<VSInternalCompletionItem> AssociatedCompletionItems)? mostUsedCommitCharacterToItems = null;
        var commitCharacterMap = new Dictionary<AliasedVSCommitCharacters, List<VSInternalCompletionItem>>(AliasedVSCommitCharactersComparer.Instance);
        foreach (var completionItem in completionList.Items)
        {
            if (completionItem is not VSInternalCompletionItem vsCompletionItem)
            {
                continue;
            }

            var vsCommitCharactersHolder = vsCompletionItem.VsCommitCharacters;
            if (vsCommitCharactersHolder is null)
            {
                continue;
            }

            var commitCharacters = vsCommitCharactersHolder.Value;
            if (!commitCharacterMap.TryGetValue(commitCharacters, out var associatedCompletionItems))
            {
                associatedCompletionItems = new List<VSInternalCompletionItem>();
                commitCharacterMap[commitCharacters] = associatedCompletionItems;
            }

            associatedCompletionItems.Add(vsCompletionItem);

            if (mostUsedCommitCharacterToItems is null ||
                associatedCompletionItems.Count > mostUsedCommitCharacterToItems.Value.AssociatedCompletionItems.Count)
            {
                mostUsedCommitCharacterToItems = (commitCharacters, associatedCompletionItems);
            }
        }

        if (mostUsedCommitCharacterToItems is null)
        {
            return completionList;
        }

        // Promote the most used commit characters onto the list and remove duplicates from child items.
        foreach (var completionItem in mostUsedCommitCharacterToItems.Value.AssociatedCompletionItems)
        {
            // Clear out the commit characters for all associated items
            completionItem.CommitCharacters = null;
            completionItem.VsCommitCharacters = null;
        }

        completionList.CommitCharacters = mostUsedCommitCharacterToItems.Value.VsCommitCharacters;
        return completionList;
    }

    private class AliasedVSCommitCharactersComparer : IEqualityComparer<AliasedVSCommitCharacters>
    {
        public static readonly AliasedVSCommitCharactersComparer Instance = new();

        private AliasedVSCommitCharactersComparer()
        {
        }

        public bool Equals(AliasedVSCommitCharacters a, AliasedVSCommitCharacters b)
        {
            if (a.TryGetFirst(out var aFirstValue) && b.TryGetFirst(out var bFirstValue))
            {
                return Enumerable.SequenceEqual(aFirstValue, bFirstValue);
            }
            else if (a.TryGetSecond(out var aSecondValue) && b.TryGetSecond(out var bSecondValue))
            {
                if (aSecondValue.Length != bSecondValue.Length)
                {
                    return false;
                }

                for (var i = 0; i < aSecondValue.Length; i++)
                {
                    var aCommitCharacter = aSecondValue[i];
                    var bCommitCharacter = bSecondValue[i];

                    if (aCommitCharacter.Character != bCommitCharacter.Character ||
                        aCommitCharacter.Insert != bCommitCharacter.Insert)
                    {
                        return false;
                    }
                }

                return true;
            }

            // Mismatch in commit character types
            return false;
        }

        public int GetHashCode(AliasedVSCommitCharacters obj)
        {
            if (obj.TryGetFirst(out var stringVal))
            {
                return stringVal.Length;
            }
            else if (obj.TryGetSecond(out var commitCharVal))
            {
                return commitCharVal.Length;
            }

            return 0;
        }
    }
}
