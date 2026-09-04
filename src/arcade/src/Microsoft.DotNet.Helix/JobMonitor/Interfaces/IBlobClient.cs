// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    internal interface IBlobClient
    {
        Uri Uri { get; }

        Task DownloadToAsync(string destinationFile, CancellationToken cancellationToken);

        Task<BinaryData> DownloadContentAsync(CancellationToken cancellationToken);

        Task UploadAsync(BinaryData content, bool overwrite, CancellationToken cancellationToken);
    }
}
