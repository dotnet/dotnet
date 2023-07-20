// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Packaging.Core;

namespace NuGet.PackageManagement
{
    public class PackageReferenceComparer : IEqualityComparer<Packaging.PackageReference>
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public static PackageReferenceComparer Instance { get; } = new();
#pragma warning restore CS0618 // Type or member is obsolete

        [Obsolete("Use singleton PackageReferenceComparer.Instance instead")]
        public PackageReferenceComparer()
        {
        }

        public bool Equals(Packaging.PackageReference x, Packaging.PackageReference y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            return PackageIdentityComparer.Default.Equals(x.PackageIdentity, y.PackageIdentity);
        }

        public int GetHashCode(Packaging.PackageReference obj)
        {
            return PackageIdentityComparer.Default.GetHashCode(obj.PackageIdentity);
        }
    }
}
