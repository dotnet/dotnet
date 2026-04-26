// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Exporter.Instana.Implementation.Processors;

internal class EventsActivityProcessor : ActivityProcessorBase, IActivityProcessor
{
    public override async Task ProcessAsync(Activity activity, InstanaSpan instanaSpan)
    {
        this.PreProcess(activity, instanaSpan);

        foreach (var activityEvent in activity.Events)
        {
            if (activityEvent.Name == InstanaExporterConstants.EXCEPTION_FIELD && instanaSpan.TransformInfo != null)
            {
                instanaSpan.TransformInfo.HasExceptionEvent = true;
            }

            var spanEvent = new SpanEvent
            {
                Name = activityEvent.Name,
                Ts = activityEvent.Timestamp.Ticks,
                Tags = [],
            };

            foreach (var eventTag in activityEvent.Tags)
            {
                if (eventTag.Value != null)
                {
                    spanEvent.Tags[eventTag.Key] = eventTag.Value.ToString();
                }
            }

            instanaSpan.Data.Events.Add(spanEvent);
        }

        await base.ProcessAsync(activity, instanaSpan).ConfigureAwait(false);
    }
}
