# Signing Validation (SignCheck)

How the script validates file signatures using the Arcade SDK's SigningValidation task, and how to configure and troubleshoot it.

## Overview

SignCheck is a tool that scans files, archives, and packages to ensure their contents have valid signatures. It is bundled as part of the Arcade SDK (`Microsoft.DotNet.SignCheckTask`) and invoked via the shared SDK task entry point (`sdk-task.sh` on Linux/macOS, `sdk-task.ps1` on Windows).

The `-SignCheck` flag in the test script runs SignCheck against the test repo's build output after the build completes. This validates that Arcade's signing infrastructure (including any local changes to SignCheck itself) works correctly end-to-end.

## Invocation

The script runs (on Linux/macOS; on Windows it uses `eng/common/sdk-task.ps1`):

```bash
cd "$TEST_PATH" && ./eng/common/sdk-task.sh --task SigningValidation --restore \
    /p:PackageBasePath="$SIGNCHECK_DIR"
```

### What the SDK task script Does Internally

1. Initializes the Arcade toolset via `eng/common/tools.sh`
2. Locates `SigningValidation.proj` inside the resolved Arcade SDK package
3. Runs MSBuild Restore target (when `--restore` is passed)
4. Runs MSBuild Execute target, which invokes `Microsoft.DotNet.SignCheckTask.SignCheckTask`
5. Produces a binary log at `artifacts/log/Debug/SigningValidation.binlog`

Because the test repo's `global.json` has been pinned to the locally-built Arcade SDK version, SignCheck runs from the **local build artifacts** — not from a published feed. This is what makes it useful for testing SignCheck code changes.

> ⚠️ **The `SigningValidation.proj` restore needs the local feed.** SignCheck resolves `Microsoft.DotNet.SignCheckTask` at the same version as the pinned Arcade SDK. Since this version only exists in the local feed, the local feed path must be passed via `/p:RestoreAdditionalProjectSources` so that the SDK task's restore can find it. The script handles this automatically.

## Default Directory

When `-SignCheck` is passed without `-SignCheckDir`, the script auto-detects the test repo's package output:

```
$TEST_PATH/artifacts/packages/<config>/NonShipping
```

It checks `Debug` first, then `Release`, using whichever exists. If neither is found, the script exits with an error suggesting `--pack` or `-SignCheckDir`.

> ⚠️ Some repos (e.g., arcade-validation) do not produce packages with the default `./build.sh`. If you need packages in the default location, build with `./build.sh --pack`. Alternatively, point `-SignCheckDir` at any directory containing files to validate.

## MSBuild Properties

The `SigningValidation.proj` task accepts these properties via `/p:` arguments:

| Property | Required | Default | Description |
|----------|----------|---------|-------------|
| `PackageBasePath` | ✅ | — | Directory containing files to validate |
| `SignCheckExclusionsFile` | ❌ | — | Path to a text file listing exclusions |
| `EnableJarSigningCheck` | ❌ | `false` | Verify `.jar` file signatures |
| `EnableStrongNameCheck` | ❌ | `false` | Verify strong names for `.exe`/`.dll` files |
| `BuildManifestFile` | ❌ | — | If set, uses `ItemsToSign` from manifest instead of scanning `PackageBasePath` |
| `SignCheckLog` | ❌ | `$(ArtifactsLogDir)/signcheck.log` | Output log file |
| `SignCheckErrorLog` | ❌ | `$(ArtifactsLogDir)/signcheck.errors.log` | Error log file |
| `SignCheckResultsXmlFile` | ❌ | `$(ArtifactsLogDir)/signcheck.xml` | XML results file |

Additional properties can be passed by appending `/p:Key=Value` arguments to the script invocation.

## Supported File Types

SignCheck validates signatures on a wide range of file types:

| Category | Extensions |
|----------|-----------|
| Managed code | `.dll`, `.exe` |
| Native binaries | `.so`, `.dylib`, `.a` |
| NuGet/VSIX | `.nupkg`, `.vsix` |
| Installers | `.msi`, `.msp`, `.pkg`, `.deb`, `.rpm` |
| Archives | `.zip`, `.tar`, `.tgz`, `.tar.gz`, `.gz`, `.cab` |
| Scripts | `.ps1`, `.psd1`, `.psm1`, `.js` |
| macOS | `.app`, `.pkg`, `.dylib` |
| Optional | `.jar` (requires `EnableJarSigningCheck`), `.xml` |

SignCheck recursively inspects containers (archives, packages) to validate signatures on their contents.

## Exclusions

To skip specific files from validation, create an exclusions file and pass it via `SignCheckExclusionsFile`. The file is typically referenced in a repo's CI pipeline as `eng/SignCheckExclusionsFile.txt`.

## Signing Configuration in the Test Repo

The test repo's signing configuration is defined in `eng/Signing.props`:

```xml
<ItemGroup>
  <ItemsToSign Include="$(ArtifactsPackagesDir)**\*.deb" />
  <ItemsToSign Include="$(ArtifactsPackagesDir)**\*.rpm" />
  <ItemsToSign Include="$(ArtifactsPackagesDir)**\*.dll" />
  <!-- ... additional file types ... -->
</ItemGroup>

<ItemGroup>
  <FileSignInfo Include="ProjectOne.dll" CertificateName="3PartySHA2" />
  <!-- ... per-file certificate assignments ... -->
</ItemGroup>
```

This file controls which files are **signed** during a build. `SignCheck` then validates that the signing was applied correctly.

## Output and Logs

After a SignCheck run, look for these files in the test repo:

```
<test-repo>/artifacts/log/
├── Debug/
│   ├── SigningValidation.binlog    ← MSBuild binary log
│   ├── signcheck.log              ← Full validation output
│   ├── signcheck.errors.log       ← Errors only (empty on success)
│   └── signcheck.xml              ← Structured XML results (per-file)
```

The task **fails the build** if `signcheck.errors.log` contains any content. An empty error log means all scanned files passed validation.

### Reading Results from `signcheck.xml`

The XML file contains one `<File>` element per scanned file with per-file outcomes:

```xml
<SignCheckResults>
  <File Name="dotnet-sdk-source-10.0.104.tar.gz" Outcome="Signed" Misc="Timestamp: 02/23/26 17:13:46 (RSA)" />
  <File Name="release.json" Outcome="Skipped" />
  <File Name="dotnet-sdk-source-10.0.104.tar.gz.sig" Outcome="Skipped" />
</SignCheckResults>
```

**Attributes:**

| Attribute | Description |
|-----------|-------------|
| `Name` | File name (relative to `PackageBasePath`) |
| `Outcome` | Validation result (see table below) |
| `Misc` | Additional details (e.g., timestamp info for signed files) |

**Possible `Outcome` values:**

| Outcome | Meaning |
|---------|---------|
| `Signed` | File has a valid signature |
| `Unsigned` | File should be signed but is not — **this is an error** |
| `Skipped` | File type is not subject to signing validation |
| `Excluded` | File matched an entry in the exclusions file |
| `SkippedAndExcluded` | File was both skipped and excluded |

To quickly extract results from the XML:

```powershell
# Show all file outcomes
Get-Content artifacts/log/Debug/signcheck.xml

# List only unsigned files (errors)
Select-String 'Outcome="Unsigned"' artifacts/log/Debug/signcheck.xml

# Count outcomes
([xml](Get-Content artifacts/log/Debug/signcheck.xml)).SignCheckResults.File | Group-Object Outcome
```

### Reading Results from `signcheck.log`

The log file contains the summary line with aggregate counts:

```
Total Files: 3, Signed: 1, Unsigned: 0, Skipped: 2, Excluded: 0, Skipped & Excluded: 0, Not Unpacked: 1
```

| Counter | Meaning |
|---------|---------|
| Signed | Files with valid signatures |
| Unsigned | Files that should be signed but are not (errors) |
| Skipped | Files whose type is not subject to signing |
| Excluded | Files matching exclusion entries |
| Not Unpacked | Archives excluded from recursive unpacking via `DO-NOT-UNPACK` |

## Troubleshooting

### SignCheck fails: "no packages directory found"

**Symptom**: Script exits with error about missing `artifacts/packages/<config>/NonShipping`.

**Cause**: The test repo build didn't produce packages.

**Fix**: Either build with `--pack` or specify a directory explicitly:
```powershell
pwsh ./scripts/Test-Arcade.ps1 -SignCheck -SignCheckDir /path/to/files
```

### SignCheck reports unsigned files

**Symptom**: SignCheck logs errors about unsigned or incorrectly signed files.

**Diagnosis**:
1. Check `artifacts/log/Debug/signcheck.errors.log` for specific file names
2. Check `artifacts/log/Debug/signcheck.log` for the full scan report
3. Open `SigningValidation.binlog` with the `mcp-binlog-tool` MCP server for detailed task execution analysis

**Common causes**:
- Files not listed in `eng/Signing.props`
- Certificate names don't match configured signing certificates
- Third-party binaries not added to exclusions file

### SignCheck task not found

**Symptom**: `sdk-task.sh` fails to locate `SigningValidation.proj`.

**Cause**: The Arcade SDK package doesn't contain the task, or `global.json` isn't pinned to the local build.

**Fix**:
1. Verify the Arcade build produced the SDK package: `ls arcade/artifacts/packages/Release/NonShipping/Microsoft.DotNet.Arcade.Sdk.*.nupkg`
2. Verify `global.json` was updated: `grep Arcade <test-repo>/global.json`
3. Clear NuGet caches and retry: `dotnet nuget locals all --clear`

### SignCheck passes but shouldn't

**Symptom**: Files you expect to fail validation pass without errors.

**Cause**: Files may not be in the scanned directory, or they match an exclusion pattern.

**Fix**: Check `signcheck.log` to see which files were actually scanned. Verify `PackageBasePath` points to the correct directory.

## Arcade SignCheck Packages

The Arcade build produces two signing-related packages:

| Package | Purpose |
|---------|---------|
| `Microsoft.DotNet.SignCheckTask` | MSBuild task used by `SigningValidation.proj` (preferred) |
| `Microsoft.DotNet.SignCheck` | Legacy CLI tool (framework-only, maintained for backward compatibility) |

Both are published to the local feed and their versions match the Arcade SDK version.
