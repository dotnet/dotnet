// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Azure;
using Azure.Storage.Blobs;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class AzureStorageContainerAssetSasCredentialPublisher : AzureStorageAssetPublisher
    {
        private readonly Uri _containerUri;
        private readonly AzureSasCredential _sasCredential;

        public AzureStorageContainerAssetSasCredentialPublisher(Uri containerUri, AzureSasCredential sasCredential, TaskLoggingHelper log) : base(log)
        {
            _containerUri = containerUri;
            _sasCredential = sasCredential;
        }

        public override BlobClient CreateBlobClient(string blobPath)
        {
            // When creating the blob client from the URI, only utilize the query parameters for the SAS uri (excluding the leading ?)
            var containerClient = new BlobContainerClient(_containerUri, _sasCredential);
            return containerClient.GetBlobClient(blobPath);
        }
    }
}
