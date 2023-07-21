// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Versioning;

#nullable enable

namespace NuGet.Packaging.Core
{
    /// <summary>
    /// Represents a package identity and the dependencies of a package.
    /// </summary>
    /// <remarks>
    /// This class does not support groups of dependencies, the group will need to be selected before
    /// populating this.
    /// </remarks>
    public class PackageDependencyInfo : PackageIdentity, IEquatable<PackageDependencyInfo>
    {
        /// <summary>
        /// Package dependencies
        /// </summary>
        public IEnumerable<PackageDependency> Dependencies { get; }

        public PackageDependencyInfo(string id, NuGetVersion version)
            : this(id, version, null)
        {
        }

        public PackageDependencyInfo(PackageIdentity identity, IEnumerable<PackageDependency>? dependencies)
            : this(identity.Id, identity.Version, dependencies)
        {
        }

        /// <summary>
        /// Represents a package identity and the dependencies of a package.
        /// </summary>
        /// <param name="id">package name</param>
        /// <param name="version">package version</param>
        /// <param name="dependencies">package dependencies</param>
        public PackageDependencyInfo(string id, NuGetVersion version, IEnumerable<PackageDependency>? dependencies)
            : base(id, version)
        {
            Dependencies = dependencies?.ToArray() ?? Array.Empty<PackageDependency>();
        }

        public bool Equals(PackageDependencyInfo? other)
        {
            return PackageDependencyInfoComparer.Default.Equals(this, other);
        }

        public override bool Equals(object? obj)
        {
            var info = obj as PackageDependencyInfo;

            if (info != null)
            {
                return Equals(info);
            }

            return false;
        }

        /// <summary>
        /// Hash code from the default PackageDependencyInfoComparer
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return PackageDependencyInfoComparer.Default.GetHashCode(this);
        }

        /// <summary>
        /// Example: Id : Dependency1, Dependency2
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Dependencies.Any())
            {
                return string.Format(CultureInfo.InvariantCulture, "{0} : {1}", base.ToString(), string.Join(", ", Dependencies.Select(e => e.ToString()).OrderBy(e => e, StringComparer.OrdinalIgnoreCase)));
            }
            else
            {
                return base.ToString();
            }
        }
    }
}
