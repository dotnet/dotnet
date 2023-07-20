// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Versioning;

namespace NuGet.Packaging.Core
{
    /// <summary>
    /// Represents the core identity of a nupkg.
    /// </summary>
    public class PackageIdentity : IEquatable<PackageIdentity>, IComparable<PackageIdentity>
    {
        private readonly string _id;
        private readonly NuGetVersion _version;
        private const string ToStringFormat = "{0}.{1}";

        /// <summary>
        /// Creates a new package identity.
        /// </summary>
        /// <param name="id">name</param>
        /// <param name="version">version</param>
        public PackageIdentity(string id, NuGetVersion version)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            _id = id;
            _version = version;
        }

        /// <summary>
        /// Package name
        /// </summary>
        public string Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Package Version
        /// </summary>
        /// <remarks>can be null</remarks>
        public NuGetVersion Version
        {
            get { return _version; }
        }

        /// <summary>
        /// True if the version is non-null
        /// </summary>
        public bool HasVersion
        {
            get { return _version != null; }
        }

        /// <summary>
        /// True if the package identities are the same.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(PackageIdentity other)
        {
            return Comparer.Equals(this, other);
        }

        /// <summary>
        /// True if the identity objects are equal based on the given comparison mode.
        /// </summary>
        public virtual bool Equals(PackageIdentity other, VersionComparison versionComparison)
        {
            var comparer = PackageIdentityComparer.Get(versionComparison);

            return comparer.Equals(this, other);
        }

        /// <summary>
        /// Sorts based on the id, then version
        /// </summary>
        public int CompareTo(PackageIdentity other)
        {
            return Comparer.Compare(this, other);
        }

        /// <summary>
        /// Compare using the default comparer.
        /// </summary>
        public override bool Equals(object obj)
        {
            var identity = obj as PackageIdentity;

            if (identity == null)
            {
                return false;
            }

            return Comparer.Equals(this, identity);
        }

        /// <summary>
        /// Creates a hash code using the default comparer.
        /// </summary>
        public override int GetHashCode()
        {
            return Comparer.GetHashCode(this);
        }

        /// <summary>
        /// An equality comparer that checks the id, version, and version release label.
        /// </summary>
        public static PackageIdentityComparer Comparer
        {
            get { return PackageIdentityComparer.Default; }
        }

        /// <summary>
        /// PackageIdentity.ToString returns packageId.packageVersion"
        /// </summary>
        public override string ToString()
        {
            return HasVersion
                ? String.Format(CultureInfo.InvariantCulture, ToStringFormat, Id, Version.ToNormalizedString())
                : Id;
        }
    }
}
