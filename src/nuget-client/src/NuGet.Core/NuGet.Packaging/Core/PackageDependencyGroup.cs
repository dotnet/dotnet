// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
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

        [JsonConstructor]
        private PackageDependencyGroup(NuGetFramework targetFramework)
        {
            if (targetFramework == null)
            {
                _targetFramework = NuGetFramework.AnyFramework;
            }
            else
            {
                _targetFramework = targetFramework;
            }

            _packages = new List<PackageDependency>();
        }

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
        [JsonProperty(PropertyName = "targetFramework")]
        public NuGetFramework TargetFramework
        {
            get { return _targetFramework; }
        }

        /// <summary>
        /// Package dependencies
        /// </summary>
        [JsonProperty(PropertyName = "dependencies")]
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
            combiner.AddUnorderedSequence(Packages);

            return combiner.CombinedHash;
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "[{0}] ({1})", TargetFramework, String.Join(", ", Packages));
        }
    }
}
