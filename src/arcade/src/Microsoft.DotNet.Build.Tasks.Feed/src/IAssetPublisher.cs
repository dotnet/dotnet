// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
#if !NET472_OR_GREATER
using Microsoft.DotNet.ProductConstructionService.Client.Models;
#endif
using Task = System.Threading.Tasks.Task;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public interface IAssetPublisher
    {
#if !NET472_OR_GREATER
        LocationType LocationType { get; }
#endif

        Task PublishAssetAsync(string file, string blobPath, PushOptions options, SemaphoreSlim clientThrottle = null);
    }
}

