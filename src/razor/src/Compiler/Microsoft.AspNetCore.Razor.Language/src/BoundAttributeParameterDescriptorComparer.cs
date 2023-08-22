// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.Language;

internal sealed class BoundAttributeParameterDescriptorComparer : IEqualityComparer<BoundAttributeParameterDescriptor?>
{
    /// <summary>
    /// A default instance of the <see cref="BoundAttributeParameterDescriptorComparer"/>.
    /// </summary>
    public static readonly BoundAttributeParameterDescriptorComparer Default = new();

    private BoundAttributeParameterDescriptorComparer()
    {
    }

    public bool Equals(BoundAttributeParameterDescriptor? descriptorX, BoundAttributeParameterDescriptor? descriptorY)
    {
        if (ReferenceEquals(descriptorX, descriptorY))
        {
            return true;
        }

        if (descriptorX is null)
        {
            return descriptorY is null;
        }
        else if (descriptorY is null)
        {
            return false;
        }

        if (descriptorX.Kind != descriptorY.Kind ||
            descriptorX.IsEnum != descriptorY.IsEnum ||
            descriptorX.Name != descriptorY.Name ||
            descriptorX.TypeName != descriptorY.TypeName ||
            descriptorX.DocumentationObject != descriptorY.DocumentationObject ||
            descriptorX.DisplayName != descriptorY.DisplayName)
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

    public int GetHashCode(BoundAttributeParameterDescriptor? descriptor)
    {
        if (descriptor == null)
        {
            return 0;
        }

        var hash = HashCodeCombiner.Start();
        hash.Add(descriptor.Kind, StringComparer.Ordinal);
        hash.Add(descriptor.Name, StringComparer.Ordinal);

        return hash.CombinedHash;
    }
}
