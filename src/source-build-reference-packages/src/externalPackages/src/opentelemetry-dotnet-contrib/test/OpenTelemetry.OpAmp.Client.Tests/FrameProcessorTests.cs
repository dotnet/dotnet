// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text;
using OpenTelemetry.OpAmp.Client.Internal;
using OpenTelemetry.OpAmp.Client.Tests.Mocks;
using OpenTelemetry.OpAmp.Client.Tests.Tools;
using Xunit;

namespace OpenTelemetry.OpAmp.Client.Tests;

public class FrameProcessorTests
{
    [Fact]
    public void FrameProcessor_Subscribe()
    {
        using var listener = new MockListener();
        var processor = new FrameProcessor();
        var mockFrame = FrameGenerator.GenerateMockServerFrame();

        processor.Subscribe(listener);
        processor.OnServerFrame(mockFrame.Frame.ToSequence());

        var message = Assert.Single(listener.Messages);
        var messageContent =
#if NET
            Encoding.UTF8.GetString(message.Data);
#else
            Encoding.UTF8.GetString([.. message.Data]);
#endif

        Assert.Equal(mockFrame.ExptectedContent, messageContent);
    }

    [Fact]
    public void FrameProcessor_Unsubscribe()
    {
        using var listener = new MockListener();
        var processor = new FrameProcessor();
        var mockFrame = FrameGenerator.GenerateMockServerFrame();

        processor.Subscribe(listener);
        processor.OnServerFrame(mockFrame.Frame.ToSequence());

        Assert.Single(listener.Messages);

        processor.Unsubscribe(listener);
        processor.OnServerFrame(mockFrame.Frame.ToSequence());
        Assert.Single(listener.Messages);
    }

    [Fact]
    public async Task FrameProcessor_ThreadSafety()
    {
        using var listener = new MockListener();
        var processor = new FrameProcessor();
        var mockFrame = FrameGenerator.GenerateMockServerFrame();
        int iterations = 1000;
        var tasks = new List<Task>
        {
            // Task to repeatedly call OnServerFrame
            Task.Run(() =>
            {
                Parallel.For(0, iterations, i =>
                {
                    processor.OnServerFrame(mockFrame.Frame.ToSequence());
                });
            }),

            // Task to repeatedly subscribe
            Task.Run(() =>
            {
                Parallel.For(0, iterations, i =>
                {
                    processor.Subscribe(listener);
                });
            }),

            // Task to repeatedly unsubscribe
            Task.Run(() =>
            {
                Parallel.For(0, iterations, i =>
                {
                    processor.Unsubscribe(listener);
                });
            }),
        };

        await Task.WhenAll(tasks);

        // After all operations, ensure no exceptions and listener.Messages is in a valid state
        Assert.True(listener.Messages.Count >= 0);
    }
}
