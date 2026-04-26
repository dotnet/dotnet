// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Trace;

namespace OpenTelemetry.Sampler.AWS;

internal class FallbackSampler : Trace.Sampler
{
    private readonly Trace.Sampler reservoirSampler;
    private readonly Trace.Sampler fixedRateSampler;

    public FallbackSampler(Clock clock)
    {
        this.reservoirSampler = new RateLimitingSampler(1, clock);
        this.fixedRateSampler = new TraceIdRatioBasedSampler(0.05);
    }

    public override SamplingResult ShouldSample(in SamplingParameters samplingParameters)
    {
        var result = this.reservoirSampler.ShouldSample(in samplingParameters);
        return result.Decision != SamplingDecision.Drop ? result : this.fixedRateSampler.ShouldSample(in samplingParameters);
    }
}
