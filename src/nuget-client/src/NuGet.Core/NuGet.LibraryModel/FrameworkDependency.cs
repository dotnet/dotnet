// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Common;
using NuGet.Shared;

namespace NuGet.LibraryModel
{
    public sealed class FrameworkDependency : IEquatable<FrameworkDependency>, IComparable<FrameworkDependency>
    {
        public string Name { get; }

        public FrameworkDependencyFlags PrivateAssets { get; }

        public FrameworkDependency(
            string name,
            FrameworkDependencyFlags privateAssets)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            PrivateAssets = privateAssets;
        }

        public int CompareTo(FrameworkDependency? other)
        {
            if (ReferenceEquals(other, null))
            {
                return 1;
            }

            var compare = ComparisonUtility.FrameworkReferenceNameComparer.Compare(Name, other.Name);

            if (compare == 0)
            {
                return PrivateAssets.CompareTo(other.PrivateAssets);
            }

            return compare;
        }

        public bool Equals(FrameworkDependency? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return ComparisonUtility.FrameworkReferenceNameComparer.Equals(Name, other.Name) &&
                   PrivateAssets.Equals(other.PrivateAssets);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCodeCombiner();
            hashCode.AddObject(ComparisonUtility.FrameworkReferenceNameComparer.GetHashCode(Name));
            hashCode.AddStruct(PrivateAssets);
            return hashCode.CombinedHash;
        }
    }
}
