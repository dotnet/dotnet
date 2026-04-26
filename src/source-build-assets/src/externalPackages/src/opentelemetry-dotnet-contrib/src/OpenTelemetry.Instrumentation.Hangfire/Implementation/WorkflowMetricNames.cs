// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.Hangfire.Implementation;

/// <summary>
/// Constants for workflow metric names outlined by the OpenTelemetry specifications.
/// </summary>
/// <remarks>
/// See: https://github.com/open-telemetry/semantic-conventions/blob/main/docs/workflow/workflow-metrics.md.
/// </remarks>
internal static class WorkflowMetricNames
{
    /// <summary>
    /// The number of task executions which have been initiated.
    /// </summary>
    public const string ExecutionOutcome = "workflow.execution.outcome";

    /// <summary>
    /// Duration of an execution grouped by task, type and result.
    /// </summary>
    public const string ExecutionDuration = "workflow.execution.duration";

    /// <summary>
    /// The number of actively running tasks grouped by task, type and the current state.
    /// </summary>
    public const string ExecutionStatus = "workflow.execution.status";

    /// <summary>
    /// The number of errors encountered in task runs (eg. compile, test failures).
    /// </summary>
    public const string ExecutionErrors = "workflow.execution.errors";

    /// <summary>
    /// The number of workflow instances which have been initiated.
    /// </summary>
    public const string WorkflowOutcome = "workflow.outcome";

    /// <summary>
    /// The number of actively running workflows grouped by definition and the current state.
    /// </summary>
    public const string WorkflowStatus = "workflow.status";
}
