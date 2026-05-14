// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Helix.Client
{

    internal interface IBlobContainer
    {
        Task<Uri> UploadFileAsync(Stream stream, string blobName, Action<string> log, CancellationToken cancellationToken);
        Task<Uri> UploadTextAsync(string text, string blobName, Action<string> log, CancellationToken cancellationToken);
        string Uri { get; }
        string ReadSas { get; }
        string WriteSas { get; }
    }
}
