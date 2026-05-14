// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Trace;

namespace OpenTelemetry.Sampler.AWS;

internal class RateLimitingSampler : Trace.Sampler
{
    private readonly RateLimiter limiter;

    public RateLimitingSampler(long numPerSecond, Clock clock)
    {
        this.limiter = new RateLimiter(numPerSecond, numPerSecond, clock);
    }

    public override SamplingResult ShouldSample(in SamplingParameters samplingParameters)
    {
        if (this.limiter.TrySpend(1))
        {
            return new SamplingResult(SamplingDecision.RecordAndSample);
        }

        return new SamplingResult(SamplingDecision.Drop);
    }
}
