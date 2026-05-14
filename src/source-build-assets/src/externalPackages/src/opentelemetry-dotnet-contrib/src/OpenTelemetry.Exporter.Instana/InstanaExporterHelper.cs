// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Resources;

namespace OpenTelemetry.Exporter.Instana;

internal class InstanaExporterHelper : IInstanaExporterHelper
{
    public Resource GetParentProviderResource(BaseExporter<Activity> otelExporter)
    {
        return otelExporter.ParentProvider.GetResource();
    }

    public bool IsWindows()
    {
        return Environment.OSVersion.Platform == PlatformID.Win32NT;
    }
}
