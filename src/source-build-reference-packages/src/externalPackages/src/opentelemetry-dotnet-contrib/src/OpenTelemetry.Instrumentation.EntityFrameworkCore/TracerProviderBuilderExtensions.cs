// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Instrumentation.EntityFrameworkCore;
using OpenTelemetry.Instrumentation.EntityFrameworkCore.Implementation;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Trace;

/// <summary>
/// Extension methods to simplify registering of dependency instrumentation.
/// </summary>
public static class TracerProviderBuilderExtensions
{
    /// <summary>
    /// Enables Microsoft.EntityFrameworkCore instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddEntityFrameworkCoreInstrumentation(
        this TracerProviderBuilder builder) =>
        AddEntityFrameworkCoreInstrumentation(builder, name: null, configure: null);

    /// <summary>
    /// Enables Microsoft.EntityFrameworkCore instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="configure">EntityFrameworkCore configuration options.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddEntityFrameworkCoreInstrumentation(
        this TracerProviderBuilder builder,
        Action<EntityFrameworkInstrumentationOptions>? configure) =>
        AddEntityFrameworkCoreInstrumentation(builder, name: null, configure);

    /// <summary>
    /// Enables Microsoft.EntityFrameworkCore instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="name">Name which is used when retrieving options.</param>
    /// <param name="configure">EntityFrameworkCore configuration options.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddEntityFrameworkCoreInstrumentation(
        this TracerProviderBuilder builder,
        string? name,
        Action<EntityFrameworkInstrumentationOptions>? configure)
    {
        Guard.ThrowIfNull(builder);

        name ??= Options.DefaultName;

        if (configure != null)
        {
            builder.ConfigureServices(services => services.Configure(name, configure));
        }

        builder.AddInstrumentation(sp =>
        {
            var options = sp.GetRequiredService<IOptionsMonitor<EntityFrameworkInstrumentationOptions>>().Get(name);
            return new EntityFrameworkInstrumentation(options);
        });

        builder.AddSource(EntityFrameworkDiagnosticListener.ActivitySourceName);

        return builder;
    }
}
