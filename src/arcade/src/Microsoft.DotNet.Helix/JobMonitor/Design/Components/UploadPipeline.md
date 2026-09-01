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
2. obtains the session's single test-run ID;
3. parses, aggregates, batches, and publishes results;
4. records the upload summary and test-only failure outcome;
5. signals session completion.

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

Test-run creation and attachment publication are not replayed after ambiguous
failures because they are non-idempotent POST operations. The final completion
PATCH is idempotent and uses bounded transient retries; repeating it applies
the same completed state and Helix-job tag to the same run without creating
duplicate results. Result publication also uses bounded transient retries
because losing an entire job's results is worse than the accepted duplicate
risk.
