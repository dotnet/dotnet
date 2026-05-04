// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Instrumentation.ServiceFabricRemoting;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Trace;

/// <summary>
/// Extension methods to simplify registering of ServiceFabric Remoting instrumentation.
/// </summary>
public static class TracerProviderBuilderExtensions
{
    /// <summary>
    /// Enables the incoming requests automatic data collection for ServiceFabric Remoting.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddServiceFabricRemotingInstrumentation(this TracerProviderBuilder builder)
    {
        return AddServiceFabricRemotingInstrumentation(builder, configure: null);
    }

    /// <summary>
    /// Enables the incoming requests automatic data collection for ServiceFabric Remoting.
    /// </summary>
    /// <param name="tracerProviderBuilder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="configure">ServiceFabric Remoting configuration options.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddServiceFabricRemotingInstrumentation(this TracerProviderBuilder tracerProviderBuilder, Action<ServiceFabricRemotingInstrumentationOptions>? configure)
    {
        Guard.ThrowIfNull(tracerProviderBuilder);

        return tracerProviderBuilder.ConfigureServices(services =>
        {
            if (configure != null)
            {
                services.Configure(configure);
            }

            services.ConfigureOpenTelemetryTracerProvider((serviceProvider, builder) =>
            {
                ServiceFabricRemotingActivitySource.Options = serviceProvider.GetRequiredService<IOptionsMonitor<ServiceFabricRemotingInstrumentationOptions>>().Get(name: null);

                builder.AddSource(ServiceFabricRemotingActivitySource.ActivitySourceName);
            });
        });
    }
}
