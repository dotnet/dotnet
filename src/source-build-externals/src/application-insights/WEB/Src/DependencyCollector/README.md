Microsoft.ApplicationInsights.DependencyCollector
===================================================

The Dependency Collector is available on [Nuget.org](http://www.nuget.org/packages/Microsoft.ApplicationInsights.DependencyCollector/).

This package provides support for auto-collection of dependencies in .NET applications. HTTP and SQL dependency calls are intercepted and sent as Dependency events to Application Insights in the [Azure Preview Portal](https://portal.azure.com). The Dependency Collector can be installed without [Microsoft.ApplicationInsights.Web](http://www.nuget.org/packages/Microsoft.ApplicationInsights.Web/) to collect dependencies in non-web applications.

If .NET 4.6 is installed on the client machine, the Framework event listeners ([FrameworkSqlEventListener][FrameworkSqlListener] and [FrameworkHttpEventListener][FrameworkHttpListener] are used to collect dependencies. If the .NET version is older than 4.6, dependencies are collected through instrumentation using the Profiler ([ProfilerSqlProcessing][ProfilerSqlListener] and [ProfilerHttpProcessing][ProfilerHttpListener]). In order for the Profiler to collect data, the [Status Monitor][StatusMonitor] must be installed on the client machine. 

[Learn More.](https://azure.microsoft.com/en-us/documentation/articles/app-insights-asp-net-dependencies/)

[FrameworkSqlListener]: https://github.com/Microsoft/ApplicationInsights-server-dotnet/blob/develop/Src/DependencyCollector/Shared/Implementation/FrameworkSqlEventListener.cs
[FrameworkHttpListener]: https://github.com/Microsoft/ApplicationInsights-server-dotnet/blob/develop/Src/DependencyCollector/Shared/Implementation/FrameworkHttpEventListener.cs
[ProfilerSqlListener]: https://github.com/Microsoft/ApplicationInsights-server-dotnet/blob/develop/Src/DependencyCollector/Shared/Implementation/ProfilerSqlProcessing.cs
[ProfilerHttpListener]: https://github.com/Microsoft/ApplicationInsights-server-dotnet/blob/develop/Src/DependencyCollector/Shared/Implementation/ProfilerHttpProcessing.cs
[StatusMonitor]: https://azure.microsoft.com/en-us/documentation/articles/app-insights-monitor-performance-live-website-now/
