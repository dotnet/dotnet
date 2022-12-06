# 0021 - Code Coverage for .NET Core
## Summary
This note provides an overview of the support for Code Coverage in .NET Core.

## Motivation
The asks for Code Coverage support for .NET Core are the most commented issue on vstest repo:
- [https://github.com/Microsoft/vstest/issues/981](https://github.com/Microsoft/vstest/issues/981)
- [https://github.com/Microsoft/vstest/issues/1312](https://github.com/Microsoft/vstest/issues/1312)

NOTE:
1. We are starting by enabling code coverage on .NET Core for Windows.
2. The functionality will enable support for .portable/embedded PDBs.
3. The coverage information will still be emitted as a .coverage file. Support for alternate forms of rendering will be considered separately in a subsequent effort.
4. Support for Linux and Mac will be considered separately in a subsequent effort.

## Scenarios to support
Code coverage collection will continue to be enabled with a dedicated gesture. The following table summarizes the support that needs to be added:

| Entry point | How will code coverage be enabled? | Syntax                                                               |
|-------------|------------------------------------|----------------------------------------------------------------------|
|dotnet test CLI              | Through a switch to condition data collection | `dotnet test --collect:"Code Coverage"`   |
|dotnet vstest CLI            | Through a switch to condition data collection | `dotnet vstest --collect:"Code Coverage"` |
|VS IDE                       | Through menu commands                         | `"Analyze Code Coverage for Selected Test"` context menu in the VS Test Explorer, and via `"Test/Analyze Code Coverage"` menu item. |
|VSTS CI                      | through vstest.console (see below)            |                                           |
|vstest.console.exe CLI       | Through a switch to condition data collection | `vstest.console.exe /Collect:"Code Coverage"` (if vstest is invoked programmatically then this conditioning will be via vstest's translation layer API). |

NOTE:
These gestures explicitly do not need the user to specify the path of the code coverage data collector. There is spike planned to ascertain feasibility. If it turns out to be infeasible the user will need to provide a path to the code coverage datacollector using `/TestAdapterPath`.

## Support Matrix
The support matrix is in terms of the following:
- __Core Runner__ aka `dotnet test`/`dotnet vstest`/`dotnet vstest.console.dll`, built for .NET Core
- __Desktop runner__ aka `vstest.console.exe`, and built for .NET Framework.

Here is how these runners can be installed:
- dotnet SDK installation
- Visual Studio installation

| Installation  | Runner         | Test target framework | Will code coverage be supported? |
|---------------|----------------|-----------------------|----------------------------------|
| dotnet SDK    | Core runner    | .NET Core             | Yes                              |
| dotnet SDK    | Core runner    | .NET Framework        | Yes                              |
| Visual Studio | Desktop runner | .NET Core             | Yes                              |
| Visual Studio | Desktop runner | .NET Framework        | Yes                              |

## Acquisition
### Potential acquisition approaches
#### Option 1: bundle code coverage binaries into the dotnet SDK.
__Pros:__ Simple user experience. No additional installation required by the user. As long as the user has the latest .NET Core SDK, code coverage lights up.

__Cons:__ dotnet SDK is cross-plat and, as mentioned, the current code coverage effort focuses on .NET Core (Windows) only. Thus code coverage binaries will get restored even on non-Windows machines, where it will not work (yet).

#### Option 2: bundle code coverage binaries as a separate NuGet package, and make the test platform SDK depend on it.
- Ship the code coverage binaries as a separate NuGet package. Repurpose the [Microsoft.CodeCoverage package](https://www.nuget.org/packages/Microsoft.CodeCoverage/) that we already ship on NuGet.
- Make that a dependency for our [test platform SDK](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk/).
- Now during a NuGet restore that happens in any context (CI, IDE, CLI), the required code coverage binaries will get automatically installed.

__Pros:__ Simple user experience. No additional installation required by the user. As long as the user's project is using the latest test platform SDK, code coverage lights up.

__Cons:__ The test platform SDK is cross-plat, and as mentioned the current code coverage effort will focus on .NET Core (Windows) only. Thus code coverage binaries will get restored even on non-Windows machines, where it will not work (yet).

#### Option 3: ship code coverage binaries as a separate NuGet package, that the user has to explicitly reference.
- Ship the code coverage binaries as a separate NuGet package. Repurpose the [Microsoft.CodeCoverage package](https://www.nuget.org/packages/Microsoft.CodeCoverage/) that we already ship on NuGet, and version it at say 15.8.
- Rename the Shims package
	- Ship the shim functionality that we currently ship in [Microsoft.CodeCoverage package](https://www.nuget.org/packages/Microsoft.CodeCoverage/) in a new Microsoft.CodeCoverage.Shim1.0 package.
	- Make that new shim package a dependency for our test platform SDK ([Microsoft.NET.Test.SDK](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk/)) for v15.8.
	- Now during a NuGet restore that happens in any context (CI, IDE, CLI), the required shim package will get automatically installed.

__Pros:__ The test platform SDK does not need to be carry the code coverage binaries. However, even if the test platform SDK did carry the code coverage binaries, the bloat will be fairly low: ~5 MB.

__Cons:__ Poor user experience. User has to explicitly add the code coverage NuGet package reference to every test project for which for which they want to collect coverage.

### Chosen option
To provide a good user experience, we will go with __Option 2__.

## Work involved
- [x] CLI support to condition code coverage collection via a switch [this is already in place]
- [x] Spike to validate early drops of portable PDB support.
- [x] Publish as an RFC on GitHub.
- [x] Port the TraceDataCollector to .NET Standard.
- [x] Spike for automatic discovery of TraceDataCollector (for the case where dotnet test is used to run tests targeting .NET Framework)
- [x] The code coverage profiler (covrun32.dll/covrun64.dll) to support portable PDB [this will use MSDIA]
- [x] The code coverage profiler (covrun32.dll/covrun64.dll) to support embedded PDB [this will use MSDIA]
- [x] The code coverage Logger(CodeCoverage.exe) and code coverage profiler to be appropriately packaged so they are available to the .NET Core user
- [x] "preview" to NuGet
- [x] Beta to NuGet
- [x] Blog on DevOps blog
- [x] MSDN doc updated
- [x] RTW to NuGet

## Ship date
Microsoft.NET.Test.Sdk v15.8 (Q3 2018).
