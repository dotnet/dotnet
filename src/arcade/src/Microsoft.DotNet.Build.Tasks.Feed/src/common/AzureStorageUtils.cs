// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Azure.Core;
using Azure.Core.Pipeline;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Microsoft.Arcade.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
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
            StorageSharedKeyCredential credential = new(AccountName, AccountKey);
            Uri endpoint = new($"https://{AccountName}.blob.core.windows.net");
            BlobServiceClient service = new(endpoint, credential, s_clientOptions);
            Container = service.GetBlobContainerClient(ContainerName);
        }

        public AzureStorageUtils(string accountName, TokenCredential credential, string containerName)
        {
            Uri endpoint = new($"https://{accountName}.blob.core.windows.net");
            BlobServiceClient service = new(endpoint, credential, s_clientOptions);
            Container = service.GetBlobContainerClient(containerName);
            service.GetBlobContainerClient(containerName);
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
            return Container.GenerateSasUri(builder).ToString();
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
    }
}
