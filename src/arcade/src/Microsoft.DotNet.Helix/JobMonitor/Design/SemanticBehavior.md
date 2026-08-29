# Semantic behavior

This document specifies the externally observable behavior and restart
invariants of the Helix Job Monitor. It describes *what* the monitor must do,
not *how* it currently does it.

The current source lives at [JobMonitorRunner.cs](../JobMonitorRunner.cs); use
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
  the latest completed Helix work items and their processed test results all
  succeeded.

## 2. Operating model

### 2.1 Stage-attempt scope

Each invocation owns exactly one Azure DevOps stage **attempt**. The guiding
principle has two halves that must both hold:

1. **Completion is gated on the current attempt only.** The monitor must never
   block on Helix work left behind by a *previous* stage attempt. Such work may
   never reach a terminal state (for example, work items stranded in `Waiting`
   after their queue was purged), and a superseded attempt's monitor is already
   gone, so nothing else will ever drive it. Waiting on it means waiting forever.
2. **Only work whose submitter did not rerun may be replayed.** Previous-stage
   work still represents the current execution only when the matching AzDO
   submitter job remains at the same `System.JobAttempt`. If the timeline shows
   a newer submitter attempt, the old Helix work is superseded and must not be
   resubmitted, even before the replacement Helix job is visible.

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

**Why both attempt axes are required.** Both Azure DevOps retry gestures advance
`System.StageAttempt`; the difference is which timeline jobs advance their
individual `System.JobAttempt`:

- **Rerun the entire stage** — every job re-runs, including the Helix submitter
  jobs, so the current attempt already contains a fresh incarnation of every
  logical work stream. Previous-attempt incarnations are superseded and need no
  resubmission.
- **Retry failed jobs in the stage** — only selected failed jobs advance their
  job attempt. If the Helix
  submitter jobs passed and only the monitor failed (e.g. it timed out), the
  monitor advances but the submitters remain at their original job attempts, so
  the current stage attempt contains **no** fresh Helix work. Naively scoping to
  the current stage attempt would make the monitor exit
  immediately as a success, silently discarding every result and failure from the
  previous attempt.

Because of the second gesture, "current-attempt scope" is not the same as
"ignore previous attempts." The monitor scopes *gating* to the current attempt
but reconciles previous-attempt work into it by resubmission (§2.3), deciding
per logical work stream by comparing the Helix job's submitter `System.JobAttempt`
with the matching current timeline record.

The monitor's stage and job attempts are provided as inputs (see §3), defaulting
to `SYSTEM_STAGEATTEMPT` and `SYSTEM_JOBATTEMPT`.

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
stream is identified by the submitter chain key (§5.7). The key combines the
stage identity, stable AzDO phase identity, Helix queue, and logical Helix job identity; the
AzDO job name and queue alone are not unique because one AzDO job may submit
multiple independent Helix jobs to the same queue. The combined identity is
stable across both stage attempts and monitor resubmissions.

1. Retry runs exactly once on entry to a **retried monitor job**
   (`System.JobAttempt > 1`). The initial monitor invocation reports failures
   but never creates additional Helix work.
2. The set of work to resubmit is decided from a single Helix snapshot taken on
   entry. Work that fails after the monitor has started is not resubmitted during
   the current invocation; a later invocation may pick it up.
3. Retry decisions are made per work stream from its **latest incarnation across
   all attempts**, ordered by stage attempt, submitter job attempt, explicit
   lineage depth, and finally Helix job ID. Let *L* be that incarnation:
   - *L* belongs to the **current stage attempt** — leave it, whether running or
     already failed. It belongs to the execution currently being monitored.
   - The current timeline shows the matching submitter at a **higher job
     attempt** than *L* — leave *L*. The submitter reran and superseded it; wait
     for the newer submitter execution rather than duplicating it. Until the
     replacement Helix job becomes visible, *L* is also excluded from current
     status and pass/fail reconciliation so stale failures do not leak through
     the visibility gap.
   - The current timeline submitter attempt **equals** *L*'s
     `System.JobAttempt` — the submitter did not rerun. Failed or unfinished
     work in *L* may be resubmitted into the current stage attempt.
   - *L* is **completed and fully passed** — nothing to resubmit; its results are
     uploaded (if not already, §2.4) and its outcome counted. It is terminal, so
     it does not block completion.
   - Missing or incompatible submitter identity / `System.JobAttempt` metadata
     makes retry classification ambiguous. Do not guess; record an actionable
     failure rather than risk duplicating a rerun.
   - A needed resubmission is **not possible** (e.g. the queue was removed, so the
     work can never run again) — for previous-attempt in-flight work, whose
     failure is not otherwise recorded, surface it as an actionable hard failure
     so the invocation fails fast rather than waiting forever. (Completed-with-
     failures work that cannot be resubmitted already fails the build via outcome
     reconciliation, §2.5.)
4. Every resubmission is stamped with the **monitor's current stage attempt**,
   preserves the original submitter's `System.JobAttempt`, records the
   resubmitting monitor attempt as `JobMonitor.JobAttempt`, and links back via
   `PreviousHelixJobName`.
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

1. **Retry-failed-jobs where only the monitor re-ran.** The stage and monitor
   attempts advance, but the submitters retain their original job attempts, so
   the current stage attempt contains no fresh Helix work. Naive scoping
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
4. **Visibility race during a full-stage rerun.** The timeline already shows the
   submitter at a newer job attempt, but its new Helix job is not visible yet.
   Blindly replaying the old job duplicates the rerun. → Timeline job attempt,
   not Helix visibility, suppresses replay.
5. **Rerun duplicates that are not lineage-linked.** A stage rerun's fresh Helix
   job has no `PreviousHelixJobName` link to its previous-attempt counterpart;
   they collapse only by chain key. → Outcome ordering breaks ties toward the
   higher stage/job incarnation so the current attempt wins (§5.7).
6. **Un-resubmittable work (e.g. purged queue).** Previous-attempt work that can
   never run again would loop forever under any "just wait" or "just resubmit and
   wait" scheme. → Resubmission-not-possible is treated as an actionable hard
   failure so the invocation fails fast instead of hanging (§2.3.3).
7. **One AzDO job submits multiple Helix jobs to the same queue.** Grouping only
   by AzDO job name and queue incorrectly merges independent streams. A failure
   in one job can be overwritten by a same-named passing work item in another,
   and retry can select only one of the jobs. → The stream key also includes the
   submitter-assigned logical Helix `jobName` (falling back to `TestRunName`).
   Resubmissions preserve that property, so incarnations of one logical job
   still chain while sibling Helix jobs remain independent.
8. **Identical phase/queue/logical-job names in different stages.** → The stage
   identity is part of the stream key, so outcomes and retry decisions cannot
   cross stage boundaries.

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
| Job attempt | Attempt of the monitor's own AzDO job. Retry reconciliation runs only after attempt 1. Defaults to `SYSTEM_JOBATTEMPT`. |
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
  submitter (see §5.1). Discovery sends both the source and
  `Properties[BuildId]` filters to Helix. Filtering `BuildId` only after
  retrieving the source's jobs is prohibited: non-PR sources such as
  `refs/heads/main` are long-lived and can contain thousands of historical
  jobs. The returned set spans every attempt of the build; the
  runner keeps the whole stage's jobs (all attempts) so the retry pass can
  reconcile previous-attempt work (§2.3), and classifies each job as
  current- or previous-stage-attempt via `System.StageName` /
  `System.StageAttempt`, and compares `System.JobAttempt` with the matching
  timeline submitter for retry eligibility (§2.1).
- **List work items for a job** — return all work-item summaries.
- **Download test results** — given one job/work-item pair, download recognized
  result files into a working directory. Individual
  per-file failures must not prevent the remaining files from being attempted.
  After the batch, transient failures cause the read-only download phase to be
  retried; permanent failures are logged and omitted.
- **Cancel a job** — best-effort cancellation.
- **Resubmit failed work items** — given the original job and a set of
  failed (or unfinished) work items, submit a new Helix job that contains only
  those items. The new job must inherit the original's submitter identity (stage,
  job name, display name, test-run name, queue, and submitter job attempt), be
  stamped with the **resubmitting monitor's current stage attempt** and
  `JobMonitor.JobAttempt` (§2.3.4), and link back via `PreviousHelixJobName`.
  May return "not possible" (e.g. queue gone), which the
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

1. Take one current AzDO timeline snapshot and one Helix snapshot of the whole
   stage (all attempts).
2. Reduce it to the latest incarnation of each logical work stream (§2.3.3):
   the leaf of each lineage chain, keyed by logical stream key, ordered by stage
   attempt, submitter job attempt, lineage depth, then Helix job ID.
3. For each latest incarnation, apply §2.3.3:
   - Initial monitor job attempt — do not retry anything.
   - Current-stage-attempt incarnation — leave it; it belongs to this execution.
   - Timeline submitter attempt is newer than the Helix submitter attempt —
     leave it; the submitter reran and superseded the old Helix work.
   - Previous-attempt, completed and fully passed — leave it (terminal); it will
     still be uploaded / reconciled by the poll loop.
   - Previous-attempt, completed with failures, or unfinished — ask the Helix
     service to resubmit the failed / not-yet-passed items, stamped with the
     current stage attempt while preserving the submitter job attempt (§2.3.4).
     If resubmission is not possible, record it
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
   uploaded (per §2.2), queue its completion-time work-item snapshot into the
   bounded upload pipeline. This pass is the only one that triggers uploads.
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

- A single AzDO matrix leg that fans out to multiple Helix queues must
  produce distinct keys (one per queue) so per-queue failures are
  preserved.
- A single AzDO job that submits multiple independent Helix jobs to the same
  queue must produce distinct keys (one per logical Helix job). The AzDO
  phase/job name and queue are therefore necessary but not sufficient.
- An original Helix job and its resubmission(s) on the same queue must
  produce the same key so the latest incarnation overwrites the older one.
- The preferred key components are:
  1. `System.StageName`;
  2. `System.PhaseName`, falling back to `System.JobName`;
  3. the Helix queue;
  4. the submitter-assigned Helix `jobName`, falling back to `TestRunName`.
  If no stable logical-job discriminator is available, the key is bound to the
  root Helix job in the `PreviousHelixJobName` lineage rather than risk merging
  unrelated jobs.
- Because these key components are stable across stage attempts, a
  rerun-stage incarnation of the same logical job on the same queue collapses
  onto the same key as its previous-attempt counterpart, even though the two
  Helix jobs are **not** linked by
  `PreviousHelixJobName` (only monitor resubmissions set that link). The map
  must therefore let the later incarnation win when two incarnations share a
  key: outcomes are applied in order of lineage depth, stage attempt, submitter
  job attempt, then Helix job ID, rather than by Helix job-name sort alone, or a stale previous-attempt outcome
  could nondeterministically overwrite the current one. `System.JobName` is
  used only when `System.PhaseName` is unavailable because some pipelines stamp
  every matrix job with `System.JobName=__default`.
- If lineage cannot be resolved (the predecessor link points outside the jobs
  the runner has observed), the root predecessor name provides the
  Helix-job-bound fallback.
- The submitter must assign different `jobName` values (or, when absent,
  different `TestRunName` values) to independent Helix jobs submitted by the
  same AzDO phase to the same queue. Without a stable distinguishing property,
  no monitor can reliably correlate an unlinked stage-rerun job with its prior
  incarnation while also distinguishing it from a sibling submission.

The same key drives a parallel map of "failed work item console info" used
to build the final failure report. When a later incarnation of a work item
passes, its entry in that map is cleared.

#### 5.7.1 Stream and attempt examples

The following examples use one logical stream:

```text
StreamKey K1 =
(
  StageName: Build,
  PhaseName: build_windows_x64_Checked_NativeAOT,
  QueueId: windows.10.amd64.open.rt,
  LogicalJobName: runtime-tests
)

Original Helix job H1:
  System.StageAttempt = 1
  System.JobAttempt = 1

Monitor replay R2:
  System.StageAttempt = 2
  System.JobAttempt = 1
  JobMonitor.JobAttempt = 2
  PreviousHelixJobName = H1

Fresh submitter rerun H2:
  System.StageAttempt = 2
  System.JobAttempt = 2
  PreviousHelixJobName is absent
```

`System.JobAttempt` on a Helix job always identifies the AzDO job that
originally submitted that logical work. It is compared with the current
timeline attempt of that same submitter. It is **not** compared with the
monitor's own job attempt. `JobMonitor.JobAttempt` records which monitor
invocation created a replay and is diagnostic metadata only.

`System.PhaseName` maps to the `refName` of the AzDO **Phase** timeline
record. When phase identity is unavailable, `System.JobName` maps to the
`refName` of the nested **Job** record. In matrix pipelines that nested job
name is frequently `__default`, which is why phase identity is preferred.

#### 5.7.2 Retry and rerun permutation matrix

| Scenario and observation time | Monitor | Current submitter timeline | Visible Helix work | Required behavior |
| --- | --- | --- | --- | --- |
| Initial execution; H1 fails | S1/M1 | J1 | H1 S1/J1 failed | Report the failure; attempt 1 never creates replay work. |
| Selective retry of only the monitor | S2/M2 | J1 completed | H1 S1/J1 failed | `J1 == J1`; replay H1 as R2. |
| Selective retry also selected the submitter, before H2 is visible | S2/M2 | J2 pending/running | Only H1 S1/J1 | `J2 > J1`; suppress H1 replay, status, and outcome using timeline state, without waiting for H2 visibility. |
| Selective retry after H2 is visible | S2/M2 | J2 | H1 S1/J1 and H2 S2/J2 | H2 supersedes H1; observe H2. |
| Full-stage rerun immediately after timeline creation | S2/M2 | J2 pending | Only H1 S1/J1 | Suppress H1 replay, status, and outcome because the submitter is part of the rerun. |
| Full-stage rerun after H2 submission | S2/M2 | J2 running/completed | H1 S1/J1 and H2 S2/J2 | H2 is authoritative; do not create a monitor replay. |
| Submitter reruns but intentionally submits no new Helix work | S2/M2 | J2 completed | Only H1 S1/J1 | Suppress H1; the newer submitter execution is authoritative. |
| Submitter did not rerun and H1 passed | S2/M2 | J1 | H1 S1/J1 passed | Upload/count H1; no replay. |
| Submitter did not rerun and H1 is failed or unfinished | S2/M2 | J1 | H1 S1/J1 failed/waiting | Replay the failed or unfinished items as R2. |
| R2 fails and the monitor is retried again | S3/M3 | J1 | H1 S1/J1 and R2 S2/J1 failed | R2 is the latest lineage leaf and still matches J1; replay its remaining failures as R3. |
| R2 remains unfinished when the monitor is retried | S3/M3 | J1 | R2 S2/J1 running/waiting | Treat the previous-stage leaf as abandoned and replay its unfinished items as R3. |
| R2 passed before the next monitor invocation | S3/M3 | J1 | R2 S2/J1 passed | Latest incarnation passed; no further replay. |
| Current-stage H2 is running when the monitor starts | S2/M2 | J2 | H2 S2/J2 running | Observe and gate on H2; never replay current-stage work on entry. |
| H2 fails after the monitor has started | S2/M2 | J2 | H2 changes running to failed | Report failure. Retry is entry-only; do not create replay work mid-invocation. |
| Monitor is retried after H2 failed | S3/M3 | J2 | H2 S2/J2 failed | `J2 == J2`; replay H2 into S3. |
| Submitter identity or attempt is missing | S2/M2 | Unknown | H1 S1/J1 failed | Fail safely; do not speculate and risk duplicate work. |
| Timeline attempt is lower than Helix metadata | S2/M2 | J1 | H1 claims S1/J2 | Treat metadata as inconsistent; do not replay. |
| Fresh H2 is visible but is not lineage-linked to H1 | S2/M2 | J2 | H1 S1/J1 and H2 S2/J2 | Collapse by `StreamKey`; the higher stage/job incarnation wins. |
| Monitor replay R2 is linked to H1 | S2/M2 | J1 | H1 and R2 with `Previous=H1` | Collapse explicit lineage to the leaf R2. |

The central retry decision is therefore:

```text
current submitter attempt > Helix System.JobAttempt
    => the submitter reran; suppress monitor replay

current submitter attempt == Helix System.JobAttempt
    => the submitter did not rerun; failed/unfinished work may be replayed
```

#### 5.7.3 Identity-isolation matrix

| Helix submissions | Example stream keys | Required behavior |
| --- | --- | --- |
| One AzDO job submits two logical jobs to the same queue | `(Build, phaseA, queue1, runtime-tests)` and `(Build, phaseA, queue1, nativeaot-smoke)` | Separate streams because logical job names differ. |
| One AzDO job submits to two queues | `(Build, phaseA, queue1, tests)` and `(Build, phaseA, queue2, tests)` | Separate streams because queues differ. |
| Two AzDO phases use the same queue and logical name | `(Build, phaseA, queue1, tests)` and `(Build, phaseB, queue1, tests)` | Separate streams because phase identities differ. |
| Two stages use identical phase, queue, and logical names | `(Build, phaseA, queue1, tests)` and `(Test, phaseA, queue1, tests)` | Separate streams because stage identities differ. |

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

### 5.9 Test-result durability

- Terminal work items may publish results before their containing Helix job
  completes.
- A Helix job is durably processed only after all admitted results are
  published and its Azure DevOps test run is completed and tagged with the
  Helix job name.
- A monitor that stops before that durable boundary leaves the job eligible
  for replay by a later invocation.
- Failed processed test results contribute to the monitor exit code even when
  the Helix work-item exit code is successful.
- Result publication may retry transient failures and therefore accepts the
  documented duplicate-result risk rather than losing an entire result set.

Channel structure, concurrency, retry budgets, and the concrete upload
sequence are implementation details documented in
[Upload pipeline](Components/UploadPipeline.md) and
[Test-result processing](Components/TestResults.md).

### 5.10 Status logging

When a status log is due, the runner emits a one-line summary of work
counts (processed / completed / running / waiting jobs and work items).
Verbose mode adds bounded pipeline diagnostics (queued/active work and uploaded
result totals) but never emits a per-work-item tree.

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
