// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Events;

namespace NuGet.Protocol
{
    public class FindPackagesByIdNupkgDownloader
    {
        private readonly object _cacheEntriesLock = new object();
        private readonly Dictionary<string, Task<CacheEntry>> _cacheEntries =
            new Dictionary<string, Task<CacheEntry>>();

        private readonly object _nuspecReadersLock = new object();
        private readonly ConcurrentDictionary<string, NuspecReader> _nuspecReaders =
            new ConcurrentDictionary<string, NuspecReader>();

        private readonly HttpSource _httpSource;
        private readonly EnhancedHttpRetryHelper _enhancedHttpRetryHelper;

        public FindPackagesByIdNupkgDownloader(HttpSource httpSource) : this(httpSource, EnvironmentVariableWrapper.Instance) { }

        internal FindPackagesByIdNupkgDownloader(HttpSource httpSource, IEnvironmentVariableReader environmentVariableReader)
        {
            if (httpSource == null)
            {
                throw new ArgumentNullException(nameof(httpSource));
            }

            _httpSource = httpSource;
            _enhancedHttpRetryHelper = new EnhancedHttpRetryHelper(environmentVariableReader);
        }

        /// <summary>
        /// Gets a <see cref="NuspecReader"/> from a .nupkg. If the URL cannot be fetched or there is a problem
        /// processing the .nuspec, an exception is throw. This method uses HTTP caching to avoid downloading the
        /// package over and over (unless <see cref="SourceCacheContext.DirectDownload"/> is specified).
        /// </summary>
        /// <param name="identity">The package identity.</param>
        /// <param name="url">The URL of the .nupkg.</param>
        /// <param name="cacheContext">The cache context.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The .nuspec reader.</returns>
        public async Task<NuspecReader> GetNuspecReaderFromNupkgAsync(
            PackageIdentity identity,
            string url,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken token)
        {
            NuspecReader reader = null;

            lock (_nuspecReadersLock)
            {
                if (_nuspecReaders.TryGetValue(url, out reader))
                {
                    return reader;
                }
            }

            await ProcessNupkgStreamAsync(
                identity,
                url,
                stream =>
                {
                    reader = PackageUtilities.OpenNuspecFromNupkg(identity.Id, stream, logger);

                    return TaskResult.True;
                },
                cacheContext,
                logger,
                token);

            if (reader == null)
            {
                // The package was not found on the feed. This typically means
                // that the feed listed the package, but then returned 404 for the nupkg.
                // The cache needs to be invaldiated and the download call made again.
                throw new PackageNotFoundProtocolException(identity);
            }

            lock (_nuspecReadersLock)
            {
                _nuspecReaders[url] = reader;
            }

            return reader;
        }

        /// <summary>
        /// Copies a .nupkg stream to the <paramref name="destination"/> stream. If the .nupkg cannot be found or if
        /// there is a network problem, no stream copy occurs.
        /// </summary>
        /// <param name="identity">The package identity.</param>
        /// <param name="url">The URL of the .nupkg.</param>
        /// <param name="destination">The destination stream. The .nupkg will be copied to this stream.</param>
        /// <param name="cacheContext">The cache context.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>Returns true if the stream was copied, false otherwise.</returns>
        public async Task<bool> CopyNupkgToStreamAsync(
            PackageIdentity identity,
            string url,
            Stream destination,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken token)
        {
            if (!destination.CanSeek)
            {
                // In order to handle retries, we need to write to a temporary file, then copy to destination in one pass.
                string tempFilePath = Path.GetTempFileName();
                using Stream tempFile = new FileStream(tempFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, bufferSize: 4096, FileOptions.DeleteOnClose);
                bool result = await CopyNupkgToStreamAsync(identity, url, tempFile, cacheContext, logger, token);

                tempFile.Position = 0;
                await tempFile.CopyToAsync(destination, token);

                return result;
            }
            else
            {
                return await ProcessNupkgStreamAsync(
                    identity,
                    url,
                    async stream =>
                    {
                        try
                        {
                            await stream.CopyToAsync(destination, token);
                            ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticNupkgCopiedEvent(_httpSource.PackageSource, destination.Length));
                        }
                        catch when (!token.IsCancellationRequested)
                        {
                            destination.Position = 0;
                            destination.SetLength(0);
                            throw;
                        }
                    },
                    cacheContext,
                    logger,
                    token);
            }
        }

        /// <summary>
        /// Manages the different ways of getting a .nupkg stream when using the global HTTP cache. When a stream is
        /// found, the <paramref name="processStreamAsync"/> method is invoked on said stream. This deals with the
        /// complexity of <see cref="SourceCacheContext.DirectDownload"/>.
        /// </summary>
        /// <param name="identity">The package identity.</param>
        /// <param name="url">The URL of the .nupkg to fetch.</param>
        /// <param name="processStreamAsync">The method to process the stream.</param>
        /// <param name="cacheContext">The cache context.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>
        /// Returns true if the stream was processed, false if the stream could not fetched (either from the HTTP cache
        /// or from the network).
        /// </returns>
        private async Task<bool> ProcessNupkgStreamAsync(
            PackageIdentity identity,
            string url,
            Func<Stream, Task> processStreamAsync,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken token)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (cacheContext == null)
            {
                throw new ArgumentNullException(nameof(cacheContext));
            }

            if (cacheContext.DirectDownload)
            {
                // Don't read from the in-memory cache if we are doing a direct download.
                var cacheEntry = await ProcessStreamAndGetCacheEntryAsync(
                    identity,
                    url,
                    processStreamAsync,
                    cacheContext,
                    logger,
                    token);

                // If we get back a cache file result from the cache, we can save it to the in-memory cache.
                lock (_cacheEntriesLock)
                {
                    if (cacheEntry.CacheFile != null && !_cacheEntries.ContainsKey(url))
                    {
                        _cacheEntries[url] = Task.FromResult(cacheEntry);
                    }
                }

                // Process the NupkgEntry
                return await ProcessCacheEntryAsync(cacheEntry, processStreamAsync, token);
            }
            else
            {
                // Try to get the NupkgEntry from the in-memory cache. If we find a match, we can open the cache file
                // and use that as the source stream, instead of going to the package source.
                Task<CacheEntry> nupkgEntryTask;
                lock (_cacheEntriesLock)
                {
                    if (!_cacheEntries.TryGetValue(url, out nupkgEntryTask))
                    {
                        nupkgEntryTask = ProcessStreamAndGetCacheEntryAsync(
                            identity,
                            url,
                            processStreamAsync,
                            cacheContext,
                            logger,
                            token);

                        _cacheEntries[url] = nupkgEntryTask;
                    }
                }

                var nupkgEntry = await nupkgEntryTask;

                return await ProcessCacheEntryAsync(nupkgEntry, processStreamAsync, token);
            }
        }

        private async Task<CacheEntry> ProcessStreamAndGetCacheEntryAsync(
            PackageIdentity identity,
            string url,
            Func<Stream, Task> processStreamAsync,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken token)
        {
            return await ProcessHttpSourceResultAsync(
                identity,
                url,
                async httpSourceResult =>
                {
                    if (httpSourceResult == null ||
                        httpSourceResult.Stream == null)
                    {
                        return new CacheEntry(cacheFile: null, alreadyProcessed: false);
                    }

                    if (httpSourceResult.CacheFile != null)
                    {
                        // Return the cache file name so that the caller can open the cache file directly
                        // and copy it to the destination stream.
                        return new CacheEntry(httpSourceResult.CacheFile, alreadyProcessed: false);
                    }
                    else
                    {
                        await processStreamAsync(httpSourceResult.Stream);

                        // When the stream came from the network directly, there is not cache file name. This
                        // happens when the caller enables DirectDownload.
                        return new CacheEntry(cacheFile: null, alreadyProcessed: true);
                    }
                },
                cacheContext,
                logger,
                token);
        }

        private async Task<T> ProcessHttpSourceResultAsync<T>(
            PackageIdentity identity,
            string url,
            Func<HttpSourceResult, Task<T>> processAsync,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken token)
        {
            int maxRetries = _enhancedHttpRetryHelper.IsEnabled ? _enhancedHttpRetryHelper.RetryCount : 3;

            for (var retry = 1; retry <= maxRetries; ++retry)
            {
                var httpSourceCacheContext = HttpSourceCacheContext.Create(cacheContext, isFirstAttempt: retry == 1);

                try
                {
                    return await _httpSource.GetAsync(
                        new HttpSourceCachedRequest(
                            url,
                            "nupkg_" + identity.Id.ToLowerInvariant() + "." + identity.Version.ToNormalizedString(),
                            httpSourceCacheContext)
                        {
                            EnsureValidContents = stream => HttpStreamValidation.ValidateNupkg(url, stream),
                            IgnoreNotFounds = true,
                            MaxTries = 1,
                            IsRetry = retry > 1,
                            IsLastAttempt = retry == maxRetries
                        },
                        async httpSourceResult => await processAsync(httpSourceResult),
                        logger,
                        token);
                }
                catch (TaskCanceledException) when (retry < maxRetries)
                {
                    // Requests can get cancelled if we got the data from elsewhere, no reason to warn.
                    var message = string.Format(CultureInfo.CurrentCulture, Strings.Log_CanceledNupkgDownload, url);

                    logger.LogMinimal(message);
                }
                catch (Exception ex) when (retry < maxRetries)
                {
                    var message = string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.Log_FailedToDownloadPackage,
                            identity,
                            url)
                        + Environment.NewLine
                        + ExceptionUtilities.DisplayMessage(ex);

                    logger.LogMinimal(message);

                    if (_enhancedHttpRetryHelper.IsEnabled &&
                        ex.InnerException != null &&
                        ex.InnerException is IOException &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException is System.Net.Sockets.SocketException)
                    {
                        // An IO Exception with inner SocketException indicates server hangup ("Connection reset by peer").
                        // Azure DevOps feeds sporadically do this due to mandatory connection cycling.
                        // Delaying gives Azure more of a chance to recover.
                        logger.LogVerbose("Enhanced retry: Encountered SocketException, delaying between tries to allow recovery");
                        await Task.Delay(TimeSpan.FromMilliseconds(_enhancedHttpRetryHelper.DelayInMilliseconds), token);
                    }
                }
                catch (Exception ex) when (retry == maxRetries)
                {
                    var message = string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.Log_FailedToDownloadPackage,
                            identity,
                            url)
                        + Environment.NewLine
                        + ExceptionUtilities.DisplayMessage(ex);

                    logger.LogError(message);
                }
            }

            return await processAsync(null);
        }

        private async Task<bool> ProcessCacheEntryAsync(
            CacheEntry cacheEntry,
            Func<Stream, Task> processStreamAsync,
            CancellationToken token)
        {
            if (cacheEntry.AlreadyProcessed)
            {
                return true;
            }

            if (cacheEntry.CacheFile == null)
            {
                return false;
            }

            // Acquire the lock on a file before we open it to prevent this process
            // from opening a file deleted by another HTTP request.
            using (var cacheStream = await ConcurrencyUtilities.ExecuteWithFileLockedAsync(
                cacheEntry.CacheFile,
                lockedToken =>
                {
                    return Task.FromResult(new FileStream(
                        cacheEntry.CacheFile,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.ReadWrite | FileShare.Delete,
                        StreamExtensions.BufferSize));
                },
                token))
            {
                await processStreamAsync(cacheStream);

                return true;
            }
        }

        private class CacheEntry
        {
            public CacheEntry(string cacheFile, bool alreadyProcessed)
            {
                CacheFile = cacheFile;
                AlreadyProcessed = alreadyProcessed;
            }

            public string CacheFile { get; }
            public bool AlreadyProcessed { get; }
        }
    }
}
