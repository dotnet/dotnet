// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Buffers;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using OpAmp.Proto.V1;
using OpenTelemetry.OpAmp.Client.Internal.Utils;
using OpenTelemetry.Tests;

namespace OpenTelemetry.OpAmp.Client.Tests.Tools;

internal class OpAmpFakeWebSocketServer : IDisposable
{
    private readonly IDisposable httpServer;
    private readonly BlockingCollection<AgentToServer> frames = [];
    private readonly CancellationTokenSource cts = new();

    public OpAmpFakeWebSocketServer(bool useSmallPackets)
    {
        this.httpServer = TestWebSocketServer.RunServer(
            async socket =>
            {
                var buffer = new byte[8 * 1024];
                using var ms = new MemoryStream();

                try
                {
                    while (socket.State == WebSocketState.Open)
                    {
                        var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), this.cts.Token);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", this.cts.Token);

                            break;
                        }

                        if (result.Count > 0)
                        {
                            ms.Write(buffer, 0, result.Count);
                        }

                        if (result.EndOfMessage)
                        {
                            var frame = ProcessReceive(ms);
                            this.frames.Add(frame);

                            var response = GenerateResponse(frame, useSmallPackets);
                            await socket.SendAsync(response, WebSocketMessageType.Binary, true, this.cts.Token).ConfigureAwait(false);
                        }
                    }
                }
                finally
                {
                    if (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived)
                    {
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server disposed", this.cts.Token);
                    }
                }
            },
            out var host,
            out var port);

        this.Endpoint = new Uri($"ws://{host}:{port}/v1/opamp");
    }

    public Uri Endpoint { get; }

    public IReadOnlyCollection<AgentToServer> GetFrames()
    {
        return this.frames.ToArray();
    }

    public void Dispose()
    {
        this.cts.Cancel();
        this.cts.Dispose();
        this.httpServer.Dispose();
    }

    private static AgentToServer ProcessReceive(MemoryStream ms)
    {
        var fullMessageBytes = new ReadOnlySequence<byte>(ms.ToArray());
        bool result = OpAmpWsHeaderHelper.TryVerifyHeader(fullMessageBytes, out var headerSize, out string errorMessage);
        if (!result)
        {
            throw new InvalidOperationException(errorMessage);
        }

        var messageBytes = fullMessageBytes.Slice(headerSize, fullMessageBytes.Length - headerSize);
        ms.SetLength(0);

        // Parse protobuf message from the websocket message bytes.
        var frame = AgentToServer.Parser.ParseFrom(messageBytes);

        return frame;
    }

    private static ArraySegment<byte> GenerateResponse(AgentToServer frame, bool useSmallPackets)
    {
        var response = FrameGenerator.GenerateMockServerFrame(frame.InstanceUid, useSmallPackets, addHeader: true);

        return response.Frame;
    }
}
