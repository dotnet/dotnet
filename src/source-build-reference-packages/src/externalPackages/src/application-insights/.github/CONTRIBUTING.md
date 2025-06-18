# How to Contribute

- If making a large change we request that you open an [issue](https://github.com/Microsoft/ApplicationInsights-dotnet/issues) first. 
- We follow the [Git Flow](http://nvie.com/posts/a-successful-git-branching-model/) approach to branching. 
- This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Solutions

- **Everything.sln** - this contains all projects and all tests.
- **ProjectsForSigning.sln** - this contains all shipping projects without tests.
- **BASE\Microsoft.ApplicationInsights.sln** - this contains the Base SDK and ServerTelemetryChannel.
- **WEB\src\Microsoft.ApplicationInsights.Web.sln** - this contains the ASP.NET projects.
- **NETCORE\ApplicationInsights.AspNetCore.sln** - this contains the .NET Core projects.
- **LOGGING\Logging.sln** - this contains the logging adapters.
- **examples\Examples.sln** - this contains example apps which demonstrate configuration concepts.

## Pre-requisites:

We depend on the [.NET CLI](https://docs.microsoft.com/dotnet/core/tools/) to build these projects/solutions.
To successfully build the sources on your machine, make sure you've installed the following prerequisites:
- Visual Studio 2022 Community, Professional or Enterprise
- .NET SDKs (https://dotnet.microsoft.com/download)
    - .NET Framework 4.5.2
    - .NET Framework 4.6.0
    - .NET Framework 4.6.1
    - .NET Framework 4.6.2
    - .NET Framework 4.7.2
    - .NET Framework 4.8.0
    - .NET Framework 4.8.1
    - .NET Core 3.1
    - .NET 6
    - .NET 7

Note: .NET has an annual release cycle and we include the preview version in our test matrix.
Visual Studio requires a setting to compile using these preview versions:
  - Tools > Options > Environment > Preview Features > "Use previews of the .NET SDK".

## Build

Solutions can be built in either Visual Studio or via .NET CLI `dotnet build` ([link](https://docs.microsoft.com/dotnet/core/tools/dotnet-build)).

See also our [GitHub Workflows](/.github/workflows) for examples of building these solutions.

## Testing

Unit tests can be run in either the Visual Studio Test Exploror or via .NET CLI `dotnet test` ([link](https://docs.microsoft.com/dotnet/core/tools/dotnet-test)).

This repo also has some Integration Tests which cannot be run as a standalone DLL and must be run in the context of their project (*.csproj).
For more information please visit [Integration tests in ASP.NET Core](https://docs.microsoft.com/aspnet/core/test/integration-tests).

See also our [GitHub Workflows](/.github/workflows) for examples of running these tests.

## Debugging the SDK in general

The "Examples.sln" has some preconfigured applications. These projects can be customized and used to debug the SDK.
