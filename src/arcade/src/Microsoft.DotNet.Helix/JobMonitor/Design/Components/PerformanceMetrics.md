# Performance metrics

The monitor emits one aggregate performance block at normal completion or
timeout. Metrics are recorded with atomic counters and stopwatch timestamps;
request-level logging is not required and normal log volume is independent of
the number of requests.

## Remote operations

Azure DevOps metrics count every HTTP attempt, separated into control-plane,
test-result batch, and attachment requests. The report includes retries,
failed attempts, serialized request payload bytes, aggregate request time, and
the slowest request. A retried request contributes one attempt and its payload
bytes for each transmission.

Helix metrics count attempts made through the shared retry wrapper. Result blob
downloads are reported separately because they use Azure Storage rather than
the Helix API.

Rate-limit metrics distinguish server-directed deferrals from actual shared
gate waits. Deferrals report the delay guidance received from Azure DevOps;
gate waits report aggregate worker wait time and the longest individual wait.
Aggregate wait can exceed wall-clock time when several workers are delayed
concurrently.

## Pipeline timings

The report includes aggregate worker time and maximum single-operation time for:

- work-item result discovery and download;
- XML parsing and aggregation;
- Azure DevOps publication;
- test-run creation;
- test-run completion and durable tagging.

Upload throughput uses the interval from the first pipeline operation to the
last completed operation, not the full monitor lifetime spent waiting for
Helix work. Parsing is measured separately from Azure DevOps publication.

Aggregate worker time can exceed monitor elapsed time because work executes in
parallel. Comparing aggregate time, maximum latency, request counts, and
throughput identifies whether a run is limited by result download, local
processing, Azure DevOps request latency, attachments, throttling, or final
test-run operations.
