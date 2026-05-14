# ASP.NET Instrumentation for OpenTelemetry

| Status | |
| ------ | --- |
| Stability | [Stable](../../README.md#stable) |
| Code Owners | [@open-telemetry/dotnet-contrib-maintainers](https://github.com/orgs/open-telemetry/teams/dotnet-contrib-maintainers) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Instrumentation.AspNet)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.AspNet)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Instrumentation.AspNet)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.AspNet)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Instrumentation.AspNet)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Instrumentation.AspNet)

This is an [Instrumentation
Library](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/glossary.md#instrumentation-library),
which instruments [ASP.NET](https://docs.microsoft.com/aspnet/overview) and
collects metrics and traces about incoming web requests.

## Steps to enable OpenTelemetry.Instrumentation.AspNet

### Step 1: Install Package

Add a reference to the
[`OpenTelemetry.Instrumentation.AspNet`](https://www.nuget.org/packages/opentelemetry.instrumentation.aspnet)
package. Also, add any other instrumentations & exporters you will need.

```shell
dotnet add package OpenTelemetry.Instrumentation.AspNet
```

### Step 2: Modify Web.config

#### Include HttpModule

`OpenTelemetry.Instrumentation.AspNet` requires adding an additional HttpModule
to your web server. This additional HttpModule is shipped as part of
[`OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule`](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule/)
which is implicitly referenced by `OpenTelemetry.Instrumentation.AspNet`. The
following shows the changes required to your `Web.config` when using IIS web server.

```xml
<system.webServer>
    <modules>
        <add
            name="TelemetryHttpModule"
            type="OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule,
                OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"
            preCondition="integratedMode,managedHandler" />
    </modules>
</system.webServer>
```

#### Configure assembly binding

When adding `OpenTelemetry.Instrumentation.AspNet` and its dependencies to a
.NET Framework project, you may encounter assembly version conflicts. Visual
Studio will typically warn you about these conflicts during build. To resolve
them, add binding redirects to the `<runtime>` section of your `Web.config` file.

The specific binding redirects needed will vary depending on your project's other
dependencies. Examples of assemblies that may require binding redirects include:

```xml
<runtime>
  <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
    <dependentAssembly>
      <assemblyIdentity name="System.Buffers" culture="neutral" publicKeyToken="cc7b13ffcd2ddd51" />
      <bindingRedirect oldVersion="0.0.0.0-4.0.5.0" newVersion="4.0.5.0" />
    </dependentAssembly>
    <dependentAssembly>
      <assemblyIdentity name="System.Memory" culture="neutral" publicKeyToken="cc7b13ffcd2ddd51" />
      <bindingRedirect oldVersion="0.0.0.0-4.0.5.0" newVersion="4.0.5.0" />
    </dependentAssembly>
    <dependentAssembly>
      <assemblyIdentity name="System.Threading.Tasks.Extensions" culture="neutral" publicKeyToken="cc7b13ffcd2ddd51" />
      <bindingRedirect oldVersion="0.0.0.0-4.2.4.0" newVersion="4.2.4.0" />
    </dependentAssembly>
  </assemblyBinding>
</runtime>
```

> [!NOTE]
> Always follow the specific binding redirect suggestions provided by Visual Studio's
> build warnings or errors for your project. You can double-click the warning in
> Visual Studio to automatically add the required binding redirects.
> The assembly versions and redirects needed may differ from the examples above.

### Step 3: Enable ASP.NET Instrumentation at application startup

ASP.NET instrumentation must be enabled at application startup. This is
typically done in the `Global.asax.cs`.

#### Traces

The following example demonstrates adding ASP.NET instrumentation with the
extension method `.AddAspNetInstrumentation()` on `TracerProviderBuilder` to
an application. This example also sets up
the OTLP (OpenTelemetry Protocol) exporter, which requires adding the package
[`OpenTelemetry.Exporter.OpenTelemetryProtocol`](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.OpenTelemetryProtocol/README.md)
to the application.

```csharp
using OpenTelemetry;
using OpenTelemetry.Trace;

public class WebApiApplication : HttpApplication
{
    private TracerProvider tracerProvider;
    protected void Application_Start()
    {
        this.tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddAspNetInstrumentation()
            .AddOtlpExporter()
            .Build();
    }
    protected void Application_End()
    {
        this.tracerProvider?.Dispose();
    }
}
```

#### Metrics

The following example demonstrates adding ASP.NET instrumentation with the
extension method `.AddAspNetInstrumentation()` on `MeterProviderBuilder` to
an application. This example also sets up
the OTLP (OpenTelemetry Protocol) exporter, which requires adding the package
[`OpenTelemetry.Exporter.OpenTelemetryProtocol`](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.OpenTelemetryProtocol/README.md)
to the application.

```csharp
using OpenTelemetry;
using OpenTelemetry.Metrics;

public class WebApiApplication : HttpApplication
{
    private MeterProvider meterProvider;
    protected void Application_Start()
    {
        this.meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddAspNetInstrumentation()
            .AddOtlpExporter()
            .Build();
    }
    protected void Application_End()
    {
        this.meterProvider?.Dispose();
    }
}
```

#### List of metrics produced

The instrumentation is implemented based on [metrics semantic
conventions](https://github.com/open-telemetry/semantic-conventions/blob/v1.29.0/docs/http/http-metrics.md#metric-httpserverrequestduration).
Currently, the instrumentation supports the following metric.

| Name | Instrument Type | Unit | Description |
| ---- | --------------- | ---- | ----------- |
| `http.server.request.duration` | Histogram | `s` | Duration of HTTP server requests. |

## Advanced trace configuration

This instrumentation can be configured to change the default behavior by using
`AspNetTraceInstrumentationOptions`, which allows configuring `Filter` as explained
below.

### Trace Filter

> [!NOTE]
> OpenTelemetry has the concept of a
[Sampler](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/sdk.md#sampling).
When using `OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule` the
`url.path` tag is supplied automatically to samplers when telemetry is started
for incoming requests. It is recommended to use a sampler which inspects
`url.path` (as opposed to the `Filter` option described below) in order to
perform filtering as it will prevent child spans from being created and bypass
data collection for anything NOT recorded by the sampler. The sampler approach
will reduce the impact on the process being instrumented for all filtered
requests.

This instrumentation by default collects all incoming HTTP requests. It
allows filtering of requests by using the `Filter` function in
`AspNetTraceInstrumentationOptions`. This defines the condition for allowable
requests. The Filter receives the `HttpContextBase` of the incoming request, and
does not collect telemetry about the request if the Filter returns false or
throws exception.

The following code snippet shows how to use `Filter` to only allow GET requests.

```csharp
this.tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddAspNetInstrumentation(
        (options) => options.Filter =
            (httpContext) =>
            {
                // only collect telemetry about HTTP GET requests
                return httpContext.Request.HttpMethod.Equals("GET");
            })
    .Build();
```

### Trace Enrich

This instrumentation library provides `EnrichWithHttpRequest`,
`EnrichWithHttpResponse` and `EnrichWithException` options that can be used to
enrich the activity with additional information from the raw `HttpRequestBase`,
`HttpResponseBase` and `Exception` objects respectively. These actions are called
only when `activity.IsAllDataRequested` is `true`. They contain the activity
itself (which can be enriched) and the actual raw object.

The following code snippet shows how to enrich the activity using all 3
different options.

```csharp
this.tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddAspNetInstrumentation(o =>
    {
        o.EnrichWithHttpRequest = (activity, httpRequest) =>
        {
            activity.SetTag("physicalPath", httpRequest.PhysicalPath);
        };
        o.EnrichWithHttpResponse = (activity, httpResponse) =>
        {
            activity.SetTag("responseType", httpResponse.ContentType);
        };
        o.EnrichWithException = (activity, exception) =>
        {
            if (exception.Source != null)
            {
                activity.SetTag("exception.source", exception.Source);
            }
        };
    })
    .Build();
```

[Processor](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/trace/extending-the-sdk/README.md#processor)
is the general extensibility point to add additional properties to any activity.
The `EnrichWithHttpRequest`, `EnrichWithHttpResponse`, and `EnrichWithException`
options are specific to this instrumentation, and are provided to get
access to `HttpRequestBase`, `HttpResponseBase`, and `Exception` respectively.

### RecordException

This instrumentation automatically sets Activity Status to Error if an unhandled
exception is thrown. Additionally, `RecordException` feature may be turned on,
to store the exception to the Activity itself as ActivityEvent.

## Advanced metric configuration

This instrumentation can be configured to change the default behavior by using
`AspNetMetricsInstrumentationOptions` as explained below.

### Metric Enrich

This option allows one to enrich the metric with additional information from
the `HttpContextBase`. The `EnrichWithHttpContext` action is always called
unless the metric was filtered. The callback allows for modifying the tag list.
If the callback throws an exception the metric will still be recorded.

```csharp
this.meterProvider = Sdk.CreateMeterProviderBuilder()
    .AddAspNetInstrumentation(options => options.EnrichWithHttpContext =
        (HttpContextBase context, ref TagList tags) =>
    {
        // Add request content type to the metric tags.
        if (!string.IsNullOrEmpty(context.Request.ContentType))
        {
            tags.Add("custom.content.type", context.Request.ContentType);
        }
    })
    .Build();
```

## References

* [ASP.NET](https://dotnet.microsoft.com/apps/aspnet)
* [OpenTelemetry Project](https://opentelemetry.io/)
