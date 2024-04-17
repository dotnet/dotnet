# The Unified Build Almanac (TUBA) - Permissible Source Policy

## Purpose

This document provides guidelines on the types of source code that are permissible in the VMR. It explains how the VMR detects binaries and licenses, and it outlines the steps to be taken when a new binary or license is encountered.

## Binaries

### Policy

OSS licensed binaries are allowed in the VMR, but the inclusion status of all binaries that could be included in the VMR must be made explicit. Any binary not explicitly specified is considered "new" and is not allowed in the VMR for all build scenarios. In general, it is preferred to not have binaries in the VMR, but they can be included as long as they are not required to build the source-build product.

To allow a binary into the VMR, you must add the binary or its file glob pattern to either [`allowed-sb-binaries.txt`](https://github.com/dotnet/dotnet/blob/main/src/installer/src/VirtualMonoRepo/allowed-sb-binaries.txt) or [`allowed-vmr-binaries.txt`](https://github.com/dotnet/dotnet/blob/main/src/installer/src/VirtualMonoRepo/allowed-vmr-binaries.txt). Which file to add the binary or its pattern to depends on whether or not the binary is allowed for source build. For help determining if the newly detected binary is allowed in the source-build context, please contact a member of the source-build team at [@dotnet/source-build-internal](https://github.com/orgs/dotnet/teams/source-build-internal).

If the binary is allowed for source-build, add it to `allowed-sb-binaries.txt`. This binary is now allowed in the VMR for source-build and non-source-build scenarios. The binary will not be removed during a build of the source-build product.

If the binary is not allowed for source-build, add it to `allowed-vmr-binaries.txt`. This binary is now allowed in the VMR for non-source-build scenarios and will be removed during a build of the source-build product.

> [!NOTE]  
> `allowed-sb-binaries.txt` is a subset of `allowed-vmr-binaries.txt`, so is imported at the top of `allowed-vmr-binaries.txt`. This means that all binaries & exclusions in `allowed-sb-binaries.txt` are also included in `allowed-vmr-binaries.txt`.

> [!IMPORTANT]  
> It is best to target a single file or use a specific pattern when adding to either `allowed-sb-binaries.txt` or `allowed-vmr-binaries.txt`. Otherwise, vague patterns may permit binaries into the VMR that were previously undetected. For example, avoid patterns such as `**/test/**` and instead use a more specific patterns like `src/arcade/test/**/*.png`.

When adding a binary or pattern to either `allowed-sb-binaries.txt` or `allowed-vmr-binaries.txt`, remember to include a link to the relevant issue and tag [@dotnet/source-build-internal](https://github.com/orgs/dotnet/teams/source-build-internal) as a reviewer.

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

If these binaries are permitted for source-build, add the binaries and/or relevent file glob pattern(s), such as `src/wpf/src/Microsoft.DotNet.Wpf/src/Shared/Tracing/resources/*.bin`, to `allowed-sb-binaries.txt`. Otherwise, add the binaries or relevent file glob pattern(s) to `allowed-vmr-binaries.txt`.

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
 
If these binaries are permitted for source-build, add the binaries and/or relevent file glob pattern(s), such as `src/wpf/src/Microsoft.DotNet.Wpf/src/Shared/Tracing/resources/*.bin`, to `allowed-sb-binaries.txt`. Otherwise, add the binaries or relevent file glob pattern(s) to `allowed-vmr-binaries.txt`.

## Licenses

### Policy

The VMR does not permit code and binaries licensed under non-OSS licenses. For a list of approved OSS licenses, you can check out the [OSI-approved list of licenses](https://opensource.org/licenses/alphabetical).

When a non-OSS license is detected, the offending code and binaries must be cloaked from the VMR. See [the VMR](./VMR-Design-And-Operation.md#repository-source-mappings) and [source-build documentation](https://github.com/dotnet/source-build/blob/main/Documentation/sourcebuild-in-repos/new-repo.md#cloaking-filtering-the-repository-sources) on cloaking.

### Detection

Licenses are detected by the [license scan test](https://github.com/dotnet/dotnet/blob/main/test/Microsoft.DotNet.SourceBuild.SmokeTests/LicenseScanTests.cs). This test is run as part of the [source-build license scan pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=1301&_a=summary) (internal Microsoft link). The test detects any license in the VMR that is not part of an exclusion listed in [`LicenseExclusions.txt`](https://github.com/dotnet/dotnet/blob/main/test/Microsoft.DotNet.SourceBuild.SmokeTests/assets/LicenseExclusions.txt).

Common cases for adding a license to [`LicenseExclusions.txt`](https://github.com/dotnet/dotnet/blob/main/test/Microsoft.DotNet.SourceBuild.SmokeTests/assets/LicenseExclusions.txt) include false positives, licenses related to test data, or needing to get a clean scan result with a relevant backport issue to remove the offending license later.

To have a license be added to the list of exclusions in [`LicenseExclusions.txt`](https://github.com/dotnet/dotnet/blob/main/test/Microsoft.DotNet.SourceBuild.SmokeTests/assets/LicenseExclusions.txt), open a PR, include a link to the relevant issue above the exclusion, and tag [@dotnet/source-build-internal](https://github.com/orgs/dotnet/teams/source-build-internal) as a reviewer.