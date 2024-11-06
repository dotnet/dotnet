// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor.Tooltip;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal static class RazorCompletionItemExtensions
{
    private readonly static string s_attributeCompletionDescriptionKey = "Razor.AttributeDescription";
    private readonly static string s_directiveCompletionDescriptionKey = "Razor.DirectiveDescription";
    private readonly static string s_markupTransitionDescriptionKey = "Razor.MarkupTransitionDescription";
    private readonly static string s_tagHelperElementCompletionDescriptionKey = "Razor.TagHelperElementDescription";

    public static void SetAttributeCompletionDescription(this RazorCompletionItem completionItem, AggregateBoundAttributeDescription attributeCompletionDescription)
    {
        completionItem.Items[s_attributeCompletionDescriptionKey] = attributeCompletionDescription;
    }

    public static AggregateBoundAttributeDescription? GetAttributeCompletionDescription(this RazorCompletionItem completionItem)
    {
        var attributeCompletionDescription = completionItem.Items[s_attributeCompletionDescriptionKey] as AggregateBoundAttributeDescription;
        return attributeCompletionDescription;
    }

    public static void SetDirectiveCompletionDescription(this RazorCompletionItem completionItem, DirectiveCompletionDescription attributeCompletionDescription)
    {
        completionItem.Items[s_directiveCompletionDescriptionKey] = attributeCompletionDescription;
    }

    public static DirectiveCompletionDescription? GetDirectiveCompletionDescription(this RazorCompletionItem completionItem)
    {
        var attributeCompletionDescription = completionItem.Items[s_directiveCompletionDescriptionKey] as DirectiveCompletionDescription;
        return attributeCompletionDescription;
    }

    public static IEnumerable<string> GetAttributeCompletionTypes(this RazorCompletionItem completionItem)
    {
        var attributeCompletionDescription = completionItem.GetAttributeCompletionDescription();

        if (attributeCompletionDescription is null)
        {
            yield break;
        }

        foreach (var descriptionInfo in attributeCompletionDescription.DescriptionInfos)
        {
            yield return descriptionInfo.ReturnTypeName;
        }
    }

    public static void SetMarkupTransitionCompletionDescription(this RazorCompletionItem completionItem, MarkupTransitionCompletionDescription markupTransitionCompletionDescription)
    {
        completionItem.Items[s_markupTransitionDescriptionKey] = markupTransitionCompletionDescription;
    }

    public static MarkupTransitionCompletionDescription? GetMarkupTransitionCompletionDescription(this RazorCompletionItem completionItem)
    {
        var markupTransitionCompletionDescription = completionItem.Items[s_markupTransitionDescriptionKey] as MarkupTransitionCompletionDescription;
        return markupTransitionCompletionDescription;
    }

    public static void SetTagHelperElementDescriptionInfo(this RazorCompletionItem completionItem, AggregateBoundElementDescription elementDescriptionInfo)
    {
        completionItem.Items[s_tagHelperElementCompletionDescriptionKey] = elementDescriptionInfo;
    }

    public static AggregateBoundElementDescription? GetTagHelperElementDescriptionInfo(this RazorCompletionItem completionItem)
    {
        var description = completionItem.Items[s_tagHelperElementCompletionDescriptionKey] as AggregateBoundElementDescription;
        return description;
    }
}
