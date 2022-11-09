// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;


namespace Microsoft.DotNet.Helix.Client
{
    internal interface IBlobHelper
    {
        Task<IBlobContainer> GetContainerAsync(string requestedName, string targetQueue, CancellationToken cancellationToken);
    }
}
