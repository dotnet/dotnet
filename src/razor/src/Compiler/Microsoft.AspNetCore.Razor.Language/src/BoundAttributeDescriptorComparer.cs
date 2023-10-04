// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.Language;

internal sealed class BoundAttributeDescriptorComparer : IEqualityComparer<BoundAttributeDescriptor?>
{
    /// <summary>
    /// A default instance of the <see cref="BoundAttributeDescriptorComparer"/>.
    /// </summary>
    public static readonly BoundAttributeDescriptorComparer Default = new();

    private BoundAttributeDescriptorComparer()
    {
    }

    public bool Equals(BoundAttributeDescriptor? descriptorX, BoundAttributeDescriptor? descriptorY)
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
            descriptorX.IsIndexerStringProperty != descriptorY.IsIndexerStringProperty ||
            descriptorX.IsEnum != descriptorY.IsEnum ||
            descriptorX.HasIndexer != descriptorY.HasIndexer ||
            descriptorX.CaseSensitive != descriptorY.CaseSensitive ||
            descriptorX.IsEditorRequired != descriptorY.IsEditorRequired ||
            descriptorX.Name != descriptorY.Name ||
            descriptorX.IndexerNamePrefix != descriptorY.IndexerNamePrefix ||
            descriptorX.TypeName != descriptorY.TypeName ||
            descriptorX.IndexerTypeName != descriptorY.IndexerTypeName ||
            descriptorX.DocumentationObject != descriptorY.DocumentationObject ||
            descriptorX.DisplayName != descriptorY.DisplayName)
        {
            return false;
        }

        if (!ComparerUtilities.Equals(descriptorX.BoundAttributeParameters, descriptorY.BoundAttributeParameters, BoundAttributeParameterDescriptorComparer.Default))
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

    public int GetHashCode(BoundAttributeDescriptor? descriptor)
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
