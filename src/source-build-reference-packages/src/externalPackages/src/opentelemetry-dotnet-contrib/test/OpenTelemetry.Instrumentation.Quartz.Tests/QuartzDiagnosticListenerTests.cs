// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Trace;
using Quartz;
using Xunit;

namespace OpenTelemetry.Instrumentation.Quartz.Tests;

public class QuartzDiagnosticListenerTests
{
    private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(125);

    public QuartzDiagnosticListenerTests()
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
    }

    [Fact]
    public async Task Should_Create_Activity()
    {
        // Arrange
        var barrier = new Barrier(2);
        List<DateTime> jobExecTimestamps = [];

        var exportedItems = new List<Activity>();
        using var tel = Sdk.CreateTracerProviderBuilder()
            .SetSampler(new AlwaysOnSampler())
            .AddQuartzInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();
        var schedulerConfig = SchedulerBuilder.Create("AUTO", "Scheduler");
        schedulerConfig.UseDefaultThreadPool(x => x.MaxConcurrency = 10);
        var scheduler = await schedulerConfig.BuildScheduler();

        scheduler.Context.Put("BARRIER", barrier);
        scheduler.Context.Put("DATESTAMPS", jobExecTimestamps);
        await scheduler.Start();

        var jobDataMap = new JobDataMap { { "A", "B" } };

        var name = Guid.NewGuid().ToString();
        var job = JobBuilder.Create<TestJob>()
            .WithIdentity(name, SchedulerConstants.DefaultGroup)
            .UsingJobData(jobDataMap)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity(name, SchedulerConstants.DefaultGroup)
            .StartNow()
            .Build();

        // Act
        await scheduler.ScheduleJob(job, trigger);

        barrier.SignalAndWait(TestTimeout);

        await scheduler.Shutdown(true);

        // Assert
        Assert.Single(exportedItems);
        var activity = exportedItems[0];

        Assert.Contains("execute ", activity.DisplayName);
        Assert.Equal("Quartz.Job.Execute", activity.OperationName);
        Assert.Equal(ActivityKind.Internal, activity.Kind);
        Assert.Equal("Scheduler", activity.Tags.SingleOrDefault(t => t.Key.Equals("scheduler.name")).Value);
        Assert.Equal(SchedulerConstants.DefaultGroup, activity.Tags.SingleOrDefault(t => t.Key.Equals("job.group")).Value);
        Assert.Equal(SchedulerConstants.DefaultGroup, activity.Tags.SingleOrDefault(t => t.Key.Equals("trigger.group")).Value);
    }

    [Fact]
    public async Task Should_Create_Activity_And_Enrich_When_Enrich()
    {
        // Arrange
        var barrier = new Barrier(2);
        List<DateTime> jobExecTimestamps = [];

        var exportedItems = new List<Activity>();

        using var tel = Sdk.CreateTracerProviderBuilder()
            .SetSampler(new AlwaysOnSampler())
            .AddQuartzInstrumentation(q =>
                q.Enrich = (a, _, payload) =>
                {
                    if (payload is IJobDetail jobDetail)
                    {
                        var dataMap = jobDetail.JobDataMap;
                        if (dataMap.TryGetValue("TestId", out var value))
                        {
                            a.SetTag("test.id", value);
                        }
                    }
                })
            .AddInMemoryExporter(exportedItems)
            .Build();

        var schedulerConfig = SchedulerBuilder.Create("AUTO", "Scheduler");
        schedulerConfig.UseDefaultThreadPool(x => x.MaxConcurrency = 10);
        var scheduler = await schedulerConfig.BuildScheduler();

        scheduler.Context.Put("BARRIER", barrier);
        scheduler.Context.Put("DATESTAMPS", jobExecTimestamps);
        await scheduler.Start();

        var testId = Guid.NewGuid().ToString();
        var jobDataMap = new JobDataMap { { "TestId", testId } };

        var name = Guid.NewGuid().ToString();
        var job = JobBuilder.Create<TestJob>()
            .WithIdentity(name, SchedulerConstants.DefaultGroup)
            .UsingJobData(jobDataMap)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity(name, SchedulerConstants.DefaultGroup)
            .StartNow()
            .Build();

        // Act
        await scheduler.ScheduleJob(job, trigger);

        barrier.SignalAndWait(TestTimeout);

        await scheduler.Shutdown(true);

        // Assert
        Assert.Single(exportedItems);
        var activity = exportedItems[0];

        Assert.Equal("Quartz.Job.Execute", activity.OperationName);
        Assert.Equal(ActivityKind.Internal, activity.Kind);
        Assert.Equal("Scheduler", activity.Tags.SingleOrDefault(t => t.Key.Equals("scheduler.name")).Value);
        Assert.Equal(SchedulerConstants.DefaultGroup, activity.Tags.SingleOrDefault(t => t.Key.Equals("job.group")).Value);
        Assert.Equal(SchedulerConstants.DefaultGroup, activity.Tags.SingleOrDefault(t => t.Key.Equals("trigger.group")).Value);
        Assert.Equal(testId, activity.Tags.SingleOrDefault(t => t.Key.Equals("test.id")).Value);
    }

    [Fact]
    public async Task Should_Record_Exception_When_Record_Exception_Enabled()
    {
        // Arrange
        var barrier = new Barrier(2);
        List<DateTime> jobExecTimestamps = [];

        var exportedItems = new List<Activity>();

        using var tel = Sdk.CreateTracerProviderBuilder()
            .SetSampler(new AlwaysOnSampler())
            .AddQuartzInstrumentation(q =>
                q.RecordException = true)
            .AddInMemoryExporter(exportedItems)
            .Build();

        var schedulerConfig = SchedulerBuilder.Create("AUTO", "Scheduler");
        schedulerConfig.UseDefaultThreadPool(x => x.MaxConcurrency = 10);
        var scheduler = await schedulerConfig.BuildScheduler();

        scheduler.Context.Put("BARRIER", barrier);
        scheduler.Context.Put("DATESTAMPS", jobExecTimestamps);
        await scheduler.Start();

        var testId = Guid.NewGuid().ToString();
        var jobDataMap = new JobDataMap { { "TestId", testId } };

        var name = Guid.NewGuid().ToString();
        var job = JobBuilder.Create<TestJobExecutionExceptionJob>()
            .WithIdentity(name, SchedulerConstants.DefaultGroup)
            .UsingJobData(jobDataMap)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity(name, SchedulerConstants.DefaultGroup)
            .StartNow()
            .Build();

        // Act
        await scheduler.ScheduleJob(job, trigger);

        barrier.SignalAndWait(TimeSpan.FromSeconds(1));

        await scheduler.Shutdown(true);

        // Assert
        Assert.Single(exportedItems);
        var activity = exportedItems[0];

        Assert.Equal(ActivityStatusCode.Error, activity.Status);
        var exception = Assert.Single(activity.Events);
        Assert.Equal("exception", exception.Name);
        Assert.Equal("Catch me if you can!", exception.Tags.SingleOrDefault(t => t.Key.Equals("exception.message")).Value);
    }

    [Fact]
    public async Task Should_Enrich_Exception_When_Record_Exception_Enabled_And_Enrich()
    {
        // Arrange
        var barrier = new Barrier(2);
        List<DateTime> jobExecTimestamps = [];

        var exportedItems = new List<Activity>();

        using var tel = Sdk.CreateTracerProviderBuilder()
            .SetSampler(new AlwaysOnSampler())
            .AddQuartzInstrumentation(q =>
            {
                q.RecordException = true;
                q.Enrich = (a, _, p) =>
                {
                    if (p is IJobDetail jobDetail)
                    {
                        var dataMap = jobDetail.JobDataMap;
                        if (dataMap.TryGetValue("TestId", out var value))
                        {
                            a.SetTag("test.id", value);
                        }
                    }
                };
            })
            .AddInMemoryExporter(exportedItems)
            .Build();

        var schedulerConfig = SchedulerBuilder.Create("AUTO", "Scheduler");
        schedulerConfig.UseDefaultThreadPool(x => x.MaxConcurrency = 10);
        var scheduler = await schedulerConfig.BuildScheduler();

        scheduler.Context.Put("BARRIER", barrier);
        scheduler.Context.Put("DATESTAMPS", jobExecTimestamps);
        await scheduler.Start();

        var testId = Guid.NewGuid().ToString();
        var jobDataMap = new JobDataMap { { "TestId", testId } };

        var name = Guid.NewGuid().ToString();
        var job = JobBuilder.Create<TestJobExecutionExceptionJob>()
            .WithIdentity(name, SchedulerConstants.DefaultGroup)
            .UsingJobData(jobDataMap)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity(name, SchedulerConstants.DefaultGroup)
            .StartNow()
            .Build();

        // Act
        await scheduler.ScheduleJob(job, trigger);

        barrier.SignalAndWait(TimeSpan.FromSeconds(1));

        await scheduler.Shutdown(true);

        // Assert
        Assert.Single(exportedItems);
        var activity = exportedItems[0];

        Assert.Equal(ActivityStatusCode.Error, activity.Status);
        var exception = Assert.Single(activity.Events);
        Assert.Equal("exception", exception.Name);
        Assert.Equal("Catch me if you can!", exception.Tags.SingleOrDefault(t => t.Key.Equals("exception.message")).Value);
        Assert.Equal(testId, activity.Tags.SingleOrDefault(t => t.Key.Equals("test.id")).Value);
    }

    [Fact]
    public async Task Should_Creates_Activity_Event_On_Job_Execution_Exception()
    {
        // Arrange
        var barrier = new Barrier(2);
        List<DateTime> jobExecTimestamps = [];

        var exportedItems = new List<Activity>();
        using var tel = Sdk.CreateTracerProviderBuilder()
            .SetSampler(new AlwaysOnSampler())
            .AddQuartzInstrumentation(q =>
            {
                q.RecordException = true;
                q.Enrich = (_, s, _) =>
                {
                    if (s.Equals("OnException"))
                    {
                        throw new Exception("Enrich Exception");
                    }
                };
            })
            .AddInMemoryExporter(exportedItems)
            .Build();

        var schedulerConfig = SchedulerBuilder.Create("AUTO", "Scheduler");
        schedulerConfig.UseDefaultThreadPool(x => x.MaxConcurrency = 10);
        var scheduler = await schedulerConfig.BuildScheduler();

        scheduler.Context.Put("BARRIER", barrier);
        scheduler.Context.Put("DATESTAMPS", jobExecTimestamps);
        await scheduler.Start();

        var testId = Guid.NewGuid().ToString();
        var jobDataMap = new JobDataMap { { "TestId", testId } };

        var name = Guid.NewGuid().ToString();
        var job = JobBuilder.Create<TestJobExecutionExceptionJob>()
            .WithIdentity(name, SchedulerConstants.DefaultGroup)
            .UsingJobData(jobDataMap)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity(name, SchedulerConstants.DefaultGroup)
            .StartNow()
            .Build();

        // Act
        await scheduler.ScheduleJob(job, trigger);

        barrier.SignalAndWait(TimeSpan.FromSeconds(1));

        await scheduler.Shutdown(true);

        // Assert
        Assert.Single(exportedItems);
        var activity = exportedItems[0];
        Assert.Equal("exception", activity.Events.First().Name);
        Assert.Equal("Quartz.JobExecutionException", activity.Events.First().Tags.SingleOrDefault(t => t.Key.Equals(SemanticConventions.AttributeExceptionType)).Value);
        Assert.Equal("Catch me if you can!", activity.Events.First().Tags.SingleOrDefault(t => t.Key.Equals(SemanticConventions.AttributeExceptionMessage)).Value);
    }

    [Fact]
    public async Task Should_Not_Record_Activity_When_Trace_Operation_Is_Not_Present()
    {
        // Arrange
        var barrier = new Barrier(2);
        List<DateTime> jobExecTimestamps = [];

        var exportedItems = new List<Activity>();

        using var tel = Sdk.CreateTracerProviderBuilder()
            .SetSampler(new AlwaysOnSampler())
            .AddQuartzInstrumentation(q =>
            {
                q.TracedOperations = [];
            })
            .AddInMemoryExporter(exportedItems)
            .Build();

        var schedulerConfig = SchedulerBuilder.Create("AUTO", "Scheduler");
        schedulerConfig.UseDefaultThreadPool(x => x.MaxConcurrency = 10);
        var scheduler = await schedulerConfig.BuildScheduler();

        scheduler.Context.Put("BARRIER", barrier);
        scheduler.Context.Put("DATESTAMPS", jobExecTimestamps);
        await scheduler.Start();

        var testId = Guid.NewGuid().ToString();
        var jobDataMap = new JobDataMap { { "TestId", testId } };

        var name = Guid.NewGuid().ToString();
        var job = JobBuilder.Create<TestJob>()
            .WithIdentity(name, SchedulerConstants.DefaultGroup)
            .UsingJobData(jobDataMap)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity(name, SchedulerConstants.DefaultGroup)
            .StartNow()
            .Build();

        // Act
        await scheduler.ScheduleJob(job, trigger);

        barrier.SignalAndWait(TestTimeout);

        await scheduler.Shutdown(true);

        // Assert
        Assert.Empty(exportedItems);
    }
}
