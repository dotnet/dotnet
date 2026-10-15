# Source-build crash replay

The diagnostic `eng/pipelines/pr.yml` is manual-only. It reuses the normal
`vmr-build.yml` job template, rather than the standalone MSBuild launcher.

A preparation job downloads the original stage-one SDK and
`Private.SourceBuilt.Artifacts` archive. Three independent replay jobs check out
the original complete VMR commit, restore its product outputs and manifests,
and execute the template's full `build.sh --test --source-only` phase using the
original source-build bootstrap SDK and package cache.

Replay1 retains the original container digest
`b06d7a274acbc1f5e7b2ae9eefeb8e73012708d79aff5c637b0321b7984a96d9`.
Replay2 and Replay3 use
`5629fb0d6a5d9c70de8479bb2b488c2845a7fa24b4e2e654c74aca21eacef6f5`,
the digest pulled by the same failing CentOS leg in normal build 1634439 on
October 15. All three jobs retain the original SDK, source commit, bootstrap
inputs, and test commands, isolating the container change rather than also
switching to that build's SDK. `replay-host.txt` records the kernel, CPU, memory,
resource limits, and installed RPM versions for comparing native dependencies.
The original crash also occurred with the older image, so this comparison does
not assume the newer image introduced the problem.

Each job runs the full test phase once by default (`testRepetitions`).
Repetitions stop on the first test-phase failure. Each invocation has a separate
orchestrator binlog, and scenario logs are preserved under
`artifacts/log/Release/repetitions/<iteration>`. The original binary-scan report
is restored from the original BuildLogs artifact so build-time validation inputs
are retained without rerunning product compilation.

`skipBuild: true` removes product compilation. The supported
`DotNetSourceOnlyTestOnly=true` validation mode also removes the scenario
harness's product-repository build dependencies. The normal build scripts still
build the test projects and generated sample projects. This mode uses bootstrap
Arcade for the scenario harness. Its shell-script toolset is populated from the
original prebuilt Arcade package, since source-built `-ci` versions cannot be
downloaded from NuGet. Validation's additional local package source includes
both shipping and nonshipping product packages, with missing packages supplied
from the original stage-one source-built package archive. Product packages take
precedence over bootstrap packages. This also supplies Arcade's source-built
tool dependencies: test-only mode does not add the normal prebuilt feeds.
The replay does not reconstruct the processes or cache contents left behind by
the original product build.

Full dumps use `COMPlus_*` settings so scenario subprocesses retain them. The
`enableDumpCapture=false` parameter omits all five capture settings from the
test environment and skips the intentional crash smoke test and command-loop
preflight. A preflight check rejects inherited `COMPlus_*` or `DOTNET_*`
dump/crash-report settings rather than silently running a contaminated control.
SDK checksum verification, inputs, container matrix, and full normal tests are
unchanged. With capture disabled, inspect the test logs and exit codes; an empty
dump summary does not establish that no process crashed.

`enableDumpCapture=false enableKernelCoreCapture=true` instead captures ordinary
Linux kernel cores without enabling runtime crash handling or running any managed
smoke/preflight commands. This mode is restricted to Azure CI workers. It saves
the worker's original `core_pattern`, verifies kernel capture using a small native
SIGQUIT smoke core under `kernel-check`, and directs subsequent cores to
`dumps/kernel.*.dmp`. An always-run finalizer restores the original kernel setting.
The container starts with unlimited core-size limits, but its sudo policy resets
both soft and hard limits to zero. Kernel mode temporarily installs
`Defaults rlimit_core=infinity` inside the job container, validates it with
`visudo`, and performs the native smoke check through the same `sudo -E
--preserve-env=HOME` boundary as the real tests. An always-run finalizer removes
this policy. `kernel-capture.txt` records the limits inside that sudo boundary,
not just those in the setup shell. Kernel cores have
no runtime crash report, so the summary marks them unclassified until a debugger
confirms the signal and crashing thread; a smoke core is not a reproduction.
Matching host/runtime/DAC binaries are published under `debugging-sdk`.
Kernel capture and runtime dump capture cannot be enabled together.

Three further independent experiments use `enableDumpCapture=false` and
`enableKernelCoreCapture=false`, retaining the normal test commands and skipping
the intentional managed crash/command-loop preflight:

| `diagnosticMode` | Captured evidence |
| --- | --- |
| `report-only` | Runtime JSON crash report with stacks, without a runtime dump. Still uses the runtime crash handler. |
| `eventpipe` | Per-process streaming `.nettrace` files under `traces`, with GC, exception, and sampling events. No runtime dump settings. |
| `verbose-dump` | Full runtime dumps and crash reports, plus verbose `createdump` diagnostics in test process output. |

The default `standard` mode preserves the existing capture/no-capture switches.
Alternative modes cannot be combined with either capture switch. Each alternative
publishes matching runtime/DAC binaries under `debugging-sdk` without executing
that SDK beforehand. `COMPlus_*` settings survive the scenario subprocess
environment filtering. Streaming traces can still lose the last events on a
crash and affect timing; a trace is not a substitute for a native crash stack.
Report-only JSON files are classified even when there is no corresponding dump.

The intentional capture smoke test uses a separate SDK extraction under
`$(Agent.TempDirectory)/sdk-crash-capture-sdk`, as well as separate dump, home,
and package-cache directories. The normal test phase alone extracts and runs
the actual test SDK; the smoke check does not prepopulate or start processes
from that SDK directory.
Each replay job's `*_BuildLogs_Attempt*` artifact is published unconditionally
and contains the logs, test results, dump reports, and matching runtime under
`artifacts/log/Release/dump-check/debugging-sdk`. Actual dumps are under
`artifacts/log/Release/dumps`.

`summarize-dumps.py` fails the job when a report identifies a crashed thread.
A diagnostic snapshot with no crashed thread (such as a snapshot of container
PID 1 in `pause`) is preserved but does not by itself count as a reproduced
crash. Dumps without reports are retained and explicitly marked unclassified.
