// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Sampler.AWS;

/// <summary>
/// A builder for AWSXRayRemoteSampler.
/// </summary>
public class AWSXRayRemoteSamplerBuilder
{
    private const string DefaultEndpoint = "http://localhost:2000";

    private static readonly TimeSpan DefaultPollingInterval = TimeSpan.FromMinutes(5);

    private readonly Resource resource;
    private TimeSpan pollingInterval;
    private string endpoint;
    private Clock clock;

    internal AWSXRayRemoteSamplerBuilder(Resource resource)
    {
        this.resource = resource;
        this.pollingInterval = DefaultPollingInterval;
        this.endpoint = DefaultEndpoint;
        this.clock = Clock.GetDefault();
    }

    /// <summary>
    /// Sets the polling interval for configuration updates. If unset, defaults to 5 minutes.
    /// Must be positive.
    /// </summary>
    /// <param name="pollingInterval">the polling interval.</param>
    /// <returns>the same instance of <see cref="AWSXRayRemoteSamplerBuilder"/>.</returns>
    /// <exception cref="ArgumentException">if the argument is non-positive.</exception>
    public AWSXRayRemoteSamplerBuilder SetPollingInterval(TimeSpan pollingInterval)
    {
        if (pollingInterval < TimeSpan.Zero)
        {
            throw new ArgumentException("Polling interval must be non-negative.");
        }

        this.pollingInterval = pollingInterval;

        return this;
    }

    /// <summary>
    /// Sets the endpoint for the TCP proxy to connect to. This is the address to the port on the
    /// OpenTelemetry Collector configured for proxying X-Ray sampling requests. If unset, defaults to
    /// <see cref="DefaultEndpoint"/>.
    /// </summary>
    /// <param name="endpoint">the endpoint for the TCP proxy.</param>
    /// <returns>the same instance of <see cref="AWSXRayRemoteSamplerBuilder"/>.</returns>
    public AWSXRayRemoteSamplerBuilder SetEndpoint(string endpoint)
    {
        if (!string.IsNullOrEmpty(endpoint))
        {
            this.endpoint = endpoint;
        }

        return this;
    }

    /// <summary>
    /// Returns a <see cref="AWSXRayRemoteSampler"/> with configuration of this builder.
    /// </summary>
    /// <returns>an instance of <see cref="Trace.Sampler"/>.</returns>
    public Trace.Sampler Build()
    {
#pragma warning disable CA2000 // Dispose objects before losing scope
        var rootSampler = new AWSXRayRemoteSampler(this.resource, this.pollingInterval, this.endpoint, this.clock);
#pragma warning restore CA2000
        return new ParentBasedSampler(rootSampler);
    }

    // This is intended for testing with a mock clock.
    // Should not be exposed to public.
    internal AWSXRayRemoteSamplerBuilder SetClock(Clock clock)
    {
        this.clock = clock ?? throw new ArgumentNullException(nameof(clock));

        return this;
    }
}
