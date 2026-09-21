# Polling and reconciliation

Each poll concurrently refreshes work-item summaries for in-scope jobs that
have not yet been reconciled, with a fixed degree of parallelism. Terminal
snapshots are retained for later status reports instead of being downloaded
again. The resulting dictionary is the poll's immutable work-item snapshot and
is reused for:

- completion fallback when a Helix job summary has not yet transitioned;
- outcome reconciliation;
- upload scheduling;
- failed-work-item links;
- aggregate status counts.

No second service call is made for status.

The one-shot entry retry pass and stage/job-attempt semantics are specified in
[the semantic document](../SemanticBehavior.md). It uses the same timeline
snapshot as the first poll to compare each Helix stream's preserved submitter
job attempt with the current timeline job attempt. Outcome updates are applied
oldest-to-newest so newer stage/job attempts and resubmission lineage supersede
older failures.

The logical stream key includes stage, AzDO phase/job identity, queue, and the
submitter-assigned Helix `jobName` (or `TestRunName` when `jobName` is absent).
That discriminator is preserved by resubmission, allowing retries to collapse
only actual incarnations of the same logical Helix job without crossing stages.
