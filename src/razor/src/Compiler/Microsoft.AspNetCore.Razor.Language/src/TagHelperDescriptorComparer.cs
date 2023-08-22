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
    public static readonly TagHelperDescriptorComparer Default = new();

    private TagHelperDescriptorComparer()
    {
    }

    public bool Equals(TagHelperDescriptor? descriptorX, TagHelperDescriptor? descriptorY)
    {
        if (ReferenceEquals(descriptorX, descriptorY))
        {
            return true;
        }

        if (descriptorX is null || descriptorY is null)
        {
            return false;
        }

        if (descriptorX.Kind != descriptorY.Kind ||
            descriptorX.AssemblyName != descriptorY.AssemblyName ||
            descriptorX.Name != descriptorY.Name ||
            descriptorX.CaseSensitive != descriptorY.CaseSensitive ||
            descriptorX.DisplayName != descriptorY.DisplayName ||
            descriptorX.DocumentationObject != descriptorY.DocumentationObject ||
            descriptorX.TagOutputHint != descriptorY.TagOutputHint)
        {
            return false;
        }

        if (!ComparerUtilities.Equals(descriptorX.BoundAttributes, descriptorY.BoundAttributes, BoundAttributeDescriptorComparer.Default) ||
            !ComparerUtilities.Equals(descriptorX.TagMatchingRules, descriptorY.TagMatchingRules, TagMatchingRuleDescriptorComparer.Default) ||
            !ComparerUtilities.Equals(descriptorX.AllowedChildTags, descriptorY.AllowedChildTags, AllowedChildTagDescriptorComparer.Default) ||
            !ComparerUtilities.Equals(descriptorX.Diagnostics, descriptorY.Diagnostics, EqualityComparer<RazorDiagnostic>.Default))
        {
            return false;
        }

        // FAST PATH: If each descriptor has a MetadataCollection, we should use their equality.
        if (descriptorX.Metadata is MetadataCollection metadataX &&
            descriptorY.Metadata is MetadataCollection metadataY)
        {
            return metadataX.Equals(metadataY);
        }

        return ComparerUtilities.Equals(descriptorX.Metadata, descriptorY.Metadata, StringComparer.Ordinal);
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
