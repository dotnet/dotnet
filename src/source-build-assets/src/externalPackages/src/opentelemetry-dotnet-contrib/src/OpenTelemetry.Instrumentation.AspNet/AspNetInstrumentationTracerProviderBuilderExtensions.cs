// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Instrumentation.AspNet;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Trace;

/// <summary>
/// Extension methods to simplify registering of ASP.NET request instrumentation.
/// </summary>
public static class AspNetInstrumentationTracerProviderBuilderExtensions
{
    /// <summary>
    /// Enables the incoming requests automatic data collection for ASP.NET.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddAspNetInstrumentation(this TracerProviderBuilder builder) =>
        AddAspNetInstrumentation(builder, configure: null);

    /// <summary>
    /// Enables the incoming requests automatic data collection for ASP.NET.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="configure">ASP.NET Request configuration options.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddAspNetInstrumentation(
        this TracerProviderBuilder builder,
        Action<AspNetTraceInstrumentationOptions>? configure)
    {
        Guard.ThrowIfNull(builder);

        return builder.ConfigureServices(services =>
        {
            if (configure != null)
            {
                services.Configure(configure);
            }

            services.RegisterOptionsFactory(
                configuration => new AspNetTraceInstrumentationOptions(configuration));

            services.ConfigureOpenTelemetryTracerProvider((sp, tracerProviderBuilder) =>
            {
                var options = sp.GetRequiredService<IOptionsMonitor<AspNetTraceInstrumentationOptions>>().Get(name: null);
                AspNetInstrumentation.Instance.TraceOptions = options;

                tracerProviderBuilder.AddInstrumentation(() =>
                {
                    return AspNetInstrumentation.Instance.HandleManager.AddTracingHandle();
                });
                tracerProviderBuilder.AddSource(AspNetInstrumentation.ActivitySource.Name);
            });
        });
    }
}
