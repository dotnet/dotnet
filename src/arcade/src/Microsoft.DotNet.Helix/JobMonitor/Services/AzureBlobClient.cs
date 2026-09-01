// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    internal sealed class AzureBlobClient : IBlobClient
    {
        private readonly BlobClient _blobClient;

        public AzureBlobClient(BlobClient blobClient)
        {
            _blobClient = blobClient ?? throw new ArgumentNullException(nameof(blobClient));
        }

        public Uri Uri => _blobClient.Uri;

        public async Task DownloadToAsync(string destinationFile, CancellationToken cancellationToken)
        {
            await _blobClient.DownloadToAsync(destinationFile, cancellationToken);
        }

        public async Task<BinaryData> DownloadContentAsync(CancellationToken cancellationToken)
        {
            Response<BlobDownloadResult> download = await _blobClient.DownloadContentAsync(cancellationToken);
            return download.Value.Content;
        }

        public async Task UploadAsync(BinaryData content, bool overwrite, CancellationToken cancellationToken)
        {
            await _blobClient.UploadAsync(content, overwrite, cancellationToken);
        }
    }
}
