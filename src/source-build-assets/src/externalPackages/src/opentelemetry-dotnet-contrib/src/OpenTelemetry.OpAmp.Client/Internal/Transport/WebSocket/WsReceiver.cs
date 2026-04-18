// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Buffers;
using System.Net.WebSockets;
using OpenTelemetry.Internal;
using OpenTelemetry.OpAmp.Client.Internal.Utils;

namespace OpenTelemetry.OpAmp.Client.Internal.Transport.WebSocket;

internal sealed class WsReceiver : IDisposable
{
    private const int RentalBufferSize = 4 * 1024; // 4 KB
    private const int ReceiveBufferSize = 8 * 1024; // 8 KB
    private const int MaxMessageSize = 128 * 1024; // 128 KB

    private readonly ClientWebSocket ws;
    private readonly Thread receiveThread;
    private readonly FrameProcessor processor;

    private readonly byte[] receiveBuffer = new byte[ReceiveBufferSize];

    private CancellationToken token;

    public WsReceiver(ClientWebSocket ws, FrameProcessor processor)
    {
        Guard.ThrowIfNull(ws, nameof(ws));
        Guard.ThrowIfNull(processor, nameof(processor));

        this.ws = ws;
        this.processor = processor;
        this.receiveThread = new Thread(this.ReceiveLoop)
        {
            Name = "OpAmp WebSocket Receive Loop",
        };
    }

    public void Start(CancellationToken token = default)
    {
        this.token = token;
        this.receiveThread.Start();
    }

    public void Dispose()
    {
        this.receiveThread?.Join();
    }

    private static void ReturnRentalBuffers(List<byte[]>? rentalBuffers)
    {
        if (rentalBuffers == null)
        {
            return;
        }

        foreach (var rental in rentalBuffers)
        {
            ArrayPool<byte>.Shared.Return(rental);
        }
    }

    private async void ReceiveLoop()
    {
        while (!this.token.IsCancellationRequested && this.ws.State == WebSocketState.Open)
        {
            await this.ReceiveAsync().ConfigureAwait(false);
        }
    }

    private async Task ReceiveAsync()
    {
        var totalCount = 0;
        var workingCount = 0;
        WebSocketReceiveResult result;
        byte[] workingBuffer = this.receiveBuffer;

        List<byte[]>? rentalBuffers = null;
        bool continueRead;
        bool isClosed;

        do
        {
            // out of space, need to rent more
            if (workingBuffer.Length - workingCount == 0)
            {
                if (rentalBuffers == null)
                {
                    rentalBuffers = [this.receiveBuffer];
                }

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
                    .ReceiveAsync(segment1, this.token)
                    .ConfigureAwait(false);

                continueRead = !result.EndOfMessage;
                isClosed = result.CloseStatus != null;
                workingCount += result.Count;
                totalCount += result.Count;
            }
            catch (OperationCanceledException)
            {
                continueRead = false;
                isClosed = true;
            }

            if (totalCount > MaxMessageSize)
            {
                // Message too large, abort the connection.
                await this.ws
                    .CloseOutputAsync(WebSocketCloseStatus.MessageTooBig, "Message too large", CancellationToken.None)
                    .ConfigureAwait(false);

                isClosed = true;
                break;
            }
        }
        while (continueRead && !this.token.IsCancellationRequested);

        if (!isClosed)
        {
            var sequence =
                rentalBuffers?.Count > 1
                    ? rentalBuffers.CreateSequenceFromBuffers(workingCount + 1)
                    : new ReadOnlySequence<byte>(this.receiveBuffer);

            this.processor.OnServerFrame(sequence, totalCount, verifyHeader: true);
        }

        ReturnRentalBuffers(rentalBuffers);
    }
}
