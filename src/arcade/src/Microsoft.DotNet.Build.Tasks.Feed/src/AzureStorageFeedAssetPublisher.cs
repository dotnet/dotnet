// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class AzureStorageFeedAssetPublisher : AzureStorageAssetPublisher
    {
        private readonly string _accountName;
        private readonly string _accountKey;
        private readonly string _containerName;

        public AzureStorageFeedAssetPublisher(string accountName, string accountKey, string containerName,
            TaskLoggingHelper log) : base(log)
        {
            _accountName = accountName;
            _accountKey = accountKey;
            _containerName = containerName;
        }

        public override BlobClient CreateBlobClient(string blobPath)
        {
            var cred = new StorageSharedKeyCredential(_accountName, _accountKey);
            var endpoint = new Uri($"https://{_accountName}.blob.core.windows.net");
            var service = new BlobServiceClient(endpoint, cred);
            var container = service.GetBlobContainerClient(_containerName);
            return container.GetBlobClient(blobPath);
        }
    }
}
