# OpAMP Client for OpenTelemetry .NET

| Status | |
| ------ | --- |
| Stability | [Development](../../README.md#Development) |
| Code Owners | [@RassK](https://github.com/RassK), [@stevejgordon](https://github.com/stevejgordon) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.OpAmp.Client)](https://www.nuget.org/packages/OpenTelemetry.OpAmp.Client)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.OpAmp.Client)](https://www.nuget.org/packages/OpenTelemetry.OpAmp.Client)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-OpAmp.Client)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-OpAmp.Client)

## Steps to use OpenTelemetry.OpAmp.Client

### Step 1: Install Package

Add a reference to the
[`OpenTelemetry.OpAmp.Client`](https://www.nuget.org/packages/OpenTelemetry.OpAmp.Client)
package.

```shell
dotnet add package --prerelease OpenTelemetry.OpAmp.Client
```

### Step 2: Create the OpAMP Client at application startup

```csharp
using OpenTelemetry.OpAmp.Client;
using OpenTelemetry.OpAmp.Client.Settings;

var client = new OpAmpClient(opts =>
{
    // Set up the OpAMP server connection.
    // Supported options are HTTP (polling) and WebSocket connection.
    opts.ServerUrl = new Uri("wss://localhost:4318/v1/opamp");
    opts.ConnectionType = ConnectionType.WebSocket;

    // Add custom resources to help the server identify your client.
    opts.Identification.AddIdentifyingAttribute("application.name", "my-application");
    opts.Identification.AddNonIdentifyingAttribute("application.version", "1.0.0");
});

// Start the client to send the identification message and get ready to receive messages.
await client.StartAsync();

// The client also supports StopAsync() for proper shutdown and to unregister from the server.
await client.StopAsync();
```

## References

* [Open Agent Management Protocol](https://opentelemetry.io/docs/specs/opamp/)
