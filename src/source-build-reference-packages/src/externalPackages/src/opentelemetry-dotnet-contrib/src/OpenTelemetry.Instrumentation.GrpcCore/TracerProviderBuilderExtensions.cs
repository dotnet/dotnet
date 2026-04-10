// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Instrumentation.GrpcCore;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Trace;

/// <summary>
/// OpenTelemetry builder extensions to simplify registration of Grpc.Core based interceptors.
/// </summary>
public static class TracerProviderBuilderExtensions
{
    /// <summary>
    /// Configures OpenTelemetry to listen for the Activities created by the client and server interceptors.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddGrpcCoreInstrumentation(
        this TracerProviderBuilder builder)
    {
        Guard.ThrowIfNull(builder);

        return builder.AddSource(GrpcCoreInstrumentation.ActivitySourceName);
    }
}
