// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Azure.Storage.Blobs;
using Azure;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class AzureStorageContainerAssetPublisher : AzureStorageAssetPublisher
    {
        private readonly Uri _containerUri;
        private readonly Uri _sasUri;

        public AzureStorageContainerAssetPublisher(Uri containerUri, Uri sasUri, TaskLoggingHelper log) : base(log)
        {
            _containerUri = containerUri;
            _sasUri = sasUri;
        }

        public override BlobClient CreateBlobClient(string blobPath)
        {
            // When creating the blob client from the URI, only utilize the query parameters for the SAS uri (excluding the leading ?)
            var containerClient = new BlobContainerClient(_containerUri, new AzureSasCredential(_sasUri.Query));
            return containerClient.GetBlobClient(blobPath);
        }
    }
}
