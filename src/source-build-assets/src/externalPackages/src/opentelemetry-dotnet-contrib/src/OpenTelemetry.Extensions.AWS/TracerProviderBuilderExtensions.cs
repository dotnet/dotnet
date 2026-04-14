// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Extensions.AWS;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Trace;

/// <summary>
/// Extension method to generate AWS X-Ray compatible trace id and replace the trace id of root activity.
/// </summary>
public static class TracerProviderBuilderExtensions
{
    /// <summary>
    /// Replace the trace id of root activity.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/>.</returns>
    public static TracerProviderBuilder AddXRayTraceId(this TracerProviderBuilder builder)
    {
        Guard.ThrowIfNull(builder);

        AWSXRayIdGenerator.ReplaceTraceId();
        return builder;
    }

#if NET
    /// <summary>
    /// Replace the trace id of root activity.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="sampler">Unused. (See deprecation message.)</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/>.</returns>
    [Obsolete($"When targeting .NET 6.0 or later, the X-Ray ID generator does not need to update the sampling decision. Use ${nameof(AddXRayTraceId)} instead.")]
#else
    /// <summary>
    /// 1. Replace the trace id of root activity.
    /// 2. Update the sampling decision for root activity when it's created through ActivitySource.StartActivity().
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="sampler"><see cref="Sampler"/> being used.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/>.</returns>
#endif
    public static TracerProviderBuilder AddXRayTraceIdWithSampler(this TracerProviderBuilder builder, Sampler sampler)
    {
        Guard.ThrowIfNull(builder);

        AWSXRayIdGenerator.ReplaceTraceId(sampler);
        return builder;
    }
}
