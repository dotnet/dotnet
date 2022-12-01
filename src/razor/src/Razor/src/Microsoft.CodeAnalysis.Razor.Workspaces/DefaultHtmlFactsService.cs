// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.AspNetCore.Razor.Language.Syntax;

namespace Microsoft.VisualStudio.Editor.Razor;

internal class DefaultHtmlFactsService : HtmlFactsService
{
    public override bool TryGetElementInfo(SyntaxNode element, out SyntaxToken containingTagNameToken, out SyntaxList<RazorSyntaxNode> attributeNodes)
    {
        switch (element)
        {
            case MarkupStartTagSyntax startTag:
                containingTagNameToken = startTag.Name;
                attributeNodes = startTag.Attributes;
                return true;
            case MarkupEndTagSyntax { Parent: MarkupElementSyntax parent } endTag:
                containingTagNameToken = endTag.Name;
                attributeNodes = parent.StartTag?.Attributes ?? new SyntaxList<RazorSyntaxNode>();
                return true;
            case MarkupTagHelperStartTagSyntax startTagHelper:
                containingTagNameToken = startTagHelper.Name;
                attributeNodes = startTagHelper.Attributes;
                return true;
            case MarkupTagHelperEndTagSyntax { Parent: MarkupTagHelperElementSyntax parent } endTagHelper:
                containingTagNameToken = endTagHelper.Name;
                attributeNodes = parent.StartTag?.Attributes ?? new SyntaxList<RazorSyntaxNode>();
                return true;
            default:
                containingTagNameToken = null;
                attributeNodes = default;
                return false;
        }
    }

    public override bool TryGetAttributeInfo(
        SyntaxNode attribute,
        out SyntaxToken containingTagNameToken,
        out TextSpan? prefixLocation,
        out string selectedAttributeName,
        out TextSpan? selectedAttributeNameLocation,
        out SyntaxList<RazorSyntaxNode> attributeNodes)
    {
        if (!TryGetElementInfo(attribute.Parent, out containingTagNameToken, out attributeNodes))
        {
            containingTagNameToken = null;
            prefixLocation = null;
            selectedAttributeName = null;
            selectedAttributeNameLocation = null;
            attributeNodes = default;
            return false;
        }

        switch (attribute)
        {
            case MarkupMinimizedAttributeBlockSyntax minimizedAttributeBlock:
                prefixLocation = minimizedAttributeBlock.NamePrefix?.Span;
                selectedAttributeName = minimizedAttributeBlock.Name.GetContent();
                selectedAttributeNameLocation = minimizedAttributeBlock.Name.Span;
                return true;
            case MarkupAttributeBlockSyntax attributeBlock:
                prefixLocation = attributeBlock.NamePrefix?.Span;
                selectedAttributeName = attributeBlock.Name.GetContent();
                selectedAttributeNameLocation = attributeBlock.Name.Span;
                return true;
            case MarkupTagHelperAttributeSyntax tagHelperAttribute:
                prefixLocation = tagHelperAttribute.NamePrefix?.Span;
                selectedAttributeName = tagHelperAttribute.Name.GetContent();
                selectedAttributeNameLocation = tagHelperAttribute.Name.Span;
                return true;
            case MarkupMinimizedTagHelperAttributeSyntax minimizedAttribute:
                prefixLocation = minimizedAttribute.NamePrefix?.Span;
                selectedAttributeName = minimizedAttribute.Name.GetContent();
                selectedAttributeNameLocation = minimizedAttribute.Name.Span;
                return true;
            case MarkupTagHelperDirectiveAttributeSyntax tagHelperDirectiveAttribute:
                {
                    prefixLocation = tagHelperDirectiveAttribute.NamePrefix?.Span;
                    selectedAttributeName = tagHelperDirectiveAttribute.FullName;
                    var fullNameSpan = TextSpan.FromBounds(tagHelperDirectiveAttribute.Transition.Span.Start, tagHelperDirectiveAttribute.Name.Span.End);
                    selectedAttributeNameLocation = fullNameSpan;
                    return true;
                }
            case MarkupMinimizedTagHelperDirectiveAttributeSyntax minimizedTagHelperDirectiveAttribute:
                {
                    prefixLocation = minimizedTagHelperDirectiveAttribute.NamePrefix?.Span;
                    selectedAttributeName = minimizedTagHelperDirectiveAttribute.FullName;
                    var fullNameSpan = TextSpan.FromBounds(minimizedTagHelperDirectiveAttribute.Transition.Span.Start, minimizedTagHelperDirectiveAttribute.Name.Span.End);
                    selectedAttributeNameLocation = fullNameSpan;
                    return true;
                }
            case MarkupMiscAttributeContentSyntax:
                prefixLocation = null;
                selectedAttributeName = null;
                selectedAttributeNameLocation = null;
                return true;
        }

        // Not an attribute type that we know of
        prefixLocation = null;
        selectedAttributeName = null;
        selectedAttributeNameLocation = null;
        return false;
    }
}
