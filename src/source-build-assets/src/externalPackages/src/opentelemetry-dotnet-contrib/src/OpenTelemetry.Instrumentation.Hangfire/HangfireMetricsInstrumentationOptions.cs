// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Metrics;

/// <summary>
/// Options for Hangfire metrics instrumentation.
/// </summary>
public sealed class HangfireMetricsInstrumentationOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether to record the pending state duration in metrics.
    /// </summary>
    /// <remarks>
    /// When enabled, records workflow.execution.duration with state="pending", representing
    /// the time jobs spend waiting in the queue before execution starts.
    /// This requires an additional database call per job execution to retrieve the enqueue timestamp.
    /// Default is <see langword="false"/> to avoid performance impact in high-throughput scenarios.
    /// When disabled, only execution duration (state="executing") is recorded.
    /// </remarks>
    public bool RecordQueueLatency { get; set; }
}
