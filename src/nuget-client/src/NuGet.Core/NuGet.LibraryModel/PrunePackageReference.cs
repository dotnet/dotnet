// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Text;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.LibraryModel
{
    /// <summary>
    /// Represents a PrunePackageReference item.
    /// </summary>
    public sealed class PrunePackageReference : IEquatable<PrunePackageReference>
    {
        /// <summary>
        /// Package Id to be pruned
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Version ranges to be pruned. Expected to be of (, MaxVersion] style.
        /// </summary>
        public VersionRange VersionRange { get; }

        public PrunePackageReference(
            string name,
            VersionRange versionRange)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            VersionRange = versionRange ?? throw new ArgumentNullException(nameof(versionRange));
        }

        public static PrunePackageReference Create(string name, string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_PrunePackageReferenceMissingVersion, name));
            }

            return new PrunePackageReference(name, new VersionRange(maxVersion: NuGetVersion.Parse(version), includeMaxVersion: true));
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
            return Equals(obj as PrunePackageReference);
        }

        public bool Equals(PrunePackageReference? other)
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
