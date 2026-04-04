// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Resources;

namespace OpenTelemetry.Exporter.Instana.Tests;

internal class TestInstanaExporterHelper : IInstanaExporterHelper
{
    public Dictionary<string, object> Attributes { get; } = [];

    public Resource GetParentProviderResource(BaseExporter<Activity> otelExporter)
    {
        return new Resource(this.Attributes);
    }

    public bool IsWindows()
    {
        return false;
    }
}
