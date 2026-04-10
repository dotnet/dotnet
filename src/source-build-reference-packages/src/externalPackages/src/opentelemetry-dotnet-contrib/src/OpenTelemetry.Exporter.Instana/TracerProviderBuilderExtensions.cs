// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Trace;

namespace OpenTelemetry.Exporter.Instana;

/// <summary>
/// Extension methods for <see cref="TracerProviderBuilder"/> for using Instana.
/// </summary>
public static class TracerProviderBuilderExtensions
{
    /// <summary>
    /// Instana tracer provider builder extension.
    /// </summary>
    /// <param name="options">Tracer provider builder.</param>
    /// <returns>todo.</returns>
    /// <exception cref="ArgumentNullException">Tracer provider builder is null.</exception>
    public static TracerProviderBuilder AddInstanaExporter(this TracerProviderBuilder options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

#pragma warning disable CA2000
        return options.AddProcessor(new BatchActivityExportProcessor(new InstanaExporter()));
#pragma warning restore CA2000
    }
}
