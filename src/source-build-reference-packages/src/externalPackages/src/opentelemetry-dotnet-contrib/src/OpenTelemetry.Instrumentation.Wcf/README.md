# WCF Instrumentation for OpenTelemetry .NET

| Status | |
| ------ | --- |
| Stability | [Beta](../../README.md#beta) |
| Code Owners | [@open-telemetry/dotnet-contrib-maintainers](https://github.com/orgs/open-telemetry/teams/dotnet-contrib-maintainers) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Instrumentation.Wcf)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.Wcf/)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Instrumentation.Wcf)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.Wcf/)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Instrumentation.Wcf)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Instrumentation.Wcf)

Instruments WCF clients and/or services.

The following configurations are verified to work with this instrumentation.
Other configurations may work as well but have not been tested.

* SOAP and JSON payloads on HTTP/HTTPS transport
* SOAP payloads on Net.TCP transport
  * Encrypted and unencrypted transports
  * Streamed and buffered transfer modes
  * Signed and unsigned messages

> [!CAUTION]
> This component is based on the OpenTelemetry semantic conventions for
[traces](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/rpc/rpc-spans.md).
> These conventions are
> [in Development](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/document-status.md),
> and hence, this package is a [pre-release](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/VERSIONING.md#pre-releases).
> Until a [stable
> version](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/telemetry-stability.md)
> is released, there can be breaking changes.

## Installation

Add the OpenTelemetry.Instrumentation.Wcf package via NuGet.

## OpenTelemetry Configuration

Use the `AddWcfInstrumentation` extension method to add WCF instrumentation to
an OpenTelemetry TracerProvider:

```csharp
using var openTelemetry = Sdk.CreateTracerProviderBuilder()
    .AddWcfInstrumentation()
    .Build();
```

Instrumentation can be configured via options overload for
`AddWcfInstrumentation` method:

```csharp
using var openTelemetry = Sdk.CreateTracerProviderBuilder()
    .AddWcfInstrumentation(options =>
    {
        options.SuppressDownstreamInstrumentation = false;

        // Enable enriching an activity after it is created.
        options.Enrich = (activity, eventName,  message) =>
        {
            var wcfMessage = message as Message;

            // For event names, please refer to string constants in
            // WcfEnrichEventNames class.
            if (eventName == WcfEnrichEventNames.AfterReceiveReply)
            {
                activity.SetTag(
                    "companyname.messagetag",
                    wcfMessage.Properties["CustomProperty"]);
            }
        };
    })
    .Build();
```

## WCF Client Configuration (.NET Framework)

Configure the `TelemetryEndpointBehaviorExtensionElement` on the clients
you want to instrument:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <system.serviceModel>
    <extensions>
      <behaviorExtensions>
        <add name="telemetryExtension" type="OpenTelemetry.Instrumentation.Wcf.TelemetryEndpointBehaviorExtensionElement, OpenTelemetry.Instrumentation.Wcf"   />
      </behaviorExtensions>
    </extensions>
    <behaviors>
      <endpointBehaviors>
        <behavior name="telemetry">
          <telemetryExtension />
        </behavior>
      </endpointBehaviors>
    </behaviors>
    <bindings>
      <netTcpBinding>
        <binding name="netTCPConfig">
          <security mode="None" />
        </binding>
      </netTcpBinding>
    </bindings>
    <client>
      <endpoint address="net.tcp://localhost:9090/Telemetry" binding="netTcpBinding" bindingConfiguration="netTCPConfig" behaviorConfiguration="telemetry" contract="Examples.Wcf.IStatusServiceContract" name="StatusService_Tcp" />
    </client>
  </system.serviceModel>
</configuration>
```

Example project available in
[examples/wcf/client-netframework](../../examples/wcf/client-netframework/)
folder.

## WCF Client Configuration (.NET Core)

Add the `TelemetryEndpointBehavior` extension to the clients you want to instrument:

```csharp
StatusServiceClient client = new StatusServiceClient(binding, remoteAddress);
client.Endpoint.EndpointBehaviors.Add(new TelemetryEndpointBehavior());
```

Example project available in
[examples/wcf/client-core](../../examples/wcf/client-core/) folder.

## WCF Server Configuration (.NET Framework)

### Option 1: Instrument by endpoint

To add the `IDispatchMessageInspector` instrumentation to select endpoints of a
service, use the endpoint behavior extension on the service endpoints you want
to instrument:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <system.serviceModel>
    <extensions>
      <behaviorExtensions>
        <add name="telemetryExtension" type="OpenTelemetry.Instrumentation.Wcf.TelemetryEndpointBehaviorExtensionElement, OpenTelemetry.Instrumentation.Wcf" />
      </behaviorExtensions>
    </extensions>
    <behaviors>
      <endpointBehaviors>
        <behavior name="telemetry">
          <telemetryExtension />
        </behavior>
      </endpointBehaviors>
    </behaviors>
    <bindings>
      <netTcpBinding>
        <binding name="netTCPConfig">
          <security mode="None" />
        </binding>
      </netTcpBinding>
    </bindings>
    <services>
      <service name="Examples.Wcf.Server.StatusService">
        <endpoint binding="netTcpBinding" bindingConfiguration="netTCPConfig" behaviorConfiguration="telemetry" contract="Examples.Wcf.IStatusServiceContract" />
        <host>
          <baseAddresses>
            <add baseAddress="net.tcp://localhost:9090/Telemetry" />
          </baseAddresses>
        </host>
      </service>
    </services>
  </system.serviceModel>
</configuration>
```

Example project available in
[examples/wcf/server-netframework](../../examples/wcf/server-netframework/)
folder.

### Option 2: Instrument by service

To add the `IDispatchMessageInspector` instrumentation for all endpoints of a
service, use the service behavior extension on the services you want to
instrument:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <system.serviceModel>
    <extensions>
      <behaviorExtensions>
        <add name="telemetryExtension" type="OpenTelemetry.Instrumentation.Wcf.TelemetryServiceBehaviorExtensionElement, OpenTelemetry.Instrumentation.Wcf" />
      </behaviorExtensions>
    </extensions>
    <behaviors>
      <serviceBehaviors>
        <behavior name="telemetry">
          <telemetryExtension />
        </behavior>
      </serviceBehaviors>
    </behaviors>
    <bindings>
      <netTcpBinding>
        <binding name="netTCPConfig">
          <security mode="None" />
        </binding>
      </netTcpBinding>
    </bindings>
    <services>
      <service name="Examples.Wcf.Server.StatusService" behaviorConfiguration="telemetry">
        <endpoint binding="netTcpBinding" bindingConfiguration="netTCPConfig" contract="Examples.Wcf.IStatusServiceContract" />
        <host>
          <baseAddresses>
            <add baseAddress="net.tcp://localhost:9090/Telemetry" />
          </baseAddresses>
        </host>
      </service>
    </services>
  </system.serviceModel>
</configuration>
```

## WCF Programmatic Configuration Server and/or Client (.NET Framework + .NET Core)

To programmatically add instrumentation for servers (.NET Framework only) and/or
for clients (.NET Framework & .NET Core), use the `TelemetryContractBehaviorAttribute`
on the service contracts you want to instrument:

```csharp
    [ServiceContract(
        Namespace = "http://opentelemetry.io/",
        Name = "StatusService",
        SessionMode = SessionMode.Allowed)]
    [TelemetryContractBehavior]
    public interface IStatusServiceContract
    {
        [OperationContract]
        Task<StatusResponse> PingAsync(StatusRequest request);
    }
```

## Advanced configuration

This instrumentation can be configured to change the default behavior by using
`WcfInstrumentationOptions`.

### RecordException

This instrumentation automatically sets Activity Status to Error if an unhandled
exception is thrown. Additionally, `RecordException` feature may be turned on,
to store the exception to the Activity itself as ActivityEvent. This feature
applies both on instrumented servers and clients.

## References

* [OpenTelemetry Project](https://opentelemetry.io/)
