# Architecture

## Ownership

`JobMonitorRunner` is the single control-plane owner. It performs the entry
retry pass, obtains one immutable Azure DevOps/Helix snapshot per poll, updates
`MonitorState`, queues newly completed jobs, reports status from the snapshot,
and decides when normal completion is possible.

Result publication is owned by `TestResultUploadPipeline`. The poller hands the
pipeline a completed job plus the work-item snapshot that established
completion. The pipeline never calls back into the poll loop and the reporter
never calls into the pipeline's external services.

## Shared parallelism

`Parallelism/ActionQueue<T>` is the shared long-lived producer/consumer
primitive. It supports bounded and unbounded `Channel<T>` instances with a
fixed worker set, cancellation, fault propagation, completion, drain, and
atomic queue counters. Only lightweight completed-job descriptors use the
unbounded form; work-item and finalization queues remain bounded.

`Parallelism/ParallelAsync` is used for bounded snapshot reads whose complete
result is needed by the current poll.

Parallelism budgets are independent:

- job expansion has low parallelism because it only creates session state and
  feeds work items;
- work-item processing uses `TestResultUploadParallelism`;
- test-run finalization has low parallelism because it performs small,
  non-replayable writes.

This prevents the old multiplication of "jobs × work items × result files"
tasks and keeps expensive state proportional to configured concurrency plus
bounded queue capacity.

## Data flow

```text
Azure DevOps + Helix snapshots
             |
             v
      JobMonitorRunner
             |
             v
 lightweight completed-job queue
             |
             v
 bounded work-item queue
   download -> parse -> aggregate -> publish
             |
             v
 bounded finalization queue
 attachment -> complete/tag
```

Backpressure is intentional. Job workers may wait while feeding a full
work-item queue, and work-item workers may wait on Azure DevOps throttling.
Completed-job acceptance remains non-blocking, so neither condition blocks the
poller or status reporter.

`JobMonitorMetrics` is shared by the runner, services, publisher, rate-limit
gate, and upload pipeline. It records atomic request counts and operation
timings without emitting per-request information logs. The final aggregate
report is described in [Performance metrics](Components/PerformanceMetrics.md).

## Logical stream identity

Retry and outcome reconciliation operate on logical Helix job streams rather
than AzDO jobs. An AzDO job can submit multiple Helix jobs to the same queue,
so AzDO job identity plus queue does not uniquely identify a stream.

`MonitorState` combines the stage identity, AzDO phase identity, queue, and
submitter-assigned logical Helix `jobName` (falling back to `TestRunName`).
Attempts are incarnation metadata rather than key components. Retry compares
the preserved submitter `System.JobAttempt` with the current timeline record;
resubmissions stamp the current stage attempt, preserve the submitter attempt,
and add `PreviousHelixJobName`.

## Durability boundary

The only durable "processed" marker is the Helix-job tag on a completed Azure
DevOps test run. A session is finalized only if every work item completed
cleanly. Any download, parse, publication, or finalization failure leaves the
run untagged so a later invocation replays the complete Helix job.
