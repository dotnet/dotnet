// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace OpenTelemetry.Extensions.Enrichment.Http.Tests;

public class HttpClientEnrichmentServiceCollectionExtensionsTests
{
    [Fact]
    public void GenericMethod_AddsSingleton()
    {
        var services = new ServiceCollection();
        services.TryAddHttpClientTraceEnricher<TestEnricher>();
        var provider = services.BuildServiceProvider();
        var enricher = provider.GetService<HttpClientTraceEnricher>();
        Assert.NotNull(enricher);
        Assert.IsType<TestEnricher>(enricher);
    }

    [Fact]
    public void InstanceMethod_AddsSingleton()
    {
        var services = new ServiceCollection();
        var instance = new TestEnricher();
        services.TryAddHttpClientTraceEnricher(instance);
        var provider = services.BuildServiceProvider();
        var enricher = provider.GetService<HttpClientTraceEnricher>();
        Assert.Same(instance, enricher);
    }

    [Fact]
    public void FactoryMethod_AddsSingleton()
    {
        var services = new ServiceCollection();
        services.TryAddHttpClientTraceEnricher(_ => new TestEnricher());
        var provider = services.BuildServiceProvider();
        var enricher = provider.GetService<HttpClientTraceEnricher>();
        Assert.NotNull(enricher);
        Assert.IsType<TestEnricher>(enricher);
    }

    [Fact]
    public void AllMethods_ThrowOnNullArguments()
    {
        Assert.Throws<ArgumentNullException>(() => HttpClientEnrichmentServiceCollectionExtensions.TryAddHttpClientTraceEnricher<TestEnricher>(null!));
        Assert.Throws<ArgumentNullException>(() => HttpClientEnrichmentServiceCollectionExtensions.TryAddHttpClientTraceEnricher(null!, new TestEnricher()));
        Assert.Throws<ArgumentNullException>(() => new ServiceCollection().TryAddHttpClientTraceEnricher(null!));
        Assert.Throws<ArgumentNullException>(() => HttpClientEnrichmentServiceCollectionExtensions.TryAddHttpClientTraceEnricher(null!, _ => new TestEnricher()));
        Assert.Throws<ArgumentNullException>(() => new ServiceCollection().TryAddHttpClientTraceEnricher<TestEnricher>(null!));
    }
}
