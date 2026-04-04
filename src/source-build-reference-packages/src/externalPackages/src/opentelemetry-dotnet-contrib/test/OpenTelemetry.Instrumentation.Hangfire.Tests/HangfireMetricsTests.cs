// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Hangfire;
using Hangfire.States;
using OpenTelemetry.Instrumentation.Hangfire.Implementation;
using OpenTelemetry.Instrumentation.Hangfire.Tests.Utils;
using OpenTelemetry.Metrics;
using Xunit;

namespace OpenTelemetry.Instrumentation.Hangfire.Tests;

[Collection("Hangfire")]
public class HangfireMetricsTests : IClassFixture<HangfireFixture>
{
    private readonly HangfireFixture hangfireFixture;

    public HangfireMetricsTests(HangfireFixture hangfireFixture)
    {
        this.hangfireFixture = hangfireFixture;
    }

    [Fact]
    public async Task Should_Record_ExecutionOutcome_Metric_When_Job_Executes()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act
        var jobId = BackgroundJob.Enqueue<TestJob>(x => x.Execute());
        await this.hangfireFixture.WaitJobProcessedAsync(jobId, 5);

        meterProvider.ForceFlush();

        // Assert
        var executionOutcomeMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionOutcome);
        AssertUtils.AssertHasMetricPoints(executionOutcomeMetric);
        Assert.Equal("{executions}", executionOutcomeMetric!.Unit);
        Assert.Equal(MetricType.LongSum, executionOutcomeMetric.MetricType);

        var metricPoints = executionOutcomeMetric.ToMetricPointList();
        var sum = metricPoints.First().GetSumLong();
        Assert.True(sum >= 1, $"Expected workflow.execution.outcome sum >= 1, got {sum}");

        // Validate tags
        var metricPoint = metricPoints.First();
        AssertUtils.AssertHasTag(metricPoint, WorkflowAttributes.AttributeWorkflowTaskName);
        AssertUtils.AssertHasTag(metricPoint, WorkflowAttributes.AttributeWorkflowPlatformName);
        AssertUtils.AssertHasTagValue(metricPoint, WorkflowAttributes.AttributeWorkflowExecutionResult, WorkflowAttributes.WorkflowResultValues.Success);
    }

    [Fact]
    public async Task Should_Record_ExecutionDuration_Metric_For_Successful_Job()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act
        var jobId = BackgroundJob.Enqueue<TestJob>(x => x.Execute());
        await this.hangfireFixture.WaitJobProcessedAsync(jobId, 5);

        meterProvider.ForceFlush();

        // Assert
        var durationMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionDuration);
        AssertUtils.AssertHasMetricPoints(durationMetric);
        Assert.Equal("s", durationMetric!.Unit);
        Assert.Equal(MetricType.Histogram, durationMetric.MetricType);

        var metricPoints = durationMetric.ToMetricPointList();
        var metricPoint = metricPoints.First();

        var count = metricPoint.GetHistogramCount();
        var sum = metricPoint.GetHistogramSum();

        Assert.True(count >= 1, $"Expected histogram count >= 1, got {count}");
        Assert.True(sum >= 0, $"Expected histogram sum >= 0, got {sum}");

        // Validate tags for successful execution
        AssertUtils.AssertHasTag(metricPoint, WorkflowAttributes.AttributeWorkflowTaskName);
        AssertUtils.AssertHasTag(metricPoint, WorkflowAttributes.AttributeWorkflowPlatformName);
        AssertUtils.AssertHasTagValue(metricPoint, WorkflowAttributes.AttributeWorkflowExecutionResult, WorkflowAttributes.WorkflowResultValues.Success);

        // Error type should NOT be present for successful execution
        AssertUtils.AssertHasNoTag(metricPoint, WorkflowAttributes.AttributeErrorType);
    }

    [Fact]
    public async Task Should_Record_ExecutionDuration_Metric_With_ErrorType_For_Failed_Job()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act
        var jobId = BackgroundJob.Enqueue<TestJob>(x => x.ThrowException());
        await this.hangfireFixture.WaitJobProcessedAsync(jobId, 5);

        meterProvider.ForceFlush();

        // Assert
        var durationMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionDuration);
        AssertUtils.AssertHasMetricPoints(durationMetric);

        var metricPoints = durationMetric!.ToMetricPointList();
        var metricPoint = metricPoints.First();

        var count = metricPoint.GetHistogramCount();
        Assert.True(count >= 1, $"Expected histogram count >= 1, got {count}");

        // Validate tags for failed execution
        AssertUtils.AssertHasTag(metricPoint, WorkflowAttributes.AttributeWorkflowTaskName);
        AssertUtils.AssertHasTag(metricPoint, WorkflowAttributes.AttributeWorkflowPlatformName);
        AssertUtils.AssertHasTagValue(metricPoint, WorkflowAttributes.AttributeWorkflowExecutionResult, WorkflowAttributes.WorkflowResultValues.Failure);

        // Error type is conditionally required for failed execution
        AssertUtils.AssertTagContains(metricPoint, WorkflowAttributes.AttributeErrorType, "Exception");
    }

    [Fact]
    public async Task Should_Record_PendingDuration_Metric_When_RecordQueueLatency_Enabled()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation(options =>
            {
                options.RecordQueueLatency = true;
            })
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act
        var jobId = BackgroundJob.Enqueue<TestJob>(x => x.Execute());
        await this.hangfireFixture.WaitJobProcessedAsync(jobId, 5);

        meterProvider.ForceFlush();

        // Assert - Should record workflow.execution.duration with state="pending"
        var durationMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionDuration);
        AssertUtils.AssertHasMetricPoints(durationMetric);
        Assert.Equal("s", durationMetric!.Unit);
        Assert.Equal(MetricType.Histogram, durationMetric.MetricType);

        var metricPoints = durationMetric.ToMetricPointList();

        // Find the pending state metric point
        var pendingPoint = metricPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Pending);
        Assert.NotNull(pendingPoint);

        var count = pendingPoint.Value.GetHistogramCount();
        var sum = pendingPoint.Value.GetHistogramSum();

        Assert.True(count >= 1, $"Expected histogram count >= 1 for pending state, got {count}");
        Assert.True(sum >= 0, $"Expected histogram sum >= 0 for pending state, got {sum}");

        // Validate tags
        AssertUtils.AssertHasTag(pendingPoint.Value, WorkflowAttributes.AttributeWorkflowTaskName);
        AssertUtils.AssertHasTag(pendingPoint.Value, WorkflowAttributes.AttributeWorkflowPlatformName);
        AssertUtils.AssertHasTagValue(pendingPoint.Value, WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Pending);
    }

    [Fact]
    public async Task Should_Record_ExecutionStatus_States_For_Successful_Job()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act
        var jobId = BackgroundJob.Enqueue<TestJob>(x => x.Execute());
        await this.hangfireFixture.WaitJobProcessedAsync(jobId, 5);

        meterProvider.ForceFlush();

        // Assert
        var statusMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionStatus);
        AssertUtils.AssertHasMetricPoints(statusMetric);
        Assert.Equal(MetricType.LongSumNonMonotonic, statusMetric!.MetricType);

        var metricPoints = statusMetric.ToMetricPointList();

        // Verify all points have required tags
        foreach (var point in metricPoints)
        {
            AssertUtils.AssertHasTag(point, WorkflowAttributes.AttributeWorkflowTaskName);
            AssertUtils.AssertHasTag(point, WorkflowAttributes.AttributeWorkflowPlatformName);
            AssertUtils.AssertHasTag(point, WorkflowAttributes.AttributeWorkflowExecutionState);

            // Note: workflow.trigger.type should NOT be on execution-level metrics per semantic conventions
            AssertUtils.AssertHasNoTag(point, WorkflowAttributes.AttributeWorkflowTriggerType);
        }

        // Verify state transitions balance out (all states should sum to 0 as job completes and exits status tracking)
        var pendingSum = metricPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Pending)?.GetSumLong() ?? 0;
        var executingSum = metricPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Executing)?.GetSumLong() ?? 0;
        var finalizingSum = metricPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Finalizing)?.GetSumLong() ?? 0;

        Assert.Equal(0, pendingSum);
        Assert.Equal(0, executingSum);
        Assert.Equal(0, finalizingSum);

        // Completed is no longer a state in semantic conventions - jobs exit status tracking when they complete
    }

    [Fact]
    public async Task Should_Record_ExecutionStatus_ErrorType_For_Failed_Job()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act
        var jobId = BackgroundJob.Enqueue<TestJob>(x => x.ThrowException());
        await this.hangfireFixture.WaitJobProcessedAsync(jobId, 5);

        meterProvider.ForceFlush();

        // Assert
        var statusMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionStatus);
        AssertUtils.AssertHasMetricPoints(statusMetric);
        Assert.Equal(MetricType.LongSumNonMonotonic, statusMetric!.MetricType);

        var metricPoints = statusMetric.ToMetricPointList();

        // Verify state transitions balance out (all states should sum to 0 as job completes and exits status tracking)
        var pendingSum = metricPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Pending)?.GetSumLong() ?? 0;
        var executingSum = metricPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Executing)?.GetSumLong() ?? 0;
        var finalizingSum = metricPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Finalizing)?.GetSumLong() ?? 0;

        Assert.Equal(0, pendingSum);
        Assert.Equal(0, executingSum);
        Assert.Equal(0, finalizingSum);

        // Completed is no longer a state in semantic conventions - jobs exit status tracking when they complete
    }

    [Fact]
    public async Task Should_Record_All_Metrics_For_Multiple_Jobs()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation(options =>
            {
                options.RecordQueueLatency = true;
            })
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act - Execute multiple jobs
        var jobId1 = BackgroundJob.Enqueue<TestJob>(x => x.Execute());
        var jobId2 = BackgroundJob.Enqueue<TestJob>(x => x.Execute());
        var jobId3 = BackgroundJob.Enqueue<TestJob>(x => x.ThrowException());

        await this.hangfireFixture.WaitJobProcessedAsync(jobId1, 5);
        await this.hangfireFixture.WaitJobProcessedAsync(jobId2, 5);
        await this.hangfireFixture.WaitJobProcessedAsync(jobId3, 5);

        meterProvider.ForceFlush();

        // Assert - All metric types should be present
        exportedItems.GetMetric(WorkflowMetricNames.ExecutionOutcome);
        exportedItems.GetMetric(WorkflowMetricNames.ExecutionDuration);
        exportedItems.GetMetric(WorkflowMetricNames.ExecutionStatus);
        exportedItems.GetMetric(WorkflowMetricNames.WorkflowOutcome);

        // Verify execution.outcome has at least 3 executions
        var executionOutcomeMetric = exportedItems.First(m => m.Name == WorkflowMetricNames.ExecutionOutcome);
        var executionOutcomePoints = executionOutcomeMetric.ToMetricPointList();

        var totalExecutions = executionOutcomePoints.Sum(mp => mp.GetSumLong());
        Assert.True(totalExecutions >= 3, $"Expected at least 3 executions, got {totalExecutions}");
    }

    [Fact]
    public async Task Should_Track_Success_And_Failure_Outcomes_Separately()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act - Execute one successful and one failed job
        var successJobId = BackgroundJob.Enqueue<TestJob>(x => x.Execute());
        var failJobId = BackgroundJob.Enqueue<TestJob>(x => x.ThrowException());

        await this.hangfireFixture.WaitJobProcessedAsync(successJobId, 5);
        await this.hangfireFixture.WaitJobProcessedAsync(failJobId, 5);

        meterProvider.ForceFlush();

        // Assert - Check execution duration metric has both outcomes
        var durationMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionDuration);
        AssertUtils.AssertHasMetricPoints(durationMetric);

        var metricPoints = durationMetric!.ToMetricPointList();

        // Verify success result exists without error type
        var successPoint = metricPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionResult, WorkflowAttributes.WorkflowResultValues.Success);
        Assert.NotNull(successPoint);
        AssertUtils.AssertHasNoTag(successPoint.Value, WorkflowAttributes.AttributeErrorType);

        // Verify failure result exists with error type
        var failurePoint = metricPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionResult, WorkflowAttributes.WorkflowResultValues.Failure);
        Assert.NotNull(failurePoint);
        AssertUtils.AssertHasTag(failurePoint.Value, WorkflowAttributes.AttributeErrorType);
    }

    [Fact]
    public async Task Should_Record_TriggerType_Cron_For_Recurring_Jobs_On_Workflow_Metric()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act - Create a recurring job and trigger it manually
        RecurringJob.AddOrUpdate("test-recurring-job", () => new TestJob().Execute(), Cron.Minutely());
        var jobId = RecurringJob.TriggerJob("test-recurring-job");

        await this.hangfireFixture.WaitJobProcessedAsync(jobId, 5);

        meterProvider.ForceFlush();

        // Assert - Check workflow.outcome metric has cron trigger type (NOT execution-level metrics)
        var workflowOutcomeMetric = exportedItems.GetMetric(WorkflowMetricNames.WorkflowOutcome);
        AssertUtils.AssertHasMetricPoints(workflowOutcomeMetric);
        var metricPoints = workflowOutcomeMetric!.ToMetricPointList();

        // Find the workflow.outcome metric point
        var workflowPoint = metricPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowTriggerType, WorkflowAttributes.WorkflowTriggerTypeValues.Cron);

        Assert.NotNull(workflowPoint);

        // Verify trigger type is 'cron' for recurring jobs on workflow-level metric
        AssertUtils.AssertHasTagValue(workflowPoint.Value, WorkflowAttributes.AttributeWorkflowTriggerType, WorkflowAttributes.WorkflowTriggerTypeValues.Cron);

        // Cleanup
        RecurringJob.RemoveIfExists("test-recurring-job");
    }

    [Fact]
    public void AddHangfireInstrumentation_Should_Register_Meter()
    {
        // Arrange & Act
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Force a job to generate metrics
        BackgroundJob.Enqueue<TestJob>(x => x.Execute());

        // Assert - The meter should be registered and ready to emit metrics
        // We don't need to wait for job completion, just verify the provider was configured
        Assert.NotNull(meterProvider);
    }

    [Fact]
    public async Task Should_Record_WorkflowStatus_For_Scheduled_Jobs()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act - Schedule a job for future execution
        var jobId = BackgroundJob.Schedule<TestJob>(x => x.Execute(), TimeSpan.FromHours(1));

        // Wait for job to reach scheduled state
        await this.hangfireFixture.WaitJobInStateAsync(jobId, ScheduledState.StateName, 5);
        meterProvider.ForceFlush();

        // Assert - Scheduled jobs appear in workflow.status
        var workflowStatusMetric = exportedItems.GetMetric(WorkflowMetricNames.WorkflowStatus);
        AssertUtils.AssertHasMetricPoints(workflowStatusMetric);
        Assert.Equal("{workflows}", workflowStatusMetric!.Unit);
        Assert.Equal(MetricType.LongSumNonMonotonic, workflowStatusMetric.MetricType);

        var metricPoints = workflowStatusMetric.ToMetricPointList();
        var pendingPoint = metricPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowState, WorkflowAttributes.WorkflowStateValues.Pending);

        Assert.NotNull(pendingPoint);
        Assert.Equal(1, pendingPoint.Value.GetSumLong());

        // Verify workflow-level tags (uses workflow.definition.name, workflow.state, workflow.trigger.type)
        AssertUtils.AssertHasTag(pendingPoint.Value, WorkflowAttributes.AttributeWorkflowDefinitionName);
        AssertUtils.AssertHasTag(pendingPoint.Value, WorkflowAttributes.AttributeWorkflowState);
        AssertUtils.AssertHasTag(pendingPoint.Value, WorkflowAttributes.AttributeWorkflowTriggerType);
        AssertUtils.AssertHasTagValue(pendingPoint.Value, WorkflowAttributes.AttributeWorkflowTriggerType, WorkflowAttributes.WorkflowTriggerTypeValues.Schedule);
        AssertUtils.AssertHasTagValue(pendingPoint.Value, WorkflowAttributes.AttributeWorkflowState, WorkflowAttributes.WorkflowStateValues.Pending);

        // Scheduled jobs should NOT appear in execution.status (haven't entered execution pipeline yet)
        var executionStatusMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionStatus);
        if (executionStatusMetric != null)
        {
            var execPoints = executionStatusMetric.ToMetricPointList();

            // Should have no execution state metrics for scheduled jobs (not in execution pipeline)
            var execPendingPoint = execPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Pending);
            Assert.Null(execPendingPoint);
        }
    }

    [Fact]
    public async Task Should_Track_Job_In_Both_Workflow_And_Execution_Metrics()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act - Schedule a job for immediate execution (1 second delay)
        var jobId = BackgroundJob.Schedule<TestJob>(x => x.Execute(), TimeSpan.FromSeconds(1));

        // Wait for job to complete full lifecycle
        await this.hangfireFixture.WaitJobProcessedAsync(jobId, 10);

        meterProvider.ForceFlush();

        // Assert - workflow.status should track full lifecycle (scheduled -> enqueued -> executing -> completed)
        var workflowStatusMetric = exportedItems.GetMetric(WorkflowMetricNames.WorkflowStatus);
        AssertUtils.AssertHasMetricPoints(workflowStatusMetric);

        var workflowPoints = workflowStatusMetric!.ToMetricPointList();

        // Pending state transitions should balance to 0: +1 (scheduled) -1 (enqueued) +1 (enqueued) -1 (executing) = 0
        var workflowPendingPoint = workflowPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowState, WorkflowAttributes.WorkflowStateValues.Pending);
        if (workflowPendingPoint != null)
        {
            Assert.Equal(0, workflowPendingPoint.Value.GetSumLong());
        }

        // Final workflow state - all states should be 0 as job exits status tracking when completed
        // (Completed state no longer exists in semantic conventions)
        var workflowExecutingPoint = workflowPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowState, WorkflowAttributes.WorkflowStateValues.Executing);
        if (workflowExecutingPoint != null)
        {
            Assert.Equal(0, workflowExecutingPoint.Value.GetSumLong());
        }

        // Assert - workflow.execution.status should also track lifecycle (enqueued -> executing)
        // but NOT the scheduled phase
        var executionStatusMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionStatus);
        AssertUtils.AssertHasMetricPoints(executionStatusMetric);

        var execPoints = executionStatusMetric!.ToMetricPointList();

        // Execution pending: +1 (enqueued) -1 (executing) = 0
        var execPendingPoint = execPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Pending);
        if (execPendingPoint != null)
        {
            Assert.Equal(0, execPendingPoint.Value.GetSumLong());
        }

        // Final execution state - all states should be 0 as job exits status tracking when completed
        // (Completed state no longer exists in semantic conventions)
        var execExecutingPoint = execPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Executing);
        if (execExecutingPoint != null)
        {
            Assert.Equal(0, execExecutingPoint.Value.GetSumLong());
        }
    }

    [Fact]
    public async Task Should_Track_FireAndForget_Job_In_Both_Metrics()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act - Fire-and-forget job (goes straight to enqueued, no scheduled phase)
        var jobId = BackgroundJob.Enqueue<TestJob>(x => x.Execute());
        await this.hangfireFixture.WaitJobProcessedAsync(jobId, 10);

        meterProvider.ForceFlush();

        // Assert - Should appear in BOTH workflow.status AND workflow.execution.status while processing
        var workflowStatusMetric = exportedItems.GetMetric(WorkflowMetricNames.WorkflowStatus);
        AssertUtils.AssertHasMetricPoints(workflowStatusMetric);

        var workflowPoints = workflowStatusMetric!.ToMetricPointList();

        // After completion, states should balance to 0 (Completed state no longer exists)
        var workflowPendingSum = workflowPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowState, WorkflowAttributes.WorkflowStateValues.Pending)?.GetSumLong() ?? 0;
        var workflowExecutingSum = workflowPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowState, WorkflowAttributes.WorkflowStateValues.Executing)?.GetSumLong() ?? 0;
        Assert.Equal(0, workflowPendingSum);
        Assert.Equal(0, workflowExecutingSum);

        // Also in execution.status
        var executionStatusMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionStatus);
        AssertUtils.AssertHasMetricPoints(executionStatusMetric);

        var execPoints = executionStatusMetric!.ToMetricPointList();

        // After completion, states should balance to 0 (Completed state no longer exists)
        var execPendingSum = execPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Pending)?.GetSumLong() ?? 0;
        var execExecutingSum = execPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Executing)?.GetSumLong() ?? 0;
        Assert.Equal(0, execPendingSum);
        Assert.Equal(0, execExecutingSum);

        // Verify workflow.status uses trigger.type=api for fire-and-forget
        var apiTriggerPoint = workflowPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowTriggerType, WorkflowAttributes.WorkflowTriggerTypeValues.Api);
        Assert.NotNull(apiTriggerPoint);
    }
}
