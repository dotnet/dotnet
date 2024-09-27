// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.PackageManagement.UI.ViewModels;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;
using ContractItemFilter = NuGet.VisualStudio.Internal.Contracts.ItemFilter;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.UI
{
    internal class PackageItemLoader : IPackageItemLoader, IDisposable
    {
        private readonly PackageLoadContext _context;
        private readonly string _searchText;
        private readonly bool _includePrerelease;
        private readonly IReadOnlyCollection<PackageSourceContextInfo> _packageSources;
        private readonly ContractItemFilter _itemFilter;
        private readonly bool _useRecommender;
        private readonly IPackageVulnerabilityService _packageVulnerabilityService;
        private PackageCollection _installedPackages;
        private IEnumerable<IPackageReferenceContextInfo> _packageReferences;
        private PackageFeedSearchState _state = new PackageFeedSearchState();
        private SearchFilter _searchFilter;
        private INuGetSearchService _searchService;
        public IItemLoaderState State => _state;
        private IServiceBroker _serviceBroker;
        private INuGetPackageFileService _packageFileService;

        public bool IsMultiSource => _packageSources.Count > 1;

        private PackageItemLoader(
            IServiceBroker serviceBroker,
            INuGetSearchService searchService,
            PackageLoadContext context,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            ContractItemFilter itemFilter,
            string searchText,
            bool includePrerelease,
            bool useRecommender,
            IPackageVulnerabilityService vulnerabilityService = default)
        {
            Assumes.NotNull(serviceBroker);
            Assumes.NotNull(context);
            Assumes.NotNullOrEmpty(packageSources);

            _serviceBroker = serviceBroker;
            _searchService = searchService;
            _context = context;
            _searchText = searchText ?? string.Empty;
            _includePrerelease = includePrerelease;
            _packageSources = packageSources;
            _itemFilter = itemFilter;
            _useRecommender = useRecommender;
            _packageVulnerabilityService = vulnerabilityService;
        }

        public static async ValueTask<PackageItemLoader> CreateAsync(
            IServiceBroker serviceBroker,
            INuGetSearchService searchService,
            PackageLoadContext context,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            ContractItemFilter itemFilter,
            string searchText = null,
            bool includePrerelease = true,
            bool useRecommender = false,
            IPackageVulnerabilityService vulnerabilityService = default)
        {
            var itemLoader = new PackageItemLoader(
                serviceBroker,
                searchService,
                context,
                packageSources,
                itemFilter,
                searchText,
                includePrerelease,
                useRecommender,
                vulnerabilityService);

            await itemLoader.InitializeAsync();

            return itemLoader;
        }

        // For unit testing purposes
        internal static async ValueTask<PackageItemLoader> CreateAsync(
            IServiceBroker serviceBroker,
            PackageLoadContext context,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            ContractItemFilter itemFilter,
            INuGetSearchService searchService,
            INuGetPackageFileService packageFileService,
            string searchText = null,
            bool includePrerelease = true,
            bool useRecommender = false,
            IPackageVulnerabilityService vulnerabilityService = default)
        {
            var itemLoader = new PackageItemLoader(
                serviceBroker,
                searchService,
                context,
                packageSources,
                itemFilter,
                searchText,
                includePrerelease,
                useRecommender,
                vulnerabilityService);

            await itemLoader.InitializeAsync(packageFileService);

            return itemLoader;
        }

        private async ValueTask InitializeAsync(INuGetPackageFileService packageFileService = null)
        {
            _searchFilter = new SearchFilter(includePrerelease: _includePrerelease)
            {
                SupportedFrameworks = await _context.GetSupportedFrameworksAsync()
            };

            _packageFileService = packageFileService ?? await GetPackageFileServiceAsync(CancellationToken.None);
            _serviceBroker.AvailabilityChanged += OnAvailabilityChanged;
        }

        private void OnAvailabilityChanged(object sender, BrokeredServicesChangedEventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                _packageFileService?.Dispose();
                _packageFileService = await GetPackageFileServiceAsync(CancellationToken.None);
            }).PostOnFailure(nameof(PackageItemLoader), nameof(OnAvailabilityChanged));
        }

        private async ValueTask<INuGetSearchService> GetSearchServiceAsync(CancellationToken cancellationToken)
        {
#pragma warning disable ISB001 // Dispose of proxies
            INuGetSearchService searchService = await _serviceBroker.GetProxyAsync<INuGetSearchService>(NuGetServices.SearchService, cancellationToken);
#pragma warning restore ISB001 // Dispose of proxies
            Assumes.NotNull(searchService);
            return searchService;
        }

        private async ValueTask<INuGetPackageFileService> GetPackageFileServiceAsync(CancellationToken cancellationToken)
        {
#pragma warning disable ISB001 // Dispose of proxies
            INuGetPackageFileService packageFileService = await _serviceBroker.GetProxyAsync<INuGetPackageFileService>(NuGetServices.PackageFileService, cancellationToken);
#pragma warning restore ISB001 // Dispose of proxies
            Assumes.NotNull(packageFileService);
            return packageFileService;
        }

        public async Task<int> GetTotalCountAsync(int maxCount, CancellationToken cancellationToken)
        {
            // Go off the UI thread to perform non-UI operations
            await TaskScheduler.Default;
            IReadOnlyCollection<string> targetFrameworks = await _context.GetSupportedFrameworksAsync();

            return await _searchService.GetTotalCountAsync(maxCount, _context.Projects, _packageSources, targetFrameworks, _searchFilter, _itemFilter, _context.IsSolution, cancellationToken);
        }

        public async Task<IReadOnlyCollection<PackageSearchMetadataContextInfo>> GetInstalledAndTransitivePackagesAsync(CancellationToken cancellationToken)
        {
            // Go off the UI thread to perform non-UI operations
            await TaskScheduler.Default;

            ActivityCorrelationId.StartNew();
            IReadOnlyCollection<string> targetFrameworks = await _context.GetSupportedFrameworksAsync();

            return await _searchService.GetAllPackagesAsync(_context.Projects, _packageSources, targetFrameworks, _searchFilter, _itemFilter, _context.IsSolution, cancellationToken);
        }

        public async Task LoadNextAsync(IProgress<IItemLoaderState> progress, CancellationToken cancellationToken)
        {
            ActivityCorrelationId.StartNew();

            cancellationToken.ThrowIfCancellationRequested();

            await UpdateStateAndReportAsync(
                new SearchResultContextInfo(Array.Empty<PackageSearchMetadataContextInfo>(),
                    ImmutableDictionary<string, LoadingStatus>.Empty,
                    hasMoreItems: _state.Results?.HasMoreItems ?? false),
                progress,
                cancellationToken);

            SearchResultContextInfo searchResult = await SearchAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            await UpdateStateAndReportAsync(searchResult, progress, cancellationToken);
        }

        public async Task UpdateStateAsync(IProgress<IItemLoaderState> progress, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            SearchResultContextInfo searchResult = await _searchService.RefreshSearchAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            await UpdateStateAndReportAsync(searchResult, progress, cancellationToken);
        }

        public async Task<SearchResultContextInfo> SearchAsync(CancellationToken cancellationToken)
        {
            await TaskScheduler.Default;
            cancellationToken.ThrowIfCancellationRequested();

            if (_state.Results != null && _state.Results.HasMoreItems)
            {
                // only continue search for the search package feed, not the recommender.
                return await _searchService.ContinueSearchAsync(cancellationToken);
            }
            IReadOnlyCollection<string> targetFrameworks = await _context.GetSupportedFrameworksAsync();

            return await _searchService.SearchAsync(_context.Projects, _packageSources, targetFrameworks, _searchText, _searchFilter, _itemFilter, _context.IsSolution, _useRecommender, cancellationToken);
        }

        public async Task UpdateStateAndReportAsync(SearchResultContextInfo searchResult, IProgress<IItemLoaderState> progress, CancellationToken cancellationToken)
        {
            // cache installed packages here for future use
            _installedPackages = await _context.GetInstalledPackagesAsync();

            // fetch package references from all the projects and cache locally
            // for solution view, we'll always show the highest available version
            // but for project view, get the allowed version range and pass it to package item view model to choose the latest version based on that
            if (_packageReferences == null && !_context.IsSolution)
            {
                IEnumerable<Task<IReadOnlyCollection<IPackageReferenceContextInfo>>> tasks = _context.Projects
                    .Select(project => project.GetInstalledPackagesAsync(
                        _context.ServiceBroker,
                        cancellationToken).AsTask());
                _packageReferences = (await Task.WhenAll(tasks)).SelectMany(p => p).Where(p => p != null);
            }

            var state = new PackageFeedSearchState(searchResult);
            _state = state;
            progress?.Report(state);
        }

        public void Reset()
        {
            _state = new PackageFeedSearchState();
        }

        public IEnumerable<PackageItemViewModel> GetCurrent()
        {
            if (_state.ItemsCount == 0)
            {
                return Enumerable.Empty<PackageItemViewModel>();
            }

            var listItemViewModels = new Dictionary<string, PackageItemViewModel>();

            foreach (PackageSearchMetadataContextInfo metadataContextInfo in _state.Results.PackageSearchItems)
            {
                var packageId = metadataContextInfo.Identity.Id;
                var packageVersion = metadataContextInfo.Identity.Version;
                var packageLevel = metadataContextInfo.TransitiveOrigins != null ? PackageLevel.Transitive : PackageLevel.TopLevel;

                if (listItemViewModels.TryGetValue(packageId, out PackageItemViewModel existingListItem))
                {
                    if (packageLevel == PackageLevel.Transitive)
                    {
                        UpdateTransitiveInfo(existingListItem, metadataContextInfo);
                    }

                    existingListItem.UpdateInstalledPackagesVulnerabilities(metadataContextInfo.Identity);
                }
                else
                {
                    VersionRange allowedVersions = VersionRange.All;
                    VersionRange versionOverride = null;

                    // get the allowed version range and pass it to package item view model to choose the latest version based on that
                    if (_packageReferences != null)
                    {
                        IEnumerable<IPackageReferenceContextInfo> matchedPackageReferences = _packageReferences.Where(r => StringComparer.OrdinalIgnoreCase.Equals(r.Identity.Id, metadataContextInfo.Identity.Id));
                        var allowedVersionsRange = new List<VersionRange>();
                        var versionOverrides = new List<VersionRange>();

                        foreach (var reference in matchedPackageReferences)
                        {
                            if (reference.AllowedVersions != null)
                            {
                                allowedVersionsRange.Add(reference.AllowedVersions);
                            }
                            if (reference.VersionOverride != null)
                            {
                                versionOverrides.Add(reference.VersionOverride);
                            }
                        }

                        allowedVersions = allowedVersionsRange.FirstOrDefault() ?? VersionRange.All;
                        versionOverride = versionOverrides.FirstOrDefault();
                    }

                    ImmutableList<KnownOwnerViewModel> knownOwnerViewModels = null;

                    // Only load KnownOwners for the Browse tab and not for any Recommended packages.
                    // Recommended packages won't have KnownOwners metadata as they are not part of the search results.
                    if (_itemFilter == ContractItemFilter.All && !metadataContextInfo.IsRecommended)
                    {
                        knownOwnerViewModels = LoadKnownOwnerViewModels(metadataContextInfo);
                    }

                    var listItem = new PackageItemViewModel(_searchService, _packageVulnerabilityService)
                    {
                        Id = metadataContextInfo.Identity.Id,
                        Version = metadataContextInfo.Identity.Version,
                        IconUrl = metadataContextInfo.IconUrl,
                        Owner = metadataContextInfo.Owners,
                        KnownOwnerViewModels = knownOwnerViewModels,
                        Author = metadataContextInfo.Authors,
                        DownloadCount = metadataContextInfo.DownloadCount,
                        Summary = metadataContextInfo.Summary,
                        AllowedVersions = allowedVersions,
                        VersionOverride = versionOverride,
                        PrefixReserved = metadataContextInfo.PrefixReserved && !IsMultiSource,
                        Recommended = metadataContextInfo.IsRecommended,
                        RecommenderVersion = metadataContextInfo.RecommenderVersion,
                        Vulnerabilities = metadataContextInfo.Vulnerabilities,
                        Sources = _packageSources,
                        PackagePath = metadataContextInfo.PackagePath,
                        PackageFileService = _packageFileService,
                        IncludePrerelease = _includePrerelease,
                        PackageLevel = packageLevel,
                    };

                    if (listItem.PackageLevel == PackageLevel.TopLevel)
                    {
                        listItem.UpdatePackageStatus(_installedPackages);
                    }
                    else
                    {
                        UpdateTransitiveInfo(listItem, metadataContextInfo);
                        listItem.UpdateTransitivePackageStatus(metadataContextInfo.Identity.Version);
                    }

                    listItemViewModels[packageId] = listItem;
                }
            }

            return listItemViewModels.Values.ToArray();
        }

        private static void UpdateTransitiveInfo(PackageItemViewModel listItem, PackageSearchMetadataContextInfo metadataContextInfo)
        {
            listItem.TransitiveInstalledVersions.Add(metadataContextInfo.Identity.Version);
            listItem.TransitiveOrigins.AddRange(metadataContextInfo.TransitiveOrigins);
            listItem.TransitiveToolTipMessage = string.Format(CultureInfo.CurrentCulture, Resources.PackageVersionWithTransitiveOrigins, string.Join(", ", listItem.TransitiveInstalledVersions), string.Join(", ", listItem.TransitiveOrigins));
        }

        private static ImmutableList<KnownOwnerViewModel> LoadKnownOwnerViewModels(PackageSearchMetadataContextInfo metadataContextInfo)
        {
            ImmutableList<KnownOwnerViewModel> knownOwnerViewModels = null;
            if (metadataContextInfo.KnownOwners != null)
            {
                knownOwnerViewModels = metadataContextInfo.KnownOwners.Select(knownOwner => new KnownOwnerViewModel(knownOwner)).ToImmutableList();
            }

            return knownOwnerViewModels;
        }

        public void Dispose()
        {
            _searchService?.Dispose();

            if (_serviceBroker != null)
            {
                _serviceBroker.AvailabilityChanged -= OnAvailabilityChanged;
            }
        }

        private class PackageFeedSearchState : IItemLoaderState
        {
            private readonly SearchResultContextInfo _results;

            public PackageFeedSearchState()
            {
            }

            public PackageFeedSearchState(SearchResultContextInfo results)
            {
                _results = results ?? throw new ArgumentNullException(nameof(results));
            }

            public SearchResultContextInfo Results => _results;

            public Guid? OperationId => _results?.OperationId;

            public LoadingStatus LoadingStatus
            {
                get
                {
                    if (_results == null || SourceLoadingStatus == null || SourceLoadingStatus.Values == null)
                    {
                        // initial status when no load called before
                        return LoadingStatus.Unknown;
                    }

                    return SourceLoadingStatus.Values.Aggregate();
                }
            }

            // returns the "raw" counter which is not the same as _results.Items.Count
            // simply because it correlates to un-merged items
            public int ItemsCount => _results?.PackageSearchItems.Count ?? 0;

            public IReadOnlyDictionary<string, LoadingStatus> SourceLoadingStatus => _results?.SourceLoadingStatus;
        }
    }
}
