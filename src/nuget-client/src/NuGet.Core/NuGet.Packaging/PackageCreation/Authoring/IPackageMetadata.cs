// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.Packaging
{
    public interface IPackageMetadata
    {
        string Id { get; }
        NuGetVersion Version { get; }

        string Title { get; }
        IEnumerable<string> Authors { get; }
        IEnumerable<string> Owners { get; }
        Uri IconUrl { get; }
        Uri LicenseUrl { get; }
        Uri ProjectUrl { get; }
        bool RequireLicenseAcceptance { get; }
        bool DevelopmentDependency { get; }
        string Description { get; }
        string Summary { get; }
        string ReleaseNotes { get; }
        string Language { get; }
        string Tags { get; }
        bool Serviceable { get; }
        string Copyright { get; }
        string Icon { get; }
        string Readme { get; }

        /// <summary>
        /// Specifies assemblies from GAC that the package depends on.
        /// </summary>
        IEnumerable<FrameworkAssemblyReference> FrameworkReferences { get; }

        /// <summary>
        /// Returns sets of References specified in the manifest.
        /// </summary>
        IEnumerable<PackageReferenceSet> PackageAssemblyReferences { get; }

        /// <summary>
        /// Specifies sets other packages that the package depends on.
        /// </summary>
        IEnumerable<PackageDependencyGroup> DependencyGroups { get; }

        Version MinClientVersion { get; }

        /// <summary>
        /// Returns sets of Content Files specified in the manifest.
        /// </summary>
        IEnumerable<ManifestContentFiles> ContentFiles { get; }

        IEnumerable<PackageType> PackageTypes { get; }

        RepositoryMetadata Repository { get; }

        LicenseMetadata LicenseMetadata { get; }

        IEnumerable<FrameworkReferenceGroup> FrameworkReferenceGroups { get; }
    }
}
