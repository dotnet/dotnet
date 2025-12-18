// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.PackageManagement.UI.Models.Package;
using NuGet.Packaging.Core;

namespace NuGet.PackageManagement.UI.Test.Models.Package
{
    internal static class PackageModelCreationTestHelper
    {
        public static RemotePackageModel CreateRemotePackageModel(
            PackageIdentity identity,
            IVulnerableCapable vulnerableCapability,
            IDeprecationCapable deprecationCapability,
            IEmbeddedResourcesCapable embeddedResourcesCapability,
            bool isListed = true,
            Uri? packageDetailsUrl = null,
            long? downloadCount = null)
        {
            return new RemotePackageModel(
                identity,
                vulnerableCapability,
                deprecationCapability,
                embeddedResourcesCapability,
                title: null,
                description: null,
                authors: null,
                projectUrl: null,
                tags: null,
                ownersList: null,
                packageDependencyGroups: null,
                summary: null,
                publishedDate: null,
                licenseMetadata: null,
                licenseUrl: null,
                requireLicenseAcceptance: false,
                isListed: isListed,
                packageDetailsUrl: packageDetailsUrl,
                downloadCount: downloadCount,
                readmeUrl: null,
                iconUrl: null);
        }

        public static RecommendedPackageModel CreateRecommendedPackageModel(
            PackageIdentity identity,
            IVulnerableCapable vulnerableCapability,
            IDeprecationCapable deprecationCapability,
            IEmbeddedResourcesCapable embeddedResourcesCapability,
            (string modelVersion, string vsixVersion) recommenderVersion)
        {
            return new RecommendedPackageModel(
                identity,
                vulnerableCapability,
                deprecationCapability,
                embeddedResourcesCapability,
                recommenderVersion,
                title: null,
                description: null,
                authors: null,
                projectUrl: null,
                tags: null,
                ownersList: null,
                packageDependencyGroups: null,
                summary: null,
                publishedDate: null,
                licenseMetadata: null,
                licenseUrl: null,
                requireLicenseAcceptance: false,
                isListed: true,
                packageDetailsUrl: null,
                downloadCount: null,
                readmeUrl: null,
                iconUrl: null);
        }

        public static TransitivelyReferencedPackageModel CreateTransitivelyReferencedPackageModel(
            PackageIdentity identity,
            IVulnerableCapable vulnerableCapability,
            IEmbeddedResourcesCapable embeddedResourcesCapability,
            IReadOnlyCollection<PackageIdentity> transitiveOrigins)
        {
            return new TransitivelyReferencedPackageModel(
                identity,
                vulnerableCapability,
                embeddedResourcesCapability,
                transitiveOrigins,
                title: null,
                description: null,
                authors: null,
                projectUrl: null,
                tags: null,
                ownersList: null,
                packageDependencyGroups: null,
                summary: null,
                publishedDate: null,
                licenseMetadata: null,
                licenseUrl: null,
                requireLicenseAcceptance: false,
                reportAbuseUrl: null,
                iconUrl: null);
        }

        public static LocalPackageModel CreateLocalPackageModel(PackageIdentity packageIdentity, string packagePath, IEmbeddedResourcesCapable embeddedResourceCapable)
        {
            return CreateLocalPackageModel(packageIdentity, packagePath, embeddedResourceCapable, authors: null, ownersList: null, iconUrl: null);
        }

        public static LocalPackageModel CreateLocalPackageModel(
            PackageIdentity packageIdentity,
            string packagePath,
            IEmbeddedResourcesCapable embeddedResourceCapable,
            string? authors,
            IReadOnlyList<string>? ownersList,
            Uri? iconUrl)
        {
            return new LocalPackageModel(
                packageIdentity,
                packagePath,
                embeddedResourceCapable,
                title: null,
                description: null,
                projectUrl: null,
                authors: authors,
                tags: null,
                ownersList: ownersList,
                packageDependencyGroups: null,
                summary: null,
                publishedDate: null,
                licenseMetadata: null,
                licenseUrl: null,
                requireLicenseAcceptance: false,
                iconUrl: iconUrl);
        }

        public static DirectlyReferencedPackageModel CreateDirectlyReferencedPackageModel(
            PackageIdentity identity,
            string packagePath,
            IVulnerableCapable vulnerableCapability,
            IDeprecationCapable deprecationCapability,
            IEmbeddedResourcesCapable embeddedResources,
            string? reportAbuseUrl = null)
        {
            return new DirectlyReferencedPackageModel(
                identity,
                packagePath,
                vulnerableCapability,
                deprecationCapability,
                embeddedResources,
                reportAbuseUrl: reportAbuseUrl,
                title: null,
                description: null,
                authors: null,
                projectUrl: null,
                tags: null,
                ownersList: null,
                packageDependencyGroups: null,
                summary: null,
                publishedDate: null,
                licenseMetadata: null,
                licenseUrl: null,
                requireLicenseAcceptance: false,
                iconUrl: null);
        }
    }
}
