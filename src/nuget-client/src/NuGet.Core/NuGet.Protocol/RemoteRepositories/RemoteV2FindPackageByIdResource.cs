// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
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
    public class RemoteV2FindPackageByIdResource : FindPackageByIdResource
    {
        private static readonly XName _xnameEntry = XName.Get("entry", "http://www.w3.org/2005/Atom");
        private static readonly XName _xnameContent = XName.Get("content", "http://www.w3.org/2005/Atom");
        private static readonly XName _xnameProperties = XName.Get("properties", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
        private static readonly XName _xnameId = XName.Get("Id", "http://schemas.microsoft.com/ado/2007/08/dataservices");
        private static readonly XName _xnameVersion = XName.Get("Version", "http://schemas.microsoft.com/ado/2007/08/dataservices");

        private readonly string _baseUri;
        private readonly HttpSource _httpSource;

        private readonly TaskResultCache<string, List<PackageInfo>> _packageVersionsCache = new(StringComparer.OrdinalIgnoreCase);
        private readonly FindPackagesByIdNupkgDownloader _nupkgDownloader;
        private readonly V2FeedQueryBuilder _queryBuilder;

        private const string ResourceTypeName = nameof(FindPackageByIdResource);
        private const string ThisTypeName = nameof(RemoteV2FindPackageByIdResource);

        /// <summary>
        /// Initializes a new <see cref="RemoteV2FindPackageByIdResource" /> class.
        /// </summary>
        /// <param name="packageSource">A package source.</param>
        /// <param name="httpSource">An HTTP source.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageSource" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="httpSource" />
        /// is <see langword="null" />.</exception>
        public RemoteV2FindPackageByIdResource(PackageSource packageSource, HttpSource httpSource)
        {
            if (packageSource == null)
            {
                throw new ArgumentNullException(nameof(packageSource));
            }

            if (httpSource == null)
            {
                throw new ArgumentNullException(nameof(httpSource));
            }

            _baseUri = packageSource.Source.EndsWith("/", StringComparison.Ordinal) ? packageSource.Source : (packageSource.Source + "/");
            _httpSource = httpSource;
            _nupkgDownloader = new FindPackagesByIdNupkgDownloader(_httpSource);
            _queryBuilder = new V2FeedQueryBuilder();

            PackageSource = packageSource;
        }

        /// <summary>
        /// Gets the package source.
        /// </summary>
        public PackageSource PackageSource { get; }

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
                    PackageSource.Source,
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
                    logger.LogWarning($"Unable to find package {id}{version}");
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
                    PackageSource.Source,
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
                    PackageSource.Source,
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

                return new RemotePackageArchiveDownloader(PackageSource.Source, this, packageInfo.Identity, cacheContext, logger);
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    PackageSource.Source,
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
                    PackageSource.Source,
                    ResourceTypeName,
                    ThisTypeName,
                    nameof(DoesPackageExistAsync),
                    stopwatch.Elapsed));
            }
        }

        private async Task<PackageInfo> GetPackageInfoAsync(
            string id,
            NuGetVersion version,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var packageInfos = await EnsurePackagesAsync(id, cacheContext, logger, cancellationToken);
            return packageInfos.FirstOrDefault(p => p.Identity.Version == version);
        }

        private async Task<IEnumerable<PackageInfo>> EnsurePackagesAsync(
            string id,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            List<PackageInfo> result = await _packageVersionsCache.GetOrAddAsync(
                id,
                refresh: cacheContext.RefreshMemoryCache,
                static state => state.caller.FindPackagesByIdAsyncCore(state.id, state.cacheContext, state.logger, state.cancellationToken),
                (caller: this, id, cacheContext, logger, cancellationToken), cancellationToken);

            return result;
        }

        private async Task<List<PackageInfo>> FindPackagesByIdAsyncCore(
            string id,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            const int maxRetries = 3;
            for (var retry = 1; retry <= maxRetries; ++retry)
            {
                var relativeUri = _queryBuilder.BuildFindPackagesByIdUri(id).TrimStart('/');
                var uri = _baseUri + relativeUri;
                var httpSourceCacheContext = HttpSourceCacheContext.Create(cacheContext, isFirstAttempt: retry == 1);

                try
                {
                    var results = new List<PackageInfo>();
                    var uris = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    uris.Add(uri);
                    var page = 1;
                    var paging = true;
                    while (paging)
                    {
                        // TODO: Pages for a package ID are cached separately.
                        // So we will get inaccurate data when a page shrinks.
                        // However, (1) In most cases the pages grow rather than shrink;
                        // (2) cache for pages is valid for only 30 min.
                        // So we decide to leave current logic and observe.
                        paging = await _httpSource.GetAsync(
                            new HttpSourceCachedRequest(
                                uri,
                                $"list_{id.ToLowerInvariant()}_page{page}",
                                httpSourceCacheContext)
                            {
                                AcceptHeaderValues =
                                {
                                new MediaTypeWithQualityHeaderValue("application/atom+xml"),
                                new MediaTypeWithQualityHeaderValue("application/xml")
                                },
                                EnsureValidContents = stream => HttpStreamValidation.ValidateXml(uri, stream),
                                MaxTries = 1,
                                IsRetry = retry > 1,
                                IsLastAttempt = retry == maxRetries
                            },
                            async httpSourceResult =>
                            {
                                if (httpSourceResult.Status == HttpSourceResultStatus.NoContent)
                                {
                                    // Team city returns 204 when no versions of the package exist
                                    // This should result in an empty list and we should not try to
                                    // read the stream as xml.
                                    return false;
                                }

                                var doc = await V2FeedParser.LoadXmlAsync(httpSourceResult.Stream, cancellationToken);

                                var result = doc.Root
                                    .Elements(_xnameEntry)
                                    .Select(x => BuildModel(id, x))
                                    .Where(x => x != null);

                                results.AddRange(result);

                                // Find the next url for continuation
                                var nextUri = V2FeedParser.GetNextUrl(doc);

                                // Stop if there's nothing else to GET
                                if (string.IsNullOrEmpty(nextUri))
                                {
                                    return false;
                                }

                                // check for any duplicate url and error out
                                if (!uris.Add(nextUri))
                                {
                                    throw new FatalProtocolException(string.Format(
                                        CultureInfo.CurrentCulture,
                                        Strings.Protocol_duplicateUri,
                                        nextUri));
                                }

                                uri = nextUri;
                                page++;

                                return true;
                            },
                            logger,
                            cancellationToken);
                    }

                    return results;
                }
                catch (Exception ex) when (retry < maxRetries)
                {
                    var message = string.Format(CultureInfo.CurrentCulture, Strings.Log_RetryingFindPackagesById, nameof(FindPackagesByIdAsyncCore), uri)
                        + Environment.NewLine
                        + ExceptionUtilities.DisplayMessage(ex);
                    logger.LogMinimal(message);
                }
                catch (Exception ex) when (retry == maxRetries)
                {
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Log_FailedToRetrievePackage,
                        id,
                        uri);

                    throw new FatalProtocolException(message, ex);
                }
            }

            return null;
        }

        private static PackageInfo BuildModel(string id, XElement element)
        {
            var properties = element.Element(_xnameProperties);
            var idElement = properties.Element(_xnameId);

            return new PackageInfo
            {
                Identity = new PackageIdentity(
                     idElement?.Value ?? id, // Use the given Id as final fallback if all elements above don't exist
                     NuGetVersion.Parse(properties.Element(_xnameVersion).Value)),
                ContentUri = element.Element(_xnameContent).Attribute("src").Value,
            };
        }

        private class PackageInfo
        {
            public PackageIdentity Identity { get; set; }

            public string Path { get; set; }

            public string ContentUri { get; set; }
        }
    }
}
