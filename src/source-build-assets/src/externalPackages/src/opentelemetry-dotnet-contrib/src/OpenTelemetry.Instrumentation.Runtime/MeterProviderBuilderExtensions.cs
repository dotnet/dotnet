// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Instrumentation.Runtime;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Metrics;

/// <summary>
/// Extension methods to simplify registering of dependency instrumentation.
/// </summary>
public static class MeterProviderBuilderExtensions
{
    private const string DotNetRuntimeMeterName = "System.Runtime";
    private static readonly bool Net9OrGreater = Environment.Version.Major >= 9;

    /// <summary>
    /// Enables runtime instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="MeterProviderBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="MeterProviderBuilder"/> to chain the calls.</returns>
    public static MeterProviderBuilder AddRuntimeInstrumentation(
        this MeterProviderBuilder builder) =>
        AddRuntimeInstrumentation(builder, configure: null);

    /// <summary>
    /// Enables runtime instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="MeterProviderBuilder"/> being configured.</param>
    /// <param name="configure">Runtime metrics options.</param>
    /// <returns>The instance of <see cref="MeterProviderBuilder"/> to chain the calls.</returns>
    public static MeterProviderBuilder AddRuntimeInstrumentation(
        this MeterProviderBuilder builder,
        Action<RuntimeInstrumentationOptions>? configure)
    {
        Guard.ThrowIfNull(builder);

        if (Net9OrGreater)
        {
            return builder.AddMeter(DotNetRuntimeMeterName);
        }

        var options = new RuntimeInstrumentationOptions();
        configure?.Invoke(options);

        var instrumentation = new RuntimeMetrics(options);
        builder.AddMeter(RuntimeMetrics.MeterInstance.Name);
        return builder.AddInstrumentation(() => instrumentation);
    }
}
