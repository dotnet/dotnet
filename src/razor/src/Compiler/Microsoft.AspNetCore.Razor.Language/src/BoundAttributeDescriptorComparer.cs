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
    public static readonly BoundAttributeDescriptorComparer Default = new BoundAttributeDescriptorComparer();

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

        return
            string.Equals(descriptorX.Kind, descriptorY.Kind, StringComparison.Ordinal) &&
            descriptorX.IsIndexerStringProperty == descriptorY.IsIndexerStringProperty &&
            descriptorX.IsEnum == descriptorY.IsEnum &&
            descriptorX.HasIndexer == descriptorY.HasIndexer &&
            descriptorX.CaseSensitive == descriptorY.CaseSensitive &&
            descriptorX.IsEditorRequired == descriptorY.IsEditorRequired &&
            string.Equals(descriptorX.Name, descriptorY.Name, StringComparison.Ordinal) &&
            string.Equals(descriptorX.IndexerNamePrefix, descriptorY.IndexerNamePrefix, StringComparison.Ordinal) &&
            string.Equals(descriptorX.TypeName, descriptorY.TypeName, StringComparison.Ordinal) &&
            string.Equals(descriptorX.IndexerTypeName, descriptorY.IndexerTypeName, StringComparison.Ordinal) &&
            string.Equals(descriptorX.Documentation, descriptorY.Documentation, StringComparison.Ordinal) &&
            string.Equals(descriptorX.DisplayName, descriptorY.DisplayName, StringComparison.Ordinal) &&
            ComparerUtilities.Equals(descriptorX.BoundAttributeParameters, descriptorY.BoundAttributeParameters, BoundAttributeParameterDescriptorComparer.Default) &&
            ComparerUtilities.Equals(descriptorX.Metadata, descriptorY.Metadata, StringComparer.Ordinal, StringComparer.Ordinal);
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
