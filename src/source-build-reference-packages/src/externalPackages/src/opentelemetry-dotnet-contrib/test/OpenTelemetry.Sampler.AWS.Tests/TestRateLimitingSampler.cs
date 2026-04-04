// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Sampler.AWS.Tests;

public class TestRateLimitingSampler
{
    [Fact]
    public void TestLimitsRate()
    {
        var clock = new TestClock();
        Trace.Sampler sampler = new RateLimitingSampler(1, clock);

        Assert.Equal(SamplingDecision.RecordAndSample, sampler.ShouldSample(Utils.CreateSamplingParameters()).Decision);

        // balance used up
        Assert.Equal(SamplingDecision.Drop, sampler.ShouldSample(Utils.CreateSamplingParameters()).Decision);

        // balance restore after 1 second, not yet
        clock.Advance(TimeSpan.FromMilliseconds(100));
        Assert.Equal(SamplingDecision.Drop, sampler.ShouldSample(Utils.CreateSamplingParameters()).Decision);

        // balance restored
        clock.Advance(TimeSpan.FromMilliseconds(900));
        Assert.Equal(SamplingDecision.RecordAndSample, sampler.ShouldSample(Utils.CreateSamplingParameters()).Decision);
    }
}
