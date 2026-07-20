// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Amazon;
using Amazon.Runtime.Telemetry;
using OpenTelemetry.AWS;
using OpenTelemetry.Instrumentation.AWS.Implementation.Metrics;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Metrics;

/// <summary>
/// Extension methods to simplify registering of dependency instrumentation.
/// </summary>
public static class MeterProviderBuilderExtensions
{
    /// <summary>
    /// Enables AWS instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="MeterProviderBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="MeterProviderBuilder"/> to chain the calls.</returns>
    public static MeterProviderBuilder AddAWSInstrumentation(
        this MeterProviderBuilder builder)
    {
        Guard.ThrowIfNull(builder);

        // This isn't currently configurable, so use the default
        var semanticConventionVersion = AWSSemanticConventions.DefaultSemanticConventionVersion;

        AWSConfigs.TelemetryProvider.RegisterMeterProvider(new AWSMeterProvider(semanticConventionVersion));
        builder.AddMeter($"{TelemetryConstants.TelemetryScopePrefix}.*");

        return builder;
    }
}
