# Building Arcade for CI

How Arcade is built by the script and the key build parameters that control package versioning and output.

## Build Command

The script builds Arcade with:

```powershell
# OfficialBuildId is auto-generated as a future date (current date + 5 years)
$OfficialBuildId = "{0}.1" -f (Get-Date).AddYears(5).ToString('yyyyMMdd')
./build.sh --configuration Release --pack /p:OfficialBuildId=$OfficialBuildId
```

### Flags Explained

| Flag | Purpose |
|------|---------|
| `--configuration Release` | Builds in Release mode (optimized, no debug symbols in packages) |
| `--pack` | Produces NuGet packages in addition to compiled binaries |
| `/p:OfficialBuildId=<future-date>.1` | Sets the build ID used for package versioning — must be a future date (see below) |

### What `build.sh` Does Internally

1. Invokes `eng/common/build.sh` (shared Arcade build infrastructure)
2. Auto-installs the correct .NET SDK from `global.json` via `eng/common/dotnet.sh` into `.dotnet/`
3. Runs MSBuild restore, build, and pack targets
4. Outputs packages to `artifacts/packages/{Configuration}/`

## OfficialBuildId and Package Versioning

The `OfficialBuildId` property controls the prerelease version suffix of all packages. Format: `YYYYMMDD.N` where N is the build number for that day.

**Version formula:**
```
{VersionPrefix}-beta.{OfficialBuildId}
```

**Example:**
- `VersionPrefix` in Arcade is `11.0.0` (from `eng/Versions.props`)
- `OfficialBuildId` = `31216.1` (auto-generated future date)
- Resulting version: `11.0.0-beta.31216.1`

The script **auto-generates a future-dated OfficialBuildId** (current date + 5 years) to ensure the locally-built version is **always newer** than any officially published version. This prevents package version conflicts — a real official build may have already used a past or present date, so the ID must always be in the future.

> 🚨 **The OfficialBuildId must be a future date.** Using today's date or a past date risks colliding with a real official build's ID, causing NuGet to resolve the wrong package or fail with version conflicts.

> ⚠️ If you need a specific version to match an official build, change the OfficialBuildId to match. Find the official build's ID from its Azure DevOps pipeline run. Only do this for reproduction — never for general testing.

## Build Outputs

After a successful build with `--pack`:

```
arcade/artifacts/
├── packages/
│   └── Release/
│       ├── NonShipping/          ← Used by the script for local feed
│       │   ├── Microsoft.DotNet.Arcade.Sdk.*.nupkg
│       │   ├── Microsoft.DotNet.Helix.Sdk.*.nupkg
│       │   └── ... (50+ packages)
│       └── Shipping/             ← Not copied by the script (add manually if needed)
│           └── ...
├── bin/                          ← Compiled binaries (not used by feed)
├── log/
│   └── Release/
│       └── Build.binlog          ← MSBuild binary log for diagnostics
└── tmp/                          ← Temporary build files
```

### NonShipping vs Shipping

- **NonShipping**: SDK packages, build tasks, internal tooling — these are what consuming repos resolve via `global.json` MSBuild SDK references
- **Shipping**: Packages intended for public NuGet.org feeds (e.g., `Microsoft.DotNet.XUnitExtensions`)

The script initializes the local feed from **NonShipping** packages because that's where the MSBuild SDK packages live. Shipping packages are **not** copied by default — if specific Shipping packages are needed, they can be added manually.

## Build Configurations

| Configuration | Use Case |
|--------------|----------|
| `Release` | Default. Optimized build, used for package validation. Most closely matches official builds |
| `Debug` | Faster build, includes debug symbols. Useful for stepping through Arcade code in a debugger |

## Platform-Specific Build Commands

| Platform | Build Command | Configuration Flag | Pack Flag |
|----------|--------------|-------------------|-----------|
| Linux | `./build.sh` | `--configuration` | `--pack` |
| macOS | `./build.sh` | `--configuration` | `--pack` |
| Windows | `Build.cmd` | `-configuration` | `-pack` |

The script auto-detects the platform-appropriate build script (`build.sh` on Linux/macOS, `Build.cmd` on Windows) and adjusts flag syntax automatically (e.g., `--configuration` vs `-configuration`, `--pack` vs `-pack`). On Windows, `Build.cmd` invokes `eng/common/build.ps1` via Windows PowerShell, which requires single-dash (`-`) parameter prefixes — double-dash (`--`) prefixes are not recognized as named parameters and cause `MSB1001: Unknown switch` errors.

## Reproducing Official CI Builds

To exactly reproduce an official CI build locally, you need:

1. **Same commit**: checkout the exact commit from the official build
2. **Same OfficialBuildId**: find it from the Azure DevOps pipeline run parameters
3. **Same configuration**: typically `Release`
4. **Same platform**: official builds run on specific OS/architecture

```bash
git checkout <commit-sha>
./build.sh --configuration Release --pack /p:OfficialBuildId=<YYYYMMDD.N>
```

The main difference from CI: official builds also run signing, validation, and publishing steps that aren't needed for local testing.
