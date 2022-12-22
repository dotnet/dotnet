![NuGet logo](https://raw.githubusercontent.com/NuGet/Home/dev/resources/nuget.png)

-----

# NuGet Client Performance test suite

This directory contains a performance test suite for the NuGet command-line client implementations (currently, dotnet.exe and NuGet.exe are supported).
The scripts handle 4 different restore scenarios and collect timing, filesystem & environment data.

Both packages.config and PackageReference styles are supported, but mixed projects are not handled very well.

The scenarios are sequential as follows:

1. Clean restore - no http cache & other local caches, no files in the global package folder, absolutely everything gets downloaded and extracted.
1. Cold restore - There is only an http cache. This tells us more about the installation/extraction time. Potentially we might see some extra http calls depending on the project graph.
1. Force restore - The http cache & global packages folder are full. This usually means that there are no package downloads or installations happening.
1. NoOp restore

## How does it work

The `PerformanceTestUtilities.ps1` script is a collection of utility functions that help the execution of the performance tests.
`RunPerformanceTests.ps1` allows you to execute the full test suite given a specific solution file, a NuGet Client and an output results file.
`PerformanceTestRunner.ps1` will execute all the test cases added in the testCases folder with the `Test-*` pattern.

Note that it's very important to initialize the script from a location that does not have a global.json in it's directory path. This can skew the results if you are dealing with SDK based projects.

To run either the performance tests or the runner, run `Get-Help scriptName.ps1` and/or `Get-Help scriptName.ps1 -examples`
