# Investigation Techniques by Failure Category

Deep-dive investigation techniques for specific pipeline failure categories. Referenced from the categorization table in SKILL.md Step 2.

## ESRP Signing and Notarization Failures

ESRP (Enterprise Security Response Platform) handles code signing for all .NET shipping builds, including Apple notarization for macOS packages.

### Interpreting ESRP status codes

- **`FailDoNotRetry`** — Deterministic content failure. The binary content is wrong (unhardened, unsigned inner components, corrupt). Retrying won't help. Investigate which commit introduced the bad binary.
- **`FailCanRetry`** — Transient service issue. Retry the build.
- **`MacSignFailed: "Archive contains critical validation errors"`** — Apple rejected the .pkg because inner binaries lack hardened runtime or proper code signatures.

### Investigation technique

1. Download the signing binlog from the build artifacts (look for `NotarizationRound*.binlog`)
2. Search for `MacSignFailed`, `FailDoNotRetry`, or `notarization` to find the failing operation
3. If `FailDoNotRetry`: compare the source commits between a succeeding and failing build — a forward flow likely introduced an unhardened binary
4. If transient: check ESRP service health, retry the build

> ⚠️ ESRP cert catalog IDs (e.g., cert 8020) are internal identifiers. Omit them from public issues — they add no diagnostic value.

## Container OOM and Resource Exhaustion

Cross-compilation legs running in containers (e.g., `azurelinux-3.0-net11.0-cross-arm64`) can OOM when NuGet restore, build, or test phases exceed the container's memory limits.

### Signals

- **`exit code null`** on a Build task inside a container leg
- No explicit error in the log — process simply vanishes
- Often hits arm64 cross-compilation (high memory usage for cross-toolchain)

### Investigation technique

1. Check the container options in `vmr-build.yml` — look for `$(defaultContainerOptions)` which may only be `--privileged` with no memory tuning
2. Check the pool — `NetCore1ESPool-Internal` vs `NetCore-Public-XL` (different VM SKUs)
3. Look for restore/build parallelism that could spike memory
4. Options: larger VM SKU, `--memory` container limits, reduced restore parallelism (`NUGET_CONCURRENCY_LIMIT`), GC heap limits

### Key files in dotnet/dotnet

- `eng/pipelines/templates/variables/vmr-build.yml` — container images, pool names, variable defaults
- `eng/pipelines/templates/stages/vmr-verticals.yml` — vertical build job definitions
- `eng/pipelines/templates/jobs/vmr-build.yml` — job template where container options are applied

## crossgen2 Crash Investigation

crossgen2 produces ReadyToRun (R2R) images of SDK assemblies during the SDK layout step. Exit code 57005 (0xDEAD) is crossgen2's hardcoded fatal error sentinel.

### Investigation technique

1. Download the SDK build binlog from the build artifacts
2. Find the `CrossgenLayout` target and its `Crossgen` tasks — there are typically 100+ tasks
3. Identify which task failed and what assembly it was crossgen'ing
4. Check if crossgen2's stderr was captured (often it isn't — MSBuild Exec only records the exit code)
5. Without a crash dump or stack trace, diagnosis is limited. Check if runtime#124181 or similar tracking issues exist.

> The VMR build does not currently produce crash dumps for crossgen2 failures. Getting dump collection enabled is a prerequisite for root-causing these crashes.

## YAML Pre-Flight Failures

When a build fails with `startTime == finishTime` and the timeline returns HTTP 204 (No Content), the build was rejected during YAML validation before any agent was allocated.

### Investigation technique

1. Check `build.validationResults` in the build API response — it contains the YAML parsing error
2. Common causes: incompatible 1ES pipeline template parameters, container configuration the template doesn't support
3. These are always PR-specific — the PR author needs to fix their YAML

## Network Transient Failures

External resource downloads (wasi-sdk, NuGet packages, npm registries) can fail transiently due to network issues.

### curl / tar failures

- curl downloads a truncated response → tar fails with "not in gzip format"
- Fix: add `--retry 3 --retry-delay 5` to curl commands
- On Linux, curl may not have retry flags — check the script

### npm ci failures

- `ETIMEDOUT` / `ECONNREFUSED` are TCP-level failures
- npm's `--fetch-retries` only covers HTTP-level errors, not TCP connection timeouts
- Fix: shell-level retry wrapper around `npm ci` (try up to 3 times with delay)
