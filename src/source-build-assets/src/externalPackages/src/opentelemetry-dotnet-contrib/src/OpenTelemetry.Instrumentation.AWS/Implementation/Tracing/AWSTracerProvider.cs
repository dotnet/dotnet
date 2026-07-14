// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using System.Diagnostics;
using Amazon.Runtime.Telemetry.Tracing;
using OpenTelemetry.AWS;

namespace OpenTelemetry.Instrumentation.AWS.Implementation.Tracing;

internal sealed class AWSTracerProvider(SemanticConventionVersion version) : TracerProvider
{
    private readonly ConcurrentDictionary<string, ActivitySource> activitySources = new();
    private readonly Version semanticConventionVersion = AWSSemanticConventions.GetVersion(version);

    public override Tracer GetTracer(string scope)
    {
        // We can add support for tags if https://github.com/aws/aws-sdk-net/issues/4393 is implemented
        if (!this.activitySources.TryGetValue(scope, out var activitySource))
        {
#if NET
            activitySource = this.activitySources.GetOrAdd(scope, static (name, version) => CreateActivitySource(name, version), this.semanticConventionVersion);
#else
            activitySource = this.activitySources.GetOrAdd(scope, (name) => CreateActivitySource(name, this.semanticConventionVersion));
#endif
        }

        return new AWSTracer(activitySource);
    }

    private static ActivitySource CreateActivitySource(string name, Version version)
        => Trace.ActivitySourceFactory.Create(typeof(AWSTracerProvider), version, name: name);
}
