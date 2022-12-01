// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.Language;

internal sealed class TagHelperDescriptorComparer : IEqualityComparer<TagHelperDescriptor?>
{
    /// <summary>
    /// A default instance of the <see cref="TagHelperDescriptorComparer"/>.
    /// </summary>
    public static readonly TagHelperDescriptorComparer Default = new TagHelperDescriptorComparer();

    private TagHelperDescriptorComparer()
    {
    }

    public bool Equals(TagHelperDescriptor? descriptorX, TagHelperDescriptor? descriptorY)
    {
        if (object.ReferenceEquals(descriptorX, descriptorY))
        {
            return true;
        }

        if (descriptorX is null || descriptorY is null)
        {
            return false;
        }

        if (!string.Equals(descriptorX.Kind, descriptorY.Kind, StringComparison.Ordinal))
        {
            return false;
        }

        if (!string.Equals(descriptorX.AssemblyName, descriptorY.AssemblyName, StringComparison.Ordinal))
        {
            return false;
        }

        if (!string.Equals(descriptorX.Name, descriptorY.Name, StringComparison.Ordinal))
        {
            return false;
        }

        if (!ComparerUtilities.Equals(
            descriptorX.BoundAttributes,
            descriptorY.BoundAttributes,
            BoundAttributeDescriptorComparer.Default))
        {
            return false;
        }

        if (!ComparerUtilities.Equals(
            descriptorX.TagMatchingRules,
            descriptorY.TagMatchingRules,
            TagMatchingRuleDescriptorComparer.Default))
        {
            return false;
        }

        if (!ComparerUtilities.Equals(
            descriptorX.AllowedChildTags,
            descriptorY.AllowedChildTags,
            AllowedChildTagDescriptorComparer.Default))
        {
            return false;
        }

        if (descriptorX.CaseSensitive != descriptorY.CaseSensitive)
        {
            return false;
        }

        if (!string.Equals(descriptorX.Documentation, descriptorY.Documentation, StringComparison.Ordinal))
        {
            return false;
        }

        if (!string.Equals(descriptorX.DisplayName, descriptorY.DisplayName, StringComparison.Ordinal))
        {
            return false;
        }

        if (!string.Equals(descriptorX.TagOutputHint, descriptorY.TagOutputHint, StringComparison.Ordinal))
        {
            return false;
        }

        if (!ComparerUtilities.Equals(descriptorX.Diagnostics, descriptorY.Diagnostics, EqualityComparer<RazorDiagnostic>.Default))
        {
            return false;
        }

        if (!ComparerUtilities.Equals(descriptorX.Metadata, descriptorY.Metadata, StringComparer.Ordinal, StringComparer.Ordinal))
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc />
    public int GetHashCode(TagHelperDescriptor? descriptor)
    {
        if (descriptor == null)
        {
            return 0;
        }

        var hash = HashCodeCombiner.Start();
        hash.Add(descriptor.Kind, StringComparer.Ordinal);
        hash.Add(descriptor.AssemblyName, StringComparer.Ordinal);
        hash.Add(descriptor.Name, StringComparer.Ordinal);

        return hash.CombinedHash;
    }
}
