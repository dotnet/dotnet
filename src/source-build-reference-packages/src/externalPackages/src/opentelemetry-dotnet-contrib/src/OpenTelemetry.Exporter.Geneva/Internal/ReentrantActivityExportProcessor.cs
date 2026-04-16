// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Exporter.Geneva;

// This export processor exports without synchronization.
// Once OpenTelemetry .NET officially support this,
// we can get rid of this class.
// This is currently only used in ETW export, where we know
// that the underlying system is safe under concurrent calls.
internal sealed class ReentrantActivityExportProcessor : ReentrantExportProcessor<Activity>
{
    public ReentrantActivityExportProcessor(BaseExporter<Activity> exporter)
        : base(exporter)
    {
    }

    protected override void OnExport(Activity data)
    {
        if (data.Recorded)
        {
            base.OnExport(data);
        }
    }
}
