![Build Status](https://mseng.visualstudio.com/DefaultCollection/_apis/public/build/definitions/96a62c4a-58c2-4dbb-94b6-5979ebc7f2af/2513/badge)



## NuGet packages

- [Microsoft.ApplicationInsights.Web](https://www.nuget.org/packages/Microsoft.ApplicationInsights.Web/)
[![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.Web.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.Web)
- [Microsoft.ApplicationInsights.DependencyCollector](https://www.nuget.org/packages/Microsoft.ApplicationInsights.DependencyCollector/)
[![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.DependencyCollector.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.DependencyCollector)
- [Microsoft.ApplicationInsights.EventCounterCollector](https://www.nuget.org/packages/Microsoft.ApplicationInsights.EventCounterCollector)
[![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.EventCounterCollector.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.EventCounterCollector)
- [Microsoft.ApplicationInsights.PerfCounterCollector](https://www.nuget.org/packages/Microsoft.ApplicationInsights.PerfCounterCollector/)
[![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.PerfCounterCollector.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.PerfCounterCollector)
- [Microsoft.ApplicationInsights.WindowsServer](https://www.nuget.org/packages/Microsoft.ApplicationInsights.WindowsServer/)
[![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.WindowsServer.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.WindowsServer)
- [Microsoft.AspNet.ApplicationInsights.HostingStartup](https://www.nuget.org/packages/Microsoft.AspNet.ApplicationInsights.HostingStartup/)
[![Nuget](https://img.shields.io/nuget/vpre/Microsoft.AspNet.ApplicationInsights.HostingStartup.svg)](https://nuget.org/packages/Microsoft.AspNet.ApplicationInsights.HostingStartup)




# Visual Studio Application Insights SDK for .NET Web Applications

The code in this repository is the .NET web application SDK for Application Insights. [Application Insights][AILandingPage] is a service that lets you monitor your live application's performance and usage. This SDK sends telemetry to the service. It collects data such as web request timings and success rates, dependency calls, exceptions, and server performance counters. You can also use the SDK to send your own telemetry and add modules to collect logs. You can use this SDK in any .NET web application, hosted either on your own servers or on Microsoft Azure.

## Get the SDK

The SDK is installed on each project by the Application Insights tools in Visual Studio (2013 and later).

To [add Application Insights to your project in Visual Studio][AddInVS]:

* If you're creating a new project, check **Add Application Insights** in the New Project dialog.
* If it's an existing project, right-click your project in Solution Explorer and select **Add Application Insights** or **Update Application Insights**.
* If these options aren't available for your project type, use Extension Manager in Visual Studio to install or update the NuGet package. Create a [new Application Insights resource][CreateResource] in the Azure portal, obtain its instrumentation key, and insert that in ApplicationInsights.config.

Run your project, and then [open your Application Insights resource][WebDocumentation] in the [Azure Preview Portal][AzurePortal] and look for events.


## To upgrade to the latest SDK 

* After you upgrade, you'll need to merge back any customizations you made to ApplicationInsights.config. If you're unsure whether you customized it, create a new project, add Application Insights to it, and compare your `.config` file with the one in the new project. Make a note of any differences.
* In Solution Explorer, right-click your project and choose **Manage NuGet packages**.
* Set the filter to show Updates. 
* Select **Microsoft.ApplicationInsights.Web** and choose **Update**. (This will also upgrade all the dependent packages.)
* Compare ApplicationInsights.config with the old copy. Most of the changes you'll see are because we removed some modules and made others parameterizable. Reinstate any customizations you made to the old file.
* Rebuild your solution.

## To build
Follow [contributor's guide](https://github.com/Microsoft/ApplicationInsights-dotnet-server/blob/develop/CONTRIBUTING.md)

## Branches
- [master][master] contains the *latest* published release located on [NuGet][WebNuGet].
- [develop][develop] contains the code for the *next* release.

## Shared Projects

Our projects target multiple frameworks (ex: Net45 & NetCore). We have framework specific projects and a shared project for common files between them. (ex: Perf.Net45, Perf.NetCore, Perf.Shared). If a file is used by both frameworks, we prefer to store that file in a Shared project and use preprocessor directives to separate framework specific code (ex: `#if NETCORE, #if !NETCORE`). We also use a conditional ItemGroup to assign files to a framework (ex: `ItemGroup Condition=" '$(TargetFramework)' != 'netcoreapp1.0' "`).

We've found that this makes our projects easier to maintain because it keeps Framework assignments in a single project. As an added bonus our Framework specific projects can include a single Shared project instead of individual files, which keeps our project files neat and clean.

## Contributing

We strongly welcome and encourage contributions to this project. Please read the [contributor's guide](https://github.com/Microsoft/ApplicationInsights-dotnet-server/blob/develop/CONTRIBUTING.md). If making a large change we request that you open an [issue][GitHubIssue] first. If we agree that an issue is a bug, we'll add the "bug" label, and issues that we plan to fix are labeled with an iteration number. We follow the [Git Flow][GitFlow] approach to branching.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

[Azure]: https://azure.com/
[AILandingPage]: http://azure.microsoft.com/services/application-insights/
[AzurePortal]: https://portal.azure.com/
[WebDocumentation]: https://azure.microsoft.com/documentation/articles/app-insights-asp-net/#monitor
[master]: https://github.com/Microsoft/ApplicationInsights-server-dotnet/tree/master/
[develop]: https://github.com/Microsoft/ApplicationInsights-server-dotnet/tree/develop/
[GitFlow]: http://nvie.com/posts/a-successful-git-branching-model/
[ContribGuide]: https://github.com/Microsoft/ApplicationInsights-server-dotnet/blob/develop/CONTRIBUTING.md/
[GitHubIssue]: https://github.com/Microsoft/ApplicationInsights-server-dotnet/issues/
[WebNuGet]: https://www.nuget.org/packages/Microsoft.ApplicationInsights.Web/
[MyGet]:http://myget.org/gallery/applicationinsights/
[AddInVS]:https://azure.microsoft.com/documentation/articles/app-insights-asp-net/#ide
[CreateResource]: https://azure.microsoft.com/documentation/articles/app-insights-create-new-resource/
