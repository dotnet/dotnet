// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Instrumentation.Quartz;
using OpenTelemetry.Instrumentation.Quartz.Implementation;
using OpenTelemetry.Internal;

// ReSharper disable once CheckNamespace
namespace OpenTelemetry.Trace;

/// <summary>
/// Extension methods to simplify registering of dependency instrumentation.
/// </summary>
public static class TraceProviderBuilderExtensions
{
    /// <summary>
    /// Enables the Quartz.NET Job automatic data collection for Quartz.NET.
    /// </summary>
    /// <param name="builder"><see cref="TraceProviderBuilderExtensions"/> being configured.</param>
    /// <returns>The instance of <see cref="TraceProviderBuilderExtensions"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddQuartzInstrumentation(
        this TracerProviderBuilder builder) => AddQuartzInstrumentation(builder, configure: null);

    /// <summary>
    /// Enables the Quartz.NET Job automatic data collection for Quartz.NET.
    /// </summary>
    /// <param name="builder"><see cref="TraceProviderBuilderExtensions"/> being configured.</param>
    /// <param name="configure">Quartz configuration options.</param>
    /// <returns>The instance of <see cref="TraceProviderBuilderExtensions"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddQuartzInstrumentation(
        this TracerProviderBuilder builder,
        Action<QuartzInstrumentationOptions>? configure)
    {
        Guard.ThrowIfNull(builder);

        var options = new QuartzInstrumentationOptions();
        configure?.Invoke(options);

        builder.AddInstrumentation(() => new QuartzJobInstrumentation(options));
        builder.AddSource(QuartzDiagnosticListener.ActivitySourceName);

        builder.AddLegacySource(OperationName.Job.Execute);
        builder.AddLegacySource(OperationName.Job.Veto);

        return builder;
    }
}
