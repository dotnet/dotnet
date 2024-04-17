// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal static class VSInternalCompletionItemExtensions
{
    private const string ResultIdKey = "_resultId";

    private static readonly Dictionary<RazorCommitCharacter, VSInternalCommitCharacter> s_commitCharacterCache = [];

    public static bool TryGetCompletionListResultIds(this VSInternalCompletionItem completion, [NotNullWhen(true)] out IReadOnlyList<int>? resultIds)
    {
        if (completion is null)
        {
            throw new ArgumentNullException(nameof(completion));
        }

        if (!CompletionListMerger.TrySplit(completion.Data, out var splitData))
        {
            resultIds = null;
            return false;
        }

        var ids = new List<int>();
        for (var i = 0; i < splitData.Count; i++)
        {
            var data = splitData[i];
            if (data.ContainsKey(ResultIdKey))
            {
                var resultId = data[ResultIdKey]?.ToObject<int>();
                if (resultId is not null)
                {
                    ids.Add(resultId.Value);
                }
            }
        }

        if (ids.Count > 0)
        {
            resultIds = ids;
            return true;
        }

        resultIds = null;
        return false;
    }

    public static void UseCommitCharactersFrom(
        this VSInternalCompletionItem completionItem,
        RazorCompletionItem razorCompletionItem,
        VSInternalClientCapabilities clientCapabilities)
    {
        var razorCommitCharacters = razorCompletionItem.CommitCharacters;
        if (razorCommitCharacters.IsEmpty)
        {
            return;
        }

        var supportsVSExtensions = clientCapabilities?.SupportsVisualStudioExtensions ?? false;
        if (supportsVSExtensions)
        {
            using var builder = new PooledArrayBuilder<VSInternalCommitCharacter>(capacity: razorCommitCharacters.Length);

            lock (s_commitCharacterCache)
            {
                foreach (var c in razorCommitCharacters)
                {
                    if (!s_commitCharacterCache.TryGetValue(c, out var commitCharacter))
                    {
                        commitCharacter = new() { Character = c.Character, Insert = c.Insert };
                        s_commitCharacterCache.Add(c, commitCharacter);
                    }

                    builder.Add(commitCharacter);
                }
            }

            completionItem.VsCommitCharacters = builder.ToArray();
        }
        else
        {
            using var builder = new PooledArrayBuilder<string>(capacity: razorCommitCharacters.Length);

            foreach (var c in razorCommitCharacters)
            {
                builder.Add(c.Character);
            }

            completionItem.CommitCharacters = builder.ToArray();
        }
    }
}
