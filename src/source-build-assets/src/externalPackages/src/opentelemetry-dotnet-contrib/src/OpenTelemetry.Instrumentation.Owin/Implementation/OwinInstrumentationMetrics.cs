// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Metrics;
using System.Reflection;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Owin.Implementation;

internal static class OwinInstrumentationMetrics
{
    internal static readonly Assembly Assembly = typeof(OwinInstrumentationMetrics).Assembly;
    internal static readonly AssemblyName AssemblyName = Assembly.GetName();
    internal static readonly string MeterName = AssemblyName.Name;
    internal static readonly Meter Instance = new(MeterName, Assembly.GetPackageVersion());
    internal static readonly Histogram<double> HttpServerDuration = Instance.CreateHistogram(
        "http.server.request.duration",
        unit: "s",
        description: "Duration of HTTP server requests.",
        advice: new InstrumentAdvice<double> { HistogramBucketBoundaries = [0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10] });
}
