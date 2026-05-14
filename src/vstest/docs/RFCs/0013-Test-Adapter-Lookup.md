# 0013 Test Adapter Lookup

## Summary
This note outlines the proposed changes for lookup and initialization of the test adapters.

## Motivation
Following is a list of issues with current design
* Test discovery fails for a solution containing projects referring to multiple versions of the same adapter.
* Upgrade of test adapter is not honored as the adapters get cached in the host process.
* Diagnostic information related to which adapter was used for which source and what adapters are loaded in not available.

## Principles
1. The adapter referenced in the project should be used for test execution
2. Performance should not degrade.
3. Adapter lookup logic is consistent across runs.
4. Adapter lookup logic should be clear and consistent for both IDE/Editor and CLI runs.

## Proposed Changes
Here are the proposed changes based on the sources available for lookup of the adapters.

### Adapter Sources
Here is the list of sources TestPlatform looks up for test adapters.

| Source                   | Remarks                                                                        |
|--------------------------|--------------------------------------------------------------------------------|
| TestAdapterPath          | Adapter is specified by user in the runsettings file or via cli arguments      |
| Nuget                    | Adapter ships as a nuget package, doesn't copy to output directory             |
| Project output directory | Adapter ships as a nuget package, copies to output directory                   |
| Extensions directory     | Adapters packaged with VS, available in the Extensions directory               |
| VSIX                     | Adapter ships as a vsix.                                                       |

### Mulitple verions of Adapter
In case there are multiple versions of the same adapter found during the lookup, TestPlatform will choose the one with the highest version.
TestPlatform will notify the user about the conflict and which version was selected for that run.

### Details for lookup

#### Nuget
This lookup is based on the nuget package references. This works only for IDE and not for CLI, as this information is only available in IDE.
IDE invokes all the test adapters with all the test projects in the solution.
If multiple versions of same adapter, or multiple adapters with same URI found, TestPlatform will throw a warning.

* Recommendation : Use one version of the given adapter in the solution.

* Best Practice : Define a constant for nuget package version of an adapter in one file and use it across all the projects.
This also helps make the upgrade experience to a newer version of adapter seamless.
For example, open source projects like
[Roslyn](https://github.com/dotnet/roslyn) and
[VSTest](https://github.com/microsoft/vstest) use *props file to define the version of the nuget packages.

#### TestAdapterpath
TestAdapterPath can be given via runsettings or as an argument in case of cli.
All the test sources will be run against all the test adapters found in the test adapter path.
If multiple versions of same adapter, or multiple adapters with same URI found, TestPlatform will throw a warning.

* Recommendation : Have one version of an adapter in the test adapter path.

#### Project Output Directory
Some adapters packaged as nuget get copied to the output directory of the project.

Since IDE picks up the adapters from the nuget package location, there is no need to look for adapters in the project output directory.
Hence, this project output directory will be probed only in case of cli. Further, cli will probe only when testAdapterPath is not specified.

* Recommendation : Use /testadapterpath option with cli.

#### Extensions directory
All the adapters in the extensions directory get loaded by default and all the test sources based on the file extensions are passed to these adapters. Test runner will provide the diagnostics information about adapters used for the test projects.

#### VSIX
In case of IDE, all vsix based adapters get initialized by default.
In case of CLI, /UseVsixExtensions argument will be available. But user will start getting a recommendation to use /TestAdapterpath instead.

* Recommendation for adapter authors : Move to nuget based acquisition for the test adapters.
* Note : TestPlatform needs to keep supporting this lookup until all adapters can be moved to nuget packages.

### Additional diagnostics changes proposed
Here are a few changes for improving diagnosibilty of Test Platform

1. Diagnostics information should include the information for all the loaded adapters along with their URIs.
2. Information regarding which source was given to which adapter gets surfaced.
3. TestPlatform will give a warning in case an attempt is made to load multiple versions of same adapter.
