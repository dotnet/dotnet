# AGENTS.md — vstest

This file contains project-specific knowledge for AI agents working on the VSTest test platform.

## Repository Overview

VSTest is the test platform that powers `dotnet test`, Visual Studio Test Explorer, and Azure DevOps test tasks. It discovers and executes tests written with MSTest, xUnit, NUnit, and other frameworks.

### Architecture

```
vstest.console (entry point)
  ├── TestRequestManager (orchestration)
  │   ├── ProxyDiscoveryManager ──IPC──► testhost (discovery)
  │   ├── ProxyExecutionManager ──IPC──► testhost (execution)
  │   └── ProxyOperationManager (shared IPC logic)
  ├── Translation Layer (bridges old/new handler interfaces for VS/AzDO)
  └── Data Collectors (code coverage, blame, etc.)
```

**Key IPC boundary**: vstest.console and testhost communicate via JSON-RPC over stdin/stdout. Wire format changes must be backward-compatible.

### Key Directories

| Directory | Purpose | Sensitivity |
|---|---|---|
| `src/Microsoft.TestPlatform.ObjectModel/` | Public API surface (NuGet-shipped) | Binary compat critical |
| `src/Microsoft.TestPlatform.CommunicationUtilities/` | JSON-RPC protocol, serialization | Wire compat critical |
| `src/Microsoft.TestPlatform.CrossPlatEngine/` | Execution engine, parallel scheduling | Thread safety critical |
| `src/Microsoft.TestPlatform.CoreUtilities/` | Shared utilities | Hot-path perf critical |
| `src/vstest.console/` | CLI entry point, arg parsing, app.config | Binding redirects |
| `src/testhost*/` | Test host processes | Assembly loading |
| `src/datacollector/` | Data collector host | Binding redirects |
| `src/Microsoft.TestPlatform.Client/` | Client-side test management | |
| `src/Microsoft.TestPlatform.Common/` | Shared platform logic | |
| `src/Microsoft.TestPlatform.Extensions.*/` | Loggers (HTML, TRX, blame) | |

## Build

| Action | Windows | Linux / macOS |
|---|---|---|
| Restore + Build | `./build.cmd` | `./build.sh` |
| Build + Pack | `./build.cmd -pack` | `./build.sh --pack` |
| Release config | `./build.cmd -c Release -pack` | `./build.sh -c Release --pack` |
| Unit tests | `./test.cmd` | `./test.sh` |
| Specific tests | `./test.cmd -projects <pattern>` | `./test.sh -p <pattern>` |
| Several test projects | `./test.cmd -projects "test\A\A.csproj;test\B\B.csproj"` | `./test.sh -p "test/A/A.csproj;test/B/B.csproj"` |
| Smoke tests | `./test.cmd -projects smoke` | `./test.sh -p smoke` |
| Single test by name | `./test.cmd -bl -c release /p:TestRunnerAdditionalArguments="--filter TestName"` | Similar |

CI builds use `-c Release`. Always build with Release config before submitting PRs.

The `.cmd` wrappers pass arguments to PowerShell literally. See [Wrapper scripts pass arguments literally](#wrapper-scripts-pass-arguments-literally) before changing one.

## Test Structure

Test projects mirror source projects under `test/`:

```
src/Microsoft.TestPlatform.ObjectModel/     → test/Microsoft.TestPlatform.ObjectModel.UnitTests/
src/Microsoft.TestPlatform.CrossPlatEngine/ → test/Microsoft.TestPlatform.CrossPlatEngine.UnitTests/
src/vstest.console/                         → test/vstest.console.UnitTests/
```

Test categories: Unit (fast, default), Smoke (P0 e2e), Acceptance (full e2e with `--integrationTest`).

## Known Gotchas

### Wrapper scripts pass arguments literally

`build.cmd`, `test.cmd`, `restore.cmd`, `open-vs.cmd`, `open-code.cmd`, and `eng/RestoreInternal.cmd` invoke PowerShell with `-File`, so everything after the script path reaches the target script as a literal argument. The first five call `eng/build.ps1`, `eng/RestoreInternal.cmd` calls `eng/common/build.ps1`.

They previously used the form `-command "& """<script>""" %*"`, which spliced `%*` into a string that PowerShell then parsed as source code. That caused two problems, both fixed:

- `;` in an argument became a statement separator. `./test.cmd -projects "test\A\A.csproj;test\B\B.csproj"` ran `Build.ps1 -projects test\A\A.csproj` and then executed `test\B\B.csproj` as its own statement. On Windows `.csproj` is file-associated with Visual Studio, so every entry after the first opened a full IDE. Three agents ran this form at the same time and opened eighteen instances of Visual Studio.
- Exit codes collapsed to `1`. `eng/build.ps1` ends with `exit $LastExitCode` to forward the real code, but `-command` discarded it, so `8` (filter matched no tests) and every other code arrived as `1`.

Use `-File` in any new `.cmd` wrapper. Do not switch back to `-command`.

Because arguments are no longer re-parsed, MSBuild properties need one level of quoting instead of two:

```
./test.cmd -bl -c release /p:TestRunnerAdditionalArguments="--filter TestName"
```

`eng/common/*` comes from Arcade and still uses `-command`. Do not edit those files here; fix them in the Arcade repository.

Independent of the wrappers: never run a `.csproj` or `.sln` path as a command. The path is always an argument, as in `dotnet test <path>.csproj`.

### Binding Redirects

Bumping a `netstandard2.0` package cascades: transitive deps need binding redirects in ALL three app.configs (`vstest.console/app.config`, `testhost.x86/app.config`, `datacollector/app.config`). Miss one and you get `FileLoadException` in net462 DTA hosts.

### Package Verification

After packaging changes, regenerate `eng/expected-nupkg-file-counts.json` and `eng/expected-dll-frameworks.json` from a clean Release build. Never hand-edit these files.

### Assert.Contains

`Assert.Contains(expected, actual)` — first param is the needle. This is the opposite of old StringAssert. This has been a recurring mistake.

### Localization

`*.xlf` files must be manually edited to match `.resx` changes. They are NOT auto-generated by the build.

### CI

- CI runs on Azure DevOps, not GitHub Actions
- `DOTNET_ROLL_FORWARD=LatestMajor` masks version mismatches — don't rely on it
- Doc-only PRs skip CI builds
- Windows builds finish first (~15 min), macOS/Ubuntu take longer

### Git Workflow

- Never commit to `main`
- Never force-push PR branches — squash-merge at the end
- Push to fork remote, PR against `microsoft/vstest`
- Don't create draft PRs — undrafting forces a rebuild

### Agentic Workflows (gh-aw)

- Use `gh aw secrets set` to manage secrets, NOT `gh secret set`. Plain `gh secret set` creates the repo secret but gh-aw can't see it.
- **Auth is company-token first — no long-lived personal PATs.** Copilot inference uses the `copilot-requests: write` permission (billed to the org Copilot subscription), so `COPILOT_GITHUB_TOKEN` is no longer referenced by any compiled workflow. Write-backs use an org-owned GitHub App (`APP_ID` variable + `APP_PRIVATE_KEY` secret) with `ignore-if-missing: true`, falling back to `GITHUB_TOKEN` until an org admin provisions it. See [`.github/workflows/README.md`](.github/workflows/README.md) for the full secrets table and rationale (the Microsoft OSS enterprise now 403s fine-grained PATs older than 8 days).
- `lockdown:` has been removed repo-wide (deprecated upstream); workflows keep `min-integrity: none` and use the default `GITHUB_TOKEN` for MCP reads.
- Workflow source files are `.md` in `.github/workflows/`. Compiled `.lock.yml` files are generated — don't hand-edit them.
- To recompile after editing a workflow: `gh aw compile` from the repo root.
- `.github/*` and `AGENTS.md` are excluded from CI path triggers — editing workflows won't trigger a full build.
