// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Resources;

namespace OpenTelemetry.Exporter.Instana;

internal interface IInstanaExporterHelper
{
    bool IsWindows();

    Resource GetParentProviderResource(BaseExporter<Activity> otelExporter);
}
