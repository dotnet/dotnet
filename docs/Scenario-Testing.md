# The Unified Build Almanac (TUBA) - Scenario Testing

## Introduction

Unified Build seeks to make .NET easier to build, contribute to, and distribute by all parties, including Microsoft. To achieve this, we need good testing. .NET today lacks solid automated scenario testing against *installed* products (or other shipping outputs). That is not to say that there are not tests for .NET. On the contrary, there are a large quantity of manual scenario validation tests, and *extensive* repository level tests. These repository level tests provide great coverage, but they do not run against the final product. They typically run against a subset. Individual assemblies, subsets of the SDK layout, etc. They do in-depth testing of specific behaviors. There are few automated tests that are designed to run against an installed product. This presents several challenges:
- Source-build partners have difficulty validating that their builds are working as expected. Most of .NET's repo testing is not applicable to source-build.
- .NET may have difficulty validating that we have not regressed product behavior when we change around how it builds.

It is time to add a formal set of automated scenario tests to .NET. Scenario tests run against an installed product have a couple qualities:
- Because they run over installed products, they are agnostic of build methodology.
- They have the ability to validate wide swaths of behavior and provide a good general read on product quality.

## Goals

The Unified Build scenario testing effort has the following goals:
- Enable .NET maintainers (including existing source-build partners) and Microsoft to validate installed products prior to shipping.
- Comply with the provenance and build/test environment requirement needs of a variety of .NET maintainers. Maintainers should be able to target a set of tests that meet their requirements. These needs may vary. Examples:
  - Maintainers may require no pre-builts when building tests.
  - Maintainers may allow online tests (access to internet resources) or may require tests to run offline.
- Be able to provide a general read on product quality. Cover breadth over depth.
- Be able to cover both source-built and traditionally built products.
- Avoid complicated pre-test setups.

## Requirements

Unified Build scenario testing has the following requirements:

- Tests shall not require a build layout to execute.
- Tests shall run against *shipping assets of .NET* or *installed .NET products*. For instance:
  - **Allowed** - Tests that verify installability/uninstallability of MSIs, PKGs, debian installers, etc.
  - **Allowed** - Tests that verify behavior of an extracted or installed SDK. Installed covers all present and future supported acquisition methods.
  - **Allowed** - Tests that verify behavior of an extracted or installed runtime. Installed covers all present and future supported acquisition methods.
  - **Allowed** - Tests that reference a shipping NuGet package and verify behavior.
  - **Not Allowed** - Tests that run against 'pre-final' or non-shipping outputs.
  - **Not Allowed** - Tests that require a repository build layout to run.
- Tests shall be tagged such that a specific subset can be targeted based on an installed product or available build output.
- Tests shall be executable by all/any project contributors.
- Tests and the test harness shall be designed so .NET distro maintainers can meet provenance and build/test environment requirements. This means:
  - Tests shall be runnable against internal only products, meaning that any resources required for tests must not assume public availabiloty. For example:
    - Shipping NuGet package feeds used as inputs to tests may be public or internal
    - Tests that work to install the product may pull from public or internal sources.
  - Tests shall be selectable/tagged based on the resources they require for execution
  - Tests shall be excludable/includable in the test preparation build based on their source-buildability.
  - The test harness shall be source-buildable for platforms/organizations that require it.
- Test execution shall be compatible with, *but be agnostic of*, distributed execution systems like Helix.
- Tests shall run against products built with Microsoft's traditional build methods, as well as products built in the VMR or source-build.

## High-Level Design

### Harness Design

.NET scenario tests will use C# and standard xunit, with a library of utility functionality to perform common test tasks (e.g. executing `dotnet` commands against an installed product). Test projects will be built as self-contained applications, so that an installed SDK product is not required to execute those tests. If an installed product was required, this would interfere with the desired test environment. A similar model has been used already in the mobile and wasm configurations today as well as runtime.

### Test Reporting

.NET scenario tests will use the standard xunit reporting format. This format is human readable and compatible with a variety of CI systems.

### Test Execution Model

Test logic should not execute tests in-proc. Instead, it should test functionality out-of-proc against installed products or shipping outputs, using a library of functionality to perform common actions. This is similar to how dotnet/sdk tests today. For example, take an SDK test that executes a pack command. The PackCommand class implements basic functionality to execute pack commands (via Process.Start), then the output is compared against an expected layout.

```csharp
public void It_packs_successfully()
{
    var helloWorldAsset = _testAssetsManager
        .CopyTestAsset("HelloWorld", "PackHelloWorld")
        .WithSource();

    var packCommand = new PackCommand(Log, helloWorldAsset.TestRoot);

    packCommand
        .Execute()
        .Should()
        .Pass();

    //  Validate the contents of the NuGet package by looking at the generated .nuspec file, as that's simpler
    //  than unzipping and inspecting the .nupkg
    string nuspecPath = packCommand.GetIntermediateNuspecPath();
    var nuspec = XDocument.Load(nuspecPath);

    var ns = nuspec.Root.Name.Namespace;
    XElement filesSection = nuspec.Root.Element(ns + "files");

    var fileTargets = filesSection.Elements().Select(files => files.Attribute("target").Value).ToList();

    var expectedFileTargets = new[]
    {
        $@"lib\{ToolsetInfo.CurrentTargetFramework}\HelloWorld.runtimeconfig.json",
        $@"lib\{ToolsetInfo.CurrentTargetFramework}\HelloWorld.dll"
    }.Select(p => p.Replace('\\', Path.DirectorySeparatorChar));

    fileTargets.Should().BeEquivalentTo(expectedFileTargets);
}
```

### Test Location

Tests shall be kept in https://github.com/dotnet/scenario-tests, which shall be vendored into the VMR alongside product source. This allows the tests to be runnable against a product without the VMR (e.g. against Microsoft's traditionally built product).

### Test Qualities

Tests should seek to test end-to-end functionality, and generally focus on product breadth over depth for specific areas. For example, a good test might use an installed SDK to build a self-contained application that reads, alters, and writes json files. The application would be run against a set of known inputs, and the output compared in each case.

## Scenario Priorization

When writing new tests, .NET should prioritize, in order:
- Platforms currently being shipped via source-build.
- Areas that currently lack coverage
- Platforms targeted for source-build bring up sooner.

.NET distro maintainers who utilize Linux source-build currently do not have a lot coverage, aside from some basic smoke testing and additional tests that they develop themselves (e.g. https://github.com/redhat-developer/dotnet-regular-tests). We primarily rely on the repo tests to validate the source-built product will work as expected. But the repo tests run within the traditional Microsoft build process, which means they may not give a good read on source-built products. Filling those gaps first not only gives those .NET maintainers better coverage, but also helps .NET ensure that as it switches away from the Microsoft traditional build for Linux builds it distributes (e.g. Linux portable), that quality does not slip.

## What about...

#### Visual Studio testing?

While there is nothing preventing testing Visual Studio scenarios in this model, Visual Studio testing should not appear within dotnet/scenario-tests. .NET's scenario tests should focus on the primary outputs of its build. Those outputs that can be validated *without* another phase of insertions. Visual Studio scenarios would require a .NET build and a VS insertion and build before validation could begin. Tests that target those scenarios are more suited to be located within various Visual Studio test suites.

#### Tests with binary dependencies?

Tests with binary dependencies may exist. However, because the VMR may not contain most kinds of binaries (these are typically automatically stripped away in the VMR sync), binaries must be kept in a separate repository. Dependency flow (via packages or other versioned containers) is used to version the binaries within the scenario-tests repo. Of course, this makes the scenario-tests repo not usable for some organizations. Use of these binaries (either in build scenarios or in tests scenarios) must be properly identified via test tags or build metadata to ensure that those .NET distro maintainers can meet provenance and build/test environment requirements.
