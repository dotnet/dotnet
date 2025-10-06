// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Azure.Core;
using Azure.Storage.Blobs;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class AzureStorageContainerAssetTokenCredentialPublisher : AzureStorageAssetPublisher
    {
        private readonly Uri _containerUri;
        private readonly TokenCredential _tokenCredential;

        public AzureStorageContainerAssetTokenCredentialPublisher(Uri containerUri, TokenCredential tokenCredential, TaskLoggingHelper log) : base(log)
        {
            _containerUri = containerUri;
            _tokenCredential = tokenCredential;
        }

        public override BlobClient CreateBlobClient(string blobPath)
        {
            // When creating the blob client from the URI, only utilize the query parameters for the SAS uri (excluding the leading ?)
            var containerClient = new BlobContainerClient(_containerUri, _tokenCredential);
            return containerClient.GetBlobClient(blobPath);
        }
    }
}
