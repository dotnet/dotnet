// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Packaging;
using NuGet.Packaging.Core;

namespace NuGet.Protocol.Core.Types
{
    public class FindPackageByIdDependencyInfo
    {
        /// <summary>
        /// Original package identity from the package.
        /// This contains the exact casing for the id and version.
        /// </summary>
        public PackageIdentity PackageIdentity { get; }

        /// <summary>
        /// Gets the package dependency groups.
        /// </summary>
        public IReadOnlyList<PackageDependencyGroup> DependencyGroups { get; }

        /// <summary>
        /// Gets the framework reference groups.
        /// </summary>
        public IReadOnlyList<FrameworkSpecificGroup> FrameworkReferenceGroups { get; }

        /// <summary>
        /// DependencyInfo
        /// </summary>
        /// <param name="packageIdentity">original package identity</param>
        /// <param name="dependencyGroups">package dependency groups</param>
        /// <param name="frameworkReferenceGroups">Sequence of <see cref="FrameworkSpecificGroup" />s.</param>
        public FindPackageByIdDependencyInfo(
            PackageIdentity packageIdentity,
            IEnumerable<PackageDependencyGroup> dependencyGroups,
            IEnumerable<FrameworkSpecificGroup> frameworkReferenceGroups)
        {
            if (dependencyGroups == null)
            {
                throw new ArgumentNullException(nameof(dependencyGroups));
            }

            if (frameworkReferenceGroups == null)
            {
                throw new ArgumentNullException(nameof(frameworkReferenceGroups));
            }

            PackageIdentity = packageIdentity ?? throw new ArgumentNullException(nameof(packageIdentity));
            DependencyGroups = dependencyGroups.ToList();
            FrameworkReferenceGroups = frameworkReferenceGroups.ToList();
        }
    }
}
