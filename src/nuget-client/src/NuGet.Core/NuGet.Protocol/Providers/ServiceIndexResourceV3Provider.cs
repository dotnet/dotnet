// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    /// <summary>
    /// Retrieves and caches service index.json files
    /// ServiceIndexResourceV3 stores the json, all work is done in the provider
    /// </summary>
    public class ServiceIndexResourceV3Provider : ResourceProvider
    {
        private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(40);
        private readonly ConcurrentDictionary<string, ServiceIndexCacheInfo> _cache;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly EnhancedHttpRetryHelper _enhancedHttpRetryHelper;


        /// <summary>
        /// Maximum amount of time to store index.json
        /// </summary>
        public TimeSpan MaxCacheDuration { get; protected set; }

        public ServiceIndexResourceV3Provider() : this(EnvironmentVariableWrapper.Instance) { }

        internal ServiceIndexResourceV3Provider(IEnvironmentVariableReader environmentVariableReader)
            : base(typeof(ServiceIndexResourceV3),
                  nameof(ServiceIndexResourceV3Provider),
                  NuGetResourceProviderPositions.Last)
        {
            _cache = new ConcurrentDictionary<string, ServiceIndexCacheInfo>(StringComparer.OrdinalIgnoreCase);
            MaxCacheDuration = DefaultCacheDuration;
            _enhancedHttpRetryHelper = new EnhancedHttpRetryHelper(environmentVariableReader);
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            ServiceIndexResourceV3 index = null;
            ServiceIndexCacheInfo cacheInfo = null;
            var url = source.PackageSource.Source;

            // the file type can easily rule out if we need to request the url
            if (source.PackageSource.ProtocolVersion == 3 ||
                (source.PackageSource.IsHttp &&
                url.EndsWith(".json", StringComparison.OrdinalIgnoreCase)))
            {
                var utcNow = DateTime.UtcNow;
                var entryValidCutoff = utcNow.Subtract(MaxCacheDuration);

                // check the cache before downloading the file
                if (!_cache.TryGetValue(url, out cacheInfo) ||
                    entryValidCutoff > cacheInfo.CachedTime)
                {
                    await _semaphore.WaitAsync(token);

                    try
                    {
                        // check the cache again, another thread may have finished this one waited for the lock
                        if (!_cache.TryGetValue(url, out cacheInfo) ||
                            entryValidCutoff > cacheInfo.CachedTime)
                        {
                            index = await GetServiceIndexResourceV3(source, utcNow, NullLogger.Instance, token);

                            // cache the value even if it is null to avoid checking it again later
                            var cacheEntry = new ServiceIndexCacheInfo
                            {
                                CachedTime = utcNow,
                                Index = index
                            };

                            // If the cache entry has expired it will already exist
                            _cache.AddOrUpdate(url, cacheEntry, (key, value) => cacheEntry);
                        }
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
            }

            if (index == null && cacheInfo != null)
            {
                index = cacheInfo.Index;
            }

            return new Tuple<bool, INuGetResource>(index != null, index);
        }

        /// <summary>
        /// Read the source's end point to get the index json.
        /// Retries are logged to any provided <paramref name="log"/> as LogMinimal.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="utcNow"></param>
        /// <param name="log"></param>
        /// <param name="token"></param>
        /// <exception cref="OperationCanceledException">Logged to any provided <paramref name="log"/> as LogMinimal prior to throwing.</exception>
        /// <exception cref="FatalProtocolException">Encapsulates all other exceptions.</exception>
        /// <returns></returns>
        private async Task<ServiceIndexResourceV3> GetServiceIndexResourceV3(
            SourceRepository source,
            DateTime utcNow,
            ILogger log,
            CancellationToken token)
        {
            var url = source.PackageSource.Source;
            var httpSourceResource = await source.GetResourceAsync<HttpSourceResource>(token);
            var client = httpSourceResource.HttpSource;

            int maxRetries = _enhancedHttpRetryHelper.IsEnabled ? _enhancedHttpRetryHelper.RetryCount : 3;

            for (var retry = 1; retry <= maxRetries; retry++)
            {
                using (var sourceCacheContext = new SourceCacheContext())
                {
                    var cacheContext = HttpSourceCacheContext.Create(sourceCacheContext, isFirstAttempt: retry == 1);

                    try
                    {
                        return await client.GetAsync(
                            new HttpSourceCachedRequest(
                                url,
                                "service_index",
                                cacheContext)
                            {
                                EnsureValidContents = stream => HttpStreamValidation.ValidateJObject(url, stream),
                                MaxTries = 1,
                                IsRetry = retry > 1,
                                IsLastAttempt = retry == maxRetries
                            },
                            async httpSourceResult =>
                            {
                                var result = await ConsumeServiceIndexStreamAsync(httpSourceResult.Stream, utcNow, source.PackageSource, token);

                                return result;
                            },
                            log,
                            token);
                    }
                    catch (OperationCanceledException ex)
                    {
                        var message = ExceptionUtilities.DisplayMessage(ex);
                        log.LogMinimal(message);
                        throw;
                    }
                    catch (Exception ex) when (retry < maxRetries)
                    {
                        var message = string.Format(CultureInfo.CurrentCulture, Strings.Log_RetryingServiceIndex, url)
                            + Environment.NewLine
                            + ExceptionUtilities.DisplayMessage(ex);
                        log.LogMinimal(message);

                        if (_enhancedHttpRetryHelper.IsEnabled &&
                            ex.InnerException != null &&
                            ex.InnerException is IOException &&
                            ex.InnerException.InnerException != null &&
                            ex.InnerException.InnerException is System.Net.Sockets.SocketException)
                        {
                            // An IO Exception with inner SocketException indicates server hangup ("Connection reset by peer").
                            // Azure DevOps feeds sporadically do this due to mandatory connection cycling.
                            // Stalling an extra <ExperimentalRetryDelayMilliseconds> gives Azure more of a chance to recover.
                            log.LogVerbose("Enhanced retry: Encountered SocketException, delaying between tries to allow recovery");
                            await Task.Delay(TimeSpan.FromMilliseconds(_enhancedHttpRetryHelper.DelayInMilliseconds), token);
                        }
                    }
                    catch (Exception ex) when (retry == maxRetries)
                    {
                        var message = string.Format(CultureInfo.CurrentCulture, Strings.Log_FailedToReadServiceIndex, url);

                        throw new FatalProtocolException(message, ex);
                    }
                }
            }

            return null;
        }

        private static async Task<ServiceIndexResourceV3> ConsumeServiceIndexStreamAsync(Stream stream, DateTime utcNow, PackageSource source, CancellationToken token)
        {
            // Parse the JSON
            JObject json = await stream.AsJObjectAsync(token);

            // Use SemVer instead of NuGetVersion, the service index should always be
            // in strict SemVer format
            JToken versionToken;
            if (json.TryGetValue("version", out versionToken) &&
                versionToken.Type == JTokenType.String)
            {
                SemanticVersion version;
                if (SemanticVersion.TryParse((string)versionToken, out version) &&
                    version.Major == 3)
                {
                    return new ServiceIndexResourceV3(json, utcNow, source);
                }
                else
                {
                    string errorMessage = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Protocol_UnsupportedVersion,
                        (string)versionToken);
                    throw new InvalidDataException(errorMessage);
                }
            }
            else
            {
                throw new InvalidDataException(Strings.Protocol_MissingVersion);
            }
        }

        protected class ServiceIndexCacheInfo
        {
            public ServiceIndexResourceV3 Index { get; set; }

            public DateTime CachedTime { get; set; }
        }
    }
}
