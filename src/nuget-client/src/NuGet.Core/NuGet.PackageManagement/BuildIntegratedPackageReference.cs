// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Extends PackageReference to include the original LibraryDependency data.
    /// </summary>
    public class BuildIntegratedPackageReference : PackageReference
    {
        /// <summary>
        /// LibraryDependency from the project.
        /// </summary>
        public LibraryDependency Dependency { get; }

        /// <summary>
        /// Create a PackageReference based on a LibraryDependency.
        /// </summary>
        /// <param name="dependency">Full PackageReference metadata.</param>
        public BuildIntegratedPackageReference(LibraryDependency dependency, NuGetFramework projectFramework)
            : base(GetIdentity(dependency),
                  targetFramework: projectFramework,
                  userInstalled: true,
                  developmentDependency: dependency?.SuppressParent == LibraryIncludeFlags.All,
                  requireReinstallation: false,
                  allowedVersions: GetAllowedVersions(dependency))
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }

            Dependency = dependency;
        }

        /// <summary>
        /// Create a PackageReference based on a LibraryDependency.
        /// </summary>
        /// <param name="dependency">Full PackageReference metadata.</param>
        /// <param name="installedVersion">Package installed.</param>
        public BuildIntegratedPackageReference(LibraryDependency dependency, NuGetFramework projectFramework, PackageIdentity installedVersion)
            : base(installedVersion,
                  targetFramework: projectFramework,
                  userInstalled: true,
                  developmentDependency: dependency?.SuppressParent == LibraryIncludeFlags.All,
                  requireReinstallation: false,
                  allowedVersions: GetAllowedVersions(dependency))
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }

            Dependency = dependency;
        }

        /// <summary>
        /// Convert range to a PackageIdentity
        /// </summary>
        private static PackageIdentity GetIdentity(LibraryDependency dependency)
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }

            // MinVersion may not exist for ranges such as ( , 2.0.0];
            var version = dependency.LibraryRange?.VersionRange?.MinVersion ?? new NuGetVersion(0, 0, 0);

            return new PackageIdentity(dependency.Name, version);
        }

        /// <summary>
        /// Get allowed version range.
        /// </summary>
        private static VersionRange GetAllowedVersions(LibraryDependency dependency)
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }

            if (dependency.AutoReferenced)
            {
                var minVersion = GetMinVersion(dependency);
                return new VersionRange(
                    minVersion: minVersion,
                    includeMinVersion: true,
                    maxVersion: minVersion,
                    includeMaxVersion: true);
            }

            // Return null if no range existed
            return dependency.LibraryRange?.VersionRange;
        }

        private static NuGetVersion GetMinVersion(LibraryDependency dependency)
        {
            // MinVersion may not exist for ranges such as ( , 2.0.0];
            return dependency.LibraryRange?.VersionRange?.MinVersion ?? new NuGetVersion(0, 0, 0);
        }
    }
}
