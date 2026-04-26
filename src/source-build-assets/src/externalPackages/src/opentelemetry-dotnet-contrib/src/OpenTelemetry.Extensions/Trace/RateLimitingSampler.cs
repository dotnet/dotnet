// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

// Based on the jaeger remote sampler for Java from https://github.com/open-telemetry/opentelemetry-java/blob/main/sdk-extensions/jaeger-remote-sampler/src/main/java/io/opentelemetry/sdk/extension/trace/jaeger/sampler/RateLimitingSampler.java

using System.Globalization;
using OpenTelemetry.Extensions.Internal;
using OpenTelemetry.Trace;

namespace OpenTelemetry;

/// <summary>
/// Rate limiting sampler that can be used to sample traces at a constant rate.
/// </summary>
public class RateLimitingSampler : Sampler
{
    private const string SAMPLERTYPE = "ratelimiting";
    private const string SAMPLERTYPEKEY = "sampler.type";
    private const string SAMPLERPARAMKEY = "sampler.param";

    private readonly RateLimiter rateLimiter;
    private readonly SamplingResult onSamplingResult;
    private readonly SamplingResult offSamplingResult;

    /// <summary>
    /// Initializes a new instance of the <see cref="RateLimitingSampler"/> class.
    /// </summary>
    /// <param name="maxTracesPerSecond">The maximum number of traces that will be emitted each second.</param>
    public RateLimitingSampler(int maxTracesPerSecond)
    {
        var maxBalance = maxTracesPerSecond < 1.0 ? 1.0 : maxTracesPerSecond;
        this.rateLimiter = new RateLimiter(maxTracesPerSecond, maxBalance);
        var attributes = new Dictionary<string, object>()
        {
            { SAMPLERTYPEKEY, SAMPLERTYPE },
            { SAMPLERPARAMKEY, (double)maxTracesPerSecond },
        };
        this.onSamplingResult = new SamplingResult(SamplingDecision.RecordAndSample, attributes);
        this.offSamplingResult = new SamplingResult(SamplingDecision.Drop, attributes);
        this.Description = $"RateLimitingSampler{{{DecimalFormat(maxTracesPerSecond)}}}";
    }

    /// <summary>
    /// Checks whether activity needs to be created and tracked.
    /// </summary>
    /// <param name="samplingParameters">
    /// The OpenTelemetry.Trace.SamplingParameters used by the OpenTelemetry.Trace.Sampler
    /// to decide if the System.Diagnostics.Activity to be created is going to be sampled
    /// or not.
    /// </param>
    /// <returns>
    /// Sampling decision on whether activity needs to be sampled or not.
    /// </returns>
    public override SamplingResult ShouldSample(in SamplingParameters samplingParameters)
    {
        return this.rateLimiter.TrySpend(1.0) ? this.onSamplingResult : this.offSamplingResult;
    }

    private static string DecimalFormat(double value)
    {
        var numberFormatInfo = new NumberFormatInfo
        {
            NumberDecimalSeparator = ".",
        };

        return value.ToString("0.00", numberFormatInfo);
    }
}
