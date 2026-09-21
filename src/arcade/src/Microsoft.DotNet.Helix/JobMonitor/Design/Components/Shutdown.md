# Shutdown and drain

## Normal completion

The runner stops producing completed jobs, completes the job queue, waits for
job expansion, completes and drains the work-item queue, then completes and
drains finalization. This ordering guarantees that no downstream producer is
still active when its channel is closed.

Work starts as soon as each Helix job completes, so normal drain contains only
the remaining tail. The drain log records elapsed time and aggregate pipeline
counts for runtime performance validation.

At drain start, the monitor also records how many work items were first
observed terminal in the final poll, how many uploads became eligible at the
whole-job boundary in that poll, and how much remaining work came from the
final poll versus earlier polls. This distinguishes the unavoidable minimum
drain from pipeline lag accumulated before Helix completion.

## Cancellation

Cancellation does not drain uploads. The pipeline worker tokens are canceled
immediately, the timeout report is emitted, and latest in-flight Helix jobs are
canceled with an independent bounded token. Incomplete test runs remain
untagged and are replayed by a later invocation.

## Crash recovery

In-memory queue/session state is never required after restart. Completed tags,
failed-work-item attachments, Helix job properties, and resubmission lineage
are sufficient to reconstruct all required work.
