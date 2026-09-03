// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Azure;
using Azure.Storage.Blobs;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    internal sealed class AzureBlobClientFactory : IBlobClientFactory
    {
        public IBlobClient CreateBlobClient(string blobUri, string sasToken = null)
        {
            var options = CreateOptions();
            if (string.IsNullOrEmpty(sasToken))
            {
                return new AzureBlobClient(new BlobClient(new Uri(blobUri), options));
            }

            string strippedUri = blobUri.Contains('?') ? blobUri.Substring(0, blobUri.LastIndexOf('?', StringComparison.Ordinal)) : blobUri;
            return new AzureBlobClient(new BlobClient(new Uri(strippedUri), new AzureSasCredential(sasToken), options));
        }

        public IBlobClient CreateBlobClient(Uri containerUri, string blobName, string sasToken)
        {
            var containerClient = new BlobContainerClient(containerUri, new AzureSasCredential(sasToken));
            return new AzureBlobClient(containerClient.GetBlobClient(blobName));
        }

        private static BlobClientOptions CreateOptions()
        {
            var options = new BlobClientOptions();
            options.Retry.NetworkTimeout = TimeSpan.FromMinutes(5);
            return options;
        }
    }
}
