// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using System.Diagnostics;
using Amazon.Runtime.Telemetry.Tracing;

namespace OpenTelemetry.Instrumentation.AWS.Implementation.Tracing;

internal sealed class AWSTracerProvider : TracerProvider
{
    private static readonly ConcurrentDictionary<string, AWSTracer> TracersDictionary = new();

    public override Tracer GetTracer(string scope)
    {
        if (TracersDictionary.TryGetValue(scope, out var awsTracer))
        {
            return awsTracer;
        }

        awsTracer = TracersDictionary.GetOrAdd(
            scope,
            new AWSTracer(new ActivitySource(scope)));

        return awsTracer;
    }
}
