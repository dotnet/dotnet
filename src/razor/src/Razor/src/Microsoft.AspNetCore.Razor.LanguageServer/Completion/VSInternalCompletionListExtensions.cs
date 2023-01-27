// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal static class VSInternalCompletionListExtensions
{
    // This needs to match what's listed in VSInternalCompletionItemExtensions.ResultIdKey
    private const string ResultIdKey = "_resultId";

    public static void SetResultId(
        this VSInternalCompletionList completionList,
        int resultId,
        VSInternalCompletionSetting? completionSetting)
    {
        if (completionList is null)
        {
            throw new ArgumentNullException(nameof(completionList));
        }

        var data = new JObject()
        {
            [ResultIdKey] = resultId,
        };

        if (completionSetting?.CompletionList?.Data == true)
        {
            // Can set data at the completion list level

            var mergedData = CompletionListMerger.MergeData(data, completionList.Data);
            completionList.Data = mergedData;
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
