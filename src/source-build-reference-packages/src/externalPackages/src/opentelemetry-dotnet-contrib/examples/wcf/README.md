# WCF Instrumentation for OpenTelemetry .NET - Examples

Project structure:

* Examples.Wcf.Client.DotNet

  An example of how to call a WCF service with OpenTelemetry instrumentation on
  .NET.

* Examples.Wcf.Client.NetFramework

  An example of how to call a WCF service with OpenTelemetry instrumentation on
  .NET Framework.

* Examples.Wcf.Server.NetFramework

  An example of how to implement a WCF service with OpenTelemetry
  instrumentation on .NET Framework. The service will listen on http which
  requires special permission on Windows. The easiest thing to do is run VS with
  admin privileges. For details see: [Configuring HTTP and
  HTTPS](https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/configuring-http-and-https).

> [!NOTE]
 > There is no .NET Core server example because only the client libraries
 for WCF are available on .NET Core / .NET 5.

* Examples.Wcf.Shared

  Contains the shared contract and request & response objects.
