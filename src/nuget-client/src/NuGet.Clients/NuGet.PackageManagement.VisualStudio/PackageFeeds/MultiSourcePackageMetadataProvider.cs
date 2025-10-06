// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Services.Common;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Resources;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Implements a consolidated metadata provider for multiple package sources 
    /// with optional local repository as a fallback metadata source.
    /// </summary>
    public sealed class MultiSourcePackageMetadataProvider : IPackageMetadataProvider, IOwnerDetailsUriService
    {
        private readonly ReadOnlyCollection<SourceRepository> _sourceRepositories;
        private readonly SourceRepository _localRepository;
        private readonly IEnumerable<SourceRepository> _globalLocalRepositories;
        private readonly Common.ILogger _logger;

        public MultiSourcePackageMetadataProvider(
            IEnumerable<SourceRepository> sourceRepositories,
            SourceRepository optionalLocalRepository,
            IEnumerable<SourceRepository> optionalGlobalLocalRepositories,
            Common.ILogger logger)
        {
            if (sourceRepositories == null)
            {
                throw new ArgumentNullException(nameof(sourceRepositories));
            }

            _sourceRepositories = new ReadOnlyCollection<SourceRepository>(sourceRepositories.ToList());

            _localRepository = optionalLocalRepository;

            _globalLocalRepositories = optionalGlobalLocalRepositories;

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _logger = logger;
        }

        public async Task<IPackageSearchMetadata> GetPackageMetadataForIdentityAsync(PackageIdentity identity, CancellationToken cancellationToken)
        {
            List<Task<IPackageSearchMetadata>> tasks = new List<Task<IPackageSearchMetadata>>(capacity: _sourceRepositories.Count);
            foreach (var sourceRepository in _sourceRepositories)
            {
                tasks.Add(GetMetadataTaskSafeAsync(() => sourceRepository.GetPackageMetadataForIdentityAsync(identity, cancellationToken)));
            }

            return await GetPackageMetadataAsync(identity, tasks, cancellationToken);
        }

        public async Task<IPackageSearchMetadata> GetPackageMetadataAsync(PackageIdentity identity, bool includePrerelease, CancellationToken cancellationToken)
        {
            List<Task<IPackageSearchMetadata>> tasks = new List<Task<IPackageSearchMetadata>>(capacity: _sourceRepositories.Count);
            foreach (var sourceRepository in _sourceRepositories)
            {
                tasks.Add(GetMetadataTaskSafeAsync(() => sourceRepository.GetPackageMetadataAsync(identity, includePrerelease, cancellationToken)));
            }

            return await GetPackageMetadataAsync(identity, tasks, cancellationToken);
        }

        private OwnerDetailsUriTemplateResourceV3 _ownerDetailsUriTemplateResource;

        private bool? _supportsKnownOwners;
        public bool SupportsKnownOwners
        {
            get
            {
                if (_supportsKnownOwners.HasValue)
                {
                    return _supportsKnownOwners.Value;
                }

                // Currently, the Owner Details resource is only utilized for a single selected package source.
                if (_sourceRepositories.Count == 1)
                {
                    _ownerDetailsUriTemplateResource = _sourceRepositories[0].GetResource<OwnerDetailsUriTemplateResourceV3>(CancellationToken.None);
                    _supportsKnownOwners = _ownerDetailsUriTemplateResource != null;
                }
                else
                {
                    _supportsKnownOwners = false;
                }

                return _supportsKnownOwners.Value;
            }
        }

        public Uri GetOwnerDetailsUri(string ownerName)
        {
            if (!SupportsKnownOwners)
            {
                return null;
            }

            return _ownerDetailsUriTemplateResource.GetUri(ownerName);
        }


        public async Task<IPackageSearchMetadata> GetLatestPackageMetadataAsync(
            PackageIdentity identity,
            NuGetProject project,
            bool includePrerelease,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            // get all package references for all the projects and cache locally
            var packageReferences = await project.GetInstalledPackagesAsync(cancellationToken);

            // filter package references for current package identity
            var matchedPackageReferences = packageReferences
                .Where(r => StringComparer.OrdinalIgnoreCase.Equals(r.PackageIdentity.Id, identity.Id));

            // Allowed version range for current package across all selected projects
            // Picks the first non-default range
            var allowedVersions = matchedPackageReferences
                .Select(r => r.AllowedVersions)
                .FirstOrDefault(v => v != null) ?? VersionRange.All;

            var tasks = _sourceRepositories
                .Select(r => GetMetadataTaskSafeAsync(() => r.GetLatestPackageMetadataAsync(identity.Id, includePrerelease, cancellationToken, allowedVersions)))
                .ToArray();

            var completed = (await Task.WhenAll(tasks))
                .Where(m => m != null);

            var highest = completed
                .OrderByDescending(e => e.Identity.Version, VersionComparer.VersionRelease)
                .FirstOrDefault();

            return highest?.WithVersions(
                asyncValueFactory: () => MergeVersionsAsync(identity, completed));
        }

        public async Task<IEnumerable<IPackageSearchMetadata>> GetPackageMetadataListAsync(
            string packageId,
            bool includePrerelease,
            bool includeUnlisted,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var tasks = _sourceRepositories
                .Select(r => GetMetadataTaskSafeAsync(() => r.GetPackageMetadataListAsync(packageId, includePrerelease, includeUnlisted, cancellationToken)))
                .ToArray();

            var completed = (await Task.WhenAll(tasks))
                .Where(m => m != null);

            var packages = completed.SelectMany(p => p);

            var uniquePackages = packages
                .GroupBy(
                    m => m.Identity.Version,
                    (v, ms) => ms.First());

            return uniquePackages;
        }

        /// <summary>
        /// Get package metadata from the package folders.
        /// </summary>
        public async Task<IPackageSearchMetadata> GetLocalPackageMetadataAsync(
            PackageIdentity identity,
            bool includePrerelease,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var sources = new List<SourceRepository>();

            if (_localRepository != null)
            {
                sources.Add(_localRepository);
            }

            if (_globalLocalRepositories != null)
            {
                sources.AddRange(_globalLocalRepositories);
            }

            // Take the package from the first source it is found in
            foreach (var source in sources)
            {
                var result = await source.GetPackageMetadataFromLocalSourceAsync(identity, cancellationToken);

                if (result != null)
                {
                    var versionsAndMetadataTask = FetchAndMergeVersionsAndMetadataAsync(identity, includePrerelease, cancellationToken);

                    return PackageSearchMetadataBuilder
                        .FromMetadata(result)
                        .WithVersions(AsyncLazy.New(async () => (await versionsAndMetadataTask).versions))
                        .WithDeprecation(AsyncLazy.New(async () => (await versionsAndMetadataTask).deprecationMetadata))
                        .Build();
                }
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<IPackageSearchMetadata> GetOnlyLocalPackageMetadataAsync(
            PackageIdentity identity,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var sources = new List<SourceRepository>();

            if (_localRepository != null)
            {
                sources.Add(_localRepository);
            }

            if (_globalLocalRepositories != null)
            {
                sources.AddRange(_globalLocalRepositories);
            }

            // Take the package from the first source it is found in
            foreach (var source in sources)
            {
                var result = await source.GetPackageMetadataFromLocalSourceAsync(identity, cancellationToken);

                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private async Task<IPackageSearchMetadata> GetPackageMetadataAsync(PackageIdentity identity, List<Task<IPackageSearchMetadata>> tasks, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_localRepository != null)
            {
                tasks.Add(_localRepository.GetPackageMetadataFromLocalSourceAsync(identity, cancellationToken));
            }

            if (_globalLocalRepositories != null)
            {
                _globalLocalRepositories.ForEach(x =>
                    tasks.Add(x.GetPackageMetadataFromLocalSourceAsync(identity, cancellationToken)));
            }

            IEnumerable<IPackageSearchMetadata> completed = (await Task.WhenAll(tasks))
                .Where(m => m != null);

            IPackageSearchMetadata packageSearchMetadataBase = (completed.FirstOrDefault(m => !string.IsNullOrEmpty(m.Summary))
                ?? completed.FirstOrDefault()
                ?? PackageSearchMetadataBuilder.FromIdentity(identity).Build()).WithVersions(() => MergeVersionsAsync(identity, completed));


            var clonedResult = packageSearchMetadataBase as PackageSearchMetadataBuilder.ClonedPackageSearchMetadata;

            if (clonedResult == null)
            {
                return packageSearchMetadataBase;
            }

            if (string.IsNullOrWhiteSpace(clonedResult.ReadmeFileUrl))
            {
                clonedResult.ReadmeFileUrl = completed.Select(m => m.ReadmeFileUrl).FirstOrDefault(u => !string.IsNullOrWhiteSpace(u));
            }

            if (string.IsNullOrWhiteSpace(clonedResult.PackagePath))
            {
                clonedResult.PackagePath = completed.Select(m => (m as PackageSearchMetadataBuilder.ClonedPackageSearchMetadata)?.PackagePath).FirstOrDefault(uri => !string.IsNullOrWhiteSpace(uri));
            }

            return clonedResult;
        }

        private static async Task<IEnumerable<VersionInfo>> MergeVersionsAsync(PackageIdentity identity, IEnumerable<IPackageSearchMetadata> packages)
        {
            var versions = await Task.WhenAll(packages.Select(m => m.GetVersionsAsync()));

            var allVersions = versions
                .SelectMany(v => v)
                .Concat(new[] { new VersionInfo(identity.Version) });

            return allVersions
                .GroupBy(v => v.Version)
                .Select(g => g.OrderBy(v => v.DownloadCount).First())
                .ToArray();
        }

        private static async Task<PackageDeprecationMetadata> MergeDeprecationMetadataAsync(IEnumerable<IPackageSearchMetadata> packages)
        {
            var deprecationMetadatas = await Task.WhenAll(packages.Select(m => m.GetDeprecationMetadataAsync()));
            return deprecationMetadatas.FirstOrDefault(d => d != null);
        }

        private static IEnumerable<PackageVulnerabilityMetadata> MergeVulnerabilityMetadata(IEnumerable<IPackageSearchMetadata> packages)
        {
            var vulnerabilityMetadatas = packages.Select(m => m.Vulnerabilities);
            return vulnerabilityMetadatas.FirstOrDefault(v => v != null && v.Any());
        }

        private async Task<(IEnumerable<VersionInfo> versions,
            PackageDeprecationMetadata deprecationMetadata,
            IEnumerable<PackageVulnerabilityMetadata> vulnerabilityMetadata)> FetchAndMergeVersionsAndMetadataAsync(
            PackageIdentity identity, bool includePrerelease, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var tasks = _sourceRepositories
                .Select(r => GetMetadataTaskSafeAsync(
                    () => r.GetPackageMetadataAsync(identity, includePrerelease, cancellationToken)))
                .ToList();

            if (_localRepository != null)
            {
                tasks.Add(_localRepository.GetPackageMetadataFromLocalSourceAsync(identity, cancellationToken));
            }

            var metadatas = (await Task.WhenAll(tasks))
                .Where(m => m != null);

            return (await MergeVersionsAsync(identity, metadatas),
                await MergeDeprecationMetadataAsync(metadatas),
                MergeVulnerabilityMetadata(metadatas));
        }

        internal async Task<T> GetMetadataTaskSafeAsync<T>(Func<Task<T>> getMetadataTask) where T : class
        {
            try
            {
                return await getMetadataTask();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogError(e);
            }
            return null;
        }

        private void LogError(Exception exception)
        {
            _logger.LogError(ExceptionUtilities.DisplayMessage(exception));
        }
    }
}
