// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol
{
    /// <summary>
    /// An interface used for throttling operations. For example, suppose the application needs to
    /// limit the concurrency of HTTP operations. Before executing each HTTP operation, the
    /// <see cref="WaitAsync"/> would be executed. After the HTTP operation has been completed, the
    /// application should call <see cref="Release"/>. The implementation of <see cref="WaitAsync"/>
    /// should only allow the application to continue if there is an appropriate number of concurrent
    /// callers. The primary implementation of this interface simply wraps a <see cref="SemaphoreSlim"/>.
    /// </summary>
    public interface IThrottle
    {
        /// <summary>
        /// Waits until an appropriate level of concurrency has been reached before allowing the
        /// caller to continue.
        /// </summary>
        Task WaitAsync();

        /// <summary>
        /// Signals that the throttled operation has been completed and other threads can being
        /// their own throttled operation.
        /// </summary>
        void Release();
    }
}
