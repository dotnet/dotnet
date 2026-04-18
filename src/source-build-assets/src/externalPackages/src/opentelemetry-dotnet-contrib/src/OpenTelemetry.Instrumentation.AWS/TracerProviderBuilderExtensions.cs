// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Amazon;
using Amazon.Runtime.Telemetry;
using OpenTelemetry.Instrumentation.AWS;
using OpenTelemetry.Instrumentation.AWS.Implementation;
using OpenTelemetry.Instrumentation.AWS.Implementation.Tracing;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Trace;

/// <summary>
/// Extension methods to simplify registering of dependency instrumentation.
/// </summary>
public static class TracerProviderBuilderExtensions
{
    /// <summary>
    /// Enables AWS Instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddAWSInstrumentation(
        this TracerProviderBuilder builder) => AddAWSInstrumentation(builder, configure: null);

    /// <summary>
    /// Enables AWS Instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="configure">AWS client configuration options.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddAWSInstrumentation(
        this TracerProviderBuilder builder,
        Action<AWSClientInstrumentationOptions>? configure)
    {
        Guard.ThrowIfNull(builder);

        var awsClientOptions = new AWSClientInstrumentationOptions();
        configure?.Invoke(awsClientOptions);

        _ = new AWSClientsInstrumentation(awsClientOptions);

        AWSConfigs.TelemetryProvider.RegisterTracerProvider(new AWSTracerProvider());
        builder.AddSource($"{TelemetryConstants.TelemetryScopePrefix}.*");

        return builder;
    }
}
