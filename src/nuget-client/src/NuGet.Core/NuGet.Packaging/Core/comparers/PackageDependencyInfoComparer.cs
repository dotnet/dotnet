// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Shared;

namespace NuGet.Packaging.Core
{
    public class PackageDependencyInfoComparer : IEqualityComparer<PackageDependencyInfo>
    {
        private readonly IPackageIdentityComparer _identityComparer;
        private readonly PackageDependencyComparer _dependencyComparer;

        public PackageDependencyInfoComparer()
            : this(PackageIdentityComparer.Default, PackageDependencyComparer.Default)
        {
        }

        public PackageDependencyInfoComparer(IPackageIdentityComparer identityComparer, PackageDependencyComparer dependencyComparer)
        {
            if (identityComparer == null)
            {
                throw new ArgumentNullException(nameof(identityComparer));
            }

            if (dependencyComparer == null)
            {
                throw new ArgumentNullException(nameof(dependencyComparer));
            }

            _identityComparer = identityComparer;
            _dependencyComparer = dependencyComparer;
        }

        /// <summary>
        /// Default comparer
        /// </summary>
        public static PackageDependencyInfoComparer Default { get; } = new PackageDependencyInfoComparer();

        public bool Equals(PackageDependencyInfo x, PackageDependencyInfo y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null)
                || ReferenceEquals(y, null))
            {
                return false;
            }

            bool areEqual = _identityComparer.Equals(x, y);

            if (areEqual)
            {
                // counts must match
                areEqual = x.Dependencies.Count() == y.Dependencies.Count();
            }

            if (areEqual)
            {
                var dependencies = new HashSet<PackageDependency>(_dependencyComparer);

                dependencies.UnionWith(x.Dependencies);

                int before = dependencies.Count;

                dependencies.UnionWith(y.Dependencies);

                // verify all dependencies are the same
                areEqual = dependencies.Count == before;
            }

            return areEqual;
        }

        public int GetHashCode(PackageDependencyInfo obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            var combiner = new HashCodeCombiner();

            combiner.AddObject(obj, PackageIdentityComparer.Default);
            combiner.AddUnorderedSequence(obj.Dependencies, _dependencyComparer);

            return combiner.CombinedHash;
        }
    }
}
