// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Grpc.Core;

namespace OpenTelemetry.Instrumentation.GrpcCore;

/// <summary>
/// A proxy stream reader with callbacks for interesting events.
/// </summary>
/// <remarks>
/// Borrowed heavily from
/// https://github.com/opentracing-contrib/csharp-grpc/blob/master/src/OpenTracing.Contrib.Grpc/Streaming/TracingAsyncStreamReader.cs.
/// </remarks>
/// <typeparam name="T">The message type.</typeparam>
/// <seealso cref="IAsyncStreamReader{T}" />
internal class AsyncStreamReaderProxy<T> : IAsyncStreamReader<T>
{
    /// <summary>
    /// The reader.
    /// </summary>
    private readonly IAsyncStreamReader<T> reader;

    /// <summary>
    /// The on message action.
    /// </summary>
    private readonly Action<T>? onMessage;

    /// <summary>
    /// The on stream end action.
    /// </summary>
    private readonly Action? onStreamEnd;

    /// <summary>
    /// The on exception action.
    /// </summary>
    private readonly Action<Exception>? onException;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncStreamReaderProxy{T}"/> class.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="onMessage">The on message action, if any.</param>
    /// <param name="onStreamEnd">The on stream end action, if any.</param>
    /// <param name="onException">The on exception action, if any.</param>
    public AsyncStreamReaderProxy(IAsyncStreamReader<T> reader, Action<T>? onMessage = null, Action? onStreamEnd = null, Action<Exception>? onException = null)
    {
        this.reader = reader;
        this.onMessage = onMessage;
        this.onStreamEnd = onStreamEnd;
        this.onException = onException;
    }

    /// <inheritdoc/>
    public T Current => this.reader.Current;

    /// <inheritdoc/>
    public async Task<bool> MoveNext(CancellationToken cancellationToken)
    {
        try
        {
            var hasNext = await this.reader.MoveNext(cancellationToken).ConfigureAwait(false);
            if (hasNext)
            {
                this.onMessage?.Invoke(this.Current);
            }
            else
            {
                this.onStreamEnd?.Invoke();
            }

            return hasNext;
        }
        catch (Exception ex)
        {
            this.onException?.Invoke(ex);
            throw;
        }
    }
}
