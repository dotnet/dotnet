// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Utilities;

internal sealed class BoundAttributeChecksumComparer : IEqualityComparer<BoundAttributeDescriptor>
{
    public static readonly BoundAttributeChecksumComparer Instance = new();

    private BoundAttributeChecksumComparer()
    {
    }

    public bool Equals(BoundAttributeDescriptor? x, BoundAttributeDescriptor? y)
        => EqualityComparer<Checksum?>.Default.Equals(x?.GetChecksum(), y?.GetChecksum());

    public int GetHashCode(BoundAttributeDescriptor obj)
        => obj.GetChecksum().GetHashCode();
}
