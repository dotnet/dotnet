// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Microsoft.AspNetCore.Razor.Language;

/// <summary>
/// Enables retrieval of <see cref="TagHelperBinding"/>'s.
/// </summary>
internal sealed class TagHelperBinder
{
    private readonly Dictionary<string, HashSet<TagHelperDescriptor>> _registrations;
    private readonly string _tagHelperPrefix;

    /// <summary>
    /// Instantiates a new instance of the <see cref="TagHelperBinder"/>.
    /// </summary>
    /// <param name="tagHelperPrefix">The tag helper prefix being used by the document.</param>
    /// <param name="descriptors">The descriptors that the <see cref="TagHelperBinder"/> will pull from.</param>
    public TagHelperBinder(string tagHelperPrefix, IReadOnlyList<TagHelperDescriptor> descriptors)
    {
        _tagHelperPrefix = tagHelperPrefix;
        // To reduce the frequency of dictionary resizes we use the incoming number of descriptors as a heuristic
        _registrations = new Dictionary<string, HashSet<TagHelperDescriptor>>(descriptors.Count, StringComparer.OrdinalIgnoreCase);

        // Populate our registrations
        foreach (var descriptor in descriptors)
        {
            Register(descriptor);
        }
    }

    /// <summary>
    /// Gets all tag helpers that match the given HTML tag criteria.
    /// </summary>
    /// <param name="tagName">The name of the HTML tag to match. Providing a '*' tag name
    /// retrieves catch-all <see cref="TagHelperDescriptor"/>s (descriptors that target every tag).</param>
    /// <param name="attributes">Attributes on the HTML tag.</param>
    /// <param name="parentTagName">The parent tag name of the given <paramref name="tagName"/> tag.</param>
    /// <param name="parentIsTagHelper">Is the parent tag of the given <paramref name="tagName"/> tag a tag helper.</param>
    /// <returns><see cref="TagHelperDescriptor"/>s that apply to the given HTML tag criteria.
    /// Will return <c>null</c> if no <see cref="TagHelperDescriptor"/>s are a match.</returns>
    public TagHelperBinding GetBinding(
        string tagName,
        ImmutableArray<KeyValuePair<string, string>> attributes,
        string parentTagName,
        bool parentIsTagHelper)
    {
        if (!string.IsNullOrEmpty(_tagHelperPrefix) &&
            (tagName.Length <= _tagHelperPrefix.Length ||
            !tagName.StartsWith(_tagHelperPrefix, StringComparison.OrdinalIgnoreCase)))
        {
            // The tagName doesn't have the tag helper prefix, we can short circuit.
            return null;
        }

        IEnumerable<TagHelperDescriptor> descriptors;

        // Ensure there's a HashSet to use.
        if (!_registrations.TryGetValue(TagHelperMatchingConventions.ElementCatchAllName, out HashSet<TagHelperDescriptor> catchAllDescriptors))
        {
            descriptors = new HashSet<TagHelperDescriptor>(TagHelperDescriptorComparer.Default);
        }
        else
        {
            descriptors = catchAllDescriptors;
        }

        // If we have a tag name associated with the requested name, we need to combine matchingDescriptors
        // with all the catch-all descriptors.
        if (_registrations.TryGetValue(tagName, out HashSet<TagHelperDescriptor> matchingDescriptors))
        {
            descriptors = matchingDescriptors.Concat(descriptors);
        }

        var tagNameWithoutPrefix = tagName.AsSpanOrDefault();
        var parentTagNameWithoutPrefix = parentTagName.AsSpanOrDefault();

        if (_tagHelperPrefix is { Length: var length and > 0 })
        {
            tagNameWithoutPrefix = tagNameWithoutPrefix[length..];

            if (parentIsTagHelper)
            {
                parentTagNameWithoutPrefix = parentTagNameWithoutPrefix[length..];
            }
        }

        Dictionary<TagHelperDescriptor, IReadOnlyList<TagMatchingRuleDescriptor>> applicableDescriptorMappings = null;
        foreach (var descriptor in descriptors)
        {
            // We're avoiding descriptor.TagMatchingRules.Where and applicableRules.Any() to avoid
            // Enumerator allocations on this hot path
            List<TagMatchingRuleDescriptor> applicableRules = null;
            for (var i = 0; i < descriptor.TagMatchingRules.Count; i++)
            {
                var rule = descriptor.TagMatchingRules[i];
                if (TagHelperMatchingConventions.SatisfiesRule(tagNameWithoutPrefix, parentTagNameWithoutPrefix, attributes, rule))
                {
                    applicableRules ??= new List<TagMatchingRuleDescriptor>();
                    applicableRules.Add(rule);
                }
            }

            if (applicableRules != null && applicableRules.Count > 0)
            {
                applicableDescriptorMappings ??= new Dictionary<TagHelperDescriptor, IReadOnlyList<TagMatchingRuleDescriptor>>();
                applicableDescriptorMappings[descriptor] = applicableRules;
            }
        }

        if (applicableDescriptorMappings == null)
        {
            return null;
        }

        var tagHelperBinding = new TagHelperBinding(
            tagName,
            attributes,
            parentTagName,
            applicableDescriptorMappings,
            _tagHelperPrefix);

        return tagHelperBinding;
    }

    private void Register(TagHelperDescriptor descriptor)
    {
        var count = descriptor.TagMatchingRules.Count;
        for (var i = 0; i < count; i++)
        {
            var rule = descriptor.TagMatchingRules[i];
            var registrationKey =
                string.Equals(rule.TagName, TagHelperMatchingConventions.ElementCatchAllName, StringComparison.Ordinal) ?
                TagHelperMatchingConventions.ElementCatchAllName :
                _tagHelperPrefix + rule.TagName;

            // Ensure there's a HashSet to add the descriptor to.
            if (!_registrations.TryGetValue(registrationKey, out HashSet<TagHelperDescriptor> descriptorSet))
            {
                descriptorSet = new HashSet<TagHelperDescriptor>(TagHelperDescriptorComparer.Default);
                _registrations[registrationKey] = descriptorSet;
            }

            _ = descriptorSet.Add(descriptor);
        }
    }
}
