// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.AWS;
using OpenTelemetry.Instrumentation.AWSLambda.Implementation;
using OpenTelemetry.Internal;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.AWSLambda;

/// <summary>
/// Extension class for TracerProviderBuilder.
/// </summary>
public static class TracerProviderBuilderExtensions
{
    /// <summary>
    /// Add AWS Lambda configurations.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddAWSLambdaConfigurations(this TracerProviderBuilder builder) =>
        AddAWSLambdaConfigurations(builder, configure: null);

    /// <summary>
    /// Add AWS Lambda configurations.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="configure">AWS lambda instrumentation options.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddAWSLambdaConfigurations(
        this TracerProviderBuilder builder,
        Action<AWSLambdaInstrumentationOptions>? configure)
    {
        Guard.ThrowIfNull(builder);

        var options = new AWSLambdaInstrumentationOptions();
        configure?.Invoke(options);

        var awsSemanticConventions = new AWSSemanticConventions(options.SemanticConventionVersion);

        AWSLambdaWrapper.AWSSemanticConventions = awsSemanticConventions;
        AWSLambdaWrapper.DisableAwsXRayContextExtraction = options.DisableAwsXRayContextExtraction;
        AWSMessagingUtils.SetParentFromMessageBatch = options.SetParentFromBatch;

        builder.AddSource(AWSLambdaWrapper.ActivitySourceName);
        builder.ConfigureResource(x => x.AddDetector(new AWSLambdaResourceDetector(awsSemanticConventions)));

        return builder;
    }
}
