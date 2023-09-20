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
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Events;
using NuGet.Protocol.Plugins;
using NuGet.Versioning;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// A <see cref="FindPackageByIdResource" /> for plugins.
    /// </summary>
    public sealed class PluginFindPackageByIdResource : FindPackageByIdResource
    {
        private readonly ConcurrentDictionary<string, AsyncLazy<SortedDictionary<NuGetVersion, PackageInfo>>> _packageInfoCache =
            new ConcurrentDictionary<string, AsyncLazy<SortedDictionary<NuGetVersion, PackageInfo>>>(StringComparer.OrdinalIgnoreCase);
        private readonly PackageSource _packageSource;
        private readonly IPlugin _plugin;
        private readonly IPluginMulticlientUtilities _utilities;

        private const string ResourceTypeName = nameof(FindPackageByIdResource);
        private const string ThisTypeName = nameof(PluginFindPackageByIdResource);

        /// <summary>
        /// Instantiates a new <see cref="PluginFindPackageByIdResource" /> class.
        /// </summary>
        /// <param name="plugin">A plugin.</param>
        /// <param name="utilities">A plugin multiclient utilities.</param>
        /// <param name="packageSource">A package source.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="plugin" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="utilities" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageSource" />
        /// is <see langword="null" />.</exception>
        public PluginFindPackageByIdResource(
            IPlugin plugin,
            IPluginMulticlientUtilities utilities,
            PackageSource packageSource)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException(nameof(plugin));
            }

            if (utilities == null)
            {
                throw new ArgumentNullException(nameof(utilities));
            }

            if (packageSource == null)
            {
                throw new ArgumentNullException(nameof(packageSource));
            }

            _plugin = plugin;
            _utilities = utilities;
            _packageSource = packageSource;
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
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override Task<bool> CopyNupkgToStreamAsync(
            string id,
            NuGetVersion version,
            Stream destination,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
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

                var packageReader = new PluginPackageReader(_plugin, packageIdentity, _packageSource.Source);
                var packageDependency = new PluginPackageDownloader(_plugin, packageIdentity, packageReader, _packageSource.Source);

                return Task.FromResult<IPackageDownloader>(packageDependency);
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    _packageSource.Source,
                    ResourceTypeName,
                    ThisTypeName,
                    nameof(GetPackageDownloaderAsync),
                    stopwatch.Elapsed));
            }
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

                AddOrUpdateLogger(_plugin, logger);

                await _utilities.DoOncePerPluginLifetimeAsync(
                    MessageMethod.SetLogLevel.ToString(),
                    () => SetLogLevelAsync(logger, cancellationToken),
                    cancellationToken);

                var packageInfos = await EnsurePackagesAsync(id, cacheContext, cancellationToken);

                return packageInfos.Keys;
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    _packageSource.Source,
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

                var packageInfos = await EnsurePackagesAsync(id, cacheContext, cancellationToken);

                PackageInfo packageInfo;

                if (packageInfos.TryGetValue(version, out packageInfo))
                {
                    AddOrUpdateLogger(_plugin, logger);

                    await _utilities.DoOncePerPluginLifetimeAsync(
                        MessageMethod.SetLogLevel.ToString(),
                        () => SetLogLevelAsync(logger, cancellationToken),
                        cancellationToken);

                    var response = await _plugin.Connection.SendRequestAndReceiveResponseAsync<PrefetchPackageRequest, PrefetchPackageResponse>(
                        MessageMethod.PrefetchPackage,
                        new PrefetchPackageRequest(
                            _packageSource.Source,
                            packageInfo.Identity.Id,
                            packageInfo.Identity.Version.ToNormalizedString()),
                        cancellationToken);

                    if (response != null && response.ResponseCode == MessageResponseCode.Success)
                    {
                        using (var packageReader = new PluginPackageReader(_plugin, packageInfo.Identity, _packageSource.Source))
                        {
                            var nuspecReader = await packageReader.GetNuspecReaderAsync(cancellationToken);

                            return GetDependencyInfo(nuspecReader);
                        }
                    }
                }

                return null;
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    _packageSource.Source,
                    ResourceTypeName,
                    ThisTypeName,
                    nameof(GetDependencyInfoAsync),
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

                var packageInfos = await EnsurePackagesAsync(id, cacheContext, cancellationToken);

                return packageInfos.TryGetValue(version, out var packageInfo);
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    _packageSource.Source,
                    ResourceTypeName,
                    ThisTypeName,
                    nameof(DoesPackageExistAsync),
                    stopwatch.Elapsed));
            }
        }

        private async Task<SortedDictionary<NuGetVersion, PackageInfo>> EnsurePackagesAsync(
            string id,
            SourceCacheContext cacheContext,
            CancellationToken cancellationToken)
        {
            AsyncLazy<SortedDictionary<NuGetVersion, PackageInfo>> result = null;

            Func<string, AsyncLazy<SortedDictionary<NuGetVersion, PackageInfo>>> findPackages =
                (keyId) => new AsyncLazy<SortedDictionary<NuGetVersion, PackageInfo>>(
                    () => FindPackagesByIdAsync(
                        keyId,
                        cancellationToken));

            if (cacheContext.RefreshMemoryCache)
            {
                // Update the cache
                result = _packageInfoCache.AddOrUpdate(id, findPackages, (k, v) => findPackages(id));
            }
            else
            {
                // Read the cache if it exists
                result = _packageInfoCache.GetOrAdd(id, findPackages);
            }

            return await result;
        }

        private async Task<SortedDictionary<NuGetVersion, PackageInfo>> FindPackagesByIdAsync(
            string id,
            CancellationToken cancellationToken)
        {
            var uri = _packageSource.Source;
            var request = new GetPackageVersionsRequest(uri, id);

            try
            {
                var response = await _plugin.Connection.SendRequestAndReceiveResponseAsync<GetPackageVersionsRequest, GetPackageVersionsResponse>(
                    MessageMethod.GetPackageVersions,
                    request,
                    cancellationToken);

                if (response != null)
                {
                    switch (response.ResponseCode)
                    {
                        case MessageResponseCode.Success:
                            var versions = response.Versions.Select(v => NuGetVersion.Parse(v));

                            return ParsePackageVersions(response.Versions, id, uri);

                        case MessageResponseCode.Error:
                            throw new PluginException(
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Strings.Plugin_FailureQueryingPackageVersions,
                                    id,
                                    _plugin.FilePath));

                        case MessageResponseCode.NotFound:
                        default:
                            break;
                    }
                }

                return new SortedDictionary<NuGetVersion, PackageInfo>();
            }
            catch (Exception ex)
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.Log_FailedToRetrievePackage,
                    id,
                    uri);

                throw new FatalProtocolException(message, ex);
            }
        }

        private SortedDictionary<NuGetVersion, PackageInfo> ParsePackageVersions(
            IEnumerable<string> versions,
            string id,
            string baseUri)
        {
            var results = new SortedDictionary<NuGetVersion, PackageInfo>();

            foreach (var packageInfo in versions
                .Select(version => CreatePackageInfo(baseUri, id, version))
                .Where(version => version != null))
            {
                if (!results.ContainsKey(packageInfo.Identity.Version))
                {
                    results.Add(packageInfo.Identity.Version, packageInfo);
                }
            }

            return results;
        }

        private PackageInfo CreatePackageInfo(string baseUri, string id, string version)
        {
            var parsedVersion = NuGetVersion.Parse(version);
            var normalizedVersionString = parsedVersion.ToNormalizedString();

            return new PackageInfo
            {
                Identity = new PackageIdentity(id, parsedVersion),
                ContentUri = $"{baseUri}{id.ToLowerInvariant()}/{normalizedVersionString}/{id.ToLowerInvariant()}.{normalizedVersionString}{PackagingCoreConstants.NupkgExtension}",
            };
        }

        private void AddOrUpdateLogger(IPlugin plugin, ILogger logger)
        {
            plugin.Connection.MessageDispatcher.RequestHandlers.AddOrUpdate(
                MessageMethod.Log,
                () => new LogRequestHandler(logger),
                existingHandler =>
                    {
                        ((LogRequestHandler)existingHandler).SetLogger(logger);

                        return existingHandler;
                    });
        }

        private async Task SetLogLevelAsync(ILogger logger, CancellationToken cancellationToken)
        {
            var logLevel = LogRequestHandler.GetLogLevel(logger);

            await _plugin.Connection.SendRequestAndReceiveResponseAsync<SetLogLevelRequest, SetLogLevelResponse>(
                MessageMethod.SetLogLevel,
                new SetLogLevelRequest(logLevel),
                cancellationToken);
        }

        private class PackageInfo
        {
            public PackageIdentity Identity { get; set; }

            public string Path { get; set; }

            public string ContentUri { get; set; }
        }
    }
}
