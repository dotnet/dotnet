// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Microsoft.AspNetCore.Razor.Language;

internal static class TagHelperMatchingConventions
{
    public const string ElementCatchAllName = "*";

    public const char ElementOptOutCharacter = '!';

    public static bool SatisfiesRule(
        ReadOnlySpan<char> tagNameWithoutPrefix,
        ReadOnlySpan<char> parentTagNameWithoutPrefix,
        ImmutableArray<KeyValuePair<string, string>> tagAttributes,
        TagMatchingRuleDescriptor rule)
    {
        return SatisfiesTagName(tagNameWithoutPrefix, rule) &&
               SatisfiesParentTag(parentTagNameWithoutPrefix, rule) &&
               SatisfiesAttributes(tagAttributes, rule);
    }

    public static bool SatisfiesTagName(ReadOnlySpan<char> tagNameWithoutPrefix, TagMatchingRuleDescriptor rule)
    {
        if (tagNameWithoutPrefix.IsEmpty)
        {
            return false;
        }

        if (tagNameWithoutPrefix[0] == ElementOptOutCharacter)
        {
            // TagHelpers can never satisfy tag names that are prefixed with the opt-out character.
            return false;
        }

        if (rule.TagName is not (null or ElementCatchAllName) &&
            !tagNameWithoutPrefix.Equals(rule.TagName.AsSpan(), rule.GetComparison()))
        {
            return false;
        }

        return true;
    }

    public static bool SatisfiesParentTag(ReadOnlySpan<char> parentTagNameWithoutPrefix, TagMatchingRuleDescriptor rule)
    {
        if (rule.ParentTag != null &&
            !parentTagNameWithoutPrefix.Equals(rule.ParentTag.AsSpan(), rule.GetComparison()))
        {
            return false;
        }

        return true;
    }

    public static bool SatisfiesAttributes(ImmutableArray<KeyValuePair<string, string>> tagAttributes, TagMatchingRuleDescriptor rule)
    {
        var requiredAttributes = rule.Attributes;
        var count = requiredAttributes.Count;

        for (var i = 0; i < count; i++)
        {
            var requiredAttribute = requiredAttributes[i];
            var satisfied = false;

            foreach (var (attributeName, attributeValue) in tagAttributes)
            {
                if (SatisfiesRequiredAttribute(attributeName, attributeValue, requiredAttribute))
                {
                    satisfied = true;
                    break;
                }
            }

            if (!satisfied)
            {
                return false;
            }
        }

        return true;
    }

    public static bool CanSatisfyBoundAttribute(string name, BoundAttributeDescriptor descriptor)
    {
        return SatisfiesBoundAttributeName(name.AsSpan(), descriptor) ||
               SatisfiesBoundAttributeIndexer(name.AsSpan(), descriptor) ||
               GetSatifyingBoundAttributeWithParameter(name, descriptor, descriptor.BoundAttributeParameters) is not null;
    }

    private static BoundAttributeParameterDescriptor? GetSatifyingBoundAttributeWithParameter(
        string name,
        BoundAttributeDescriptor descriptor,
        IReadOnlyList<BoundAttributeParameterDescriptor> boundAttributeParameters)
    {
        var count = boundAttributeParameters.Count;
        for (var i = 0; i < count; i++)
        {
            if (SatisfiesBoundAttributeWithParameter(name, descriptor, boundAttributeParameters[i]))
            {
                return boundAttributeParameters[i];
            }
        }

        return null;
    }

    public static bool SatisfiesBoundAttributeIndexer(ReadOnlySpan<char> name, BoundAttributeDescriptor descriptor)
    {
        return descriptor.IndexerNamePrefix != null &&
               !SatisfiesBoundAttributeName(name, descriptor) &&
               name.StartsWith(descriptor.IndexerNamePrefix.AsSpan(), descriptor.GetComparison());
    }

    public static bool SatisfiesBoundAttributeWithParameter(string name, BoundAttributeDescriptor parent, BoundAttributeParameterDescriptor descriptor)
    {
        if (TryGetBoundAttributeParameter(name, out var attributeName, out var parameterName))
        {
            var satisfiesBoundAttributeName = SatisfiesBoundAttributeName(attributeName, parent);
            var satisfiesBoundAttributeIndexer = SatisfiesBoundAttributeIndexer(attributeName, parent);
            var matchesParameter = parameterName.Equals(descriptor.Name.AsSpanOrDefault(), descriptor.GetComparison());
            return (satisfiesBoundAttributeName || satisfiesBoundAttributeIndexer) && matchesParameter;
        }

        return false;
    }

    public static bool TryGetBoundAttributeParameter(string fullAttributeName, out ReadOnlySpan<char> boundAttributeName)
    {
        boundAttributeName = default;

        var span = fullAttributeName.AsSpanOrDefault();

        if (span.IsEmpty)
        {
            return false;
        }

        var index = span.IndexOf(':');
        if (index < 0)
        {
            return false;
        }

        boundAttributeName = span[..index];
        return true;
    }

    private static bool TryGetBoundAttributeParameter(string fullAttributeName, out ReadOnlySpan<char> boundAttributeName, out ReadOnlySpan<char> parameterName)
    {
        boundAttributeName = default;
        parameterName = default;

        var span = fullAttributeName.AsSpanOrDefault();

        if (span.IsEmpty)
        {
            return false;
        }

        var index = span.IndexOf(':');
        if (index < 0)
        {
            return false;
        }

        boundAttributeName = span[..index];
        parameterName = span[(index + 1)..];
        return true;
    }

    public static bool TryGetFirstBoundAttributeMatch(
        string name,
        TagHelperDescriptor descriptor,
        out BoundAttributeDescriptor? boundAttribute,
        out bool indexerMatch,
        out bool parameterMatch,
        out BoundAttributeParameterDescriptor? boundAttributeParameter)
    {
        indexerMatch = false;
        parameterMatch = false;
        boundAttribute = null;
        boundAttributeParameter = null;

        if (string.IsNullOrEmpty(name) || descriptor == null)
        {
            return false;
        }

        // First, check if we have a bound attribute descriptor that matches the parameter if it exists.
        foreach (var attribute in descriptor.BoundAttributes)
        {
            boundAttributeParameter = GetSatifyingBoundAttributeWithParameter(name, attribute, attribute.BoundAttributeParameters);

            if (boundAttributeParameter != null)
            {
                boundAttribute = attribute;
                indexerMatch = SatisfiesBoundAttributeIndexer(name.AsSpan(), attribute);
                parameterMatch = true;
                return true;
            }
        }

        // If we reach here, either the attribute name doesn't contain a parameter portion or
        // the specified parameter isn't supported by any of the BoundAttributeDescriptors.
        foreach (var attribute in descriptor.BoundAttributes)
        {
            if (CanSatisfyBoundAttribute(name, attribute))
            {
                boundAttribute = attribute;
                indexerMatch = SatisfiesBoundAttributeIndexer(name.AsSpan(), attribute);
                return true;
            }
        }

        // No matches found.
        return false;
    }

    private static bool SatisfiesBoundAttributeName(ReadOnlySpan<char> name, BoundAttributeDescriptor descriptor)
    {
        return name.Equals(descriptor.Name.AsSpanOrDefault(), descriptor.GetComparison());
    }

    // Internal for testing
    internal static bool SatisfiesRequiredAttribute(string attributeName, string attributeValue, RequiredAttributeDescriptor descriptor)
    {
        var nameMatches = false;
        if (descriptor.NameComparison == RequiredAttributeDescriptor.NameComparisonMode.FullMatch)
        {
            nameMatches = string.Equals(descriptor.Name, attributeName, descriptor.GetComparison());
        }
        else if (descriptor.NameComparison == RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch)
        {
            // attributeName cannot equal the Name if comparing as a PrefixMatch.
            nameMatches = attributeName.Length != descriptor.Name.Length &&
                attributeName.StartsWith(descriptor.Name, descriptor.GetComparison());
        }
        else
        {
            Debug.Assert(false, "Unknown name comparison.");
        }

        if (!nameMatches)
        {
            return false;
        }

        switch (descriptor.ValueComparison)
        {
            case RequiredAttributeDescriptor.ValueComparisonMode.None:
                return true;
            case RequiredAttributeDescriptor.ValueComparisonMode.PrefixMatch: // Value starts with
                return attributeValue.StartsWith(descriptor.Value, StringComparison.Ordinal);
            case RequiredAttributeDescriptor.ValueComparisonMode.SuffixMatch: // Value ends with
                return attributeValue.EndsWith(descriptor.Value, StringComparison.Ordinal);
            case RequiredAttributeDescriptor.ValueComparisonMode.FullMatch: // Value equals
                return string.Equals(attributeValue, descriptor.Value, StringComparison.Ordinal);
            default:
                Debug.Assert(false, "Unknown value comparison.");
                return false;
        }
    }

    private static StringComparison GetComparison(this BoundAttributeDescriptor descriptor)
        => descriptor.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

    private static StringComparison GetComparison(this BoundAttributeParameterDescriptor descriptor)
        => descriptor.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

    private static StringComparison GetComparison(this RequiredAttributeDescriptor descriptor)
        => descriptor.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

    private static StringComparison GetComparison(this TagMatchingRuleDescriptor descriptor)
        => descriptor.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
}
