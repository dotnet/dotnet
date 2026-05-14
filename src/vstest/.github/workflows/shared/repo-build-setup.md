## Build Setup

The repository build is bootstrapped via `./build.sh` (Linux/macOS) or `./build.cmd` (Windows).

### Quick Reference

| Action | Linux / macOS | Windows |
|---|---|---|
| Build | `./build.sh` | `./build.cmd` |
| Build + Pack | `./build.sh --pack` | `./build.cmd -pack` |
| Release config | `./build.sh -c Release --pack` | `./build.cmd -c Release -pack` |
| Unit tests | `./test.sh` | `./test.cmd` |
| Specific tests | `./test.sh -p <pattern>` | `./test.cmd -projects <pattern>` |
| Smoke tests | `./test.sh -p smoke` | `./test.cmd -projects smoke` |

### Pre-Build: .dotnet Mismatch Detection

The repo bootstraps its own .NET SDK into `.dotnet/`. If `.dotnet/` contains binaries for the wrong OS (e.g. Windows binaries on Linux), delete `.dotnet/`, `.packages/`, and `artifacts/` before building.

### CI Configuration

- CI runs on **Azure DevOps**, not GitHub Actions.
- CI builds use `-c Release`.
- `DOTNET_ROLL_FORWARD=LatestMajor` masks version mismatches — always test on the CI runtime, not with roll-forward.

### Key Verification Files

After packaging (`-pack`), these files must be consistent:

- `eng/expected-nupkg-file-counts.json` — expected file counts per nupkg
- `eng/expected-dll-frameworks.json` — expected TFM per DLL

Regenerate from a clean `artifacts/` with: `./build.cmd -c Release -pack` (or `./build.sh -c Release --pack`).
