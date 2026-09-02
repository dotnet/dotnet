// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.JobMonitor;
using Microsoft.DotNet.Helix.JobMonitor.Models;
using Microsoft.DotNet.Helix.Sdk.Tests.Fakes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using static Microsoft.DotNet.Helix.Sdk.Tests.ScenarioHelpers.ScenarioHelpers;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    [Collection("NonParallel")]
    public class JobMonitorRunnerTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void MissingAttemptMetadata_DefaultsToFirstAttempt(string value)
        {
            MonitorState.ParseStageAttempt(value).Should().Be(1);
            MonitorState.ParseJobAttempt(value).Should().Be(1);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("0")]
        [InlineData("-1")]
        public void MalformedAttemptMetadata_IsRejected(string value)
        {
            Action parseStageAttempt = () => MonitorState.ParseStageAttempt(value);
            Action parseJobAttempt = () => MonitorState.ParseJobAttempt(value);

            parseStageAttempt.Should().Throw<InvalidOperationException>();
            parseJobAttempt.Should().Throw<InvalidOperationException>();
        }

        /// <summary>
        /// Single pipeline job goes from queued → in progress → completed (succeeded).
        /// No Helix jobs are ever submitted.
        /// The monitor should poll 3 times and exit with code 1.
        /// </summary>
        [Fact]
        public async Task SinglePipelineJobSucceeds_NoHelixJobs_ExitOne()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            // Poll 1: other job is queued
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "pending"));

            // Poll 2: other job is in progress
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "inProgress"));

            // Poll 3: other job completed successfully
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "completed", "succeeded"));

            // No Helix jobs on any poll
            helix.AddResponse(jobs: []);
            helix.AddResponse(jobs: []);
            helix.AddResponse(jobs: []);

            var runner = CreateRunner(azdo, helix, logger: logger);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            azdo.TimelineCallCount.Should().Be(3);
            azdo.CreatedTestRuns.Should().BeEmpty();
            azdo.UploadedJobNames.Should().BeEmpty();
            logger.Messages.Should().Contain(message =>
                message.Contains("No Helix jobs were submitted by this stage in any attempt."));
        }

        [Fact]
        public async Task SinglePipelineJobSucceeds_NoHelixJobsAllowed_ExitZero()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "completed", "succeeded"));
            helix.AddResponse(jobs: []);

            JobMonitorOptions options = DefaultOptions();
            options.AllowNoHelixJobs = true;
            var runner = new JobMonitorRunner(options, logger, azdo, helix, NoDelay);

            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            logger.Messages.Should().NotContain(message =>
                message.Contains("No Helix jobs were submitted by this stage in any attempt."));
        }

        /// <summary>
        /// Two pipeline jobs (plus the monitor). No Helix jobs submitted.
        /// Jobs finish at different times: one passes, one fails.
        /// The monitor should detect the failure and exit with code 1.
        /// </summary>
        [Fact]
        public async Task TwoPipelineJobs_OnePassesOneFails_NoHelixJobs_ExitOne()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            // Poll 1: both jobs queued
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "pending"),
                PipelineJob("Build Windows", "pending"));

            // Poll 2: Linux in progress, Windows still queued
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "inProgress"),
                PipelineJob("Build Windows", "pending"));

            // Poll 3: Linux completed (passed), Windows in progress
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "completed", "succeeded"),
                PipelineJob("Build Windows", "inProgress"));

            // Poll 4: Linux completed (passed), Windows completed (failed)
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "completed", "succeeded"),
                PipelineJob("Build Windows", "completed", "failed"));

            // No Helix jobs on any poll
            helix.AddResponse(jobs: []);
            helix.AddResponse(jobs: []);
            helix.AddResponse(jobs: []);
            helix.AddResponse(jobs: []);

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            azdo.TimelineCallCount.Should().Be(4);
            azdo.CreatedTestRuns.Should().BeEmpty();
            azdo.UploadedJobNames.Should().BeEmpty();
        }

        /// <summary>
        /// Two pipeline jobs (plus the monitor). No Helix jobs submitted.
        /// Both pipeline jobs fail.
        /// The monitor should exit with code 1.
        /// </summary>
        [Fact]
        public async Task TwoPipelineJobs_AllFailed_NoHelixJobs_ExitOne()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            // Poll 1: both in progress
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "inProgress"),
                PipelineJob("Build Windows", "inProgress"));

            // Poll 2: Linux failed, Windows still going
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "completed", "failed"),
                PipelineJob("Build Windows", "inProgress"));

            // Poll 3: both failed
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "completed", "failed"),
                PipelineJob("Build Windows", "completed", "failed"));

            helix.AddResponse(jobs: []);
            helix.AddResponse(jobs: []);
            helix.AddResponse(jobs: []);

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            azdo.TimelineCallCount.Should().Be(3);
            azdo.CreatedTestRuns.Should().BeEmpty();
            azdo.UploadedJobNames.Should().BeEmpty();
        }

        /// <summary>
        /// Two pipeline jobs (plus the monitor). No Helix jobs submitted.
        /// Both pipeline jobs pass.
        /// The monitor should exit with code 1.
        /// </summary>
        [Fact]
        public async Task TwoPipelineJobs_AllPassed_NoHelixJobs_ExitOne()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            // Poll 1: both in progress
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "inProgress"),
                PipelineJob("Build Windows", "inProgress"));

            // Poll 2: Linux done, Windows still going
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "completed", "succeeded"),
                PipelineJob("Build Windows", "inProgress"));

            // Poll 3: both done
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "completed", "succeeded"),
                PipelineJob("Build Windows", "completed", "succeeded"));

            helix.AddResponse(jobs: []);
            helix.AddResponse(jobs: []);
            helix.AddResponse(jobs: []);

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            azdo.TimelineCallCount.Should().Be(3);
            azdo.CreatedTestRuns.Should().BeEmpty();
            azdo.UploadedJobNames.Should().BeEmpty();
        }

        /// <summary>
        /// Three jobs (monitor + Build Linux + Build Windows). No Helix jobs.
        /// First run: Linux passes, Windows fails → monitor exits 1.
        /// Then a retry happens: the monitor and Build Windows are re-run (attempt 2).
        /// In AzDO, retried jobs appear in the timeline with attempt=2. The old
        /// attempt=1 records for the retried jobs are replaced. Non-retried jobs
        /// (Build Linux) keep their attempt=1 records.
        /// The retried Windows job queues, runs, then passes.
        /// The monitor should still exit 1 because no Helix jobs exist in either attempt.
        /// </summary>
        [Fact]
        public async Task RetryAfterFailure_RetriedJobPasses_NoHelixJobs_ExitOne()
        {
            // --- First run (attempt 1) ---
            var azdo1 = new FakeAzureDevOpsService();
            var helix1 = new FakeHelixService();

            // Poll 1: both jobs queued
            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "pending"),
                PipelineJob("Build Windows", "pending"));

            // Poll 2: both in progress
            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "inProgress"),
                PipelineJob("Build Windows", "inProgress"));

            // Poll 3: Linux passed, Windows still going
            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "completed", "succeeded"),
                PipelineJob("Build Windows", "inProgress"));

            // Poll 4: Linux passed, Windows failed
            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "completed", "succeeded"),
                PipelineJob("Build Windows", "completed", "failed"));

            helix1.AddResponse(jobs: []);
            helix1.AddResponse(jobs: []);
            helix1.AddResponse(jobs: []);
            helix1.AddResponse(jobs: []);

            var runner1 = CreateRunner(azdo1, helix1);
            int exitCode1 = await runner1.RunAsync(CancellationToken.None);

            exitCode1.Should().Be(1);
            azdo1.TimelineCallCount.Should().Be(4);

            // --- Retry (attempt 2): monitor and Build Windows re-run ---
            // AzDO replaces the retried jobs' records with attempt=2.
            // Build Linux was not retried, so it keeps attempt=1 and its completed state.
            var azdo2 = new FakeAzureDevOpsService();
            var helix2 = new FakeHelixService();

            // Poll 1: Linux still passed (attempt 1), Windows queued (attempt 2)
            azdo2.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Build Linux", "completed", "succeeded"),
                PipelineJob("Build Windows", "pending", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]));

            // Poll 2: Windows in progress
            azdo2.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Build Linux", "completed", "succeeded"),
                PipelineJob("Build Windows", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]));

            // Poll 3: Windows completed (passed this time)
            azdo2.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Build Linux", "completed", "succeeded"),
                PipelineJob("Build Windows", "completed", "succeeded", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]));

            helix2.AddResponse(jobs: []);
            helix2.AddResponse(jobs: []);
            helix2.AddResponse(jobs: []);

            var runner2 = CreateRunner(azdo2, helix2);
            int exitCode2 = await runner2.RunAsync(CancellationToken.None);

            exitCode2.Should().Be(1);
            azdo2.TimelineCallCount.Should().Be(3);
            azdo2.CreatedTestRuns.Should().BeEmpty();
            azdo2.UploadedJobNames.Should().BeEmpty();
        }

        /// <summary>
        /// Three jobs (monitor + Build Linux + Build Windows). Build Linux submits Helix work
        /// that passes; Build Windows submits nothing and fails → attempt 1 exits 1.
        /// Then a retry happens (attempt 2): only the monitor and Build Windows re-run, and the
        /// retried Build Windows again submits no Helix jobs before passing. Build Linux was not
        /// retried, so its passed attempt-1 Helix job is still discoverable for the build.
        /// The no-Helix-jobs gate is cross-attempt aware: because Helix jobs WERE submitted by
        /// this stage in a previous attempt, the monitor must NOT fail with "no Helix jobs" on the
        /// retry — it should exit 0.
        /// </summary>
        [Fact]
        public async Task RetryAfterFailure_RetriedJobSubmitsNothing_PreviousAttemptHadHelixJobs_ExitZero()
        {
            // --- First run (attempt 1) ---
            // Build Linux submits a Helix job (passes); Build Windows submits nothing and fails.
            var azdo1 = new FakeAzureDevOpsService();
            var helix1 = new FakeHelixService();

            // Poll 1: both jobs in progress; Build Linux's Helix job is running.
            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "inProgress"),
                PipelineJob("Build Windows", "inProgress"));

            // Poll 2: Build Linux passed, Build Windows failed (it submitted no Helix work).
            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Build Linux", "completed", "succeeded"),
                PipelineJob("Build Windows", "completed", "failed"));

            // Retry pass + poll 1 see the Helix job running (attempt 1).
            helix1.AddResponse(
                jobs: [HelixJob("helix-linux", "running", stageName: "Test",
                    submitterJobName: "Build Linux", stageAttempt: "1")]);

            // Poll 2 sees the Helix job finished — its work item passed.
            helix1.AddResponse(
                jobs: [HelixJob("helix-linux", "finished", stageName: "Test",
                    submitterJobName: "Build Linux", stageAttempt: "1")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"]),
                });

            JobMonitorOptions options1 = DefaultOptions();
            options1.StageAttempt = "1";
            var runner1 = new JobMonitorRunner(options1, NullLogger.Instance, azdo1, helix1, NoDelay);

            int exitCode1 = await runner1.RunAsync(CancellationToken.None);

            // Attempt 1 fails because Build Windows failed (a non-monitor pipeline failure), even
            // though Helix jobs were submitted and passed.
            exitCode1.Should().Be(1);

            // --- Retry (attempt 2): monitor and Build Windows re-run ---
            // Build Windows again submits no Helix jobs. Build Linux was not retried, so its
            // passed attempt-1 Helix job is still returned for the build.
            var azdo2 = new FakeAzureDevOpsService();
            var helix2 = new FakeHelixService();
            var logger = new RecordingLogger();

            // Poll 1: Linux still passed (attempt 1), Windows in progress (attempt 2).
            azdo2.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Build Linux", "completed", "succeeded"),
                PipelineJob("Build Windows", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]));

            // Poll 2: Windows completed (passed this time), still no Helix work submitted.
            azdo2.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Build Linux", "completed", "succeeded"),
                PipelineJob("Build Windows", "completed", "succeeded", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]));

            // The only Helix job for the build is Build Linux's passed attempt-1 job.
            helix2.AddResponse(
                jobs: [HelixJob("helix-linux", "finished", stageName: "Test",
                    submitterJobName: "Build Linux", stageAttempt: "1")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"]),
                });

            JobMonitorOptions options2 = DefaultOptions();
            options2.StageAttempt = "2";
            var runner2 = new JobMonitorRunner(options2, logger, azdo2, helix2, NoDelay);

            int exitCode2 = await runner2.RunAsync(CancellationToken.None);

            // The retry must not exit 1 for "no Helix jobs": a previous attempt submitted Helix
            // work, so the stage did produce jobs.
            exitCode2.Should().Be(0);
            logger.Messages.Should().NotContain(message =>
                message.Contains("No Helix jobs were submitted by this stage in any attempt."));
        }

        /// <summary>
        /// Two jobs: monitor + a build job that submits Helix work.
        /// The build job queues, runs for a couple iterations, then completes.
        /// A single Helix job appears (running), then its work items finish (passed).
        /// The monitor should upload test results and exit 0.
        /// </summary>
        [Fact]
        public async Task BuildJobSubmitsHelixWork_WorkItemsPassed_ResultsUploaded_ExitZero()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            // --- AzDO timeline ---
            // Poll 1: build job queued
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "pending"));

            // Poll 2: build job in progress
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "inProgress"));

            // Poll 3: build job completed (it has submitted Helix work and exited)
            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded"));

            // Polls 4-6: build job still completed (monitor waiting for Helix)
            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded"));
            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded"));
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));

            // --- Helix ---
            // Poll 1: no Helix jobs yet (build hasn't submitted)
            helix.AddResponse(jobs: []);

            // Poll 2: no Helix jobs yet (build still running)
            helix.AddResponse(jobs: []);

            // Poll 3: Helix job appears, still running (submitted by the build job)
            helix.AddResponse(jobs: [HelixJob("helix-linux-tests", "running")]);

            // Poll 4: Helix job still running (work items executing)
            helix.AddResponse(jobs: [HelixJob("helix-linux-tests", "running")]);

            // Poll 5: Helix job still running (work items still going)
            helix.AddResponse(jobs: [HelixJob("helix-linux-tests", "running")]);

            // Poll 6: Helix job finished — work items passed
            helix.AddResponse(
                jobs: [HelixJob("helix-linux-tests", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux-tests"] = PassFail(passed: ["workitem-1"]),
                });

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            // Monitor should exit successfully
            exitCode.Should().Be(0);

            // 6 poll iterations (5 delays before exit on 6th)
            azdo.TimelineCallCount.Should().Be(6);

            // One test run created and completed for the Helix job
            azdo.CreatedTestRuns.Should().ContainSingle();
            azdo.CompletedTestRunIds.Should().ContainSingle();

            // Test results uploaded for the Helix job
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux-tests"]);
        }

        [Fact]
        public async Task CompletedHelixJob_QueuesTestResultUploadWithoutBlockingNextPoll()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();
            var uploadRelease = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            bool delayedBeforeUploadCompleted = false;

            azdo.UploadBlocker = uploadRelease.Task;
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "inProgress"));
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished", stageName: "Test",
                    submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"]),
                },
                testResultsByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] =
                    [
                        new WorkItemTestResults("helix-linux", "workitem-1", ["a.trx", "b.trx"])
                    ],
                });

            var runner = new JobMonitorRunner(DefaultOptions(), logger, azdo, helix,
                async (_, _) =>
                {
                    Task completed = await Task.WhenAny(azdo.UploadStarted.Task, Task.Delay(TimeSpan.FromSeconds(5)));
                    completed.Should().BeSameAs(azdo.UploadStarted.Task);
                    delayedBeforeUploadCompleted = !azdo.UploadCompleted.Task.IsCompleted;
                    uploadRelease.SetResult();
                });

            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            delayedBeforeUploadCompleted.Should().BeTrue();
            azdo.TimelineCallCount.Should().Be(2);
            // Initial monitor attempts do not run retry reconciliation; the completed job is
            // listed once by the poll loop.
            helix.GetListWorkItemsCallCount("helix-linux").Should().Be(1);
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux"]);
            azdo.CompletedTestRunIds.Should().ContainSingle();
            logger.Messages.Should().Contain(message =>
                message.Contains(
                    "Test result processing completed for job 'Test Linux (helix-linux)': "
                    + "1 work item(s), 2 recognized result file(s), and 2 test result(s) uploaded.",
                    StringComparison.Ordinal));
        }

        [Fact]
        public async Task DrainReportsAggregatePipelineProgress()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();
            var uploadRelease = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            azdo.UploadBlocker = uploadRelease.Task;
            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1));
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished", stageName: "Test",
                    submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"]),
                },
                testResultsByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] =
                    [
                        new WorkItemTestResults("helix-linux", "workitem-1", ["a.trx"])
                    ],
                });

            JobMonitorOptions options = DefaultOptions();
            options.Verbose = true;
            options.PollingIntervalSeconds = 1;
            var runner = new JobMonitorRunner(options, logger, azdo, helix, NoDelay);

            Task<int> run = runner.RunAsync(CancellationToken.None);
            await azdo.UploadStarted.Task.WaitAsync(TimeSpan.FromSeconds(5));
            await Task.Delay(TimeSpan.FromMilliseconds(1200));
            uploadRelease.SetResult();

            int exitCode = await run.WaitAsync(TimeSpan.FromSeconds(5));

            exitCode.Should().Be(0);
            logger.Messages.Should().Contain(message =>
                message.Contains(
                    "Starting final test result drain: 1 work item(s) were first observed terminal "
                    + "and 1 work item upload(s) became eligible in the final poll; "
                    + "1 work item upload(s) remain (1 from the final poll, 0 from earlier polls)",
                    StringComparison.Ordinal));
            logger.Messages.Should().Contain(message =>
                message.Contains("Test result pipeline drained in", StringComparison.Ordinal)
                && message.Contains("1 job(s), 1 work item(s), and 1 result(s)", StringComparison.Ordinal));
            logger.Messages.Should().Contain(message =>
                message.Contains("Performance metrics", StringComparison.Ordinal)
                && message.Contains("Azure DevOps HTTP:", StringComparison.Ordinal)
                && message.Contains("Pipeline aggregate worker time", StringComparison.Ordinal));
        }

        [Fact]
        public async Task DrainSeparatesFinalPollArrivalsFromEarlierBacklog()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();
            var uploadRelease = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            azdo.UploadBlocker = uploadRelease.Task;
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "inProgress"));
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-earlier", "finished"),
                    HelixJob("helix-final", "running"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-earlier"] = PassFail(passed: ["earlier-workitem"]),
                },
                testResultsByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-earlier"] =
                    [
                        new WorkItemTestResults("helix-earlier", "earlier-workitem", ["earlier.trx"])
                    ],
                });
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-earlier", "finished"),
                    HelixJob("helix-final", "finished"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-earlier"] = PassFail(passed: ["earlier-workitem"]),
                    ["helix-final"] = PassFail(passed: ["final-workitem"]),
                },
                testResultsByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-earlier"] =
                    [
                        new WorkItemTestResults("helix-earlier", "earlier-workitem", ["earlier.trx"])
                    ],
                    ["helix-final"] =
                    [
                        new WorkItemTestResults("helix-final", "final-workitem", ["final.trx"])
                    ],
                });

            var runner = new JobMonitorRunner(
                DefaultOptions(),
                logger,
                azdo,
                helix,
                async (_, _) => await azdo.UploadStarted.Task.WaitAsync(TimeSpan.FromSeconds(5)));

            Task<int> run = runner.RunAsync(CancellationToken.None);
            await WaitForAsync(() => logger.Messages.Any(message =>
                message.Contains("Starting final test result drain:", StringComparison.Ordinal)));

            logger.Messages.Should().Contain(message =>
                message.Contains(
                    "1 work item(s) were first observed terminal and 1 work item upload(s) became eligible "
                    + "in the final poll; 2 work item upload(s) remain "
                    + "(1 from the final poll, 1 from earlier polls)",
                    StringComparison.Ordinal));

            uploadRelease.SetResult();
            (await run.WaitAsync(TimeSpan.FromSeconds(5))).Should().Be(0);
        }

        [Fact]
        public async Task StatusReportsEveryFiveMinutesWhileDrainIsBlocked()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();
            var uploadRelease = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var statusTick = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            int statusDelayCount = 0;

            azdo.UploadBlocker = uploadRelease.Task;
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"]),
                });

            var runner = new JobMonitorRunner(
                DefaultOptions(),
                logger,
                azdo,
                helix,
                NoDelay,
                async (delay, cancellationToken) =>
                {
                    delay.Should().Be(TimeSpan.FromMinutes(5));
                    if (Interlocked.Increment(ref statusDelayCount) == 1)
                    {
                        await statusTick.Task.WaitAsync(cancellationToken);
                    }
                    else
                    {
                        await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
                    }
                });

            Task<int> run = runner.RunAsync(CancellationToken.None);
            await WaitForAsync(() => logger.Messages.Any(message =>
                message.Contains("Starting final test result drain:", StringComparison.Ordinal)));
            logger.Messages.Count(IsStatusMessage).Should().Be(1);

            statusTick.SetResult();
            await WaitForAsync(() => logger.Messages.Count(IsStatusMessage) == 2);
            logger.Messages.Last(IsStatusMessage).Should().Contain("1 completed");

            uploadRelease.SetResult();
            (await run.WaitAsync(TimeSpan.FromSeconds(5))).Should().Be(0);

            static bool IsStatusMessage(string message)
                => message.Contains("ℹ️ Status:", StringComparison.Ordinal);
        }

        [Fact]
        public async Task UploadPipeline_BoundsWorkItemParallelismAndCreatesOneRunPerJob()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var uploadRelease = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            string[] workItems = [.. Enumerable.Range(1, 20).Select(index => $"workitem-{index}")];

            azdo.UploadBlocker = uploadRelease.Task;
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: workItems),
                },
                testResultsByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] =
                    [
                        ..workItems.Select(name =>
                            new WorkItemTestResults("helix-linux", name, [$"{name}.trx"]))
                    ],
                });

            JobMonitorOptions options = DefaultOptions();
            options.TestResultUploadParallelism = 4;
            var runner = new JobMonitorRunner(options, new RecordingLogger(), azdo, helix, NoDelay);

            Task<int> run = runner.RunAsync(CancellationToken.None);
            await WaitForAsync(() => azdo.MaximumConcurrentUploads == 4);
            uploadRelease.SetResult();

            (await run.WaitAsync(TimeSpan.FromSeconds(5))).Should().Be(0);
            azdo.MaximumConcurrentUploads.Should().Be(4);
            azdo.CreateTestRunCallCount.Should().Be(1);
            azdo.CompleteTestRunCallCount.Should().Be(1);
            azdo.PublishTestResultsCallCount.Should().Be(20);
        }

        [Fact]
        public async Task UploadPipeline_UploadsTerminalWorkItemsBeforeJobCompletesAndPreservesTestFailures()
        {
            var azdo = new FakeAzureDevOpsService()
                .WithFailedTestResults("helix-linux", "workitem-1");
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "inProgress"));
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "running", initialWorkItemCount: 2)],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"]),
                });
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished", initialWorkItemCount: 2)],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1", "workitem-2"]),
                });

            int delayCount = 0;
            var runner = new JobMonitorRunner(
                DefaultOptions(),
                NullLogger.Instance,
                azdo,
                helix,
                async (_, _) =>
                {
                    delayCount++;
                    await azdo.UploadCompleted.Task.WaitAsync(TimeSpan.FromSeconds(5));
                    azdo.PublishTestResultsCallCount.Should().Be(1);
                    azdo.CompleteTestRunCallCount.Should().Be(0);
                });

            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            delayCount.Should().Be(1);
            azdo.CreateTestRunCallCount.Should().Be(1);
            azdo.PublishTestResultsCallCount.Should().Be(2);
            azdo.CompleteTestRunCallCount.Should().Be(1);
        }

        [Fact]
        public void IncrementalTestFailure_DoesNotStickAcrossPassingIncarnation()
        {
            var state = new MonitorState();
            HelixJobInfo original = HelixJob(
                "helix-original",
                "finished",
                submitterPhaseName: "Test_Linux",
                queueId: "q",
                logicalJobName: "tests");
            HelixJobInfo retry = HelixJob(
                "helix-retry",
                "finished",
                submitterPhaseName: "Test_Linux",
                queueId: "q",
                previousHelixJobName: "helix-original",
                logicalJobName: "tests");
            state.ObserveJobs([original, retry]);

            state.ObserveTestResult(
                original.JobName,
                "workitem",
                allPassed: false);
            state.TryRecordWorkItemOutcomes(
                original,
                [new WorkItemSummary("original/workitem", original.JobName, "workitem", "Finished") { ExitCode = 1 }]);
            state.TryRecordWorkItemOutcomes(
                retry,
                [new WorkItemSummary("retry/workitem", retry.JobName, "workitem", "Finished") { ExitCode = 0 }]);

            state.HasFailedWorkItem.Should().BeFalse();
            state.SnapshotFailedWorkItemConsoleInfo().Should().BeEmpty();
        }

        [Fact]
        public async Task UploadPipeline_DoesNotDropCompletedJobsWhenBacklogged()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            HelixJobInfo[] jobs =
            [
                ..Enumerable.Range(1, 300)
                    .Select(index => HelixJob($"helix-{index}", "finished"))
            ];
            var passFailByJob = jobs.ToDictionary(
                static job => job.JobName,
                static _ => PassFail(),
                StringComparer.OrdinalIgnoreCase);

            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));
            helix.AddResponse(jobs, passFailByJob);

            var runner = new JobMonitorRunner(
                DefaultOptions(),
                new RecordingLogger(),
                azdo,
                helix,
                NoDelay);

            (await runner.RunAsync(CancellationToken.None).WaitAsync(TimeSpan.FromSeconds(10))).Should().Be(0);
            azdo.CreateTestRunCallCount.Should().Be(300);
            azdo.CompleteTestRunCallCount.Should().Be(300);
        }

        [Fact]
        public async Task FailedTestResult_TransientUploadFailure_PreservesObservedOutcome()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();
            int delayCount = 0;

            azdo.FailNextUpload();
            azdo.WithFailedTestResults("helix-linux", "workitem-1");

            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"]),
                },
                testResultsByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] =
                    [
                        new WorkItemTestResults("helix-linux", "workitem-1", ["results.trx"])
                    ],
                });

            var runner = new JobMonitorRunner(DefaultOptions(), logger, azdo, helix,
                (_, _) =>
                {
                    delayCount++;
                    return Task.CompletedTask;
                });
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            azdo.PublishTestResultsCallCount.Should().Be(1);
            delayCount.Should().Be(0);
            azdo.CreatedTestRuns.Should().ContainSingle();
            azdo.CompletedTestRunIds.Should().BeEmpty();
            azdo.UploadedJobNames.Should().BeEmpty();
            logger.Messages.Should().Contain(message =>
                message.Contains("remains untagged", StringComparison.Ordinal)
                && message.Contains("later monitor invocation can replay it", StringComparison.Ordinal));
        }

        [Fact]
        public async Task PassedHelixWork_CreateTestRunFailure_PreservesTestOnlyFailures()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            azdo.FailNextCreate();
            azdo.WithFailedTestResults("helix-linux", "workitem-2");
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1", "workitem-2"]),
                });

            var runner = CreateRunner(azdo, helix, logger: logger);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            azdo.CreateTestRunCallCount.Should().Be(1);
            helix.DownloadTestResultsCallCount.Should().Be(2);
            azdo.PublishTestResultsCallCount.Should().Be(0);
            azdo.CompleteTestRunCallCount.Should().Be(0);
            azdo.CompletedTestRunIds.Should().BeEmpty();
            logger.Messages.Should().ContainSingle(message =>
                message.Contains("remains untagged", StringComparison.Ordinal)
                && message.Contains("observed", StringComparison.Ordinal)
                && message.Contains("later monitor invocation can replay it", StringComparison.Ordinal));
        }

        [Fact]
        public async Task PassedHelixWork_TransientCompletionFailure_DoesNotReplayCompletionSequence()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();
            int delayCount = 0;

            azdo.FailNextComplete();
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"]),
                });

            var runner = new JobMonitorRunner(DefaultOptions(), logger, azdo, helix,
                (_, _) =>
                {
                    delayCount++;
                    return Task.CompletedTask;
                });

            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            azdo.CreateTestRunCallCount.Should().Be(1);
            azdo.PublishTestResultsCallCount.Should().Be(1);
            azdo.CompleteTestRunCallCount.Should().Be(1);
            delayCount.Should().Be(0);
            azdo.CompletedTestRunIds.Should().BeEmpty();
            logger.Messages.Should().Contain(message =>
                message.Contains("remains untagged", StringComparison.Ordinal)
                && message.Contains("later monitor invocation can replay it", StringComparison.Ordinal));
        }

        [Fact]
        public async Task PassedHelixWork_PermanentDownloadFailure_DoesNotCreateTestRun()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            helix.FailDownloadForJob("helix-linux");
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"]),
                });

            var runner = CreateRunner(azdo, helix, logger: logger);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            azdo.CreateTestRunCallCount.Should().Be(0);
            helix.DownloadTestResultsCallCount.Should().Be(1);
            azdo.PublishTestResultsCallCount.Should().Be(0);
            azdo.CompleteTestRunCallCount.Should().Be(0);
            logger.Messages.Should().Contain(message =>
                message.Contains("remains untagged", StringComparison.Ordinal)
                && message.Contains("later monitor invocation can replay it", StringComparison.Ordinal));
        }

        [Fact]
        public async Task MultipleWorkItems_OneUploadFails_JobRemainsUntagged()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            azdo.FailNextUpload(new InvalidOperationException("Injected permanent upload failure."));
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1", "workitem-2"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.TestResultUploadParallelism = 1;
            var runner = new JobMonitorRunner(options, logger, azdo, helix, NoDelay);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            azdo.CreateTestRunCallCount.Should().Be(1);
            azdo.PublishTestResultsCallCount.Should().Be(2);
            azdo.CompleteTestRunCallCount.Should().Be(0);
            azdo.CompletedTestRunIds.Should().BeEmpty();
            logger.Messages.Should().Contain(message =>
                message.Contains("remains untagged", StringComparison.Ordinal)
                && message.Contains("later monitor invocation can replay it", StringComparison.Ordinal));
        }

        [Fact]
        public async Task PassedHelixWork_PermanentUploadFailure_DoesNotHangOrFailBuild()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            azdo.FailNextUpload(new InvalidOperationException("Injected permanent upload failure."));
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"]),
                });

            var runner = CreateRunner(azdo, helix, logger: logger);
            int exitCode = await runner.RunAsync(CancellationToken.None).WaitAsync(TimeSpan.FromSeconds(10));

            exitCode.Should().Be(0);
            azdo.CreateTestRunCallCount.Should().Be(1);
            azdo.PublishTestResultsCallCount.Should().Be(1);
            azdo.CompleteTestRunCallCount.Should().Be(0);
            azdo.CompletedTestRunIds.Should().BeEmpty();
            logger.Messages.Should().Contain(message =>
                message.Contains("remains untagged", StringComparison.Ordinal)
                && message.Contains("later monitor invocation can replay it", StringComparison.Ordinal));
        }

        [Fact]
        public async Task PassedHelixWork_TransientDownloadFailure_RetriesBeforeCreatingTestRun()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));

            helix.FailNextDownloadForJob("helix-linux");
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"]),
                });

            var runner = CreateRunner(azdo, helix);

            int exitCode = await runner.RunAsync(CancellationToken.None).WaitAsync(TimeSpan.FromSeconds(10));

            exitCode.Should().Be(0);
            azdo.CreateTestRunCallCount.Should().Be(1);
            azdo.PublishTestResultsCallCount.Should().Be(1);
            azdo.CompleteTestRunCallCount.Should().Be(1);
            azdo.CompletedTestRunIds.Should().ContainSingle();
        }

        /// <summary>
        /// A single build job submits a Helix job with multiple work items.
        /// The work items finish at different times (the Helix job goes from running → finished
        /// only when the last work item completes). All work items pass.
        /// </summary>
        [Fact]
        public async Task MultipleWorkItems_FinishAtDifferentTimes_AllPass_ExitZero()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            // Build job runs then completes quickly
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "inProgress"));
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));
            // Remaining polls: build done, waiting for Helix
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            // Poll 1: no Helix jobs yet
            helix.AddResponse(jobs: []);
            // Poll 2: Helix job appears, running (work items not all done)
            helix.AddResponse(jobs: [HelixJob("helix-linux", "running")]);
            // Poll 3: still running (some work items done, some not)
            helix.AddResponse(jobs: [HelixJob("helix-linux", "running")]);
            // Poll 4: finished — all 3 work items passed
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-fast", "wi-medium", "wi-slow"]),
                });

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            azdo.TimelineCallCount.Should().Be(4);
            azdo.CreatedTestRuns.Should().ContainSingle();
            azdo.CompletedTestRunIds.Should().ContainSingle();
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux"]);
        }

        /// <summary>
        /// Two build jobs each submit Helix work. Work items finish at different times.
        /// Some work items pass, some fail.
        /// The monitor should upload all results and exit 1 (due to failures).
        /// </summary>
        [Fact]
        public async Task TwoJobsSubmitHelixWork_MixedPassFail_ExitOne()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            // Both build jobs run and complete
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "inProgress"),
                PipelineJob("Test Windows", "inProgress"));
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "inProgress"));
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "completed", "succeeded"));
            // Remaining polls while Helix runs
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "completed", "succeeded"));
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "completed", "succeeded"));

            // Poll 1: no Helix jobs
            helix.AddResponse(jobs: []);
            // Poll 2: Linux helix job appears (running)
            helix.AddResponse(jobs: [HelixJob("helix-linux", "running")]);
            // Poll 3: Linux finished (has failures), Windows helix job appears (running)
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished"), HelixJob("helix-windows", "running")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["linux-wi-1"], failed: ["linux-wi-2"]),
                });
            // Poll 4: Windows still running
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished"), HelixJob("helix-windows", "running")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["linux-wi-1"], failed: ["linux-wi-2"]),
                });
            // Poll 5: Windows finished (all passed)
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished"), HelixJob("helix-windows", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["linux-wi-1"], failed: ["linux-wi-2"]),
                    ["helix-windows"] = PassFail(passed: ["win-wi-1", "win-wi-2"]),
                });

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            // Exit 1 because helix-linux had a failed work item
            exitCode.Should().Be(1);
            azdo.TimelineCallCount.Should().Be(5);
            // Both Helix jobs had results uploaded
            azdo.CreatedTestRuns.Should().HaveCount(2);
            azdo.CompletedTestRunIds.Should().HaveCount(2);
            azdo.UploadedJobNames.Should().Contain("helix-linux");
            azdo.UploadedJobNames.Should().Contain("helix-windows");
        }

        /// <summary>
        /// Regression: two separate AzDO submitter jobs each submit a Helix job that contains
        /// a work item with the same name (e.g. <c>Microsoft.DotNet.Helix.Sdk.Tests.dll</c>).
        /// One job's work item fails; the other passes. The failure must NOT be overwritten
        /// by the successful job processed afterwards. The monitor must exit non-zero.
        /// </summary>
        [Fact]
        public async Task TwoAzDOJobs_SameWorkItemName_OneFails_OtherPasses_FailurePropagates()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Linux_Build_Debug", "completed", "succeeded"),
                PipelineJob("Windows_Build_Release", "completed", "succeeded"));

            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished", submitterJobName: "Linux_Build_Debug"),
                    HelixJob("helix-windows", "finished", submitterJobName: "Windows_Build_Release"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["Microsoft.DotNet.Helix.Sdk.Tests.dll"]),
                    ["helix-windows"] = PassFail(passed: ["Microsoft.DotNet.Helix.Sdk.Tests.dll"]),
                });

            var runner = CreateRunner(azdo, helix, logger: logger);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            // The Linux work-item failure must not be overwritten by the same-named work item
            // that passed on Windows. Final summary must report 1 failed work item and the
            // monitor must exit non-zero.
            exitCode.Should().Be(1);
        }

        /// <summary>
        /// Regression: a single AzDO matrix leg (one <c>System.JobName</c>) fans out to
        /// multiple Helix queues — each queue produces its own Helix job sharing the same
        /// <c>SubmitterJobName</c>. A work item with the same name (e.g.
        /// <c>Microsoft.DotNet.Helix.Sdk.Tests.dll</c>) runs in every queue; it fails on one
        /// queue and passes on another. The earlier queue's failure must NOT be overwritten
        /// by the later queue's pass on the same submitter, and the monitor must exit
        /// non-zero. This mirrors the production scenario where eight Helix jobs (two
        /// configurations × three queues + extras) reported "0 failed" in the final summary
        /// even though one work item failed.
        /// </summary>
        [Fact]
        public async Task OneSubmitter_FansOutToMultipleQueues_SameWorkItemName_FailureNotOverwrittenByPass()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Linux_Build_Debug", "completed", "succeeded"));

            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-ubuntu", "finished", submitterJobName: "Linux_Build_Debug", queueId: "ubuntu.2204.amd64.open"),
                    HelixJob("helix-osx", "finished", submitterJobName: "Linux_Build_Debug", queueId: "osx.15.amd64.open"),
                    HelixJob("helix-windows", "finished", submitterJobName: "Linux_Build_Debug", queueId: "windows.11.amd64.client.open"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-ubuntu"] = PassFail(failed: ["Microsoft.DotNet.Helix.Sdk.Tests.dll"]),
                    ["helix-osx"] = PassFail(passed: ["Microsoft.DotNet.Helix.Sdk.Tests.dll"]),
                    ["helix-windows"] = PassFail(passed: ["Microsoft.DotNet.Helix.Sdk.Tests.dll"]),
                });

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
        }

        /// <summary>
        /// Regression: a single AzDO job can submit multiple independent Helix jobs to the same
        /// queue. Their work-item names can overlap, so the submitter and queue alone are not a
        /// unique stream identifier. The submitter-assigned logical Helix job name must preserve
        /// one job's failure when a later sibling job reports the same work-item name as passed.
        /// </summary>
        [Fact]
        public async Task OneSubmitter_SameQueue_DifferentLogicalJobs_FailureNotOverwrittenByPass()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("__default", "completed", "succeeded"));

            helix.AddResponse(
                jobs:
                [
                    HelixJob(
                        "helix-wasm-tests",
                        "finished",
                        submitterJobName: "__default",
                        queueId: "azurelinux.3.amd64.open.rt",
                        logicalJobName: "WasmTestOnChrome-MONO-ST"),
                    HelixJob(
                        "helix-wasm-build-tests",
                        "finished",
                        submitterJobName: "__default",
                        queueId: "azurelinux.3.amd64.open.rt",
                        logicalJobName: "WasmBuildTests"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-wasm-tests"] = PassFail(failed: ["System.Formats.Tar.Manual.Tests"]),
                    ["helix-wasm-build-tests"] = PassFail(passed: ["System.Formats.Tar.Manual.Tests"]),
                });

            var runner = CreateRunner(azdo, helix, logger: logger);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            logger.Messages.Should().Contain(message =>
                message.Contains("Work items: 2 submitted / 0 resubmitted / 1 failed", StringComparison.Ordinal));
            logger.Messages.Should().Contain(message =>
                message.Contains("Failed work item information:", StringComparison.Ordinal)
                && message.Contains("System.Formats.Tar.Manual.Tests", StringComparison.Ordinal));
        }

        /// <summary>
        /// Production regression: runtime stamps <c>System.JobName=__default</c> onto many
        /// independent matrix jobs. Some of those jobs submit the same logical Helix scenario
        /// to the same queue, so only <c>System.PhaseName</c> distinguishes their streams.
        /// A pass from one phase must not erase a same-named failure from another phase.
        /// </summary>
        [Fact]
        public async Task DefaultJobName_SameQueueAndLogicalJob_DifferentPhases_FailureNotOverwrittenByPass()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("__default", "completed", "succeeded"));

            helix.AddResponse(
                jobs:
                [
                    HelixJob(
                        "helix-wasm-eat",
                        "finished",
                        submitterJobName: "__default",
                        submitterPhaseName: "build_browser_wasm_linux_Release_LibraryTests_EAT",
                        queueId: "azurelinux.3.amd64.open.rt",
                        logicalJobName: "WasmTestOnChrome-MONO-ST"),
                    HelixJob(
                        "helix-wasm-smoke",
                        "finished",
                        submitterJobName: "__default",
                        submitterPhaseName: "build_browser_wasm_linux_Release_LibraryTests_Smoke",
                        queueId: "azurelinux.3.amd64.open.rt",
                        logicalJobName: "WasmTestOnChrome-MONO-ST"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-wasm-eat"] = PassFail(failed: ["System.Formats.Tar.Manual.Tests"]),
                    ["helix-wasm-smoke"] = PassFail(passed: ["System.Formats.Tar.Manual.Tests"]),
                });

            var runner = CreateRunner(azdo, helix, logger: logger);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            logger.Messages.Should().Contain(message =>
                message.Contains("Work items: 2 submitted / 0 resubmitted / 1 failed", StringComparison.Ordinal));
            logger.Messages.Should().Contain(message =>
                message.Contains("Failed work item information:", StringComparison.Ordinal)
                && message.Contains("System.Formats.Tar.Manual.Tests", StringComparison.Ordinal));
        }

        [Fact]
        public async Task OneSubmitter_SendsMultipleJobsToSameQueue_FailureNotOverwrittenByPass()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Linux_Build_Debug", "completed", "succeeded"));

            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-failed", "finished", submitterJobName: "Linux_Build_Debug",
                        queueId: "ubuntu.2204.amd64.open", logicalJobName: "failed-job"),
                    HelixJob("helix-passed", "finished", submitterJobName: "Linux_Build_Debug",
                        queueId: "ubuntu.2204.amd64.open", logicalJobName: "passed-job"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-failed"] = PassFail(failed: ["Microsoft.DotNet.Helix.Sdk.Tests.dll"]),
                    ["helix-passed"] = PassFail(passed: ["Microsoft.DotNet.Helix.Sdk.Tests.dll"]),
                });

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);

            var azdoAttempt2 = new FakeAzureDevOpsService()
                .WithPreviouslyProcessedJob("helix-failed")
                .WithPreviouslyProcessedJob("helix-passed");
            var helixAttempt2 = new FakeHelixService();

            azdoAttempt2.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Linux_Build_Debug", "completed", "succeeded"));
            azdoAttempt2.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Linux_Build_Debug", "completed", "succeeded"));

            helixAttempt2.AddResponse(
                jobs:
                [
                    HelixJob("helix-failed", "finished", submitterJobName: "Linux_Build_Debug",
                        queueId: "ubuntu.2204.amd64.open", stageAttempt: "1",
                        jobAttempt: "1", logicalJobName: "failed-job"),
                    HelixJob("helix-passed", "finished", submitterJobName: "Linux_Build_Debug",
                        queueId: "ubuntu.2204.amd64.open", stageAttempt: "1",
                        jobAttempt: "1", logicalJobName: "passed-job"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-failed"] = PassFail(failed: ["Microsoft.DotNet.Helix.Sdk.Tests.dll"]),
                    ["helix-passed"] = PassFail(passed: ["Microsoft.DotNet.Helix.Sdk.Tests.dll"]),
                });
            helixAttempt2.ConfigureResubmission("helix-failed", "helix-failed-resub");
            helixAttempt2.AddResponse(
                jobs:
                [
                    HelixJob("helix-failed", "finished", submitterJobName: "Linux_Build_Debug",
                        queueId: "ubuntu.2204.amd64.open", stageAttempt: "1",
                        jobAttempt: "1", logicalJobName: "failed-job"),
                    HelixJob("helix-passed", "finished", submitterJobName: "Linux_Build_Debug",
                        queueId: "ubuntu.2204.amd64.open", stageAttempt: "1",
                        jobAttempt: "1", logicalJobName: "passed-job"),
                    HelixJob("helix-failed-resub", "finished", submitterJobName: "Linux_Build_Debug",
                        queueId: "ubuntu.2204.amd64.open", previousHelixJobName: "helix-failed",
                        stageAttempt: "2", jobAttempt: "1", logicalJobName: "failed-job"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-failed-resub"] = PassFail(passed: ["Microsoft.DotNet.Helix.Sdk.Tests.dll"]),
                });

            JobMonitorOptions attempt2Options = DefaultOptions();
            attempt2Options.StageAttempt = "2";
            attempt2Options.JobAttempt = "2";
            var attempt2Runner = new JobMonitorRunner(
                attempt2Options,
                NullLogger.Instance,
                azdoAttempt2,
                helixAttempt2,
                NoDelay);

            int attempt2ExitCode = await attempt2Runner.RunAsync(CancellationToken.None);

            attempt2ExitCode.Should().Be(0);
            helixAttempt2.Resubmissions.Should().ContainSingle();
            helixAttempt2.Resubmissions[0].OriginalJob.Should().Be("helix-failed");
        }

        [Fact]
        public async Task StageRerun_UploadsNewHelixWorkItemsWithoutReuploadingPreviousWorkItems()
        {
            var azdo = new FakeAzureDevOpsService();

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress"),
                MonitorJob(parentId: "stage-test"),
                PipelineJob("Linux Build_Debug", "completed", "succeeded", parentId: "stage-test"),
                PipelineJob("Windows_NT Build_Release", "completed", "succeeded", parentId: "stage-test"));

            var helix1 = new FakeHelixService();
            helix1.AddResponse(
                jobs:
                [
                    HelixJob("old-helix-linux", "finished", stageName: "Test"),
                    HelixJob("old-helix-windows", "finished", stageName: "Test"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["old-helix-linux"] = PassFail(passed: ["common-work-item"]),
                    ["old-helix-windows"] = PassFail(passed: ["common-work-item"]),
                });

            var runner1 = CreateRunner(azdo, helix1, stageName: "Test");
            int exitCode1 = await runner1.RunAsync(CancellationToken.None);

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress"),
                MonitorJob(parentId: "stage-test"),
                PipelineJob("Linux Build_Debug", "completed", "succeededWithIssues", attempt: 2, parentId: "stage-test"),
                PipelineJob("Windows_NT Build_Release", "completed", "succeededWithIssues", attempt: 2, parentId: "stage-test"));

            var helix2 = new FakeHelixService();
            helix2.AddResponse(
                jobs:
                [
                    HelixJob("old-helix-linux", "finished", stageName: "Test"),
                    HelixJob("old-helix-windows", "finished", stageName: "Test"),
                    HelixJob("new-helix-linux", "running", stageName: "Test", initialWorkItemCount: 2),
                    HelixJob("new-helix-windows", "running", stageName: "Test", initialWorkItemCount: 2),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["old-helix-linux"] = PassFail(passed: ["common-work-item"]),
                    ["old-helix-windows"] = PassFail(passed: ["common-work-item"]),
                    ["new-helix-linux"] = PassFail(passed: ["common-work-item", "linux-only-work-item"]),
                    ["new-helix-windows"] = PassFail(passed: ["common-work-item", "windows-only-work-item"]),
                });

            var runner2 = CreateRunner(azdo, helix2, stageName: "Test");
            int exitCode2 = await runner2.RunAsync(CancellationToken.None);

            exitCode1.Should().Be(0);
            exitCode2.Should().Be(0);
            azdo.UploadedJobNames.Should().BeEquivalentTo(
                ["old-helix-linux", "old-helix-windows", "new-helix-linux", "new-helix-windows"]);

            var uploadedWorkItems = azdo.UploadedResultsByRunId
                .Values
                .SelectMany(results => results)
                .Select(result => $"{result.JobName}/{result.WorkItemName}")
                .ToList();

            uploadedWorkItems.Should().OnlyHaveUniqueItems();
            uploadedWorkItems.Should().Contain("old-helix-linux/common-work-item");
            uploadedWorkItems.Should().Contain("old-helix-windows/common-work-item");
            uploadedWorkItems.Should().Contain("new-helix-linux/common-work-item");
            uploadedWorkItems.Should().Contain("new-helix-linux/linux-only-work-item");
            uploadedWorkItems.Should().Contain("new-helix-windows/common-work-item");
            uploadedWorkItems.Should().Contain("new-helix-windows/windows-only-work-item");
        }

        /// <summary>
        /// Stage-scoped monitoring. A job in another stage runs — the monitor should
        /// ignore it entirely. Only the job in the monitor's own stage matters.
        /// </summary>
        [Fact]
        public async Task StageScopedMonitor_IgnoresJobsOutsideStage_NoHelixJobs_ExitOne()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            // Timeline includes jobs from two stages: "Test" (monitor's stage) and "Build".
            // The Build stage job is still running, but the monitor shouldn't care.
            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress"),
                MonitorJob(parentId: "stage-test"),
                PipelineJob("Test Linux", "inProgress", parentId: "stage-test"),
                StageRecord("Build", "stage-build", "inProgress"),
                PipelineJob("Build Windows", "inProgress", parentId: "stage-build"));

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]),
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1, parentId: "stage-test"),
                StageRecord("Build", "stage-build", "inProgress"),
                PipelineJob("Build Windows", "inProgress", parentId: "stage-build"));

            // No Helix jobs
            helix.AddResponse(jobs: []);
            helix.AddResponse(jobs: []);

            var runner = CreateRunner(azdo, helix, stageName: "Test");
            int exitCode = await runner.RunAsync(CancellationToken.None);

            // Monitor only watches Test stage — Test Linux passed, but no Helix → exit 1
            // Build Windows being in progress doesn't block the monitor.
            exitCode.Should().Be(1);
            azdo.TimelineCallCount.Should().Be(2);
            azdo.UploadedJobNames.Should().BeEmpty();
        }

        /// <summary>
        /// Stage-scoped monitoring. A job outside the monitor's stage submits Helix work.
        /// The monitor should ignore that Helix job entirely.
        /// </summary>
        [Fact]
        public async Task StageScopedMonitor_IgnoresHelixJobsFromOtherStage_ExitOne()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            // Test stage job completes quickly with no Helix submissions.
            // Build stage job submits a Helix job, but that's not the monitor's concern.
            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]),
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1, parentId: "stage-test"),
                StageRecord("Build", "stage-build", "inProgress"),
                PipelineJob("Build Windows", "inProgress", parentId: "stage-build"));

            // Helix returns a job from the Build stage — monitor should ignore it
            helix.AddResponse(
                jobs: [HelixJob("helix-build-windows", "running", stageName: "Build")]);

            var runner = CreateRunner(azdo, helix, stageName: "Test");
            int exitCode = await runner.RunAsync(CancellationToken.None);

            // Test stage is done, no Helix jobs in Test stage → exit 1
            exitCode.Should().Be(1);
            azdo.TimelineCallCount.Should().Be(1);
            azdo.UploadedJobNames.Should().BeEmpty();
            azdo.CreatedTestRuns.Should().BeEmpty();
        }

        /// <summary>
        /// The first monitor invocation observes and reports an already-failed Helix job but does
        /// not create another Helix job. Work-item replay starts only when AzDO retries the monitor.
        /// </summary>
        [Fact]
        public async Task AttemptScoped_InitialMonitorInvocation_DoesNotResubmit()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress"),
                MonitorJob(parentId: "stage-test"),
                PipelineJob("Test_Linux", "completed", "succeeded", parentId: "stage-test"));
            helix.AddResponse(
                jobs: [HelixJob("helix-linux-a1", "finished", stageName: "Test",
                    submitterJobName: "Test_Linux", queueId: "q1", stageAttempt: "1",
                    jobAttempt: "1", logicalJobName: "tests")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux-a1"] = PassFail(failed: ["wi"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "1";
            options.JobAttempt = "1";

            int exitCode = await new JobMonitorRunner(
                options, NullLogger.Instance, azdo, helix, NoDelay).RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            helix.Resubmissions.Should().BeEmpty();
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux-a1"]);
        }

        /// <summary>
        /// Retry-failed-jobs advanced the stage and monitor to attempt 2, but the successful Helix
        /// submitters remain at job attempt 1. Their previous-stage Helix work is therefore still
        /// authoritative: passed work is retained and failed work is replayed.
        /// </summary>
        [Fact]
        public async Task AttemptScoped_RetryOnlyMonitor_ReconcilesPreviousAttemptWork()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]),
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"),
                PipelineJob("Test_Linux", "completed", "succeeded", attempt: 1, parentId: "stage-test"),
                PipelineJob("Test_Windows", "completed", "succeeded", attempt: 1, parentId: "stage-test"));

            HelixJobInfo linuxA1 = HelixJob("helix-linux-a1", "finished", stageName: "Test",
                submitterJobName: "Test_Linux", queueId: "q1", stageAttempt: "1",
                jobAttempt: "1", logicalJobName: "tests");
            HelixJobInfo winA1 = HelixJob("helix-win-a1", "finished", stageName: "Test",
                submitterJobName: "Test_Windows", queueId: "q2", stageAttempt: "1",
                jobAttempt: "1", logicalJobName: "tests");
            HelixJobInfo winResub = HelixJob("helix-win-a1-resub", "finished", stageName: "Test",
                submitterJobName: "Test_Windows", queueId: "q2", previousHelixJobName: "helix-win-a1",
                stageAttempt: "2", jobAttempt: "1", logicalJobName: "tests");

            // Snapshot 0 (retry pass + poll 1): previous-attempt jobs only.
            helix.AddResponse(
                jobs: [linuxA1, winA1],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux-a1"] = PassFail(passed: ["linux-wi"]),
                    ["helix-win-a1"] = PassFail(failed: ["win-wi"]),
                });
            // Snapshot 1 (poll 2+): the current-attempt resubmission has finished and passed.
            helix.AddResponse(
                jobs: [linuxA1, winA1, winResub],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux-a1"] = PassFail(passed: ["linux-wi"]),
                    ["helix-win-a1"] = PassFail(failed: ["win-wi"]),
                    ["helix-win-a1-resub"] = PassFail(passed: ["win-wi"]),
                });
            helix.ConfigureResubmission("helix-win-a1", "helix-win-a1-resub");

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix, NoDelay);

            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            // Previous attempt's failed work was resubmitted into the current attempt (attempt 2)...
            helix.Resubmissions.Should().ContainSingle();
            helix.Resubmissions[0].OriginalJob.Should().Be("helix-win-a1");
            helix.Resubmissions[0].FailedItems.Should().BeEquivalentTo(["win-wi"]);
            helix.Resubmissions[0].TargetStageAttempt.Should().Be("2");
            helix.Resubmissions[0].MonitorJobAttempt.Should().Be("2");
            helix.ResubmittedJobInfos.Should().ContainSingle()
                .Which.Should().Match<HelixJobInfo>(job =>
                    job.StageAttempt == "2" && job.JobAttempt == "1");
            // ...and the previous attempt's results (passed and failed) were still uploaded.
            azdo.UploadedJobNames.Should().Contain(["helix-linux-a1", "helix-win-a1", "helix-win-a1-resub"]);
        }

        /// <summary>
        /// Corner case 3 (the original issue #17156, generalized). Previous-attempt work that is
        /// permanently stuck in <c>Waiting</c> (never dispatched, e.g. its queue was purged) must
        /// be resubmitted into the current attempt and driven to completion — the monitor must not
        /// wait on the stranded incarnation. Termination here proves the stranded attempt-1 job
        /// (still <c>running</c>/<c>Waiting</c> in the final snapshot) does not gate the monitor.
        /// </summary>
        [Fact]
        public async Task AttemptScoped_StrandedWaitingPreviousWork_ResubmittedNotWaitedOn()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]),
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"),
                PipelineJob("Test_Linux", "completed", "succeeded", attempt: 1, parentId: "stage-test"));

            HelixJobInfo stranded = HelixJob("helix-stranded-a1", "running", stageName: "Test",
                submitterJobName: "Test_Linux", queueId: "q1", stageAttempt: "1", jobAttempt: "1");
            HelixJobInfo resub = HelixJob("helix-stranded-a1-r2", "finished", stageName: "Test",
                submitterJobName: "Test_Linux", queueId: "q1", previousHelixJobName: "helix-stranded-a1",
                stageAttempt: "2", jobAttempt: "1");

            // The stranded attempt-1 job's work items are stuck Waiting (never dispatched).
            helix.WithWorkItems("helix-stranded-a1",
            [
                new WorkItemSummary("helix-stranded-a1/Loader", "helix-stranded-a1", "Loader", "Waiting"),
                new WorkItemSummary("helix-stranded-a1/async", "helix-stranded-a1", "async", "Waiting"),
            ]);

            // Snapshot 0: only the stranded attempt-1 job (still running/Waiting).
            helix.AddResponse(jobs: [stranded]);
            // Snapshot 1: the stranded job is STILL running/Waiting, but the current-attempt
            // resubmission has finished and passed. The monitor must terminate on the resubmission.
            helix.AddResponse(
                jobs: [stranded, resub],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-stranded-a1-r2"] = PassFail(passed: ["Loader", "async"]),
                });
            helix.ConfigureResubmission("helix-stranded-a1", "helix-stranded-a1-r2");

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix, NoDelay);

            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            helix.Resubmissions.Should().ContainSingle();
            helix.Resubmissions[0].OriginalJob.Should().Be("helix-stranded-a1");
            helix.Resubmissions[0].FailedItems.Should().BeEquivalentTo(["Loader", "async"]);
            helix.Resubmissions[0].TargetStageAttempt.Should().Be("2");
        }

        /// <summary>
        /// A full-stage rerun has advanced the submitter to job attempt 2. The monitor starts
        /// before the attempt-2 Helix job is visible. It must use the timeline attempt to suppress
        /// replay of H1, wait for H2, and then gate on H2.
        /// </summary>
        [Fact]
        public async Task AttemptScoped_LinkedCurrentIncarnationExists_DoesNotResubmitPrevious()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]),
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"),
                PipelinePhase("Test_Linux", "Test Linux", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"),
                PipelineJob("__default", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)], parentId: "Test_Linux"));
            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]),
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"),
                PipelinePhase("Test_Linux", "Test Linux", "completed", "succeeded", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"),
                PipelineJob("__default", "completed", "succeeded", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)], parentId: "Test_Linux"));

            // Previous incarnation still running; fresh current incarnation of the SAME stream
            // (same submitter + queue), not linked by PreviousHelixJobName.
            HelixJobInfo previousRunning = HelixJob("helix-x-a1", "finished", stageName: "Test",
                submitterJobName: "__default", submitterPhaseName: "Test_Linux",
                queueId: "q1", stageAttempt: "1",
                jobAttempt: "1", logicalJobName: "tests");
            HelixJobInfo currentDone = HelixJob("helix-x-a2", "finished", stageName: "Test",
                submitterJobName: "__default", submitterPhaseName: "Test_Linux",
                queueId: "q1", stageAttempt: "2",
                jobAttempt: "2", logicalJobName: "tests");

            helix.AddResponse(
                jobs: [previousRunning],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-x-a1"] = PassFail(failed: ["wi"]),
                });
            helix.AddResponse(
                jobs: [previousRunning, currentDone],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-x-a1"] = PassFail(failed: ["wi"]),
                    ["helix-x-a2"] = PassFail(passed: ["wi"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";
            var runner = new JobMonitorRunner(options, logger, azdo, helix, NoDelay);

            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            helix.Resubmissions.Should().BeEmpty();
            azdo.UploadedJobNames.Should().Contain("helix-x-a2");
            logger.Messages.Should().NotContain(message =>
                message.Contains("helix-x-a1", StringComparison.Ordinal)
                && message.Contains("failed", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Corner case 5 (unlinked rerun duplicates). Two incarnations of the same work stream on
        /// different attempts are NOT connected by <c>PreviousHelixJobName</c> (a stage rerun, not
        /// a monitor resubmission). The higher stage/job incarnation must win the outcome map.
        /// Job names are chosen so a naive job-name sort would let the failed attempt-1 job win.
        /// </summary>
        [Fact]
        public async Task AttemptScoped_UnlinkedRerunDuplicates_HigherAttemptWinsOutcome()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]),
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"),
                PipelineJob("Test_Linux", "completed", "succeeded", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"));

            // Same stream (submitter + queue + logical job), not lineage-linked. "zzz-old"
            // (attempt 1, failed) sorts after "aaa-new" (attempt 2, passed): a job-name-ordered
            // reconciliation would let the stale failure overwrite the current pass.
            HelixJobInfo oldFailed = HelixJob("zzz-old", "finished", stageName: "Test",
                submitterJobName: "Test_Linux", queueId: "q1", stageAttempt: "1",
                jobAttempt: "1", logicalJobName: "tests");
            HelixJobInfo newPassed = HelixJob("aaa-new", "finished", stageName: "Test",
                submitterJobName: "Test_Linux", queueId: "q1", stageAttempt: "2",
                jobAttempt: "2", logicalJobName: "tests");

            helix.AddResponse(
                jobs: [oldFailed, newPassed],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["zzz-old"] = PassFail(failed: ["wi-1"]),
                    ["aaa-new"] = PassFail(passed: ["wi-1"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix, NoDelay);

            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            helix.Resubmissions.Should().BeEmpty();
        }

        /// <summary>
        /// Retry-failed-jobs can rerun only a subset of submitters. Retry eligibility is decided
        /// independently for each submitter: A's newer timeline attempt supersedes HA1, while B's
        /// unchanged attempt leaves HB1 eligible for monitor replay.
        /// </summary>
        [Fact]
        public async Task AttemptScoped_MixedSubmitterRetries_ReconcilesPerSubmitterAttempt()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]),
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"),
                PipelineJob("A", "completed", "succeeded", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"),
                PipelineJob("B", "completed", "succeeded", attempt: 1, parentId: "stage-test"));

            HelixJobInfo ha1 = HelixJob("ha1", "finished", stageName: "Test",
                submitterJobName: "A", queueId: "q", stageAttempt: "1",
                jobAttempt: "1", logicalJobName: "tests");
            HelixJobInfo ha2 = HelixJob("ha2", "finished", stageName: "Test",
                submitterJobName: "A", queueId: "q", stageAttempt: "2",
                jobAttempt: "2", logicalJobName: "tests");
            HelixJobInfo hb1 = HelixJob("hb1", "finished", stageName: "Test",
                submitterJobName: "B", queueId: "q", stageAttempt: "1",
                jobAttempt: "1", logicalJobName: "tests");
            HelixJobInfo rb2 = HelixJob("rb2", "finished", stageName: "Test",
                submitterJobName: "B", queueId: "q", previousHelixJobName: "hb1",
                stageAttempt: "2", jobAttempt: "1", logicalJobName: "tests");

            helix.AddResponse(
                jobs: [ha1, ha2, hb1],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["ha1"] = PassFail(failed: ["a"]),
                    ["ha2"] = PassFail(passed: ["a"]),
                    ["hb1"] = PassFail(failed: ["b"]),
                });
            helix.ConfigureResubmission("hb1", "rb2");
            helix.AddResponse(
                jobs: [ha1, ha2, hb1, rb2],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["ha1"] = PassFail(failed: ["a"]),
                    ["ha2"] = PassFail(passed: ["a"]),
                    ["hb1"] = PassFail(failed: ["b"]),
                    ["rb2"] = PassFail(passed: ["b"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";

            int exitCode = await new JobMonitorRunner(
                options, NullLogger.Instance, azdo, helix, NoDelay).RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            helix.Resubmissions.Should().ContainSingle()
                .Which.OriginalJob.Should().Be("hb1");
        }

        /// <summary>
        /// A submitter that reran supersedes its previous Helix work even when the new execution
        /// intentionally produces no replacement Helix job.
        /// </summary>
        [Fact]
        public async Task AttemptScoped_RerunSubmitterProducesNoReplacement_DoesNotReplayOldWork()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]),
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"),
                PipelineJob("A", "completed", "succeeded", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"));
            helix.AddResponse(
                jobs: [HelixJob("ha1", "finished", stageName: "Test",
                    submitterJobName: "A", queueId: "q", stageAttempt: "1",
                    jobAttempt: "1", logicalJobName: "tests")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["ha1"] = PassFail(failed: ["a"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";

            int exitCode = await new JobMonitorRunner(
                options, NullLogger.Instance, azdo, helix, NoDelay).RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            helix.Resubmissions.Should().BeEmpty();
        }

        /// <summary>
        /// Corner case 6 (un-resubmittable previous work). Previous-attempt work that cannot be
        /// resubmitted (e.g. its Helix queue was removed) must fail the monitor fast with actionable
        /// output rather than hanging forever or silently passing.
        /// </summary>
        [Fact]
        public async Task AttemptScoped_UnresubmittablePreviousWork_FailsFast()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]),
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"),
                PipelineJob("Test_Linux", "completed", "succeeded", attempt: 1, parentId: "stage-test"));

            HelixJobInfo purged = HelixJob("helix-purged-a1", "running", stageName: "Test",
                submitterJobName: "Test_Linux", queueId: "gone", stageAttempt: "1", jobAttempt: "1");
            helix.WithWorkItems("helix-purged-a1",
                [new WorkItemSummary("helix-purged-a1/wi-1", "helix-purged-a1", "wi-1", "Waiting")]);

            helix.AddResponse(jobs: [purged]);
            helix.ConfigureNullResubmission("helix-purged-a1");

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";
            var runner = new JobMonitorRunner(options, logger, azdo, helix, NoDelay);

            int exitCode = await runner.RunAsync(CancellationToken.None);

            // Failed fast (did not hang), reported the abandoned work, and failed the monitor.
            exitCode.Should().Be(1);
            helix.Resubmissions.Should().ContainSingle()
                .Which.NewJob.Should().BeNull();
            logger.Messages.Should().Contain(m =>
                m.Contains("Could not resubmit", StringComparison.Ordinal)
                && m.Contains("previous attempt", StringComparison.Ordinal));
        }

        [Fact]
        public async Task AttemptScoped_MissingSubmitterAttemptMetadata_DoesNotSpeculativelyResubmit()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]),
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"),
                PipelineJob("A", "completed", "succeeded", attempt: 1, parentId: "stage-test"));
            helix.AddResponse(
                jobs: [HelixJob("ha1", "finished", stageName: "Test",
                    submitterJobName: "A", queueId: "q", stageAttempt: "1",
                    logicalJobName: "tests")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["ha1"] = PassFail(failed: ["a"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";

            int exitCode = await new JobMonitorRunner(
                options, logger, azdo, helix, NoDelay).RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            helix.Resubmissions.Should().BeEmpty();
            logger.Messages.Should().Contain(message =>
                message.Contains("could not be matched to compatible System.JobAttempt metadata", StringComparison.Ordinal));
        }

        [Fact]
        public async Task AttemptScoped_TimelineAttemptOlderThanHelixMetadata_DoesNotSpeculativelyResubmit()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: 3,
                    previousAttempts: [PreviousAttempt(1), PreviousAttempt(2)]),
                MonitorJob(attempt: 3, previousAttempts: [PreviousAttempt(1), PreviousAttempt(2)],
                    parentId: "stage-test"),
                PipelineJob("A", "completed", "succeeded", attempt: 1, parentId: "stage-test"));
            helix.AddResponse(
                jobs: [HelixJob("ha2", "finished", stageName: "Test",
                    submitterJobName: "A", queueId: "q", stageAttempt: "2",
                    jobAttempt: "2", logicalJobName: "tests")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["ha2"] = PassFail(failed: ["a"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "3";
            options.JobAttempt = "3";

            int exitCode = await new JobMonitorRunner(
                options, logger, azdo, helix, NoDelay).RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            helix.Resubmissions.Should().BeEmpty();
            logger.Messages.Should().Contain(message =>
                message.Contains("could not be matched to compatible System.JobAttempt metadata", StringComparison.Ordinal));
        }

        [Fact]
        public async Task AttemptScoped_AmbiguousDefaultJobIdentity_DoesNotSpeculativelyResubmit()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]),
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"),
                PipelineJob("__default", "completed", "succeeded", attempt: 1,
                    parentId: "phase-a", id: "job-a"),
                PipelineJob("__default", "completed", "succeeded", attempt: 1,
                    parentId: "phase-b", id: "job-b"));
            helix.AddResponse(
                jobs: [HelixJob("ha1", "finished", stageName: "Test",
                    submitterJobName: "__default", queueId: "q", stageAttempt: "1",
                    jobAttempt: "1", logicalJobName: "tests")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["ha1"] = PassFail(failed: ["a"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";

            int exitCode = await new JobMonitorRunner(
                options, logger, azdo, helix, NoDelay).RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            helix.Resubmissions.Should().BeEmpty();
            logger.Messages.Should().Contain(message =>
                message.Contains("could not be matched to compatible System.JobAttempt metadata", StringComparison.Ordinal));
        }

        [Fact]
        public async Task AttemptScoped_CurrentStageFailure_IsObservedButNotReplayedOnEntry()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)]),
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"),
                PipelineJob("A", "completed", "succeeded", attempt: 2,
                    previousAttempts: [PreviousAttempt(1)], parentId: "stage-test"));
            helix.AddResponse(
                jobs: [HelixJob("ha2", "finished", stageName: "Test",
                    submitterJobName: "A", queueId: "q", stageAttempt: "2",
                    jobAttempt: "2", logicalJobName: "tests")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["ha2"] = PassFail(failed: ["a"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";

            int exitCode = await new JobMonitorRunner(
                options, NullLogger.Instance, azdo, helix, NoDelay).RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            helix.Resubmissions.Should().BeEmpty();
        }

        [Fact]
        public void SubmitterChainKey_IncludesStageIdentity()
        {
            var state = new MonitorState();
            HelixJobInfo build = HelixJob("build-job", "finished", stageName: "Build",
                submitterPhaseName: "Tests", queueId: "q", logicalJobName: "tests");
            HelixJobInfo test = HelixJob("test-job", "finished", stageName: "Test",
                submitterPhaseName: "Tests", queueId: "q", logicalJobName: "tests");

            state.GetHelixJobChainKey(build).Should().NotBe(state.GetHelixJobChainKey(test));
        }

        /// <summary>
        /// Backward compatibility: when the monitor's own stage attempt is unknown (no
        /// <c>SYSTEM_STAGEATTEMPT</c>), attempt scoping is disabled and the monitor tracks jobs
        /// from every attempt, matching the historical build + stage behavior.
        /// </summary>
        [Fact]
        public async Task AttemptScopedMonitor_UnknownMonitorAttempt_TracksAllAttempts()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress"),
                MonitorJob(parentId: "stage-test"),
                PipelineJob("Test Linux", "completed", "succeeded", parentId: "stage-test"));

            helix.AddResponse(
                jobs: [HelixJob("helix-attempt1", "finished", stageName: "Test", stageAttempt: "1")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-attempt1"] = PassFail(passed: ["workitem-1"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = null;
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix, NoDelay);

            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-attempt1"]);
        }

        /// <summary>
        /// End-to-end multi-attempt scenario spanning five stage attempts, including one attempt
        /// in which the monitor never ran:
        /// <list type="number">
        /// <item>Attempt 1: submitters submit three streams (A, B, C); A passes, B and C fail.</item>
        /// <item>Attempt 2: the monitor resubmits both failing streams (B, C). B's resubmission
        /// fails and C's is still Waiting when the monitor times out.</item>
        /// <item>Attempt 3: the monitor resubmits the failing (B) and Waiting (C) streams. B now
        /// passes; C is still Waiting at timeout.</item>
        /// <item>Attempt 4: the monitor crashes before starting — no invocation, no change.</item>
        /// <item>Attempt 5: the monitor must resubmit ONLY the still-Waiting C stream — not the
        /// now-passing B stream — even though both were last touched in attempt 3.</item>
        /// </list>
        /// Each attempt is a distinct monitor invocation stamped with an increasing
        /// SYSTEM_STAGEATTEMPT, over durable Helix state that accumulates across invocations.
        /// </summary>
        [Fact]
        public async Task MultiAttempt_ResubmitsOnlyUnfinishedStreamsAcrossAttemptsAndMonitorCrash()
        {
            HelixJobInfo a1 = HelixJob("a1", "finished", stageName: "Test", submitterJobName: "Test_A", queueId: "q1", stageAttempt: "1", jobAttempt: "1");
            HelixJobInfo b1 = HelixJob("b1", "finished", stageName: "Test", submitterJobName: "Test_B", queueId: "q1", stageAttempt: "1", jobAttempt: "1");
            HelixJobInfo c1 = HelixJob("c1", "finished", stageName: "Test", submitterJobName: "Test_C", queueId: "q1", stageAttempt: "1", jobAttempt: "1");

            // Attempt 2: resubmit both failing streams (B, C) into attempt 2, then time out.
            FakeHelixService helix2 = await RunAttemptToTimeoutAsync(
                stageAttempt: "2",
                entryLeaves: [a1, b1, c1],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["a1"] = PassFail(passed: ["a-wi"]),
                    ["b1"] = PassFail(failed: ["b-wi"]),
                    ["c1"] = PassFail(failed: ["c-wi"]),
                },
                waitingWorkItems: [],
                resubmissions: [("b1", "b2"), ("c1", "c2")]);

            helix2.Resubmissions.Select(r => r.OriginalJob).Should().BeEquivalentTo(["b1", "c1"]);
            helix2.Resubmissions.Should().OnlyContain(r => r.TargetStageAttempt == "2");

            // Attempt 3: B's attempt-2 resubmission failed; C's is still Waiting. Resubmit both
            // into attempt 3, then time out.
            HelixJobInfo b2 = HelixJob("b2", "finished", stageName: "Test", submitterJobName: "Test_B", queueId: "q1", previousHelixJobName: "b1", stageAttempt: "2", jobAttempt: "1");
            HelixJobInfo c2 = HelixJob("c2", "running", stageName: "Test", submitterJobName: "Test_C", queueId: "q1", previousHelixJobName: "c1", stageAttempt: "2", jobAttempt: "1");

            FakeHelixService helix3 = await RunAttemptToTimeoutAsync(
                stageAttempt: "3",
                entryLeaves: [a1, b2, c2],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["a1"] = PassFail(passed: ["a-wi"]),
                    ["b2"] = PassFail(failed: ["b-wi"]),
                },
                waitingWorkItems: [("c2", [WaitingItem("c2", "c-wi")])],
                resubmissions: [("b2", "b3"), ("c2", "c3")]);

            helix3.Resubmissions.Select(r => r.OriginalJob).Should().BeEquivalentTo(["b2", "c2"]);
            helix3.Resubmissions.Should().OnlyContain(r => r.TargetStageAttempt == "3");

            // Attempt 4: the monitor crashes before starting. No invocation runs, so Helix state
            // is unchanged (represented by simply not running an attempt here).

            // Attempt 5: B's attempt-3 resubmission PASSED; C's is still Waiting. The monitor must
            // resubmit ONLY the still-Waiting C stream, not the now-passing B stream, and complete
            // once C's resubmission finishes.
            HelixJobInfo b3 = HelixJob("b3", "finished", stageName: "Test", submitterJobName: "Test_B", queueId: "q1", previousHelixJobName: "b2", stageAttempt: "3", jobAttempt: "1");
            HelixJobInfo c3 = HelixJob("c3", "running", stageName: "Test", submitterJobName: "Test_C", queueId: "q1", previousHelixJobName: "c2", stageAttempt: "3", jobAttempt: "1");
            HelixJobInfo c5 = HelixJob("c5", "finished", stageName: "Test", submitterJobName: "Test_C", queueId: "q1", previousHelixJobName: "c3", stageAttempt: "5", jobAttempt: "1");

            var azdo5 = new FakeAzureDevOpsService();
            azdo5.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: 5),
                MonitorJob(attempt: 5, parentId: "stage-test"),
                PipelineJob("Test_C", "completed", "succeeded", attempt: 1, parentId: "stage-test"),
                PipelineJob("Test_B", "completed", "succeeded", attempt: 1, parentId: "stage-test"),
                PipelineJob("Test_A", "completed", "succeeded", attempt: 1, parentId: "stage-test"));
            // A and B were already uploaded by earlier attempts (durable AzDO tags).
            azdo5.WithPreviouslyProcessedJob("a1").WithPreviouslyProcessedJob("b3");

            var helix5 = new FakeHelixService();
            // On entry: A passed, B passed, C still Waiting.
            helix5.AddResponse(
                jobs: [a1, b3, c3],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["a1"] = PassFail(passed: ["a-wi"]),
                    ["b3"] = PassFail(passed: ["b-wi"]),
                });
            helix5.WithWorkItems("c3", [WaitingItem("c3", "c-wi")]);
            // After the retry pass resubmits C, its resubmission finishes and passes.
            helix5.AddResponse(
                jobs: [a1, b3, c3, c5],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["a1"] = PassFail(passed: ["a-wi"]),
                    ["b3"] = PassFail(passed: ["b-wi"]),
                    ["c5"] = PassFail(passed: ["c-wi"]),
                });
            helix5.ConfigureResubmission("c3", "c5");

            var options5 = DefaultOptions();
            options5.StageAttempt = "5";
            options5.JobAttempt = "5";
            var runner5 = new JobMonitorRunner(options5, NullLogger.Instance, azdo5, helix5, NoDelay);

            int exit5 = await runner5.RunAsync(CancellationToken.None);

            exit5.Should().Be(0);
            helix5.Resubmissions.Should().ContainSingle();
            helix5.Resubmissions[0].OriginalJob.Should().Be("c3");
            helix5.Resubmissions[0].FailedItems.Should().BeEquivalentTo(["c-wi"]);
            helix5.Resubmissions[0].TargetStageAttempt.Should().Be("5");
        }

        /// <summary>
        /// Variant of the multi-attempt scenario: if the C stream — Waiting at the previous
        /// timeout — has PASSED by the time the monitor is retried, it must not be resubmitted.
        /// With every stream's latest incarnation passed, the monitor completes with no
        /// resubmissions.
        /// </summary>
        [Fact]
        public async Task MultiAttempt_PreviouslyWaitingStreamHasPassedOnRetry_NotResubmitted()
        {
            HelixJobInfo a1 = HelixJob("a1", "finished", stageName: "Test", submitterJobName: "Test_A", queueId: "q1", stageAttempt: "1", jobAttempt: "1");
            HelixJobInfo b3 = HelixJob("b3", "finished", stageName: "Test", submitterJobName: "Test_B", queueId: "q1", previousHelixJobName: "b2", stageAttempt: "3", jobAttempt: "1");
            // Same C incarnation that was Waiting at the last timeout, now reported finished+passed.
            HelixJobInfo c3 = HelixJob("c3", "finished", stageName: "Test", submitterJobName: "Test_C", queueId: "q1", previousHelixJobName: "c2", stageAttempt: "3", jobAttempt: "1");

            var azdo = new FakeAzureDevOpsService();
            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: 5),
                MonitorJob(attempt: 5, parentId: "stage-test"),
                PipelineJob("Test_A", "completed", "succeeded", attempt: 1, parentId: "stage-test"),
                PipelineJob("Test_B", "completed", "succeeded", attempt: 1, parentId: "stage-test"),
                PipelineJob("Test_C", "completed", "succeeded", attempt: 1, parentId: "stage-test"));
            azdo.WithPreviouslyProcessedJob("a1").WithPreviouslyProcessedJob("b3");

            var helix = new FakeHelixService();
            helix.AddResponse(
                jobs: [a1, b3, c3],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["a1"] = PassFail(passed: ["a-wi"]),
                    ["b3"] = PassFail(passed: ["b-wi"]),
                    ["c3"] = PassFail(passed: ["c-wi"]),
                });

            var options = DefaultOptions();
            options.StageAttempt = "5";
            options.JobAttempt = "5";
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix, NoDelay);

            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            helix.Resubmissions.Should().BeEmpty();
        }

        /// <summary>
        /// A monitor job times out while a work item is still <c>Waiting</c> (never dispatched);
        /// nothing is uploaded for that job because it never completed. On the retry attempt that
        /// same work item has completed and passed: the monitor must NOT resubmit it, and it must
        /// process (upload) its test results.
        /// </summary>
        [Fact]
        public async Task WorkItemWaitingAtTimeout_CompletedOnRetry_NotResubmittedAndResultsUploaded()
        {
            // --- Attempt 1: monitor times out with the work item still Waiting ---
            var azdo1 = new FakeAzureDevOpsService();
            azdo1.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress"),
                MonitorJob(parentId: "stage-test"),
                PipelineJob("Test Suite", "completed", "succeeded", parentId: "stage-test"));

            var helix1 = new FakeHelixService();
            helix1.AddResponse(
                jobs: [HelixJob("helix-a", "running", stageName: "Test", submitterJobName: "Test_A", queueId: "q1", stageAttempt: "1")]);
            helix1.WithWorkItems("helix-a", [WaitingItem("helix-a", "wi-1")]);

            var options1 = DefaultOptions();
            options1.StageAttempt = "1";
            using var cts = new CancellationTokenSource();
            var runner1 = new JobMonitorRunner(options1, NullLogger.Instance, azdo1, helix1,
                (_, _) => { cts.Cancel(); return Task.CompletedTask; });

            int exit1 = await runner1.RunAsync(cts.Token);

            exit1.Should().Be(1);
            // Current-attempt in-flight work is gated on, not resubmitted; and since the job never
            // completed, nothing was uploaded for it.
            helix1.Resubmissions.Should().BeEmpty();
            azdo1.UploadedJobNames.Should().BeEmpty();

            // --- Attempt 2 (retry): the previously-Waiting work item has now completed + passed ---
            var azdo2 = new FakeAzureDevOpsService();
            azdo2.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress"),
                MonitorJob(parentId: "stage-test"),
                PipelineJob("Test Suite", "completed", "succeeded", parentId: "stage-test"));

            var helix2 = new FakeHelixService();
            helix2.AddResponse(
                jobs: [HelixJob("helix-a", "finished", stageName: "Test", submitterJobName: "Test_A", queueId: "q1", stageAttempt: "1")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-a"] = PassFail(passed: ["wi-1"]),
                });

            var options2 = DefaultOptions();
            options2.StageAttempt = "2";
            var runner2 = new JobMonitorRunner(options2, NullLogger.Instance, azdo2, helix2, NoDelay);

            int exit2 = await runner2.RunAsync(CancellationToken.None);

            exit2.Should().Be(0);
            // Completed on retry → not resubmitted, and its results are processed (uploaded).
            helix2.Resubmissions.Should().BeEmpty();
            azdo2.UploadedJobNames.Should().BeEquivalentTo(["helix-a"]);
        }

        /// <summary>
        /// Stage-scoped monitoring also applies to entry-time retry. A failed Helix job from
        /// another stage must not be resubmitted or uploaded by this monitor.
        /// </summary>
        [Fact]
        public async Task StageScopedMonitor_DoesNotResubmitFailedHelixJobsOutsideStage_ExitOne()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress"),
                MonitorJob(parentId: "stage-test"),
                PipelineJob("Test Linux", "completed", "succeeded", parentId: "stage-test"),
                StageRecord("Build", "stage-build", "completed", "failed"),
                PipelineJob("Build Windows", "completed", "failed", parentId: "stage-build"));

            helix.AddResponse(
                jobs: [HelixJob("helix-build-windows", "finished", stageName: "Build")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-build-windows"] = PassFail(failed: ["build-fail"]),
                });
            helix.ConfigureResubmission("helix-build-windows", "helix-build-windows-resub");

            var runner = CreateRunner(azdo, helix, stageName: "Test");
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            helix.Resubmissions.Should().BeEmpty();
            azdo.UploadedJobNames.Should().BeEmpty();
            azdo.CreatedTestRuns.Should().BeEmpty();
        }

        /// <summary>
        /// Two stage-scoped monitors in the same build should each exit as soon as the
        /// pipeline and Helix work from their own stage is complete, even when another stage
        /// still has unfinished pipeline or Helix work.
        /// </summary>
        [Fact]
        public async Task StageScopedMonitors_ShutDownWhenTheirOwnStageWorkCompletes()
        {
            var testAzdo = new FakeAzureDevOpsService();
            var testHelix = new FakeHelixService();
            AddStageTimelineFrames(testAzdo);
            AddHelixFrames(testHelix);

            var buildAzdo = new FakeAzureDevOpsService();
            var buildHelix = new FakeHelixService();
            AddStageTimelineFrames(buildAzdo);
            AddHelixFrames(buildHelix);

            var testRunner = CreateRunner(testAzdo, testHelix, stageName: "Test");
            var buildRunner = CreateRunner(buildAzdo, buildHelix, stageName: "Build");

            int testExitCode = await testRunner.RunAsync(CancellationToken.None);
            int buildExitCode = await buildRunner.RunAsync(CancellationToken.None);

            testExitCode.Should().Be(0);
            buildExitCode.Should().Be(0);

            // Test stage finishes in frame 2 while Build stage and its Helix job are still running.
            testAzdo.TimelineCallCount.Should().Be(2);
            testAzdo.UploadedJobNames.Should().BeEquivalentTo(["helix-test-linux"]);

            // Build stage pipeline work finishes in frame 3, but its Helix job finishes in frame 4.
            buildAzdo.TimelineCallCount.Should().Be(4);
            buildAzdo.UploadedJobNames.Should().BeEquivalentTo(["helix-build-windows"]);

            static void AddStageTimelineFrames(FakeAzureDevOpsService azdo)
            {
                azdo.AddTimelineResponse(
                    StageRecord("Test", "stage-test", "inProgress"),
                    MonitorJob(parentId: "stage-test"),
                    PipelineJob("Test Linux", "inProgress", parentId: "stage-test"),
                    StageRecord("Build", "stage-build", "inProgress"),
                    MonitorJob(parentId: "stage-build"),
                    PipelineJob("Build Windows", "inProgress", parentId: "stage-build"));

                azdo.AddTimelineResponse(
                    StageRecord("Test", "stage-test", "inProgress"),
                    MonitorJob(parentId: "stage-test"),
                    PipelineJob("Test Linux", "completed", "succeeded", parentId: "stage-test"),
                    StageRecord("Build", "stage-build", "inProgress"),
                    MonitorJob(parentId: "stage-build"),
                    PipelineJob("Build Windows", "inProgress", parentId: "stage-build"));

                azdo.AddTimelineResponse(
                    StageRecord("Test", "stage-test", "inProgress"),
                    MonitorJob(parentId: "stage-test"),
                    PipelineJob("Test Linux", "completed", "succeeded", parentId: "stage-test"),
                    StageRecord("Build", "stage-build", "inProgress"),
                    MonitorJob(parentId: "stage-build"),
                    PipelineJob("Build Windows", "completed", "succeeded", parentId: "stage-build"));

                azdo.AddTimelineResponse(
                    StageRecord("Test", "stage-test", "inProgress"),
                    MonitorJob(parentId: "stage-test"),
                    PipelineJob("Test Linux", "completed", "succeeded", parentId: "stage-test"),
                    StageRecord("Build", "stage-build", "inProgress"),
                    MonitorJob(parentId: "stage-build"),
                    PipelineJob("Build Windows", "completed", "succeeded", parentId: "stage-build"));
            }

            static void AddHelixFrames(FakeHelixService helix)
            {
                helix.AddResponse(
                    jobs:
                    [
                        HelixJob("helix-test-linux", "running", stageName: "Test"),
                        HelixJob("helix-build-windows", "running", stageName: "Build"),
                    ]);

                helix.AddResponse(
                    jobs:
                    [
                        HelixJob("helix-test-linux", "finished", stageName: "Test"),
                        HelixJob("helix-build-windows", "running", stageName: "Build"),
                    ],
                    passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                    {
                        ["helix-test-linux"] = PassFail(passed: ["test-linux-wi"]),
                    });

                helix.AddResponse(
                    jobs:
                    [
                        HelixJob("helix-test-linux", "finished", stageName: "Test"),
                        HelixJob("helix-build-windows", "running", stageName: "Build"),
                    ],
                    passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                    {
                        ["helix-test-linux"] = PassFail(passed: ["test-linux-wi"]),
                    });

                helix.AddResponse(
                    jobs:
                    [
                        HelixJob("helix-test-linux", "finished", stageName: "Test"),
                        HelixJob("helix-build-windows", "finished", stageName: "Build"),
                    ],
                    passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                    {
                        ["helix-test-linux"] = PassFail(passed: ["test-linux-wi"]),
                        ["helix-build-windows"] = PassFail(passed: ["build-windows-wi"]),
                });
            }
        }

        /// <summary>
        /// When only one stage has a job monitor, retry-on-entry should resubmit only failed Helix
        /// work from that stage. Failed Helix work from a different stage without a monitor is not
        /// in scope for this runner.
        /// </summary>
        [Fact]
        public async Task StageScopedMonitor_OnStageRetry_OnlyRetriesFailedHelixWorkFromItsStage()
        {
            var azdo1 = new FakeAzureDevOpsService();
            var helix1 = new FakeHelixService();

            AddSingleMonitorStageTimeline(azdo1, attempt: 1);
            AddSingleMonitorStageTimeline(azdo1, attempt: 1);

            helix1.AddResponse(
                jobs:
                [
                    HelixJob("helix-test-linux", "running", stageName: "Test", submitterJobName: "Test Linux"),
                    HelixJob("helix-build-windows", "running", stageName: "Build", submitterJobName: "Build Windows"),
                ]);
            helix1.AddResponse(
                jobs:
                [
                    HelixJob("helix-test-linux", "finished", stageName: "Test", submitterJobName: "Test Linux"),
                    HelixJob("helix-build-windows", "finished", stageName: "Build", submitterJobName: "Build Windows"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-test-linux"] = PassFail(failed: ["test-fail"]),
                    ["helix-build-windows"] = PassFail(failed: ["build-fail"]),
                });

            var runner1 = CreateRunner(azdo1, helix1, stageName: "Test");
            int exitCode1 = await runner1.RunAsync(CancellationToken.None);

            exitCode1.Should().Be(1);
            helix1.Resubmissions.Should().BeEmpty();
            azdo1.UploadedJobNames.Should().BeEquivalentTo(["helix-test-linux"]);

            var azdo2 = new FakeAzureDevOpsService();
            azdo2.WithPreviouslyProcessedJob("helix-test-linux");
            var helix2 = new FakeHelixService();

            AddSingleMonitorStageTimeline(azdo2, attempt: 2);
            AddSingleMonitorStageTimeline(azdo2, attempt: 2);

            helix2.AddResponse(
                jobs:
                [
                    HelixJob("helix-test-linux", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-build-windows", "finished", stageName: "Build", submitterJobName: "Build Windows"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-test-linux"] = PassFail(failed: ["test-fail"]),
                    ["helix-build-windows"] = PassFail(failed: ["build-fail"]),
                });
            helix2.ConfigureResubmission("helix-test-linux", "helix-test-linux-resub");
            helix2.ConfigureResubmission("helix-build-windows", "helix-build-windows-resub");
            helix2.AddResponse(
                jobs:
                [
                    HelixJob("helix-test-linux", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-test-linux-resub", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", previousHelixJobName: "helix-test-linux",
                        stageAttempt: "2", jobAttempt: "1"),
                    HelixJob("helix-build-windows", "finished", stageName: "Build", submitterJobName: "Build Windows"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-test-linux"] = PassFail(failed: ["test-fail"]),
                    ["helix-test-linux-resub"] = PassFail(passed: ["test-fail"]),
                    ["helix-build-windows"] = PassFail(failed: ["build-fail"]),
                });

            JobMonitorOptions options2 = DefaultOptions();
            options2.StageName = "Test";
            options2.StageAttempt = "2";
            options2.JobAttempt = "2";
            var runner2 = new JobMonitorRunner(options2, NullLogger.Instance, azdo2, helix2, NoDelay);
            int exitCode2 = await runner2.RunAsync(CancellationToken.None);

            exitCode2.Should().Be(0);
            helix2.Resubmissions.Should().ContainSingle();
            helix2.Resubmissions[0].OriginalJob.Should().Be("helix-test-linux");
            helix2.Resubmissions[0].FailedItems.Should().BeEquivalentTo(["test-fail"]);
            helix2.Resubmissions.Should().NotContain(r => r.OriginalJob == "helix-build-windows");
            azdo2.UploadedJobNames.Should().BeEquivalentTo(["helix-test-linux-resub"]);

            static void AddSingleMonitorStageTimeline(FakeAzureDevOpsService azdo, int attempt)
            {
                PreviousAttemptReference[] previousAttempts = attempt == 1 ? null : [PreviousAttempt(1)];

                azdo.AddTimelineResponse(
                    StageRecord("Test", "stage-test", "inProgress", attempt: attempt,
                        previousAttempts: previousAttempts),
                    MonitorJob(attempt: attempt, previousAttempts: previousAttempts, parentId: "stage-test"),
                    PipelineJob("Test Linux", "completed", "succeeded", attempt: 1, parentId: "stage-test"),
                    StageRecord("Build", "stage-build", "completed", "succeeded"),
                    PipelineJob("Build Windows", "completed", "succeeded", parentId: "stage-build"));
            }
        }

        /// <summary>
        /// In real AzDO timelines, the monitor phase has refName "HelixJobMonitor", but the
        /// monitor job underneath it has refName "__default". A retried monitor must still ignore
        /// that in-progress job when deciding whether its stage's pipeline work is complete.
        /// </summary>
        [Fact]
        public async Task StageScopedMonitor_OnRetry_IgnoresDefaultRefNameMonitorJobUnderMonitorPhase()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            AddRetriedStageTimeline(azdo);
            AddRetriedStageTimeline(azdo);

            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished", stageName: "Test",
                    submitterJobName: "Build_Debug", submitterPhaseName: "Linux Build_Debug",
                    stageAttempt: "1", jobAttempt: "1")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["linux-fail"]),
                });
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub");
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished", stageName: "Test",
                        submitterJobName: "Build_Debug", submitterPhaseName: "Linux Build_Debug",
                        stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-linux-resub", "finished", stageName: "Test",
                        submitterJobName: "Build_Debug", submitterPhaseName: "Linux Build_Debug",
                        previousHelixJobName: "helix-linux", stageAttempt: "2", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["linux-fail"]),
                    ["helix-linux-resub"] = PassFail(failed: ["linux-fail"]),
                });

            int delayCount = 0;
            var options = DefaultOptions();
            options.StageName = "Test";
            options.StageAttempt = "2";
            options.JobAttempt = "2";
            options.JobMonitorName = "HelixJobMonitor";
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix,
                (_, _) =>
                {
                    delayCount++;
                    if (delayCount > 1)
                    {
                        throw new InvalidOperationException("The monitor should exit after the retried Helix job completes.");
                    }

                    return Task.CompletedTask;
                });

            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            delayCount.Should().Be(1);
            helix.Resubmissions.Should().ContainSingle();
            helix.Resubmissions[0].OriginalJob.Should().Be("helix-linux");
            helix.Resubmissions[0].FailedItems.Should().BeEquivalentTo(["linux-fail"]);
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux", "helix-linux-resub"]);

            static void AddRetriedStageTimeline(FakeAzureDevOpsService azdo)
            {
                azdo.AddTimelineResponse(
                    StageRecord("Test", "stage-test", "inProgress", attempt: 2,
                        previousAttempts: [PreviousAttempt(1)]),
                    new AzureDevOpsTimelineRecord
                    {
                        Id = "monitor-phase",
                        ParentId = "stage-test",
                        Type = "Phase",
                        ReferenceName = "HelixJobMonitor",
                        Name = "Monitor Helix Jobs",
                        State = "inProgress",
                    },
                    new AzureDevOpsTimelineRecord
                    {
                        Id = "monitor-job",
                        ParentId = "monitor-phase",
                        Type = "Job",
                        ReferenceName = "__default",
                        Name = "Monitor Helix Jobs",
                        Identifier = "Test.HelixJobMonitor.__default",
                        State = "inProgress",
                        Attempt = 2,
                        PreviousAttempts = [PreviousAttempt(1, recordId: "previous-monitor-job")],
                    },
                    PipelinePhase(
                        "Linux Build_Debug",
                        "Linux Build_Debug",
                        "completed",
                        "succeeded",
                        attempt: 1,
                        parentId: "stage-test",
                        id: "linux-phase"),
                    PipelineJob(
                        "__default",
                        "completed",
                        "succeeded",
                        attempt: 1,
                        parentId: "linux-phase",
                        id: "linux-job"));
            }
        }

        /// <summary>
        /// The monitor times out while Helix tests are in progress.
        /// On relaunch, it should pick up where it left off: skip already-processed
        /// Helix jobs and upload results for newly-completed ones.
        /// </summary>
        [Fact]
        public async Task MonitorTimesOut_Relaunched_UploadsRemainingResults()
        {
            // --- First run: processes helix-linux, then times out while helix-windows is running ---
            var azdo1 = new FakeAzureDevOpsService();
            var helix1 = new FakeHelixService();

            // Build jobs both completed
            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "completed", "succeeded"));
            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "completed", "succeeded"));

            // Poll 1: helix-linux finished, helix-windows running
            helix1.AddResponse(
                jobs: [HelixJob("helix-linux", "finished"), HelixJob("helix-windows", "running")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["linux-wi-1"]),
                });
            // Poll 2: helix-windows still running — then the monitor times out
            helix1.AddResponse(
                jobs: [HelixJob("helix-linux", "finished"), HelixJob("helix-windows", "running")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["linux-wi-1"]),
                });

            // Simulate timeout via cancellation after 2 polls. Wait for helix-linux's upload
            // to finish before cancelling so the in-flight upload isn't aborted by the
            // cancellation token.
            int pollCount1 = 0;
            using var cts = new CancellationTokenSource();
            var runner1 = new JobMonitorRunner(DefaultOptions(), NullLogger.Instance, azdo1, helix1,
                async (_, _) =>
                {
                    pollCount1++;
                    if (pollCount1 >= 2)
                    {
                        Task completed = await Task.WhenAny(azdo1.TestRunCompleted.Task, Task.Delay(TimeSpan.FromSeconds(5)));
                        Assert.Same(azdo1.TestRunCompleted.Task, completed);
                        cts.Cancel();
                    }
                });

            int exitCode1 = await runner1.RunAsync(cts.Token);

            // Timed out → exit 1. helix-linux was uploaded, helix-windows was not.
            exitCode1.Should().Be(1);
            azdo1.UploadedJobNames.Should().BeEquivalentTo(["helix-linux"]);

            // --- Second run: monitor relaunched, helix-linux already processed ---
            var azdo2 = new FakeAzureDevOpsService();
            azdo2.WithPreviouslyProcessedJob("helix-linux");
            var helix2 = new FakeHelixService();

            azdo2.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "completed", "succeeded"));

            // helix-windows now finished
            helix2.AddResponse(
                jobs: [HelixJob("helix-linux", "finished"), HelixJob("helix-windows", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["linux-wi-1"]),
                    ["helix-windows"] = PassFail(passed: ["win-wi-1", "win-wi-2"]),
                });

            var runner2 = CreateRunner(azdo2, helix2);
            int exitCode2 = await runner2.RunAsync(CancellationToken.None);

            // helix-linux skipped (already processed), helix-windows uploaded → exit 0
            exitCode2.Should().Be(0);
            azdo2.UploadedJobNames.Should().BeEquivalentTo(["helix-windows"]);
            azdo2.CreatedTestRuns.Should().ContainSingle();
        }

        [Fact]
        public async Task MonitorTimesOut_CancelsLatestInFlightHelixJobs()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "inProgress"));

            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-finished", "finished"),
                    HelixJob("helix-running", "running"),
                    HelixJob("helix-old-attempt", "running"),
                    HelixJob("helix-new-attempt", "running", previousHelixJobName: "helix-old-attempt"),
                    HelixJob("helix-waiting", "waiting"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-finished"] = PassFail(passed: ["finished-work-item"]),
                });

            using var cts = new CancellationTokenSource();
            var runner = new JobMonitorRunner(DefaultOptions(), logger, azdo, helix,
                async (_, _) =>
                {
                    Task completed = await Task.WhenAny(azdo.TestRunCompleted.Task, Task.Delay(TimeSpan.FromSeconds(5)));
                    completed.Should().BeSameAs(azdo.TestRunCompleted.Task);
                    cts.Cancel();
                });

            int exitCode = await runner.RunAsync(cts.Token);

            exitCode.Should().Be(1);
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-finished"]);
            helix.CanceledJobs.Should().BeEquivalentTo(["helix-new-attempt", "helix-running", "helix-waiting"]);
            logger.Messages.Should().Contain(message =>
                message.Contains("Helix job(s) were unfinished or unprocessed", StringComparison.Ordinal)
                && message.Contains("helix-new-attempt", StringComparison.Ordinal)
                && message.Contains("status=running", StringComparison.Ordinal)
                && message.Contains("helix-waiting", StringComparison.Ordinal)
                && message.Contains("status=waiting", StringComparison.Ordinal));
            logger.Messages.Should().Contain(message =>
                message.Contains("non-monitor Azure DevOps pipeline job(s) were still in progress or queued", StringComparison.Ordinal)
                && message.Contains("Test Linux [state=inProgress, result=none]", StringComparison.Ordinal));
        }

        /// <summary>
        /// Regression: on cancellation the monitor must cancel in-flight Helix jobs immediately and
        /// must not gate that on the test-result upload queue draining. An upload that is stuck in a
        /// non-cancellable operation when the timeout fires must neither delay nor prevent Helix job
        /// cancellation; its unfinished results are re-uploaded by a later monitor invocation.
        /// </summary>
        [Fact]
        public async Task MonitorTimesOut_DoesNotWaitForUploadDrainBeforeCancellingHelixJobs()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            // The upload of helix-finished starts but never completes and ignores cancellation,
            // simulating an upload stuck in a non-cancellable network call when the timeout fires.
            var uploadGate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            azdo.UploadBlocker = uploadGate.Task;
            azdo.UploadBlockerIgnoresCancellation = true;

            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "inProgress"));

            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-finished", "finished"),
                    HelixJob("helix-running", "running"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-finished"] = PassFail(passed: ["finished-work-item"]),
                });

            using var cts = new CancellationTokenSource();
            var runner = new JobMonitorRunner(DefaultOptions(), logger, azdo, helix,
                async (_, _) =>
                {
                    // Cancel once the (stuck) upload is genuinely in flight.
                    Task started = await Task.WhenAny(azdo.UploadStarted.Task, Task.Delay(TimeSpan.FromSeconds(5)));
                    started.Should().BeSameAs(azdo.UploadStarted.Task);
                    cts.Cancel();
                });

            // RunAsync must return promptly even though an upload is stuck: it must not block on the
            // drain. The WaitAsync guard fails the test (instead of hanging) if the drain is
            // reintroduced ahead of cancellation.
            int exitCode = await runner.RunAsync(cts.Token).WaitAsync(TimeSpan.FromSeconds(10));

            exitCode.Should().Be(1);
            // The still-running Helix job was cancelled despite the stuck in-flight upload.
            helix.CanceledJobs.Should().BeEquivalentTo(["helix-running"]);
            // The stuck upload never completed, so its job was not recorded as uploaded; a later
            // monitor invocation re-uploads it.
            azdo.UploadedJobNames.Should().BeEmpty();
            azdo.UploadCompleted.Task.IsCompleted.Should().BeFalse();
            logger.Messages.Should().Contain(message =>
                message.Contains("Helix job(s) were unfinished or unprocessed", StringComparison.Ordinal)
                && message.Contains("helix-finished", StringComparison.Ordinal));

            // Release the stuck upload so the abandoned task can finish.
            uploadGate.TrySetResult();
        }

        /// <summary>
        /// Regression: a Helix job that was "running" in the first poll and "finished" in a
        /// later poll must not be listed in the timeout error message or cancelled, because the
        /// monitor's cached snapshot has to reflect the latest Helix-side state when the
        /// timeout fires. Only jobs that are still genuinely in flight at timeout should be
        /// reported / cancelled.
        /// </summary>
        [Fact]
        public async Task MonitorTimesOut_DoesNotReportOrCancelJobsThatFinishedAfterFirstPoll()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var resultsProcessed = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var logger = new RecordingLogger(message =>
            {
                if (message?.Contains(
                    "Test result processing completed for job 'helix-good'",
                    StringComparison.Ordinal) == true)
                {
                    resultsProcessed.TrySetResult();
                }
            });

            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "inProgress"));

            // Poll 1: both jobs running.
            helix.AddResponse(jobs:
            [
                HelixJob("helix-good", "running"),
                HelixJob("helix-stuck", "running"),
            ]);
            // Poll 2+: helix-good has finished on Helix, helix-stuck still running.
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-good", "finished"),
                    HelixJob("helix-stuck", "running"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-good"] = PassFail(passed: ["good-wi"]),
                });

            int pollCount = 0;
            using var cts = new CancellationTokenSource();
            var runner = new JobMonitorRunner(DefaultOptions(), logger, azdo, helix,
                async (_, _) =>
                {
                    pollCount++;
                    if (pollCount >= 2)
                    {
                        // Wait until helix-good is durably marked as processed. Test-run completion
                        // alone occurs before the monitor state is updated and races cancellation.
                        await resultsProcessed.Task.WaitAsync(TimeSpan.FromSeconds(5));
                        cts.Cancel();
                    }
                });

            int exitCode = await runner.RunAsync(cts.Token);

            exitCode.Should().Be(1);

            // helix-good must not appear in the timeout's "had not finished" list because its
            // cached snapshot was overwritten with the latest (finished) state.
            string timeoutMessage = logger.Messages.Should().ContainSingle(m =>
                m.Contains("Helix Job Monitor timed out", StringComparison.Ordinal)).Subject;
            timeoutMessage.Should().StartWith("##vso[task.logissue type=error]");
            timeoutMessage.Should().Contain("helix-stuck");
            timeoutMessage.Should().NotContain("helix-good");

            // And the best-effort cancel pass must only target the still-in-flight job.
            helix.CanceledJobs.Should().BeEquivalentTo(["helix-stuck"]);
        }

        /// <summary>
        /// The monitor times out while some pipeline jobs haven't submitted Helix work yet,
        /// but one job's Helix results were already uploaded. On relaunch, the monitor picks up:
        /// skips already-processed Helix jobs, processes the new ones that have now completed.
        /// </summary>
        [Fact]
        public async Task MonitorTimesOut_PartialProgress_Relaunched_CompletesSuccessfully()
        {
            // --- First run ---
            // Test Linux submitted helix-linux (finished, uploaded).
            // Test Windows is still running (hasn't submitted Helix work yet).
            // Monitor times out.
            var azdo1 = new FakeAzureDevOpsService();
            var helix1 = new FakeHelixService();

            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "inProgress"));
            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "inProgress"));

            // Poll 1: helix-linux finished, no Windows Helix job yet
            helix1.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["linux-wi-1"]),
                });
            // Poll 2: same — Windows build still running, timeout fires
            helix1.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["linux-wi-1"]),
                });

            int pollCount1 = 0;
            using var cts = new CancellationTokenSource();
            var runner1 = new JobMonitorRunner(DefaultOptions(), NullLogger.Instance, azdo1, helix1,
                async (_, _) =>
                {
                    pollCount1++;
                    if (pollCount1 >= 2)
                    {
                        Task completed = await Task.WhenAny(azdo1.TestRunCompleted.Task, Task.Delay(TimeSpan.FromSeconds(5)));
                        Assert.Same(azdo1.TestRunCompleted.Task, completed);
                        cts.Cancel();
                    }
                });

            int exitCode1 = await runner1.RunAsync(cts.Token);
            exitCode1.Should().Be(1);
            azdo1.UploadedJobNames.Should().BeEquivalentTo(["helix-linux"]);

            // --- Second run: monitor relaunched ---
            // Test Windows has now completed and submitted helix-windows.
            // helix-linux already processed.
            var azdo2 = new FakeAzureDevOpsService();
            azdo2.WithPreviouslyProcessedJob("helix-linux");
            var helix2 = new FakeHelixService();

            // Both build jobs done
            azdo2.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "completed", "succeeded"));
            // helix-windows still running
            azdo2.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "completed", "succeeded"));

            // Poll 1: helix-linux still visible (but processed), helix-windows running
            helix2.AddResponse(
                jobs: [HelixJob("helix-linux", "finished"), HelixJob("helix-windows", "running")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["linux-wi-1"]),
                });
            // Poll 2: helix-windows finished
            helix2.AddResponse(
                jobs: [HelixJob("helix-linux", "finished"), HelixJob("helix-windows", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["linux-wi-1"]),
                    ["helix-windows"] = PassFail(passed: ["win-wi-1"]),
                });

            var runner2 = CreateRunner(azdo2, helix2);
            int exitCode2 = await runner2.RunAsync(CancellationToken.None);

            exitCode2.Should().Be(0);
            azdo2.TimelineCallCount.Should().Be(2);
            // Only helix-windows uploaded (helix-linux was already processed)
            azdo2.UploadedJobNames.Should().BeEquivalentTo(["helix-windows"]);
            azdo2.CreatedTestRuns.Should().ContainSingle();
        }

        // -----------------------------------------------------------------------
        // Retry with Helix work item resubmission
        // -----------------------------------------------------------------------

        /// <summary>
        /// On restart after a failed attempt, the monitor finds a completed Helix job with 2
        /// failed work items. It resubmits the failed items. The resubmitted job completes with
        /// all items passing.
        /// The monitor should exit 0 (the resubmission "healed" the failures).
        /// Only the failed items should be resubmitted. Results from the original job are not
        /// uploaded by this invocation because the failed work items are being resubmitted.
        /// </summary>
        [Fact]
        public async Task RetryAttempt2_ResubmitsFailedWorkItems_ResubmissionPasses_ExitZero()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            // Pipeline jobs already completed
            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1));
            // Extra polls while resubmitted job runs
            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1));
            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1));

            // Poll 1: original Helix job finished with 1 pass + 2 failures
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished", stageName: "Test",
                    submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-ok"], failed: ["wi-fail-1", "wi-fail-2"]),
                });
            // Configure: resubmission of helix-linux creates "helix-linux-resub"
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub");

            // Poll 2: resubmitted job appears, running
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-linux-resub", "running", stageName: "Test",
                        submitterJobName: "Test Linux", previousHelixJobName: "helix-linux",
                        stageAttempt: "2", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-ok"], failed: ["wi-fail-1", "wi-fail-2"]),
                });

            // Poll 3: resubmitted job finished — both items now pass
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-linux-resub", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", previousHelixJobName: "helix-linux",
                        stageAttempt: "2", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-ok"], failed: ["wi-fail-1", "wi-fail-2"]),
                    ["helix-linux-resub"] = PassFail(passed: ["wi-fail-1", "wi-fail-2"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix, NoDelay);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            // Resubmission healed the failures → exit 0
            exitCode.Should().Be(0);

            // Test result upload is independent from retry: original results upload before the resubmission.
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux", "helix-linux-resub"]);
            azdo.CreatedTestRuns.Should().HaveCount(2);

            // Only the 2 failed items were resubmitted (not the passing one)
            helix.Resubmissions.Should().ContainSingle();
            helix.Resubmissions[0].OriginalJob.Should().Be("helix-linux");
            helix.Resubmissions[0].FailedItems.Should().HaveCount(2);
            helix.Resubmissions[0].FailedItems.Should().Contain("wi-fail-1");
            helix.Resubmissions[0].FailedItems.Should().Contain("wi-fail-2");
        }

        /// <summary>
        /// On restart after a failed attempt, the monitor resubmits failed work items, but the
        /// resubmission also fails. The monitor should exit 1.
        /// </summary>
        [Fact]
        public async Task RetryAttempt2_ResubmitsFailedWorkItems_ResubmissionAlsoFails_ExitOne()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded"));
            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded"));

            // Poll 1: original job has 1 failure
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished", stageName: "Test",
                    submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-ok"], failed: ["wi-fail"]),
                });
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub");

            // Poll 2: resubmission finished but STILL fails
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-linux-resub", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", previousHelixJobName: "helix-linux",
                        stageAttempt: "2", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-ok"], failed: ["wi-fail"]),
                    ["helix-linux-resub"] = PassFail(failed: ["wi-fail"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix, NoDelay);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            // Resubmission also failed → exit 1
            exitCode.Should().Be(1);
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux", "helix-linux-resub"]);
        }

        /// <summary>
        /// On restart after a failed attempt, two Helix jobs have failures. Only the failed work
        /// items from each are resubmitted (not passing items). As resubmissions pass, the set of
        /// still-failing items shrinks. Both resubmissions pass → exit 0.
        /// </summary>
        [Fact]
        public async Task RetryAttempt2_MultipleJobs_OnlyFailedItemsResubmitted_ExitZero()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "completed", "succeeded"));
            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "completed", "succeeded"));
            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "completed", "succeeded"));

            // Poll 1: both original jobs finished with mixed results
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-windows", "finished", stageName: "Test",
                        submitterJobName: "Test Windows", stageAttempt: "1", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["linux-ok-1", "linux-ok-2"], failed: ["linux-fail"]),
                    ["helix-windows"] = PassFail(passed: ["win-ok"], failed: ["win-fail-1", "win-fail-2"]),
                });
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub");
            helix.ConfigureResubmission("helix-windows", "helix-windows-resub");

            // Poll 2: linux resubmission finished (passes), windows still running
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished", stageName: "Test", submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-windows", "finished", stageName: "Test", submitterJobName: "Test Windows", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-linux-resub", "finished", stageName: "Test", submitterJobName: "Test Linux", previousHelixJobName: "helix-linux", stageAttempt: "2", jobAttempt: "1"),
                    HelixJob("helix-windows-resub", "running", stageName: "Test", submitterJobName: "Test Windows", previousHelixJobName: "helix-windows", stageAttempt: "2", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["linux-ok-1", "linux-ok-2"], failed: ["linux-fail"]),
                    ["helix-windows"] = PassFail(passed: ["win-ok"], failed: ["win-fail-1", "win-fail-2"]),
                    ["helix-linux-resub"] = PassFail(passed: ["linux-fail"]),
                });

            // Poll 3: windows resubmission also passes
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished", stageName: "Test", submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-windows", "finished", stageName: "Test", submitterJobName: "Test Windows", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-linux-resub", "finished", stageName: "Test", submitterJobName: "Test Linux", previousHelixJobName: "helix-linux", stageAttempt: "2", jobAttempt: "1"),
                    HelixJob("helix-windows-resub", "finished", stageName: "Test", submitterJobName: "Test Windows", previousHelixJobName: "helix-windows", stageAttempt: "2", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["linux-ok-1", "linux-ok-2"], failed: ["linux-fail"]),
                    ["helix-windows"] = PassFail(passed: ["win-ok"], failed: ["win-fail-1", "win-fail-2"]),
                    ["helix-linux-resub"] = PassFail(passed: ["linux-fail"]),
                    ["helix-windows-resub"] = PassFail(passed: ["win-fail-1", "win-fail-2"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix, NoDelay);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);

            // Test result upload is independent from retry: originals upload before resubmissions.
            azdo.CreatedTestRuns.Should().HaveCount(4);
            azdo.UploadedJobNames.Should().BeEquivalentTo(
                ["helix-linux", "helix-windows", "helix-linux-resub", "helix-windows-resub"]);

            // Each original job only had its failed items resubmitted
            helix.Resubmissions.Should().HaveCount(2);
            var linuxResub = helix.Resubmissions.Single(r => r.OriginalJob == "helix-linux");
            linuxResub.FailedItems.Should().ContainSingle(); // only "linux-fail"
            linuxResub.FailedItems.Should().Contain("linux-fail");

            var windowsResub = helix.Resubmissions.Single(r => r.OriginalJob == "helix-windows");
            windowsResub.FailedItems.Should().HaveCount(2);
            windowsResub.FailedItems.Should().Contain("win-fail-1");
            windowsResub.FailedItems.Should().Contain("win-fail-2");
        }

        /// <summary>
        /// Retry is entry-only. If a Helix job is running when the monitor starts and later
        /// completes with failures, that failed work is not resubmitted until the next monitor
        /// invocation.
        /// </summary>
        [Fact]
        public async Task HelixJobFailsAfterMonitorEntry_IsNotResubmittedUntilNextEntry()
        {
            var azdo1 = new FakeAzureDevOpsService();
            var helix1 = new FakeHelixService();

            azdo1.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));
            azdo1.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            helix1.AddResponse(jobs: [HelixJob("helix-linux", "running")]);
            helix1.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["wi-fail"]),
                });

            var runner1 = CreateRunner(azdo1, helix1);
            int exitCode1 = await runner1.RunAsync(CancellationToken.None);

            exitCode1.Should().Be(1);
            helix1.Resubmissions.Should().BeEmpty();
            azdo1.UploadedJobNames.Should().BeEquivalentTo(["helix-linux"]);

            var azdo2 = new FakeAzureDevOpsService();
            azdo2.WithPreviouslyProcessedJob("helix-linux");
            var helix2 = new FakeHelixService();

            azdo2.AddTimelineResponse(MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]), PipelineJob("Test Linux", "completed", "succeeded"));
            azdo2.AddTimelineResponse(MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]), PipelineJob("Test Linux", "completed", "succeeded"));

            helix2.AddResponse(
                jobs: [HelixJob("helix-linux", "finished", stageName: "Test",
                    submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["wi-fail"]),
                });
            helix2.ConfigureResubmission("helix-linux", "helix-linux-resub");
            helix2.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-linux-resub", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", previousHelixJobName: "helix-linux",
                        stageAttempt: "2", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux-resub"] = PassFail(passed: ["wi-fail"]),
                });

            JobMonitorOptions options2 = DefaultOptions();
            options2.StageAttempt = "2";
            options2.JobAttempt = "2";
            var runner2 = new JobMonitorRunner(options2, NullLogger.Instance, azdo2, helix2, NoDelay);
            int exitCode2 = await runner2.RunAsync(CancellationToken.None);

            exitCode2.Should().Be(0);
            helix2.Resubmissions.Should().ContainSingle();
            helix2.Resubmissions[0].OriginalJob.Should().Be("helix-linux");
            helix2.Resubmissions[0].FailedItems.Should().BeEquivalentTo(["wi-fail"]);
            azdo2.UploadedJobNames.Should().BeEquivalentTo(["helix-linux-resub"]);
        }

        /// <summary>
        /// If the monitor cold-starts after a newer incarnation already passed, older failed
        /// Helix jobs are not resubmitted. Test results still upload old-to-new.
        /// </summary>
        [Fact]
        public async Task NewerPassedIncarnationExistsOnEntry_DoesNotResubmitOlderFailure_ExitZero()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished"),
                    HelixJob("helix-linux-resub", "finished", previousHelixJobName: "helix-linux"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["wi-fail"]),
                    ["helix-linux-resub"] = PassFail(passed: ["wi-fail"]),
                });
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub-2");
            helix.ConfigureResubmission("helix-linux-resub", "helix-linux-resub-2");

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            helix.Resubmissions.Should().BeEmpty();
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux", "helix-linux-resub"]);
        }

        /// <summary>
        /// If the latest incarnation is still running, older failed incarnations are not
        /// resubmitted again. The old results still upload once, and the monitor waits for the
        /// running latest incarnation.
        /// </summary>
        [Fact]
        public async Task NewerRunningIncarnationExistsOnEntry_DoesNotResubmitOlderFailure_ExitZero()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished"),
                    HelixJob("helix-linux-resub", "running", previousHelixJobName: "helix-linux"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["wi-fail"]),
                });
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub-2");
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished"),
                    HelixJob("helix-linux-resub", "finished", previousHelixJobName: "helix-linux"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux-resub"] = PassFail(passed: ["wi-fail"]),
                });

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            helix.Resubmissions.Should().BeEmpty();
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux", "helix-linux-resub"]);
        }

        /// <summary>
        /// When a retry partially heals prior failures, the next monitor entry resubmits only the
        /// items that failed in the latest completed incarnation.
        /// </summary>
        [Fact]
        public async Task LatestCompletedIncarnationPartiallyHealed_ResubmitsOnlyRemainingFailures()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                MonitorJob(attempt: 3, previousAttempts: [PreviousAttempt(1), PreviousAttempt(2)]),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1));
            azdo.AddTimelineResponse(
                MonitorJob(attempt: 3, previousAttempts: [PreviousAttempt(1), PreviousAttempt(2)]),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1));

            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-linux-resub", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", previousHelixJobName: "helix-linux",
                        stageAttempt: "2", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["wi-1", "wi-2"]),
                    ["helix-linux-resub"] = PassFail(passed: ["wi-1"], failed: ["wi-2"]),
                });
            helix.ConfigureResubmission("helix-linux-resub", "helix-linux-resub-2");
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-linux-resub", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", previousHelixJobName: "helix-linux",
                        stageAttempt: "2", jobAttempt: "1"),
                    HelixJob("helix-linux-resub-2", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", previousHelixJobName: "helix-linux-resub",
                        stageAttempt: "3", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["wi-1", "wi-2"]),
                    ["helix-linux-resub"] = PassFail(passed: ["wi-1"], failed: ["wi-2"]),
                    ["helix-linux-resub-2"] = PassFail(passed: ["wi-2"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "3";
            options.JobAttempt = "3";
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix, NoDelay);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            helix.Resubmissions.Should().ContainSingle();
            helix.Resubmissions[0].OriginalJob.Should().Be("helix-linux-resub");
            helix.Resubmissions[0].FailedItems.Should().BeEquivalentTo(["wi-2"]);
            azdo.UploadedJobNames.Should().BeEquivalentTo(
                ["helix-linux", "helix-linux-resub", "helix-linux-resub-2"]);
        }

        /// <summary>
        /// Helix snapshots are not guaranteed to be returned in lineage order. Upload still needs
        /// to publish older jobs before newer retry incarnations.
        /// </summary>
        [Fact]
        public async Task CompletedHelixJobsReturnedOutOfOrder_UploadsOldToNew()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux-resub", "finished", previousHelixJobName: "helix-linux"),
                    HelixJob("helix-linux", "finished"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["wi-fail"]),
                    ["helix-linux-resub"] = PassFail(passed: ["wi-fail"]),
                });

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux", "helix-linux-resub"]);
        }

        /// <summary>
        /// A, B, and C start in the first invocation. A and B submit Helix jobs. A's AzDO job
        /// succeeds but its Helix work fails and gets uploaded. B's AzDO job fails after
        /// submitting Helix work, and the monitor exits before B produces test results because C
        /// also fails. On retry, A and B's failed Helix work items should be resubmitted, while C
        /// is restarted as an AzDO job. On the following retry, only B's still-failed resubmission
        /// is submitted again because A's resubmission already passed.
        /// </summary>
        [Fact]
        public async Task RetryAfterMixedAzDOAndHelixFailures_RestartsFailedHelixWorkAndFailedAzDOJob()
        {
            var azdo1 = new FakeAzureDevOpsService();
            var helix1 = new FakeHelixService();

            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("A", "pending"),
                PipelineJob("B", "pending"),
                PipelineJob("C", "pending"));
            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("A", "inProgress"),
                PipelineJob("B", "inProgress"),
                PipelineJob("C", "pending"));
            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("A", "inProgress"),
                PipelineJob("B", "inProgress"),
                PipelineJob("C", "pending"));
            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("A", "inProgress"),
                PipelineJob("B", "inProgress"),
                PipelineJob("C", "inProgress"));
            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("A", "completed", "succeeded"),
                PipelineJob("B", "completed", "failed"),
                PipelineJob("C", "inProgress"));
            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("A", "completed", "succeeded"),
                PipelineJob("B", "completed", "failed"),
                PipelineJob("C", "completed", "failed"));
            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("A", "completed", "succeeded"),
                PipelineJob("B", "completed", "failed"),
                PipelineJob("C", "completed", "failed"));

            helix1.AddResponse(jobs: []);
            helix1.AddResponse(jobs: []);
            helix1.AddResponse(jobs: [HelixJob("helix-a", "running", submitterJobName: "A"), HelixJob("helix-b", "running", submitterJobName: "B")]);
            helix1.AddResponse(jobs: [HelixJob("helix-a", "running", submitterJobName: "A"), HelixJob("helix-b", "running", submitterJobName: "B")]);
            helix1.AddResponse(
                jobs: [HelixJob("helix-a", "finished", submitterJobName: "A"), HelixJob("helix-b", "running", submitterJobName: "B")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-a"] = PassFail(failed: ["a-fail"]),
                });
            helix1.AddResponse(
                jobs: [HelixJob("helix-a", "finished", submitterJobName: "A"), HelixJob("helix-b", "running", submitterJobName: "B")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-a"] = PassFail(failed: ["a-fail"]),
                });
            helix1.AddResponse(
                jobs: [HelixJob("helix-a", "finished", submitterJobName: "A"), HelixJob("helix-b", "finished", submitterJobName: "B")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-b"] = PassFail(passed: ["b-pass"]),
                });

            var runner1 = CreateRunner(azdo1, helix1);
            int exitCode1 = await runner1.RunAsync(CancellationToken.None);

            exitCode1.Should().Be(1);
            azdo1.UploadedJobNames.Should().BeEquivalentTo(["helix-a", "helix-b"]);
            helix1.Resubmissions.Should().BeEmpty();

            var azdo2 = new FakeAzureDevOpsService();
            azdo2.WithPreviouslyProcessedJob("helix-a");
            var helix2 = new FakeHelixService();

            azdo2.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("A", "completed", "succeeded"),
                PipelineJob("B", "completed", "failed"),
                PipelineJob("C", "inProgress", attempt: 2, previousAttempts: [PreviousAttempt(1)]));
            azdo2.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("A", "completed", "succeeded"),
                PipelineJob("B", "completed", "failed"),
                PipelineJob("C", "completed", "succeeded", attempt: 2, previousAttempts: [PreviousAttempt(1)]));

            helix2.AddResponse(
                jobs:
                [
                    HelixJob("helix-a", "finished", stageName: "Test", submitterJobName: "A", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-b", "finished", stageName: "Test", submitterJobName: "B", stageAttempt: "1", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-a"] = PassFail(failed: ["a-fail"]),
                    ["helix-b"] = PassFail(failed: ["b-fail"]),
                });
            helix2.ConfigureResubmission("helix-a", "helix-a-resub");
            helix2.ConfigureResubmission("helix-b", "helix-b-resub");
            helix2.AddResponse(
                jobs:
                [
                    HelixJob("helix-a", "finished", stageName: "Test", submitterJobName: "A", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-b", "finished", stageName: "Test", submitterJobName: "B", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-a-resub", "finished", stageName: "Test", submitterJobName: "A", previousHelixJobName: "helix-a", stageAttempt: "2", jobAttempt: "1"),
                    HelixJob("helix-b-resub", "finished", stageName: "Test", submitterJobName: "B", previousHelixJobName: "helix-b", stageAttempt: "2", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-a-resub"] = PassFail(passed: ["a-fail"]),
                    ["helix-b-resub"] = PassFail(failed: ["b-fail"]),
                });

            JobMonitorOptions options2 = DefaultOptions();
            options2.StageAttempt = "2";
            options2.JobAttempt = "2";
            var runner2 = new JobMonitorRunner(options2, NullLogger.Instance, azdo2, helix2, NoDelay);
            int exitCode2 = await runner2.RunAsync(CancellationToken.None);

            exitCode2.Should().Be(1);
            azdo2.TimelineCallCount.Should().Be(2);
            azdo2.UploadedJobNames.Should().BeEquivalentTo(["helix-b", "helix-a-resub", "helix-b-resub"]);

            helix2.Resubmissions.Should().HaveCount(2);
            helix2.Resubmissions.Single(r => r.OriginalJob == "helix-a").FailedItems.Should().BeEquivalentTo(["a-fail"]);
            helix2.Resubmissions.Single(r => r.OriginalJob == "helix-b").FailedItems.Should().BeEquivalentTo(["b-fail"]);

            var azdo3 = new FakeAzureDevOpsService();
            azdo3.WithPreviouslyProcessedJob("helix-a");
            azdo3.WithPreviouslyProcessedJob("helix-b");
            azdo3.WithPreviouslyProcessedJob("helix-a-resub");
            azdo3.WithPreviouslyProcessedJob("helix-b-resub");
            var helix3 = new FakeHelixService();

            azdo3.AddTimelineResponse(
                MonitorJob(attempt: 3, previousAttempts: [PreviousAttempt(1), PreviousAttempt(2)]),
                PipelineJob("A", "completed", "succeeded"),
                PipelineJob("B", "completed", "failed"),
                PipelineJob("C", "completed", "succeeded", attempt: 2, previousAttempts: [PreviousAttempt(1)]));
            azdo3.AddTimelineResponse(
                MonitorJob(attempt: 3, previousAttempts: [PreviousAttempt(1), PreviousAttempt(2)]),
                PipelineJob("A", "completed", "succeeded"),
                PipelineJob("B", "completed", "failed"),
                PipelineJob("C", "completed", "succeeded", attempt: 2, previousAttempts: [PreviousAttempt(1)]));

            helix3.AddResponse(
                jobs:
                [
                    HelixJob("helix-a", "finished", stageName: "Test", submitterJobName: "A", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-b", "finished", stageName: "Test", submitterJobName: "B", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-a-resub", "finished", stageName: "Test", submitterJobName: "A", previousHelixJobName: "helix-a", stageAttempt: "2", jobAttempt: "1"),
                    HelixJob("helix-b-resub", "finished", stageName: "Test", submitterJobName: "B", previousHelixJobName: "helix-b", stageAttempt: "2", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-a-resub"] = PassFail(passed: ["a-fail"]),
                    ["helix-b-resub"] = PassFail(failed: ["b-fail"]),
                });
            helix3.ConfigureResubmission("helix-b-resub", "helix-b-resub-2");
            helix3.AddResponse(
                jobs:
                [
                    HelixJob("helix-a", "finished", stageName: "Test", submitterJobName: "A", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-b", "finished", stageName: "Test", submitterJobName: "B", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-a-resub", "finished", stageName: "Test", submitterJobName: "A", previousHelixJobName: "helix-a", stageAttempt: "2", jobAttempt: "1"),
                    HelixJob("helix-b-resub", "finished", stageName: "Test", submitterJobName: "B", previousHelixJobName: "helix-b", stageAttempt: "2", jobAttempt: "1"),
                    HelixJob("helix-b-resub-2", "finished", stageName: "Test", submitterJobName: "B", previousHelixJobName: "helix-b-resub", stageAttempt: "3", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-b-resub-2"] = PassFail(passed: ["b-fail"]),
                });

            JobMonitorOptions options3 = DefaultOptions();
            options3.StageAttempt = "3";
            options3.JobAttempt = "3";
            var runner3 = new JobMonitorRunner(options3, NullLogger.Instance, azdo3, helix3, NoDelay);
            int exitCode3 = await runner3.RunAsync(CancellationToken.None);

            exitCode3.Should().Be(0);
            helix3.Resubmissions.Should().ContainSingle();
            helix3.Resubmissions[0].OriginalJob.Should().Be("helix-b-resub");
            helix3.Resubmissions[0].FailedItems.Should().BeEquivalentTo(["b-fail"]);
            azdo3.UploadedJobNames.Should().BeEquivalentTo(["helix-b-resub-2"]);
        }

        /// <summary>
        /// A and B submit Helix jobs before the monitor starts. A's work item fails before the
        /// first monitor invocation, so A is resubmitted on entry and A's original failed results
        /// are still uploaded. After the monitor crashes, A's resubmission is still waiting, so it
        /// is not resubmitted again. After another crash, B fails while the monitor is down; the
        /// next monitor entry resubmits only B. Once A's resubmission and B's resubmission pass,
        /// the monitor exits successfully.
        /// </summary>
        [Fact]
        public async Task RetryOnEntryWithCrashes_ResubmitsOnlyLatestFailedWork()
        {
            var azdo1 = new FakeAzureDevOpsService();
            var helix1 = new FakeHelixService();

            azdo1.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("A", "completed", "succeeded"),
                PipelineJob("B", "completed", "succeeded"));

            helix1.AddResponse(
                jobs: [HelixJob("helix-a", "finished", submitterJobName: "A"), HelixJob("helix-b", "running", submitterJobName: "B")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-a"] = PassFail(failed: ["a-fail"]),
                });
            helix1.ConfigureResubmission("helix-a", "helix-a-resub");
            helix1.AddResponse(
                jobs:
                [
                    HelixJob("helix-a", "finished", submitterJobName: "A"),
                    HelixJob("helix-a-resub", "running", submitterJobName: "A", previousHelixJobName: "helix-a"),
                    HelixJob("helix-b", "running", submitterJobName: "B"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-a"] = PassFail(failed: ["a-fail"]),
                });

            using var cts1 = new CancellationTokenSource();
            var runner1 = new JobMonitorRunner(DefaultOptions(), NullLogger.Instance, azdo1, helix1,
                async (_, _) =>
                {
                    // Wait for helix-a's test-result upload to finish before cancelling so the
                    // assertion below is deterministic. Otherwise the background upload task
                    // (bound to the runner's cancellation token) races against cts1.Cancel().
                    Task completed = await Task.WhenAny(azdo1.TestRunCompleted.Task, Task.Delay(TimeSpan.FromSeconds(5)));
                    completed.Should().BeSameAs(azdo1.TestRunCompleted.Task);
                    cts1.Cancel();
                });

            int exitCode1 = await runner1.RunAsync(cts1.Token);

            exitCode1.Should().Be(1);
            helix1.Resubmissions.Should().BeEmpty();
            azdo1.UploadedJobNames.Should().BeEquivalentTo(["helix-a"]);

            var azdo2 = new FakeAzureDevOpsService();
            azdo2.WithPreviouslyProcessedJob("helix-a");
            var helix2 = new FakeHelixService();

            azdo2.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("A", "completed", "succeeded"),
                PipelineJob("B", "completed", "succeeded"));

            helix2.AddResponse(
                jobs:
                [
                    HelixJob("helix-a", "finished", submitterJobName: "A"),
                    HelixJob("helix-a-resub", "running", submitterJobName: "A", previousHelixJobName: "helix-a"),
                    HelixJob("helix-b", "running", submitterJobName: "B"),
                ]);
            helix2.ConfigureResubmission("helix-a-resub", "helix-a-resub-2");
            helix2.ConfigureResubmission("helix-b", "helix-b-resub");
            helix2.AddResponse(
                jobs:
                [
                    HelixJob("helix-a", "finished", submitterJobName: "A"),
                    HelixJob("helix-a-resub", "running", submitterJobName: "A", previousHelixJobName: "helix-a"),
                    HelixJob("helix-b", "running", submitterJobName: "B"),
                ]);

            using var cts2 = new CancellationTokenSource();
            var runner2 = new JobMonitorRunner(DefaultOptions(), NullLogger.Instance, azdo2, helix2,
                (_, _) =>
                {
                    cts2.Cancel();
                    return Task.CompletedTask;
                });

            int exitCode2 = await runner2.RunAsync(cts2.Token);

            exitCode2.Should().Be(1);
            helix2.Resubmissions.Should().BeEmpty();
            azdo2.UploadedJobNames.Should().BeEmpty();

            var azdo3 = new FakeAzureDevOpsService();
            azdo3.WithPreviouslyProcessedJob("helix-a");
            var helix3 = new FakeHelixService();

            azdo3.AddTimelineResponse(
                MonitorJob(attempt: 3, previousAttempts: [PreviousAttempt(1), PreviousAttempt(2)]),
                PipelineJob("A", "completed", "succeeded"),
                PipelineJob("B", "completed", "succeeded"));
            azdo3.AddTimelineResponse(
                MonitorJob(attempt: 3, previousAttempts: [PreviousAttempt(1), PreviousAttempt(2)]),
                PipelineJob("A", "completed", "succeeded"),
                PipelineJob("B", "completed", "succeeded"));

            helix3.AddResponse(
                jobs:
                [
                    HelixJob("helix-a", "finished", stageName: "Test", submitterJobName: "A", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-a-resub", "running", stageName: "Test", submitterJobName: "A", previousHelixJobName: "helix-a", stageAttempt: "2", jobAttempt: "1"),
                    HelixJob("helix-b", "finished", stageName: "Test", submitterJobName: "B", stageAttempt: "1", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-b"] = PassFail(failed: ["b-fail"]),
                });
            helix3.ConfigureResubmission("helix-b", "helix-b-resub");
            helix3.AddResponse(
                jobs:
                [
                    HelixJob("helix-a", "finished", stageName: "Test", submitterJobName: "A", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-a-resub", "running", stageName: "Test", submitterJobName: "A", previousHelixJobName: "helix-a", stageAttempt: "2", jobAttempt: "1"),
                    HelixJob("helix-b", "finished", stageName: "Test", submitterJobName: "B", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-b-resub", "running", stageName: "Test", submitterJobName: "B", previousHelixJobName: "helix-b", stageAttempt: "3", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-b"] = PassFail(failed: ["b-fail"]),
                });
            helix3.AddResponse(
                jobs:
                [
                    HelixJob("helix-a", "finished", stageName: "Test", submitterJobName: "A", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-a-resub", "finished", stageName: "Test", submitterJobName: "A", previousHelixJobName: "helix-a", stageAttempt: "2", jobAttempt: "1"),
                    HelixJob("helix-b", "finished", stageName: "Test", submitterJobName: "B", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-b-resub", "finished", stageName: "Test", submitterJobName: "B", previousHelixJobName: "helix-b", stageAttempt: "3", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-a-resub"] = PassFail(passed: ["a-fail"]),
                    ["helix-b"] = PassFail(failed: ["b-fail"]),
                    ["helix-b-resub"] = PassFail(passed: ["b-fail"]),
                });

            JobMonitorOptions options3 = DefaultOptions();
            options3.StageAttempt = "3";
            options3.JobAttempt = "3";
            var runner3 = new JobMonitorRunner(options3, NullLogger.Instance, azdo3, helix3, NoDelay);
            int exitCode3 = await runner3.RunAsync(CancellationToken.None);

            exitCode3.Should().Be(0);
            helix3.Resubmissions.Should().ContainSingle();
            helix3.Resubmissions[0].OriginalJob.Should().Be("helix-b");
            helix3.Resubmissions[0].FailedItems.Should().BeEquivalentTo(["b-fail"]);
            azdo3.UploadedJobNames.Should().BeEquivalentTo(["helix-b", "helix-a-resub", "helix-b-resub"]);
        }

        /// <summary>
        /// Failed work items that already exist when the monitor starts are resubmitted even on
        /// the monitor's first attempt. The original failed results are still uploaded normally.
        /// </summary>
        [Fact]
        public async Task Attempt1_DoesNotResubmitFailedWorkItemsFoundOnEntry_ExitOne()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-ok"], failed: ["wi-fail"]),
                });

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            helix.Resubmissions.Should().BeEmpty();
            azdo.UploadedJobNames.Should().ContainSingle(); // only original
        }

        /// <summary>
        /// A failed AzDO submitter is ignored only when the Helix job that is being retried is
        /// durably linked to it through System.JobName. Retrying unrelated Helix work must not mask
        /// the pipeline job failure.
        /// </summary>
        [Fact]
        public async Task FailedAzdoSubmitter_WithoutSystemJobNameLink_IsNotIgnored()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("A", "completed", "failed"));

            helix.AddResponse(
                jobs: [HelixJob("helix-a", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-a"] = PassFail(failed: ["a-fail"]),
                });
            helix.ConfigureResubmission("helix-a", "helix-a-resub");

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            helix.Resubmissions.Should().BeEmpty();
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-a"]);
        }

        /// <summary>
        /// A failed AzDO submitter is ignored only after a replacement Helix job is actually
        /// created. If resubmission returns null, the original AzDO failure still fails the monitor.
        /// </summary>
        [Fact]
        public async Task ResubmitReturnsNull_FailedSubmitterIsNotIgnored()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("A", "completed", "failed", attempt: 1));

            helix.AddResponse(
                jobs: [HelixJob("helix-a", "finished", stageName: "Test",
                    submitterJobName: "A", stageAttempt: "1", jobAttempt: "1")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-a"] = PassFail(failed: ["a-fail"]),
                });
            helix.ConfigureNullResubmission("helix-a");

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix, NoDelay);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            helix.Resubmissions.Should().ContainSingle();
            helix.Resubmissions[0].OriginalJob.Should().Be("helix-a");
            helix.Resubmissions[0].NewJob.Should().BeNull();
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-a"]);
        }

        /// <summary>
        /// A latest running or waiting incarnation suppresses retry even after multiple completed
        /// failed ancestors. Older failed attempts are not resubmitted while a newer incarnation is
        /// still active.
        /// </summary>
        [Fact]
        public async Task LatestRunningAfterMultipleCompletedFailures_DoesNotResubmitOlderFailures()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished"),
                    HelixJob("helix-linux-r1", "finished", previousHelixJobName: "helix-linux"),
                    HelixJob("helix-linux-r2", "waiting", previousHelixJobName: "helix-linux-r1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["wi-fail"]),
                    ["helix-linux-r1"] = PassFail(failed: ["wi-fail"]),
                });
            helix.ConfigureResubmission("helix-linux-r1", "helix-linux-r3");

            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished"),
                    HelixJob("helix-linux-r1", "finished", previousHelixJobName: "helix-linux"),
                    HelixJob("helix-linux-r2", "finished", previousHelixJobName: "helix-linux-r1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["wi-fail"]),
                    ["helix-linux-r1"] = PassFail(failed: ["wi-fail"]),
                    ["helix-linux-r2"] = PassFail(passed: ["wi-fail"]),
                });

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            helix.Resubmissions.Should().BeEmpty();
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux", "helix-linux-r1", "helix-linux-r2"]);
        }

        /// <summary>
        /// Canceled non-monitor AzDO jobs fail the monitor even when no Helix work is present.
        /// </summary>
        [Fact]
        public async Task CanceledAzdoJob_FailsMonitor()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "canceled"));
            helix.AddResponse(jobs: []);

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            azdo.UploadedJobNames.Should().BeEmpty();
        }

        /// <summary>
        /// The same completed Helix job may appear in multiple polling snapshots. It should only
        /// create one AzDO test run and one upload in a single monitor invocation.
        /// </summary>
        [Fact]
        public async Task SameCompletedHelixJobSeenAcrossPolls_UploadsOnce()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "inProgress"));
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-pass"]),
                });
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-pass"]),
                });

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux"]);
            azdo.CreatedTestRuns.Should().ContainSingle();
            azdo.CompletedTestRunIds.Should().ContainSingle();
        }

        /// <summary>
        /// Uploaded/processed state controls only duplicate result upload. A previously uploaded
        /// failed Helix job still contributes to retry and pass/fail on a later monitor entry.
        /// </summary>
        [Fact]
        public async Task PreviouslyProcessedFailedJob_StillContributesToRetryAndPassFail()
        {
            var azdo = new FakeAzureDevOpsService();
            azdo.WithPreviouslyProcessedJob("helix-linux");
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1));
            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1));

            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished", stageName: "Test",
                    submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["wi-fail"]),
                });
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub");
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-linux-resub", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", previousHelixJobName: "helix-linux",
                        stageAttempt: "2", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux-resub"] = PassFail(passed: ["wi-fail"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix, NoDelay);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            helix.Resubmissions.Should().ContainSingle();
            helix.Resubmissions[0].OriginalJob.Should().Be("helix-linux");
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux-resub"]);
        }

        /// <summary>
        /// A previously-uploaded failed Helix job that cannot be resubmitted must still have its
        /// work-item outcomes reconciled so it contributes to a non-zero final exit code. The
        /// processed/uploaded state only suppresses the duplicate upload and the noisy
        /// completion/console-link logs — it must never omit the job from pass/fail.
        /// </summary>
        [Fact]
        public async Task PreviouslyProcessedFailedJob_WithoutResubmission_StillFailsBuild()
        {
            var azdo = new FakeAzureDevOpsService();
            azdo.WithPreviouslyProcessedJob("helix-linux");
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1));
            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1));

            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished", stageName: "Test",
                    submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["wi-fail"]),
                });
            // The failed work item cannot be resubmitted (e.g. resubmission limit reached).
            helix.ConfigureNullResubmission("helix-linux");

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix, NoDelay);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            // The resubmission was attempted but produced no new job, so nothing was retried.
            helix.Resubmissions.Should().ContainSingle()
                .Which.NewJob.Should().BeNull();
            // The job was already uploaded by the prior attempt, so it is not re-uploaded.
            azdo.UploadedJobNames.Should().BeEmpty();
        }

        /// <summary>
        /// On a second monitor attempt within the same build, jobs that already have an
        /// uploaded test run (tracked via ProcessedHelixJobs, seeded from AzDO test-run
        /// tags) must not be re-logged as completed and must not have their failed
        /// work-item console links re-emitted, nor must their results be re-uploaded.
        /// </summary>
        [Fact]
        public async Task PreviouslyProcessedJob_DoesNotReLogCompletionOrReUpload()
        {
            var azdo = new FakeAzureDevOpsService();
            azdo.WithPreviouslyProcessedJob("helix-linux");
            azdo.WithPreviouslyProcessedJob("helix-windows");
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "completed", "succeeded"));

            // Both jobs are completed and present in the Helix snapshot. helix-linux had a
            // failed work item in the prior attempt; helix-windows passed. Neither should be
            // resubmitted in this attempt (the failure was already resolved upstream — for
            // this test we just need them to remain terminal in their previously-processed
            // state, so we configure no resubmission).
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished"),
                    HelixJob("helix-windows", "finished"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-linux-pass"]),
                    ["helix-windows"] = PassFail(passed: ["wi-win-pass"]),
                });

            var runner = CreateRunner(azdo, helix, logger: logger);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);

            // Neither job is re-uploaded (no CreateTestRun/UploadTestResults/CompleteTestRun
            // calls for these helix job names).
            azdo.UploadedJobNames.Should().BeEmpty();
            azdo.CreatedTestRuns.Should().BeEmpty();

            // Neither job triggers the "Job X completed. Processing test results..." log
            // line nor the "✅ Job 'X' succeeded" / "❌ Job 'X' failed" completion line that
            // ReconcileCompletedJobAsync emits for jobs it actually processes.
            logger.Messages.Should().NotContain(message =>
                message.Contains("helix-linux", StringComparison.Ordinal)
                && message.Contains("Processing test results", StringComparison.Ordinal));
            logger.Messages.Should().NotContain(message =>
                message.Contains("helix-windows", StringComparison.Ordinal)
                && message.Contains("Processing test results", StringComparison.Ordinal));
            logger.Messages.Should().NotContain(message =>
                message.Contains("helix-linux", StringComparison.Ordinal)
                && (message.Contains("succeeded", StringComparison.Ordinal)
                    || message.Contains("failed (", StringComparison.Ordinal)));
            logger.Messages.Should().NotContain(message =>
                message.Contains("helix-windows", StringComparison.Ordinal)
                && (message.Contains("succeeded", StringComparison.Ordinal)
                    || message.Contains("failed (", StringComparison.Ordinal)));
        }

        /// <summary>
        /// If the beginning of a lineage was already uploaded, upload skips it but still publishes
        /// every unprocessed completed successor in old-to-new order.
        /// </summary>
        [Fact]
        public async Task PartiallyProcessedLineage_UploadsOnlyUnprocessedOldToNew()
        {
            var azdo = new FakeAzureDevOpsService();
            azdo.WithPreviouslyProcessedJob("helix-linux");
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux-r2", "finished", previousHelixJobName: "helix-linux-r1"),
                    HelixJob("helix-linux", "finished"),
                    HelixJob("helix-linux-r1", "finished", previousHelixJobName: "helix-linux"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["wi-fail"]),
                    ["helix-linux-r1"] = PassFail(failed: ["wi-fail"]),
                    ["helix-linux-r2"] = PassFail(passed: ["wi-fail"]),
                });

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            helix.Resubmissions.Should().BeEmpty();
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux-r1", "helix-linux-r2"]);
        }

        /// <summary>
        /// Stage scope is applied before lineage supersession. A retry from another stage must not
        /// suppress a failed in-stage job from being retried by this monitor.
        /// </summary>
        [Fact]
        public async Task StageScopedMonitor_LineageCrossesStageBoundary_IgnoresOutOfStageIncarnation()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress"),
                MonitorJob(parentId: "stage-test"),
                PipelineJob("Test Linux", "completed", "succeeded", parentId: "stage-test"),
                StageRecord("Build", "stage-build", "inProgress"),
                PipelineJob("Build Windows", "inProgress", parentId: "stage-build"));
            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress"),
                MonitorJob(parentId: "stage-test"),
                PipelineJob("Test Linux", "completed", "succeeded", parentId: "stage-test"),
                StageRecord("Build", "stage-build", "inProgress"),
                PipelineJob("Build Windows", "inProgress", parentId: "stage-build"));

            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-test", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-build-retry", "running", stageName: "Build", previousHelixJobName: "helix-test"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-test"] = PassFail(failed: ["test-fail"]),
                });
            helix.ConfigureResubmission("helix-test", "helix-test-resub");
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-test", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-test-resub", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", previousHelixJobName: "helix-test",
                        stageAttempt: "2", jobAttempt: "1"),
                    HelixJob("helix-build-retry", "running", stageName: "Build", previousHelixJobName: "helix-test"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-test"] = PassFail(failed: ["test-fail"]),
                    ["helix-test-resub"] = PassFail(passed: ["test-fail"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageName = "Test";
            options.StageAttempt = "2";
            options.JobAttempt = "2";
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix, NoDelay);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            helix.Resubmissions.Should().ContainSingle();
            helix.Resubmissions[0].OriginalJob.Should().Be("helix-test");
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-test", "helix-test-resub"]);
        }

        /// <summary>
        /// Helix status "failed" is terminal for monitor purposes and should be processed like a
        /// completed job.
        /// </summary>
        [Fact]
        public async Task HelixJobStatusFailed_IsTreatedAsCompleted()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "failed")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-pass"]),
                });

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux"]);
        }

        /// <summary>
        /// A work item with exit code 0 but a non-Finished state is still failed and should be
        /// resubmitted on monitor entry.
        /// </summary>
        [Fact]
        public async Task FinishedHelixJob_WithNonFinishedWorkItemState_IsFailure()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1));
            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1));

            helix.AddResponse(jobs: [HelixJob("helix-linux", "finished", stageName: "Test",
                submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1")]);
            helix.WithWorkItems(
                "helix-linux",
                [new WorkItemSummary("helix-linux/wi-timeout", "helix-linux", "wi-timeout", "TimedOut") { ExitCode = 0 }]);
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub");
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-linux-resub", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", previousHelixJobName: "helix-linux",
                        stageAttempt: "2", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux-resub"] = PassFail(passed: ["wi-timeout"]),
                });

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix, NoDelay);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            helix.Resubmissions.Should().ContainSingle();
            helix.Resubmissions[0].FailedItems.Should().BeEquivalentTo(["wi-timeout"]);
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux", "helix-linux-resub"]);
        }

        [Fact]
        public async Task CompletedHelixJob_LogsFailedWorkItemConsoleLinks()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));
            helix.AddResponse(jobs: [HelixJob("helix-linux", "finished")]);
            helix.WithWorkItems(
                "helix-linux",
                [
                    new WorkItemSummary("details/wi-pass", "helix-linux", "wi-pass", "Finished")
                    {
                        ConsoleOutputUri = "https://helix.example/wi-pass/console",
                        ExitCode = 0,
                    },
                    new WorkItemSummary("details/wi-fail", "helix-linux", "wi-fail", "Finished")
                    {
                        ConsoleOutputUri = "https://helix.example/wi-fail/console",
                        ExitCode = 1,
                    },
                ]);

            var runner = CreateRunner(azdo, helix, logger: logger);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            logger.Messages.Should().NotContain(message =>
                message.Contains("wi-pass", StringComparison.Ordinal)
                && message.Contains("https://helix.example/wi-pass/console", StringComparison.Ordinal));
            logger.Messages.Should().Contain(message =>
                message.Contains($"Work item 'wi-fail' in job 'helix-linux' failed (Finished, exit code 1).{Environment.NewLine}Console: https://helix.example/wi-fail/console", StringComparison.Ordinal));
            logger.Messages.Should().Contain(message =>
                message.Contains("##vso[task.logissue type=warning]Work item 'wi-fail' in job 'helix-linux' failed", StringComparison.Ordinal));
            logger.Messages.Should().Contain(message =>
                message.Contains("Failed work item information:", StringComparison.Ordinal)
                && message.Contains("Test results: https://dev.azure.com/dnceng/public/_build/results?buildId=123&view=ms.vss-test-web.build-test-results-tab", StringComparison.Ordinal)
                && message.Contains("└─ wi-fail (Job: helix-linux) (Finished, exit code 1)", StringComparison.Ordinal)
                && message.Contains("└─ Console: https://helix.example/wi-fail/console", StringComparison.Ordinal));
            logger.Messages.Should().Contain(message =>
                message.Contains("##vso[task.logissue type=error]Failed work item information:", StringComparison.Ordinal));
            logger.Messages.Should().NotContain(message =>
                message.Contains("Helix job: helix-linux", StringComparison.Ordinal));
        }

        [Fact]
        public async Task NonCallerOperationCanceledException_Propagates()
        {
            var azdo = new FakeAzureDevOpsService()
                .FailNextTimeline(new OperationCanceledException("Injected service timeout."));
            var runner = CreateRunner(azdo, new FakeHelixService());

            Func<Task> action = () => runner.RunAsync(CancellationToken.None);

            await action.Should().ThrowAsync<OperationCanceledException>()
                .WithMessage("Injected service timeout.");
        }

        [Fact]
        public async Task LoopStatus_LogsAggregateHelixJobWorkItemCounts()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();
            using var cts = new CancellationTokenSource();

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "inProgress"));
            helix.AddResponse(jobs: [HelixJob("helix-linux", "running")]);
            helix.WithWorkItems(
                "helix-linux",
                [
                    new WorkItemSummary("details/wi-1", "helix-linux", "wi-1", "Running"),
                    new WorkItemSummary("details/wi-pass", "helix-linux", "wi-pass", "Finished")
                    {
                        ConsoleOutputUri = "https://helix.example/wi-pass/console",
                        ExitCode = 0,
                    },
                    new WorkItemSummary("details/wi-2", "helix-linux", "wi-2", "Finished")
                    {
                        ConsoleOutputUri = "https://helix.example/wi-2/console",
                        ExitCode = 1,
                    },
                ]);

            var runner = new JobMonitorRunner(DefaultOptions(), logger, azdo, helix,
                (_, _) =>
                {
                    cts.Cancel();
                    return Task.CompletedTask;
                });

            int exitCode = await runner.RunAsync(cts.Token);

            exitCode.Should().Be(1);
            logger.Messages.Should().Contain(message =>
                message.Contains("0 processed / 0 completed / 1 running / 0 waiting jobs", StringComparison.Ordinal));
            logger.Messages.Should().Contain(message =>
                message.Contains("0 processed / 0 completed / 3 running / 0 waiting work items", StringComparison.Ordinal));
            logger.Messages.Should().NotContain(message =>
                message.Contains("Helix job details:", StringComparison.Ordinal));
            logger.Messages.Should().Contain(message =>
                message.Contains($"Work item 'wi-2' in job 'helix-linux' failed (Finished, exit code 1).{Environment.NewLine}Console: https://helix.example/wi-2/console", StringComparison.Ordinal));
        }

        [Fact]
        public async Task LoopStatus_VerboseLogsBoundedPipelineDiagnostics()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();
            using var cts = new CancellationTokenSource();

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "inProgress"));
            helix.AddResponse(jobs: [HelixJob("helix-linux", "running")]);
            helix.WithWorkItems(
                "helix-linux",
                [
                    ..Enumerable.Range(1, 12)
                        .Select(index => new WorkItemSummary(
                            $"details/wi-{index:00}",
                            "helix-linux",
                            $"wi-{index:00}",
                            "Running")),
                ]);

            JobMonitorOptions options = DefaultOptions();
            options.Verbose = true;
            var runner = new JobMonitorRunner(options, logger, azdo, helix,
                (_, _) =>
                {
                    cts.Cancel();
                    return Task.CompletedTask;
                });

            int exitCode = await runner.RunAsync(cts.Token);

            exitCode.Should().Be(1);
            logger.Messages.Should().Contain(message =>
                message.Contains("Upload pipeline:", StringComparison.Ordinal));
            logger.Messages.Should().NotContain(message =>
                message.Contains("wi-01", StringComparison.Ordinal)
                || message.Contains("wi-12", StringComparison.Ordinal));
        }

        /// <summary>
        /// When Helix work items are in the "Waiting" state, the summary should keep them in
        /// the "waiting" work item bucket while still reporting the parent job as "Running"
        /// because work items have already been assigned.
        /// </summary>
        [Fact]
        public async Task LoopStatus_WaitingWorkItems_CountedAsWaitingWhileJobIsRunning()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();
            using var cts = new CancellationTokenSource();

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "inProgress"));
            helix.AddResponse(jobs: [HelixJob("helix-linux", "running")]);
            helix.WithWorkItems(
                "helix-linux",
                [
                    new WorkItemSummary("details/wi-1", "helix-linux", "wi-1", "Waiting"),
                    new WorkItemSummary("details/wi-2", "helix-linux", "wi-2", "Waiting"),
                ]);

            JobMonitorOptions options = DefaultOptions();
            options.Verbose = true;
            var runner = new JobMonitorRunner(options, logger, azdo, helix,
                (_, _) =>
                {
                    cts.Cancel();
                    return Task.CompletedTask;
                });

            int exitCode = await runner.RunAsync(cts.Token);

            exitCode.Should().Be(1);
            logger.Messages.Should().Contain(message =>
                message.Contains("0 processed / 0 completed / 1 running / 0 waiting jobs", StringComparison.Ordinal));
            logger.Messages.Should().Contain(message =>
                message.Contains("0 processed / 0 completed / 0 running / 2 waiting work items", StringComparison.Ordinal));
            logger.Messages.Should().Contain(message =>
                message.Contains("Upload pipeline:", StringComparison.Ordinal));
        }

        /// <summary>
        /// A Helix work item passes by exit code (0) but the AzDO upload reports that its test
        /// results contained failures. <see cref="MonitorState.ObserveTestResults"/> should mark
        /// the work item as failed so the monitor exits 1.
        /// </summary>
        [Fact]
        public async Task HelixWorkItemPassesByExitCode_TestUploadReportsFailure_ExitOne()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            // Configure the fake upload to report AllPassed=false for this work item, simulating
            // an uploaded TRX that contained at least one failing test even though the work
            // item exited 0 on the Helix side.
            azdo.WithFailedTestResults("helix-linux", "workitem-1");

            // Poll 1: pipeline + helix still in flight (job "running" so the retry pass does
            // not act on it). Poll 2: everything completes and the upload runs.
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "inProgress"));
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(jobs: [HelixJob("helix-linux", "running")]);
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"]),
                },
                testResultsByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] =
                    [
                        new WorkItemTestResults("helix-linux", "workitem-1", ["results.trx"])
                    ],
                });

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(1);
            helix.Resubmissions.Should().BeEmpty();
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux"]);
            azdo.CompletedTestRunIds.Should().ContainSingle();
            IReadOnlyDictionary<string, IReadOnlySet<string>> failedTestWorkItems =
                await azdo.GetFailedTestWorkItemsAsync(CancellationToken.None);
            failedTestWorkItems["helix-linux"].Should().BeEquivalentTo(["workitem-1"]);
        }

        /// <summary>
        /// On entry the monitor finds a completed Helix job with two failed work items: one
        /// that passed by Helix exit code but has prior failed AzDO test results, and one that
        /// failed by Helix exit code AND also has prior failed AzDO test results. The retry
        /// pass must resubmit both. The work item that qualifies as a failure under both
        /// reasons must only be resubmitted once, not twice.
        /// </summary>
        [Fact]
        public async Task RetryPass_ResubmitsWorkItemThatPassedExitCodeButHasPriorFailedTests()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            // workitem-1 exit code is 0, but AzDO already has a failed test recorded for it
            // from a prior monitor invocation (a test-only failure).
            azdo.WithRecordedFailedTest("helix-linux", "workitem-1");
            // workitem-2 fails by Helix exit code AND also has a failed AzDO test recorded.
            // It must not be resubmitted twice (once per failure reason).
            azdo.WithRecordedFailedTest("helix-linux", "workitem-2");

            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1));
            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1));

            // Snapshot 1 (retry pass): only the original Helix job exists, finished, with
            // workitem-1 passing by exit code and workitem-2 failing by exit code. Without the
            // AzDO test-failure lookup the retry pass would only resubmit workitem-2.
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished", stageName: "Test",
                    submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"], failed: ["workitem-2"]),
                });

            // Snapshot 2 (next poll): the resub has finished too with both items passing.
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-linux-resub", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", previousHelixJobName: "helix-linux",
                        stageAttempt: "2", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"], failed: ["workitem-2"]),
                    ["helix-linux-resub"] = PassFail(passed: ["workitem-1", "workitem-2"]),
                });
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub");

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";
            var runner = new JobMonitorRunner(options, logger, azdo, helix, NoDelay);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            helix.Resubmissions.Should().ContainSingle();
            helix.Resubmissions[0].OriginalJob.Should().Be("helix-linux");
            // workitem-2 fails by both exit code and failed tests but is only resubmitted once.
            helix.Resubmissions[0].FailedItems.Should().OnlyHaveUniqueItems();
            helix.Resubmissions[0].FailedItems.Should().BeEquivalentTo(["workitem-1", "workitem-2"]);

            // Both the original and the resub uploaded results, and the resub's tests all
            // passed so the monitor exits 0.
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux", "helix-linux-resub"]);
            exitCode.Should().Be(0);

            // The retry log distinguishes the resubmission reason and counts each work item once.
            logger.Messages.Should().Contain(message =>
                message.Contains("Resubmitting 2 work item(s)", StringComparison.Ordinal)
                && message.Contains("1 by Helix exit code, 1 by failed AzDO tests", StringComparison.Ordinal)
                && message.Contains("workitem-2 (Helix exit code 1)", StringComparison.Ordinal)
                && message.Contains("workitem-1 (exit code 0, failed AzDO tests)", StringComparison.Ordinal));
        }

        /// <summary>
        /// One completed Helix job has two failed work items for different reasons: one with a
        /// non-zero Helix exit code and another that exited 0 but whose AzDO test results were
        /// previously recorded as failed. The retry-pass log should distinguish each reason.
        /// </summary>
        [Fact]
        public async Task RetryPass_LogDistinguishesExitCodeAndTestFailureReasons()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            var logger = new RecordingLogger();

            azdo.WithRecordedFailedTest("helix-linux", "wi-test-only");

            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1));
            azdo.AddTimelineResponse(
                MonitorJob(attempt: 2, previousAttempts: [PreviousAttempt(1)]),
                PipelineJob("Test Linux", "completed", "succeeded", attempt: 1));

            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished", stageName: "Test",
                    submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-test-only"], failed: ["wi-exit-fail"]),
                });

            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", stageAttempt: "1", jobAttempt: "1"),
                    HelixJob("helix-linux-resub", "finished", stageName: "Test",
                        submitterJobName: "Test Linux", previousHelixJobName: "helix-linux",
                        stageAttempt: "2", jobAttempt: "1"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-test-only"], failed: ["wi-exit-fail"]),
                    ["helix-linux-resub"] = PassFail(passed: ["wi-test-only", "wi-exit-fail"]),
                });
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub");

            JobMonitorOptions options = DefaultOptions();
            options.StageAttempt = "2";
            options.JobAttempt = "2";
            var runner = new JobMonitorRunner(options, logger, azdo, helix, NoDelay);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            helix.Resubmissions.Should().ContainSingle();
            helix.Resubmissions[0].FailedItems.Should().BeEquivalentTo(["wi-exit-fail", "wi-test-only"]);

            logger.Messages.Should().Contain(message =>
                message.Contains("Resubmitting 2 work item(s)", StringComparison.Ordinal)
                && message.Contains("1 by Helix exit code, 1 by failed AzDO tests", StringComparison.Ordinal)
                && message.Contains("wi-exit-fail (Helix exit code 1)", StringComparison.Ordinal)
                && message.Contains("wi-test-only (exit code 0, failed AzDO tests)", StringComparison.Ordinal));
        }

        /// <summary>
        /// When <see cref="JobMonitorOptions.FailWorkItemsWithFailedTests"/> is false, neither
        /// the retry pass nor <see cref="MonitorState.ObserveTestResults"/> should promote
        /// test failures to work-item failures. The monitor exits 0 even though the upload
        /// reports failed tests and AzDO has prior failed tests recorded for the same item.
        /// </summary>
        [Fact]
        public async Task FailOnFailedTestsDisabled_TestFailuresIgnoredForOutcomeAndRetry()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            azdo.WithRecordedFailedTest("helix-linux", "workitem-1");
            azdo.WithFailedTestResults("helix-linux", "workitem-1");

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"]),
                },
                testResultsByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] =
                    [
                        new WorkItemTestResults("helix-linux", "workitem-1", ["results.trx"])
                    ],
                });

            JobMonitorOptions options = DefaultOptions();
            options.FailWorkItemsWithFailedTests = false;
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix, NoDelay);

            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            helix.Resubmissions.Should().BeEmpty();
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux"]);
            IReadOnlyDictionary<string, IReadOnlySet<string>> failedTestWorkItems =
                await azdo.GetFailedTestWorkItemsAsync(CancellationToken.None);
            failedTestWorkItems["helix-linux"].Should().BeEquivalentTo(["workitem-1"]);
        }

        // -----------------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------------

        private static JobMonitorOptions DefaultOptions() => new()
        {
            BuildId = "123",
            CollectionUri = "https://dev.azure.com/dnceng/",
            JobMonitorName = DefaultMonitorName,
            MaximumWaitMinutes = 1,
            PollingIntervalSeconds = 0,
            Organization = "dotnet",
            RepositoryName = "arcade",
            BuildReason = "PullRequest",
            SourceBranch = "refs/pull/99999/merge",
            StageName = "Test",
            StageAttempt = "1",
            JobAttempt = "1",
            SystemAccessToken = "token",
            TeamProject = "public",
            WorkingDirectory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "job-monitor-test"),
        };

        private static readonly Func<TimeSpan, CancellationToken, Task> NoDelay = (_, _) => Task.CompletedTask;

        private static async Task WaitForAsync(Func<bool> condition)
        {
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            while (!condition())
            {
                await Task.Delay(10, timeout.Token);
            }
        }

        private static WorkItemSummary WaitingItem(string jobName, string workItemName)
            => new($"{jobName}/{workItemName}", jobName, workItemName, "Waiting");

        /// <summary>
        /// Runs a single monitor invocation for the given stage attempt that is expected to time
        /// out (a work stream remains unfinished). The retry pass runs on entry and records its
        /// resubmissions before the first poll; the invocation is then cancelled after that poll
        /// to simulate the monitor's timeout. Returns the Helix fake so the caller can assert the
        /// resubmissions the retry pass made.
        /// </summary>
        private static async Task<FakeHelixService> RunAttemptToTimeoutAsync(
            string stageAttempt,
            HelixJobInfo[] entryLeaves,
            Dictionary<string, HelixJobPassFail> passFailByJob,
            (string JobName, WorkItemSummary[] Items)[] waitingWorkItems,
            (string From, string To)[] resubmissions)
        {
            var azdo = new FakeAzureDevOpsService();
            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress", attempt: int.Parse(stageAttempt)),
                MonitorJob(attempt: int.Parse(stageAttempt), parentId: "stage-test"),
                PipelineJob("Test_A", "completed", "succeeded", attempt: 1, parentId: "stage-test"),
                PipelineJob("Test_B", "completed", "succeeded", attempt: 1, parentId: "stage-test"),
                PipelineJob("Test_C", "completed", "succeeded", attempt: 1, parentId: "stage-test"));

            var helix = new FakeHelixService();
            helix.AddResponse(jobs: entryLeaves, passFailByJob: passFailByJob);
            foreach ((string jobName, WorkItemSummary[] items) in waitingWorkItems)
            {
                helix.WithWorkItems(jobName, items);
            }
            foreach ((string from, string to) in resubmissions)
            {
                helix.ConfigureResubmission(from, to);
            }

            var options = DefaultOptions();
            options.StageAttempt = stageAttempt;
            options.JobAttempt = stageAttempt;
            options.JobAttempt = stageAttempt;

            using var cts = new CancellationTokenSource();
            var runner = new JobMonitorRunner(options, NullLogger.Instance, azdo, helix,
                (_, _) => { cts.Cancel(); return Task.CompletedTask; });

            int exitCode = await runner.RunAsync(cts.Token);
            exitCode.Should().Be(1); // still-unfinished work stream → timeout
            return helix;
        }

        private static JobMonitorRunner CreateRunner(
            FakeAzureDevOpsService azdo,
            FakeHelixService helix,
            string stageName = "Test",
            ILogger logger = null)
        {
            var options = DefaultOptions();
            options.StageName = stageName;

            return new(options, logger ?? NullLogger.Instance, azdo, helix, NoDelay);
        }

        private sealed class RecordingLogger : ILogger
        {
            private readonly Action<string> _onLog;

            public RecordingLogger(Action<string> onLog = null)
            {
                _onLog = onLog;
            }

            public ConcurrentQueue<string> Messages { get; } = [];

            public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception exception,
                Func<TState, Exception, string> formatter)
            {
                string message = formatter(state, exception) ?? string.Empty;
                Messages.Enqueue(message);
                _onLog?.Invoke(message);
            }

            private sealed class NullScope : IDisposable
            {
                public static NullScope Instance { get; } = new();

                public void Dispose()
                {
                }
            }
        }
    }
}
