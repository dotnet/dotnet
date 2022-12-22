// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor.Tooltip;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal static class RazorCompletionItemExtensions
{
    private readonly static string s_attributeCompletionDescriptionKey = "Razor.AttributeDescription";
    private readonly static string s_directiveCompletionDescriptionKey = "Razor.DirectiveDescription";
    private readonly static string s_markupTransitionDescriptionKey = "Razor.MarkupTransitionDescription";

    public static void SetAttributeCompletionDescription(this RazorCompletionItem completionItem, AggregateBoundAttributeDescription attributeCompletionDescription)
    {
        if (completionItem is null)
        {
            throw new ArgumentNullException(nameof(completionItem));
        }

        completionItem.Items[s_attributeCompletionDescriptionKey] = attributeCompletionDescription;
    }

    public static AggregateBoundAttributeDescription? GetAttributeCompletionDescription(this RazorCompletionItem completionItem)
    {
        if (completionItem is null)
        {
            throw new ArgumentNullException(nameof(completionItem));
        }

        var attributeCompletionDescription = completionItem.Items[s_attributeCompletionDescriptionKey] as AggregateBoundAttributeDescription;
        return attributeCompletionDescription;
    }

    public static void SetDirectiveCompletionDescription(this RazorCompletionItem completionItem, DirectiveCompletionDescription attributeCompletionDescription)
    {
        if (completionItem is null)
        {
            throw new ArgumentNullException(nameof(completionItem));
        }

        completionItem.Items[s_directiveCompletionDescriptionKey] = attributeCompletionDescription;
    }

    public static DirectiveCompletionDescription? GetDirectiveCompletionDescription(this RazorCompletionItem completionItem)
    {
        if (completionItem is null)
        {
            throw new ArgumentNullException(nameof(completionItem));
        }

        var attributeCompletionDescription = completionItem.Items[s_directiveCompletionDescriptionKey] as DirectiveCompletionDescription;
        return attributeCompletionDescription;
    }

    public static void SetMarkupTransitionCompletionDescription(this RazorCompletionItem completionItem, MarkupTransitionCompletionDescription markupTransitionCompletionDescription)
    {
        if (completionItem is null)
        {
            throw new ArgumentNullException(nameof(completionItem));
        }

        completionItem.Items[s_markupTransitionDescriptionKey] = markupTransitionCompletionDescription;
    }

    public static MarkupTransitionCompletionDescription? GetMarkupTransitionCompletionDescription(this RazorCompletionItem completionItem)
    {
        if (completionItem is null)
        {
            throw new ArgumentNullException(nameof(completionItem));
        }

        var markupTransitionCompletionDescription = completionItem.Items[s_markupTransitionDescriptionKey] as MarkupTransitionCompletionDescription;
        return markupTransitionCompletionDescription;
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
}
