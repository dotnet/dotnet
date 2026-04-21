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

### Specific Test Assembly

Use `-p` to filter by assembly name pattern:

```bash
# Linux / macOS
./test.sh -p htmllogger       # HTML logger tests
./test.sh -p trxlogger        # TRX logger tests
./test.sh -p datacollector    # Data collector tests
./test.sh -p smoke            # Smoke tests

# Windows (-p is ambiguous in PowerShell; use -projects)
./test.cmd -projects htmllogger
./test.cmd -projects smoke
```

### Specific Test by Name

```bash
# Windows
./test.cmd -bl -c release /p:TestRunnerAdditionalArguments="'--filter TestName'" -Integration

# Linux / macOS
./test.sh -bl -c release /p:TestRunnerAdditionalArguments="'--filter TestName'" --integrationTest
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

| Category | Speed | What it tests | Filter |
|---|---|---|---|
| Unit tests | Fast | Individual units | `./test.sh` / `./test.cmd` (default) |
| Smoke tests | Slow | P0 end-to-end scenarios | `-p smoke` |
| Acceptance tests | Slowest | Extensive coverage | `--integrationTest` / `-Integration` flag |

## Troubleshooting

- **OS mismatch errors:** If you see SDK load failures, run the mismatch detection script above to clean and re-bootstrap.
- If build fails asking for .NET 4.6 targeting pack, install it from [Microsoft Downloads](https://www.microsoft.com/download/details.aspx?id=48136)
- Enable verbose diagnostics: see `docs/diagnose.md`
- For debugging, add `Debugger.Launch` at process entry points (testhost.exe, vstest.console.exe)
