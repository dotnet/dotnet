# Reporting test results

Test discovery, execution results in a test run can be controlled with test
loggers. This document will cover details on installation, usage and authoring
of test loggers.

## Test loggers

A test logger is a test platform extension to control reporting of test results.
It can perform tasks when a test run message, individual test
results or the test run completion events are reported by the test platform.

You can author a test logger to print messages on the console, generate result
files of a specific reporting format, or even report results to various CI/CD
services. Default inputs to a test logger can be provided in the command line.

 Please refer to [this section](./report.md#create-a-test-logger) for instructions on creating a test logger and [todo]()
if you're interested in the architecture of a test logger.

### Available test loggers

| Scenario | NuGet Package | Source Repository |
| -------- | ------------- | ----------------- |
| Local, CI, CD | Inbuilt | [Trx Logger][] |
| Local, CI, CD | Inbuilt | [Console Logger][] |
| Local, CI, CD | Inbuilt | [Html Logger][] |
| Local, CI, CD | [XunitXml.TestLogger][xunit.nuget] | [Xunit Logger][] |
| Local, CI, CD | [NunitXml.TestLogger][nunit.nuget] | [Nunit Logger][] |
| Local, CI, CD | [JunitXml.TestLogger][junit.nuget] | [Junit Logger][] |
| AppVeyor | [AppVeyor.TestLogger][appveyor.nuget] | [AppVeyor Logger][] |
| Azure Pipelines | [AzurePipelines.TestLogger][azurepipelines.nuget] | [Azure Pipelines Logger][] |
| GitHub Actions | [GitHubActionsTestLogger][githubactions.nuget] | [GitHub Actions Test Logger][] |
| TeamCity | [TeamCity.VSTest.TestAdapter][teamcity.nuget] | [TeamCity Logger][] |

[Trx Logger]: https://github.com/Microsoft/vstest/tree/main/src/Microsoft.TestPlatform.Extensions.TrxLogger
[Html Logger]: https://github.com/Microsoft/vstest/tree/main/src/Microsoft.TestPlatform.Extensions.HtmlLogger
[Console Logger]: ./src/vstest.console/Internal/ConsoleLogger.cs
[Xunit Logger]: https://github.com/spekt/xunit.testlogger
[Nunit Logger]: https://github.com/spekt/nunit.testlogger
[Junit Logger]: https://github.com/spekt/junit.testlogger
[AppVeyor Logger]: https://github.com/spekt/appveyor.testlogger
[Azure Pipelines Logger]: https://github.com/daveaglick/AzurePipelines.TestLogger
[GitHub Actions Test Logger]: https://github.com/Tyrrrz/GitHubActionsTestLogger
[TeamCity Logger]: https://github.com/JetBrains/TeamCity.VSTest.TestAdapter

[xunit.nuget]: https://www.nuget.org/packages/XunitXml.TestLogger
[nunit.nuget]: https://www.nuget.org/packages/NUnitXml.TestLogger/
[junit.nuget]: https://www.nuget.org/packages/JUnitXml.TestLogger/
[appveyor.nuget]: https://www.nuget.org/packages/AppVeyor.TestLogger
[azurepipelines.nuget]: https://www.nuget.org/packages/AzurePipelines.TestLogger
[githubactions.nuget]: https://www.nuget.org/packages/GitHubActionsTestLogger
[teamcity.nuget]: https://www.nuget.org/packages/TeamCity.VSTest.TestAdapter

 Want to add your logger? Please send a PR with changes in this doc.

## Acquisition

A test logger should be made available as a NuGet package (preferred), or as
a zip file (for e.g. loggers for C++ etc.).

If it's a NuGet package, the test logger assemblies should get copied to the
build output directory. When looking for a test logger, vstest will look for
them in the same directory as the test assemblies. In most cases this means that
test projects reference either the project (same codebase) or NuGet package 
with the logger.

If the test logger is made available as a zip file, it should be extracted
to one of the following locations:

1. the `Extensions` folder along side `vstest.console.exe`. E.g. in case of
dotnet-cli, the path could be `/sdk/<version>/Extensions` directory.
2. any well known location on the filesystem

> [!NOTE]
> **New in 15.1**
>
> In case of #2, user can specify the full path to the location using `/TestAdapterPath:<path>`
> command line switch. Test platform will locate extensions from the provided
> directory.

## Naming

Test platform will look for assemblies named `*.testlogger.dll` when it's trying
to load test loggers.

> [!NOTE]
> For the 15.0 version, the test loggers are also discovered from `*.testadapter.dll`

## Create a test logger

Go through the following steps to create your own logger

1) Add a nuget reference of package `Microsoft.TestPlatform.ObjectModel`.
2) Implement `ITestLoggerWithParameters` (or `ITestLogger`, if your logger is not expecting any parameters). [Logger Example](https://github.com/spekt/xunit.testlogger/blob/49d2416f24acb30225adc6e65753cc829010bec9/src/Xunit.Xml.TestLogger/XunitXmlTestLogger.cs#L19)
3) Name your logger assembly `*.testlogger.dll`. [Detailed](./report.md#naming)

## Enable a test logger

A test logger must be explicitly enabled using the command line. E.g.

```shell
 vstest.console test_project.dll /logger:mylogger
```

Where `mylogger` is the `LoggerUri` or `FriendlyName` of the logger.

## Configure reporting

Additional arguments to a logger can also be passed in the command line. E.g.

```shell
 vstest.console test_project.dll /logger:mylogger;Setting=Value
```

Where `mylogger` is the `LoggerUri` or `FriendlyName` of the logger.
`Setting` is the name of the additional argument and `Value` is its value.

It is up to the logger implementation to support additional arguments.

## Syntax of default loggers

### 1) Console logger

Console logger is the default logger and it is used to output the test results to a terminal.

#### Syntax

For dotnet test or dotnet vstest:

```shell
--logger:console[;verbosity=<Defaults to "minimal">]
```

For vstest.console.exe:

```shell
/logger:console[;verbosity=<Defaults to "normal">]
```
 
Argument `verbosity` defines the verbosity level of the console logger. Allowed values for verbosity are `quiet`, `minimal`, `normal` and `detailed`.

#### Example

```shell
vstest.console.exe Tests.dll /logger:"console;verbosity=normal"
```

If you are using `dotnet test`, then use the following command:

```shell
dotnet test Tests.csproj --logger:"console;verbosity=normal"
```

or you can also use argument `-v | --verbosity` of `dotnet test`:

```shell
dotnet test Tests.csproj -v normal
```

### 2) Trx logger

Trx logger is used to log test results into a Visual Studio Test Results File (TRX).

#### Syntax

```shell
/logger:trx [;LogFileName=<Defaults to unique file name>]
```

Where `LogFileName` can be absolute or relative path. If the path is relative, it will be relative to the `TestResults` directory, created under current working directory.


#### Examples

Suppose the current working directory is `c:\tempDirectory`.

```shell
vstest.console.exe Tests.dll /logger:trx
```

trx file will get generated in location `c:\tempDirectory\TestResults`.

```shell
vstest.console.exe Tests.dll /logger:"trx;LogFileName=relativeDir\logFile.txt"

trx file will be `c:\tempDirectory\TestResults\relativeDir\logFile.txt`.

```shell
vstest.console.exe Tests.dll /logger:"trx;LogFileName=c:\temp\logFile.txt"
```

trx file will be `c:\temp\logFile.txt`.

### 3) Html logger

Html logger is used to log test results into a HTML file.

#### Syntax

```shell
/logger:html [;LogFileName=<Defaults to unique file name>]

Where "LogFileName" can be absolute or relative path. If path is relative, it will be relative to "TestResults" directory, created under current working directory.

```

#### Examples

Suppose the current working directory is `c:\tempDirectory`.

```shell
vstest.console.exe Tests.dll /logger:html
```

HTML file will get generated in location `c:\tempDirectory\TestResults`.

```shell
vstest.console.exe Tests.dll /logger:"html;LogFileName=relativeDir\logFile.html"
```

HTML file will be `c:\tempDirectory\TestResults\relativeDir\logFile.html`.

```shell
vstest.console.exe Tests.dll /logger:"html;LogFileName=c:\temp\logFile.html"
```

HTML file will be `c:\temp\logFile.html`.

## Related links

TODO: link to author a test logger
