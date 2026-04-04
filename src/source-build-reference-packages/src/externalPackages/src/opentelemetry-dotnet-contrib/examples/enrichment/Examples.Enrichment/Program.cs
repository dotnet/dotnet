// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Extensions.Enrichment;
using OpenTelemetry.Trace;

namespace Examples.Enrichment;

internal static class Program
{
    public static void Main()
    {
        // Create an ActivitySource.
        using var myActivitySource = new ActivitySource("MyCompany.MyProduct.MyLibrary");

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()

            // Register MyService in the DI container.
            .ConfigureServices(services => services.AddSingleton<IMyService, MyService>())

            // Register the ActivitySource as usual.
            .AddSource("MyCompany.MyProduct.MyLibrary")

            // Register an enricher class.
            // Important: TryAddTraceEnricher() must be called before any exporters.
            .TryAddTraceEnricher<MyTraceEnricher>()

            // Add Console exporter to see the output of this example.
            .AddConsoleExporter()

            .Build();

        // Create an Activity and add some tags to it.
        using var activity = myActivitySource.StartActivity("SayHello");
        activity?.SetTag("hello", "world");

        // Tags from the enricher class will be added automatically to the created Activity.
    }
}
