# Source-build crash replay

The diagnostic `eng/pipelines/pr.yml` is manual-only. It reuses the normal
`vmr-build.yml` job template, rather than the standalone MSBuild launcher.

A preparation job downloads the original stage-one SDK and
`Private.SourceBuilt.Artifacts` archive. Three independent replay jobs check out
the original complete VMR commit, restore its product outputs and manifests,
and execute the template's full `build.sh --test --source-only` phase using the
original source-build bootstrap SDK and package cache.

Each job repeats the full test phase ten times by default (`testRepetitions`).
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
intentional capture smoke test uses a separate SDK extraction under
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
