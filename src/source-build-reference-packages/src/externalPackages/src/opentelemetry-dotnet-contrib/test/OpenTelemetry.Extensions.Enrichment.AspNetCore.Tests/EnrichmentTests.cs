// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Extensions.Enrichment.AspNetCore.Tests;

public sealed class EnrichmentTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    public EnrichmentTests(WebApplicationFactory<Program> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task BasicEnrichment()
    {
        var exportedItems = new List<Activity>();
        var path = "/api/values";

        // Arrange
        using var traceprovider = Sdk.CreateTracerProviderBuilder()
            .AddAspNetCoreInstrumentation()
            .TryAddAspNetCoreTraceEnricher<MyAspNetCoreTraceEnricher>()
            .AddInMemoryExporter(exportedItems)
            .Build();

        using (var client = this.factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders());
            })
            .CreateClient())
        {
            try
            {
                using var response = await client.GetAsync(new Uri(path, UriKind.Relative));
            }
            catch (Exception)
            {
                // ignore errors
            }

            WaitForActivityExport(exportedItems, 1);
        }

        Assert.Single(exportedItems);
        var activity = exportedItems[0];

        // Assert enrichment tags from MyAspNetCoreTraceEnricher
        var tags = activity.TagObjects.ToDictionary(t => t.Key, t => t.Value);
        Assert.Contains(MyAspNetCoreTraceEnricher.Key + ".request", tags.Keys);
        Assert.Contains(MyAspNetCoreTraceEnricher.Key + ".response", tags.Keys);
        Assert.Equal(1, (int)tags[MyAspNetCoreTraceEnricher.Key + ".request"]!);
        Assert.Equal(1, (int)tags[MyAspNetCoreTraceEnricher.Key + ".response"]!);
    }

    private static void WaitForActivityExport(List<Activity> exportedItems, int count)
    {
        // We need to let End callback execute as it is executed AFTER response was returned.
        // In unit tests environment there may be a lot of parallel unit tests executed, so
        // giving some breezing room for the End callback to complete
        Assert.True(
            SpinWait.SpinUntil(
            () =>
            {
                Thread.Sleep(10);
                return exportedItems.Count >= count;
            },
            TimeSpan.FromSeconds(1)),
            $"Actual: {exportedItems.Count} Expected: {count}");
    }
}
