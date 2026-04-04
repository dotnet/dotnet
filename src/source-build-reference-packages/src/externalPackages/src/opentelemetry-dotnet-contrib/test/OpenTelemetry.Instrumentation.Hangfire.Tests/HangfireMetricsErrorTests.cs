// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Hangfire;
using OpenTelemetry.Instrumentation.Hangfire.Implementation;
using OpenTelemetry.Instrumentation.Hangfire.Tests.Utils;
using OpenTelemetry.Metrics;
using Xunit;

namespace OpenTelemetry.Instrumentation.Hangfire.Tests;

[Collection("Hangfire")]
public class HangfireMetricsErrorTests : IClassFixture<HangfireFixture>
{
    private readonly HangfireFixture hangfireFixture;

    public HangfireMetricsErrorTests(HangfireFixture hangfireFixture)
    {
        this.hangfireFixture = hangfireFixture;
    }

    [Fact]
    public async Task Should_Record_ExecutionErrors_Metric_When_Job_Fails()
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
        var errorMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionErrors);
        AssertUtils.AssertHasMetricPoints(errorMetric);
        Assert.Equal("{error}", errorMetric!.Unit);
        Assert.Equal(MetricType.LongSum, errorMetric.MetricType);

        var metricPoints = errorMetric.ToMetricPointList();

        var metricPoint = metricPoints.First();
        var sum = metricPoint.GetSumLong();
        Assert.True(sum >= 1, $"Expected workflow.execution.errors sum >= 1, got {sum}");

        // Validate required tags
        AssertUtils.AssertTagContains(metricPoint, WorkflowAttributes.AttributeErrorType, "Exception");
        AssertUtils.AssertTagContains(metricPoint, WorkflowAttributes.AttributeWorkflowTaskName, "TestJob.ThrowException");

        // Validate recommended tags
        AssertUtils.AssertHasTagValue(metricPoint, WorkflowAttributes.AttributeWorkflowPlatformName, WorkflowAttributes.WorkflowPlatformNameValues.Hangfire);
    }

    [Fact]
    public async Task Should_Not_Record_ExecutionErrors_Metric_When_Job_Succeeds()
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

        AssertUtils.AssertMetricNotRecordedOrZero(exportedItems, WorkflowMetricNames.ExecutionErrors);
    }

    [Fact]
    public async Task Should_Increment_ExecutionErrors_Counter_For_Multiple_Failures()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act - Execute multiple failing jobs
        var jobId1 = BackgroundJob.Enqueue<TestJob>(x => x.ThrowException());
        var jobId2 = BackgroundJob.Enqueue<TestJob>(x => x.ThrowException());
        var jobId3 = BackgroundJob.Enqueue<TestJob>(x => x.ThrowException());

        await this.hangfireFixture.WaitJobProcessedAsync(jobId1, 5);
        await this.hangfireFixture.WaitJobProcessedAsync(jobId2, 5);
        await this.hangfireFixture.WaitJobProcessedAsync(jobId3, 5);

        meterProvider.ForceFlush();

        // Assert
        var errorMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionErrors);
        AssertUtils.AssertHasMetricPoints(errorMetric);

        var metricPoints = errorMetric!.ToMetricPointList();
        var totalErrors = metricPoints.Sum(mp => mp.GetSumLong());
        Assert.True(totalErrors >= 3, $"Expected at least 3 errors, got {totalErrors}");
    }

    [Fact]
    public async Task Should_Record_Different_Error_Types_Separately()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act - Execute jobs with different exception types
        var jobId1 = BackgroundJob.Enqueue<TestJob>(x => x.ThrowException());
        var jobId2 = BackgroundJob.Enqueue<TestJobWithCustomException>(x => x.ThrowCustomException());

        await this.hangfireFixture.WaitJobProcessedAsync(jobId1, 5);
        await this.hangfireFixture.WaitJobProcessedAsync(jobId2, 5);

        meterProvider.ForceFlush();

        // Assert
        var errorMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionErrors);
        AssertUtils.AssertHasMetricPoints(errorMetric);
        var metricPoints = errorMetric!.ToMetricPointList();

        // Should have at least 2 different metric points for different error types
        var distinctErrorTypes = metricPoints
            .Select(mp => mp.GetTagValue(WorkflowAttributes.AttributeErrorType))
            .Distinct()
            .ToList();

        Assert.True(distinctErrorTypes.Count >= 2, $"Expected at least 2 distinct error types, got {distinctErrorTypes.Count}");
        Assert.Contains(distinctErrorTypes, et => et != null && et.Contains("Exception"));
        Assert.Contains(distinctErrorTypes, et => et != null && et.Contains("CustomTestException"));
    }

    [Fact]
    public async Task Should_Track_Success_And_Failure_Separately()
    {
        // Arrange
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHangfireInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        // Act - Execute one successful and two failed jobs
        var successJobId = BackgroundJob.Enqueue<TestJob>(x => x.Execute());
        var failJobId1 = BackgroundJob.Enqueue<TestJob>(x => x.ThrowException());
        var failJobId2 = BackgroundJob.Enqueue<TestJob>(x => x.ThrowException());

        await this.hangfireFixture.WaitJobProcessedAsync(successJobId, 5);
        await this.hangfireFixture.WaitJobProcessedAsync(failJobId1, 5);
        await this.hangfireFixture.WaitJobProcessedAsync(failJobId2, 5);

        meterProvider.ForceFlush();

        // Assert - errors metric should only count the 2 failures
        var errorMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionErrors);
        AssertUtils.AssertHasMetricPoints(errorMetric);
        var metricPoints = errorMetric!.ToMetricPointList();

        var totalErrors = metricPoints.Sum(mp => mp.GetSumLong());
        Assert.True(totalErrors >= 2, $"Expected at least 2 errors, got {totalErrors}");

        // All error metrics should have error.type tag
        foreach (var point in metricPoints)
        {
            AssertUtils.AssertHasTag(point, WorkflowAttributes.AttributeErrorType);
        }
    }

    [Fact]
    public async Task Should_Record_All_Required_And_Recommended_Tags()
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
        var errorMetric = exportedItems.GetMetric(WorkflowMetricNames.ExecutionErrors);
        AssertUtils.AssertHasMetricPoints(errorMetric);
        var metricPoints = errorMetric!.ToMetricPointList();

        var metricPoint = metricPoints.First();

        // Required tags per semantic conventions
        AssertUtils.AssertHasTag(metricPoint, WorkflowAttributes.AttributeErrorType);
        AssertUtils.AssertHasTag(metricPoint, WorkflowAttributes.AttributeWorkflowTaskName);

        // Recommended tags per semantic conventions
        AssertUtils.AssertHasTagValue(metricPoint, WorkflowAttributes.AttributeWorkflowPlatformName, WorkflowAttributes.WorkflowPlatformNameValues.Hangfire);
    }
}
