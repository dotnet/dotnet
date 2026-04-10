// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Instrumentation.Hangfire.Implementation;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Trace;

/// <summary>
/// Extension methods to simplify registering of Hangfire job instrumentation.
/// </summary>
public static class TracerProviderBuilderExtensions
{
    /// <summary>
    /// Adds Hangfire instrumentation to the tracer provider.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddHangfireInstrumentation(
        this TracerProviderBuilder builder) =>
        AddHangfireInstrumentation(builder, name: null, configure: null);

    /// <summary>
    /// Adds Hangfire instrumentation to the tracer provider.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="configure">Callback action for configuring <see cref="HangfireInstrumentationOptions"/>.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddHangfireInstrumentation(
        this TracerProviderBuilder builder,
        Action<HangfireInstrumentationOptions>? configure) =>
        AddHangfireInstrumentation(builder, name: null, configure);

    /// <summary>
    /// Adds Hangfire instrumentation to the tracer provider.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="name">Name which is used when retrieving options.</param>
    /// <param name="configure"><see cref="HangfireInstrumentationOptions"/> configuration options.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddHangfireInstrumentation(
        this TracerProviderBuilder builder,
        string? name,
        Action<HangfireInstrumentationOptions>? configure)
    {
        Guard.ThrowIfNull(builder);

        name ??= Options.DefaultName;

        if (configure != null)
        {
            builder.ConfigureServices(services => services.Configure(name, configure));
        }

        builder.AddInstrumentation(sp =>
        {
            var options = sp.GetRequiredService<IOptionsMonitor<HangfireInstrumentationOptions>>().Get(name);
            return new HangfireInstrumentation(options);
        });

        return builder.AddSource(HangfireInstrumentation.ActivitySourceName);
    }
}
