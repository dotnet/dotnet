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
    public static readonly BoundAttributeParameterDescriptorComparer Default = new BoundAttributeParameterDescriptorComparer();

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

        return
            string.Equals(descriptorX.Kind, descriptorY.Kind, StringComparison.Ordinal) &&
            descriptorX.IsEnum == descriptorY.IsEnum &&
            string.Equals(descriptorX.Name, descriptorY.Name, StringComparison.Ordinal) &&
            string.Equals(descriptorX.TypeName, descriptorY.TypeName, StringComparison.Ordinal) &&
            string.Equals(descriptorX.Documentation, descriptorY.Documentation, StringComparison.Ordinal) &&
            string.Equals(descriptorX.DisplayName, descriptorY.DisplayName, StringComparison.Ordinal) &&
            ComparerUtilities.Equals(descriptorX.Metadata, descriptorY.Metadata, StringComparer.Ordinal, StringComparer.Ordinal);
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
