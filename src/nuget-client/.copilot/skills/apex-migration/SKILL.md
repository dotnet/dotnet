---
name: apex-migration
description: >-
  Migrate NuGet PowerShell E2E tests to C# Apex tests or unit tests. 
Use this skill whenever the user asks to migrate, convert, or port a PowerShell end-to-end test from test/EndToEnd/tests/ to an Apex test in test/NuGet.Tests.Apex/ or to a unit test in test/NuGet.Clients.Tests/NuGetConsole.Host.PowerShell.Test/. 
Also trigger when the user mentions "Apex test", "migrate PS test", "E2E test migration", "PMC test", "cmdlet unit test", "PowerShell unit test", or references any PowerShell test function like Install-PackageTest or Update-PackageTest and wants it rewritten in C#. Even if the user just says "migrate this test" while looking at a PS E2E file, use this skill.
Also trigger whenever the user wants to write a new Apex test or a new PMC cmdlet unit test.
---

# Migrating PowerShell E2E Tests to Apex Tests

PowerShell E2E tests live in `test/EndToEnd/tests/`. Apex tests live in
`test/NuGet.Tests.Apex/NuGet.Tests.Apex/NuGetEndToEndTests/`. The goal is to migrate PS tests
to C# Apex tests that run the **exact same scenario**, then remove the PS test function.

## Workflow

1. **Read the PS test** — understand what it does: which project type, which PMC commands, what assertions.
2. **Decide: unit test or Apex test** — if it only tests cmdlet behavior (filtering, listing, prerelease logic) without needing VS project system integration, write a unit test. Otherwise use Apex.
3. **Pick the right file** — for Apex, match the scenario to an existing test class (see File Placement below). For unit tests, use the cmdlet-specific file in `Cmdlets/` (see Unit test file placement below).
4. **Translate** — use the mappings in this skill to convert each PS construct to its C# equivalent.
5. **Verify** — run `get_errors` or build the project to confirm it compiles cleanly.
6. **Remove the PS function** — delete the migrated function from the PS test file.
7. **If already covered** — if an existing Apex or unit test already covers the same scenario, just delete the PS test. No new test needed.
8. **Update this skill** — if you discovered new mappings, gotchas, or corrections, add them to the appropriate section of this file.

## File placement

Choose the target file by **interaction surface**, not project type:

| Interaction surface | Apex file |
|---|---|
| `Get-Package` command scenarios (`-ListAvailable`, `-Updates`, `-Filter`, path source variants, prerelease behaviors) | `GetPackageTestCase.cs` |
| Other PMC commands (Install-Package, Update-Package, Uninstall-Package, Get-Project) | `NuGetConsoleTestCase.cs` |
| NuGet UI / Package Manager dialog | `NuGetUITestCase.cs` |
| IVsPackageInstaller / IVsServices API | `IVsServicesTestCase.cs` |
| Sync/binding redirect scenarios | `SyncPackageTestCase.cs` |
| Audit / vulnerability scenarios | `NuGetAuditTests.cs` |
| .NET Core project-creation / restore / source-mapping | `NetCoreProjectTestCase.cs` |

PMC tests for PackageReference projects still follow command-based placement. For `Get-Package`
scenarios use `GetPackageTestCase.cs`; for other PMC commands use `NuGetConsoleTestCase.cs`.

## Template mapping

| PowerShell function | Apex `ProjectTemplate` | Package management | Verified |
|---|---|---|---|
| `New-ConsoleApplication` | `ProjectTemplate.ConsoleApplication` | packages.config | ✅ |
| `New-ClassLibrary` | `ProjectTemplate.ClassLibrary` | packages.config | ✅ |
| `New-WebSite` | `ProjectTemplate.WebSiteEmpty` | packages.config | ✅ |
| `New-WebApplication` | `ProjectTemplate.WebApplicationEmpty` | packages.config | ❌ |
| `New-WPFApplication` | `ProjectTemplate.WPFApplication` | packages.config | ❌ |
| `New-MvcApplication` | `ProjectTemplate.WebApplicationEmptyMvc` | packages.config | ❌ |
| `New-FSharpLibrary` | `ProjectTemplate.FSharpLibrary` | PackageReference | ❌ |
| `New-NetCoreConsoleApp` | `ProjectTemplate.NetCoreConsoleApp` | PackageReference | ✅ |
| `New-NetStandardClassLib` | `ProjectTemplate.NetStandardClassLib` | PackageReference | ✅ |

| PowerShell function | Apex equivalent |
|---|---|
| `New-SolutionFolder 'Name'` | `testContext.SolutionService.AddSolutionFolder("Name")` |

The package management style determines which assertion methods to use — packages.config projects
use `AssertPackageInPackagesConfig`, while PackageReference projects use `AssertPackageInAssetsFile`.

> **Note:** This table covers the most common PS project factories. Some PS tests use specialized
> factories like `New-ClassLibraryNET46`, `New-BuildIntegratedProj`, `New-UwpPackageRefClassLibrary`,
> or `New-NetCoreConsoleMultipleTargetFrameworksApp`. These don't have a 1:1 `ProjectTemplate` enum
> value — check the Apex `ProjectTemplate` enum and existing tests for the closest match, or create
> a standard template and modify the csproj afterward (e.g., for multi-targeting).

## Command execution

| Scenario | Apex API |
|---|---|
| Standard install with `-Version` | `nugetConsole.InstallPackageFromPMC(packageName, packageVersion)` |
| Install with extra flags (`-Source`, `-WhatIf`, `-IgnoreDependencies`) | `nugetConsole.Execute($"Install-Package {packageName} -ProjectName {project.Name} -Source {source}")` |
| Standard uninstall | `nugetConsole.UninstallPackageFromPMC(packageName)` |
| Standard update with `-Version` | `nugetConsole.UpdatePackageFromPMC(packageName, packageVersion)` |
| Update with `-Safe`, `-Reinstall`, etc. | `nugetConsole.Execute($"Update-Package {packageName} -Safe")` |
| Any raw PMC command | `nugetConsole.Execute(command)` |

**Key rule:** Both `InstallPackageFromPMC()` and `UpdatePackageFromPMC()` always inject `-Version`.
If the original PS test does **not** use `-Version`, use `Execute()` with the raw command string
instead — using the helper changes the semantics.

**PowerShell session state is accessible.** `nugetConsole.Execute()` runs in a live PMC PowerShell
session. It can execute **any** PowerShell command, not just NuGet commands. This means PS session
state — global variables (`$global:InstallVar`), registered functions
(`Test-Path function:\Get-World`), environment checks — can all be queried and asserted via
`Execute()` + `IsMessageFoundInPMC()`. Do not skip tests just because they assert PS session state.

**Project-level build operations:**

| Scenario | Apex API |
|---|---|
| Clean (deletes obj/bin) | `testContext.Project.Clean()` |
| Rebuild | `testContext.Project.Rebuild()` |
| Cache file path | `CommonUtility.GetCacheFilePath(testContext.Project)` |
| Wait for file to appear | `CommonUtility.WaitForFileExists(path)` |
| Wait for file to disappear | `CommonUtility.WaitForFileNotExists(path)` |

For single-project solutions, project-level Clean/Rebuild is equivalent to solution-level.

## Assertion mapping

| PowerShell assertion | Apex equivalent |
|---|---|
| `Assert-Package $p PackageName Version` (packages.config) | `CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, version, Logger)` |
| `Assert-Package $p PackageName` (no version, packages.config) | `CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, Logger)` |
| `Assert-Package $p PackageName Version` (PackageReference) | `CommonUtility.AssertPackageInAssetsFile(VisualStudio, testContext.Project, packageName, version, Logger)` |
| `Assert-Throws { ... } $expectedMessage` | `nugetConsole.IsMessageFoundInPMC(expectedMessage)` — PMC errors appear as text, not C# exceptions |
| `Assert-Null (Get-ProjectPackage ...)` / not installed | `CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, testContext.Project, packageName, Logger)` |
| `Assert-NoPackage $p PackageName Version` (PackageReference) | `CommonUtility.AssertPackageNotInAssetsFile(VisualStudio, testContext.Project, packageName, version, Logger)` |
| `Assert-PackageReference $p PackageName Version` | `CommonUtility.AssertPackageReferenceExists(VisualStudio, testContext.Project, packageName, version, Logger)` |
| `Assert-NoPackageReference $p PackageName` | `CommonUtility.AssertPackageReferenceDoesNotExist(VisualStudio, testContext.Project, packageName, Logger)` |

## Package sources

| PowerShell source | Apex equivalent |
|---|---|
| `$context.RepositoryRoot` / `$context.RepositoryPath` | `testContext.PackageSource` — create packages with `CommonUtility.CreatePackageInSourceAsync()` |
| No `-Source` (uses nuget.org) | Create a local package with `CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, ...)` — never depend on nuget.org |
| Hardcoded invalid sources (`http://example.com`, `ftp://...`) | Use the same hardcoded strings directly |

### Creating test packages

For simple packages:
```csharp
await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);
```

For packages with dependencies:
```csharp
await CommonUtility.CreateDependenciesPackageInSourceAsync(
    testContext.PackageSource, packageName, packageVersion, dependencyName, dependencyVersion);
```

For .NET Framework-specific packages:
```csharp
await CommonUtility.CreateNetFrameworkPackageInSourceAsync(
    testContext.PackageSource, packageName, packageVersion);
```

## NuGet.Config manipulation

PS tests that use `Get-VSComponentModel` + `ISettings` to modify NuGet config at runtime can be
migrated by pre-configuring `SimpleTestPathContext` before passing it to `ApexTestContext`.

**Via Settings API** (preferred):
```csharp
using var simpleTestPathContext = new SimpleTestPathContext();
simpleTestPathContext.Settings.AddSource("PrivateRepo", privatePath);

using var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger,
    simpleTestPathContext: simpleTestPathContext);
```

**Via raw config file** (for settings not covered by the API like `dependencyVersion` or `bindingRedirects`):
```csharp
using var simpleTestPathContext = new SimpleTestPathContext();
File.WriteAllText(simpleTestPathContext.NuGetConfig,
    $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <config>
        <add key=""dependencyVersion"" value=""HighestPatch"" />
    </config>
    <packageSources>
        <clear />
        <add key=""source"" value=""{simpleTestPathContext.PackageSource}"" />
    </packageSources>
</configuration>");

using var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger,
    simpleTestPathContext: simpleTestPathContext);
```

## Test structure patterns

### Error-path tests (no package creation needed) — synchronous

```csharp
[TestMethod]
[Timeout(DefaultTimeout)]
public void DescriptiveTestName_Fails()
{
    using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

    var packageName = "Rules";
    var source = @"c:\temp\data";
    var expectedMessage = $"Unable to find package '{packageName}' at source '{source}'. Source not found.";

    var nugetConsole = GetConsole(testContext.Project);
    nugetConsole.Execute($"Install-Package {packageName} -ProjectName {testContext.Project.Name} -Source {source}");

    Assert.IsTrue(
        nugetConsole.IsMessageFoundInPMC(expectedMessage),
        $"Expected error message was not found in PMC output. Actual output: {nugetConsole.GetText()}");
}
```

### Success-path tests (need package creation) — async

```csharp
[TestMethod]
[Timeout(DefaultTimeout)]
public async Task DescriptiveTestNameAsync()
{
    using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

    var packageName = "TestPackage";
    var packageVersion = "1.0.0";
    await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);

    var nugetConsole = GetConsole(testContext.Project);
    nugetConsole.InstallPackageFromPMC(packageName, packageVersion);

    CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion, Logger);
}
```

### Data-driven tests (multiple project templates)

When the same scenario applies to multiple project types, use `[DataTestMethod]` or `[DynamicData]`:
```csharp
[DataTestMethod]
[DynamicData(nameof(GetPackageReferenceTemplates), DynamicDataSourceType.Method)]
[Timeout(DefaultTimeout)]
public async Task InstallPackageAsync(ProjectTemplate projectTemplate)
{
    using var simpleTestPathContext = new SimpleTestPathContext();
    EnsurePackageReferenceFormat(simpleTestPathContext, projectTemplate);
    using var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger,
        simpleTestPathContext: simpleTestPathContext);
    // ... test body
}

// Yields both legacy and SDK-style PackageReference templates
private static IEnumerable<object[]> GetPackageReferenceTemplates()
{
    yield return new object[] { ProjectTemplate.ConsoleApplication };
    yield return new object[] { ProjectTemplate.NetCoreConsoleApp };
}

// Only legacy (ConsoleApplication) needs this; SDK projects ignore it
private static void EnsurePackageReferenceFormat(SimpleTestPathContext context, ProjectTemplate template)
{
    if (template == ProjectTemplate.ConsoleApplication)
        context.Settings.SetPackageFormatToPackageReference();
}
```

Use `[DynamicData]` over `[DataRow]` when the template set is reused across multiple tests in the
same class. This ensures one test method covers both legacy and SDK-style PackageReference projects.

### Multi-targeted project tests

To create a multi-targeted project, modify the csproj after project creation:
```csharp
using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.NetCoreConsoleApp, Logger);
// Modify csproj to multi-target via XDocument:
// change <TargetFramework> to <TargetFrameworks>net8.0;netstandard2.0</TargetFrameworks>
```

## Style rules

Follow the repo-wide coding guidelines first — they apply to all code, not just migrations:
[`docs/coding-guidelines.md`](../../../docs/coding-guidelines.md) (e.g. no `#region` blocks, no
reflection, `var` usage, nullable enabled). Don't restate or duplicate those rules here; this section
only lists conventions **specific to test migration** that supplement the common guidelines:

- Use `using var` (inline using declaration), not `using (var ...) { }`.
- Place migrated tests before the static helper methods (`GetNetCoreTemplates`, etc.) in the file.
- Method names: `{Action}FromPMC{Scenario}[_Fails|Async]`. Suffix with `_Fails` for error tests,
  `Async` for async tests.
- Always include `[Timeout(DefaultTimeout)]`.
- Always include `nugetConsole.GetText()` in assertion failure messages for diagnostics.
- The test class inherits `SharedVisualStudioHostTestClass` which provides `VisualStudio` and `Logger`.
- Get PMC console via `GetConsole(testContext.Project)` helper method in the test class.
- Don't use the method-delegates-to-async-helper pattern unless the helper is actually called from
  multiple classes. Inline the test logic directly in the test method.
- Do NOT add "Migrated from" comments in the test code. The PR description tracks the mapping — the code should stand on its own without referencing the old PS test.

## Tests that should NOT be migrated

Skip PS tests that:
- Use `Assert-BindingRedirect` — binding redirect tests are already `[SkipTest]` in PS and not
  worth migrating.
- Depend on **DTE project hierarchy semantics** (e.g., `Get-ProjectItem` to check tree structure,
  parent/child relationships). However, if the PS test only uses `Get-ProjectItem` /
  `Get-ProjectItemPath` to verify a **file exists on disk**, migrate it using filesystem assertions
  instead: `File.Exists(path)`, XML reads on the project file, or
  `CommonUtility.WaitForFileExists()`.

## After migration checklist

1. ✅ Remove the migrated function from the PS test file.
2. ✅ If a PS test is already covered by an existing Apex test (duplicate), just delete the PS
   test — no new Apex test needed.
3. ✅ Build the Apex project or run `get_errors` to verify it compiles cleanly.
4. ✅ Verify assertion methods match the project's package management style
   (packages.config vs PackageReference).

## Common gotchas

- **Console width**: PMC output assertions are text-sensitive. The Apex infrastructure forces
  console width to 1024 to avoid wrapping issues.
- **Restore timing**: After install/update operations, the Apex infrastructure handles waiting for
  restore completion. You generally don't need explicit waits.
- **nuget.org dependency**: PS tests that don't specify `-Source` implicitly use nuget.org. Always
  replace this with local package creation via `CreatePackageInSourceAsync` — tests must not depend
  on external feeds.
- **`NuGetApexTestService` limitations**: It does NOT expose `ISolutionManager` or VS DTE project
  item inspection. Only `IVsPackageInstaller`, `IVsSolutionRestoreStatusProvider`,
  `IVsPackageUninstaller`, `IVsPathContextProvider2`, and `IVsUIShell` are available.
- **IVs error-path tests**: `NuGetApexTestService.InstallPackage()` swallows
  `InvalidOperationException` and logs it — it does NOT rethrow. For error-path IVs tests, assert
  that the package was NOT installed (`AssertPackageNotInPackagesConfig`) rather than trying to
  catch exceptions.
- **Feature renaming**: Some PS tests use older feature names (e.g., "PackageNameSpace"). When
  migrating, use the current feature name (e.g., "PackageSourceMapping") in test method names and
  comments.
- **IVs tests use `EnvDTE.Project`**: IVs API methods like `InstallPackage()` take
  `project.UniqueName` (from `EnvDTE.Project`), not a `ProjectTestExtension`. Get it via
  `VisualStudio.Dte.Solution.Projects.Item(1)`.
- **`_pathContext` vs `testContext`**: `IVsServicesTestCase` uses a class-level
  `SimpleTestPathContext _pathContext` (initialized in constructor), not per-test `ApexTestContext`.
  PMC tests in `NuGetConsoleTestCase` use per-test `ApexTestContext`.
- **Never overwrite `SimpleTestPathContext`'s NuGet.config**: Using `CreateConfigurationFile` to
  write a full config replaces defaults like `globalPackagesFolder`, `fallbackPackageFolders`, and
  `httpCacheFolder` — causing packages to pollute the user's real global packages folder. Instead
  use `simpleTestPathContext.Settings.AddSource()` and `AddPackageSourceMapping()` to layer config
  on top of the defaults.
- **Legacy PackageReference projects don't auto-restore** — never call `WaitForAutoRestore()` for
  `ConsoleApplication` with `SetPackageFormatToPackageReference()`. Only SDK-style projects
  (e.g., `NetCoreConsoleApp`) auto-restore on project open.
- **Always check for existing Apex coverage before migrating** — search the Apex test files for
  equivalent scenarios. Common negative tests like `UpdatePackageFromPMCNotInstalled_Fails` and
  `UninstallPackageFromPMCNotInstalled_Fails` already exist. If covered, just delete the PS test.
- **Daily vs regular Apex cadence matters** — a test in `NuGet.Tests.Apex.Daily` does NOT count as
  coverage for removing an E2E test that runs on every PR. Only regular Apex tests
  (`NuGet.Tests.Apex`) provide equivalent gating.

## Learnings (continuously updated)

This section captures lessons learned from actual migration runs that don't fit neatly into the sections above. **Update this section after every migration run** with new discoveries or corrections.

### 2026-05-20: PackageReferenceTestCase patterns

- **PR #7246 incorrectly removed `Test-NetCoreConsoleAppClean`** — it was claimed to be covered by Apex Daily's `VerifyCacheFileInsideObjFolder`, but that test is in Daily (different cadence) and bundles 3 behaviors. We properly covered it by adding `NetCoreConsoleApp` to `GetPackageReferenceTemplates()` in `PackageReferenceTestCase.CleanDeletesCacheFile`.

---

# Migrating PowerShell E2E Tests to Unit Tests

Some E2E tests exercise **cmdlet behavior** (parameter handling, filtering, prerelease logic) rather than Visual Studio integration. These are better covered by fast unit tests that invoke the cmdlet directly in a PowerShell runspace — no VS instance needed.

## When to use unit tests vs Apex tests

| Scenario | Target |
|---|---|
| Tests that verify cmdlet parameter behavior (filtering, prerelease, -AllVersions, -Updates) | **Unit test** |
| Tests that verify VS integration (project creation, restore, UI, solution operations) | **Apex test** |
| Tests that exercise Install/Update/Uninstall side effects on actual project files | **Apex test** |
| Tests that only query package sources or list installed packages | **Unit test** |

The key distinction: if the test needs a real Visual Studio instance, solution, or project system, use Apex. If it only needs the cmdlet logic + a mock package source, use a unit test.

## Unit test project

Unit tests live in:
```
test/NuGet.Clients.Tests/NuGetConsole.Host.PowerShell.Test/Cmdlets/
```

The project (`NuGetConsole.Host.PowerShell.Test.csproj`) references:
- `NuGet.PackageManagement.PowerShellCmdlets` (the source project)
- `VisualStudio.Test.Utility` (shared test helpers)
- `Microsoft.VisualStudio.Sdk.TestFramework.Xunit` (VS mocking infrastructure)

Tests run sequentially via `xunit.runner.json`:
```json
{
  "maxParallelThreads": 1,
  "parallelizeTestCollections": false
}
```

## Test infrastructure

The reference implementation is in:
```
test/NuGet.Clients.Tests/NuGetConsole.Host.PowerShell.Test/Cmdlets/GetPackageCommandTests.cs
```

Read this file for the full working patterns. Key architectural details below.

### CmdletRunspaceFixture (inner class in test file)

Creates a real PowerShell runspace with the cmdlet registered via `SessionStateCmdletEntry`. Key points:
- Constructor takes `activeSource` (always pass `pathContext.PackageSource` — never nuget.org)
- Registers the cmdlet type (e.g., `typeof(GetPackageCommand)`) into `InitialSessionState`
- `Invoke(cmdletName, parameters)` runs the cmdlet and returns `IList<PSObject>`
- Is `IDisposable` — use with `using var`

When adding tests for a **different cmdlet** (e.g., `Install-Package`), create a new fixture class that registers that cmdlet's type instead.

### TestPSHost (inner class in test file)

A minimal `PSHost` that provides `PrivateData` with two properties NuGet cmdlets require:
- `"activePackageSource"` — the source URL/path the cmdlet defaults to
- `"CancellationTokenKey"` — a `CancellationToken` (use `CancellationToken.None` in tests)

Without these, cmdlets throw during initialization.

### VS Service Mocking (constructor)

The test class implements `IAsyncServiceProvider` and sets up mocks in the constructor:
- Takes `GlobalServiceProvider` (from xUnit collection `MockedVS.Collection`)
- Mocks `IVsSolutionManager` — must set `IsSolutionOpen = true`, `IsSolutionAvailableAsync = true`, and provide a default project via `GetDefaultNuGetProjectAsync()`
- Mocks `IComponentModel` — registers all services the cmdlet's `Preprocess()` method resolves: `ISettings`, `IVsSolutionManager`, `ISourceControlManagerProvider`, `ICommonOperations`, `IPackageRestoreManager`, `IDeleteOnRestartManager`, `IRestoreProgressReporter`, and `ISourceRepositoryProvider`
- Calls `ServiceLocator.InitializePackageServiceProvider(this)` to wire it all up

If a new cmdlet requires additional services, add them to the `IComponentModel` mock setup.

## Unit test file placement

| Cmdlet | Test file |
|---|---|
| `Get-Package` | `Cmdlets/GetPackageCommandTests.cs` |
| `Install-Package` | `Cmdlets/InstallPackageCommandTests.cs` (create if needed) |
| `Update-Package` | `Cmdlets/UpdatePackageCommandTests.cs` (create if needed) |
| `Uninstall-Package` | `Cmdlets/UninstallPackageCommandTests.cs` (create if needed) |
| `Find-Package` | `Cmdlets/FindPackageCommandTests.cs` (create if needed) |

When creating a new test file, follow the same structure as `GetPackageCommandTests.cs` — copy the class skeleton (constructor, `IAsyncServiceProvider`, inner fixture/host classes) and adapt the cmdlet type.

## Key patterns

### Creating test packages

Use `SimpleTestPathContext` + `SimpleTestPackageUtility` (same utilities as other NuGet tests):

```csharp
using var pathContext = new SimpleTestPathContext();

await SimpleTestPackageUtility.CreatePackagesAsync(
    pathContext.PackageSource,
    new SimpleTestPackageContext("PackageA", "1.0.0"),
    new SimpleTestPackageContext("PackageA", "2.0.0-beta"));
```

### Setting up the source repository provider

Register a real `ISourceRepositoryProvider` pointing at the local test source via `TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(localPath))`. See `SetupSourceRepositoryProvider()` in the reference file.

### Setting up installed packages (for -Updates tests)

Create a mock `NuGetProject` with `PackageReference` entries and wire it into `_solutionManager.GetNuGetProjectsAsync()` / `GetDefaultNuGetProjectAsync()`. See `CreateMockProject()` and `SetupProjectWithInstalledPackage()` helpers in the reference file.

### Invoking cmdlets

```csharp
using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

var results = fixture.Invoke(
    "Get-Package",
    new Dictionary<string, object>
    {
        { "ListAvailable", true },
        { "Source", pathContext.PackageSource },
        { "Filter", "TestPackage" },
    });
```

Switch parameters (`-ListAvailable`, `-AllVersions`, `-IncludePrerelease`) are passed as `true` in the dictionary.

### Asserting results

Results come back as `PSObject` wrappers. Cast `BaseObject` to the expected type:

| Scenario | BaseObject type |
|---|---|
| `-ListAvailable` | `PowerShellRemotePackage` |
| Installed (no switch) | `PowerShellInstalledPackage` |
| `-Updates` | `PowerShellUpdatePackage` |

Use FluentAssertions: `results.Should().ContainSingle()`, then cast and assert `.Id`, `.Version`, `.Versions`.

## Unit test style rules

- Use xUnit (`[Fact]`) with FluentAssertions.
- Test class uses `[Collection(MockedVS.Collection)]` and takes `GlobalServiceProvider` in constructor.
- Method names: `{CmdletName}{Scenario}_{Expectation}Async` (e.g., `GetPackageListAvailable_WithFilter_ReturnsMatchingPackageAsync`).
- Use `using var` for disposables.
- Arrange/Act/Assert pattern with comments.
- All tests are `async Task` (package creation is async).
- Use `var` for locals except value tuples.

## Unit test migration workflow

1. **Identify testable behavior** — read the PS test and determine if it's testing cmdlet logic or VS integration.
2. **Check existing unit test coverage** — look in `Cmdlets/` for the relevant command test file.
3. **Write the unit test** — use the patterns above.
4. **Build** — `dotnet build test/NuGet.Clients.Tests/NuGetConsole.Host.PowerShell.Test/NuGetConsole.Host.PowerShell.Test.csproj`
5. **Run** — `dotnet test test/NuGet.Clients.Tests/NuGetConsole.Host.PowerShell.Test/NuGetConsole.Host.PowerShell.Test.csproj`
6. **Remove the PS test function** — delete the migrated function from the PS E2E file.
7. **Keep one Apex sanity test** — if migrating *all* PS tests for a given cmdlet to unit tests, keep (or create) one Apex test that exercises the basic end-to-end flow through VS. Unit tests mock the VS layer, so a single Apex test ensures the real integration still works. This sanity test should cover multiple scenarios in a single test case to maximize breadth without adding multiple slow VS-launching tests — e.g., `GetPackageTestCase.cs` has a lifecycle test that installs, lists, updates, and uninstalls all in one method.

## Tests that should be unit tests (not Apex)

PS E2E tests that do any of the following are good candidates for unit tests:
- Only call `Get-Package` with `-ListAvailable`, `-Filter`, `-AllVersions`, `-Updates`, `-Prerelease`
- Assert returned package IDs/versions without modifying project state
- Test behavior with different source configurations
- Verify filtering/sorting logic
- Test prerelease inclusion/exclusion behavior
