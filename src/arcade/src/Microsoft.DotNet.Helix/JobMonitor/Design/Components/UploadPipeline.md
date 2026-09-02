# Upload pipeline

`TestResultUploadPipeline` contains three worker stages. The lightweight job
stage is unbounded so completed jobs cannot be dropped and upload backpressure
cannot block polling. The expensive work-item and finalization stages are
bounded.

## Job expansion

A completed Helix job is accepted once. A `JobUploadSession` captures the
completion-time work-item snapshot and owns:

- a single-flight Azure DevOps test-run creation task;
- expected and finished work-item counts;
- failed-test work-item names;
- uploaded-result count;
- a sticky failure flag.

The job stage expands work items into the global work-item queue. A job with no
work items proceeds directly to finalization.

## Work-item processing

Terminal work items are admitted as soon as polling observes their immutable
exit code, even if the containing Helix job is still running. This lets large
jobs overlap result publication with their remaining execution instead of
making every completed work item part of the final drain.

Each worker:

1. downloads recognized result files for one work item, retrying only
   transient read failures;
2. asks `ITestResultProcessor` to parse and aggregate the local files into
   prepared results;
3. records the test-only failure outcome from the prepared results;
4. obtains the session's single test-run ID, whose creation uses bounded
   transient retries through `IAzureDevOpsService`;
5. asks `IAzureDevOpsResultPublisher` to convert and publish the prepared
   results to the session's test run in bounded batches;
6. signals session completion.

`IAzureDevOpsService` owns authenticated Azure DevOps REST operations and the
test-run lifecycle. It does not read local result files. The result publisher
uses run-specific transports supplied by the service, keeping conversion and
batching separate from HTTP retry and authentication.

Test outcomes are observed before test-run creation or result publication, so
an Azure DevOps failure cannot hide a failing test. If test-run creation still
fails, no publication is attempted. The shared creation failure is reported
once for the Helix job, and the session remains untagged for a later monitor
invocation to replay.

Work-item concurrency is global. A build with many jobs therefore cannot create
an unbounded task graph or multiply the configured Azure DevOps pressure.
Consumers can tune this global budget through the
`testResultUploadParallelism` pipeline-template parameter, which forwards to
the monitor's `--test-result-upload-parallelism` option. The default is 48,
selected from runtime validation with approximately 6,800 work items and
3.1 million results: it kept final drain below 2% while reducing service
throttling guidance compared with 64 workers.

## Finalization

Once the Helix job is complete, the last outstanding work item queues its
session for finalization. If any work item failed, the session remains
untagged. Otherwise finalization uploads the
failed-work-item attachment, marks the run completed, applies the Helix-job
tag, and only then marks the job durably processed.

Test-run creation uses bounded transient retries. A retry can leave an empty
orphaned run if Azure DevOps created the first run but its response was lost,
but results are uploaded only after a run ID is returned, so retrying creation
cannot duplicate results. Attachment publication is not replayed after an
ambiguous failure because it is a non-idempotent POST operation. The final
completion PATCH is idempotent and uses bounded transient retries; repeating it
applies the same completed state and Helix-job tag to the same run without
creating duplicate results. Result publication also uses bounded transient
retries because losing an entire job's results is worse than the accepted
duplicate risk.
