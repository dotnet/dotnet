// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    internal interface IBlobClientFactory
    {
        IBlobClient CreateBlobClient(string blobUri, string sasToken = null);

        IBlobClient CreateBlobClient(Uri containerUri, string blobName, string sasToken);
    }
}
