// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpAmp.Proto.V1;
using OpenTelemetry.OpAmp.Client.Internal;
using OpenTelemetry.OpAmp.Client.Tests.DataGenerators;
using Xunit;

namespace OpenTelemetry.OpAmp.Client.Tests;

public class FrameBuilderTests
{
    [Fact]
    public void FrameBuilder_InitializesCorrectly()
    {
        var frameBuilder = new FrameBuilder(new());

        var frame = frameBuilder
            .StartBaseMessage()
            .Build();

        Assert.NotNull(frame);
        Assert.NotEmpty(frame.InstanceUid);
        Assert.Equal(1UL, frame.SequenceNum);
    }

    [Fact]
    public void FrameBuilder_Sequence()
    {
        var frameBuilder = new FrameBuilder(new());

        var frame1 = frameBuilder
            .StartBaseMessage()
            .Build();

        var frame2 = frameBuilder
            .StartBaseMessage()
            .Build();

        var frame3 = frameBuilder
            .StartBaseMessage()
            .Build();

        Assert.Equal(1UL, frame1.SequenceNum);
        Assert.Equal(2UL, frame2.SequenceNum);
        Assert.Equal(3UL, frame3.SequenceNum);
    }

    [Fact]
    public void FrameBuilder_ThrowsOnDoubleStartBaseMessage()
    {
        var frameBuilder = new FrameBuilder(new());
        frameBuilder.StartBaseMessage();

        Assert.Throws<InvalidOperationException>(frameBuilder.StartBaseMessage);
    }

    [Theory]
    [ClassData(typeof(FrameBuilderTestData))]
    internal void FrameBuilder_AddPartial(Func<IFrameBuilder, IFrameBuilder> addMessage, Func<AgentToServer, object> propertyFetcher)
    {
        var frameBuilder = new FrameBuilder(new());
        var messageBuilder = frameBuilder.StartBaseMessage();
        addMessage(messageBuilder);

        var message = messageBuilder.Build();
        var property = propertyFetcher(message);

        Assert.NotNull(property);
    }
}
