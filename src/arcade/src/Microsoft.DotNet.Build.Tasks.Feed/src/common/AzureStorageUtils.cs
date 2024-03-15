// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Microsoft.Arcade.Common;
using Microsoft.Build.Framework;
using MsBuildUtils = Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using System.Net.Http;
using Microsoft.DotNet.Build.Tasks.Feed;

namespace Microsoft.DotNet.Build.CloudTestTasks
{
    public class AzureStorageUtils
    {
        private static readonly Dictionary<string, string> MimeMappings = new Dictionary<string, string>()
        {
            {".svg", "image/svg+xml"},
            {".version", "text/plain"}
        };

        private static readonly Dictionary<string, string> CacheMappings = new Dictionary<string, string>()
        {
            {".svg", "no-cache"}
        };

        /// <summary>
        ///  Enum describing the states of a given package on a feed
        /// </summary>
        public enum PackageFeedStatus
        {
            DoesNotExist,
            ExistsAndIdenticalToLocal,
            ExistsAndDifferent,
            Unknown
        }

        // Save the credential so we can sign SAS tokens
        private readonly StorageSharedKeyCredential _credential;

        public BlobContainerClient Container { get; set; }

        private static readonly HttpClient s_httpClient = new HttpClient(
            new HttpClientHandler() 
            { 
                CheckCertificateRevocationList = true 
            })
        { 
            Timeout = TimeSpan.FromSeconds(300) 
        };
        private static readonly BlobClientOptions s_clientOptions = new BlobClientOptions()
        {
            Transport = new HttpClientTransport(s_httpClient)
        };

        public AzureStorageUtils(string AccountName, string AccountKey, string ContainerName)
        {
            _credential = new StorageSharedKeyCredential(AccountName, AccountKey);
            Uri endpoint = new Uri($"https://{AccountName}.blob.core.windows.net");
            BlobServiceClient service = new BlobServiceClient(endpoint, _credential, s_clientOptions);
            Container = service.GetBlobContainerClient(ContainerName);
        }

        public BlobClient GetBlob(string destinationBlob) =>
            Container.GetBlobClient(destinationBlob);

        public BlockBlobClient GetBlockBlob(string destinationBlob) =>
            Container.GetBlockBlobClient(destinationBlob);

        public static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())  // lgtm [cs/weak-crypto] Azure Storage specifies use of MD5
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return Convert.ToBase64String(hash);
                }
            }
        }

        public async Task UploadBlockBlobAsync(string filePath, string blobPath, Stream stream)
        {
            BlobClient blob = GetBlob(blobPath.Replace("\\", "/"));
            BlobHttpHeaders headers = GetBlobHeadersByExtension(filePath);

            // This function can sporadically throw 
            // "System.Net.Http.HttpRequestException: Error while copying content to a stream."
            // Ideally it should retry for itself internally, but the existing retry seems 
            // to be intended for throttling only.
            var retryHandler = new ExponentialRetry
            {
                MaxAttempts = 5,
                DelayBase = 2.5 // 2.5 ^ 5 = ~1.5 minutes max wait between retries
            };

            Exception mostRecentlyCaughtException = null;

            bool success = await retryHandler.RunAsync(async attempt =>
            {
                try
                {
                    await blob.UploadAsync(
                        stream,
                        headers)
                        .ConfigureAwait(false);
                    return true;
                }
                catch (System.Net.Http.HttpRequestException toStore)
                {
                    mostRecentlyCaughtException = toStore;
                    return false;
                }
            }).ConfigureAwait(false);

            // If retry failed print out a nice looking exception
            if (!success)
            {
                throw new Exception($"Failed to upload local file '{filePath}' to '{blobPath} in  {retryHandler.MaxAttempts} attempts.  See inner exception for details.", mostRecentlyCaughtException);
            }
        }

        public async Task<bool> IsFileIdenticalToBlobAsync(string localFileFullPath, string blobPath) =>
            await GetBlob(blobPath).IsFileIdenticalToBlobAsync(localFileFullPath);

        public async Task<string> CreateContainerAsync(PublicAccessType publicAccess)
        {
            await Container.CreateIfNotExistsAsync(publicAccess).ConfigureAwait(false);
            return Container.Uri.ToString();
        }

        public string CreateSASToken(int tokenExpirationInDays, BlobContainerSasPermissions containerPermissions)
        {
            BlobSasBuilder builder = new BlobSasBuilder
            {
                BlobContainerName = Container.Name,
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(tokenExpirationInDays)
            };
            builder.SetPermissions(containerPermissions);
            return builder.ToSasQueryParameters(_credential).ToString();
        }

        public async Task<bool> CheckIfContainerExistsAsync() =>
            await Container.ExistsAsync().ConfigureAwait(false);

        public async Task<bool> CheckIfBlobExistsAsync(string blobPath) =>
            await GetBlob(blobPath).ExistsAsync().ConfigureAwait(false);


        public static BlobHttpHeaders GetBlobHeadersByExtension(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("An attempt to get the MIME mapping of an empty path was made.");
            }

            var fileExtension = Path.GetExtension(filePath).ToLowerInvariant();

            BlobHttpHeaders headers = new BlobHttpHeaders
            {
                ContentType = MimeMappings.TryGetValue(fileExtension, out string cttType) ?
                cttType :
                "application/octet-stream"
            };

            if (CacheMappings.TryGetValue(fileExtension, out string cacheCtrl))
            {
                headers.CacheControl = cacheCtrl;
            }

            return headers;
        }

        /// <summary>
        ///     Determine whether a local package is the same as a package on an AzDO feed.
        /// </summary>
        /// <param name="localPackageFullPath"></param>
        /// <param name="packageContentUrl"></param>
        /// <param name="client"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Open a stream to the local file and an http request to the package. There are a couple possibilities:
        ///     - The returned headers include a content MD5 header, in which case we can
        ///       hash the local file and just compare those.
        ///     - No content MD5 hash, and the streams must be compared in blocks. This is a bit trickier to do efficiently,
        ///       since we do not necessarily want to read all bytes if we can help it. Thus, we should compare in blocks.  However,
        ///       the streams make no guarantee that they will return a full block each time when read operations are performed, so we
        ///       must be sure to only compare the minimum number of bytes returned.
        /// </remarks>
        public static async Task<PackageFeedStatus> CompareLocalPackageToFeedPackage(
            string localPackageFullPath,
            string packageContentUrl,
            HttpClient client,
            MsBuildUtils.TaskLoggingHelper log)
        {
            return await CompareLocalPackageToFeedPackage(
                localPackageFullPath,
                packageContentUrl,
                client,
                log,
                GeneralUtils.CreateDefaultRetryHandler());
        }

        /// <summary>
        ///     Determine whether a local package is the same as a package on an AzDO feed.
        /// </summary>
        /// <param name="localPackageFullPath"></param>
        /// <param name="packageContentUrl"></param>
        /// <param name="client"></param>
        /// <param name="log"></param>
        /// <param name="retryHandler"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Open a stream to the local file and an http request to the package. There are a couple possibilities:
        ///     - The returned headers include a content MD5 header, in which case we can
        ///       hash the local file and just compare those.
        ///     - No content MD5 hash, and the streams must be compared in blocks. This is a bit trickier to do efficiently,
        ///       since we do not necessarily want to read all bytes if we can help it. Thus, we should compare in blocks.  However,
        ///       the streams make no guarantee that they will return a full block each time when read operations are performed, so we
        ///       must be sure to only compare the minimum number of bytes returned.
        /// </remarks>
        public static async Task<PackageFeedStatus> CompareLocalPackageToFeedPackage(
            string localPackageFullPath,
            string packageContentUrl,
            HttpClient client,
            MsBuildUtils.TaskLoggingHelper log,
            IRetryHandler retryHandler)
        {
            log.LogMessage($"Getting package content from {packageContentUrl} and comparing to {localPackageFullPath}");

            PackageFeedStatus result = PackageFeedStatus.Unknown;

            bool success = await retryHandler.RunAsync(async attempt =>
            {
                try
                {
                    using (Stream localFileStream = File.OpenRead(localPackageFullPath))
                    using (HttpResponseMessage response = await client.GetAsync(packageContentUrl))
                    {
                        response.EnsureSuccessStatusCode();

                        // Check the headers for content length and md5 
                        bool md5HeaderAvailable = response.Headers.TryGetValues("Content-MD5", out var md5);
                        bool lengthHeaderAvailable = response.Headers.TryGetValues("Content-Length", out var contentLength);

                        if (lengthHeaderAvailable && long.Parse(contentLength.Single()) != localFileStream.Length)
                        {
                            log.LogMessage(MessageImportance.Low, $"Package '{localPackageFullPath}' has different length than remote package '{packageContentUrl}'.");
                            result = PackageFeedStatus.ExistsAndDifferent;
                            return true;
                        }

                        if (md5HeaderAvailable)
                        {
                            var localMD5 = AzureStorageUtils.CalculateMD5(localPackageFullPath);
                            if (!localMD5.Equals(md5.Single(), StringComparison.OrdinalIgnoreCase))
                            {
                                log.LogMessage(MessageImportance.Low, $"Package '{localPackageFullPath}' has different MD5 hash than remote package '{packageContentUrl}'.");
                            }

                            result = PackageFeedStatus.ExistsAndDifferent;
                            return true;
                        }

                        const int BufferSize = 64 * 1024;

                        // Otherwise, compare the streams
                        var remoteStream = await response.Content.ReadAsStreamAsync();
                        var streamsMatch = await GeneralUtils.CompareStreamsAsync(localFileStream, remoteStream, BufferSize);
                        result = streamsMatch ? PackageFeedStatus.ExistsAndIdenticalToLocal : PackageFeedStatus.ExistsAndDifferent;
                        return true;
                    }
                }
                // String based comparison because the status code isn't exposed in HttpRequestException
                // see here: https://github.com/dotnet/runtime/issues/23648
                catch (Exception e) when (e is HttpRequestException || e is TaskCanceledException)
                {
                    if (e.Message.Contains("404 (Not Found)"))
                    {
                        result = PackageFeedStatus.DoesNotExist;
                        return true;
                    }

                    // Retry this. Could be an http client timeout, 500, etc.
                    return false;
                }
            });

            return result;
        }
    }
}
