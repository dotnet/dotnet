// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// An awaitable that is returned from asynchronous lock requests.
    /// </summary>
    public interface IAsyncLockAwaitable
    {
        /// <summary>
        /// Gets the awaiter value.
        /// </summary>
        AsyncLockAwaiter GetAwaiter();
    }
}
