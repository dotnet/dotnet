// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Internal;
using OpenTelemetry.Resources.OperatingSystem;

namespace OpenTelemetry.Resources;

/// <summary>
/// Extension methods to simplify registering of operating system detectors.
/// </summary>
public static class OperatingSystemResourceBuilderExtensions
{
    /// <summary>
    /// Enables operating system detector.
    /// </summary>
    /// <param name="builder">The <see cref="ResourceBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="ResourceBuilder"/> being configured.</returns>
    public static ResourceBuilder AddOperatingSystemDetector(this ResourceBuilder builder)
    {
        Guard.ThrowIfNull(builder);
        return builder.AddDetector(new OperatingSystemDetector());
    }
}
