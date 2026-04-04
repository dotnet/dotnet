// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Net.Http;
#endif

using System.Text;
using OpenTelemetry.OpAmp.Client.Internal;
using OpenTelemetry.OpAmp.Client.Internal.Transport.Http;
using OpenTelemetry.OpAmp.Client.Settings;
using OpenTelemetry.OpAmp.Client.Tests.Mocks;
using OpenTelemetry.OpAmp.Client.Tests.Tools;
using Xunit;

namespace OpenTelemetry.OpAmp.Client.Tests;

public class PlainHttpTransportTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task PlainHttpTransport_SendReceiveCommunication(bool useSmallPackets)
    {
        // Arrange
        using var opAmpServer = new OpAmpFakeHttpServer(useSmallPackets);
        var opAmpEndpoint = opAmpServer.Endpoint;
        var settings = new OpAmpClientSettings { ServerUrl = opAmpEndpoint };

        using var mockListener = new MockListener();
        var frameProcessor = new FrameProcessor();
        frameProcessor.Subscribe(mockListener);

        var httpTransport = new PlainHttpTransport(settings, frameProcessor);

        var mockFrame = FrameGenerator.GenerateMockAgentFrame(useSmallPackets);

        // Act
        await httpTransport.SendAsync(mockFrame.Frame, CancellationToken.None);

        // Assert
        var serverReceivedFrames = opAmpServer.GetFrames();
        var clientReceivedFrames = mockListener.Messages;
        var receivedTextData =
#if NET
            Encoding.UTF8.GetString(clientReceivedFrames.First().Data);
#else
            Encoding.UTF8.GetString([.. clientReceivedFrames.First().Data]);
#endif

        var frame = Assert.Single(serverReceivedFrames);
        Assert.Equal(mockFrame.Uid, frame.InstanceUid);

        Assert.Single(clientReceivedFrames);
        Assert.StartsWith("This is a mock server frame for testing purposes.", receivedTextData);
    }

    [Fact]
    public async Task PlainHttpTransport_UsesConfiguredHttpClientFactory()
    {
        // Arrange
        using var opAmpServer = new OpAmpFakeHttpServer(false);
        var opAmpEndpoint = opAmpServer.Endpoint;
        var settings = new OpAmpClientSettings
        {
            ServerUrl = opAmpEndpoint,
            HttpClientFactory = () =>
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-Custom-Header", "CustomValue");
                return client;
            },
        };

        using var mockListener = new MockListener();
        var frameProcessor = new FrameProcessor();
        frameProcessor.Subscribe(mockListener);

        var httpTransport = new PlainHttpTransport(settings, frameProcessor);
        var mockFrame = FrameGenerator.GenerateMockAgentFrame(false);

        // Act
        await httpTransport.SendAsync(mockFrame.Frame, CancellationToken.None);

        // Assert
        var serverReceivedHeaders = opAmpServer.GetHeaders();
        Assert.Contains(serverReceivedHeaders, headers => headers["X-Custom-Header"] == "CustomValue");
    }
}
