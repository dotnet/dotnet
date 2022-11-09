// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Microsoft.DotNet.Helix.Client.Models;

namespace Microsoft.DotNet.Helix.Client
{

    internal class ApiBlobHelper : IBlobHelper
    {
        private readonly IStorage _helixApiStorage;

        public ApiBlobHelper(IStorage helixApiStorage)
        {
            _helixApiStorage = helixApiStorage;
        }

        public async Task<IBlobContainer> GetContainerAsync(string requestedName, string targetQueue, CancellationToken cancellationToken)
        {
            ContainerInformation info = await _helixApiStorage.NewAsync(new ContainerCreationRequest(30, requestedName, targetQueue), cancellationToken).ConfigureAwait(false);
            Uri containerUri = new Uri($"https://{info.StorageAccountName}.blob.core.windows.net/{info.ContainerName}");
            AzureSasCredential creds = new AzureSasCredential(info.WriteToken);
            var container = new BlobContainerClient(containerUri, creds, StorageRetryPolicy.GetBlobClientOptionsRetrySettings());
            return new Container(container, info);
        }

        private class Container : ContainerBase
        {
            private readonly BlobContainerClient _container;
            private readonly ContainerInformation _info;

            public Container(BlobContainerClient container, ContainerInformation info)
            {
                _container = container;
                _info = info;
            }

            public override string Uri => _container.Uri.ToString();
            public override string ReadSas => _info.ReadToken;
            public override string WriteSas => _info.WriteToken;

            protected override (BlobClient blob, string sasToken) GetBlob(string blobName)
            {
                string sasToken = _info.ReadToken;
                if (sasToken.StartsWith("?"))
                {
                    sasToken = sasToken.Substring(1);
                }

                BlobClient blob = _container.GetBlobClient(blobName);
                return (blob, sasToken);
            }
        }
    }
}
