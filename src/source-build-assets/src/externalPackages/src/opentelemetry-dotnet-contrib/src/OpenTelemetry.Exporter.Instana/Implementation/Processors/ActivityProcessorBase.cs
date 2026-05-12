// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Exporter.Instana.Implementation.Processors;

internal abstract class ActivityProcessorBase : IActivityProcessor
{
    public IActivityProcessor? NextProcessor { get; set; }

    public virtual async Task ProcessAsync(Activity activity, InstanaSpan instanaSpan)
    {
        if (this.NextProcessor != null)
        {
            await this.NextProcessor.ProcessAsync(activity, instanaSpan).ConfigureAwait(false);
        }
    }

    protected virtual void PreProcess(Activity activity, InstanaSpan instanaSpan)
    {
        instanaSpan.TransformInfo ??= new InstanaSpanTransformInfo();

        instanaSpan.Data ??= new Data()
        {
            data = [],
            Events = new List<SpanEvent>(8),
            Tags = [],
        };
    }
}
