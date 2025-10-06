// <copyright file="IAsyncFlushable.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a task based flush for sending telemetry to application insights.
    /// </summary>
    public interface IAsyncFlushable : IDisposable
    {
        /// <summary>
        /// Flushes the in-memory buffer asynchronously.
        /// </summary>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns>
        /// Returns true when telemetry data is transferred out of process (application insights server or local storage) and are emitted before the flush invocation.
        /// Returns false when transfer of telemetry data to server has failed with non-retriable http status code.
        /// </returns>
        Task<bool> FlushAsync(CancellationToken cancellationToken);
    }
}
