// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Instrumentation.AspNetCore;
using Xunit;

namespace OpenTelemetry.Extensions.Enrichment.AspNetCore.Tests;

public sealed class AspNetCoreTraceEnrichmentServiceCollectionExtensionsTests
{
    [Fact]
    public void TryAdd_Generic_AddsSingleEnricher_AndIsIdempotent()
    {
        var services = new ServiceCollection();

        services.TryAddAspNetCoreTraceEnricher<MyAspNetCoreTraceEnricher>();
        services.TryAddAspNetCoreTraceEnricher<MyAspNetCoreTraceEnricher>();

        using var sp = services.BuildServiceProvider();
        var enrichers = sp.GetServices<AspNetCoreTraceEnricher>().OfType<MyAspNetCoreTraceEnricher>().ToArray();
        Assert.Single(enrichers);
    }

    [Fact]
    public void TryAdd_Instance_AddsSingleInstance_AndIsIdempotent()
    {
        var services = new ServiceCollection();
        var instance = new MyAspNetCoreTraceEnricher();
        services.TryAddAspNetCoreTraceEnricher(instance);
        services.TryAddAspNetCoreTraceEnricher(instance);

        using var sp = services.BuildServiceProvider();
        var enrichers = sp.GetServices<AspNetCoreTraceEnricher>().ToArray();
        Assert.Single(enrichers);
        Assert.Same(instance, enrichers[0]);
    }

    [Fact]
    public void Add_Factory_AddsEachFactoryResult()
    {
        var services = new ServiceCollection();
        services.TryAddAspNetCoreTraceEnricher(_ => new MyAspNetCoreTraceEnricher());
        services.TryAddAspNetCoreTraceEnricher(_ => new MyAspNetCoreTraceEnricher2());

        using var sp = services.BuildServiceProvider();
        var enricherTypes = sp.GetServices<AspNetCoreTraceEnricher>().Select(e => e.GetType()).ToArray();
        Assert.Contains(typeof(MyAspNetCoreTraceEnricher), enricherTypes);
        Assert.Contains(typeof(MyAspNetCoreTraceEnricher2), enricherTypes);
    }

    [Fact]
    public void Processor_And_ConfigureOptions_Singleton_RegisteredOnce()
    {
        var services = new ServiceCollection();
        services.TryAddAspNetCoreTraceEnricher<MyAspNetCoreTraceEnricher>();
        services.TryAddAspNetCoreTraceEnricher(_ => new MyAspNetCoreTraceEnricher2());

        var processorDescriptors = services.Where(d => d.ImplementationType?.Name == nameof(AspNetCoreTraceEnrichmentProcessor)).ToArray();
        Assert.Single(processorDescriptors);

        var configureOptionsDescriptors = services.Where(d => d.ServiceType == typeof(IConfigureOptions<AspNetCoreTraceInstrumentationOptions>)).ToArray();
        Assert.Single(configureOptionsDescriptors);
    }

    [Fact]
    public void Options_Have_Delegates_Configured()
    {
        var services = new ServiceCollection();
        services.TryAddAspNetCoreTraceEnricher(_ => new MyAspNetCoreTraceEnricher2());

        using var sp = services.BuildServiceProvider();
        var opts = sp.GetRequiredService<IOptions<AspNetCoreTraceInstrumentationOptions>>().Value;
        Assert.NotNull(opts.EnrichWithHttpRequest);
        Assert.NotNull(opts.EnrichWithHttpResponse);
        Assert.NotNull(opts.EnrichWithException);
    }

    [Fact]
    public void NullArguments_Throw()
    {
        ServiceCollection? servicesNull = null;
        Assert.Throws<ArgumentNullException>(() => servicesNull!.TryAddAspNetCoreTraceEnricher<MyAspNetCoreTraceEnricher>());
        Assert.Throws<ArgumentNullException>(() => servicesNull!.TryAddAspNetCoreTraceEnricher(new MyAspNetCoreTraceEnricher()));
        Assert.Throws<ArgumentNullException>(() => servicesNull!.TryAddAspNetCoreTraceEnricher((Func<IServiceProvider, AspNetCoreTraceEnricher>)null!));

        var services = new ServiceCollection();
        Assert.Throws<ArgumentNullException>(() => services.TryAddAspNetCoreTraceEnricher(null!));
        Assert.Throws<ArgumentNullException>(() => services.TryAddAspNetCoreTraceEnricher((Func<IServiceProvider, AspNetCoreTraceEnricher>)null!));
    }
}
