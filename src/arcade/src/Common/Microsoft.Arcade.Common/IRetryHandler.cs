// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Arcade.Common
{
    public interface IRetryHandler
    {
        Task<bool> RunAsync(
            Func<int, Task<bool>> actionSuccessfulAsync);

        Task<bool> RunAsync(
            Func<int, Task<bool>> actionSuccessfulAsync,
            CancellationToken cancellationToken);
    }
}
