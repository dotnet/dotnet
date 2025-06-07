// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language;

/// <summary>
/// Enables retrieval of <see cref="TagHelperBinding"/>'s.
/// </summary>
internal sealed class TagHelperBinder
{
    private readonly ImmutableArray<TagHelperDescriptor> _catchAllDescriptors;
    private readonly ReadOnlyDictionary<string, ImmutableArray<TagHelperDescriptor>> _tagNameToDescriptorsMap;

    public string? TagNamePrefix { get; }
    public ImmutableArray<TagHelperDescriptor> Descriptors { get; }

    /// <summary>
    /// Instantiates a new instance of the <see cref="TagHelperBinder"/>.
    /// </summary>
    /// <param name="tagNamePrefix">The tag helper prefix being used by the document.</param>
    /// <param name="descriptors">The <see cref="TagHelperDescriptor"/>s that the <see cref="TagHelperBinder"/>
    /// will pull from.</param>
    public TagHelperBinder(string? tagNamePrefix, ImmutableArray<TagHelperDescriptor> descriptors)
    {
        TagNamePrefix = tagNamePrefix;
        Descriptors = descriptors.NullToEmpty();

        ProcessDescriptors(descriptors, tagNamePrefix, out _tagNameToDescriptorsMap, out _catchAllDescriptors);
    }

    private static void ProcessDescriptors(
        ImmutableArray<TagHelperDescriptor> descriptors,
        string? tagNamePrefix,
        out ReadOnlyDictionary<string, ImmutableArray<TagHelperDescriptor>> tagNameToDescriptorsMap,
        out ImmutableArray<TagHelperDescriptor> catchAllDescriptors)
    {
        using var catchAllBuilder = new PooledArrayBuilder<TagHelperDescriptor>();
        using var pooledMap = StringDictionaryPool<ImmutableArray<TagHelperDescriptor>.Builder>.OrdinalIgnoreCase.GetPooledObject(out var mapBuilder);
        using var pooledSet = HashSetPool<TagHelperDescriptor>.GetPooledObject(out var distinctSet);

        // Build a map of tag name -> tag helpers.
        foreach (var descriptor in descriptors)
        {
            if (!distinctSet.Add(descriptor))
            {
                // We're already seen this descriptor, skip it.
                continue;
            }

            foreach (var rule in descriptor.TagMatchingRules)
            {
                if (rule.TagName == TagHelperMatchingConventions.ElementCatchAllName)
                {
                    // This is a catch-all descriptor, we can keep track of it separately.
                    catchAllBuilder.Add(descriptor);
                }
                else
                {
                    // This is a specific tag name, we need to add it to the map.
                    var tagName = tagNamePrefix + rule.TagName;
                    var builder = mapBuilder.GetOrAdd(tagName, _ => ImmutableArray.CreateBuilder<TagHelperDescriptor>());

                    builder.Add(descriptor);
                }
            }
        }

        // Build the final dictionary with immutable arrays.
        var map = new Dictionary<string, ImmutableArray<TagHelperDescriptor>>(capacity: mapBuilder.Count, StringComparer.OrdinalIgnoreCase);

        foreach (var (key, value) in mapBuilder)
        {
            map.Add(key, value.ToImmutableAndClear());
        }

        tagNameToDescriptorsMap = new ReadOnlyDictionary<string, ImmutableArray<TagHelperDescriptor>>(map);

        // Build the catch all descriptors array.
        catchAllDescriptors = catchAllBuilder.ToImmutableAndClear();
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
    /// Will return <see langword="null"/> if no <see cref="TagHelperDescriptor"/>s are a match.</returns>
    public TagHelperBinding? GetBinding(
        string tagName,
        ImmutableArray<KeyValuePair<string, string>> attributes,
        string? parentTagName,
        bool parentIsTagHelper)
    {
        var tagNameSpan = tagName.AsSpan();
        var parentTagNameSpan = parentTagName.AsSpan();
        var tagNamePrefixSpan = TagNamePrefix.AsSpan();

        if (!tagNamePrefixSpan.IsEmpty)
        {
            if (!tagNameSpan.StartsWith(tagNamePrefixSpan, StringComparison.OrdinalIgnoreCase))
            {
                // The tag name doesn't start with the prefix. So, we're done.
                return null;
            }

            tagNameSpan = tagNameSpan[tagNamePrefixSpan.Length..];

            if (parentIsTagHelper)
            {
                Debug.Assert(
                    parentTagNameSpan.StartsWith(tagNamePrefixSpan, StringComparison.OrdinalIgnoreCase),
                    "If the parent is a tag helper, it must start with the tag name prefix.");

                parentTagNameSpan = parentTagNameSpan[tagNamePrefixSpan.Length..];
            }
        }

        using var resultsBuilder = new PooledArrayBuilder<TagHelperBoundRulesInfo>();
        using var tempRulesBuilder = new PooledArrayBuilder<TagMatchingRuleDescriptor>();
        using var pooledSet = HashSetPool<TagHelperDescriptor>.GetPooledObject(out var distinctSet);

        // First, try any tag helpers with this tag name.
        if (_tagNameToDescriptorsMap.TryGetValue(tagName, out var matchingDescriptors))
        {
            CollectBoundRulesInfo(
                matchingDescriptors,
                tagNameSpan, parentTagNameSpan, attributes,
                ref resultsBuilder.AsRef(), ref tempRulesBuilder.AsRef(), distinctSet);
        }

        // Next, try any "catch all" descriptors.
        CollectBoundRulesInfo(
            _catchAllDescriptors,
            tagNameSpan, parentTagNameSpan, attributes,
            ref resultsBuilder.AsRef(), ref tempRulesBuilder.AsRef(), distinctSet);

        return resultsBuilder.Count > 0
            ? new(resultsBuilder.ToImmutableAndClear(), tagName, parentTagName, attributes, TagNamePrefix)
            : null;

        static void CollectBoundRulesInfo(
            ImmutableArray<TagHelperDescriptor> descriptors,
            ReadOnlySpan<char> tagName,
            ReadOnlySpan<char> parentTagName,
            ImmutableArray<KeyValuePair<string, string>> attributes,
            ref PooledArrayBuilder<TagHelperBoundRulesInfo> resultsBuilder,
            ref PooledArrayBuilder<TagMatchingRuleDescriptor> tempRulesBuilder,
            HashSet<TagHelperDescriptor> distinctSet)
        {
            foreach (var descriptor in descriptors)
            {
                if (!distinctSet.Add(descriptor))
                {
                    // We're already seen this descriptor, skip it.
                    continue;
                }

                Debug.Assert(tempRulesBuilder.Count == 0);

                foreach (var rule in descriptor.TagMatchingRules)
                {
                    if (TagHelperMatchingConventions.SatisfiesRule(rule, tagName, parentTagName, attributes))
                    {
                        tempRulesBuilder.Add(rule);
                    }
                }

                if (tempRulesBuilder.Count > 0)
                {
                    resultsBuilder.Add(new(descriptor, tempRulesBuilder.ToImmutable()));
                }

                tempRulesBuilder.Clear();
            }
        }
    }
}
