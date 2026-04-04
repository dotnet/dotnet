// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Extensions.Enrichment.AspNetCore.Tests;

public sealed class AspNetCoreTraceEnrichmentProviderBuilderExtensionsTests
{
    [Fact]
    public void GenericOverload_RegistersEnricherInDI()
    {
        var services = new ServiceCollection();

        services.AddOpenTelemetry()
            .WithTracing(builder => builder
                .TryAddAspNetCoreTraceEnricher<MyAspNetCoreTraceEnricher>());

        using var sp = services.BuildServiceProvider();

        var enrichers = sp.GetServices<AspNetCoreTraceEnricher>().ToArray();
        Assert.Single(enrichers);
        Assert.IsType<MyAspNetCoreTraceEnricher>(enrichers[0]);
    }

    [Fact]
    public void InstanceOverload_RegistersExactInstance()
    {
        var services = new ServiceCollection();
        var instance = new MyAspNetCoreTraceEnricher();

        services.AddOpenTelemetry()
            .WithTracing(builder => builder
                .TryAddAspNetCoreTraceEnricher(instance));

        using var sp = services.BuildServiceProvider();

        var resolved = sp.GetRequiredService<IEnumerable<AspNetCoreTraceEnricher>>()
            .OfType<MyAspNetCoreTraceEnricher>()
            .Single();

        Assert.Same(instance, resolved);
    }

    [Fact]
    public void FactoryOverload_RegistersCreatedInstance()
    {
        var services = new ServiceCollection();
        MyAspNetCoreTraceEnricher? created = null;

        services.AddOpenTelemetry()
            .WithTracing(builder => builder
                .TryAddAspNetCoreTraceEnricher(_ =>
                {
                    created = new MyAspNetCoreTraceEnricher();
                    return created;
                }));

        using var sp = services.BuildServiceProvider();

        var enricher = sp.GetRequiredService<IEnumerable<AspNetCoreTraceEnricher>>()
            .OfType<MyAspNetCoreTraceEnricher>()
            .Single();

        Assert.Same(created, enricher);
    }

    [Fact]
    public void MultipleEnricherRegistrations_SingleProcessorAdded()
    {
        var services = new ServiceCollection();

        services.AddOpenTelemetry()
            .WithTracing(builder => builder
                .TryAddAspNetCoreTraceEnricher<MyAspNetCoreTraceEnricher>()
                .TryAddAspNetCoreTraceEnricher<MyAspNetCoreTraceEnricher2>()
                .TryAddAspNetCoreTraceEnricher(_ => new MyAspNetCoreTraceEnricher2()));

        var processorDescriptors = services
            .Where(d => d.ImplementationType?.Name == nameof(AspNetCoreTraceEnrichmentProcessor))
            .ToArray();

        Assert.Single(processorDescriptors);

        using var sp = services.BuildServiceProvider();

        var enrichers = sp.GetServices<AspNetCoreTraceEnricher>().ToArray();
        Assert.Equal(2, enrichers.Length);
    }

    [Fact]
    public void OptionsConfigured_InstrumentationDelegatesAttached()
    {
        var services = new ServiceCollection();

        services.AddOpenTelemetry()
            .WithTracing(builder => builder
                .TryAddAspNetCoreTraceEnricher<MyAspNetCoreTraceEnricher>());

        using var sp = services.BuildServiceProvider();

        var options = sp.GetRequiredService<IOptions<AspNetCoreTraceInstrumentationOptions>>().Value;
        Assert.NotNull(options);

        Assert.NotNull(options.EnrichWithHttpRequest);
        Assert.NotNull(options.EnrichWithHttpResponse);
        Assert.NotNull(options.EnrichWithException);
    }

    [Fact]
    public void GenericOverload_NullBuilder_Throws()
    {
        TracerProviderBuilder? builder = null;
        Assert.Throws<ArgumentNullException>(() => builder!.TryAddAspNetCoreTraceEnricher<MyAspNetCoreTraceEnricher>());
    }

    [Fact]
    public void InstanceOverload_NullArgs_Throw()
    {
        var goodBuilder = Sdk.CreateTracerProviderBuilder();
        Assert.Throws<ArgumentNullException>(() => goodBuilder.TryAddAspNetCoreTraceEnricher(null!));

        TracerProviderBuilder? nullBuilder = null;
        Assert.Throws<ArgumentNullException>(() => nullBuilder!.TryAddAspNetCoreTraceEnricher(new MyAspNetCoreTraceEnricher()));
    }

    [Fact]
    public void FactoryOverload_NullArgs_Throw()
    {
        var goodBuilder = Sdk.CreateTracerProviderBuilder();
        Assert.Throws<ArgumentNullException>(() => goodBuilder.TryAddAspNetCoreTraceEnricher((Func<IServiceProvider, AspNetCoreTraceEnricher>)null!));

        TracerProviderBuilder? nullBuilder = null;
        Assert.Throws<ArgumentNullException>(() => nullBuilder!.TryAddAspNetCoreTraceEnricher(_ => new MyAspNetCoreTraceEnricher()));
    }

    [Fact]
    public void ChainingMultipleOverloads_AllRegistered()
    {
        var services = new ServiceCollection();
        var inst = new MyAspNetCoreTraceEnricher();

        services.AddOpenTelemetry()
            .WithTracing(builder => builder
                .TryAddAspNetCoreTraceEnricher<MyAspNetCoreTraceEnricher2>()
                .TryAddAspNetCoreTraceEnricher(inst)
                .TryAddAspNetCoreTraceEnricher(_ => new MyAspNetCoreTraceEnricher()));

        using var sp = services.BuildServiceProvider();

        var enrichers = sp.GetServices<AspNetCoreTraceEnricher>().ToArray();

        Assert.Equal(2, enrichers.Length);
        Assert.Contains(enrichers, e => e is MyAspNetCoreTraceEnricher2);
        Assert.Contains(enrichers, e => ReferenceEquals(e, inst));
    }
}
