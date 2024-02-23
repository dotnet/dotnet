// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Events;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    /// <summary>
    /// A resource capable of fetching packages, package versions and package dependency information.
    /// </summary>
#pragma warning disable CA10001
    public class RemoteV3FindPackageByIdResource : FindPackageByIdResource
#pragma warning restore CA10001
    {
        private readonly SemaphoreSlim _dependencyInfoSemaphore = new SemaphoreSlim(initialCount: 1);

        private readonly TaskResultCache<string, IEnumerable<RemoteSourceDependencyInfo>> _packageVersionsCache = new(StringComparer.OrdinalIgnoreCase);

        private readonly HttpSource _httpSource;
        private readonly FindPackagesByIdNupkgDownloader _nupkgDownloader;

        private DependencyInfoResource _dependencyInfoResource;

        private const string ResourceTypeName = nameof(FindPackageByIdResource);
        private const string ThisTypeName = nameof(RemoteV3FindPackageByIdResource);

        /// <summary>
        /// Initializes a new <see cref="RemoteV3FindPackageByIdResource" /> class.
        /// </summary>
        /// <param name="sourceRepository">A source repository.</param>
        /// <param name="httpSource">An HTTP source.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sourceRepository" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="httpSource" />
        /// is <see langword="null" />.</exception>
        public RemoteV3FindPackageByIdResource(SourceRepository sourceRepository, HttpSource httpSource)
        {
            if (sourceRepository == null)
            {
                throw new ArgumentNullException(nameof(sourceRepository));
            }

            if (httpSource == null)
            {
                throw new ArgumentNullException(nameof(httpSource));
            }

            SourceRepository = sourceRepository;
            _httpSource = httpSource;
            _nupkgDownloader = new FindPackagesByIdNupkgDownloader(httpSource);
        }

        /// <summary>
        /// Gets the source repository.
        /// </summary>
        public SourceRepository SourceRepository { get; }

        /// <summary>
        /// Asynchronously gets all package versions for a package ID.
        /// </summary>
        /// <param name="id">A package ID.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{NuGetVersion}" />.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="id" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<IEnumerable<NuGetVersion>> GetAllVersionsAsync(
            string id,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(id));
            }

            if (cacheContext == null)
            {
                throw new ArgumentNullException(nameof(cacheContext));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var stopwatch = Stopwatch.StartNew();
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await EnsurePackagesAsync(id, cacheContext, logger, cancellationToken);

                return result.Select(item => item.Identity.Version);
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    SourceRepository.PackageSource.Source,
                    ResourceTypeName,
                    ThisTypeName,
                    nameof(GetAllVersionsAsync),
                    stopwatch.Elapsed));
            }
        }

        /// <summary>
        /// Asynchronously gets dependency information for a specific package.
        /// </summary>
        /// <param name="id">A package id.</param>
        /// <param name="version">A package version.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{NuGetVersion}" />.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="id" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="version" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<FindPackageByIdDependencyInfo> GetDependencyInfoAsync(
            string id,
            NuGetVersion version,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(id));
            }

            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            if (cacheContext == null)
            {
                throw new ArgumentNullException(nameof(cacheContext));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var stopwatch = Stopwatch.StartNew();
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var packageInfo = await GetPackageInfoAsync(id, version, cacheContext, logger, cancellationToken);
                if (packageInfo == null)
                {
                    return null;
                }

                var reader = await _nupkgDownloader.GetNuspecReaderFromNupkgAsync(
                    packageInfo.Identity,
                    packageInfo.ContentUri,
                    cacheContext,
                    logger,
                    cancellationToken);

                return GetDependencyInfo(reader);
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    SourceRepository.PackageSource.Source,
                    ResourceTypeName,
                    ThisTypeName,
                    nameof(GetDependencyInfoAsync),
                    stopwatch.Elapsed));
            }
        }

        /// <summary>
        /// Asynchronously copies a .nupkg to a stream.
        /// </summary>
        /// <param name="id">A package ID.</param>
        /// <param name="version">A package version.</param>
        /// <param name="destination">A destination stream.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="bool" /> indicating whether or not the .nupkg file was copied.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="id" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="version" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="destination" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<bool> CopyNupkgToStreamAsync(
            string id,
            NuGetVersion version,
            Stream destination,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(id));
            }

            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (cacheContext == null)
            {
                throw new ArgumentNullException(nameof(cacheContext));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var stopwatch = Stopwatch.StartNew();
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var packageInfo = await GetPackageInfoAsync(id, version, cacheContext, logger, cancellationToken);
                if (packageInfo == null)
                {
                    return false;
                }

                return await _nupkgDownloader.CopyNupkgToStreamAsync(
                    packageInfo.Identity,
                    packageInfo.ContentUri,
                    destination,
                    cacheContext,
                    logger,
                    cancellationToken);
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    SourceRepository.PackageSource.Source,
                    ResourceTypeName,
                    ThisTypeName,
                    nameof(CopyNupkgToStreamAsync),
                    stopwatch.Elapsed));
            }
        }

        /// <summary>
        /// Asynchronously gets a package downloader for a package identity.
        /// </summary>
        /// <param name="packageIdentity">A package identity.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an <see cref="IPackageDownloader" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<IPackageDownloader> GetPackageDownloaderAsync(
            PackageIdentity packageIdentity,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (cacheContext == null)
            {
                throw new ArgumentNullException(nameof(cacheContext));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var stopwatch = Stopwatch.StartNew();
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var packageInfo = await GetPackageInfoAsync(
                    packageIdentity.Id,
                    packageIdentity.Version,
                    cacheContext,
                    logger,
                    cancellationToken);

                if (packageInfo == null)
                {
                    return null;
                }

                return new RemotePackageArchiveDownloader(SourceRepository.PackageSource.Source, this, packageInfo.Identity, cacheContext, logger);
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    SourceRepository.PackageSource.Source,
                    ResourceTypeName,
                    ThisTypeName,
                    nameof(GetPackageDownloaderAsync),
                    stopwatch.Elapsed));
            }
        }

        /// <summary>
        /// Asynchronously check if exact package (id/version) exists at this source.
        /// </summary>
        /// <param name="id">A package id.</param>
        /// <param name="version">A package version.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{NuGetVersion}" />.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="id" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="version" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<bool> DoesPackageExistAsync(
            string id,
            NuGetVersion version,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(id));
            }

            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            if (cacheContext == null)
            {
                throw new ArgumentNullException(nameof(cacheContext));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var stopwatch = Stopwatch.StartNew();
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var packageInfo = await GetPackageInfoAsync(id, version, cacheContext, logger, cancellationToken);

                return packageInfo != null;
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    SourceRepository.PackageSource.Source,
                    ResourceTypeName,
                    ThisTypeName,
                    nameof(DoesPackageExistAsync),
                    stopwatch.Elapsed));
            }
        }

        private async Task<RemoteSourceDependencyInfo> GetPackageInfoAsync(
            string id,
            NuGetVersion version,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var packageInfos = await EnsurePackagesAsync(id, cacheContext, logger, cancellationToken);
            return packageInfos.FirstOrDefault(p => p.Identity.Version == version);
        }

        private Task<IEnumerable<RemoteSourceDependencyInfo>> EnsurePackagesAsync(
            string id,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            return _packageVersionsCache.GetOrAddAsync(
                id,
                cacheContext.RefreshMemoryCache,
                static state => state.caller.FindPackagesByIdAsyncCore(state.id, state.cacheContext, state.logger, state.cancellationToken),
                (caller: this, id, cacheContext, logger, cancellationToken), cancellationToken);
        }

        private async Task<IEnumerable<RemoteSourceDependencyInfo>> FindPackagesByIdAsyncCore(
            string id,
            SourceCacheContext sourceCacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            // This is invoked from inside a lock.
            await EnsureDependencyProvider(cancellationToken);

            var result = await _dependencyInfoResource.ResolvePackages(id, sourceCacheContext, logger, cancellationToken);

            return result;
        }

        private async Task EnsureDependencyProvider(CancellationToken cancellationToken)
        {
            if (_dependencyInfoResource == null)
            {
                try
                {
                    await _dependencyInfoSemaphore.WaitAsync(cancellationToken);
                    if (_dependencyInfoResource == null)
                    {
                        _dependencyInfoResource = await SourceRepository.GetResourceAsync<DependencyInfoResource>(cancellationToken);
                    }
                }
                finally
                {
                    _dependencyInfoSemaphore.Release();
                }
            }
        }
    }
}
