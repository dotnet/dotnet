// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace Microsoft.DotNet.Helix.Client
{

    internal class ConnectionStringBlobHelper : IBlobHelper
    {
        private readonly string _connectionString;

        public ConnectionStringBlobHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IBlobContainer> GetContainerAsync(string requestedName, string targetQueue, CancellationToken cancellationToken)
        {
            BlobServiceClient account = new BlobServiceClient(_connectionString, StorageRetryPolicy.GetBlobClientOptionsRetrySettings());

            BlobContainerClient container = account.GetBlobContainerClient(requestedName);
            await container.CreateIfNotExistsAsync();
            return new Container(container);
        }

        private class Container : ContainerBase
        {
            private readonly BlobContainerClient _container;

            public Container(BlobContainerClient container)
            {
                _container = container;
            }

            public override string Uri => _container.Uri.ToString();
            public override string ReadSas => GetSasTokenForPermissions(BlobContainerSasPermissions.Read, DateTime.UtcNow.AddDays(30));
            public override string WriteSas => GetSasTokenForPermissions(BlobContainerSasPermissions.Write | 
                                                                         BlobContainerSasPermissions.Read,
                                                                         DateTime.UtcNow.AddDays(30));

            private string GetSasTokenForPermissions(BlobContainerSasPermissions permissions, DateTime expiration)
            {
                string sas = _container.GenerateSasUri(permissions, expiration).ToString();
                return sas.Substring(sas.IndexOf('?'));
            }

            protected override (BlobClient blob, string sasToken) GetBlob(string blobName)
            {
                string sasToken = GetSasTokenForPermissions(BlobContainerSasPermissions.Read, DateTime.UtcNow.AddDays(30));
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
