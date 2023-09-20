// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Commands
{
    /// <summary>
    /// A source repository dependency provider.
    /// </summary>
    public class SourceRepositoryDependencyProvider : IRemoteDependencyProvider
    {
        private readonly object _lock = new object();
        private readonly SourceRepository _sourceRepository;
        private readonly ILogger _logger;
        private readonly SourceCacheContext _cacheContext;
        private readonly LocalPackageFileCache _packageFileCache;
        private FindPackageByIdResource _findPackagesByIdResource;
        private bool _ignoreFailedSources;
        private bool _ignoreWarning;
        private bool _isFallbackFolderSource;
        private bool _useLegacyAssetTargetFallbackBehavior;

        private readonly ConcurrentDictionary<LibraryRangeCacheKey, AsyncLazy<LibraryDependencyInfo>> _dependencyInfoCache
            = new ConcurrentDictionary<LibraryRangeCacheKey, AsyncLazy<LibraryDependencyInfo>>();

        private readonly ConcurrentDictionary<LibraryRange, AsyncLazy<LibraryIdentity>> _libraryMatchCache
            = new ConcurrentDictionary<LibraryRange, AsyncLazy<LibraryIdentity>>();

        // Limiting concurrent requests to limit the amount of files open at a time.
        private readonly static SemaphoreSlim _throttle = GetThrottleSemaphoreSlim(EnvironmentVariableWrapper.Instance);
        internal static SemaphoreSlim GetThrottleSemaphoreSlim(IEnvironmentVariableReader env)
        {
            // Determine default concurrency limit based on operating system constraints.
            int concurrencyLimit = 0;
            if (RuntimeEnvironmentHelper.IsMacOSX)
            {
                // Limit concurrent requests on Mac OSX to limit the amount of files
                // open at a time, since the default limit is 256.
                concurrencyLimit = 16;
            }
            // Allow user to override concurrency limit via environment variable.
            var variableValue = env.GetEnvironmentVariable("NUGET_CONCURRENCY_LIMIT");
            if (!string.IsNullOrEmpty(variableValue))
            {
                if (int.TryParse(variableValue, out int parsedValue))
                {
                    concurrencyLimit = parsedValue;
                }
            }
            // Construct throttle semaphore if requested.
            return concurrencyLimit > 0
                ? new SemaphoreSlim(concurrencyLimit, concurrencyLimit)
                : null;
        }

        /// <summary>
        /// Initializes a new <see cref="SourceRepositoryDependencyProvider" /> class.
        /// </summary>
        /// <param name="sourceRepository">A source repository.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="ignoreFailedSources"><c>true</c> to ignore failed sources; otherwise <c>false</c>.</param>
        /// <param name="ignoreWarning"><c>true</c> to ignore warnings; otherwise <c>false</c>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sourceRepository" />
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" /> is <c>null</c>.</exception>
        public SourceRepositoryDependencyProvider(
            SourceRepository sourceRepository,
            ILogger logger,
            SourceCacheContext cacheContext,
            bool ignoreFailedSources,
            bool ignoreWarning)
            : this(sourceRepository, logger, cacheContext, ignoreFailedSources, ignoreWarning, fileCache: null, isFallbackFolderSource: false)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="SourceRepositoryDependencyProvider" /> class.
        /// </summary>
        /// <param name="sourceRepository">A source repository.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="ignoreFailedSources"><c>true</c> to ignore failed sources; otherwise <c>false</c>.</param>
        /// <param name="ignoreWarning"><c>true</c> to ignore warnings; otherwise <c>false</c>.</param>
        /// <param name="fileCache">Optional nuspec/file cache.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sourceRepository" />
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> is <c>null</c>.</exception>
        public SourceRepositoryDependencyProvider(
            SourceRepository sourceRepository,
            ILogger logger,
            SourceCacheContext cacheContext,
            bool ignoreFailedSources,
            bool ignoreWarning,
            LocalPackageFileCache fileCache,
            bool isFallbackFolderSource) :
            this(sourceRepository,
                logger,
                cacheContext,
                ignoreFailedSources,
                ignoreWarning,
                fileCache,
                isFallbackFolderSource,
                environmentVariableReader: EnvironmentVariableWrapper.Instance)
        {
        }

        internal SourceRepositoryDependencyProvider(
            SourceRepository sourceRepository,
            ILogger logger,
            SourceCacheContext cacheContext,
            bool ignoreFailedSources,
            bool ignoreWarning,
            LocalPackageFileCache fileCache,
            bool isFallbackFolderSource,
            IEnvironmentVariableReader environmentVariableReader)
        {
            _sourceRepository = sourceRepository ?? throw new ArgumentNullException(nameof(sourceRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheContext = cacheContext ?? throw new ArgumentNullException(nameof(cacheContext));
            _ignoreFailedSources = ignoreFailedSources;
            _ignoreWarning = ignoreWarning;
            _packageFileCache = fileCache;
            _isFallbackFolderSource = isFallbackFolderSource;
            _useLegacyAssetTargetFallbackBehavior = MSBuildStringUtility.IsTrue(environmentVariableReader.GetEnvironmentVariable("NUGET_USE_LEGACY_ASSET_TARGET_FALLBACK_DEPENDENCY_RESOLUTION"));
        }

        /// <summary>
        /// Gets a flag indicating whether or not the provider source is HTTP or HTTPS.
        /// </summary>
        public bool IsHttp => _sourceRepository.PackageSource.IsHttp;

        /// <summary>
        /// Gets the package source.
        /// </summary>
        /// <remarks>Optional. This will be <c>null</c> for project providers.</remarks>
        public PackageSource Source => _sourceRepository.PackageSource;

        public SourceRepository SourceRepository => _sourceRepository;

        /// <summary>
        /// Asynchronously discovers all versions of a package from a source and selects the best match.
        /// </summary>
        /// <remarks>This does not download the package.</remarks>
        /// <param name="libraryRange">A library range.</param>
        /// <param name="targetFramework">A target framework.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="LibraryIdentity" />
        /// instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="libraryRange" />
        /// is either <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="targetFramework" />
        /// is either <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" />
        /// is either <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" />
        /// is either <c>null</c> or empty.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public async Task<LibraryIdentity> FindLibraryAsync(
            LibraryRange libraryRange,
            NuGetFramework targetFramework,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            if (libraryRange == null)
            {
                throw new ArgumentNullException(nameof(libraryRange));
            }

            if (targetFramework == null)
            {
                throw new ArgumentNullException(nameof(targetFramework));
            }

            if (cacheContext == null)
            {
                throw new ArgumentNullException(nameof(cacheContext));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            cancellationToken.ThrowIfCancellationRequested();

            AsyncLazy<LibraryIdentity> result = null;

            var action = new AsyncLazy<LibraryIdentity>(async () =>
                await FindLibraryCoreAsync(libraryRange, cacheContext, logger, cancellationToken));

            if (cacheContext.RefreshMemoryCache)
            {
                result = _libraryMatchCache.AddOrUpdate(libraryRange, action, (k, v) => action);
            }
            else
            {
                result = _libraryMatchCache.GetOrAdd(libraryRange, action);
            }

            try
            {
                return await result;
            }
            catch (FatalProtocolException e)
            {
                if (_ignoreFailedSources)
                {
                    await LogWarningAsync(logger, libraryRange.Name, e);
                }
                else
                {
                    await LogErrorAsync(logger, libraryRange.Name, e);
                    throw;
                }
            }
            return null;
        }

        private async Task<LibraryIdentity> FindLibraryCoreAsync(
            LibraryRange libraryRange,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {

            await EnsureResource();

            if (libraryRange.VersionRange?.MinVersion != null && libraryRange.VersionRange.IsMinInclusive && !libraryRange.VersionRange.IsFloating)
            {
                // first check if the exact min version exist then simply return that
                bool versionExists = false;
                try
                {
                    if (_throttle != null)
                    {
                        await _throttle.WaitAsync(cancellationToken);
                    }

                    versionExists = await _findPackagesByIdResource.DoesPackageExistAsync(
                        libraryRange.Name,
                        libraryRange.VersionRange.MinVersion,
                        cacheContext,
                        logger,
                        cancellationToken);
                }
                finally
                {
                    _throttle?.Release();
                }

                if (versionExists)
                {
                    return new LibraryIdentity
                    {
                        Name = libraryRange.Name,
                        Version = libraryRange.VersionRange.MinVersion,
                        Type = LibraryType.Package
                    };
                }
            }

            // Discover all versions from the feed
            var packageVersions = await GetAllVersionsInternalAsync(libraryRange.Name, cacheContext, logger, false, cancellationToken);

            // Select the best match
            var packageVersion = packageVersions?.FindBestMatch(libraryRange.VersionRange, version => version);

            if (packageVersion != null)
            {
                return new LibraryIdentity
                {
                    Name = libraryRange.Name,
                    Version = packageVersion,
                    Type = LibraryType.Package
                };
            }

            return null;
        }

        /// <summary>
        /// Asynchronously gets package dependencies.
        /// </summary>
        /// <param name="libraryIdentity">A library identity.</param>
        /// <param name="targetFramework">A target framework.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="LibraryDependencyInfo" />
        /// instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="libraryIdentity" />
        /// is either <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="targetFramework" />
        /// is either <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" />
        /// is either <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" />
        /// is either <c>null</c> or empty.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public async Task<LibraryDependencyInfo> GetDependenciesAsync(
            LibraryIdentity libraryIdentity,
            NuGetFramework targetFramework,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            if (libraryIdentity == null)
            {
                throw new ArgumentNullException(nameof(libraryIdentity));
            }

            if (targetFramework == null)
            {
                throw new ArgumentNullException(nameof(targetFramework));
            }

            if (cacheContext == null)
            {
                throw new ArgumentNullException(nameof(cacheContext));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            cancellationToken.ThrowIfCancellationRequested();

            AsyncLazy<LibraryDependencyInfo> result = null;

            var action = new AsyncLazy<LibraryDependencyInfo>(async () =>
                await GetDependenciesCoreAsync(libraryIdentity, targetFramework, cacheContext, logger, cancellationToken));

            var key = new LibraryRangeCacheKey(libraryIdentity, targetFramework);

            if (cacheContext.RefreshMemoryCache)
            {
                result = _dependencyInfoCache.AddOrUpdate(key, action, (k, v) => action);
            }
            else
            {
                result = _dependencyInfoCache.GetOrAdd(key, action);
            }

            return await result;
        }

        private async Task<LibraryDependencyInfo> GetDependenciesCoreAsync(
            LibraryIdentity match,
            NuGetFramework targetFramework,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            FindPackageByIdDependencyInfo packageInfo = null;
            try
            {
                await EnsureResource();

                if (_throttle != null)
                {
                    await _throttle.WaitAsync(cancellationToken);
                }

                // Read package info, this will download the package if needed.
                packageInfo = await _findPackagesByIdResource.GetDependencyInfoAsync(
                    match.Name,
                    match.Version,
                    cacheContext,
                    logger,
                    cancellationToken);
            }
            catch (FatalProtocolException e) when (e is not InvalidCacheProtocolException)
            {
                if (_ignoreFailedSources)
                {
                    await LogWarningAsync(logger, match.Name, e);
                }
                else
                {
                    await LogErrorAsync(logger, match.Name, e);
                    throw;
                }
            }
            finally
            {
                _throttle?.Release();
            }

            if (packageInfo == null)
            {
                // Package was not found
                return LibraryDependencyInfo.CreateUnresolved(match, targetFramework);
            }
            else
            {
                // Package found
                var originalIdentity = new LibraryIdentity(
                    packageInfo.PackageIdentity.Id,
                    packageInfo.PackageIdentity.Version,
                    match.Type);

                IEnumerable<LibraryDependency> dependencyGroup = GetDependencies(packageInfo, targetFramework);

                return LibraryDependencyInfo.Create(originalIdentity, targetFramework, dependencies: dependencyGroup);
            }
        }

        /// <summary>
        /// Asynchronously gets a package downloader.
        /// </summary>
        /// <param name="packageIdentity">A package identity.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="IPackageDownloader" />
        /// instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is either <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" />
        /// is either <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" />
        /// is either <c>null</c> or empty.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public async Task<IPackageDownloader> GetPackageDownloaderAsync(
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

            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await EnsureResource();

                if (_throttle != null)
                {
                    await _throttle.WaitAsync(cancellationToken);
                }

                cancellationToken.ThrowIfCancellationRequested();

                var packageDownloader = await _findPackagesByIdResource.GetPackageDownloaderAsync(
                    packageIdentity,
                    cacheContext,
                    logger,
                    cancellationToken);

                packageDownloader.SetThrottle(_throttle);
                packageDownloader.SetExceptionHandler(async exception =>
                {
                    if (exception is FatalProtocolException e)
                    {
                        if (_ignoreFailedSources)
                        {
                            await LogWarningAsync(logger, packageIdentity.Id, e);
                        }
                        else
                        {
                            await LogErrorAsync(logger, packageIdentity.Id, e);
                        }
                        return true;
                    }

                    return false;
                });

                return packageDownloader;
            }
            catch (FatalProtocolException e)
            {
                if (_ignoreFailedSources)
                {
                    await LogWarningAsync(logger, packageIdentity.Id, e);
                }
                else
                {
                    await LogErrorAsync(logger, packageIdentity.Id, e);
                    throw;
                }
            }
            finally
            {
                _throttle?.Release();
            }

            return null;
        }

        private IEnumerable<LibraryDependency> GetDependencies(
            FindPackageByIdDependencyInfo packageInfo,
            NuGetFramework targetFramework)
        {
            if (packageInfo == null)
            {
                return Enumerable.Empty<LibraryDependency>();
            }

            var dependencyGroup = NuGetFrameworkUtility.GetNearest(packageInfo.DependencyGroups,
                targetFramework,
                item => item.TargetFramework);

            if (dependencyGroup == null && DeconstructFallbackFrameworks(targetFramework) is DualCompatibilityFramework dualCompatibilityFramework)
            {
                dependencyGroup = NuGetFrameworkUtility.GetNearest(packageInfo.DependencyGroups, dualCompatibilityFramework.SecondaryFramework, item => item.TargetFramework);
            }

            if (!_useLegacyAssetTargetFallbackBehavior)
            {
                // FrameworkReducer.GetNearest does not consider ATF since it is used for more than just compat

                if (dependencyGroup == null &&
                    targetFramework is AssetTargetFallbackFramework assetTargetFallbackFramework)
                {
                    dependencyGroup = NuGetFrameworkUtility.GetNearest(packageInfo.DependencyGroups,
                        assetTargetFallbackFramework.AsFallbackFramework(),
                        item => item.TargetFramework);
                }
            }

            if (dependencyGroup != null)
            {
                return dependencyGroup.Packages.Select(PackagingUtility.GetLibraryDependencyFromNuspec).ToArray();
            }

            return Enumerable.Empty<LibraryDependency>();
        }

        private static NuGetFramework DeconstructFallbackFrameworks(NuGetFramework nuGetFramework)
        {
            if (nuGetFramework is AssetTargetFallbackFramework assetTargetFallbackFramework)
            {
                return assetTargetFallbackFramework.RootFramework;
            }

            if (nuGetFramework is FallbackFramework fallbackFramework)
            {
                return fallbackFramework;
            }

            return nuGetFramework;
        }

        private async Task EnsureResource()
        {
            if (_findPackagesByIdResource == null)
            {
                var resource = await _sourceRepository.GetResourceAsync<FindPackageByIdResource>();

                lock (_lock)
                {
                    if (_findPackagesByIdResource == null)
                    {
                        AddLocalV3ResourceOptions(resource);

                        _findPackagesByIdResource = resource;
                    }
                }
            }
        }

        private void AddLocalV3ResourceOptions(FindPackageByIdResource resource)
        {
            var localV3 = resource as LocalV3FindPackageByIdResource;
            if (localV3 != null)
            {
                // Link the nuspec cache to the new resource if it exists.
                if (_packageFileCache != null)
                {
                    localV3.PackageFileCache = _packageFileCache;
                }

                localV3.IsFallbackFolder = _isFallbackFolderSource;
            }
        }

        /// <summary>
        /// Asynchronously discover all package versions from a feed.
        /// </summary>
        /// <param name="id">A package ID.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{NuGetVersion}" />.</returns>
        public async Task<IEnumerable<NuGetVersion>> GetAllVersionsAsync(
            string id,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            return await GetAllVersionsInternalAsync(id, cacheContext, logger, catchAndLogExceptions: true, cancellationToken: cancellationToken);
        }

        internal async Task<IEnumerable<NuGetVersion>> GetAllVersionsInternalAsync(
            string id,
            SourceCacheContext cacheContext,
            ILogger logger,
            bool catchAndLogExceptions,
            CancellationToken cancellationToken)
        {
            try
            {
                if (_throttle != null)
                {
                    await _throttle.WaitAsync(cancellationToken);
                }
                if (_findPackagesByIdResource == null)
                {
                    return null;
                }
                return await _findPackagesByIdResource.GetAllVersionsAsync(
                    id,
                    cacheContext,
                    logger,
                    cancellationToken);
            }
            catch (FatalProtocolException e) when (catchAndLogExceptions)
            {
                if (_ignoreFailedSources)
                {
                    await LogWarningAsync(logger, id, e);
                    return null;
                }
                else
                {
                    await LogErrorAsync(logger, id, e);
                    throw;
                }
            }
            finally
            {
                _throttle?.Release();
            }
        }

        private async Task LogWarningAsync(ILogger logger, string id, FatalProtocolException e)
        {
            if (!_ignoreWarning)
            {
                await logger.LogAsync(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1801, e.Message, id));
            }
        }

        private async Task LogErrorAsync(ILogger logger, string id, FatalProtocolException e)
        {
            if (!_ignoreWarning)
            {
                // Sometimes, there's a better root cause for a source failures we log that instead of NU1301.
                // We only do this for errors, and not warnings.
                var unwrappedLogMessage = UnwrapToLogMessage(e);
                if (unwrappedLogMessage != null)
                {
                    await logger.LogAsync(unwrappedLogMessage);
                }
                else
                {
                    await logger.LogAsync(RestoreLogMessage.CreateError(NuGetLogCode.NU1301, e.Message, id));
                }
            }

            static ILogMessage UnwrapToLogMessage(Exception e)
            {
                var currentException = ExceptionUtilities.Unwrap(e);
                while ((currentException is FatalProtocolException || currentException is not ILogMessageException) && currentException != null)
                {
                    currentException = currentException.InnerException;
                }
                var logMessageException = currentException as ILogMessageException;

                return logMessageException?.AsLogMessage();
            }
        }
    }
}
