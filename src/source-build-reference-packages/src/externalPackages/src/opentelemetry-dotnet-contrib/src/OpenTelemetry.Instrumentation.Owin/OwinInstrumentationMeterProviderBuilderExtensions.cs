// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Instrumentation.Owin.Implementation;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Metrics;

/// <summary>
/// Extension methods to simplify registering of OWIN request instrumentation.
/// </summary>
public static class OwinInstrumentationMeterProviderBuilderExtensions
{
    /// <summary>
    /// Enables the incoming requests automatic data collection for OWIN.
    /// </summary>
    /// <param name="builder"><see cref="MeterProviderBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="MeterProviderBuilder"/> to chain the calls.</returns>
    public static MeterProviderBuilder AddOwinInstrumentation(
        this MeterProviderBuilder builder)
    {
        Guard.ThrowIfNull(builder);

        builder.AddMeter(OwinInstrumentationMetrics.MeterName);
        return builder;
    }
}
