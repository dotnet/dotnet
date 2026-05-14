// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Internal;
using OpenTelemetry.Resources.Gcp;

namespace OpenTelemetry.Resources;

/// <summary>
/// Extension methods to simplify registering of Google Cloud Platform resource detectors.
/// </summary>
public static class GcpResourceBuilderExtensions
{
    /// <summary>
    /// Enables Google Cloud Platform resource detector.
    /// </summary>
    /// <param name="builder">The <see cref="ResourceBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="ResourceBuilder"/> being configured.</returns>
    public static ResourceBuilder AddGcpDetector(this ResourceBuilder builder)
    {
        Guard.ThrowIfNull(builder);
        return builder.AddDetector(new GcpResourceDetector());
    }
}
