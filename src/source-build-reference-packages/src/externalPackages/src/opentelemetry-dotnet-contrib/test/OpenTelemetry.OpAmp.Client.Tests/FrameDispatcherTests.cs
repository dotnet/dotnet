// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET

using OpenTelemetry.OpAmp.Client.Internal;
using OpenTelemetry.OpAmp.Client.Settings;
using OpenTelemetry.OpAmp.Client.Tests.Mocks;
using Xunit;

namespace OpenTelemetry.OpAmp.Client.Tests;

public class FrameDispatcherTests
{
    [Fact]
    public async Task FrameDispatcher_IsThreadSafe_WhenDispatchingConcurrently()
    {
        // Simulate concurrent dispatches
        var taskCount = 20; // Number of concurrent tasks

        var transport = new MockTransport(taskCount);
        var settings = new OpAmpClientSettings();
        var dispatcher = new FrameDispatcher(transport, settings);

        await Parallel.ForEachAsync(Enumerable.Range(0, taskCount), async (i, token) =>
        {
            if (i % 2 == 0)
            {
                await dispatcher.DispatchIdentificationAsync(token);
            }
            else
            {
                await dispatcher.DispatchIdentificationAsync(token);
            }
        });

        // Assert that all messages were sent without exceptions
        Assert.Equal(taskCount, transport.Messages.Count);

        // Assert that sequence numbers are from 1 to N (no duplicates, no gaps)
        var sequenceNumbers = transport.Messages
            .Select(m => m.SequenceNum)
            .ToArray();
        Assert.Equal(Enumerable.Range(1, taskCount).Select(i => (ulong)i), sequenceNumbers);
    }
}
#endif
