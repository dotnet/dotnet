// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Azure;
using Azure.Storage.Blobs;

namespace Microsoft.DotNet.Helix.JobMonitor;

internal sealed class AzureBlobClientFactory : IBlobClientFactory
{
    public IBlobClient CreateBlobClient(string blobUri, string sasToken = null)
    {
        var options = CreateOptions();
        if (string.IsNullOrEmpty(sasToken))
        {
            return new AzureBlobClient(new BlobClient(new Uri(blobUri), options));
        }

        int queryIndex = blobUri.IndexOf('?');
        string strippedUri = queryIndex >= 0 ? blobUri.Substring(0, queryIndex) : blobUri;
        string trimmedSasToken = sasToken.StartsWith("?", StringComparison.Ordinal)
            ? sasToken.Substring(1)
            : sasToken;
        return new AzureBlobClient(new BlobClient(new Uri(strippedUri), new AzureSasCredential(trimmedSasToken), options));
    }

    public IBlobClient CreateBlobClient(Uri containerUri, string blobName, string sasToken)
    {
        string trimmedSasToken = !string.IsNullOrEmpty(sasToken) && sasToken.StartsWith("?", StringComparison.Ordinal)
            ? sasToken.Substring(1)
            : sasToken;
        var containerClient = new BlobContainerClient(containerUri, new AzureSasCredential(trimmedSasToken));
        return new AzureBlobClient(containerClient.GetBlobClient(blobName));
    }

    private static BlobClientOptions CreateOptions()
    {
        var options = new BlobClientOptions();
        options.Retry.NetworkTimeout = TimeSpan.FromMinutes(5);
        return options;
    }
}
