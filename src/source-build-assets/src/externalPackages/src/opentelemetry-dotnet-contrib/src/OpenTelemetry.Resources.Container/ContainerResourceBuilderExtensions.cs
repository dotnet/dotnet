// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Internal;
using OpenTelemetry.Resources.Container;

namespace OpenTelemetry.Resources;

/// <summary>
/// Extension methods to simplify registering of Container resource detector.
/// </summary>
public static class ContainerResourceBuilderExtensions
{
    /// <summary>
    /// Enables Container resource detector.
    /// </summary>
    /// <param name="builder">The <see cref="ResourceBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="ResourceBuilder"/> being configured.</returns>
    public static ResourceBuilder AddContainerDetector(this ResourceBuilder builder)
    {
        Guard.ThrowIfNull(builder);
        return builder.AddDetector(new ContainerDetector());
    }
}
