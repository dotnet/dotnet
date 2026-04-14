// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Hangfire.Common;
using Hangfire.Server;

namespace OpenTelemetry.Instrumentation.Hangfire.Implementation;

/// <summary>
/// Hangfire filter that records OpenTelemetry error metrics for job execution failures.
/// Follows OpenTelemetry workflow semantic conventions for workflow.execution.errors metric.
/// </summary>
internal sealed class HangfireMetricsErrorFilterAttribute : JobFilterAttribute, IServerFilter
{
    public void OnPerforming(PerformingContext performingContext)
    {
    }

    public void OnPerformed(PerformedContext performedContext)
    {
        if (performedContext.Exception != null)
        {
            var errorTags = HangfireTagBuilder.BuildErrorTags(performedContext);

            HangfireMetrics.ExecutionErrors.Add(1, errorTags);
        }
    }
}
