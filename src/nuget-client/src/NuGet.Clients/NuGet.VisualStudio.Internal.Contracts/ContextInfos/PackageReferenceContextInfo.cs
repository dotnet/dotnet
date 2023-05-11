// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Versioning;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public sealed class PackageReferenceContextInfo : IPackageReferenceContextInfo
    {
        public PackageReferenceContextInfo(PackageIdentity identity, NuGetFramework? framework)
        {
            Assumes.NotNull(identity);
            // framework is null for project.json package references. 

            Identity = identity;
            Framework = framework;
        }

        // For testing only
        internal static PackageReferenceContextInfo Create(PackageIdentity identity, NuGetFramework? framework)
        {
            return new PackageReferenceContextInfo(identity, framework);
        }

        public static PackageReferenceContextInfo Create(PackageReference packageReference)
        {
            Assumes.NotNull(packageReference);

            var packageReferenceContextInfo = new PackageReferenceContextInfo(packageReference.PackageIdentity, packageReference.TargetFramework)
            {
                IsAutoReferenced = (packageReference as BuildIntegratedPackageReference)?.Dependency?.AutoReferenced == true,
                AllowedVersions = packageReference.AllowedVersions,
                IsUserInstalled = packageReference.IsUserInstalled,
                IsDevelopmentDependency = packageReference.IsDevelopmentDependency,
                VersionOverride = (packageReference as BuildIntegratedPackageReference)?.Dependency?.VersionOverride
            };

            return packageReferenceContextInfo;
        }

        public PackageIdentity Identity { get; internal set; }
        public NuGetFramework? Framework { get; internal set; }
        public VersionRange? AllowedVersions { get; internal set; }
        public bool IsAutoReferenced { get; internal set; }
        public bool IsUserInstalled { get; internal set; }
        public bool IsDevelopmentDependency { get; internal set; }
        public VersionRange? VersionOverride { get; internal set; }
    }
}
