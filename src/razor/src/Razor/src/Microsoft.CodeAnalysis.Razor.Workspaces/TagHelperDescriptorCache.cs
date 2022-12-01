// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Internal;

namespace Microsoft.CodeAnalysis.Razor;

internal static class TagHelperDescriptorCache
{
    private static readonly MemoryCache<int, TagHelperDescriptor> s_cachedTagHelperDescriptors = new(4500);

    internal static bool TryGetDescriptor(int hashCode, [NotNullWhen(returnValue: true)] out TagHelperDescriptor? descriptor) =>
        s_cachedTagHelperDescriptors.TryGetValue(hashCode, out descriptor);

    internal static void Set(int hashCode, TagHelperDescriptor descriptor) =>
        s_cachedTagHelperDescriptors.Set(hashCode, descriptor);

    internal static int GetTagHelperDescriptorCacheId(TagHelperDescriptor descriptor)
    {
        var hash = HashCodeCombiner.Start();
        hash.Add(descriptor.Kind, StringComparer.Ordinal);
        hash.Add(descriptor.AssemblyName, StringComparer.Ordinal);
        hash.Add(descriptor.Name, StringComparer.Ordinal);
        hash.Add(descriptor.DisplayName, StringComparer.Ordinal);
        hash.Add(descriptor.CaseSensitive ? 1 : 0);

        CalculateForProperty(ref hash, descriptor.BoundAttributes, CalculateBoundAttributeCacheId);
        CalculateForProperty(ref hash, descriptor.TagMatchingRules, CalculateTagMatchingRuleCacheId);
        CalculateForProperty(ref hash, descriptor.AllowedChildTags, CalculateAllowedChildTagsCacheId);
        ComparerUtilities.AddToHash(ref hash, descriptor.Diagnostics ?? Array.Empty<RazorDiagnostic>(), EqualityComparer<RazorDiagnostic>.Default);
        ComparerUtilities.AddToHash(ref hash, descriptor.Metadata, StringComparer.Ordinal, StringComparer.Ordinal);

        return hash.CombinedHash;
    }

    private static void CalculateForProperty<T>(ref HashCodeCombiner hash, IReadOnlyList<T> property, CalculationDelegate<T> calculationDelegate)
    {
        if (property != null)
        {
            for (var i = 0; i < property.Count; i++)
            {
                calculationDelegate(ref hash, property[i]);
            }
        }
    }

    private delegate void CalculationDelegate<T>(ref HashCodeCombiner hash, T? value);

    private static void CalculateBoundAttributeCacheId(ref HashCodeCombiner hash, BoundAttributeDescriptor? descriptor)
    {
        if (descriptor is null)
        {
            return;
        }

        hash.Add(descriptor.Kind, StringComparer.Ordinal);
        hash.Add(descriptor.Name, StringComparer.Ordinal);
        hash.Add(descriptor.IsEditorRequired);
        hash.Add(descriptor.TypeName, comparer: StringComparer.Ordinal);
        hash.Add(descriptor.Documentation, StringComparer.Ordinal);

        CalculateForProperty(ref hash, descriptor.BoundAttributeParameters, CalculateBoundAttributeParameterCacheId);
        ComparerUtilities.AddToHash(ref hash, descriptor.Metadata, StringComparer.Ordinal, StringComparer.Ordinal);
    }

    private static void CalculateBoundAttributeParameterCacheId(ref HashCodeCombiner hash, BoundAttributeParameterDescriptor? descriptor)
    {
        if (descriptor is null)
        {
            return;
        }

        hash.Add(descriptor.Kind, StringComparer.Ordinal);
        hash.Add(descriptor.Name, StringComparer.Ordinal);
        hash.Add(descriptor.TypeName, StringComparer.Ordinal);
        hash.Add(descriptor.Metadata?.Count);
    }

    private static void CalculateTagMatchingRuleCacheId(ref HashCodeCombiner hash, TagMatchingRuleDescriptor? rule)
    {
        if (rule is null)
        {
            return;
        }

        hash.Add(rule.TagName, StringComparer.Ordinal);
        hash.Add(rule.ParentTag, StringComparer.Ordinal);

        CalculateForProperty(ref hash, rule.Attributes, CalculateRequiredAttributeDescriptorCacheId);
    }

    private static void CalculateRequiredAttributeDescriptorCacheId(ref HashCodeCombiner combiner, RequiredAttributeDescriptor? descriptor)
    {
        if (descriptor is null)
        {
            return;
        }

        var hash = HashCodeCombiner.Start();
        hash.Add(descriptor.Name, StringComparer.Ordinal);
        hash.Add(descriptor.Value, StringComparer.Ordinal);
    }

    private static void CalculateAllowedChildTagsCacheId(ref HashCodeCombiner hash, AllowedChildTagDescriptor? descriptor)
    {
        if (descriptor is null)
        {
            return;
        }

        hash.Add(descriptor.Name, StringComparer.Ordinal);
    }
}

