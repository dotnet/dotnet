// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.CloudTestTasks;
#if !NET472_OR_GREATER
using Microsoft.DotNet.ProductConstructionService.Client.Models;
#endif
using Task = System.Threading.Tasks.Task;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
#if !NET472_OR_GREATER
    public abstract class AzureStorageAssetPublisher : IAssetPublisher
    {
        private readonly TaskLoggingHelper _log;

        protected AzureStorageAssetPublisher(TaskLoggingHelper log)
        {
            _log = log;
        }

        public LocationType LocationType => LocationType.Container;

        public abstract BlobClient CreateBlobClient(string blobPath);

        public async Task PublishAssetAsync(string file, string blobPath, PushOptions options, SemaphoreSlim clientThrottle = null)
        {
            using (await SemaphoreLock.LockAsync(clientThrottle))
            {
                blobPath = blobPath.Replace("\\", "/");
                var blobClient = CreateBlobClient(blobPath);
                if (!options.AllowOverwrite && await blobClient.ExistsAsync())
                {
                    if (options.PassIfExistingItemIdentical)
                    {
                        if (!await blobClient.IsFileIdenticalToBlobAsync(file))
                        {
                            _log.LogError($"Asset '{file}' already exists with different contents at '{blobClient.Uri}'");
                        }

                        return;
                    }

                    _log.LogError($"Asset '{file}' already exists at '{blobClient.Uri}'");
                    return;
                }

                _log.LogMessage($"Uploading '{file}' to '{blobClient.Uri}'");

                try
                {
                    BlobUploadOptions blobUploadOptions = new()
                    {
                        HttpHeaders = AzureStorageUtils.GetBlobHeadersByExtension(file)
                    };
                    await blobClient.UploadAsync(file, blobUploadOptions);
                }
                catch (Exception e)
                {
                    _log.LogError($"Unexpected exception publishing file {file} to {blobClient.Uri}: {e.Message}");
                }
            }
        }
    }
#else
    public abstract class AzureStorageAssetPublisher : IAssetPublisher
    {
        private readonly TaskLoggingHelper _log;

        protected AzureStorageAssetPublisher(TaskLoggingHelper log)
        {
            _log = log;
        }

        public abstract BlobClient CreateBlobClient(string blobPath);

        public Task PublishAssetAsync(string file, string blobPath, PushOptions options, SemaphoreSlim clientThrottle = null) => throw new NotImplementedException();
    }
#endif
}
