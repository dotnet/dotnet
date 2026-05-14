// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Instrumentation.Remoting;
using OpenTelemetry.Instrumentation.Remoting.Implementation;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Trace;

/// <summary>
/// Extension methods to simplify registering .NET Remoting instrumentation.
/// </summary>
public static class TracerProviderBuilderExtensions
{
    /// <summary>
    /// Enables .NET Remoting instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilderExtensions"/> being configured.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilderExtensions"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddRemotingInstrumentation(this TracerProviderBuilder builder) =>
        AddRemotingInstrumentation(builder, configure: null);

    /// <summary>
    /// Enables .NET Remoting instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilderExtensions"/> being configured.</param>
    /// <param name="configure">Instrumentation options.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilderExtensions"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddRemotingInstrumentation(
        this TracerProviderBuilder builder,
        Action<RemotingInstrumentationOptions>? configure)
    {
        Guard.ThrowIfNull(builder);

        builder.AddSource(TelemetryDynamicSinkProvider.ActivitySourceName);

        if (configure != null)
        {
            builder.ConfigureServices(services => services.Configure(configure));
        }

        builder.AddInstrumentation(sp =>
        {
            var remotingOptions = sp.GetRequiredService<IOptionsMonitor<RemotingInstrumentationOptions>>();
            return new RemotingInstrumentation(remotingOptions);
        });

        return builder;
    }
}
