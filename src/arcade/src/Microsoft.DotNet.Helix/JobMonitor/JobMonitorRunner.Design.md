# Helix Job Monitor Runner Design

The Helix Job Monitor is intended to be a resilient observer for Azure DevOps
pipeline jobs and the Helix work submitted by those jobs. It must assume it may
crash, time out, or be retried at any point. Any behavior that must survive a
restart can only be reconstructed from durable external state:

- Azure DevOps test run name markers written by the monitor.
- Helix job properties written when jobs are submitted or resubmitted.

The monitor should not rely on in-memory state to make decisions that must remain
correct across restarts.

## Stage scope

Each monitor invocation owns exactly one Azure DevOps stage unless it is
explicitly configured to monitor all stages. In the normal stage-scoped mode, the
monitor must only look at:

- Azure DevOps timeline jobs that belong to the monitor's stage.
- Helix jobs whose `System.StageName` property is empty or matches the monitor's
  stage.

Stage scope applies to retry, test-result upload, and pass/fail calculation. A
failed job or failed Helix work item from another stage must not be retried,
uploaded, or used to fail this monitor invocation.

## Durable state

### Azure DevOps test run name markers

The monitor appends a Helix job name marker to each Azure DevOps test run name
it creates. The marker format is `[HelixJob:<helix-job-name>]`, so the full test
run name is `{original-name} [HelixJob:<helix-job-name>]`. Completed test runs
with that marker are used only to determine whether test results for a Helix job
have already been uploaded.

The monitor uses the test run name instead of Azure DevOps test run tags because
the Azure DevOps test runs API silently drops tags created with the run. Test run
name markers do not determine whether Helix work should be retried, and test
result upload success or failure does not determine whether the monitor passes or
fails.

### Helix job properties

Helix job properties are the durable state for retry lineage. Resubmitted Helix
jobs preserve the original job properties and add `PreviousHelixJobName`, which
points to the Helix job that was resubmitted.

This creates a chain of Helix job incarnations for the same logical work. The
monitor uses that chain to find the latest incarnation of each job when deciding
what to retry and what currently passes or fails.

## Retry behavior

Retry scheduling is separate from test result upload.

1. Retry is performed only when the monitor starts.
2. The monitor takes a Helix snapshot on entry and decides what to resubmit from
   that snapshot.
3. Work that fails after the monitor has started is not resubmitted during that
   invocation. It can be resubmitted only by a later monitor invocation.
4. A Helix job is considered for retry only if it is the latest completed
   incarnation in its `PreviousHelixJobName` chain.
5. If that latest completed incarnation has failed work items, only those failed
   work items are resubmitted.
6. Passing work items from the same Helix job are not included in the
   resubmission.
7. If a newer incarnation of a work item has completed and passed, older failed
   incarnations must not cause another resubmission.
8. If a newer incarnation is still running or waiting, it is not resubmitted
   again on monitor entry.

This means repeated monitor retries should naturally submit fewer work items over
time as newer incarnations complete successfully.

## Test result upload behavior

Test result upload is also restart-resilient, but it is independent from retry.

1. Never upload the same Helix job's test results twice.
2. Use Azure DevOps test run name markers on completed test runs to discover
   which Helix jobs have already been uploaded by previous monitor invocations.
3. For completed Helix jobs that have not been uploaded, upload all available
   test results.
4. Upload completed jobs in lineage order, from old to new. For example, if both
   an original job and a resubmitted job have completed and neither has been
   uploaded, upload the original job first and the resubmitted job second.
5. Test result upload failures should be logged, but they do not affect the
   monitor's pass/fail result.

This separation is important: a failed original Helix job may be resubmitted on
entry and still have its original test results uploaded during the same monitor
invocation if those results were not uploaded earlier.

## Monitor pass/fail behavior

The monitor result must reflect both:

- The Azure DevOps jobs it monitors.
- The latest completed Helix work-item outcomes for submitted Helix work.

The monitor should fail when a monitored Azure DevOps job failed or was canceled,
unless that Azure DevOps job's failure is represented by Helix work that the
monitor is actively retrying on this invocation.

The monitor should fail when the latest completed Helix incarnation for any work
item failed. A newer passing incarnation supersedes an older failed incarnation.

Test result upload state does not affect pass/fail. Uploads are reporting
artifacts; Helix work-item state and monitored Azure DevOps job state determine
the monitor result.

## Crash and timeout resilience

Because the monitor may stop at any time:

- It should be safe to rerun after partially uploading test results.
- It should skip uploads for Helix jobs whose marker appears in completed Azure
   DevOps test run names.
- It should discover retry candidates again from Helix job properties on the
  next entry.
- It should not require any process-local memory from a prior invocation.

The monitor may still have useful in-memory state while a single invocation is
running, but durable correctness must come from Azure DevOps test run name
markers and Helix job properties.
