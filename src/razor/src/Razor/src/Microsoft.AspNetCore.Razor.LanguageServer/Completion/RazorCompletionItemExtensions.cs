// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Tooltip;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal static class RazorCompletionItemExtensions
{
    private readonly static string s_tagHelperElementCompletionDescriptionKey = "Razor.TagHelperElementDescription";

    public static void SetTagHelperElementDescriptionInfo(this RazorCompletionItem completionItem, AggregateBoundElementDescription elementDescriptionInfo)
    {
        completionItem.Items[s_tagHelperElementCompletionDescriptionKey] = elementDescriptionInfo;
    }

    public static AggregateBoundElementDescription? GetTagHelperElementDescriptionInfo(this RazorCompletionItem completionItem)
    {
        if (completionItem is null)
        {
            throw new ArgumentNullException(nameof(completionItem));
        }

        var description = completionItem.Items[s_tagHelperElementCompletionDescriptionKey] as AggregateBoundElementDescription;
        return description;
    }
}
