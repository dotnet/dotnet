---
name: nuget-restore-repro-triage
description: >
  Assists with reproducing and triaging NuGet restore issues. Given a GitHub issue from NuGet/Home,
  analyzes the issue, extracts repro-relevant data, and writes C# tests in the NuGet.Client repo
  that reproduce the described scenario. Use for NuGet restore bugs, error code investigations,
  and test authoring tasks.
---

# NuGet Restore Repro/Triage Skill

You are a NuGet restore test author. Given a GitHub issue from [NuGet/Home](https://github.com/NuGet/Home), your job is to:

1. Analyze the issue and extract repro-relevant data
2. Write one or more C# tests in the NuGet.Client repo that reproduce the described scenario
3. **First, assert the buggy behavior** — the test must pass against the current code and confirm the reported symptom (e.g., wrong error code, wrong result). This proves the test actually reproduces the bug.
4. Then, add a secondary assertion showing what the **correct** behavior should be after the fix. It is on the developer to remove the primary assertion.

This document teaches you the NuGet.Client restore test infrastructure so you can do this effectively.

---

## Step 1: Analyze the Issue

### What to Extract

Read the issue (title, body, and all comments) and extract these fields:

| Field | Where to look | Example |
|---|---|---|
| **Error code(s)** | Error output, logs | `NU1004`, `NU1102`, `NU1510`, `NU3028` |
| **Command** | Repro steps | `dotnet restore`, `dotnet restore --locked-mode` |
| **Package graph** | Project files, repro steps | `System.Management 10.0.2 → System.CodeDom 10.0.2` |
| **Project configuration** | `.csproj` snippets, description | TFM, PackageReference, CPM, lock files, pruning |
| **NuGet.Config** | Config snippets, source descriptions | Custom sources, package source mapping |
| **Restore mode** | Flags, properties | Locked mode, force evaluate, RestorePackagesWithLockFile |
| **Expected behavior** | "Expected" section | "Should restore successfully" or "Should show NU1004" |
| **Actual behavior** | "Observed" section, logs | "Gets NU1102 instead of NU1004" |
| **Environment** | dotnet --info, OS info | Cross-platform, specific SDK version |
| **Regression info** | "Worked before" notes | "Broke in 10.0.100", "works without pruning" |

### Handling Incomplete Issues

- If the package graph is unclear, construct the **simplest possible graph** that exercises the described behavior (e.g., `A 1.0.0 → B 1.0.0` for a transitive dependency issue).
- If the TFM is unspecified, use `net10.0` for modern scenarios or `net472` for legacy/.NET Framework scenarios.
- If the error is about lock files, ensure the test enables `RestorePackagesWithLockFile`.
- Always use **local package sources** — never depend on nuget.org.

---

## Step 2: Choose the Right Test Location

### Decision Tree

```
Can the scenario be reproduced by constructing a PackageSpec in-process and running RestoreCommand directly?
  │
  ├─ YES → test/NuGet.Core.FuncTests/NuGet.Commands.FuncTest/  (preferred — fast, deterministic)
  │         Pick the file by scenario:
  │         ├─ Lock file / locked mode     → RestoreCommand_PackagesLockFileTests.cs
  │         ├─ Package pruning             → RestoreCommand_PrunePackageReference.cs
  │         ├─ Framework aliases           → RestoreCommand_Aliases.cs
  │         ├─ Package source mapping      → RestoreCommand_PackageSourceMapping.cs
  │         ├─ General restore behavior    → RestoreCommandTests.cs
  │         └─ New scenario category       → Create a new RestoreCommand_<Scenario>.cs file
  │
  └─ NO  → Does it require real MSBuild/SDK evaluation, actual dotnet CLI invocation,
           SDK props/targets, solution file parsing, or command-line property behavior?
           │
           ├─ YES → test/NuGet.Core.FuncTests/Dotnet.Integration.Test/DotnetRestoreTests.cs
           │        Uses DotnetIntegrationTestFixture to run the real `dotnet restore` process.
           │        Examples of scenarios that NEED this:
           │        ├─ MSBuild property evaluation (e.g., $(SolutionDir), conditional TFM selection)
           │        ├─ SDK-injected props/targets (e.g., PrunePackageReference from SDK packs,
           │        │  UseWindowsForms importing WindowsDesktop targets)
           │        ├─ Command-line /p: property overrides (e.g., /p:TargetFramework=...)
           │        ├─ Solution-level restore (.sln / .slnx parsing)
           │        └─ Real .csproj file with SDK-style project evaluation
           │
           └─ NO  → Is it a unit test for a specific utility or component?
                   └─ YES → test/NuGet.Core.Tests/NuGet.Commands.Test/
```

**Rule of thumb:** Start with the in-process functional tests (`RestoreCommand_*.cs`). Only escalate to `DotnetRestoreTests` when the bug depends on behavior that comes from MSBuild evaluation or the real SDK pipeline — things that `PackageSpec` JSON alone cannot represent.

### Test Class Setup

All functional restore test classes in `NuGet.Commands.FuncTest` use this pattern:

```csharp
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Commands.Test;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Commands.FuncTest
{
    [Collection(TestCollection.Name)]
    public class RestoreCommand_YourScenario
    {
        // Tests go here...

        // Standard restore helper (copy into each test class)
        internal static Task<RestoreResult> RunRestoreAsync(
            SimpleTestPathContext pathContext, params PackageSpec[] projects)
        {
            return RunRestoreAsync(pathContext, new TestLogger(), projects);
        }

        internal static Task<RestoreResult> RunRestoreAsync(
            SimpleTestPathContext pathContext, TestLogger logger, params PackageSpec[] projects)
        {
            return RunRestoreAsync(pathContext, forceEvaluate: false, logger, projects);
        }

        internal static Task<RestoreResult> RunRestoreAsync(
            SimpleTestPathContext pathContext, bool forceEvaluate, TestLogger logger,
            params PackageSpec[] projects)
        {
            var request = ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, projects);
            request.RestoreForceEvaluate = forceEvaluate;
            return new RestoreCommand(request).ExecuteAsync();
        }
    }
}
```

---

## Step 3: Test Infrastructure Reference

### Core Classes

| Class | Source file | Purpose |
|---|---|---|
| `SimpleTestPathContext` | `test/TestUtilities/Test.Utility/SimpleTestSetup/SimpleTestPathContext.cs` | Creates an isolated temp directory with `PackageSource`, `SolutionRoot`, `UserPackagesFolder`, `NuGetConfig`, etc. |
| `SimpleTestPackageContext` | `test/TestUtilities/Test.Utility/SimpleTestSetup/SimpleTestPackageContext.cs` | Defines a test package with `Dependencies`, `PerFrameworkDependencies`, custom files. Use one or the other, never both. |
| `SimpleTestPackageUtility` | `test/TestUtilities/Test.Utility/SimpleTestSetup/SimpleTestPackageUtility.cs` | Publishes `SimpleTestPackageContext` packages to a local V3 feed via `CreateFolderFeedV3Async`. |
| `ProjectTestHelpers` | `test/TestUtilities/Test.Utility/Commands/ProjectTestHelpers.cs` | Creates `PackageSpec` from JSON (`GetPackageSpecWithProjectNameAndSpec`) and `RestoreRequest` from `PathContext` + `PackageSpec` (`CreateRestoreRequest`). |
| `RestoreCommand` | `src/NuGet.Core/NuGet.Commands/RestoreCommand/RestoreCommand.cs` | Executes an in-process restore. Call `ExecuteAsync()` → `RestoreResult`. Call `CommitAsync()` to write assets/lock files to disk. |
| `TestLogger` | `test/TestUtilities/Test.Utility/TestLogger.cs` | Captures log messages. Use `ErrorMessages`, `WarningMessages`, `ShowMessages()` for diagnostics. |
| `DotnetIntegrationTestFixture` | `test/NuGet.Core.FuncTests/Dotnet.Integration.Test/DotnetIntegrationTestFixture.cs` | For integration tests only. Runs the real `dotnet` CLI via `RunDotnetExpectSuccess`/`RunDotnetExpectFailure`. |

Read the source files for full API details.

### Workflow: How They Fit Together

Every in-process restore test follows this recipe:

```csharp
// 1. Create isolated environment
using var pathContext = new SimpleTestPathContext();

// 2. Create test packages and publish to local feed
var packageA = new SimpleTestPackageContext("A", "1.0.0")
{
    Dependencies = [new SimpleTestPackageContext("B", "1.0.0")]
};
await SimpleTestPackageUtility.CreateFolderFeedV3Async(
    pathContext.PackageSource, PackageSaveMode.Defaultv3, packageA);

// 3. Define project spec via JSON
var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec(
    "Project1", pathContext.SolutionRoot, projectJson);

// 4. Optionally configure lock file, pruning, etc. on the spec
projectSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
    restorePackagesWithLockFile: "true", nuGetLockFilePath: null, restoreLockedMode: false);

// 5. Run restore
var logger = new TestLogger();
var result = await RunRestoreAsync(pathContext, logger, projectSpec);
// — or for two-phase tests, call CommitAsync() then restore again in locked mode

// 6. Assert
result.Success.Should().BeTrue(because: logger.ShowMessages());
```

### PackageSpec JSON Format

The JSON DSL used in `GetPackageSpecWithProjectNameAndSpec` supports these fields:

```jsonc
{
  // Optional: restore-level settings (CPM, transitive pinning)
  "restore": {
    "centralPackageVersionsManagementEnabled": true,
    "CentralPackageTransitivePinningEnabled": true
  },
  "frameworks": {
    "<tfm>": {
      // Optional: alias for multi-targeting same framework
      "targetAlias": "apple",
      // Package dependencies
      "dependencies": {
        "<packageId>": {
          "version": "[1.0.0,)",
          "target": "Package",
          // Required when CPM is enabled:
          "versionCentrallyManaged": true
        }
      },
      // Central package versions (Directory.Packages.props equivalent)
      "centralPackageVersions": {
        "<packageId>": "[1.0.0,)"
      },
      // Package pruning (PrunePackageReference)
      "packagesToPrune": {
        "<packageId>": "(,<version>]"
      },
      // Asset target fallback
      "assetTargetFallback": true,
      "imports": ["net472"],
      "warn": true
    }
  }
}
```

### Central Package Management (CPM) Setup

When the issue involves `Directory.Packages.props`, `ManagePackageVersionsCentrally`, or `CentralPackageTransitivePinningEnabled`, use these JSON fields:

```csharp
// CPM project: A 1.0.0 depends on B, central versions define both A and B
var cpmProject = @"
{
  ""restore"": {
    ""centralPackageVersionsManagementEnabled"": true
  },
  ""frameworks"": {
    ""net472"": {
        ""dependencies"": {
                ""packageA"": {
                    ""version"": ""[1.0.0,)"",
                    ""target"": ""Package"",
                    ""versionCentrallyManaged"": true
                }
        },
        ""centralPackageVersions"": {
            ""packageA"": ""[1.0.0,)"",
            ""packageB"": ""[1.0.0,)""
        }
    }
  }
}";
var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec(
    "Project1", pathContext.SolutionRoot, cpmProject);
```

With transitive pinning enabled:
```csharp
// CPM + transitive pinning: central version of B pins transitive B to that version
var cpmWithPinning = @"
{
  ""restore"": {
    ""centralPackageVersionsManagementEnabled"": true,
    ""CentralPackageTransitivePinningEnabled"": true
  },
  ""frameworks"": {
    ""net472"": {
        ""dependencies"": {
                ""packageA"": {
                    ""version"": ""[1.0.0,)"",
                    ""target"": ""Package"",
                    ""versionCentrallyManaged"": true
                }
        },
        ""centralPackageVersions"": {
            ""packageA"": ""[1.0.0,)"",
            ""packageB"": ""[1.0.0,)""
        }
    }
  }
}";
```

**Key CPM rules:**
- Set `"centralPackageVersionsManagementEnabled": true` in the `"restore"` section
- Every dependency must have `"versionCentrallyManaged": true`
- Add `"centralPackageVersions"` in each framework with the centrally defined versions
- For transitive pinning, also set `"CentralPackageTransitivePinningEnabled": true`

### Multi-Project Scenarios

```csharp
var rootSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec(
    "Root", pathContext.SolutionRoot, rootProjectJson);
var leafSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec(
    "Leaf", pathContext.SolutionRoot, leafProjectJson);

// Add project reference
rootSpec = rootSpec.WithTestProjectReference(leafSpec);

// Restore both (root project is restored, leaf is in the closure)
var result = await RunRestoreAsync(pathContext, rootSpec, leafSpec);
```

---

## Step 4: Assertion Reference

### Success / Failure

```csharp
result.Success.Should().BeTrue();
result.Success.Should().BeTrue(because: logger.ShowMessages()); // includes logs on failure
result.Success.Should().BeFalse();
result.Success.Should().BeFalse(because: logger.ShowMessages());
```

### Resolved Packages (assets file / lock file)

```csharp
// Check target count
result.LockFile.Targets.Should().HaveCount(1);

// Check resolved libraries in a target
result.LockFile.Targets[0].Libraries.Should().HaveCount(2);
result.LockFile.Targets[0].Libraries[0].Name.Should().Be("packageA");
result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));
result.LockFile.Targets[0].Libraries[0].Dependencies.Should().BeEmpty();
result.LockFile.Targets[0].Libraries[0].Dependencies.Should().HaveCount(1);
result.LockFile.Targets[0].Libraries[0].Dependencies[0].Id.Should().Be("packageB");

// By alias (when using targetAlias)
var target = result.LockFile.GetTarget("apple", null);
target.Libraries.Should().ContainSingle(e => e.Name!.Equals("packageX"));
```

### Error Codes and Log Messages

```csharp
// Via LockFile log messages (structured)
result.LockFile.LogMessages.Should().HaveCount(1);
result.LockFile.LogMessages[0].Code.Should().Be(NuGetLogCode.NU1004);
result.LockFile.LogMessages[0].Level.Should().Be(LogLevel.Error);
result.LockFile.LogMessages[0].Message.Should().Contain("expected text");
result.LockFile.LogMessages[0].LibraryId.Should().Be("packageName");
result.LockFile.LogMessages[0].TargetGraphs.Should().HaveCount(1);

// Via logger (string-based)
logger.ErrorMessages.Should().HaveCount(1);
logger.ErrorMessages.Single().Should().Contain("NU1004");
logger.WarningMessages.Should().ContainSingle(m => m.Contains("NU1510"));
```

### Packages Lock File (packages.lock.json)

```csharp
// Check the generated packages.lock.json
result._newPackagesLockFile.Should().NotBeNull();
result._newPackagesLockFile.Targets.Should().HaveCount(1);
result._newPackagesLockFile.Targets[0].Dependencies.Should().HaveCount(2);
result._newPackagesLockFile.Targets[0].Dependencies[0].Id.Should().Be("packageA");
result._newPackagesLockFile.Targets[0].Dependencies[0].Dependencies.Should().BeEmpty();

// Null means lock file didn't need updating (no-op)
result._newPackagesLockFile.Should().BeNull();
```

### NuGet Error / Warning Codes

Error and warning codes are defined as enum values in `NuGetLogCode`:

**Source:** `src/NuGet.Core/NuGet.Common/Errors/NuGetLogCode.cs`

Look up the specific code from the issue (e.g., `NU1004`, `NU1102`) in that file. Each enum member has a doc comment explaining its meaning. Use the enum value in assertions:
```csharp
result.LockFile.LogMessages[0].Code.Should().Be(NuGetLogCode.NU1004);
```

---

## Step 5: Test Patterns by Scenario

### Pattern A: Package Pruning

When the issue involves `PrunePackageReference`, `RestoreEnablePackagePruning`, or `PackageOverrides.txt`:

```csharp
[Fact]
public async Task DescriptiveTestName()
{
    using var pathContext = new SimpleTestPathContext();

    // 1. Create packages that form the dependency graph from the issue
    var packageA = new SimpleTestPackageContext("A", "1.0.0")
    {
        Dependencies = [new SimpleTestPackageContext("B", "1.0.0")]
    };
    await SimpleTestPackageUtility.CreateFolderFeedV3Async(
        pathContext.PackageSource, PackageSaveMode.Defaultv3, packageA);

    // 2. Define project spec with packagesToPrune matching the issue
    var rootProject = @"
    {
      ""frameworks"": {
        ""net10.0"": {
            ""dependencies"": {
                    ""A"": { ""version"": ""[1.0.0,)"", ""target"": ""Package"" }
            },
            ""packagesToPrune"": {
                ""B"" : ""(,1.0.0]""
            }
        }
      }
    }";
    var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec(
        "Project1", pathContext.SolutionRoot, rootProject);

    // 3. Restore and assert
    var result = await RunRestoreAsync(pathContext, projectSpec);
    result.Success.Should().BeTrue();
    result.LockFile.Targets[0].Libraries.Should().HaveCount(1); // B was pruned
}
```

### Pattern B: Lock File / Locked Mode

When the issue involves `--locked-mode`, `RestorePackagesWithLockFile`, or `packages.lock.json`:

```csharp
[Fact]
public async Task DescriptiveTestName()
{
    using var pathContext = new SimpleTestPathContext();
    var logger = new TestLogger();

    // 1. Create packages
    var packageA = new SimpleTestPackageContext("a", "1.0.0");
    await SimpleTestPackageUtility.CreateFolderFeedV3Async(
        pathContext.PackageSource, packageA);

    // 2. Define project spec with lock file enabled via JSON + RestoreLockProperties
    var rootProject = @"
    {
      ""frameworks"": {
        ""net10.0"": {
            ""dependencies"": {
                    ""a"": {
                        ""version"": ""[1.0.0,)"",
                        ""target"": ""Package""
                    }
            }
        }
      }
    }";
    var packageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec(
        "TestProject", pathContext.SolutionRoot, rootProject);
    packageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
        restorePackagesWithLockFile: "true",
        nuGetLockFilePath: null,
        restoreLockedMode: false);

    // 3. Initial restore to generate lock file
    var result = await new RestoreCommand(
        ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec))
        .ExecuteAsync();
    await result.CommitAsync(logger, CancellationToken.None);
    result.Success.Should().BeTrue();

    // 4. Modify something (simulate the issue's scenario)
    packageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
        restorePackagesWithLockFile: "true",
        packageSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
        restoreLockedMode: true);
    // ... make changes that trigger the bug ...
    logger.Clear();

    // 5. Restore in locked mode and check error
    result = await new RestoreCommand(
        ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec))
        .ExecuteAsync();

    result.Success.Should().BeFalse();
    logger.ErrorMessages.Single().Should().Contain("NU1004");
}
```

### Pattern C: Error Path (Expected Failure)

When the issue is about a confusing/incorrect error message:

```csharp
[Fact]
public async Task DescriptiveTestName()
{
    using var pathContext = new SimpleTestPathContext();
    var logger = new TestLogger();

    // Setup minimal scenario that triggers the bad error message
    // ...

    var result = await RunRestoreAsync(pathContext, logger, projectSpec);

    result.Success.Should().BeFalse();
    // Assert the CORRECT error code/message (what it SHOULD say after the fix)
    logger.ErrorMessages.Single().Should().Contain("NU1004"); // not NU1102
    logger.ErrorMessages.Single().Should().Contain("lock file");
}
```

---

## Worked Example: Issue #14727

### Issue Summary

**Title:** Bad error message when lock file is inconsistent due to pruning changes

**Scenario:** A project uses `RestorePackagesWithLockFile` and package pruning. Lock file was generated on Windows where `System.CodeDom` is pruned (via WindowsDesktop SDK pack). On Linux, the pruning data differs (lower version bound), so `System.CodeDom` is needed but missing from the lock file. Instead of reporting NU1004 (lock file inconsistency), NuGet reports NU1102 ("Unable to find package System.CodeDom").

**Key data extracted:**
- Lock file + locked mode enabled
- Package pruning active
- Transitive dependency (`System.CodeDom`) pruned on one platform but not another
- Wrong error: NU1102 instead of NU1004
- Simplified repro from comment: different `PrunePackageReference` versions per platform

### Mapped Test

This maps to `RestoreCommand_PrunePackageReference.cs` or `RestoreCommand_PackagesLockFileTests.cs`.

**Test file:** `test/NuGet.Core.FuncTests/NuGet.Commands.FuncTest/RestoreCommand_PrunePackageReference.cs`

```csharp
// P -> A 1.0.0 -> B 1.0.0
// Prune B with version (,1.0.0] (lock file generated with B pruned)
// Then restore with different prune data: B (,0.5.0] (B no longer pruned)
// In locked mode: should fail with NU1004 (lock file inconsistency),
// NOT NU1102 (package not found)
[Fact]
public async Task RestoreCommand_WithLockedMode_WhenPruneDataChanges_ReportsNU1004_NotNU1102()
{
    using var pathContext = new SimpleTestPathContext();

    // Setup packages: A depends on B
    var packageA = new SimpleTestPackageContext("packageA", "1.0.0")
    {
        Dependencies = [new SimpleTestPackageContext("packageB", "1.0.0")]
    };
    await SimpleTestPackageUtility.CreateFolderFeedV3Async(
        pathContext.PackageSource, PackageSaveMode.Defaultv3, packageA);

    // Project with pruning that removes B (simulates Windows with SDK pack pruning data)
    var windowsProject = @"
    {
      ""frameworks"": {
        ""net10.0"": {
            ""dependencies"": {
                    ""packageA"": { ""version"": ""[1.0.0,)"", ""target"": ""Package"" }
            },
            ""packagesToPrune"": {
                ""packageB"" : ""(,1.0.0]""
            }
        }
      }
    }";
    var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec(
        "Project1", pathContext.SolutionRoot, windowsProject);
    projectSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
        restorePackagesWithLockFile: "true", nuGetLockFilePath: null,
        restoreLockedMode: false);

    // Step 1: Generate lock file (B is pruned, so lock file won't include B)
    var setupResult = await RunRestoreAsync(pathContext, projectSpec);
    setupResult.Success.Should().BeTrue();
    await setupResult.CommitAsync(NullLogger.Instance, CancellationToken.None);

    // Step 2: Change pruning data to simulate Linux
    // (lower version bound, so B 1.0.0 is NOT pruned)
    var linuxProject = @"
    {
      ""frameworks"": {
        ""net10.0"": {
            ""dependencies"": {
                    ""packageA"": { ""version"": ""[1.0.0,)"", ""target"": ""Package"" }
            },
            ""packagesToPrune"": {
                ""packageB"" : ""(,0.5.0]""
            }
        }
      }
    }";
    var linuxSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec(
        "Project1", pathContext.SolutionRoot, linuxProject);
    linuxSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
        restorePackagesWithLockFile: "true", nuGetLockFilePath: null,
        restoreLockedMode: true);

    // Step 3: Restore in locked mode with different prune data
    var testLogger = new TestLogger();
    var result = await RunRestoreAsync(pathContext, testLogger, linuxSpec);

    // Assert: Should fail because lock file is inconsistent
    result.Success.Should().BeFalse(because: testLogger.ShowMessages());

    // STEP A: First, assert the BUGGY behavior to confirm the test reproduces the issue.
    // The bug is that NuGet reports NU1102 ("Unable to find package B") instead of NU1004.
    result.LockFile.LogMessages.Should().Contain(m => m.Code == NuGetLogCode.NU1102,
        $"Expected NU1102 (the reported bug) but got: {string.Join(", ", result.LockFile.LogMessages.Select(m => m.Code))}");

    // STEP B: Once confirmed, replace the assertion above with the CORRECT behavior:
    // result.LockFile.LogMessages.Should().Contain(m => m.Code == NuGetLogCode.NU1004,
    //     $"Expected NU1004 (lock file inconsistency) but got: {string.Join(", ", result.LockFile.LogMessages.Select(m => m.Code))}");
}
```

### Why This Test Works

1. **Step 1** generates a lock file where `packageB` is pruned (not in the lock file)
2. **Step 2** changes the prune version range so `packageB 1.0.0` is no longer prunable
3. **Step 3** restores in locked mode — the lock file doesn't have `packageB`, but the resolver now needs it
4. The **bug** is that NuGet tries to find `packageB` on the source and fails with NU1102, instead of detecting the lock file inconsistency and reporting NU1004
5. **The test first asserts NU1102** (the buggy behavior) to prove it reproduces the issue. After confirming, the assertion is swapped to NU1004 (the correct behavior) so the test will pass only after the fix lands.

---

## Tips for Writing Good Repro Tests

1. **Reproduce the reported behavior first.** The test must initially assert the **buggy** behavior (e.g., the wrong error code the issue reports). Run it and confirm it passes — this proves your test actually hits the bug. Only then add the correct/fixed assertion. For example, if an issue reports NU1102 when it should be NU1004, first assert NU1102 passes, then update to assert NU1004. A test that asserts the fixed behavior but never reproduced the bug proves nothing.

2. **Use the simplest package graph possible.** If the issue involves `System.Management → System.CodeDom`, abstract it to `A → B`. Real package names are unnecessary.

3. **Name tests descriptively.** Follow the pattern: `RestoreCommand_<Context>_<Action>_<ExpectedResult>`. Examples:
   - `RestoreCommand_WithLockedMode_WhenPruneDataChanges_ReportsNU1004_NotNU1102`
   - `RestoreCommand_WithPrunePackageReferences_DoesNotPruneDirectDependencies`

4. **Always capture the logger.** Use `new TestLogger()` and pass it to `RunRestoreAsync` so you can assert on error/warning messages and include `logger.ShowMessages()` in failure diagnostics.

5. **Comment the package graph.** Use the convention at the top of the test:
   ```csharp
   // P -> A 1.0.0 -> B 1.0.0
   // Prune B (,1.0.0]
   ```

6. **For two-phase tests** (generate lock file, then restore in locked mode), always call `result.CommitAsync()` after the setup phase to write the lock file to disk.

7. **Use `FluentAssertions`** (`Should().Be(...)`, `Should().Contain(...)`) — this is the standard in NuGet.Client tests.

8. **One test per bug.** If the issue describes multiple problems, write separate tests for each.
