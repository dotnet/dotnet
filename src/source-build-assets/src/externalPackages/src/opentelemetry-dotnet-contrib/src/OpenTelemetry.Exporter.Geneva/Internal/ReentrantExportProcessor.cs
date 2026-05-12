// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.Geneva;

internal class ReentrantExportProcessor<T> : BaseExportProcessor<T>
    where T : class
{
    public ReentrantExportProcessor(BaseExporter<T> exporter)
        : base(exporter)
    {
    }

    protected override void OnExport(T data)
    {
        this.exporter.Export(new(data));
    }
}
