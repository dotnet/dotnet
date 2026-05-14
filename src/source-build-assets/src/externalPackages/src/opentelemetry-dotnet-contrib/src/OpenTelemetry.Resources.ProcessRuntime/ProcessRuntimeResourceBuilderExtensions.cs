// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Internal;
using OpenTelemetry.Resources.ProcessRuntime;

namespace OpenTelemetry.Resources;

/// <summary>
/// Extension methods to simplify registering of process runtime resource detectors.
/// </summary>
public static class ProcessRuntimeResourceBuilderExtensions
{
    /// <summary>
    /// Enables process runtime resource detector.
    /// </summary>
    /// <param name="builder">The <see cref="ResourceBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="ResourceBuilder"/> being configured.</returns>
    public static ResourceBuilder AddProcessRuntimeDetector(this ResourceBuilder builder)
    {
        Guard.ThrowIfNull(builder);
        return builder.AddDetector(new ProcessRuntimeDetector());
    }
}
