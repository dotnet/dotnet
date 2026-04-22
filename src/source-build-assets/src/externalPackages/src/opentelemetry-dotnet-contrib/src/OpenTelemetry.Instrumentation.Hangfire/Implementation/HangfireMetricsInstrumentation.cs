// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Hangfire;
using OpenTelemetry.Metrics;

namespace OpenTelemetry.Instrumentation.Hangfire.Implementation;

/// <summary>
/// Hangfire metrics instrumentation following OpenTelemetry workflow semantic conventions.
/// </summary>
internal sealed class HangfireMetricsInstrumentation : IDisposable
{
    private readonly List<object> filters = new();

    public HangfireMetricsInstrumentation(HangfireMetricsInstrumentationOptions options)
    {
        this.AddFilter(new HangfireMetricsJobFilterAttribute());
        this.AddFilter(new HangfireMetricsStateFilter());
        this.AddFilter(new HangfireMetricsErrorFilterAttribute());

        // Only register pending duration filter if enabled (requires DB call per job)
        if (options.RecordQueueLatency)
        {
            this.AddFilter(new HangfirePendingDurationFilterAttribute());
        }
    }

    public void Dispose()
    {
        foreach (var filter in this.filters)
        {
            GlobalJobFilters.Filters.Remove(filter);
        }
    }

    private void AddFilter(object filter)
    {
        this.filters.Add(filter);
        GlobalJobFilters.Filters.Add(filter);
    }
}
