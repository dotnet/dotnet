// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.VisualStudio.Editor.Razor;

[Shared]
[Export(typeof(ITagHelperFactsService))]
internal class TagHelperFactsService : ITagHelperFactsService
{
    public TagHelperBinding? GetTagHelperBinding(
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

        var descriptors = documentContext.TagHelpers;
        if (descriptors is null or { Count: 0 })
        {
            return null;
        }

        var prefix = documentContext.Prefix;
        var tagHelperBinder = new TagHelperBinder(prefix, descriptors);
        var binding = tagHelperBinder.GetBinding(tagName, attributes, parentTag, parentIsTagHelper);

        return binding;
    }

    public ImmutableArray<BoundAttributeDescriptor> GetBoundTagHelperAttributes(
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

        return matchingBoundAttributes.DrainToImmutable();
    }

    public ImmutableArray<TagHelperDescriptor> GetTagHelpersGivenTag(
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

        var tagHelpers = documentContext?.TagHelpers;
        if (tagHelpers is not { Count: > 0})
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
        for (var i = 0; i < tagHelpers.Count; i++)
        {
            var tagHelper = tagHelpers[i];

            foreach (var rule in tagHelper.TagMatchingRules)
            {
                if (TagHelperMatchingConventions.SatisfiesTagName(tagNameWithoutPrefix, rule) &&
                    TagHelperMatchingConventions.SatisfiesParentTag(parentTag.AsSpanOrDefault(), rule))
                {
                    matchingDescriptors.Add(tagHelper);
                    break;
                }
            }
        }

        return matchingDescriptors.DrainToImmutable();
    }

    public ImmutableArray<TagHelperDescriptor> GetTagHelpersGivenParent(TagHelperDocumentContext documentContext, string? parentTag)
    {
        if (documentContext is null)
        {
            throw new ArgumentNullException(nameof(documentContext));
        }

        var tagHelpers = documentContext?.TagHelpers;
        if (tagHelpers is not { Count: > 0 })
        {
            return ImmutableArray<TagHelperDescriptor>.Empty;
        }

        using var matchingDescriptors = new PooledArrayBuilder<TagHelperDescriptor>();

        for (var i = 0; i < tagHelpers.Count; i++)
        {
            var descriptor = tagHelpers[i];

            foreach (var rule in descriptor.TagMatchingRules)
            {
                if (TagHelperMatchingConventions.SatisfiesParentTag(parentTag.AsSpanOrDefault(), rule))
                {
                    matchingDescriptors.Add(descriptor);
                    break;
                }
            }
        }

        return matchingDescriptors.DrainToImmutable();
    }

    public ImmutableArray<KeyValuePair<string, string>> StringifyAttributes(SyntaxList<RazorSyntaxNode> attributes)
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

        return stringifiedAttributes.DrainToImmutable();
    }

    public (string? ancestorTagName, bool ancestorIsTagHelper) GetNearestAncestorTagInfo(IEnumerable<SyntaxNode> ancestors)
    {
        foreach (var ancestor in ancestors)
        {
            if (ancestor is MarkupElementSyntax element)
            {
                // It's possible for start tag to be null in malformed cases.
                var name = element.StartTag?.Name?.Content ?? string.Empty;
                return (name, ancestorIsTagHelper: false);
            }
            else if (ancestor is MarkupTagHelperElementSyntax tagHelperElement)
            {
                // It's possible for start tag to be null in malformed cases.
                var name = tagHelperElement.StartTag?.Name?.Content ?? string.Empty;
                return (name, ancestorIsTagHelper: true);
            }
        }

        return (ancestorTagName: null, ancestorIsTagHelper: false);
    }
}
