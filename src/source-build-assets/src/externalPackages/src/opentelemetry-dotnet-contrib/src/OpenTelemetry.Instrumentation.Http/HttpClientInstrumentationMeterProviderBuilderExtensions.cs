// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NET
#if !NETFRAMEWORK
using OpenTelemetry.Instrumentation.Http;
#endif
using OpenTelemetry.Instrumentation.Http.Implementation;
#endif

using OpenTelemetry.Internal;

namespace OpenTelemetry.Metrics;

/// <summary>
/// Extension methods to simplify registering of HttpClient instrumentation.
/// </summary>
public static class HttpClientInstrumentationMeterProviderBuilderExtensions
{
    /// <summary>
    /// Enables HttpClient instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="MeterProviderBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="MeterProviderBuilder"/> to chain the calls.</returns>
    public static MeterProviderBuilder AddHttpClientInstrumentation(
        this MeterProviderBuilder builder)
    {
        Guard.ThrowIfNull(builder);

#if NET
        return builder
            .AddMeter("System.Net.Http")
            .AddMeter("System.Net.NameResolution");
#else
        // Note: Warm-up the status code and method mapping.
        _ = TelemetryHelper.BoxedStatusCodes;
        _ = HttpTagHelper.RequestDataHelper;

#if NETFRAMEWORK
        builder.AddMeter(HttpWebRequestActivitySource.MeterName);
#else
        builder.AddMeter(HttpHandlerMetricsDiagnosticListener.MeterName);

#pragma warning disable CA2000 // Dispose objects before losing scope
        builder.AddInstrumentation(new HttpClientMetrics());
#pragma warning restore CA2000 // Dispose objects before losing scope
#endif
        return builder;
#endif
    }
}
