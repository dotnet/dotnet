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
        TagMatchingRuleDescriptor rule,
        ReadOnlySpan<char> tagNameWithoutPrefix,
        ReadOnlySpan<char> parentTagNameWithoutPrefix,
        ImmutableArray<KeyValuePair<string, string>> tagAttributes)
    {
        return SatisfiesTagName(rule, tagNameWithoutPrefix) &&
               SatisfiesParentTag(rule, parentTagNameWithoutPrefix) &&
               SatisfiesAttributes(rule, tagAttributes);
    }

    public static bool SatisfiesTagName(
        TagMatchingRuleDescriptor rule,
        ReadOnlySpan<char> tagNameWithoutPrefix,
        StringComparison? comparisonOverride = null)
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
            !tagNameWithoutPrefix.Equals(rule.TagName.AsSpan(), comparisonOverride ?? rule.GetComparison()))
        {
            return false;
        }

        return true;
    }

    public static bool SatisfiesParentTag(
        TagMatchingRuleDescriptor rule,
        ReadOnlySpan<char> parentTagNameWithoutPrefix)
    {
        if (rule.ParentTag != null &&
            !parentTagNameWithoutPrefix.Equals(rule.ParentTag.AsSpan(), rule.GetComparison()))
        {
            return false;
        }

        return true;
    }

    public static bool SatisfiesAttributes(
        TagMatchingRuleDescriptor rule,
        ImmutableArray<KeyValuePair<string, string>> tagAttributes)
    {
        foreach (var requiredAttribute in rule.Attributes)
        {
            var satisfied = false;

            foreach (var (attributeName, attributeValue) in tagAttributes)
            {
                if (SatisfiesRequiredAttribute(requiredAttribute, attributeName, attributeValue))
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
        return SatisfiesBoundAttributeName(descriptor, name.AsSpan()) ||
               SatisfiesBoundAttributeIndexer(descriptor, name.AsSpan()) ||
               GetSatisfyingBoundAttributeWithParameter(descriptor, name) is not null;
    }

    private static BoundAttributeParameterDescriptor? GetSatisfyingBoundAttributeWithParameter(
        BoundAttributeDescriptor descriptor,
        string name)
    {
        foreach (var parameter in descriptor.Parameters)
        {
            if (SatisfiesBoundAttributeWithParameter(parameter, name, descriptor))
            {
                return parameter;
            }
        }

        return null;
    }

    public static bool SatisfiesBoundAttributeIndexer(BoundAttributeDescriptor descriptor, ReadOnlySpan<char> name)
    {
        return descriptor.IndexerNamePrefix != null &&
               !SatisfiesBoundAttributeName(descriptor, name) &&
               name.StartsWith(descriptor.IndexerNamePrefix.AsSpan(), descriptor.GetComparison());
    }

    public static bool SatisfiesBoundAttributeWithParameter(BoundAttributeParameterDescriptor descriptor, string name, BoundAttributeDescriptor parent)
    {
        if (TryGetBoundAttributeParameter(name, out var attributeName, out var parameterName))
        {
            var satisfiesBoundAttributeName = SatisfiesBoundAttributeName(parent, attributeName);
            var satisfiesBoundAttributeIndexer = SatisfiesBoundAttributeIndexer(parent, attributeName);
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
        TagHelperDescriptor descriptor,
        string name,
        out BoundAttributeDescriptor? boundAttribute,
        out bool indexerMatch,
        out bool parameterMatch,
        out BoundAttributeParameterDescriptor? boundAttributeParameter)
    {
        indexerMatch = false;
        parameterMatch = false;
        boundAttribute = null;
        boundAttributeParameter = null;

        if (descriptor == null || name.IsNullOrEmpty())
        {
            return false;
        }

        // First, check if we have a bound attribute descriptor that matches the parameter if it exists.
        foreach (var attribute in descriptor.BoundAttributes)
        {
            boundAttributeParameter = GetSatisfyingBoundAttributeWithParameter(attribute, name);

            if (boundAttributeParameter != null)
            {
                boundAttribute = attribute;
                indexerMatch = SatisfiesBoundAttributeIndexer(attribute, name.AsSpan());
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
                indexerMatch = SatisfiesBoundAttributeIndexer(attribute, name.AsSpan());
                return true;
            }
        }

        // No matches found.
        return false;
    }

    private static bool SatisfiesBoundAttributeName(BoundAttributeDescriptor descriptor, ReadOnlySpan<char> name)
    {
        return name.Equals(descriptor.Name.AsSpanOrDefault(), descriptor.GetComparison());
    }

    // Internal for testing
    internal static bool SatisfiesRequiredAttribute(
        RequiredAttributeDescriptor descriptor,
        string attributeName,
        string attributeValue)
    {
        var nameMatches = false;
        if (descriptor.NameComparison == RequiredAttributeNameComparison.FullMatch)
        {
            nameMatches = string.Equals(descriptor.Name, attributeName, descriptor.GetComparison());
        }
        else if (descriptor.NameComparison == RequiredAttributeNameComparison.PrefixMatch)
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
            case RequiredAttributeValueComparison.None:
                return true;
            case RequiredAttributeValueComparison.PrefixMatch: // Value starts with
                return attributeValue.StartsWith(descriptor.Value.AssumeNotNull(), StringComparison.Ordinal);
            case RequiredAttributeValueComparison.SuffixMatch: // Value ends with
                return attributeValue.EndsWith(descriptor.Value.AssumeNotNull(), StringComparison.Ordinal);
            case RequiredAttributeValueComparison.FullMatch: // Value equals
                return string.Equals(attributeValue, descriptor.Value, StringComparison.Ordinal);
            default:
                Debug.Assert(false, "Unknown value comparison.");
                return false;
        }
    }

    internal static StringComparison GetComparison(this BoundAttributeDescriptor descriptor)
        => descriptor.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

    private static StringComparison GetComparison(this BoundAttributeParameterDescriptor descriptor)
        => descriptor.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

    private static StringComparison GetComparison(this RequiredAttributeDescriptor descriptor)
        => descriptor.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

    private static StringComparison GetComparison(this TagMatchingRuleDescriptor descriptor)
        => descriptor.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
}
