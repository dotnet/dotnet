// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text;
using OpenTelemetry.OpAmp.Client.Internal;
using OpenTelemetry.OpAmp.Client.Internal.Transport.WebSocket;
using OpenTelemetry.OpAmp.Client.Tests.Mocks;
using OpenTelemetry.OpAmp.Client.Tests.Tools;
using Xunit;

namespace OpenTelemetry.OpAmp.Client.Tests;

public class WsTransportTest
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task WsTransport_SendReceiveCommunication(bool useSmallPackets)
    {
        // Arrange
        using var opAmpServer = new OpAmpFakeWebSocketServer(useSmallPackets);
        var opAmpEndpoint = opAmpServer.Endpoint;

        using var mockListener = new MockListener();
        var frameProcessor = new FrameProcessor();
        frameProcessor.Subscribe(mockListener);

        using var cts = new CancellationTokenSource();
        using var wsTransport = new WsTransport(opAmpEndpoint, frameProcessor);
        await wsTransport.StartAsync(cts.Token);

        var mockFrame = FrameGenerator.GenerateMockAgentFrame(useSmallPackets);

        // Act
        await wsTransport.SendAsync(mockFrame.Frame, cts.Token);

        mockListener.WaitForMessages(TimeSpan.FromSeconds(30));

        await wsTransport.StopAsync(cts.Token);

        cts.Cancel();

        // Assert
        var serverReceivedFrames = opAmpServer.GetFrames();
        var clientReceivedFrames = mockListener.Messages;
        var receivedTextData =
#if NET
            Encoding.UTF8.GetString(clientReceivedFrames.First().Data);
#else
            Encoding.UTF8.GetString([.. clientReceivedFrames.First().Data]);
#endif

        Assert.Single(serverReceivedFrames);
        Assert.Equal(mockFrame.Uid, serverReceivedFrames.First().InstanceUid);

        Assert.Single(clientReceivedFrames);
        Assert.StartsWith("This is a mock server frame for testing purposes.", receivedTextData);
    }
}
