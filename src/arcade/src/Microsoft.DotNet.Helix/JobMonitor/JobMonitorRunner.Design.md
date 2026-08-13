# JobMonitorRunner — Technical Specification

This document is a behavioral specification of the Helix job monitor runner.
It describes *what* the runner must do, not *how* it currently does it.

The current source lives at [JobMonitorRunner.cs](JobMonitorRunner.cs); use
it only as the reference implementation, not as the specification.

---

## 1. Purpose

The runner is the body of a standalone CLI invoked as a single job inside an
Azure DevOps pipeline stage. Its job is to:

- Observe the Helix jobs submitted from the same build (by the Helix SDK
  submitter) and the Azure DevOps timeline jobs running alongside it.
- Resubmit failed Helix work items once per invocation.
- Upload Helix work-item test results to Azure DevOps.
- Return an exit code that reflects whether the monitored pipeline jobs and
  the latest completed Helix work items all succeeded.

## 2. Operating model

### 2.1 Stage-attempt scope

Each invocation owns exactly one Azure DevOps stage **attempt**. The guiding
principle has two halves that must both hold:

1. **Completion is gated on the current attempt only.** The monitor must never
   block on Helix work left behind by a *previous* stage attempt. Such work may
   never reach a terminal state (for example, work items stranded in `Waiting`
   after their queue was purged), and a superseded attempt's monitor is already
   gone, so nothing else will ever drive it. Waiting on it means waiting forever.
2. **All pipeline-submitted work must still complete.** The monitor cannot simply
   discard a previous attempt's work either: that work represents tests the
   pipeline asked to run. Any previous-attempt work that is not already
   terminally passed (i.e. it failed or is still unfinished) and that the current
   attempt has *not* already re-submitted must be **resubmitted into the current
   attempt** so it is actually carried to completion (§2.3).

Concretely, all decisions (retry, completion gating, upload, pass/fail) consider:

- Azure DevOps timeline jobs belonging to the monitor's stage.
- Helix jobs whose `System.StageName` property is empty (stage unknown) or
  matches the monitor's stage. Within that stage, a job's `System.StageAttempt`
  classifies it as **current-attempt** (empty/unknown, or equal to the monitor's
  own attempt) or **previous-attempt** (a lower attempt). Previous-attempt work
  is reconciled into the current attempt per §2.3 but is never *gated on*
  directly.

Jobs and work items from other stages must not be retried, uploaded, or used to
fail this invocation.

**Why per-attempt, and why not just ignore previous attempts.** Azure DevOps
offers two distinct re-run gestures, and the monitor cannot tell them apart from
the timeline alone:

- **Rerun the entire stage** — every job re-runs, including the Helix submitter
  jobs, so the current attempt already contains a fresh incarnation of every
  logical work stream. Previous-attempt incarnations are superseded and need no
  resubmission.
- **Retry failed jobs in the stage** — only failed jobs re-run. If the Helix
  submitter jobs passed and only the monitor failed (e.g. it timed out), the
  submitters do **not** re-run, so the current attempt contains **no** Helix work
  at all. Naively scoping to the current attempt would make the monitor exit
  immediately as a success, silently discarding every result and failure from the
  previous attempt.

Because of the second gesture, "current-attempt scope" is not the same as
"ignore previous attempts." The monitor scopes *gating* to the current attempt
but reconciles previous-attempt work into it by resubmission (§2.3), deciding
per logical work stream (not per attempt) whether a resubmission is needed.

The monitor's own stage attempt is provided as an input (see §3) and defaults to
the `SYSTEM_STAGEATTEMPT` pipeline variable. When it is unknown the monitor
cannot distinguish attempts and falls back to build + stage scope, gating on
every attempt's work (historical behavior).

### 2.2 Durable state

The runner may crash, time out, or be retried at any point. Any behavior that
must survive a restart can only be reconstructed from durable external state:

- **Azure DevOps test-run tags** — when the monitor completes a test run it
  tags it with the Helix job name, encoded as `helixjob<guid-without-dashes>`
  (AzDO test-run tags must be alphanumeric and at most 50 characters). The
  presence of that tag on a completed test run is the durable signal that the
  corresponding Helix job's results have already been uploaded. Tags must be
  posted as objects (`{ "name": "..." }`); the string form is silently dropped.
  Tags are not returned inline on a run and are read back via the build-scoped
  test results tags endpoint on the `vstmr` host. The tag is applied at
  completion (not creation) so it exists if and only if the run completed and
  results finished uploading.
- **Helix job properties** — every resubmitted Helix job preserves the
  original submitter's properties and adds `PreviousHelixJobName`, linking
  to the job that was resubmitted. The chain of `PreviousHelixJobName`
  links represents the incarnations of one logical piece of work.

In-memory state may be used freely within a single invocation but must never
be the source of truth for cross-invocation correctness.

### 2.3 Retry invariants

Retry is the mechanism that reconciles previous-attempt work into the current
attempt (§2.1). It operates on *logical work streams*, not on attempts: a work
stream is identified by the root Helix job in its `PreviousHelixJobName` lineage
(§5.7), which is stable across monitor resubmissions.

1. Retry runs exactly once per invocation, on entry, before polling begins.
2. The set of work to resubmit is decided from a single Helix snapshot taken on
   entry. Work that fails after the monitor has started is not resubmitted during
   the current invocation; a later invocation may pick it up.
3. Retry decisions are made per work stream from its **latest incarnation across
   all attempts** (the leaf of its lineage chain, breaking ties toward the higher
   stage attempt). Let *L* be that incarnation:
   - *L* is **still in flight** (running/waiting) and belongs to the **current
     attempt** — leave it; the current attempt is actively driving it and
     completion gating waits on it. This is the rerun-entire-stage case and also
     prevents duplicate submissions when a previous-attempt incarnation of the
     same stream is still running.
   - *L* is **still in flight** and belongs to a **previous attempt** — the
     previous attempt has abandoned it (its monitor is gone and nothing else will
     drive it); resubmit the not-yet-passed items into the current attempt.
   - *L* is **completed and fully passed** — nothing to resubmit; its results are
     uploaded (if not already, §2.4) and its outcome counted. It is terminal, so
     it does not block completion.
   - *L* is **completed with failures** — resubmit the failed items, regardless of
     attempt. (For a current-attempt incarnation this is the pre-existing
     per-invocation retry; for a previous-attempt one it carries the failure into
     the current attempt.)
   - A needed resubmission is **not possible** (e.g. the queue was removed, so the
     work can never run again) — for previous-attempt in-flight work, whose
     failure is not otherwise recorded, surface it as an actionable hard failure
     so the invocation fails fast rather than waiting forever. (Completed-with-
     failures work that cannot be resubmitted already fails the build via outcome
     reconciliation, §2.5.)
4. Every resubmission is stamped with the **monitor's current stage attempt**
   (not the original job's attempt) and linked back via `PreviousHelixJobName`.
   This is what brings the resubmitted work into current-attempt scope so the
   monitor gates on it; copying the original attempt would leave the monitor
   unable to see its own resubmission.
5. A resubmission supersedes the incarnation it was created from for pass/fail
   and ordering purposes (latest incarnation wins).

Repeated monitor runs therefore submit progressively fewer work items as newer
incarnations succeed, and — crucially — a monitor that was cancelled part-way
through its retry pass can be re-run: the next invocation re-derives the
remaining work from the Helix snapshot (latest incarnation + attempt + status per
stream), so partially-resubmitted state is self-correcting.

#### 2.3.1 Corner cases the reconciliation model must handle

These are the scenarios that a naive "scope strictly to the current attempt and
ignore all previous-attempt jobs" design gets wrong, and how the model above
addresses each:

1. **Retry-failed-jobs where only the monitor re-ran.** The submitters passed and
   were not re-run, so the current attempt contains no Helix work. Naive scoping
   exits `0` immediately, discarding every previous-attempt result and failure.
   → The retry pass reconciles previous-attempt streams: passed work is uploaded
   and counted, failed/unfinished work is resubmitted into the current attempt
   and then gated on.
2. **Resubmission stamped with the wrong attempt.** If a resubmission inherits the
   original job's `System.StageAttempt` (a previous attempt), the monitor cannot
   see its own resubmission and never gates on it. → Resubmissions are stamped
   with the monitor's current attempt (§2.3.4).
3. **Cancel before/while resubmitting, then retry again.** Attempt *N* begins
   resubmitting but is cancelled before finishing; attempt *N+1* must still find
   the streams that were never resubmitted (previous-attempt, non-terminal, no
   current incarnation) and resubmit them. → Decisions are re-derived from the
   Helix snapshot each invocation (latest incarnation + attempt + status per
   stream), not from in-memory state, so partial progress is self-correcting.
4. **Independent jobs with identical submission metadata.** One AzDO job can
   submit multiple original Helix jobs to the same queue, so submitter and queue
   cannot safely identify a stream. → Only an explicit `PreviousHelixJobName`
   lineage collapses jobs; unlinked jobs remain independent (§5.7).
5. **Un-resubmittable work (e.g. purged queue).** Previous-attempt work that can
   never run again would loop forever under any "just wait" or "just resubmit and
   wait" scheme. → Resubmission-not-possible is treated as an actionable hard
   failure so the invocation fails fast instead of hanging (§2.3.3).

### 2.4 Upload invariants

Upload is restart-resilient but logically independent from retry.

1. A Helix job has at most one durably completed, tagged test run. The durable
   deduplication signal is the Helix-job-name tag on completed AzDO test runs.
   An interrupted, untagged upload is intentionally replayed.
2. For every completed Helix job without a completed, tagged upload, all
   available test results are uploaded.
3. Uploads happen in lineage order — oldest incarnation first. If both an
   original job and its resubmission have completed and neither has been
   uploaded, the original uploads first.
4. Upload failures are logged as warnings but never affect pass/fail. Read-only
   download failures are retried a bounded number of times. State-changing
   operations are not replayed by the queue after an ambiguous failure because
   they may have partially succeeded. A failed upload remains untagged so a
   later monitor invocation may replay it in a new run.
5. A failed original Helix job may be resubmitted on entry and still have
   its original test results uploaded during the same invocation if those
   results were not uploaded earlier.

### 2.5 Pass/fail invariants

The exit code is determined by combining two checks:

- **AzDO side** — the monitor fails if any monitored AzDO job failed or
  was canceled. Jobs whose work is being actively retried this invocation
  are excluded from this check (their failure is represented by the
  resubmitted Helix work).
- **Helix side** — the monitor fails if the latest completed incarnation of
  any submitted work item failed. A newer passing incarnation supersedes an
  older failed one.

Upload state never affects pass/fail.

Exit code is `0` only when both checks pass; otherwise `1`. Cancellation
(timeout) also exits with `1`.

### 2.6 Crash and timeout resilience

The runner must be safe to re-run after any abrupt termination. In
particular:

- Completed, tagged uploads must not be repeated. Interrupted, untagged uploads
  are intentionally replayed on the next run.
- Retry candidates must be rediscovered from Helix job properties, not from
  prior in-memory state.
- Cancellation must not wait for pending or in-flight uploads. An upload that
  does not complete remains untagged and is rediscovered and re-uploaded by a
  later invocation.
- On cancellation, immediately emit the timeout report, then best-effort cancel
  the latest in-flight Helix job incarnation in each lineage even though the
  runner's own cancellation token has already fired. This cleanup uses a fresh,
  bounded cancellation token so it cannot extend shutdown indefinitely.

Re-attach spans stage attempts. Within the same attempt (a monitor job-retry or
a crashed-and-restarted monitor process) the runner re-attaches to the same
current-attempt Helix jobs. Across attempts it does not passively re-attach to a
previous attempt's jobs — instead it reconciles them into the current attempt by
resubmission (§2.1, §2.3). Both paths are driven from durable Helix/AzDO state,
never from prior in-memory state, so any abrupt termination is recoverable.

## 3. Inputs

The runner is configured by an options object. The semantically meaningful
inputs are:

| Input | Purpose |
| --- | --- |
| Helix endpoint + access token | Talk to the Helix service. |
| Organization, project, repository, branch, build reason | Compose the Helix `source` filter (see §5.1). |
| Build ID | Scope Helix and AzDO queries to this build. |
| AzDO collection URI + project | Construct the test-results URL used in failure reports. |
| Stage name | Stage scope (see §2.1). |
| Stage attempt | Per-attempt scope (see §2.1). Defaults to `SYSTEM_STAGEATTEMPT`; when unknown the monitor tracks jobs from every attempt of the stage. |
| Polling interval | Delay between poll iterations; a minimum floor applies. |
| Maximum wait | Reported in the timeout message; the timeout itself is enforced by the caller through cancellation. |
| Job monitor name | Identifier of the monitor's own AzDO timeline record; used to exclude it from pass/fail. |
| Working directory | Local staging directory for downloaded test results. |
| Verbose flag | Forces a status snapshot every poll. |

## 4. External contracts

The runner depends on two service interfaces. The contracts are described
behaviorally; method names are illustrative.

### 4.1 Helix service

- **List jobs for a build** — given the source filter and build ID, return
  all Helix jobs that the submitter recorded for the build. The source
  filter must be derivable from build metadata in lockstep with the
  submitter (see §5.1). The returned set spans every attempt of the build; the
  runner keeps the whole stage's jobs (all attempts) so the retry pass can
  reconcile previous-attempt work (§2.3), and classifies each job as
  current- or previous-attempt via `System.StageName` / `System.StageAttempt`
  for gating (§2.1).
- **List work items for a job** — return all work-item summaries.
- **Download test results** — given a job and a set of work-item names,
  download recognized result files into a working directory. Individual
  per-file failures must not prevent the remaining files from being attempted.
  After the batch, transient failures cause the read-only download phase to be
  retried; permanent failures are logged and omitted.
- **Cancel a job** — best-effort cancellation.
- **Resubmit failed work items** — given the original job and a set of
  failed (or unfinished) work items, submit a new Helix job that contains only
  those items. The new job must inherit the original's submitter identity (stage,
  job name, display name, test-run name, queue) but be stamped with the
  **resubmitting monitor's current stage attempt** (§2.3.4), and link back via
  `PreviousHelixJobName`. May return "not possible" (e.g. queue gone), which the
  runner treats as an actionable hard failure for that work rather than silently
  skipping it (§2.3.3).

### 4.2 Azure DevOps service

- **Get timeline records** — return the build's timeline.
- **Get processed Helix job names** — extract Helix job names from
  `helixjob<guid>` tags on completed test runs (read via the build-scoped
  test results tags endpoint). This is the durable upload-dedup signal.
- **Create test run / upload results / complete test run** — the standard three-call sequence. Creation always creates a new in-progress test run with a plain name; completion tags the run with the Helix job name. Durable deduplication is based on that completion-time tag (§2.2).

## 5. Behavior

### 5.1 Helix source filter

On entry the runner derives a Helix `source` string from the build metadata
(organization, project, repository, branch, build reason). This string must
match what the Helix SDK submitter produced for the same build — for PR,
scheduled, manual, IndividualCI, BatchedCI, and internal-official runs
alike. Any change in derivation must be made in lockstep with the submitter,
or the runner will silently fail to see its own jobs.

### 5.2 Lifecycle

1. Log the build and stage being monitored.
2. Load the set of already-uploaded Helix job names (§2.2).
3. Perform the one-shot retry pass (§5.3).
4. Enter the poll loop (§5.4) until the build finishes or cancellation
   fires.
5. On cancellation (timeout), do not wait for pending uploads. Immediately emit
   a timeout report (§5.6), use a fresh bounded token to best-effort cancel the
   latest in-flight Helix jobs (§2.6), and exit `1`. Incomplete uploads remain
   untagged and are retried by a later invocation.
6. On normal completion, emit the final summary and exit per §2.5.

### 5.3 Retry pass

1. Take a Helix snapshot of the whole stage (all attempts).
2. Reduce it to the latest incarnation of each logical work stream (§2.3.3):
   the leaf of each lineage chain, keyed by its root Helix job.
3. For each latest incarnation, apply §2.3.3:
   - Current-attempt incarnation — leave it; it is already being driven.
   - Previous-attempt, completed and fully passed — leave it (terminal); it will
     still be uploaded / reconciled by the poll loop.
   - Previous-attempt, completed with failures, or unfinished — ask the Helix
     service to resubmit the failed / not-yet-passed items, stamped with the
     current stage attempt (§2.3.4). If resubmission is not possible, record it
     as a hard failure (§2.3.3).
4. Remember the AzDO submitter-job identifiers of successfully retried work;
   these are the jobs to exclude from the AzDO failure check while this
   invocation runs.
5. Carry the current-attempt jobs plus the newly resubmitted jobs forward as the
   input to the first poll iteration so the first iteration sees them
   immediately. (Subsequent iterations refetch from Helix.)

If nothing was eligible for retry, log that fact.

### 5.4 Poll loop

Each iteration:

1. Check for cancellation.
2. Fetch the AzDO timeline and the Helix snapshot for the stage (all attempts),
   classifying each Helix job as current- or previous-attempt (§2.1).
3. Update the in-memory view of each Helix job with the freshest snapshot
   (so completion/failure transitions are not missed).
4. Compute the set of completed Helix jobs (§5.5).
5. **First pass — upload**: for each completed Helix job not already
   uploaded (per §2.2), upload its test results and remember it as
   processed. This pass is the only one that triggers uploads.
6. **Second pass — outcome reconciliation**: for every completed Helix
   job in scope, ensure its per-work-item outcomes are reflected in the
   running outcome map (§5.7), processing lineage from oldest to newest so
   newer incarnations supersede older ones. This pass must consider all
   completed jobs — including ones uploaded by an earlier invocation —
   because the outcome map is the only source for the pass/fail decision
   and is not durable across invocations.
7. Decide whether to log status this iteration. The decision uses the
   verbose flag, whether any counts changed since the last status log, and
   a maximum interval (so long-stable builds still emit periodic progress).
8. Evaluate termination: all monitored AzDO jobs complete *and* every
   **current-attempt** Helix job complete. Previous-attempt jobs are not gated
   on — by this point each has either a current-attempt incarnation, been
   resubmitted into the current attempt, or is itself already terminal (§2.1,
   §2.3). When true, wait for pending uploads, emit the final report, and exit
   per §2.5.
9. Otherwise sleep for the configured poll interval and repeat.

### 5.5 Completion of a Helix job

A Helix job is considered complete when the Helix service reports it
finished or failed. As a fallback for jobs whose status transition has not
yet been observed, the runner may treat a job as complete when every one of
its expected work items has a terminal exit code. The fallback is only safe
when the expected work-item count is known and non-zero.

### 5.6 Timeout report

On cancellation, the runner emits two grouped reports:

- All scoped Helix jobs that are either not yet finished or finished but
  not yet uploaded — each with its display name, status, expected
  work-item count, and a clickable details URI.
- All scoped non-monitor AzDO timeline jobs not yet in `completed` state —
  each with its name, state, and result.

If both groups are empty, emit a single critical-level note that nothing
unfinished was tracked at the time of timeout (this means timeout fired
during the brief window between completion and termination).

### 5.7 Per-work-item outcome map

The runner maintains an in-memory map from *logical work item* to its
latest observed pass/fail status. A logical work item is identified by the
work-item name plus a stable key that survives resubmission, so all
incarnations of the same item collapse onto a single entry.

The chain key must be deterministic and uniqueness-preserving:

- Every original Helix job must produce a distinct key, including jobs submitted
  by the same AzDO job to the same Helix queue.
- An original Helix job and its resubmission(s) on the same queue must
  produce the same key so the latest incarnation overwrites the older one.
- Jobs not linked by `PreviousHelixJobName` remain distinct. In particular,
  submitter name and queue are insufficient to correlate jobs across stage
  attempts because one AzDO job may submit multiple jobs to the same queue.
- If lineage cannot be resolved (the predecessor link points outside the
  jobs the runner has observed), the referenced predecessor name is used as
  the root so later incarnations still resolve consistently.

The same key drives a parallel map of "failed work item console info" used
to build the final failure report. When a later incarnation of a work item
passes, its entry in that map is cleared.

### 5.8 Failure reporting

Failed Helix work items must produce clickable console-link warnings in the
AzDO build log:

- Once per failed work-item observation during status logs (deduplicated
  across the invocation so we don't spam the same link).
- Once per failed work item in a completed job during the upload pass
  (same dedup).
- At termination, a single aggregated error block listing every still-
  failing work item, prefixed with the test-results URL for the build.

Warnings use AzDO `task.logissue type=warning` formatting; the final
aggregated error uses `task.logissue type=error`. Informational status
lines are plain logger output.

### 5.9 Test-result upload pipeline

Uploads are asynchronous tasks tracked for normal completion. Their in-memory
lifecycle distinguishes queued, in-progress, durably completed, and failed
uploads; only a completed, tagged test run is considered durable.

- Each upload is queued asynchronously and tracked. Multiple uploads may
  proceed concurrently.
- Test results are downloaded before the AzDO test run is created. Transient
  download failures are safe to retry and use a bounded retry budget.
- Test-run creation and completion/tagging are each attempted once. These
  lifecycle writes determine the durable upload boundary, so replaying an
  ambiguous response could create an extra run or incorrectly mark an
  incompletely uploaded run as processed.
- Publishing test results and their attachments uses bounded retries for
  throttling, server errors, and transient transport failures. These Azure
  DevOps POST APIs do not expose an idempotency key or document deduplication.
  A timeout or connection failure can occur after the service commits the
  request but before the response reaches the client, so retrying may create
  duplicate results or attachments. The design accepts that risk to avoid
  losing an entire job's test results after a transient failure.
- Permanent failures and exhausted retries are logged as warnings and stop the
  upload task without affecting pass/fail.
- The normal-termination path waits for queued uploads to drain before exiting.
- The cancellation path does not wait for pending or in-flight uploads. If an
  upload has not completed and applied its Helix-job tag, it remains untagged;
  durable-state discovery causes a later invocation to upload it again.

The upload sequence per job is: create (or reuse) a test run with the plain
`{TestRunName}`, download results, upload them, complete the test run and tag
it with the Helix job name (`helixjob<guid>`).

### 5.10 Status logging

When a status log is due, the runner emits a one-line summary of work
counts (processed / completed / running / waiting jobs and work items). In
verbose mode it additionally emits a tree-style breakdown per job and work
item. The verbose tree is informational only.

A Helix job is classified for status purposes as `Processed` (already
uploaded), `Completed` (terminal but not yet uploaded), `Running` (has at
least one work item), or `Waiting` (no work items observed yet).

## 6. Externally observable formats

These shapes are observed by other tools or downstream parsers and must be
preserved:

- AzDO test-run tag: `helixjob<guid-without-dashes>`, applied to a completed
  test run as an object-form tag (`{ "name": "..." }`).
- AzDO log decorations: `##vso[task.logissue type=warning]` for warnings,
  `##vso[task.logissue type=error]` for errors. Informational lines use
  plain logger output (no `##vso` prefix).
- Test-results URL: the standard AzDO build-test-results-tab URL for the
  build, used as the link in the final failure block.
- A Helix work item is considered failed if its exit code is non-zero or
its state is not the terminal success state. A work item is considered
failed-and-terminal (worth reporting eagerly) when it is failed and not
still in flight.
