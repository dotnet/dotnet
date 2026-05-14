// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Grpc.Core;

namespace OpenTelemetry.Instrumentation.GrpcCore;

/// <summary>
/// A proxy server stream writer.
/// </summary>
/// <remarks>
/// Borrowed heavily from
/// https://github.com/opentracing-contrib/csharp-grpc/blob/master/src/OpenTracing.Contrib.Grpc/Streaming/TracingServerStreamWriter.cs.
/// </remarks>
/// <typeparam name="T">The message type.</typeparam>
/// <seealso cref="IServerStreamWriter{T}" />
internal class ServerStreamWriterProxy<T> : IServerStreamWriter<T>
{
    /// <summary>
    /// The writer.
    /// </summary>
    private readonly IServerStreamWriter<T> writer;

    /// <summary>
    /// The on write action.
    /// </summary>
    private readonly Action<T>? onWrite;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerStreamWriterProxy{T}"/> class.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="onWrite">The on write action, if any.</param>
    public ServerStreamWriterProxy(IServerStreamWriter<T> writer, Action<T>? onWrite = null)
    {
        this.writer = writer;
        this.onWrite = onWrite;
    }

    /// <inheritdoc/>
    public WriteOptions? WriteOptions
    {
        get => this.writer.WriteOptions;
        set => this.writer.WriteOptions = value;
    }

    /// <inheritdoc/>
    public Task WriteAsync(T message)
    {
        this.onWrite?.Invoke(message);
        return this.writer.WriteAsync(message);
    }
}
