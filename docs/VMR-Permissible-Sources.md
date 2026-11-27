# The Unified Build Almanac (TUBA) - Permissible Source Policy

## Purpose

This document provides guidelines on the types of source code that are permissible in the VMR. It explains how the VMR detects binaries and licenses, it outlines the steps to be taken when a new binary or license is encountered, and it describes how to keep files out of the VMR entirely by [cloaking](#cloaking) them.

## Binaries

### Binary Policy

OSS licensed binaries are allowed in the VMR, but the inclusion status of all binaries that could be included in the VMR must be made explicit. Any binary not explicitly specified is considered "new" and is not allowed in the VMR for all build scenarios. In general, it is preferred to not have binaries in the VMR, but they can be included as long as they are not required to build the source-build product.

To allow a binary into the VMR, you must add the binary or its file glob pattern to either [`allowed-sb-binaries.txt`](https://github.com/dotnet/dotnet/blob/main/eng/allowed-sb-binaries.txt) or [`allowed-vmr-binaries.txt`](https://github.com/dotnet/dotnet/blob/main/eng/allowed-vmr-binaries.txt). Which file to add the binary or its pattern to depends on whether or not the binary is allowed for source build. For help determining if the newly detected binary is allowed in the source-build context, please contact a member of the source-build team at [@dotnet/source-build](https://github.com/orgs/dotnet/teams/source-build).

If the binary is allowed for source-build, add it to `allowed-sb-binaries.txt`. This binary is now allowed in the VMR for source-build and non-source-build scenarios. The binary will not be removed during a build of the source-build product.

If the binary is not allowed for source-build, add it to `allowed-vmr-binaries.txt`. This binary is now allowed in the VMR for non-source-build scenarios and will be removed during a build of the source-build product.

> [!NOTE]
>
> - `allowed-sb-binaries.txt` is a subset of `allowed-vmr-binaries.txt`, so is imported at the top of `allowed-vmr-binaries.txt`. This means that all binaries & exclusions in `allowed-sb-binaries.txt` are also included in `allowed-vmr-binaries.txt`.
> - It is best to target a single file or use a specific pattern when adding to either `allowed-sb-binaries.txt` or `allowed-vmr-binaries.txt`. Otherwise, vague patterns may permit binaries into the VMR that were previously undetected. For example, avoid patterns such as `**/test/**` and instead use a more specific patterns like `src/arcade/test/**/*.png`.

When adding a binary or pattern to either `allowed-sb-binaries.txt` or `allowed-vmr-binaries.txt`, remember to include a link to the relevant issue and tag [@dotnet/source-build](https://github.com/orgs/dotnet/teams/source-build) as a reviewer.

### Validation and Cleaning

The [BinaryTool](https://github.com/dotnet/dotnet/tree/main/eng/tools/BinaryToolKit) is used to validate binaries in the VMR and clean binaries from the VMR. You can run the tool locally by running `./eng/detect-binaries.sh` to detect binaries and  `./eng/detect-binaries.sh --clean` to remove binaries.  This functionality is also exposed via `./prep-source-build/sh`.

#### Validation

The tool flags all binaries not listed in `allowed-vmr-binaries.txt` and `allowed-sb-binaries.txt`. Note that the tool only uses `allowed-vmr-binaries.txt` as a baseline during validation, but, as noted above, `allowed-sb-binaries.txt` is imported at the top of `allowed-vmr-binaries.txt`, meaning that all binaries in `allowed-sb-binaries.txt` are also relevant.

To run default validation, execute `eng/detect-binaries.sh`.

An example output is as follows:

```bash
00:21:03 info: BinaryTool[0] Starting binary tool at 03/19/2024 00:21:03 in Validate mode
00:21:03 info: BinaryTool[0] Detecting binaries in '/vmr' not listed in '/vmr/eng/allowed-vmr-binaries.txt'...
00:21:10 info: BinaryTool[0] Finished binary detection.
00:21:10 dbug: BinaryTool[0] New binaries:
00:21:10 dbug: BinaryTool[0]     src/wpf/src/Microsoft.DotNet.Wpf/src/Shared/Tracing/resources/MSG00001.bin
00:21:10 dbug: BinaryTool[0]     src/wpf/src/Microsoft.DotNet.Wpf/src/Shared/Tracing/resources/wpf-etwTEMP.BIN
00:21:10 fail: BinaryTool[0] ERROR: 2 new binaries. Check '/vmr/artifacts/log/binary-report/NewBinaries.txt' for details.
00:21:10 info: BinaryTool[0] Finished all binary tasks. Took 6.9387884 seconds.
```

In this example, the tool detected 2 new binaries:

- `src/wpf/src/Microsoft.DotNet.Wpf/src/Shared/Tracing/resources/MSG00001.bin`
- `src/wpf/src/Microsoft.DotNet.Wpf/src/Shared/Tracing/resources/wpf-etwTEMP.BIN`.

If these binaries are permitted for source-build, add the binaries and/or relevant file glob pattern(s), such as `src/wpf/src/Microsoft.DotNet.Wpf/src/Shared/Tracing/resources/*.bin`, to `allowed-sb-binaries.txt`. Otherwise, add the binaries or relevant file glob pattern(s) to `allowed-vmr-binaries.txt`.

#### Cleaning

The tool removes all binaries from the VMR that are not listed in `allowed-vmr-binaries.txt` and `allowed-sb-binaries.txt`. Again, note that the tool only uses `allowed-vmr-binaries.txt` as a baseline during validation, but, as noted above, `allowed-sb-binaries.txt` is imported at the top of `allowed-vmr-binaries.txt`, meaning that all binaries in `allowed-sb-binaries.txt` are also relevant.

To run cleaning, execute `./eng/detect-binaries.sh --clean` or `./prep-source-build.sh`. Executing `./prep-source-build.sh` will build the tool using previously source-built artifacts and remove non-source-build allowed binaries from the VMR whereas executing `./eng/detect-binaries.sh --clean` will build the tool using online resources and remove new binaries from the VMR.

An example output is as follows:

```bash
16:42:19 info: BinaryTool[0] Starting binary tool at 3/19/2024 4:42:19 PM in Clean mode
16:42:19 info: BinaryTool[0] Detecting binaries in '/vmr' not listed in '/vmr/eng/allowed-vmr-binaries.txt'...
16:42:23 info: BinaryTool[0] Finished binary detection.
16:42:24 info: BinaryTool[0] Removing binaries from '/vmr'...
16:42:24 dbug: BinaryTool[0]     src/wpf/src/Microsoft.DotNet.Wpf/src/Shared/Tracing/resources/MSG00001.bin
16:42:24 dbug: BinaryTool[0]     src/wpf/src/Microsoft.DotNet.Wpf/src/Shared/Tracing/resources/wpf-etwTEMP.BIN
16:42:24 info: BinaryTool[0] Finished binary removal. Removed 2 binaries.
16:42:24 info: BinaryTool[0] Finished all binary tasks. Took 4.9003778 seconds.
```

In this example, the tool removed 2 binaries:

- `src/wpf/src/Microsoft.DotNet.Wpf/src/Shared/Tracing/resources/MSG00001.bin`
- `src/wpf/src/Microsoft.DotNet.Wpf/src/Shared/Tracing/resources/wpf-etwTEMP.BIN`.

If these binaries are permitted for source-build, add the binaries and/or relevant file glob pattern(s), such as `src/wpf/src/Microsoft.DotNet.Wpf/src/Shared/Tracing/resources/*.bin`, to `allowed-sb-binaries.txt`. Otherwise, add the binaries or relevant file glob pattern(s) to `allowed-vmr-binaries.txt`.

## Licenses

### License Policy

The VMR does not permit code and binaries licensed under non-OSS licenses. For a list of approved OSS licenses, you can check out the [OSI-approved list of licenses](https://opensource.org/licenses/alphabetical).

When a non-OSS license is detected, the offending code and binaries must be cloaked from the VMR. See [Cloaking](#cloaking) for how to do this.

### Detection

Licenses are detected by the [license scan test](https://github.com/dotnet/dotnet/blob/main/test/Microsoft.DotNet.SourceBuild.Tests/LicenseScanTests.cs). This test is run as part of the [source-build license scan pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=1301&_a=summary) (internal Microsoft link). The test detects any license in the VMR that is not part of an exclusion listed in [`LicenseExclusions.txt`](https://github.com/dotnet/dotnet/blob/main/test/Microsoft.DotNet.SourceBuild.Tests/assets/LicenseScanTests/LicenseExclusions.txt).

Common cases for adding a license to [`LicenseExclusions.txt`](https://github.com/dotnet/dotnet/blob/main/test/Microsoft.DotNet.SourceBuild.Tests/assets/LicenseScanTests/LicenseExclusions.txt) include false positives, licenses related to test data, or needing to get a clean scan result with a relevant backport issue to remove the offending license later.

To have a license be added to the list of exclusions in [`LicenseExclusions.txt`](https://github.com/dotnet/dotnet/blob/main/test/Microsoft.DotNet.SourceBuild.Tests/assets/LicenseScanTests/LicenseExclusions.txt), open a PR, include a link to the relevant issue above the exclusion, and tag [@dotnet/source-build](https://github.com/orgs/dotnet/teams/source-build) as a reviewer.

## Cloaking

### What is cloaking

Cloaking is the process of filtering files out when the sources of a development repository are synchronized into the VMR. A cloaked file continues to exist in its development repository, but it is never copied into the VMR, and changes to it never flow into the VMR. Backflow does not touch cloaked files either, so they are not deleted from the development repository when the VMR flows back.

Cloaking rules are defined as `exclude` globs of the repository's entry in [`src/source-mappings.json`](../src/source-mappings.json). The paths are relative to the root of the development repository, not to the VMR. For example, the following entry cloaks a file of the `aspnetcore` repository that would otherwise end up at `src/aspnetcore/src/submodules/.../EditorUserSettings.asset` in the VMR:

```json
{
  "name": "aspnetcore",
  "defaultRemote": "https://github.com/dotnet/aspnetcore",
  "exclude": [
    "src/submodules/MessagePack-CSharp/src/MessagePack.UnityClient/UserSettings/EditorUserSettings.asset"
  ]
}
```

For the full list of source mapping options, including defaults that apply to all repositories, see [Repository Source Mappings](./VMR-Full-Code-Flow.md#repository-source-mappings).

### When to cloak

Cloak a file or directory when it must not be part of the VMR in any build scenario. Common reasons are:

- **Non-OSS licenses** - Code, binaries or license files under a license that is not OSS-approved (see [License Policy](#license-policy)). For example, `vstest` cloaks `src/package/licenses/LICENSE_VS.txt`.
- **Binaries that are not needed to build the VMR** - Binaries that are not allowed for source-build and are not required by any VMR build, such as certificates and private keys (`.pfx` files in `source-build-assets`) or editor and IDE artifacts.
- **Files that break the VMR build or its infrastructure** - For example, `roslyn` cloaks the `SignatureCycle2.exe` test resource because it blocked signing of the VMR source archive.

### Cloaking vs. allowing binaries

Cloaking and the binary allow-lists described in [Binary Policy](#binary-policy) solve different problems:

| | Allow-listed in `allowed-vmr-binaries.txt` | Cloaked in `source-mappings.json` |
|---|---|---|
| Present in the VMR git repository | Yes | No |
| Available to Microsoft (non-source-build) builds | Yes | No |
| Available to source-build | Only if also in `allowed-sb-binaries.txt` | No |

Prefer the allow-lists for OSS binaries that are needed by some VMR build. Use cloaking when the file must not be in the VMR at all, or when nothing in the VMR needs it.

### How to cloak a file

1. Add the path or a glob pattern to the `exclude` list of the repository's entry in [`src/source-mappings.json`](../src/source-mappings.json). As with the binary allow-lists, prefer targeting a single file or a specific pattern over broad globs.
1. Delete the file from the VMR in the same PR. Adding an exclusion prevents future synchronization of the file, but codeflow does not reliably remove a file that is already in the VMR.
1. If anything in the VMR build references the cloaked file (for example a test project that embeds it as a resource), update it so that the build succeeds without the file.
1. Include a link to the relevant issue in the PR description.
1. Repeat the change for every release branch of the VMR that contains the file. In older release branches such as `release/9.0.1xx`, the VMR's `src/source-mappings.json` is synchronized from [`src/VirtualMonoRepo/source-mappings.json`](https://github.com/dotnet/sdk/blob/release/9.0.1xx/src/VirtualMonoRepo/source-mappings.json) in the `dotnet/sdk` repository, so the exclusion must be added there, while the file is still deleted from the VMR directly.

### Validation

The [CloakedFileScan](https://github.com/dotnet/dotnet/tree/main/eng/tools/CloakedFileScan) tool reports any file in the VMR that matches a cloaking rule in `src/source-mappings.json`. It runs in the `Scan` stage of the official VMR pipeline. Because that stage does not run in public PR validation, run the tool locally from the VMR root before submitting a cloaking PR:

```bash
./eng/common/dotnet.sh run --project eng/tools/CloakedFileScan/CloakedFileScan.csproj
```

The tool exits with a non-zero exit code and lists the offending files if any cloaked file is still present.
