// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudio.Editor.Razor;

internal static class HtmlFacts
{
    private static readonly HashSet<string> s_htmlSchemaTagNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "DOCTYPE",
        "a",
        "abbr",
        "acronym",
        "address",
        "applet",
        "area",
        "article",
        "aside",
        "audio",
        "b",
        "base",
        "basefont",
        "bdi",
        "bdo",
        "big",
        "blockquote",
        "body",
        "br",
        "button",
        "canvas",
        "caption",
        "center",
        "cite",
        "code",
        "col",
        "colgroup",
        "data",
        "datalist",
        "dd",
        "del",
        "details",
        "dfn",
        "dialog",
        "dir",
        "div",
        "dl",
        "dt",
        "em",
        "embed",
        "fieldset",
        "figcaption",
        "figure",
        "font",
        "footer",
        "form",
        "frame",
        "frameset",
        "h1",
        "h2",
        "h3",
        "h4",
        "h5",
        "h6",
        "head",
        "header",
        "hr",
        "html",
        "i",
        "iframe",
        "img",
        "input",
        "ins",
        "kbd",
        "label",
        "legend",
        "li",
        "link",
        "main",
        "map",
        "mark",
        "meta",
        "meter",
        "nav",
        "noframes",
        "noscript",
        "object",
        "ol",
        "optgroup",
        "option",
        "output",
        "p",
        "param",
        "picture",
        "pre",
        "progress",
        "q",
        "rp",
        "rt",
        "ruby",
        "s",
        "samp",
        "script",
        "section",
        "select",
        "small",
        "source",
        "span",
        "strike",
        "strong",
        "style",
        "sub",
        "summary",
        "sup",
        "svg",
        "table",
        "tbody",
        "td",
        "template",
        "textarea",
        "tfoot",
        "th",
        "thead",
        "time",
        "title",
        "tr",
        "track",
        "tt",
        "u",
        "ul",
        "var",
        "video",
        "wbr",
    };

    public static bool IsHtmlTagName(string name)
        => s_htmlSchemaTagNames.Contains(name);

    public static bool TryGetElementInfo(
        SyntaxNode element,
        [NotNullWhen(true)] out SyntaxToken? containingTagNameToken,
        out SyntaxList<RazorSyntaxNode> attributeNodes,
        [NotNullWhen(true)] out SyntaxToken? closingForwardSlashOrCloseAngleToken)
    {
        switch (element)
        {
            case MarkupStartTagSyntax startTag:
                containingTagNameToken = startTag.Name;
                attributeNodes = startTag.Attributes;
                closingForwardSlashOrCloseAngleToken = startTag.ForwardSlash ?? startTag.CloseAngle;
                return true;
            case MarkupEndTagSyntax { Parent: MarkupElementSyntax parent } endTag:
                containingTagNameToken = endTag.Name;
                attributeNodes = parent.StartTag?.Attributes ?? new SyntaxList<RazorSyntaxNode>();
                closingForwardSlashOrCloseAngleToken = endTag.ForwardSlash ?? endTag.CloseAngle;
                return true;
            case MarkupTagHelperStartTagSyntax startTagHelper:
                containingTagNameToken = startTagHelper.Name;
                attributeNodes = startTagHelper.Attributes;
                closingForwardSlashOrCloseAngleToken = startTagHelper.ForwardSlash ?? startTagHelper.CloseAngle;
                return true;
            case MarkupTagHelperEndTagSyntax { Parent: MarkupTagHelperElementSyntax parent } endTagHelper:
                containingTagNameToken = endTagHelper.Name;
                attributeNodes = parent.StartTag?.Attributes ?? new SyntaxList<RazorSyntaxNode>();
                closingForwardSlashOrCloseAngleToken = endTagHelper.ForwardSlash ?? endTagHelper.CloseAngle;
                return true;
            default:
                containingTagNameToken = null;
                attributeNodes = default;
                closingForwardSlashOrCloseAngleToken = null;
                return false;
        }
    }

    public static bool TryGetAttributeInfo(
        SyntaxNode attribute,
        [NotNullWhen(true)] out SyntaxToken? containingTagNameToken,
        out TextSpan? prefixLocation,
        out string? selectedAttributeName,
        out TextSpan? selectedAttributeNameLocation,
        out SyntaxList<RazorSyntaxNode> attributeNodes)
    {
        if (!TryGetElementInfo(attribute.Parent, out containingTagNameToken, out attributeNodes, closingForwardSlashOrCloseAngleToken: out _))
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
