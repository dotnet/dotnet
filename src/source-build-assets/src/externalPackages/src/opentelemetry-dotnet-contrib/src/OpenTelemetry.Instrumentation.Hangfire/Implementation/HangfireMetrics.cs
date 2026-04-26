// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Metrics;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Hangfire.Implementation;

/// <summary>
/// Centralized metrics definitions for Hangfire instrumentation.
/// </summary>
internal static class HangfireMetrics
{
    /// <summary>
    /// Counter for the number of task executions which have been initiated.
    /// Follows OpenTelemetry workflow semantic conventions.
    /// </summary>
    public static readonly Counter<long> ExecutionOutcome =
        Meter!.CreateCounter<long>(
            WorkflowMetricNames.ExecutionOutcome,
            unit: "{executions}",
            description: "The number of task executions which have been initiated.");

    /// <summary>
    /// Histogram for duration of an execution grouped by task, type and result.
    /// Follows OpenTelemetry workflow semantic conventions.
    /// Records duration for different execution phases using workflow.execution.state attribute:
    /// - state=pending: Time spent waiting in queue before execution.
    /// - state=executing: Time spent in actual execution.
    /// </summary>
    public static readonly Histogram<double> ExecutionDuration =
        Meter!.CreateHistogram<double>(
            WorkflowMetricNames.ExecutionDuration,
            unit: "s",
            description: "Duration of an execution grouped by task, type and result.");

    /// <summary>
    /// UpDownCounter for the number of actively running tasks grouped by task and state.
    /// Follows OpenTelemetry workflow semantic conventions.
    /// </summary>
    public static readonly UpDownCounter<long> ExecutionStatus =
        Meter!.CreateUpDownCounter<long>(
            WorkflowMetricNames.ExecutionStatus,
            unit: "{executions}",
            description: "The number of actively running tasks grouped by task, type and the current state.");

    /// <summary>
    /// Counter for the number of errors encountered in task runs (eg. compile, test failures).
    /// Follows OpenTelemetry workflow semantic conventions.
    /// </summary>
    public static readonly Counter<long> ExecutionErrors =
        Meter!.CreateCounter<long>(
            WorkflowMetricNames.ExecutionErrors,
            unit: "{error}",
            description: "The number of errors encountered in task runs (eg. compile, test failures).");

    /// <summary>
    /// Counter for the number of workflow instances which have been initiated.
    /// Follows OpenTelemetry workflow semantic conventions.
    /// In Hangfire, this tracks individual job completions. For batch workflows, this would track batch completion.
    /// </summary>
    public static readonly Counter<long> WorkflowOutcome =
        Meter!.CreateCounter<long>(
            WorkflowMetricNames.WorkflowOutcome,
            unit: "{workflows}",
            description: "The number of workflow instances which have been initiated.");

    /// <summary>
    /// UpDownCounter for the number of actively running workflows grouped by definition and state.
    /// Follows OpenTelemetry workflow semantic conventions.
    /// In Hangfire, this tracks workflows that haven't entered the execution pipeline yet (e.g., scheduled jobs).
    /// </summary>
    public static readonly UpDownCounter<long> WorkflowStatus =
        Meter!.CreateUpDownCounter<long>(
            WorkflowMetricNames.WorkflowStatus,
            unit: "{workflows}",
            description: "The number of actively running workflows grouped by definition and the current state.");

    /// <summary>
    /// Returns Hangfire metric name.
    /// </summary>
    internal static readonly string MeterName = Meter.Name;

    private static Meter? meter;

    /// <summary>
    /// Gets the meter instance for all Hangfire metrics.
    /// </summary>
    private static Meter Meter => meter ??= new(typeof(HangfireMetrics).Assembly.GetName().Name, typeof(HangfireMetrics).Assembly.GetPackageVersion());
}
