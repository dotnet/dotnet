// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal record RazorCompletionResolveData(
    // NOTE: Uppercase T here is required to match Roslyn's DocumentResolveData structure, so that the Roslyn
    //       language server can correctly route requests to us in cohosting. In future when we normalize
    //       on to Roslyn types, we should inherit from that class so we don't have to remember to do this.
    [property: JsonPropertyName("TextDocument")] TextDocumentIdentifier TextDocument,
    [property: JsonPropertyName("data")] object? OriginalData)
{
    public static RazorCompletionResolveData Unwrap(CompletionItem completionItem)
    {
        if (completionItem.Data is not JsonElement paramsObj)
        {
            throw new InvalidOperationException($"Invalid completion item received'{completionItem.Label}'.");
        }

        if (paramsObj.Deserialize<RazorCompletionResolveData>() is not { } context)
        {
            throw new InvalidOperationException($"completionItem.Data should be convertible to {nameof(RazorCompletionResolveData)}");
        }

        return context;
    }

    public static void Wrap(VSInternalCompletionList completionList, TextDocumentIdentifier textDocument, bool supportsCompletionListData)
    {
        var data = new RazorCompletionResolveData(textDocument, OriginalData: null);

        if (supportsCompletionListData)
        {
            if (completionList.Data is not null)
            {
                // Can set data at the completion list level
                completionList.Data = data with { OriginalData = completionList.Data };
            }

            if (completionList.ItemDefaults?.Data is not null)
            {
                // Set data for the item defaults
                completionList.ItemDefaults.Data = data with { OriginalData = completionList.ItemDefaults.Data };
            }

            // Set data for items that won't inherit the default
            foreach (var completionItem in completionList.Items.Where(static c => c.Data is not null))
            {
                completionItem.Data = data with { OriginalData = completionItem.Data };
            }
        }
        else
        {
            // No CompletionList.Data support, so set data for all items
            foreach (var completionItem in completionList.Items)
            {
                completionItem.Data = data with { OriginalData = completionItem.Data };
            }
        }
    }
}
