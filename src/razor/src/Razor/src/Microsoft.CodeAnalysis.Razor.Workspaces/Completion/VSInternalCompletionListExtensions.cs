// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal static class VSInternalCompletionListExtensions
{
    // This needs to match what's listed in VSInternalCompletionItemExtensions.ResultIdKey
    private const string ResultIdKey = "_resultId";

    public static void SetResultId(
        this VSInternalCompletionList completionList,
        int resultId,
        VSInternalCompletionSetting? completionSetting)
    {
        var data = JsonSerializer.SerializeToElement(new JsonObject()
        {
            [ResultIdKey] = resultId,
        });

        if (completionSetting?.CompletionList?.Data == true)
        {
            // Can set data at the completion list level

            var mergedData = CompletionListMerger.MergeData(data, completionList.Data);
            completionList.Data = mergedData;

            // Merge data for items that won't inherit the default
            foreach (var completionItem in completionList.Items.Where(c => c.Data is not null))
            {
                mergedData = CompletionListMerger.MergeData(data, completionItem.Data);
                completionItem.Data = mergedData;
            }
        }
        else
        {
            // No CompletionList.Data support

            foreach (var completionItem in completionList.Items)
            {
                var mergedData = CompletionListMerger.MergeData(data, completionItem.Data);
                completionItem.Data = mergedData;
            }
        }
    }
}
