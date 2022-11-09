// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.Language;

internal sealed class AllowedChildTagDescriptorComparer : IEqualityComparer<AllowedChildTagDescriptor?>
{
    /// <summary>
    /// A default instance of the <see cref="AllowedChildTagDescriptorComparer"/>.
    /// </summary>
    public static readonly AllowedChildTagDescriptorComparer Default =
        new AllowedChildTagDescriptorComparer();

    private AllowedChildTagDescriptorComparer()
    {
    }

    /// <inheritdoc />
    public bool Equals(
        AllowedChildTagDescriptor? descriptorX,
        AllowedChildTagDescriptor? descriptorY)
    {
        if (object.ReferenceEquals(descriptorX, descriptorY))
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
            string.Equals(descriptorX.Name, descriptorY.Name, StringComparison.Ordinal) &&
            string.Equals(descriptorX.DisplayName, descriptorY.DisplayName, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public int GetHashCode(AllowedChildTagDescriptor? descriptor)
    {
        if (descriptor is null)
        {
            return 0;
        }

        var hash = HashCodeCombiner.Start();
        hash.Add(descriptor.Name, StringComparer.Ordinal);

        return hash.CombinedHash;
    }
}
