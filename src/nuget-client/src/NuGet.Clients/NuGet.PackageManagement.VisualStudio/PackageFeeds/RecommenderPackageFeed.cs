// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DataAI.NuGetRecommender.Contracts;
using Microsoft.VisualStudio.Shell;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using AsyncLazyNuGetRecommender = Microsoft.VisualStudio.Threading.AsyncLazy<Microsoft.DataAI.NuGetRecommender.Contracts.IVsNuGetPackageRecommender>;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Represents a package feed which recommends packages based on currently loaded project info
    /// </summary>
    public class RecommenderPackageFeed : IPackageFeed
    {
        public bool IsMultiSource => false;

        private readonly SourceRepository _sourceRepository;
        private readonly List<string> _installedPackages;
        private readonly List<string> _transitivePackages;
        private readonly IReadOnlyCollection<string> _targetFrameworks;
        private readonly IPackageMetadataProvider _metadataProvider;
        private readonly Common.ILogger _logger;

        public (string modelVersion, string vsixVersion) VersionInfo { get; set; } = (modelVersion: (string)null, vsixVersion: (string)null);

        private readonly AsyncLazyNuGetRecommender _nuGetRecommender;

        private IVsNuGetPackageRecommender NuGetRecommender { get; set; }

        private const int MaxRecommended = 5;

        public RecommenderPackageFeed(
            IEnumerable<SourceRepository> sourceRepositories,
            PackageCollection installedPackages,
            PackageCollection transitivePackages,
            IReadOnlyCollection<string> targetFrameworks,
            IPackageMetadataProvider metadataProvider,
            Common.ILogger logger)
        {
            if (sourceRepositories == null)
            {
                throw new ArgumentNullException(nameof(sourceRepositories));
            }
            if (installedPackages is null)
            {
                throw new ArgumentNullException(nameof(installedPackages));
            }
            if (transitivePackages is null)
            {
                throw new ArgumentNullException(nameof(transitivePackages));
            }
            _targetFrameworks = targetFrameworks ?? throw new ArgumentNullException(nameof(targetFrameworks));
            _metadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // The recommender package feed should only created when one of the sources is nuget.org.
            if (sourceRepositories.Any(item => UriUtility.IsNuGetOrg(item.PackageSource.Source)))
            {
                _sourceRepository = sourceRepositories.First(item => UriUtility.IsNuGetOrg(item.PackageSource.Source));

                _installedPackages = installedPackages.Select(item => item.Id).ToList();
                _transitivePackages = transitivePackages.Select(item => item.Id).ToList();

                _nuGetRecommender = new AsyncLazyNuGetRecommender(
                    async () =>
                    {
                        return await AsyncServiceProvider.GlobalProvider.GetServiceAsync<SVsNuGetRecommenderService, IVsNuGetPackageRecommender>();
                    },
                    NuGetUIThreadHelper.JoinableTaskFactory);
            }
        }

        private class RecommendSearchToken : ContinuationToken
        {
            public string SearchString { get; set; }
            public SearchFilter SearchFilter { get; set; }
        }

        public Task<SearchResult<IPackageSearchMetadata>> SearchAsync(string searchText, SearchFilter searchFilter, CancellationToken cancellationToken)
        {
            var searchToken = new RecommendSearchToken
            {
                SearchString = searchText,
                SearchFilter = searchFilter,
            };

            return RecommendPackagesAsync(searchToken, cancellationToken);
        }

        public async Task<SearchResult<IPackageSearchMetadata>> RecommendPackagesAsync(ContinuationToken continuationToken, CancellationToken cancellationToken)
        {
            var searchToken = continuationToken as RecommendSearchToken;
            if (searchToken is null)
            {
                throw new ArgumentException("Invalid continuation token", nameof(continuationToken));
            }
            // don't make recommendations if the user entered a search string
            if (!string.IsNullOrEmpty(searchToken.SearchString))
            {
                return SearchResult.Empty<IPackageSearchMetadata>();
            }

            // get recommender service and version info
            if (NuGetRecommender is null && _nuGetRecommender != null)
            {
                try
                {
                    NuGetRecommender = await _nuGetRecommender.GetValueAsync(cancellationToken);
                }
                catch (ServiceUnavailableException)
                {
                    // if the recommender service is not available, NuGetRecommender remains null and we show only the default package list
                }
                if (!(NuGetRecommender is null))
                {
                    var VersionDict = NuGetRecommender.GetVersionInfo();
                    VersionInfo = (modelVersion: VersionDict.ContainsKey("Model") ? VersionDict["Model"] : (string)null,
                                    vsixVersion: VersionDict.ContainsKey("Vsix") ? VersionDict["Vsix"] : (string)null);
                }
            }

            List<string> recommendIds = null;
            if (!(NuGetRecommender is null))
            {
                // call the recommender to get package recommendations
                recommendIds = await NuGetRecommender.GetRecommendedPackageIdsAsync(_targetFrameworks, _installedPackages, _transitivePackages, cancellationToken);
            }

            if (recommendIds is null || !recommendIds.Any())
            {
                return SearchResult.Empty<IPackageSearchMetadata>();
            }

            // get PackageIdentity info for the top 5 recommended packages
            int index = 0;
            List<PackageIdentity> recommendPackages = new List<PackageIdentity>();
            MetadataResource _metadataResource = await _sourceRepository.GetResourceAsync<MetadataResource>(cancellationToken);
            PackageMetadataResource _packageMetadataResource = await _sourceRepository.GetResourceAsync<PackageMetadataResource>(cancellationToken);
            while (index < recommendIds.Count && recommendPackages.Count < MaxRecommended)
            {
                Versioning.NuGetVersion ver = await _metadataResource.GetLatestVersion(recommendIds[index], includePrerelease: false, includeUnlisted: false, NullSourceCacheContext.Instance, Common.NullLogger.Instance, cancellationToken);
                if (!(ver is null))
                {
                    var pid = new PackageIdentity(recommendIds[index], ver);
                    recommendPackages.Add(pid);
                }
                index++;
            }
            var packages = recommendPackages.ToArray();

            // get metadata for recommended packages
            var items = await TaskCombinators.ThrottledAsync(
                packages,
                (p, t) => GetPackageMetadataAsync(p, searchToken.SearchFilter.IncludePrerelease, t),
                cancellationToken);

            // The asynchronous execution has randomly returned the packages, so we need to resort
            // based on the original recommendation order.
            var result = SearchResult.FromItems(items.OrderBy(p => Array.IndexOf(packages, p.Identity)).ToArray());

            // Set status to indicate that there are no more items to load
            result.SourceSearchStatus = new Dictionary<string, LoadingStatus>
            {
                { _sourceRepository.PackageSource.Name, LoadingStatus.NoMoreItems }
            };
            result.NextToken = null;

            return result;
        }

        public Task<SearchResult<IPackageSearchMetadata>> ContinueSearchAsync(ContinuationToken continuationToken, CancellationToken cancellationToken)
            => System.Threading.Tasks.Task.FromResult(SearchResult.Empty<IPackageSearchMetadata>());

        public Task<SearchResult<IPackageSearchMetadata>> RefreshSearchAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
            => System.Threading.Tasks.Task.FromResult(SearchResult.Empty<IPackageSearchMetadata>());

        public async Task<IPackageSearchMetadata> GetPackageMetadataAsync(PackageIdentity identity, bool includePrerelease, CancellationToken cancellationToken)
        {
            // first we try and load the metadata from a local package
            var packageMetadata = await _metadataProvider.GetLocalPackageMetadataAsync(identity, includePrerelease, cancellationToken);
            if (packageMetadata is null)
            {
                // and failing that we go to the network
                packageMetadata = await _metadataProvider.GetPackageMetadataAsync(identity, includePrerelease, cancellationToken);
            }
            return packageMetadata;
        }

    }
}
