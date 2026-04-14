// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Instrumentation.AspNet.Implementation;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.AspNet;

/// <summary>
/// Asp.Net Requests instrumentation.
/// </summary>
internal sealed class AspNetInstrumentation : IDisposable
{
    public static readonly AspNetInstrumentation Instance = new();

    private static readonly (ActivitySource ActivitySource, Meter Meter) Telemetry = CreateTelemetry();
#pragma warning disable SA1202 // Elements must be ordered by accessibility. Telemetry field should be private and initialized earlier
    public static readonly ActivitySource ActivitySource = Telemetry.ActivitySource;
#pragma warning restore SA1202 // Elements must be ordered by accessibility. Telemetry field should be private and initialized earlier
    public static readonly Meter Meter = Telemetry.Meter;

    public static readonly Histogram<double> HttpServerDuration = Meter.CreateHistogram(
        "http.server.request.duration",
        unit: "s",
        description: "Duration of HTTP server requests.",
        advice: new InstrumentAdvice<double> { HistogramBucketBoundaries = [0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10] });

    public readonly InstrumentationHandleManager HandleManager = new();
    private readonly HttpInListener httpInListener;

    /// <summary>
    /// Initializes a new instance of the <see cref="AspNetInstrumentation"/> class.
    /// </summary>
    private AspNetInstrumentation()
    {
        this.httpInListener = new();
    }

    public AspNetTraceInstrumentationOptions TraceOptions { get; set; } = new();

    public AspNetMetricsInstrumentationOptions MetricOptions { get; set; } = new();

    /// <inheritdoc/>
    public void Dispose()
    {
        this.httpInListener?.Dispose();
    }

    private static (ActivitySource ActivitySource, Meter Meter) CreateTelemetry()
    {
        const string telemetrySchemaUrl = "https://opentelemetry.io/schemas/1.36.0";
        var assembly = typeof(AspNetInstrumentation).Assembly;
        var assemblyName = assembly.GetName();
        var name = assemblyName.Name!;
        var version = assembly.GetPackageVersion();

        var activitySourceOptions = new ActivitySourceOptions(name)
        {
            Version = version,
            TelemetrySchemaUrl = telemetrySchemaUrl,
        };

        var meterOptions = new MeterOptions(name)
        {
            Version = version,
            TelemetrySchemaUrl = telemetrySchemaUrl,
        };

        return (new ActivitySource(activitySourceOptions), new Meter(meterOptions));
    }
}
