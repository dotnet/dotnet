// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.VisualStudio.Editor.Razor;

internal static class TagHelperFacts
{
    public static TagHelperBinding? GetTagHelperBinding(
        TagHelperDocumentContext documentContext,
        string? tagName,
        ImmutableArray<KeyValuePair<string, string>> attributes,
        string? parentTag,
        bool parentIsTagHelper)
    {
        if (documentContext is null)
        {
            throw new ArgumentNullException(nameof(documentContext));
        }

        if (attributes.IsDefault)
        {
            throw new ArgumentNullException(nameof(attributes));
        }

        if (tagName is null)
        {
            return null;
        }

        if (documentContext.TagHelpers.Length == 0)
        {
            return null;
        }

        var binder = documentContext.GetBinder();

        return binder.GetBinding(tagName, attributes, parentTag, parentIsTagHelper);
    }

    public static ImmutableArray<BoundAttributeDescriptor> GetBoundTagHelperAttributes(
        TagHelperDocumentContext documentContext,
        string attributeName,
        TagHelperBinding binding)
    {
        if (documentContext is null)
        {
            throw new ArgumentNullException(nameof(documentContext));
        }

        if (attributeName is null)
        {
            throw new ArgumentNullException(nameof(attributeName));
        }

        if (binding is null)
        {
            throw new ArgumentNullException(nameof(binding));
        }

        using var matchingBoundAttributes = new PooledArrayBuilder<BoundAttributeDescriptor>();

        foreach (var tagHelper in binding.Descriptors)
        {
            foreach (var boundAttribute in tagHelper.BoundAttributes)
            {
                if (TagHelperMatchingConventions.CanSatisfyBoundAttribute(attributeName, boundAttribute))
                {
                    matchingBoundAttributes.Add(boundAttribute);

                    // Only one bound attribute can match an attribute
                    break;
                }
            }
        }

        return matchingBoundAttributes.ToImmutableAndClear();
    }

    public static ImmutableArray<TagHelperDescriptor> GetTagHelpersGivenTag(
        TagHelperDocumentContext documentContext,
        string tagName,
        string? parentTag)
    {
        if (documentContext is null)
        {
            throw new ArgumentNullException(nameof(documentContext));
        }

        if (tagName is null)
        {
            throw new ArgumentNullException(nameof(tagName));
        }

        if (documentContext?.TagHelpers is not { Length: > 0 } tagHelpers)
        {
            return ImmutableArray<TagHelperDescriptor>.Empty;
        }

        var prefix = documentContext?.Prefix ?? string.Empty;
        if (!tagName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            // Can't possibly match TagHelpers, it doesn't start with the TagHelperPrefix.
            return ImmutableArray<TagHelperDescriptor>.Empty;
        }

        using var matchingDescriptors = new PooledArrayBuilder<TagHelperDescriptor>();

        var tagNameWithoutPrefix = tagName.AsSpan()[prefix.Length..];

        foreach (var tagHelper in tagHelpers)
        {
            foreach (var rule in tagHelper.TagMatchingRules)
            {
                if (TagHelperMatchingConventions.SatisfiesTagName(rule, tagNameWithoutPrefix) &&
                    TagHelperMatchingConventions.SatisfiesParentTag(rule, parentTag.AsSpanOrDefault()))
                {
                    matchingDescriptors.Add(tagHelper);
                    break;
                }
            }
        }

        return matchingDescriptors.ToImmutableAndClear();
    }

    public static ImmutableArray<TagHelperDescriptor> GetTagHelpersGivenParent(TagHelperDocumentContext documentContext, string? parentTag)
    {
        if (documentContext is null)
        {
            throw new ArgumentNullException(nameof(documentContext));
        }

        if (documentContext?.TagHelpers is not { Length: > 0 } tagHelpers)
        {
            return ImmutableArray<TagHelperDescriptor>.Empty;
        }

        using var matchingDescriptors = new PooledArrayBuilder<TagHelperDescriptor>();

        foreach (var descriptor in tagHelpers)
        {
            foreach (var rule in descriptor.TagMatchingRules)
            {
                if (TagHelperMatchingConventions.SatisfiesParentTag(rule, parentTag.AsSpanOrDefault()))
                {
                    matchingDescriptors.Add(descriptor);
                    break;
                }
            }
        }

        return matchingDescriptors.ToImmutableAndClear();
    }

    public static ImmutableArray<KeyValuePair<string, string>> StringifyAttributes(SyntaxList<RazorSyntaxNode> attributes)
    {
        using var stringifiedAttributes = new PooledArrayBuilder<KeyValuePair<string, string>>();

        foreach (var attribute in attributes)
        {
            switch (attribute)
            {
                case MarkupTagHelperAttributeSyntax tagHelperAttribute:
                    {
                        var name = tagHelperAttribute.Name.GetContent();
                        var value = tagHelperAttribute.Value?.GetContent() ?? string.Empty;
                        stringifiedAttributes.Add(new KeyValuePair<string, string>(name, value));
                        break;
                    }

                case MarkupMinimizedTagHelperAttributeSyntax minimizedTagHelperAttribute:
                    {
                        var name = minimizedTagHelperAttribute.Name.GetContent();
                        stringifiedAttributes.Add(new KeyValuePair<string, string>(name, string.Empty));
                        break;
                    }

                case MarkupAttributeBlockSyntax markupAttribute:
                    {
                        var name = markupAttribute.Name.GetContent();
                        var value = markupAttribute.Value?.GetContent() ?? string.Empty;
                        stringifiedAttributes.Add(new KeyValuePair<string, string>(name, value));
                        break;
                    }

                case MarkupMinimizedAttributeBlockSyntax minimizedMarkupAttribute:
                    {
                        var name = minimizedMarkupAttribute.Name.GetContent();
                        stringifiedAttributes.Add(new KeyValuePair<string, string>(name, string.Empty));
                        break;
                    }

                case MarkupTagHelperDirectiveAttributeSyntax directiveAttribute:
                    {
                        var name = directiveAttribute.FullName;
                        var value = directiveAttribute.Value?.GetContent() ?? string.Empty;
                        stringifiedAttributes.Add(new KeyValuePair<string, string>(name, value));
                        break;
                    }

                case MarkupMinimizedTagHelperDirectiveAttributeSyntax minimizedDirectiveAttribute:
                    {
                        var name = minimizedDirectiveAttribute.FullName;
                        stringifiedAttributes.Add(new KeyValuePair<string, string>(name, string.Empty));
                        break;
                    }
            }
        }

        return stringifiedAttributes.ToImmutableAndClear();
    }

    public static (string? ancestorTagName, bool ancestorIsTagHelper) GetNearestAncestorTagInfo(IEnumerable<SyntaxNode> ancestors)
    {
        foreach (var ancestor in ancestors)
        {
            switch (ancestor)
            {
                case MarkupElementSyntax { StartTag: var startTag }:
                    {
                        // It's possible for start tag to be null in malformed cases.
                        var name = startTag?.Name.Content ?? string.Empty;
                        return (name, ancestorIsTagHelper: false);
                    }

                case MarkupTagHelperElementSyntax { StartTag: var startTag }:
                    {
                        // It's possible for start tag to be null in malformed cases.
                        var name = startTag?.Name.Content ?? string.Empty;
                        return (name, ancestorIsTagHelper: true);
                    }
            }
        }

        return (ancestorTagName: null, ancestorIsTagHelper: false);
    }
}
