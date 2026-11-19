// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using NuGet.Packaging.PackageCreation.Resources;
using NuGet.Versioning;

namespace NuGet.Packaging.Core
{
    /// <summary>
    /// Represents the core identity of a nupkg.
    /// </summary>
    public class PackageIdentity : IEquatable<PackageIdentity>, IComparable<PackageIdentity>
    {
        private readonly NuGetVersion? _version;
        private const string ToStringFormat = "{0}.{1}";

        /// <summary>
        /// Creates a new package identity.
        /// </summary>
        /// <param name="id">name</param>
        /// <param name="version">version</param>
        public PackageIdentity(string id, NuGetVersion? version)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Id = id;
            _version = version;
        }

        /// <summary>
        /// Package name
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Package Version
        /// </summary>
        /// <remarks>can be null</remarks>
        /// <remarks>NU_NULL_INC: This property is annotated as not nullable intentionally.
        /// The null is used in very few scenarios and practically illegal in cases dealing with an actual package identity. </remarks>
        public NuGetVersion Version
        {
            get { return _version!; }
        }

        /// <summary>
        /// True if the version is non-null
        /// </summary>
        public bool HasVersion
        {
            get { return _version != null; }
        }

        /// <summary>
        /// Parses a package ID optionally followed by '@' and an exact NuGet version.
        /// </summary>
        /// <param name="value">A package ID, such as "Example", or a version-qualified ID, such as "Example@1.2.3".</param>
        /// <returns>The package identity. <see cref="HasVersion"/> is false when no version is specified.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException">The package ID or version is invalid, or the version after '@' is empty.</exception>
        public static PackageIdentity Parse(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            GetParts(value, out string id, out string? versionString);
            if (!PackageIdValidator.IsValidPackageId(id))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, NuGetResources.InvalidPackageId, id));
            }
            NuGetVersion? version = null;
            if (versionString != null)
            {
                try
                {
                    version = NuGetVersion.Parse(versionString);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException(
                        string.Format(CultureInfo.CurrentCulture, Strings.ErrorInvalidPackageVersion, id, versionString),
                        nameof(value),
                        ex);
                }
            }

            return new PackageIdentity(id, version);
        }

        /// <summary>
        /// Tries to parse a package ID optionally followed by '@' and an exact NuGet version.
        /// </summary>
        /// <param name="value">A package ID, such as "Example", or a version-qualified ID, such as "Example@1.2.3".</param>
        /// <param name="identity">The parsed identity on success, or null on failure.
        /// <see cref="HasVersion"/> is false when no version is specified.</param>
        /// <returns>True if the package ID and optional version are valid; otherwise, false.</returns>
        public static bool TryParse(string? value, [NotNullWhen(true)] out PackageIdentity? identity)
        {
            identity = null;
            if (value == null)
            {
                return false;
            }

            GetParts(value, out string id, out string? versionString);
            if (!PackageIdValidator.IsValidPackageId(id))
            {
                return false;
            }

            if (versionString == null)
            {
                identity = new PackageIdentity(id, version: null);
                return true;
            }

            if (!NuGetVersion.TryParse(versionString, out NuGetVersion? version))
            {
                return false;
            }

            identity = new PackageIdentity(id, version);
            return true;
        }

        private static void GetParts(string value, out string id, out string? version)
        {
            var separator = value.IndexOf('@');
            id = separator < 0 ? value : value.Substring(0, separator);
            version = separator < 0 ? null : value.Substring(separator + 1);
        }

        /// <summary>
        /// True if the package identities are the same.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(PackageIdentity? other)
        {
            return Comparer.Equals(this, other);
        }

        /// <summary>
        /// True if the identity objects are equal based on the given comparison mode.
        /// </summary>
        public virtual bool Equals(PackageIdentity? other, VersionComparison versionComparison)
        {
            var comparer = PackageIdentityComparer.Get(versionComparison);

            return comparer.Equals(this, other);
        }

        /// <summary>
        /// Sorts based on the id, then version
        /// </summary>
        public int CompareTo(PackageIdentity? other)
        {
            return Comparer.Compare(this, other);
        }

        /// <summary>
        /// Compare using the default comparer.
        /// </summary>
        public override bool Equals(object? obj)
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
                ? String.Format(CultureInfo.InvariantCulture, ToStringFormat, Id, Version!.ToNormalizedString())
                : Id;
        }
    }
}
