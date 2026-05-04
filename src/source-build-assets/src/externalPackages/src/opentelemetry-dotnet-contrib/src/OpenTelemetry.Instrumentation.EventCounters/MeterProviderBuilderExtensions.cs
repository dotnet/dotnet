// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Instrumentation.EventCounters;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Metrics;

/// <summary>
/// Extension methods to simplify registering of dependency instrumentation.
/// </summary>
public static class MeterProviderBuilderExtensions
{
    /// <summary>
    /// Enables EventCounter instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="MeterProviderBuilder"/> being configured.</param>
    /// <param name="configure">EventCounters instrumentation options.</param>
    /// <returns>The instance of <see cref="MeterProviderBuilder"/> to chain the calls.</returns>
    public static MeterProviderBuilder AddEventCountersInstrumentation(
        this MeterProviderBuilder builder,
        Action<EventCountersInstrumentationOptions>? configure = null)
    {
        Guard.ThrowIfNull(builder);

        var options = new EventCountersInstrumentationOptions();
        configure?.Invoke(options);

        builder.AddMeter(EventCountersMetrics.MeterInstance.Name);
        return builder.AddInstrumentation(() => new EventCountersMetrics(options));
    }
}
