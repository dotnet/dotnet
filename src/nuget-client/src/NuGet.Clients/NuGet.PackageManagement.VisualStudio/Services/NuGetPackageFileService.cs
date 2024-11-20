// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.ServiceHub.Framework.Services;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.PackageManagement.Telemetry;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.PackageManagement.VisualStudio
{
    public sealed class NuGetPackageFileService : INuGetPackageFileService, IDisposable
    {
        public static readonly string IconPrefix = "icon:";
        public static readonly string LocalIconPrefix = "localIcon:";
        public static readonly string LicensePrefix = "license:";

        private ServiceActivationOptions? _options;
        private IServiceBroker _serviceBroker;
        private AuthorizationServiceClient? _authorizationServiceClient;
        private INuGetTelemetryProvider _nuGetTelemetryProvider;
        private bool _disposedValue;
        private HttpClient _httpClient = new HttpClient();

        internal readonly static MemoryCache IdentityToUriCache = new MemoryCache("PackageSearchMetadata",
            new NameValueCollection
            {
                { "cacheMemoryLimitMegabytes", "4" },
                { "physicalMemoryLimitPercentage", "0" },
                { "pollingInterval", "00:02:00" }
            });

        private static readonly CacheItemPolicy CacheItemPolicy = new CacheItemPolicy
        {
            SlidingExpiration = ObjectCache.NoSlidingExpiration,
            AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
        };

        public static void AddIconToCache(PackageIdentity packageIdentity, Uri iconUri)
        {
            string key = NuGetPackageFileService.IconPrefix + packageIdentity.ToString();
            if (iconUri != null)
            {
                IdentityToUriCache.Set(key, iconUri, CacheItemPolicy);
            }
        }

        public static void AddLocalIconToCache(PackageIdentity packageIdentity, Uri iconUri)
        {
            string key = NuGetPackageFileService.LocalIconPrefix + packageIdentity.ToString();
            if (iconUri != null)
            {
                IdentityToUriCache.Set(key, iconUri, CacheItemPolicy);
            }
        }

        public static void AddLicenseToCache(PackageIdentity packageIdentity, Uri embeddedLicenseUri)
        {
            Assumes.NotNull(embeddedLicenseUri);
            string key = NuGetPackageFileService.LicensePrefix + packageIdentity.ToString();
            IdentityToUriCache.Set(key, embeddedLicenseUri, CacheItemPolicy);
        }

        public NuGetPackageFileService(
            ServiceActivationOptions options,
            IServiceBroker serviceBroker,
            AuthorizationServiceClient authorizationServiceClient,
            INuGetTelemetryProvider nuGetTelemetryProvider)
        {
            _options = options;
            _serviceBroker = serviceBroker;
            _authorizationServiceClient = authorizationServiceClient;
            _nuGetTelemetryProvider = nuGetTelemetryProvider;
            Assumes.NotNull(_serviceBroker);
            Assumes.NotNull(_authorizationServiceClient);
            Assumes.NotNull(_nuGetTelemetryProvider);
        }

        public NuGetPackageFileService(IServiceBroker serviceBroker, INuGetTelemetryProvider nuGetTelemetryProvider)
        {
            _serviceBroker = serviceBroker;
            Assumes.NotNull(_serviceBroker);

            _nuGetTelemetryProvider = nuGetTelemetryProvider;
            Assumes.NotNull(_nuGetTelemetryProvider);
        }

        public async ValueTask<Stream?> GetPackageIconAsync(PackageIdentity packageIdentity, CancellationToken cancellationToken)
        {
            Assumes.NotNull(packageIdentity);
            string packageId = packageIdentity.ToString();
            string key = NuGetPackageFileService.IconPrefix + packageId;

            Uri? uri = IdentityToUriCache.Get(key) as Uri;

            if (uri == null)
            {
                var exception = new CacheMissException();
                await _nuGetTelemetryProvider.PostFaultAsync(exception, typeof(NuGetPackageFileService).FullName, nameof(NuGetPackageFileService.GetPackageIconAsync));
                return null;
            }

            Stream? stream = null;
            if (IsEmbeddedUri(uri))
            {
                stream = await GetEmbeddedFileAsync(uri, cancellationToken);
            }
            else
            {
                Uri? localUri = GetLocalEmbeddedIconUri(packageId);
                if (localUri is not null)
                {
                    stream = await GetEmbeddedFileAsync(localUri, cancellationToken);
                }

                if (stream == null)
                {
                    stream = await GetStream(uri);
                }
            }

            return stream;
        }

        private static Uri? GetLocalEmbeddedIconUri(string packageId)
        {
            string localIconKey = NuGetPackageFileService.LocalIconPrefix + packageId;
            Uri? localUri = IdentityToUriCache.Get(localIconKey) as Uri;
            if (localUri is not null && IsEmbeddedUri(localUri))
            {
                return localUri;
            }

            return null;
        }

        public async ValueTask<Stream?> GetEmbeddedLicenseAsync(PackageIdentity packageIdentity, CancellationToken cancellationToken)
        {
            Assumes.NotNull(packageIdentity);
            string key = NuGetPackageFileService.LicensePrefix + packageIdentity.ToString();
            Uri? uri = IdentityToUriCache.Get(key) as Uri;
            if (uri == null)
            {
                var exception = new CacheMissException();
                await _nuGetTelemetryProvider.PostFaultAsync(exception, typeof(NuGetPackageFileService).FullName, nameof(NuGetPackageFileService.GetEmbeddedLicenseAsync));
                return null;
            }

            Stream? stream = await GetEmbeddedFileAsync(uri, cancellationToken);
            return stream;
        }

        public async ValueTask<Stream?> GetReadmeAsync(Uri readmeUri, CancellationToken cancellationToken)
        {
            Assumes.NotNull(readmeUri);

            Stream? stream;
            if (IsEmbeddedUri(readmeUri))
            {
                stream = await GetEmbeddedFileAsync(readmeUri, cancellationToken);
            }
            else
            {
                stream = await GetStream(readmeUri);
            }

            return stream;
        }

        private async ValueTask<Stream?> GetEmbeddedFileAsync(Uri uri, CancellationToken cancellationToken)
        {
            string packagePath = uri.LocalPath;
            if (File.Exists(packagePath))
            {
                string fileRelativePath = PathUtility.StripLeadingDirectorySeparators(
                Uri.UnescapeDataString(uri.Fragment)
                    .Substring(1)); // Substring skips the '#' in the URI fragment

                string dirPath = Path.GetDirectoryName(packagePath);
                string extractedIconPath = Path.Combine(dirPath, fileRelativePath);

                // use GetFullPath to normalize "..", so that zip slip attack cannot allow a user to walk up the file directory
                if (Path.GetFullPath(extractedIconPath).StartsWith(dirPath, StringComparison.OrdinalIgnoreCase) && File.Exists(extractedIconPath))
                {
                    Stream fileStream = File.OpenRead(extractedIconPath);
                    return fileStream;
                }
                else
                {
                    try
                    {
                        using (PackageArchiveReader reader = new PackageArchiveReader(packagePath))
                        using (Stream parStream = await reader.GetStreamAsync(fileRelativePath, cancellationToken))
                        {
                            var memoryStream = new MemoryStream();
                            await parStream.CopyToAsync(memoryStream);
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            return memoryStream;
                        }
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        private async Task<Stream?> GetStream(Uri uri)
        {
            if (uri.IsFile)
            {
                if (File.Exists(uri.LocalPath))
                {
                    return File.OpenRead(uri.LocalPath);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                try
                {
                    return await _httpClient.GetStreamAsync(uri);
                }
                catch (HttpRequestException)
                {
                    return null;
                }
                catch (TaskCanceledException)
                {
                    return null;
                }
                catch (ArgumentException)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// NuGet Embedded Uri verification
        /// </summary>
        /// <param name="uri">An URI to test</param>
        /// <returns><see langword="true" /> if <c>uri</c> is an URI to an embedded file in a NuGet package</returns>
        public static bool IsEmbeddedUri(Uri uri)
        {
            return uri != null
                && uri.IsAbsoluteUri
                && uri.IsFile
                && !string.IsNullOrEmpty(uri.Fragment)
                && uri.Fragment.Length > 1;
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _authorizationServiceClient?.Dispose();
                    _httpClient?.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
