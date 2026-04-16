// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Instrumentation.Owin;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Trace;

/// <summary>
/// Extension methods to simplify registering of OWIN request instrumentation.
/// </summary>
public static class TracerProviderBuilderExtensions
{
    /// <summary>
    /// Enables the incoming requests automatic data collection for OWIN.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddOwinInstrumentation(this TracerProviderBuilder builder) =>
        AddOwinInstrumentation(builder, configure: null);

    /// <summary>
    /// Enables the incoming requests automatic data collection for OWIN.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="configure">OWIN Request configuration options.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddOwinInstrumentation(
        this TracerProviderBuilder builder,
        Action<OwinInstrumentationOptions>? configure)
    {
        Guard.ThrowIfNull(builder);

        return builder.ConfigureServices(services =>
        {
            if (configure != null)
            {
                services.Configure(configure);
            }

            services.RegisterOptionsFactory(
                configuration => new OwinInstrumentationOptions(configuration));

            services.ConfigureOpenTelemetryTracerProvider((sp, builder) =>
            {
                OwinInstrumentationActivitySource.Options = sp.GetRequiredService<IOptionsMonitor<OwinInstrumentationOptions>>().Get(name: null);

                builder.AddSource(OwinInstrumentationActivitySource.ActivitySourceName);
            });
        });
    }
}
