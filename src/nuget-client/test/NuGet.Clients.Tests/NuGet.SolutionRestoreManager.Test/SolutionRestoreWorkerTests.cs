// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.SolutionRestoreManager.Test
{
    public class SolutionRestoreWorkerTests
    {
        [Fact]
        public void CalculateTimeoutTime_WithTimeoutLargerThanTimeElapsed_ReturnsPositiveValue()
        {
            var startTime = new DateTimeOffset(year: 2021, month: 7, day: 21, hour: 10, minute: 5, second: 20, offset: TimeSpan.Zero);
            var currentTime = new DateTimeOffset(year: 2021, month: 7, day: 21, hour: 10, minute: 7, second: 00, offset: TimeSpan.Zero);
            TimeSpan timeoutSpan = new(hours: 0, minutes: 5, seconds: 0);

            var timeout = SolutionRestoreWorker.CalculateTimeoutTime(startTime: startTime, currentTime: currentTime, timeoutTime: timeoutSpan);
            timeout.TotalMilliseconds.Should().Be(200000);
        }

        [Fact]
        public void CalculateTimeoutTime_WithTimeElapsedLargerThanTimeout_Returns0()
        {
            var startTime = new DateTimeOffset(year: 2021, month: 7, day: 21, hour: 10, minute: 5, second: 20, offset: TimeSpan.Zero);
            var currentTime = new DateTimeOffset(year: 2021, month: 7, day: 21, hour: 11, minute: 0, second: 00, offset: TimeSpan.Zero);
            TimeSpan timeoutSpan = new(hours: 0, minutes: 5, seconds: 0);

            var timeout = SolutionRestoreWorker.CalculateTimeoutTime(startTime: startTime, currentTime: currentTime, timeoutTime: timeoutSpan);
            timeout.TotalMilliseconds.Should().Be(0);
        }

        [Fact]
        public async Task WaitForOnBuildRestoreReadinessCoreAsync_WhenSolutionLoadCompletes_ThenChecksNomination()
        {
            var solutionLoadCompleted = new TaskCompletionSource<bool>();
            var utcNow = new DateTimeOffset(year: 2026, month: 9, day: 16, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero);
            int isAllProjectsNominatedCallCount = 0;

            Task<SolutionRestoreWorker.RestoreReadinessResult> coordinationTask = SolutionRestoreWorker.WaitForOnBuildRestoreReadinessAsync(
                waitForSolutionLoadedAsync: _ => solutionLoadCompleted.Task,
                isAllProjectsNominatedAsync: () =>
                {
                    Interlocked.Increment(ref isAllProjectsNominatedCallCount);
                    return Task.FromResult(true);
                },
                getUtcNow: () => utcNow,
                token: CancellationToken.None);

            isAllProjectsNominatedCallCount.Should().Be(0);

            solutionLoadCompleted.SetResult(true);

            SolutionRestoreWorker.RestoreReadinessResult readiness = await coordinationTask;
            readiness.RestoreReason.Should().Be(ImplicitRestoreReason.AllProjectsNominated);
            readiness.BulkRestoreCoordinationCheckStartTime.Should().Be(utcNow);
            readiness.ProjectsReadyCheckCount.Should().Be(0);
            readiness.ProjectRestoreInfoSourcesCount.Should().Be(-1);
            readiness.ProjectReadyTimings.Should().BeEmpty();
            isAllProjectsNominatedCallCount.Should().Be(1);
        }

        [Fact]
        public async Task WaitForOnBuildRestoreReadinessCoreAsync_WhenNominationsComplete_StopsWaiting()
        {
            var utcNow = new DateTimeOffset(year: 2026, month: 9, day: 16, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero);
            int isAllProjectsNominatedCallCount = 0;

            SolutionRestoreWorker.RestoreReadinessResult readiness = await SolutionRestoreWorker.WaitForOnBuildRestoreReadinessAsync(
                waitForSolutionLoadedAsync: _ => Task.CompletedTask,
                isAllProjectsNominatedAsync: () =>
                    Task.FromResult(Interlocked.Increment(ref isAllProjectsNominatedCallCount) > 1),
                getUtcNow: () => utcNow,
                token: CancellationToken.None);

            readiness.RestoreReason.Should().Be(ImplicitRestoreReason.AllProjectsNominated);
            readiness.BulkRestoreCoordinationCheckStartTime.Should().Be(utcNow);
            readiness.ProjectsReadyCheckCount.Should().Be(0);
            readiness.ProjectRestoreInfoSourcesCount.Should().Be(-1);
            readiness.ProjectReadyTimings.Should().BeEmpty();
            isAllProjectsNominatedCallCount.Should().Be(2);
        }

        [Fact]
        public async Task WaitForOnBuildRestoreReadinessCoreAsync_WhenNominationsDoNotComplete_TimesOut()
        {
            var utcNow = new DateTimeOffset(year: 2026, month: 9, day: 11, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero);
            int getUtcNowCallCount = 0;

            SolutionRestoreWorker.RestoreReadinessResult readiness = await SolutionRestoreWorker.WaitForOnBuildRestoreReadinessAsync(
                waitForSolutionLoadedAsync: _ => Task.CompletedTask,
                isAllProjectsNominatedAsync: () => Task.FromResult(false),
                getUtcNow: () => utcNow + TimeSpan.FromMinutes(6 * getUtcNowCallCount++),
                token: CancellationToken.None);

            readiness.RestoreReason.Should().Be(ImplicitRestoreReason.NominationsIdleTimeout);
            readiness.BulkRestoreCoordinationCheckStartTime.Should().Be(utcNow);
            readiness.ProjectsReadyCheckCount.Should().Be(0);
            readiness.ProjectRestoreInfoSourcesCount.Should().Be(-1);
            readiness.ProjectReadyTimings.Should().BeEmpty();
        }
    }
}
