// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Configuration;
using System.Web;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Examples.Wcf.Server.AspNetFramework;

#pragma warning disable SA1649 // File name should match first type name
public class WebApiApplication : HttpApplication
#pragma warning restore SA1649 // File name should match first type name
{
    private TracerProvider? tracerProvider;

    protected void Application_Start()
    {
        var builder = Sdk.CreateTracerProviderBuilder()
            .ConfigureResource(resource => resource.AddService("Wcf-AspNetServer"))
            .AddAspNetInstrumentation()
            .AddWcfInstrumentation();

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
    }

    protected void Application_End()
    {
        this.tracerProvider?.Dispose();
    }
}
