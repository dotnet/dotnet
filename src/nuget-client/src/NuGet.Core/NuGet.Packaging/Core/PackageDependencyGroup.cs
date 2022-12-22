// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Shared;

namespace NuGet.Packaging
{
    /// <summary>
    /// Package dependencies grouped to a target framework.
    /// </summary>
    public class PackageDependencyGroup : IEquatable<PackageDependencyGroup>, IFrameworkSpecific
    {
        private readonly NuGetFramework _targetFramework;
        private readonly IEnumerable<PackageDependency> _packages;

        /// <summary>
        /// Dependency group
        /// </summary>
        /// <param name="targetFramework">target framework</param>
        /// <param name="packages">dependant packages</param>
        public PackageDependencyGroup(NuGetFramework targetFramework, IEnumerable<PackageDependency> packages)
        {
            if (targetFramework == null)
            {
                throw new ArgumentNullException(nameof(targetFramework));
            }

            if (packages == null)
            {
                throw new ArgumentNullException(nameof(packages));
            }

            _targetFramework = targetFramework;
            _packages = packages;
        }

        /// <summary>
        /// Dependency group target framework
        /// </summary>
        public NuGetFramework TargetFramework
        {
            get { return _targetFramework; }
        }

        /// <summary>
        /// Package dependencies
        /// </summary>
        public IEnumerable<PackageDependency> Packages
        {
            get { return _packages; }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PackageDependencyGroup);
        }

        public bool Equals(PackageDependencyGroup other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return EqualityComparer<NuGetFramework>.Default.Equals(TargetFramework, other.TargetFramework)
                && Packages.OrderedEquals(other.Packages, p => p.Id, StringComparer.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(TargetFramework.GetHashCode());

            // order the dependencies by hash code to make this consistent
            foreach (int hash in Packages.Select(p => p.GetHashCode()).OrderBy(h => h))
            {
                combiner.AddObject(hash);
            }

            return combiner.CombinedHash;
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "[{0}] ({1})", TargetFramework, String.Join(", ", Packages));
        }
    }
}
