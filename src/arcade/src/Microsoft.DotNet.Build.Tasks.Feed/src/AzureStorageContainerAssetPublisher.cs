// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Azure.Storage.Blobs;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class AzureStorageContainerAssetPublisher : AzureStorageAssetPublisher
    {
        private readonly Uri _containerUri;

        public AzureStorageContainerAssetPublisher(Uri containerUri, TaskLoggingHelper log) : base(log)
        {
            _containerUri = containerUri;
        }

        public override BlobClient CreateBlobClient(string blobPath)
        {
            var containerClient = new BlobContainerClient(_containerUri); 
            return containerClient.GetBlobClient(blobPath);
        }
    }
}
