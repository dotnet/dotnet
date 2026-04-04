// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Configuration;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Examples.AspNet;

#pragma warning disable SA1649 // File name should match first type name
public class WebApiApplication : HttpApplication
#pragma warning restore SA1649 // File name should match first type name
{
    private TracerProvider? tracerProvider;
    private MeterProvider? meterProvider;

    protected void Application_Start()
    {
        var builder = Sdk.CreateTracerProviderBuilder()
             .AddAspNetInstrumentation()
             .AddHttpClientInstrumentation();

        switch (ConfigurationManager.AppSettings["UseExporter"].ToUpperInvariant())
        {
            case "OTLP":
                builder.AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint = new Uri(ConfigurationManager.AppSettings["OtlpEndpoint"]);
                    });
                break;
            default:
                builder.AddConsoleExporter(options => options.Targets = ConsoleExporterOutputTargets.Debug);
                break;
        }

        this.tracerProvider = builder.Build();

        // Metrics
        // Note: Tracerprovider is needed for metrics to work
        // https://github.com/open-telemetry/opentelemetry-dotnet/issues/2994

        var meterBuilder = Sdk.CreateMeterProviderBuilder()
             .AddAspNetInstrumentation();

        switch (ConfigurationManager.AppSettings["UseMetricsExporter"].ToUpperInvariant())
        {
            case "OTLP":
                meterBuilder.AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri(ConfigurationManager.AppSettings["OtlpEndpoint"]);
                });
                break;
            case "PROMETHEUS":
                meterBuilder.AddPrometheusHttpListener();
                break;
            default:
                meterBuilder.AddConsoleExporter((exporterOptions, metricReaderOptions) =>
                {
                    exporterOptions.Targets = ConsoleExporterOutputTargets.Debug;
                });
                break;
        }

        this.meterProvider = meterBuilder.Build();

        GlobalConfiguration.Configure(WebApiConfig.Register);

        AreaRegistration.RegisterAllAreas();
        RouteConfig.RegisterRoutes(RouteTable.Routes);
    }

    protected void Application_End()
    {
        this.tracerProvider?.Dispose();
        this.meterProvider?.Dispose();
    }
}
