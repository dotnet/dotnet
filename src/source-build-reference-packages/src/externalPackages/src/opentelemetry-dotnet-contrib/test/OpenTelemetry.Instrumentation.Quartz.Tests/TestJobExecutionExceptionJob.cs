// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Quartz;

namespace OpenTelemetry.Instrumentation.Quartz.Tests;

internal class TestJobExecutionExceptionJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        throw new JobExecutionException("Catch me if you can!");
    }
}
