// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.OpAmp.Client.Internal;
using OpenTelemetry.OpAmp.Client.Internal.Services.Heartbeat;
using OpenTelemetry.OpAmp.Client.Settings;
using OpenTelemetry.OpAmp.Client.Tests.Mocks;
using Xunit;

namespace OpenTelemetry.OpAmp.Client.Tests;

public class HeartbeatServiceTests
{
    [Fact]
    public void HeartbeatService_EmitsHeartbeats()
    {
        const int messagesCount = 3;
        const int intervalMs = 300;

        var settings = new OpAmpClientSettings
        {
            Heartbeat = new HeartbeatSettings
            {
                Interval = TimeSpan.FromMilliseconds(intervalMs), // Set a short interval for testing
            },
        };

        using var transport = new MockTransport(messagesCount);
        using var dispatcher = new FrameDispatcher(transport, settings);
        using var service = new HeartbeatService(dispatcher, new FrameProcessor());

        service.Configure(settings);
        service.Start();

        transport.WaitForMessages(timeout: TimeSpan.FromSeconds(5));

        service.Stop();

        var count = transport.Messages.Count;
        Assert.True(count >= messagesCount, $"Expecting at least {messagesCount} heartbeats, got {count}.");
    }
}
