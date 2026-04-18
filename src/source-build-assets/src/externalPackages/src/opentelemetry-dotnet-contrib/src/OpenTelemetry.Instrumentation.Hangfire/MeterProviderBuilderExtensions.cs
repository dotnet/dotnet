// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Instrumentation.Hangfire.Implementation;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Metrics;

/// <summary>
/// Extension methods to simplify registering of Hangfire metrics instrumentation.
/// </summary>
public static class MeterProviderBuilderExtensions
{
    /// <summary>
    /// Enables Hangfire metrics instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="MeterProviderBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="MeterProviderBuilder"/> to chain the calls.</returns>
    public static MeterProviderBuilder AddHangfireInstrumentation(
        this MeterProviderBuilder builder)
        => AddHangfireInstrumentation(builder, name: null, configure: null);

    /// <summary>
    /// Enables Hangfire metrics instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="MeterProviderBuilder"/> being configured.</param>
    /// <param name="configure">Callback action for configuring <see cref="HangfireMetricsInstrumentationOptions"/>.</param>
    /// <returns>The instance of <see cref="MeterProviderBuilder"/> to chain the calls.</returns>
    public static MeterProviderBuilder AddHangfireInstrumentation(
        this MeterProviderBuilder builder,
        Action<HangfireMetricsInstrumentationOptions>? configure)
        => AddHangfireInstrumentation(builder, name: null, configure);

    /// <summary>
    /// Enables Hangfire metrics instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="MeterProviderBuilder"/> being configured.</param>
    /// <param name="name">Name which is used when retrieving options.</param>
    /// <param name="configure"><see cref="HangfireMetricsInstrumentationOptions"/> configuration options.</param>
    /// <returns>The instance of <see cref="MeterProviderBuilder"/> to chain the calls.</returns>
    public static MeterProviderBuilder AddHangfireInstrumentation(
        this MeterProviderBuilder builder,
        string? name,
        Action<HangfireMetricsInstrumentationOptions>? configure)
    {
        Guard.ThrowIfNull(builder);

        name ??= Options.DefaultName;

        if (configure != null)
        {
            builder.ConfigureServices(services => services.Configure(name, configure));
        }

        builder.AddMeter(HangfireMetrics.MeterName);

        return builder.AddInstrumentation(sp =>
        {
            var options = sp.GetRequiredService<IOptionsMonitor<HangfireMetricsInstrumentationOptions>>().Get(name);
            return new HangfireMetricsInstrumentation(options);
        });
    }
}
