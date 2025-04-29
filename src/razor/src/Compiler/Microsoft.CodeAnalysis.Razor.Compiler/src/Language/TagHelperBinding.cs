// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.Language;

internal sealed class TagHelperBinding
{
    public string TagName { get; }
    public string? ParentTagName { get; }
    public ImmutableArray<KeyValuePair<string, string>> Attributes { get; }
    public FrozenDictionary<TagHelperDescriptor, ImmutableArray<TagMatchingRuleDescriptor>> Mappings { get; }
    public string? TagHelperPrefix { get; }

    public ImmutableArray<TagHelperDescriptor> Descriptors => Mappings.Keys;

    internal TagHelperBinding(
        string tagName,
        ImmutableArray<KeyValuePair<string, string>> attributes,
        string? parentTagName,
        FrozenDictionary<TagHelperDescriptor, ImmutableArray<TagMatchingRuleDescriptor>> mappings,
        string? tagHelperPrefix)
    {
        TagName = tagName;
        Attributes = attributes;
        ParentTagName = parentTagName;
        Mappings = mappings;
        TagHelperPrefix = tagHelperPrefix;
    }

    /// <summary>
    /// Gets a value that indicates whether the the binding matched on attributes only.
    /// </summary>
    /// <returns><c>false</c> if the entire element should be classified as a tag helper.</returns>
    /// <remarks>
    /// If this returns <c>true</c>, use <c>TagHelperFactsService.GetBoundTagHelperAttributes</c> to find the
    /// set of attributes that should be considered part of the match.
    /// </remarks>
    public bool IsAttributeMatch
    {
        get
        {
            foreach (var descriptor in Mappings.Keys)
            {
                if (!descriptor.Metadata.TryGetValue(TagHelperMetadata.Common.ClassifyAttributesOnly, out var value) ||
                    !string.Equals(value, bool.TrueString, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            // All the matching tag helpers want to be classified with **attributes only**.
            //
            // Ex: (components)
            //
            //      <button onclick="..." />
            return true;
        }
    }
}
