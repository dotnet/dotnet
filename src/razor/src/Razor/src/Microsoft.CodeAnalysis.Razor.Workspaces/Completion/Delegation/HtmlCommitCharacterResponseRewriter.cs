// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Completion.Delegation;

internal class HtmlCommitCharacterResponseRewriter
{
    public VSInternalCompletionList Rewrite(
        VSInternalCompletionList completionList,
        RazorCompletionOptions completionOptions)
    {
        if (completionOptions.CommitElementsWithSpace)
        {
            return completionList;
        }

        string[]? itemCommitChars = null;
        if (completionList.CommitCharacters is { } commitCharacters)
        {
            if (commitCharacters.TryGetFirst(out var commitChars))
            {
                itemCommitChars = commitChars.Where(s => s != " ").ToArray();

                // If the default commit characters didn't include " " already, then we set our list to null to avoid over-specifying commit characters
                if (itemCommitChars.Length == commitChars.Length)
                {
                    itemCommitChars = null;
                }
            }
            else if (commitCharacters.TryGetSecond(out var vsCommitChars))
            {
                itemCommitChars = vsCommitChars.Where(s => s.Character != " ").Select(s => s.Character).ToArray();

                // If the default commit characters didn't include " " already, then we set our list to null to avoid over-specifying commit characters
                if (itemCommitChars.Length == vsCommitChars.Length)
                {
                    itemCommitChars = null;
                }
            }
        }

        foreach (var item in completionList.Items)
        {
            if (item.Kind == CompletionItemKind.Element)
            {
                if (item.CommitCharacters is null)
                {
                    if (itemCommitChars is not null)
                    {
                        // This item wants to use the default commit characters, so change it to our updated version of them, without the space
                        item.CommitCharacters = itemCommitChars;
                    }
                }
                else
                {
                    // This item has its own commit characters, so just remove spaces
                    item.CommitCharacters = item.CommitCharacters?.Where(s => s != " ").ToArray();
                }
            }
        }

        return completionList;
    }
}
