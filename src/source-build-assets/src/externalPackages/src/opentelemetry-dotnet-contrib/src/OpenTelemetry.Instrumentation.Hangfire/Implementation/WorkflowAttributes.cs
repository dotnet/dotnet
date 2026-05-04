// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.Hangfire.Implementation;

/// <summary>
/// Constants for workflow semantic attribute names and values outlined by the OpenTelemetry specifications.
/// </summary>
/// <remarks>
/// See: https://github.com/open-telemetry/semantic-conventions/blob/main/docs/workflow/workflow-metrics.md.
/// </remarks>
internal static class WorkflowAttributes
{
    // ===== Attribute Names =====

    /// <summary>
    /// Name of the task.
    /// </summary>
    public const string AttributeWorkflowTaskName = "workflow.task.name";

    /// <summary>
    /// Name of the workflow.
    /// </summary>
    public const string AttributeWorkflowDefinitionName = "workflow.definition.name";

    /// <summary>
    /// The result of executing the task.
    /// </summary>
    public const string AttributeWorkflowExecutionResult = "workflow.execution.result";

    /// <summary>
    /// The result of a workflow run.
    /// </summary>
    public const string AttributeWorkflowResult = "workflow.result";

    /// <summary>
    /// The execution state.
    /// </summary>
    public const string AttributeWorkflowExecutionState = "workflow.execution.state";

    /// <summary>
    /// The workflow state.
    /// </summary>
    public const string AttributeWorkflowState = "workflow.state";

    /// <summary>
    /// The product being used to co-ordinate the execution of the tasks.
    /// </summary>
    public const string AttributeWorkflowPlatformName = "workflow.platform.name";

    /// <summary>
    /// Type of trigger that was called.
    /// </summary>
    public const string AttributeWorkflowTriggerType = "workflow.trigger.type";

    /// <summary>
    /// Describes a class of error the operation ended with.
    /// </summary>
    public const string AttributeErrorType = "error.type";

    // ===== Attribute Value Constants =====

    /// <summary>
    /// Values for workflow.execution.result and workflow.result attributes.
    /// </summary>
    public static class WorkflowResultValues
    {
        /// <summary>
        /// The execution/workflow finished successfully.
        /// </summary>
        public const string Success = "success";

        /// <summary>
        /// The execution/workflow did not finish successfully, eg. due to a compile error or a failing test.
        /// </summary>
        public const string Failure = "failure";

        /// <summary>
        /// The execution/workflow was cancelled, eg. by a user manually cancelling the execution.
        /// </summary>
        public const string Cancellation = "cancellation";

        /// <summary>
        /// The execution/workflow failed due to an error in the system, eg. due to the worker being killed.
        /// </summary>
        public const string Error = "error";

        /// <summary>
        /// A timeout caused the execution/workflow to be interrupted.
        /// </summary>
        public const string Timeout = "timeout";

        /// <summary>
        /// The execution/workflow was skipped, eg. due to a precondition not being met.
        /// </summary>
        public const string Skip = "skip";
    }

    /// <summary>
    /// Values for workflow.execution.state and workflow.state attributes.
    /// </summary>
    public static class WorkflowStateValues
    {
        /// <summary>
        /// The pending state spans from when the workflow/execution is triggered until it starts executing.
        /// For workflows: includes time waiting for scheduled time (scheduled jobs).
        /// For executions: includes time spent in queue waiting for a worker (enqueued jobs).
        /// </summary>
        public const string Pending = "pending";

        /// <summary>
        /// The executing state spans the actual execution of the workflow/task (eg. build, test).
        /// </summary>
        public const string Executing = "executing";

        /// <summary>
        /// The finalizing state spans from when execution has finished and covers cleanup of resources, etc.
        /// </summary>
        public const string Finalizing = "finalizing";
    }

    /// <summary>
    /// Values for workflow.platform.name attribute.
    /// </summary>
    public static class WorkflowPlatformNameValues
    {
        /// <summary>
        /// Hangfire platform.
        /// </summary>
        public const string Hangfire = "hangfire";
    }

    /// <summary>
    /// Values for workflow.trigger.type attribute.
    /// </summary>
    public static class WorkflowTriggerTypeValues
    {
        /// <summary>
        /// Cron trigger (recurring jobs).
        /// </summary>
        public const string Cron = "cron";

        /// <summary>
        /// API trigger (fire-and-forget, continuation jobs).
        /// </summary>
        public const string Api = "api";

        /// <summary>
        /// Schedule trigger (delayed jobs).
        /// </summary>
        public const string Schedule = "schedule";
    }
}
