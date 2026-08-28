# Helix Job Monitor design

The job monitor is a restart-safe coordinator between Azure DevOps and Helix. It
polls control-plane state, reconciles retries, publishes large test-result sets,
and computes the final stage outcome without tying progress reporting to result
upload latency.

The design is split by concern:

- [Semantic behavior](SemanticBehavior.md) defines externally
  observable behavior and restart invariants.
- [Architecture](Architecture.md) describes process structure, ownership,
  backpressure, and shared parallelism utilities.
- [Polling and reconciliation](Components/Polling.md) describes discovery,
  stage-attempt scoping, retry, and outcome ordering.
- [Upload pipeline](Components/UploadPipeline.md) describes the bounded
  job/work-item/finalization stages and durable completion boundary.
- [Test-result processing](Components/TestResults.md) describes XML parsing,
  aggregation, batching, attachments, and Azure DevOps limits.
- [State and status](Components/StateAndStatus.md) describes thread-safe state,
  snapshots, progress, and bounded verbose logging.
- [Performance metrics](Components/PerformanceMetrics.md) describes request
  counts, retries, throughput, rate-limit waits, and pipeline stage timings.
- [Shutdown](Components/Shutdown.md) describes normal drain, cancellation, and
  crash recovery.
