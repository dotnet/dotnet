// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public interface INuGetSearchService : IDisposable
    {
        ValueTask<SearchResultContextInfo> RefreshSearchAsync(CancellationToken cancellationToken);

        ValueTask<SearchResultContextInfo> ContinueSearchAsync(CancellationToken cancellationToken);

        ValueTask<SearchResultContextInfo> SearchAsync(
            IReadOnlyCollection<IProjectContextInfo> projectContextInfos,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            IReadOnlyCollection<string> targetFrameworks,
            string searchText,
            SearchFilter searchFilter,
            ItemFilter itemFilter,
            bool isSolution,
            bool useRecommender,
            CancellationToken cancellationToken);

        ValueTask<IReadOnlyCollection<PackageSearchMetadataContextInfo>> GetAllPackagesAsync(
            IReadOnlyCollection<IProjectContextInfo> projectContextInfos,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            IReadOnlyCollection<string> targetFrameworks,
            SearchFilter searchFilter,
            ItemFilter itemFilter,
            bool isSolution,
            CancellationToken cancellationToken);

        ValueTask<int> GetTotalCountAsync(
            int maxCount,
            IReadOnlyCollection<IProjectContextInfo> projectContextInfos,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            IReadOnlyCollection<string> targetFrameworks,
            SearchFilter searchFilter,
            ItemFilter itemFilter,
            bool isSolution,
            CancellationToken cancellationToken);

        ValueTask<IReadOnlyCollection<VersionInfoContextInfo>> GetPackageVersionsAsync(
            PackageIdentity identity,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            bool includePrerelease,
            CancellationToken cancellationToken);

        ValueTask<IReadOnlyCollection<VersionInfoContextInfo>> GetPackageVersionsAsync(
            PackageIdentity identity,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            bool includePrerelease,
            bool isTransitive,
            IEnumerable<IProjectContextInfo> projects,
            CancellationToken cancellationToken);

        ValueTask<IReadOnlyCollection<VersionInfoContextInfo>> GetPackageVersionsAsync(
            PackageIdentity identity,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            bool includePrerelease,
            bool isTransitive,
            CancellationToken cancellationToken);

        ValueTask<IReadOnlyCollection<PackageSearchMetadataContextInfo>> GetPackageMetadataListAsync(
            string id,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            bool includePrerelease,
            bool includeUnlisted,
            CancellationToken cancellationToken);

        ValueTask<(PackageSearchMetadataContextInfo, PackageDeprecationMetadataContextInfo?)> GetPackageMetadataAsync(
            PackageIdentity identity,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            bool includePrerelease,
            CancellationToken cancellationToken);

        Task<IReadOnlyList<SourceRepository>> GetAllPackageFoldersAsync(
            IReadOnlyCollection<IProjectContextInfo> projectContextInfos,
            CancellationToken cancellationToken);

        void ClearFromCache(
            string id,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            bool includePrerelease);
    }
}
