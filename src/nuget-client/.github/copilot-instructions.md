# Instructions

## General Guidelines

- When creating pull requests, always follow the [PR template](PULL_REQUEST_TEMPLATE.md).
- Always format before submitting a pull request.
- Before implementing any code changes, read all files in the `docs/` folder. It contains the NuGet development guidelines, including rules for SDKAnalysisLevel gating, public API policies, error handling patterns, and feature design requirements.
- Do not edit xlf files directly. These are generated from resx files and any manual changes will be overwritten. Instead, edit the resx files and build to update the xlf files.

## Coding Standards

- Use the following coding guidelines: https://github.com/NuGet/NuGet.Client/blob/dev/docs/coding-guidelines.md
- Never use reflection.
- When using value tuples, never use `var` (e.g., `var result = Method()`), but always use decomposed names (e.g., `(var name, var value) = Method()`).

## Project-Specific Rules

- All files in the repository are nullable by default (project-level nullable enable). No need to add `#nullable enable` directives to individual files.

## Nullable Migration Rules

- **Shipped.txt format must be precise** — e.g. `string![]!` not `string![]`, `byte[]?` not `byte?[]`. Always match the format of existing base class entries in the same file.
- **`~` (oblivious) entries get replaced in place** — replace the `~` prefixed line with the annotated line in `PublicAPI.Shipped.txt`. Do not add to `PublicAPI.Unshipped.txt`.
- **Internal types don't need Shipped.txt updates** — only public API surfaces require `PublicAPI.Shipped.txt` changes.
- **Don't suppress nullability with `!`** when the value genuinely can be null — make the type honest, let callers handle it.
- **Covariant return nullability** — `byte[]` override of `byte[]?` base is valid in C# 9+. Use it when a subclass guarantees non-null.
- **`Debug.Assert(x != null)` + `x!` can be replaced** by removing both when the parameter is non-null typed and all callers are nullable-enabled.
- **`required` on private/internal types** is cleaner than `null!` field initializers.
- **TryCreate/TryGet patterns** — out params need `?`, callers use `!` after the success guard. Out parameters that are guaranteed non-null when the method returns true should be annotated with `[NotNullWhen(true)]`. Don't annotate `[NotNullWhen]` unless it's actually true for all code paths.
- **Work in batches** — group related files, fix source, fix cascading, build, repeat. If this means we need multiple pull requests for enabling nullable, that's fine. Don't try to do it all in one go.

## Migrating PowerShell E2E Tests to Apex Tests

### Overview

PowerShell E2E tests live in `test/EndToEnd/tests/`. Apex tests live in `test/NuGet.Tests.Apex/NuGet.Tests.Apex/NuGetEndToEndTests/`. The goal is to migrate PS tests to C# Apex tests that run the **exact same scenario**, then remove the PS test function.

### Template mapping

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

### Command execution

| Scenario | Apex API |
|---|---|
| Standard install with `-Version` | `nugetConsole.InstallPackageFromPMC(packageName, packageVersion)` |
| Install with extra flags (`-Source`, `-WhatIf`, `-IgnoreDependencies`) | `nugetConsole.Execute($"Install-Package {packageName} -ProjectName {project.Name} -Source {source}")` |
| Standard uninstall | `nugetConsole.UninstallPackageFromPMC(packageName)` |
| Standard update | `nugetConsole.UpdatePackageFromPMC(packageName, packageVersion)` |
| Any raw PMC command | `nugetConsole.Execute(command)` |

**Rule:** If the PS test does not use `-Version`, use `Execute()` with the raw command string. `InstallPackageFromPMC()` always adds `-Version`.

**Important:** `nugetConsole.Execute()` runs in a live PMC PowerShell session. It can execute **any** PowerShell command, not just NuGet commands. This means PS session state — global variables (`$global:InstallVar`), registered functions (`Test-Path function:\Get-World`), environment checks — can all be queried and asserted via `Execute()` + `IsMessageFoundInPMC()`. Do not skip tests just because they assert PS session state.

### Assertion mapping

| PowerShell assertion | Apex equivalent |
|---|---|
| `Assert-Package $p PackageName Version` (packages.config project) | `CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, version, Logger)` |
| `Assert-Package $p PackageName` (no version, packages.config) | `CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, Logger)` |
| `Assert-Package $p PackageName Version` (PackageReference project) | `CommonUtility.AssertPackageInAssetsFile(VisualStudio, testContext.Project, packageName, version, Logger)` |
| `Assert-Throws { ... } $expectedMessage` | `nugetConsole.IsMessageFoundInPMC(expectedMessage)` — PMC errors appear as text, not C# exceptions |
| `Assert-Null (Get-ProjectPackage ...)` / package not installed | `CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, testContext.Project, packageName, Logger)` |

### Package sources

| PowerShell source | Apex equivalent |
|---|---|
| `$context.RepositoryRoot` or `$context.RepositoryPath` | `testContext.PackageSource` — create packages with `CommonUtility.CreatePackageInSourceAsync()` |
| No `-Source` (uses nuget.org) | Create a local package with `CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, ...)` — never depend on nuget.org |
| Hardcoded invalid sources (`http://example.com`, `ftp://...`) | Use the same hardcoded strings directly |

### NuGet.Config manipulation

PS tests that use `Get-VSComponentModel` + `ISettings` to modify NuGet config at runtime can be migrated by pre-configuring `SimpleTestPathContext` before passing it to `ApexTestContext`.

**Via Settings API** (preferred):
```csharp
using var simpleTestPathContext = new SimpleTestPathContext();
simpleTestPathContext.Settings.AddSource("PrivateRepo", privatePath);
// ... then pass it in:
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

### Test structure patterns

**Error-path tests** (no package creation needed) — synchronous:
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

**Success-path tests** (need package creation) — async:
```csharp
[TestMethod]
[Timeout(DefaultTimeout)]
public async Task DescriptiveTestNameAsync(/* or [DataTestMethod] with ProjectTemplate */)
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

### Style rules

- Use `using var` (inline declaration), not `using (var ...) { }`.
- Place migrated tests before the static helper methods (`GetNetCoreTemplates`, etc.) in the file.
- Method names: `{Action}FromPMC{Scenario}[_Fails|Async]`. Suffix with `_Fails` for error tests, `Async` for async tests.
- Always include `[Timeout(DefaultTimeout)]`.
- Include `nugetConsole.GetText()` in assertion failure messages for diagnostics.

### Tests that should NOT be migrated

Skip PS tests that:
- Use `Assert-BindingRedirect` — binding redirect tests are already `[SkipTest]` in PS and not worth migrating.
- Use `Get-ProjectItem`, `Get-ProjectItemPath`, or other VS DTE project-item inspection not available in Apex.

### After migration

1. Remove the migrated function from the PS test file.
2. If a PS test is already covered by an existing Apex test (duplicate), just delete the PS test — no new Apex test needed.
3. Verify with `get_errors` that the Apex file compiles cleanly.
