# Isolated TaskHost nested-callback diagnostic

This opt-in diagnostic ports the finalized file-polled local graph without
changing the file-gate or snapshot-logger semantics. It is **not a performance run**.
A read-only observer additionally inspects existing handler stacks and packet
queues after 20 seconds. It does not establish the identity of the original
full-VMR failure. Four successful full-graph MT follow-ups do not
establish an outcome for this smaller graph.

## Launch

In definition **303**, manually run the private branch with
`experimentTaskHostProbe: true`. Leave the other experiment parameters at their
defaults. This compile-time switch includes exactly one stage,
`TaskHostReentrancyProbe`, with one `Linux_x64` job; it omits ordinary VMR,
mutex-probe and `ValidateUserChanges` jobs even if other switches are supplied.
Default `false` preserves the existing pipeline, including the normal
PR-only `ValidateUserChanges` condition (false for a manual private-branch run).
The existing build time limits are untouched.

The job uses the usual Linux pool, no container, shallow/blob-filtered checkout,
and the unchanged `eng/common/tools.sh` SDK bootstrap. Its existing public-feed
then `https://ci.dot.net/public` fallback is used without adding feeds.
No source-built SDK or Windows SDK payload is substituted.

Direct invocation on **Linux x64**, from the checkout:

```sh
python3 eng/taskhost-reentrancy-probe/run.py \
  --sdk /absolute/private/sdk \
  --output /absolute/artifact-staging/taskhost-reentrancy-probe \
  --tools /absolute/private/diagnostic-tools
```

Use a fresh output directory **outside the checkout**, separate from the SDK.
Bootstrap logs and pre-existing numeric binlogs are permitted; numbering
continues from the greatest existing numeric binlog. Payload staging into
`output/harness` and local empty `Directory.Build.props/targets` prevent VMR
ancestor imports. The exact pin is SDK `11.0.100-rc.1.26420.103`, product commit
`1d599674e31ad86ca9e6e1dad7055b531e4d58e4`, with SDK roll-forward disabled.
Actual `--version`, `.version` commit/RID, `--info`, relevant SDK/source hashes
and pipeline build/source/job identity are retained separately.

The supervisor reuses the existing `vmr-perf-experiment.py` procfs, descendants,
diagnostic-command and setup helpers. It validates the existing pinned
dotnet-stack package version/hash and extracted assembly; missing tools are
reported and prepared. It does not install global tools or change the agent.
Only allowlisted, non-secret environment settings enter the build, with private
CLI home, package cache and process-file directories. Ambient `MSBUILD*` flags
are absent except `MSBUILDDISABLENODEREUSE=1`.
Development-certificate generation and global-tool PATH setup are disabled.

## Graph and observation

Root builds two wrappers concurrently. Both request distinct targets in the
same `P.proj` configuration. Unmarked tasks A and B call
`IBuildEngine.BuildProjectFile`. Marked, yielding file gates ensure B starts
while A is outstanding. B's child then depends on the parent's A result.
All gate paths are normalized real children of the absolute `RunDir`.
The gates poll files, never a watcher or time-based successful release.

`PacketObserverTask` is marked multi-threadable and captures its existing
BuildEngine host before the proxy becomes inactive. Its bounded traversal reads
only allowlisted existing component fields; it never invokes a component factory,
changes engine state, dumps environment values, or takes a memory dump.
`packet-ownership-*.json` identifies the top handler, task roles/contexts and
queued packet types. The proof condition requires B on top with A's successful
child-return message and a successful task-completion packet, while A's queue is
empty. Observer errors are explicit and make the observation inconclusive.

The harness compiles once, then runs normal, MT1 and MT2 in the same job.
Each graph command uses `/m:2 /nr:false`; `-mt` is the only mode switch.
Both modes use the identical snapshot logger and diagnostics. Logger arguments
remain single argv items, including their semicolons.

Every build has a 120-second observation bound. Owned-process managed stacks
are attempted around 20 seconds and at 96 seconds (before the bound), with
6 seconds per process and 18 seconds per round. Stack attachment failures
remain visible in their reports; they are not fabricated successes.
Collector overhead is separately clocked, but observation time includes that
overhead and must not be interpreted as benchmark time.

Finally cleanup runs on success, timeout, signals and exceptions: short
SIGINT/SIGTERM grace periods, then SIGKILL only for still-owned PIDs whose
procfs start identities match. No name-based or process-group killing, process
environment capture, memory dumps, or unrelated-process stack collection.
Core dumps are disabled only for the supervisor and its children, not the agent
machine. Cleanup must succeed before another run starts.

## Outcomes and artifacts

`TaskHostReentrancyProbe` is published with `always()` from an explicit evidence
allowlist, not the private working directory. CLI home, package caches, process
scratch files, and staged build outputs are never published. The publication
manifest records copied paths and hashes. Keep the full published artifact:
stage clock, exact argv/cwd, identity/hashes, per-run stdout/stderr/build logs,
trace/gate files, process snapshots/history, stacks, cleanup and result JSON.
`observation-evidence.json` freezes markers **before cancellation**.

* **control-passed**: natural exit 0, root/both parent targets/both nested calls
  completed, all gates returned true, and the real-event snapshot finalized.
* **observed-reproduction**: still running at 120 seconds with all gates
  completed; A returned after B entered in the same observed TaskHost
  (`/nodemode:2`); B, both parents and root remained outstanding. This labels
  that structural stall. The separate `packet_misrouting_proved` field requires
  the read-only queue evidence above. Inspect stacks and the snapshot binlog
  independently before attributing the original full-VMR instance.
* **non-reproduction**: MT naturally completed with all the control markers.
  This is a legitimate result, not a false claim that the hypothesized issue
  was reproduced or disproved.
* **inconclusive**: other timeouts/exits, incomplete evidence, inconsistent
  repetitions, setup/control/cleanup failure.

Supervisor exit 0 means both MT runs yielded the same valid experimental
outcome, **not that a timed-out MSBuild invocation passed**. Each run retains
`cli_succeeded`, `timed_out`, `observed_exit_code` (null while blocked), and
`post_cleanup_exit_code` separately. Invalid/inconsistent experiments exit 2.

`snapshot.binlog` is a finalized prefix of **real events only**, frozen after
15 seconds or normal shutdown, with its status file. It does not synthesize
BuildFinished or TaskFinished. The ordinary numeric binlog may be incomplete
after forced termination; never treat it, or unclosed event durations in the
snapshot, as completed-build/performance proof.

The XML/C# payload can be compiled and controlled locally on Windows with the
same exact SDK, but this Linux supervisor deliberately rejects Windows.
Local syntax/structure and Windows control checks are not Linux execution
validation. The server template preview and actual Linux job remain separate
validation steps for the pipeline owner.
