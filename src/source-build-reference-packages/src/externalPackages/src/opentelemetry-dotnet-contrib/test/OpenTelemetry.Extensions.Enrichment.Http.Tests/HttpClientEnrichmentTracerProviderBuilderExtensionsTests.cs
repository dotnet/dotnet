// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Extensions.Enrichment.Http.Tests;

public class HttpClientEnrichmentTracerProviderBuilderExtensionsTests
{
    [Fact]
    public void GenericMethod_AddsSingleton()
    {
        IServiceCollection? captured = null;
        using var provider = Sdk.CreateTracerProviderBuilder()
            .TryAddHttpClientTraceEnricher<TestEnricher>()
            .ConfigureServices(services => captured = services)
            .Build();

        var serviceProvider = captured!.BuildServiceProvider();
        var enricher = serviceProvider.GetService<HttpClientTraceEnricher>();
        Assert.NotNull(enricher);
        Assert.IsType<TestEnricher>(enricher);
    }

    [Fact]
    public void InstanceMethod_AddsSingleton()
    {
        var instance = new TestEnricher();
        IServiceCollection? captured = null;
        using var provider = Sdk.CreateTracerProviderBuilder()
            .TryAddHttpClientTraceEnricher(instance)
            .ConfigureServices(services => captured = services)
            .Build();

        var serviceProvider = captured!.BuildServiceProvider();
        var enricher = serviceProvider.GetService<HttpClientTraceEnricher>();
        Assert.Same(instance, enricher);
    }

    [Fact]
    public void FactoryMethod_AddsSingleton()
    {
        IServiceCollection? captured = null;
        using var provider = Sdk.CreateTracerProviderBuilder()
            .TryAddHttpClientTraceEnricher(_ => new TestEnricher())
            .ConfigureServices(services => captured = services)
            .Build();

        var serviceProvider = captured!.BuildServiceProvider();
        var enricher = serviceProvider.GetService<HttpClientTraceEnricher>();
        Assert.NotNull(enricher);
        Assert.IsType<TestEnricher>(enricher);
    }

    [Fact]
    public void AllMethods_ThrowOnNullArguments()
    {
        TracerProviderBuilder? builder = null;
        Assert.Throws<ArgumentNullException>(() => builder!.TryAddHttpClientTraceEnricher<TestEnricher>());
        Assert.Throws<ArgumentNullException>(() => builder!.TryAddHttpClientTraceEnricher(new TestEnricher()));
        Assert.Throws<ArgumentNullException>(() => builder!.TryAddHttpClientTraceEnricher(_ => new TestEnricher()));
        Assert.Throws<ArgumentNullException>(() => Sdk.CreateTracerProviderBuilder().TryAddHttpClientTraceEnricher(null!));
        Assert.Throws<ArgumentNullException>(() => Sdk.CreateTracerProviderBuilder().TryAddHttpClientTraceEnricher<TestEnricher>(null!));
    }
}
