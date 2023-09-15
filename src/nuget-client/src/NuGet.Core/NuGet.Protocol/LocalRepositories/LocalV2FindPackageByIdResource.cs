// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using NuGet.Common;
using NuGet.Configuration;
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
    public class LocalV2FindPackageByIdResource : FindPackageByIdResource
    {
        private readonly ConcurrentDictionary<string, IReadOnlyList<LocalPackageInfo>> _packageInfoCache
            = new ConcurrentDictionary<string, IReadOnlyList<LocalPackageInfo>>(StringComparer.OrdinalIgnoreCase);

        private readonly string _source;

        private const string ResourceTypeName = nameof(FindPackageByIdResource);
        private const string ThisTypeName = nameof(LocalV2FindPackageByIdResource);

        /// <summary>
        /// Initializes a new <see cref="LocalV2FindPackageByIdResource" /> class.
        /// </summary>
        /// <param name="packageSource">A package source.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageSource" />
        /// is <see langword="null" />.</exception>
        public LocalV2FindPackageByIdResource(PackageSource packageSource)
        {
            if (packageSource == null)
            {
                throw new ArgumentNullException(nameof(packageSource));
            }

            var rootDirInfo = LocalFolderUtility.GetAndVerifyRootDirectory(packageSource.Source);

            _source = rootDirInfo.FullName;
        }

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
        public override Task<IEnumerable<NuGetVersion>> GetAllVersionsAsync(
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

                var infos = GetPackageInfos(id, cacheContext, logger);

                return Task.FromResult(infos.Select(p => p.Identity.Version));
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    _source,
                    ResourceTypeName,
                    ThisTypeName,
                    nameof(GetAllVersionsAsync),
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

                var info = GetPackageInfo(id, version, cacheContext, logger);

                if (info != null)
                {
                    using (var fileStream = File.OpenRead(info.Path))
                    {
                        await fileStream.CopyToAsync(destination, cancellationToken);
                        ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticNupkgCopiedEvent(_source, destination.Length));
                        return true;
                    }
                }

                return false;
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    _source,
                    ResourceTypeName,
                    ThisTypeName,
                    nameof(CopyNupkgToStreamAsync),
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
        public override Task<FindPackageByIdDependencyInfo> GetDependencyInfoAsync(
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

                FindPackageByIdDependencyInfo dependencyInfo = null;
                var info = GetPackageInfo(id, version, cacheContext, logger);
                if (info != null)
                {
                    dependencyInfo = GetDependencyInfo(info.Nuspec);
                }

                return Task.FromResult(dependencyInfo);
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    _source,
                    ResourceTypeName,
                    ThisTypeName,
                    nameof(GetDependencyInfoAsync),
                    stopwatch.Elapsed));
            }
        }

        private LocalPackageInfo GetPackageInfo(
            string id,
            NuGetVersion version,
            SourceCacheContext cacheContext,
            ILogger logger)
        {
            return GetPackageInfos(id, cacheContext, logger)
                .FirstOrDefault(package => package.Identity.Version == version);
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
        public override Task<IPackageDownloader> GetPackageDownloaderAsync(
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

                var packageInfo = GetPackageInfo(packageIdentity.Id, packageIdentity.Version, cacheContext, logger);
                IPackageDownloader packageDownloader = null;

                if (packageInfo != null)
                {
                    packageDownloader = new LocalPackageArchiveDownloader(_source, packageInfo.Path, packageInfo.Identity, logger);
                }

                return Task.FromResult(packageDownloader);
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    _source,
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
        public override Task<bool> DoesPackageExistAsync(
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

                return Task.FromResult(GetPackageInfo(id, version, cacheContext, logger) != null);
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    _source,
                    ResourceTypeName,
                    ThisTypeName,
                    nameof(DoesPackageExistAsync),
                    stopwatch.Elapsed));
            }
        }

        private IReadOnlyList<LocalPackageInfo> GetPackageInfos(
            string id,
            SourceCacheContext cacheContext,
            ILogger logger)
        {
            IReadOnlyList<LocalPackageInfo> results = null;

            Func<string, IReadOnlyList<LocalPackageInfo>> findPackages = (packageId) => GetPackageInfosCore(packageId, logger);

            if (cacheContext.RefreshMemoryCache)
            {
                results = _packageInfoCache.AddOrUpdate(id, findPackages, (k, v) => findPackages(k));
            }
            else
            {
                results = _packageInfoCache.GetOrAdd(id, findPackages);
            }

            return results;
        }

        private IReadOnlyList<LocalPackageInfo> GetPackageInfosCore(string id, ILogger logger)
        {
            var result = new List<LocalPackageInfo>();

            // packages\{packageId}.{version}.nupkg
            var nupkgFiles = LocalFolderUtility.GetNupkgsFromFlatFolder(_source, logger, CancellationToken.None)
                .Where(path => LocalFolderUtility.IsPossiblePackageMatch(path, id));

            foreach (var nupkgInfo in nupkgFiles)
            {
                using (var stream = nupkgInfo.OpenRead())
                using (var packageReader = new PackageArchiveReader(stream))
                {
                    NuspecReader reader;
                    try
                    {
                        reader = new NuspecReader(packageReader.GetNuspec());
                    }
                    catch (XmlException ex)
                    {
                        var message = string.Format(CultureInfo.CurrentCulture, Strings.Protocol_PackageMetadataError, nupkgInfo.Name, _source);

                        throw new FatalProtocolException(message, ex);
                    }
                    catch (PackagingException ex)
                    {
                        var message = string.Format(CultureInfo.CurrentCulture, Strings.Protocol_PackageMetadataError, nupkgInfo.Name, _source);

                        throw new FatalProtocolException(message, ex);
                    }

                    var identity = reader.GetIdentity();

                    if (string.Equals(identity.Id, id, StringComparison.OrdinalIgnoreCase))
                    {
                        var cachePackage = new LocalPackageInfo(
                            identity,
                            nupkgInfo.FullName,
                            nupkgInfo.LastWriteTimeUtc,
                            new Lazy<NuspecReader>(() => reader),
                            useFolder: false
                        );

                        result.Add(cachePackage);
                    }
                }
            }

            return result;
        }
    }
}
