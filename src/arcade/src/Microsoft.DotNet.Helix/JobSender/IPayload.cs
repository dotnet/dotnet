// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Helix.Client
{
    internal interface IPayload
    {
        Task<string> UploadAsync(IBlobContainer payloadContainer, Action<string> log, CancellationToken cancellationToken);
    }
}
