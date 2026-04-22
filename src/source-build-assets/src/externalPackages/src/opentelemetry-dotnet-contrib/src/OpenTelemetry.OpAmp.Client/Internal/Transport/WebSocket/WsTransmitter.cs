// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Net.WebSockets;
using Google.Protobuf;
using OpenTelemetry.Internal;
using OpenTelemetry.OpAmp.Client.Internal.Utils;

namespace OpenTelemetry.OpAmp.Client.Internal.Transport.WebSocket;

internal sealed class WsTransmitter
{
    private const int BufferSize = 4096;

    private readonly byte[] buffer = new byte[BufferSize];

    private readonly ClientWebSocket ws;

    public WsTransmitter(ClientWebSocket ws)
    {
        Guard.ThrowIfNull(ws, nameof(ws));

        this.ws = ws;
    }

    public async Task SendAsync(IMessage message, CancellationToken token = default)
    {
        var headerSize = OpAmpWsHeaderHelper.WriteHeader(new ArraySegment<byte>(this.buffer));
        var size = message.CalculateSize();

        // fits to the buffer, send completely
        if (size + headerSize <= BufferSize)
        {
            var segment = new ArraySegment<byte>(this.buffer, headerSize, size);
            message.WriteTo(segment);

            // resegment to include the header byte
            segment = new ArraySegment<byte>(this.buffer, 0, size + headerSize);

            await this.ws.SendAsync(segment, WebSocketMessageType.Binary, true, token).ConfigureAwait(false);
        }

        // Does not fit, need to chunk the message
        else
        {
            // It's expected that large messages are created very rarely by the client.
            var messageBuffer = message.ToByteArray();
            var frameBuffer = new byte[headerSize + size];
            var offset = 0;

            // Copy the already written header
            Buffer.BlockCopy(this.buffer, 0, frameBuffer, 0, headerSize);

            // Copy the message
            Buffer.BlockCopy(messageBuffer, 0, frameBuffer, headerSize, size);

            while (true)
            {
                token.ThrowIfCancellationRequested();

                var count = frameBuffer.Length - offset < BufferSize
                    ? frameBuffer.Length - offset
                    : BufferSize;

                var segment = new ArraySegment<byte>(frameBuffer, offset, count);
                var isEnd = (offset + count) == frameBuffer.Length;
                await this.ws.SendAsync(segment, WebSocketMessageType.Binary, isEnd, token).ConfigureAwait(false);

                if (isEnd)
                {
                    break;
                }

                offset += count;
            }
        }
    }
}
