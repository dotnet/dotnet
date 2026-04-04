// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Hangfire;
using OpenTelemetry.Instrumentation.Hangfire.Implementation;
using OpenTelemetry.Instrumentation.Hangfire.Tests.Utils;
using OpenTelemetry.Metrics;
using Xunit;

namespace OpenTelemetry.Instrumentation.Hangfire.Tests;

[Collection("Hangfire")]
public class HangfireQueueLatencyTests : IClassFixture<HangfireFixture>
{
    private readonly HangfireFixture hangfireFixture;

    public HangfireQueueLatencyTests(HangfireFixture hangfireFixture)
    {
        this.hangfireFixture = hangfireFixture;
    }

    [Fact]
    public async Task Should_NOT_Record_PendingDuration_By_Default()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation() // Default: RecordQueueLatency = false
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act
        var jobId = BackgroundJob.Enqueue<TestJob>(x => x.Execute());
        await this.hangfireFixture.WaitJobProcessedAsync(jobId, 5);

        meterProvider.ForceFlush();

        // Assert - workflow.execution.duration with state="pending" should NOT be present
        var durationMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionDuration);

        // Duration metric will exist (for state="executing"), but NOT for state="pending"
        Assert.NotNull(durationMetric);
        var metricPoints = durationMetric.ToMetricPointList();

        // Ensure NO pending state metric point exists
        var pendingPoints = metricPoints.Where(mp =>
        {
            var stateValue = mp.GetTagValue(WorkflowAttributes.AttributeWorkflowExecutionState);
            return stateValue != null && stateValue.Equals(WorkflowAttributes.WorkflowStateValues.Pending);
        }).ToList();

        Assert.Empty(pendingPoints);
    }

    [Fact]
    public async Task Should_Record_PendingDuration_When_RecordQueueLatency_Enabled()
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

        // Assert - workflow.execution.duration with state="pending" should be present
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
        AssertUtils.AssertHasTagValue(pendingPoint.Value, WorkflowAttributes.AttributeWorkflowExecutionResult, WorkflowAttributes.WorkflowResultValues.Success);
    }

    [Fact]
    public async Task Should_NOT_Record_PendingDuration_When_Explicitly_Disabled()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation(options =>
            {
                options.RecordQueueLatency = false; // Explicitly disabled
            })
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act
        var jobId = BackgroundJob.Enqueue<TestJob>(x => x.Execute());
        await this.hangfireFixture.WaitJobProcessedAsync(jobId, 5);

        meterProvider.ForceFlush();

        // Assert - workflow.execution.duration with state="pending" should NOT be present
        var durationMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionDuration);

        // Duration metric will exist (for state="executing"), but NOT for state="pending"
        Assert.NotNull(durationMetric);
        var metricPoints = durationMetric.ToMetricPointList();

        // Ensure NO pending state metric point exists
        var pendingPoints = metricPoints.Where(mp =>
        {
            var stateValue = mp.GetTagValue(WorkflowAttributes.AttributeWorkflowExecutionState);
            return stateValue != null && stateValue.Equals(WorkflowAttributes.WorkflowStateValues.Pending);
        }).ToList();

        Assert.Empty(pendingPoints);
    }

    [Fact]
    public async Task Should_Record_Both_Pending_And_Executing_Duration_When_Enabled()
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

        // Assert - Both pending and executing durations should be recorded
        var durationMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionDuration);
        AssertUtils.AssertHasMetricPoints(durationMetric);

        var metricPoints = durationMetric!.ToMetricPointList();

        // Verify pending state duration
        var pendingPoint = metricPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Pending);
        Assert.NotNull(pendingPoint);
        Assert.True(pendingPoint.Value.GetHistogramCount() >= 1);

        // Verify executing state duration
        var executingPoint = metricPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Executing);
        Assert.NotNull(executingPoint);
        Assert.True(executingPoint.Value.GetHistogramCount() >= 1);
    }

    [Fact]
    public async Task Should_Record_PendingDuration_For_Failed_Job()
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
        var jobId = BackgroundJob.Enqueue<TestJob>(x => x.ThrowException());
        await this.hangfireFixture.WaitJobProcessedAsync(jobId, 5);

        meterProvider.ForceFlush();

        // Assert - Pending duration should be recorded even for failed jobs
        var durationMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionDuration);
        AssertUtils.AssertHasMetricPoints(durationMetric);

        var metricPoints = durationMetric!.ToMetricPointList();

        // Find pending state metric point (should have success outcome since pending phase succeeded)
        var pendingPoint = metricPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Pending);
        Assert.NotNull(pendingPoint);

        // Pending phase should be successful (job started executing)
        AssertUtils.AssertHasTagValue(pendingPoint.Value, WorkflowAttributes.AttributeWorkflowExecutionResult, WorkflowAttributes.WorkflowResultValues.Success);

        // Executing phase should have failure result
        var executingPoint = metricPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Executing);
        Assert.NotNull(executingPoint);
        AssertUtils.AssertHasTagValue(executingPoint.Value, WorkflowAttributes.AttributeWorkflowExecutionResult, WorkflowAttributes.WorkflowResultValues.Failure);
    }

    [Fact]
    public async Task Should_Record_PendingDuration_For_Multiple_Jobs()
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
        var jobId3 = BackgroundJob.Enqueue<TestJob>(x => x.Execute());

        await this.hangfireFixture.WaitJobProcessedAsync(jobId1, 5);
        await this.hangfireFixture.WaitJobProcessedAsync(jobId2, 5);
        await this.hangfireFixture.WaitJobProcessedAsync(jobId3, 5);

        meterProvider.ForceFlush();

        // Assert - Pending duration should be recorded for all jobs
        var durationMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionDuration);
        AssertUtils.AssertHasMetricPoints(durationMetric);

        var metricPoints = durationMetric!.ToMetricPointList();

        // Find pending state metric point
        var pendingPoint = metricPoints.FindFirstWithTag(WorkflowAttributes.AttributeWorkflowExecutionState, WorkflowAttributes.WorkflowStateValues.Pending);
        Assert.NotNull(pendingPoint);

        var count = pendingPoint.Value.GetHistogramCount();
        Assert.True(count >= 3, $"Expected at least 3 pending duration measurements, got {count}");
    }

    [Fact]
    public void Should_NOT_Register_PendingDurationFilter_When_Disabled()
    {
        // Arrange
        var options = new HangfireMetricsInstrumentationOptions
        {
            RecordQueueLatency = false,
        };

        // Act
        using var instrumentation = new HangfireMetricsInstrumentation(options);

        // Verify no HangfirePendingDurationFilterAttribute is registered
        var pendingDurationFilterExists = GlobalJobFilters.Filters
            .Any(f => f.Instance is HangfirePendingDurationFilterAttribute);
        Assert.False(pendingDurationFilterExists);
    }

    [Fact]
    public void Should_Register_PendingDurationFilter_When_Enabled()
    {
        // Arrange
        var options = new HangfireMetricsInstrumentationOptions
        {
            RecordQueueLatency = true,
        };

        // Act
        using var instrumentation = new HangfireMetricsInstrumentation(options);

        // Verify HangfirePendingDurationFilterAttribute IS registered
        var pendingDurationFilterExists = GlobalJobFilters.Filters
            .Any(f => f.Instance is HangfirePendingDurationFilterAttribute);
        Assert.True(pendingDurationFilterExists);
    }
}
