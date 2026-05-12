// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Buffers;
using System.Net.WebSockets;
using OpenTelemetry.Internal;
using OpenTelemetry.OpAmp.Client.Internal.Utils;

namespace OpenTelemetry.OpAmp.Client.Internal.Transport.WebSocket;

/// <summary>
/// Reads OpAMP WebSocket messages from the server and dispatches them to the frame processor.
/// </summary>
internal sealed class WsReceiver : IDisposable
{
    private const int RentalBufferSize = 4 * 1024; // 4 KB
    private const int ReceiveBufferSize = 8 * 1024; // 8 KB

    private readonly ClientWebSocket ws;
    private readonly FrameProcessor processor;
    private readonly CancellationTokenSource disposeTokenSource = new();

    private readonly byte[] receiveBuffer = new byte[ReceiveBufferSize];

    private CancellationTokenSource? linkedReceiveTokenSource;
    private Task? receiveTask;
    private bool disposed;

    public WsReceiver(ClientWebSocket ws, FrameProcessor processor)
    {
        Guard.ThrowIfNull(ws, nameof(ws));
        Guard.ThrowIfNull(processor, nameof(processor));

        this.ws = ws;
        this.processor = processor;
    }

    public void Start(CancellationToken token = default)
    {
#if NET8_0_OR_GREATER
        ObjectDisposedException.ThrowIf(this.disposed, this);
#else
        if (this.disposed)
        {
            throw new ObjectDisposedException(nameof(WsReceiver));
        }
#endif

        if (this.receiveTask != null)
        {
            throw new InvalidOperationException("The WebSocket receiver has already been started.");
        }

        this.linkedReceiveTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, this.disposeTokenSource.Token);
        this.receiveTask = this.ReceiveLoopAsync(this.linkedReceiveTokenSource.Token);
    }

    /// <summary>
    /// Releases resources used by the receiver.
    /// </summary>
    /// <remarks>
    /// This method blocks until the background receive task has finished so pooled receive buffers
    /// are always returned and the <see cref="ClientWebSocket"/> is not used concurrently after
    /// disposal. <see cref="WsTransport.Dispose"/> aborts the socket before calling this method, so
    /// the wait is usually brief. For cooperative shutdown without blocking here, cancel the token
    /// passed to <see cref="Start"/> and complete a graceful close on the transport before disposing.
    /// </remarks>
    public void Dispose()
    {
        if (this.disposed)
        {
            return;
        }

        this.disposed = true;
        this.disposeTokenSource.Cancel();

        try
        {
            // Synchronous wait: see remarks on this method. WsTransport.Dispose aborts the socket
            // before disposing the receiver, and the receive path uses ConfigureAwait(false).
            this.receiveTask?.GetAwaiter().GetResult();
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            // Swallow any other fault so Dispose() does not throw, per IDisposable contract.
            // This can happen if, for example, the frame processor throws while handling a frame.
            OpAmpClientEventSource.Log.TransportCloseException(ex);
        }
        finally
        {
            this.linkedReceiveTokenSource?.Dispose();
            this.disposeTokenSource.Dispose();
        }
    }

    private static void StopReading(out bool continueRead, out bool isClosed)
    {
        continueRead = false;
        isClosed = true;
    }

    private static void ReturnRentalBuffers(List<byte[]>? rentalBuffers)
    {
        if (rentalBuffers == null)
        {
            return;
        }

        // Skip index 0 because that is always the non-pooled receiveBuffer field.
        for (var i = 1; i < rentalBuffers.Count; i++)
        {
            ArrayPool<byte>.Shared.Return(rentalBuffers[i]);
        }
    }

    private async Task ReceiveLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested && this.ws.State == WebSocketState.Open)
        {
            await this.ReceiveAsync(token).ConfigureAwait(false);
        }
    }

    private async Task ReceiveAsync(CancellationToken token)
    {
        var totalCount = 0;
        var workingCount = 0;
        WebSocketReceiveResult result;
        var workingBuffer = this.receiveBuffer;

        List<byte[]>? rentalBuffers = null;
        bool continueRead = true;
        bool isClosed = false;

        do
        {
            // out of space, need to rent more
            if (workingBuffer.Length - workingCount == 0)
            {
                rentalBuffers ??= [this.receiveBuffer];

                workingBuffer = ArrayPool<byte>.Shared.Rent(RentalBufferSize);
                workingCount = 0;
                rentalBuffers.Add(workingBuffer);
            }

            if (this.ws.State != WebSocketState.Open)
            {
                // Connection is closed, nothing more to read.
                isClosed = true;
                break;
            }

            var segment1 = new ArraySegment<byte>(workingBuffer, workingCount, workingBuffer.Length - workingCount);

            try
            {
                result = await this.ws
                    .ReceiveAsync(segment1, token)
                    .ConfigureAwait(false);

                continueRead = !result.EndOfMessage;
                isClosed = result.CloseStatus != null;
                workingCount += result.Count;
                totalCount += result.Count;
            }
            catch (OperationCanceledException)
            {
                StopReading(out continueRead, out isClosed);
            }
            catch (Exception ex) when (ex is WebSocketException || ex is ObjectDisposedException)
            {
                StopReading(out continueRead, out isClosed);
            }

            // Reject only after a receive pushes past the limit (see TransportConstants.MaxMessageSize remarks).
            if (totalCount > TransportConstants.MaxMessageSize)
            {
                OpAmpClientEventSource.Log.OversizedWebSocketMessageReceived(totalCount, TransportConstants.MaxMessageSize);

                // Message too large, abort the connection.
                try
                {
                    await this.ws
                        .CloseOutputAsync(WebSocketCloseStatus.MessageTooBig, "Message too large", CancellationToken.None)
                        .ConfigureAwait(false);
                }
                catch (Exception ex) when (ex is WebSocketException || ex is ObjectDisposedException)
                {
                    OpAmpClientEventSource.Log.TransportCloseException(ex);
                }

                isClosed = true;
                break;
            }
        }
        while (continueRead && !token.IsCancellationRequested);

        try
        {
            if (!isClosed)
            {
                var sequence =
                    rentalBuffers?.Count > 1
                        ? rentalBuffers.CreateSequenceFromBuffers(workingCount)
                        : new ReadOnlySequence<byte>(this.receiveBuffer, 0, totalCount);

                this.processor.OnServerFrame(sequence, totalCount, verifyHeader: true);
            }
        }
        catch (Exception ex)
        {
            // Frame deserialization or listener dispatch failed. Log and continue the
            // receive loop so a single bad frame does not kill the long-lived connection.
            OpAmpClientEventSource.Log.FrameProcessingException(ex);
        }
        finally
        {
            ReturnRentalBuffers(rentalBuffers);
        }
    }
}
