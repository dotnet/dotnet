// Updated PackageModelFactory class
using System;
using System.Collections.Generic;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using ContractItemFilter = NuGet.VisualStudio.Internal.Contracts.ItemFilter;

namespace NuGet.PackageManagement.UI.Models.Package
{
    internal class PackageModelFactory
    {
        private readonly INuGetSearchService _searchService;
        private readonly INuGetPackageFileService _packageFileService;
        private readonly IPackageVulnerabilityService _packageVulnerabilityService;
        private readonly bool _includePrerelease;
        private IReadOnlyCollection<PackageSourceContextInfo> _packageSources;

        public PackageModelFactory(INuGetSearchService searchService, INuGetPackageFileService packageFileService, IPackageVulnerabilityService packageVulnerabilityService, bool includePrerelease, IReadOnlyCollection<PackageSourceContextInfo> packageSources)
        {
            _searchService = searchService ?? throw new ArgumentNullException(nameof(_searchService));
            _packageFileService = packageFileService ?? throw new ArgumentNullException(nameof(_packageFileService));
            _packageVulnerabilityService = packageVulnerabilityService ?? throw new ArgumentNullException(nameof(_packageVulnerabilityService));
            _includePrerelease = includePrerelease;
            _packageSources = packageSources ?? throw new ArgumentNullException(nameof(_packageSources));
        }

        public PackageModel Create(PackageSearchMetadataContextInfo metadata, ContractItemFilter itemFilter)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            EmbeddedResourcesCapability embeddedResources = new EmbeddedResourcesCapability(_packageFileService, metadata.Identity!, metadata.ReadmeUrl);
            IVulnerableCapable vulnerableCapability = new VulnerableDatabaseCapability(_packageVulnerabilityService, metadata.Identity!);

            if (metadata.TransitiveOrigins != null)
            {
                return CreateTransitivelyReferencedPackageModel(metadata, vulnerableCapability, embeddedResources);
            }
            else
            {
                if (metadata.PackagePath != null)
                {
                    PackageMetadataRetrievalAdapter packageMetadataRetrievalAdapter = new PackageMetadataRetrievalAdapter(_searchService, metadata.Identity!, _packageSources, _includePrerelease);

                    if (itemFilter.Equals(ContractItemFilter.All))
                    {
                        return CreateLocalPackageModel(metadata, vulnerableCapability, embeddedResources);
                    }

                    IDeprecationCapable deprecationCapable = new DeprecationPackageMetadataCapability(packageMetadataRetrievalAdapter);
                    return CreateDirectlyReferencedPackageModel(metadata, vulnerableCapability, deprecationCapable, embeddedResources);
                }
                else
                {
                    PackageMetadataRetrievalAdapter packageMetadataRetrievalAdapter = new PackageMetadataRetrievalAdapter(_searchService, metadata.Identity!, _packageSources, _includePrerelease);
                    IDeprecationCapable deprecationCapable = new DeprecationPackageMetadataCapability(packageMetadataRetrievalAdapter);

                    if (metadata.IsRecommended)
                    {
                        return CreateRecommendedPackageModel(metadata, vulnerableCapability, deprecationCapable, embeddedResources);
                    }

                    return CreateRemotePackageModel(metadata, vulnerableCapability, deprecationCapable, embeddedResources);
                }
            }
        }

        private static LocalPackageModel CreateLocalPackageModel(PackageSearchMetadataContextInfo metadata, IVulnerableCapable vulnerableCapability, EmbeddedResourcesCapability embeddedResources)
        {
            return new LocalPackageModel(
                metadata.Identity!,
                metadata.PackagePath!,
                embeddedResources,
                metadata.Title,
                metadata.Description,
                metadata.Authors,
                metadata.ProjectUrl,
                metadata.Tags?.Split(','),
                metadata.OwnersList,
                metadata.DependencySets,
                metadata.Summary,
                metadata.Published,
                metadata.LicenseMetadata,
                metadata.LicenseUrl,
                metadata.RequireLicenseAcceptance,
                metadata.IconUrl);
        }

        private static DirectlyReferencedPackageModel CreateDirectlyReferencedPackageModel(PackageSearchMetadataContextInfo metadata, IVulnerableCapable vulnerableCapability, IDeprecationCapable deprecationCapable, EmbeddedResourcesCapability embeddedResources)
        {
            return new DirectlyReferencedPackageModel(
                metadata.Identity!,
                metadata.PackagePath!,
                vulnerableCapability,
                deprecationCapable,
                embeddedResources,
                metadata.Title,
                metadata.Description,
                metadata.Authors,
                metadata.ProjectUrl,
                metadata.Tags?.Split(','),
                metadata.OwnersList,
                metadata.DependencySets,
                metadata.Summary,
                metadata.Published,
                metadata.LicenseMetadata,
                metadata.LicenseUrl,
                metadata.RequireLicenseAcceptance,
                metadata.ReportAbuseUrl?.ToString(),
                metadata.IconUrl);
        }

        private static TransitivelyReferencedPackageModel CreateTransitivelyReferencedPackageModel(PackageSearchMetadataContextInfo metadata, IVulnerableCapable vulnerableDatabaseCapability, EmbeddedResourcesCapability embeddedResources)
        {
            return new TransitivelyReferencedPackageModel(
                metadata.Identity!,
                vulnerableDatabaseCapability,
                embeddedResources,
                metadata.TransitiveOrigins!,
                metadata.Title,
                metadata.Description,
                metadata.Authors,
                metadata.ProjectUrl,
                metadata.Tags?.Split(','),
                metadata.OwnersList,
                metadata.DependencySets,
                metadata.Summary,
                metadata.Published,
                metadata.LicenseMetadata,
                metadata.LicenseUrl,
                metadata.RequireLicenseAcceptance,
                metadata.ReportAbuseUrl?.ToString(),
                metadata.IconUrl);
        }

        private static RecommendedPackageModel CreateRecommendedPackageModel(PackageSearchMetadataContextInfo metadata, IVulnerableCapable vulnerableCapability, IDeprecationCapable deprecationCapable, EmbeddedResourcesCapability embeddedResources)
        {
            var recommenderVersion = metadata.RecommenderVersion ?? throw new ArgumentNullException(nameof(metadata.RecommenderVersion));

            return new RecommendedPackageModel(
                metadata.Identity!,
                vulnerableCapability,
                deprecationCapable,
                embeddedResources,
                recommenderVersion,
                metadata.Title,
                metadata.Description,
                metadata.Authors,
                metadata.ProjectUrl,
                metadata.Tags?.Split(','),
                metadata.OwnersList,
                metadata.DependencySets,
                metadata.Summary,
                metadata.Published,
                metadata.LicenseMetadata,
                metadata.LicenseUrl,
                metadata.RequireLicenseAcceptance,
                metadata.IsListed,
                metadata.PackageDetailsUrl,
                metadata.DownloadCount,
                metadata.ReadmeUrl,
                metadata.IconUrl);
        }

        private static RemotePackageModel CreateRemotePackageModel(PackageSearchMetadataContextInfo metadata, IVulnerableCapable vulnerableCapability, IDeprecationCapable deprecationCapable, EmbeddedResourcesCapability embeddedResources)
        {
            return new RemotePackageModel(
                metadata.Identity!,
                vulnerableCapability,
                deprecationCapable,
                embeddedResources,
                metadata.Title,
                metadata.Description,
                metadata.Authors,
                metadata.ProjectUrl,
                metadata.Tags?.Split(','),
                metadata.OwnersList,
                metadata.DependencySets,
                metadata.Summary,
                metadata.Published,
                metadata.LicenseMetadata,
                metadata.LicenseUrl,
                metadata.RequireLicenseAcceptance,
                metadata.IsListed,
                metadata.PackageDetailsUrl,
                metadata.DownloadCount,
                metadata.ReadmeUrl,
                metadata.IconUrl);
        }
    }
}
