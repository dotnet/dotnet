# 0010 Source Information for discovered tests

## Summary
This note outlines the proposed change in behavior of source information collection for test methods discovered by test adapters.
This change is applicable for test discovery from:
1. Visual Studio Test Explorer (IDE) for managed test projects which are supported by .NET Compiler Platform (Roslyn)
2. Vstest.console.exe runner (CLI) 

## Motivation
a. Performance improvement in IDE scenario because of two factors:
* Use of better performant Roslyn APIs instead of reflection based approach on PDBs
* Reduced test method payload transfer from adapter to Test Explorer

b. Performance improvement in CLI scenario:
* Source information is not required in CLI scenario and can be skipped entirely

## Overview of changes
Visual Studio Test Explorer needs source information for discovered tests to power following 3 scenarios:
1. Navigation to test method
2. Test method execution from code editor context menu
3. Code lens markers

At present, for managed (C# and VB) test projects, this information is obtained from PDBs using reflection based APIs. While this approach works, it has perf impact.
Performance can be improved by using Roslyn APIs which gather this information during compile time.

During discovery, Visual Studio Test Explorer groups the test containers on basis of their target framework (E.g.: NET46, NetCore etc.). 
For each group a separate discovery request is sent to the adapters. 
If all test containers in a group are supported by Roslyn, then for that discovery request, Visual Studio Test Explorer sets **'CollectSourceInformation'** flag in 
runsettings with value 'false' and passes it to the test adapters, else it is set to 'true'.
Test adapters can rely on this flag and skip source information collection for discovered tests, if the flag is set to 'false'. In such cases, Visual Studio Test Explorer
will determine source information for the discovered tests.

In case of CLI, source information is not required. Hence this flag is set to 'false' in the runsettings.

### A sample runsettings file with this flag:
```xml
<RunSettings>
    <RunConfiguration>
      <CollectSourceInformation>False</CollectSourceInformation>
    </RunConfiguration>
</RunSettings>
```

Following are the perf numbers obtained for this flag in IDE scenario for MSTest and XUnit test adapters on TPv2 for a test project having 25K tests:

| Adapter         | Without Changes = A (Sec) | With Changes = B (Sec) | Improvement, C = A - B (Sec) | Improvement (%) = C * 100 / A |
|-----------------|---------------------------|------------------------|------------------------------|-------------------------------|
| MSTest V2       |  27.28                    |  26.09                 |  1.19                        |  4.36                         |
| XUnit           | 129.97                    |  79.32                 | 50.65                        | 38.97                         |


## How to test this change
While the changes for CLI will be available in a future release, changes for IDE are already available in Visual Studio 2017 Update 3 Preview 3 and newer builds but is hidden behind a feature flag.
Steps to turn ON the feature flag:
1. Close all instances of Visual Studio
2. Locate Microsoft.VisualStudio.FeatureFlags.pkgdef file in Visual Studio installation folder. This file generally gets installed at:
C:\Program Files (x86)\Microsoft Visual Studio\Preview\Enterprise\Common7\IDE\CommonExtensions\Platform\Shell
3. Take a backup of this file
4. Open the file in a text editor and location following key:
[$RootKey$\FeatureFlags\TestingTools\UnitTesting\UseSipToFetchSourceInformation]
5. Update its dword value from 00000000 to 00000001
6. Save the file and update it in the Visual Studio location mentioned in (2)
7. Open Developer Command prompt in administrator mode and run following command:
   devenv /UpdateConfiguration
8. Launch Visual Studio and do test discovery for a managed (C# or VB) test project

To disable the feature, restore backup file in the Visual Studio location mentioned in (2) and run step (7)

Note that for perf improvement to show up, the test adapter needs to be updated to honor 'CollectSourceInformation' flag. In MSTest adapter this functionality has been added in v1.2.0-beta.