---
name: vstest-build-test
description: Build, test, and validate changes in the vstest repository. Use when building vstest projects, running unit tests, smoke tests, or acceptance tests, or when deploying locally built vstest.console for manual testing.
---

# Building and Testing vstest

## Pre-Build: Environment Setup

Before building, verify the `.dotnet` toolchain matches the current OS. The repo bootstraps its own .NET SDK into `.dotnet/`.

### Detect OS vs .dotnet Mismatch

Run this check **before every first build in a session**:

```bash
# Determine current OS
OS=$(uname -s)   # "Linux", "Darwin" (macOS), or contains "MINGW"/"MSYS" (Windows/Git Bash)

if [ -d ".dotnet" ]; then
  if [ "$OS" = "Linux" ] || [ "$OS" = "Darwin" ]; then
    # On Linux/macOS the dotnet binary must be an ELF/Mach-O executable, not .exe
    if [ -f ".dotnet/dotnet.exe" ] && [ ! -f ".dotnet/dotnet" ]; then
      echo "MISMATCH: .dotnet contains Windows binaries but OS is $OS"
      rm -rf .dotnet .packages artifacts
      echo "Cleaned .dotnet, .packages, and artifacts for fresh bootstrap"
    fi
  else
    # On Windows the dotnet binary should be dotnet.exe
    if [ -f ".dotnet/dotnet" ] && [ ! -f ".dotnet/dotnet.exe" ]; then
      echo "MISMATCH: .dotnet contains Linux/macOS binaries but OS is Windows"
      rm -rf .dotnet .packages artifacts
      echo "Cleaned .dotnet, .packages, and artifacts for fresh bootstrap"
    fi
  fi
fi
```

After cleanup (or if `.dotnet` doesn't exist), the build script automatically downloads the correct SDK version from `global.json`.

## Build

### Platform Commands

| Action | Windows | Linux / macOS |
|---|---|---|
| Restore + Build | `./build.cmd` | `./build.sh` |
| Restore only | `./restore.cmd` | `./restore.sh` |
| Build + Pack | `./build.cmd -pack` | `./build.sh --pack` |
| Release config | `./build.cmd -c Release -pack` | `./build.sh -c Release --pack` |
| Single project | `./build.cmd -project <csproj>` | `./build.sh --projects <csproj>` |

### Full Build (Recommended)

For projects with many cross-project dependencies (e.g., HtmlLogger, TrxLogger, vstest.console):

```bash
# Linux / macOS
./build.sh --pack

# Windows
./build.cmd -pack
```

This produces NuGet packages under `artifacts/packages/Debug/Shipping/`.

### Single Project Build

For isolated projects with few dependencies:

```bash
# Linux / macOS
./build.sh --projects <path-to-csproj>

# Windows
./build.cmd -project <path-to-csproj>
```

> **Warning:** This does NOT work for projects like HtmlLogger that have many transitive dependencies. Use `--pack` / `-pack` instead.

## Test

### Unit Tests (Default)

```bash
# Linux / macOS
./test.sh

# Windows
./test.cmd
```

### Specific Project(s)

`-projects` / `--projects` takes a **resolvable path or glob** — it is passed through
`Resolve-Path`, so a bare project nickname or category (e.g. `smoke`, `htmllogger`) fails with
`Cannot find path`. Point it at the csproj(s):

```bash
# Windows
./test.cmd -projects "test\**\*HtmlLogger*\*.csproj"

# Linux / macOS
./test.sh --projects "test/**/*HtmlLogger*/*.csproj"
```

For a single project you can also build+test its csproj directly with the bootstrapped SDK:

```bash
# Windows
./.dotnet/dotnet.exe test test/Microsoft.TestPlatform.Extensions.HtmlLogger.UnitTests/*.csproj -c Debug

# Linux / macOS
./.dotnet/dotnet test test/Microsoft.TestPlatform.Extensions.HtmlLogger.UnitTests/*.csproj -c Debug
```

### Test Categories (smoke / integration / performance / compatibility)

These are **switches** handled by `eng/build.ps1` — NOT `-projects` values:

```bash
# Windows
./test.cmd -smokeTest           # TestCategory=Smoke (a subset of integration tests)
./test.cmd -integrationTest     # full acceptance / integration suite
./test.cmd -performanceTest
./test.cmd -compatibilityTest

# Linux / macOS use the same switch names
./test.sh -smokeTest
```

> `-smokeTest` and `-integrationTest` are mutually exclusive (smoke is a subset); passing both throws.

### Filter by Test Name

Use the `-filter` parameter. Do **not** pass `--filter` inside `TestRunnerAdditionalArguments` —
`eng/build.ps1` explicitly throws if you do.

```bash
# Windows
./test.cmd -integrationTest -filter "FullyQualifiedName~MyScenario"

# Linux / macOS
./test.sh -integrationTest -filter "FullyQualifiedName~MyScenario"
```

### Running integration / smoke tests locally (DOTNET_ROOT gotcha)

Integration and smoke tests self-host: they launch test-asset apphosts built against the repo's
preview TFM (e.g. `net11.0`). An apphost resolves its shared runtime from `DOTNET_ROOT`, falling
back to the machine-wide install (`C:\Program Files\dotnet`), which usually lacks the preview
runtime — so it fails instantly with *"You must install or update .NET to run this application."*

- `test.sh` (Linux/macOS) sets `DOTNET_ROOT` to the repo `.dotnet` automatically.
- `test.cmd` (Windows) does **not** — set it yourself before running:

```powershell
$env:DOTNET_ROOT = "$PWD\.dotnet"
${env:DOTNET_ROOT(x86)} = "$PWD\.dotnet\dotnet-sdk-x86"   # only if x86 test hosts run
$env:DOTNET_MULTILEVEL_LOOKUP = "0"
./test.cmd -smokeTest
```

## Manual Validation with vstest.console

After building with `--pack` / `-pack`, validate vstest.console changes by unzipping the built package:

1. Locate the package: `artifacts/packages/Debug/Shipping/Microsoft.TestPlatform.<version>-dev.nupkg`
2. Unzip it (`.nupkg` files are ZIP archives)
3. Run the local vstest.console against a test project

### Alternative: Direct Artifact Paths

- **xplat (netcoreapp):** `artifacts/<Configuration>/netcoreapp1.0/vstest.console.dll`
- **Windows desktop:** `artifacts/<Configuration>/net46/win7-x64/vstest.console.exe`

## Test Categories

| Category | Speed | What it tests | How to run |
|---|---|---|---|
| Unit tests | Fast | Individual units | `./test.cmd` / `./test.sh` (default) |
| Smoke tests | Slow | P0 end-to-end scenarios | `-smokeTest` switch |
| Acceptance / integration | Slowest | Extensive coverage | `-integrationTest` switch |

## Troubleshooting

- **OS mismatch errors:** If you see SDK load failures, run the mismatch detection script above to clean and re-bootstrap.
- **Integration/smoke tests fail instantly on Windows with "You must install or update .NET to run this application":** `test.cmd` does not set `DOTNET_ROOT`, so the self-hosted preview-TFM apphosts look in `C:\Program Files\dotnet` (which lacks the preview runtime). Set `$env:DOTNET_ROOT = "$PWD\.dotnet"` before running — see "Running integration / smoke tests locally".
- If build fails asking for .NET 4.6 targeting pack, install it from [Microsoft Downloads](https://www.microsoft.com/download/details.aspx?id=48136)
- Enable verbose diagnostics: see `docs/diagnose.md`
- For debugging, add `Debugger.Launch` at process entry points (testhost.exe, vstest.console.exe)
