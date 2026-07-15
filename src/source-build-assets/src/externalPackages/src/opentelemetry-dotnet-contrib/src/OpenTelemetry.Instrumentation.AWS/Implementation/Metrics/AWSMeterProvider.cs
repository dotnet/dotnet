// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using Amazon.Runtime.Telemetry;
using Amazon.Runtime.Telemetry.Metrics;
using OpenTelemetry.AWS;

namespace OpenTelemetry.Instrumentation.AWS.Implementation.Metrics;

internal sealed class AWSMeterProvider(SemanticConventionVersion version) : MeterProvider
{
    private readonly ConcurrentDictionary<string, System.Diagnostics.Metrics.Meter> meters = new();
    private readonly Version semanticConventionVersion = AWSSemanticConventions.GetVersion(version);

    public override Meter GetMeter(string scope, Attributes? attributes = null)
    {
        if (!this.meters.TryGetValue(scope, out var meter))
        {
#if NET
            meter = this.meters.GetOrAdd(scope, static (name, state) => CreateMeter(name, state), (this.semanticConventionVersion, attributes?.AllAttributes));
#else
            meter = this.meters.GetOrAdd(scope, (name) => CreateMeter(name, (this.semanticConventionVersion, attributes?.AllAttributes)));
#endif
        }

        return new AWSMeter(meter);
    }

    private static System.Diagnostics.Metrics.Meter CreateMeter(string name, (Version Version, IEnumerable<KeyValuePair<string, object?>>? Attributes) state)
        => OpenTelemetry.Metrics.MeterFactory.Create(typeof(AWSMeterProvider), state.Version, state.Attributes, name);
}
