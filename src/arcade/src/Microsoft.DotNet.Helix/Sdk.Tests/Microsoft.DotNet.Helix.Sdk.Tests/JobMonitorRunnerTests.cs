// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
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
        /// <summary>
        /// Single pipeline job goes from queued → in progress → completed (succeeded).
        /// No Helix jobs are ever submitted.
        /// The monitor should poll 3 times and exit with code 0.
        /// </summary>
        [Fact]
        public async Task SinglePipelineJobSucceeds_NoHelixJobs_ExitZero()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

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

            var runner = CreateRunner(azdo, helix);
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            azdo.TimelineCallCount.Should().Be(3);
            azdo.CreatedTestRuns.Should().BeEmpty();
            azdo.UploadedJobNames.Should().BeEmpty();
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
        /// The monitor should exit with code 0.
        /// </summary>
        [Fact]
        public async Task TwoPipelineJobs_AllPassed_NoHelixJobs_ExitZero()
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

            exitCode.Should().Be(0);
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
        /// The monitor should exit 0 on the retry.
        /// </summary>
        [Fact]
        public async Task RetryAfterFailure_RetriedJobPasses_ExitZero()
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

            exitCode2.Should().Be(0);
            azdo2.TimelineCallCount.Should().Be(3);
            azdo2.CreatedTestRuns.Should().BeEmpty();
            azdo2.UploadedJobNames.Should().BeEmpty();
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
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));

            // Polls 4-6: build job still completed (monitor waiting for Helix)
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));
            azdo.AddTimelineResponse(
                MonitorJob(),
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
                jobs: [HelixJob("helix-linux", "finished")],
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
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux"]);
            azdo.CompletedTestRunIds.Should().ContainSingle();
            logger.Messages.Should().Contain(message =>
                message.Contains("2 test results for job 'helix-linux' processed.", StringComparison.Ordinal));
        }

        [Fact]
        public async Task PassedHelixWork_UploadFailsOnce_RetriesAndProcessesResults()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();
            int delayCount = 0;

            azdo.FailNextUpload();

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

            var runner = new JobMonitorRunner(DefaultOptions(), NullLogger.Instance, azdo, helix,
                (_, _) =>
                {
                    delayCount++;
                    return Task.CompletedTask;
                });
            int exitCode = await runner.RunAsync(CancellationToken.None);

            exitCode.Should().Be(0);
            azdo.UploadTestResultsCallCount.Should().Be(2);
            delayCount.Should().Be(1);
            // The first attempt creates a run then fails mid-upload, orphaning it (untagged,
            // never completed). The retry creates a second run that uploads and completes.
            azdo.CreatedTestRuns.Should().HaveCount(2);
            azdo.CompletedTestRunIds.Should().ContainSingle();
            azdo.UploadedJobNames.Should().BeEquivalentTo(["helix-linux"]);
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
        public async Task StageScopedMonitor_IgnoresJobsOutsideStage_ExitZero()
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
                StageRecord("Test", "stage-test", "inProgress"),
                MonitorJob(parentId: "stage-test"),
                PipelineJob("Test Linux", "completed", "succeeded", parentId: "stage-test"),
                StageRecord("Build", "stage-build", "inProgress"),
                PipelineJob("Build Windows", "inProgress", parentId: "stage-build"));

            // No Helix jobs
            helix.AddResponse(jobs: []);
            helix.AddResponse(jobs: []);

            var runner = CreateRunner(azdo, helix, stageName: "Test");
            int exitCode = await runner.RunAsync(CancellationToken.None);

            // Monitor only watches Test stage — Test Linux passed, no Helix → exit 0
            // Build Windows being in progress doesn't block the monitor.
            exitCode.Should().Be(0);
            azdo.TimelineCallCount.Should().Be(2);
            azdo.UploadedJobNames.Should().BeEmpty();
        }

        /// <summary>
        /// Stage-scoped monitoring. A job outside the monitor's stage submits Helix work.
        /// The monitor should ignore that Helix job entirely.
        /// </summary>
        [Fact]
        public async Task StageScopedMonitor_IgnoresHelixJobsFromOtherStage_ExitZero()
        {
            var azdo = new FakeAzureDevOpsService();
            var helix = new FakeHelixService();

            // Test stage job completes quickly with no Helix submissions.
            // Build stage job submits a Helix job, but that's not the monitor's concern.
            azdo.AddTimelineResponse(
                StageRecord("Test", "stage-test", "inProgress"),
                MonitorJob(parentId: "stage-test"),
                PipelineJob("Test Linux", "completed", "succeeded", parentId: "stage-test"),
                StageRecord("Build", "stage-build", "inProgress"),
                PipelineJob("Build Windows", "inProgress", parentId: "stage-build"));

            // Helix returns a job from the Build stage — monitor should ignore it
            helix.AddResponse(
                jobs: [HelixJob("helix-build-windows", "running", stageName: "Build")]);

            var runner = CreateRunner(azdo, helix, stageName: "Test");
            int exitCode = await runner.RunAsync(CancellationToken.None);

            // Test stage is done, no Helix jobs in Test stage → exit 0
            exitCode.Should().Be(0);
            azdo.TimelineCallCount.Should().Be(1);
            azdo.UploadedJobNames.Should().BeEmpty();
            azdo.CreatedTestRuns.Should().BeEmpty();
        }

        /// <summary>
        /// Stage-scoped monitoring also applies to entry-time retry. A failed Helix job from
        /// another stage must not be resubmitted or uploaded by this monitor.
        /// </summary>
        [Fact]
        public async Task StageScopedMonitor_DoesNotResubmitFailedHelixJobsOutsideStage_ExitZero()
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

            exitCode.Should().Be(0);
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
                    HelixJob("helix-test-linux", "finished", stageName: "Test", submitterJobName: "Test Linux"),
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
                    HelixJob("helix-test-linux", "finished", stageName: "Test", submitterJobName: "Test Linux"),
                    HelixJob("helix-test-linux-resub", "finished", stageName: "Test", submitterJobName: "Test Linux", previousHelixJobName: "helix-test-linux"),
                    HelixJob("helix-build-windows", "finished", stageName: "Build", submitterJobName: "Build Windows"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-test-linux"] = PassFail(failed: ["test-fail"]),
                    ["helix-test-linux-resub"] = PassFail(passed: ["test-fail"]),
                    ["helix-build-windows"] = PassFail(failed: ["build-fail"]),
                });

            var runner2 = CreateRunner(azdo2, helix2, stageName: "Test");
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
                    StageRecord("Test", "stage-test", "inProgress"),
                    MonitorJob(attempt: attempt, previousAttempts: previousAttempts, parentId: "stage-test"),
                    PipelineJob("Test Linux", "completed", "succeeded", attempt: attempt, previousAttempts: previousAttempts, parentId: "stage-test"),
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
                jobs: [HelixJob("helix-linux", "finished", stageName: "Test", submitterJobName: "Build_Debug")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["linux-fail"]),
                });
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub");
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished", stageName: "Test", submitterJobName: "Build_Debug"),
                    HelixJob("helix-linux-resub", "finished", stageName: "Test", submitterJobName: "Build_Debug", previousHelixJobName: "helix-linux"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["linux-fail"]),
                    ["helix-linux-resub"] = PassFail(failed: ["linux-fail"]),
                });

            int delayCount = 0;
            var options = DefaultOptions();
            options.StageName = "Test";
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
                    StageRecord("Test", "stage-test", "inProgress"),
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
                    PipelineJob("Linux Build_Debug", "completed", "succeeded", parentId: "stage-test"));
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
                        Task completed = await Task.WhenAny(azdo1.UploadCompleted.Task, Task.Delay(TimeSpan.FromSeconds(5)));
                        Assert.Same(azdo1.UploadCompleted.Task, completed);
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
                    Task completed = await Task.WhenAny(azdo.UploadCompleted.Task, Task.Delay(TimeSpan.FromSeconds(5)));
                    completed.Should().BeSameAs(azdo.UploadCompleted.Task);
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
            var runner = new JobMonitorRunner(DefaultOptions(), NullLogger.Instance, azdo, helix,
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
            var logger = new RecordingLogger();

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
                        // Wait until helix-good's results have actually been uploaded before
                        // cancelling, so the monitor has had a chance to record its terminal
                        // state.
                        Task completed = await Task.WhenAny(azdo.UploadCompleted.Task, Task.Delay(TimeSpan.FromSeconds(5)));
                        completed.Should().BeSameAs(azdo.UploadCompleted.Task);
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
                        Task completed = await Task.WhenAny(azdo1.UploadCompleted.Task, Task.Delay(TimeSpan.FromSeconds(5)));
                        Assert.Same(azdo1.UploadCompleted.Task, completed);
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
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));
            // Extra polls while resubmitted job runs
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));

            // Poll 1: original Helix job finished with 1 pass + 2 failures
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-ok"], failed: ["wi-fail-1", "wi-fail-2"]),
                });
            // Configure: resubmission of helix-linux creates "helix-linux-resub"
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub");

            // Poll 2: resubmitted job appears, running
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished"), HelixJob("helix-linux-resub", "running", previousHelixJobName: "helix-linux")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-ok"], failed: ["wi-fail-1", "wi-fail-2"]),
                });

            // Poll 3: resubmitted job finished — both items now pass
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished"), HelixJob("helix-linux-resub", "finished", previousHelixJobName: "helix-linux")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-ok"], failed: ["wi-fail-1", "wi-fail-2"]),
                    ["helix-linux-resub"] = PassFail(passed: ["wi-fail-1", "wi-fail-2"]),
                });

            var runner = CreateRunner(azdo, helix);
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
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"));

            // Poll 1: original job has 1 failure
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-ok"], failed: ["wi-fail"]),
                });
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub");

            // Poll 2: resubmission finished but STILL fails
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished"), HelixJob("helix-linux-resub", "finished", previousHelixJobName: "helix-linux")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-ok"], failed: ["wi-fail"]),
                    ["helix-linux-resub"] = PassFail(failed: ["wi-fail"]),
                });

            var runner = CreateRunner(azdo, helix);
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
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "completed", "succeeded"));
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "completed", "succeeded"));
            azdo.AddTimelineResponse(
                MonitorJob(),
                PipelineJob("Test Linux", "completed", "succeeded"),
                PipelineJob("Test Windows", "completed", "succeeded"));

            // Poll 1: both original jobs finished with mixed results
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished"), HelixJob("helix-windows", "finished")],
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
                    HelixJob("helix-linux", "finished"), HelixJob("helix-windows", "finished"),
                    HelixJob("helix-linux-resub", "finished", previousHelixJobName: "helix-linux"),
                    HelixJob("helix-windows-resub", "running", previousHelixJobName: "helix-windows"),
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
                    HelixJob("helix-linux", "finished"), HelixJob("helix-windows", "finished"),
                    HelixJob("helix-linux-resub", "finished", previousHelixJobName: "helix-linux"),
                    HelixJob("helix-windows-resub", "finished", previousHelixJobName: "helix-windows"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["linux-ok-1", "linux-ok-2"], failed: ["linux-fail"]),
                    ["helix-windows"] = PassFail(passed: ["win-ok"], failed: ["win-fail-1", "win-fail-2"]),
                    ["helix-linux-resub"] = PassFail(passed: ["linux-fail"]),
                    ["helix-windows-resub"] = PassFail(passed: ["win-fail-1", "win-fail-2"]),
                });

            var runner = CreateRunner(azdo, helix);
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
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["wi-fail"]),
                });
            helix2.ConfigureResubmission("helix-linux", "helix-linux-resub");
            helix2.AddResponse(
                jobs: [HelixJob("helix-linux", "finished"), HelixJob("helix-linux-resub", "finished", previousHelixJobName: "helix-linux")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux-resub"] = PassFail(passed: ["wi-fail"]),
                });

            var runner2 = CreateRunner(azdo2, helix2);
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

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished"),
                    HelixJob("helix-linux-resub", "finished", previousHelixJobName: "helix-linux"),
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
                    HelixJob("helix-linux", "finished"),
                    HelixJob("helix-linux-resub", "finished", previousHelixJobName: "helix-linux"),
                    HelixJob("helix-linux-resub-2", "finished", previousHelixJobName: "helix-linux-resub"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["wi-1", "wi-2"]),
                    ["helix-linux-resub"] = PassFail(passed: ["wi-1"], failed: ["wi-2"]),
                    ["helix-linux-resub-2"] = PassFail(passed: ["wi-2"]),
                });

            var runner = CreateRunner(azdo, helix);
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
                jobs: [HelixJob("helix-a", "finished", submitterJobName: "A"), HelixJob("helix-b", "finished", submitterJobName: "B")],
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
                    HelixJob("helix-a", "finished", submitterJobName: "A"),
                    HelixJob("helix-b", "finished", submitterJobName: "B"),
                    HelixJob("helix-a-resub", "finished", submitterJobName: "A", previousHelixJobName: "helix-a"),
                    HelixJob("helix-b-resub", "finished", submitterJobName: "B", previousHelixJobName: "helix-b"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-a-resub"] = PassFail(passed: ["a-fail"]),
                    ["helix-b-resub"] = PassFail(failed: ["b-fail"]),
                });

            var runner2 = CreateRunner(azdo2, helix2);
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
                    HelixJob("helix-a", "finished", submitterJobName: "A"),
                    HelixJob("helix-b", "finished", submitterJobName: "B"),
                    HelixJob("helix-a-resub", "finished", submitterJobName: "A", previousHelixJobName: "helix-a"),
                    HelixJob("helix-b-resub", "finished", submitterJobName: "B", previousHelixJobName: "helix-b"),
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
                    HelixJob("helix-a", "finished", submitterJobName: "A"),
                    HelixJob("helix-b", "finished", submitterJobName: "B"),
                    HelixJob("helix-a-resub", "finished", submitterJobName: "A", previousHelixJobName: "helix-a"),
                    HelixJob("helix-b-resub", "finished", submitterJobName: "B", previousHelixJobName: "helix-b"),
                    HelixJob("helix-b-resub-2", "finished", submitterJobName: "B", previousHelixJobName: "helix-b-resub"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-b-resub-2"] = PassFail(passed: ["b-fail"]),
                });

            var runner3 = CreateRunner(azdo3, helix3);
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
                    Task completed = await Task.WhenAny(azdo1.UploadCompleted.Task, Task.Delay(TimeSpan.FromSeconds(5)));
                    completed.Should().BeSameAs(azdo1.UploadCompleted.Task);
                    cts1.Cancel();
                });

            int exitCode1 = await runner1.RunAsync(cts1.Token);

            exitCode1.Should().Be(1);
            helix1.Resubmissions.Should().ContainSingle();
            helix1.Resubmissions[0].OriginalJob.Should().Be("helix-a");
            helix1.Resubmissions[0].FailedItems.Should().BeEquivalentTo(["a-fail"]);
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
                    HelixJob("helix-a", "finished", submitterJobName: "A"),
                    HelixJob("helix-a-resub", "running", submitterJobName: "A", previousHelixJobName: "helix-a"),
                    HelixJob("helix-b", "finished", submitterJobName: "B"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-b"] = PassFail(failed: ["b-fail"]),
                });
            helix3.ConfigureResubmission("helix-b", "helix-b-resub");
            helix3.AddResponse(
                jobs:
                [
                    HelixJob("helix-a", "finished", submitterJobName: "A"),
                    HelixJob("helix-a-resub", "running", submitterJobName: "A", previousHelixJobName: "helix-a"),
                    HelixJob("helix-b", "finished", submitterJobName: "B"),
                    HelixJob("helix-b-resub", "running", submitterJobName: "B", previousHelixJobName: "helix-b"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-b"] = PassFail(failed: ["b-fail"]),
                });
            helix3.AddResponse(
                jobs:
                [
                    HelixJob("helix-a", "finished", submitterJobName: "A"),
                    HelixJob("helix-a-resub", "finished", submitterJobName: "A", previousHelixJobName: "helix-a"),
                    HelixJob("helix-b", "finished", submitterJobName: "B"),
                    HelixJob("helix-b-resub", "finished", submitterJobName: "B", previousHelixJobName: "helix-b"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-a-resub"] = PassFail(passed: ["a-fail"]),
                    ["helix-b"] = PassFail(failed: ["b-fail"]),
                    ["helix-b-resub"] = PassFail(passed: ["b-fail"]),
                });

            var runner3 = CreateRunner(azdo3, helix3);
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
        public async Task Attempt1_ResubmitsFailedWorkItemsFoundOnEntry_ExitOne()
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
            helix.Resubmissions.Should().ContainSingle();
            helix.Resubmissions[0].OriginalJob.Should().Be("helix-linux");
            helix.Resubmissions[0].FailedItems.Should().BeEquivalentTo(["wi-fail"]);
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
            helix.Resubmissions.Should().ContainSingle();
            helix.Resubmissions[0].OriginalJob.Should().Be("helix-a");
            helix.Resubmissions[0].FailedItems.Should().BeEquivalentTo(["a-fail"]);
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
                MonitorJob(),
                PipelineJob("A", "completed", "failed"));

            helix.AddResponse(
                jobs: [HelixJob("helix-a", "finished", submitterJobName: "A")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-a"] = PassFail(failed: ["a-fail"]),
                });
            helix.ConfigureNullResubmission("helix-a");

            var runner = CreateRunner(azdo, helix);
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

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["wi-fail"]),
                });
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub");
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

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(failed: ["wi-fail"]),
                });
            // The failed work item cannot be resubmitted (e.g. resubmission limit reached).
            helix.ConfigureNullResubmission("helix-linux");

            var runner = CreateRunner(azdo, helix);
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
                    HelixJob("helix-test", "finished", stageName: "Test"),
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
                    HelixJob("helix-test", "finished", stageName: "Test"),
                    HelixJob("helix-test-resub", "finished", stageName: "Test", previousHelixJobName: "helix-test"),
                    HelixJob("helix-build-retry", "running", stageName: "Build", previousHelixJobName: "helix-test"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-test"] = PassFail(failed: ["test-fail"]),
                    ["helix-test-resub"] = PassFail(passed: ["test-fail"]),
                });

            var runner = CreateRunner(azdo, helix, stageName: "Test");
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

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(jobs: [HelixJob("helix-linux", "finished")]);
            helix.WithWorkItems(
                "helix-linux",
                [new WorkItemSummary("helix-linux/wi-timeout", "helix-linux", "wi-timeout", "TimedOut") { ExitCode = 0 }]);
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub");
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished"),
                    HelixJob("helix-linux-resub", "finished", previousHelixJobName: "helix-linux"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux-resub"] = PassFail(passed: ["wi-timeout"]),
                });

            var runner = CreateRunner(azdo, helix);
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
        public async Task LoopStatus_VerboseLogsFullHelixJobWorkItemList()
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
                message.Contains("Helix job details:", StringComparison.Ordinal)
                && message.Contains("└─ 🧪 Helix job helix-linux [Running]", StringComparison.Ordinal)
                && message.Contains("   ├─ wi-01 (Running)", StringComparison.Ordinal)
                && message.Contains("   ├─ wi-10 (Running)", StringComparison.Ordinal)
                && message.Contains("   ├─ wi-11 (Running)", StringComparison.Ordinal)
                && message.Contains("   └─ wi-12 (Running)", StringComparison.Ordinal));
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
                message.Contains("└─ 🧪 Helix job helix-linux [Running]", StringComparison.Ordinal));
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
            azdo.WithFailedUpload("helix-linux", "workitem-1");

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

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            // Snapshot 1 (retry pass): only the original Helix job exists, finished, with
            // workitem-1 passing by exit code and workitem-2 failing by exit code. Without the
            // AzDO test-failure lookup the retry pass would only resubmit workitem-2.
            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"], failed: ["workitem-2"]),
                });

            // Snapshot 2 (next poll): the resub has finished too with both items passing.
            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished"),
                    HelixJob("helix-linux-resub", "finished", previousHelixJobName: "helix-linux"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["workitem-1"], failed: ["workitem-2"]),
                    ["helix-linux-resub"] = PassFail(passed: ["workitem-1", "workitem-2"]),
                });
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub");

            var runner = CreateRunner(azdo, helix, logger: logger);
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

            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));
            azdo.AddTimelineResponse(MonitorJob(), PipelineJob("Test Linux", "completed", "succeeded"));

            helix.AddResponse(
                jobs: [HelixJob("helix-linux", "finished")],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-test-only"], failed: ["wi-exit-fail"]),
                });

            helix.AddResponse(
                jobs:
                [
                    HelixJob("helix-linux", "finished"),
                    HelixJob("helix-linux-resub", "finished", previousHelixJobName: "helix-linux"),
                ],
                passFailByJob: new(StringComparer.OrdinalIgnoreCase)
                {
                    ["helix-linux"] = PassFail(passed: ["wi-test-only"], failed: ["wi-exit-fail"]),
                    ["helix-linux-resub"] = PassFail(passed: ["wi-test-only", "wi-exit-fail"]),
                });
            helix.ConfigureResubmission("helix-linux", "helix-linux-resub");

            var runner = CreateRunner(azdo, helix, logger: logger);
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
            azdo.WithFailedUpload("helix-linux", "workitem-1");

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
            SystemAccessToken = "token",
            TeamProject = "public",
            WorkingDirectory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "job-monitor-test"),
        };

        private static readonly Func<TimeSpan, CancellationToken, Task> NoDelay = (_, _) => Task.CompletedTask;

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
            public List<string> Messages { get; } = [];

            public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception exception,
                Func<TState, Exception, string> formatter)
            {
                Messages.Add(formatter(state, exception));
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
