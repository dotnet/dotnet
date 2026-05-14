// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.LibraryModel
{
    public sealed class CentralPackageVersion : IEquatable<CentralPackageVersion>
    {
        public string Name { get; }

        public VersionRange VersionRange { get; }

        public CentralPackageVersion(
            string name,
            VersionRange versionRange)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            VersionRange = versionRange ?? throw new ArgumentNullException(nameof(versionRange));
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Name);
            sb.Append(" ");
            sb.Append(VersionRange);
            return sb.ToString();
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCodeCombiner();

            hashCode.AddObject(Name.ToLowerInvariant());
            hashCode.AddObject(VersionRange);

            return hashCode.CombinedHash;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as CentralPackageVersion);
        }

        public bool Equals(CentralPackageVersion? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return
                Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase) &&
                EqualityUtility.EqualsWithNullCheck(VersionRange, other.VersionRange);
        }
    }
}
