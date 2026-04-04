// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Quartz;

namespace OpenTelemetry.Instrumentation.Quartz.Tests;

internal class TestJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        try
        {
            var jobExecTimestamps = (List<DateTime>)context.Scheduler.Context.Get("DATESTAMPS");
            var barrier = (Barrier)context.Scheduler.Context.Get("BARRIER");

            jobExecTimestamps.Add(DateTime.UtcNow);

            barrier.SignalAndWait(TimeSpan.FromSeconds(125));
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            Console.Write(e);
        }

        return Task.CompletedTask;
    }
}
