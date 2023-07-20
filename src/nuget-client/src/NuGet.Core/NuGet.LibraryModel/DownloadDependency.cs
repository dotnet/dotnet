// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.LibraryModel
{
    /// <remarks>
    /// Immutable.
    /// </remarks>
    public sealed class DownloadDependency : IEquatable<DownloadDependency>, IComparable<DownloadDependency>
    {
        public string Name { get; }

        public VersionRange VersionRange { get; }

        public DownloadDependency(
            string name,
            VersionRange versionRange)
        {
            Name = name;
            VersionRange = versionRange;
        }

        public static implicit operator LibraryRange(DownloadDependency library)
        {
            return new LibraryRange
            {
                Name = library.Name,
                TypeConstraint = LibraryDependencyTarget.Package,
                VersionRange = library.VersionRange
            };
        }

        public int CompareTo(DownloadDependency other)
        {

            var compare = string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
            if (compare == 0)
            {

                if (VersionRange == null
                    && other.VersionRange == null)
                {
                    // NOOP;
                }
                else if (VersionRange == null)
                {
                    compare = -1;
                }
                else if (other.VersionRange == null)
                {
                    compare = 1;
                }
                else
                {
                    compare = VersionRange.GetHashCode().CompareTo(other.VersionRange.GetHashCode());
                }
            }
            return compare;
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

        public override bool Equals(object obj)
        {
            return Equals(obj as DownloadDependency);
        }

        public bool Equals(DownloadDependency other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase) &&
                   EqualityUtility.EqualsWithNullCheck(VersionRange, other.VersionRange);
        }

        [Obsolete("This type is immutable, so there is no need or point to clone it.")]
        public DownloadDependency Clone()
        {
            return this;
        }
    }
}
