// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET
using System.Diagnostics.CodeAnalysis;
#endif
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OpenTelemetry.Extensions.Enrichment.AspNetCore;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Internal;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to register ASP.NET Core specific telemetry trace enrichers.
/// </summary>
public static class AspNetCoreTraceEnrichmentServiceCollectionExtensions
{
    /// <summary>
    /// Adds the specified <typeparamref name="T"/> implementation as a Singleton <see cref="AspNetCoreTraceEnricher"/> service
    /// to the <paramref name="services"/> if the same service and implementation does not already exist in <paramref name="services"/>.
    /// </summary>
    /// <typeparam name="T">Concrete <see cref="AspNetCoreTraceEnricher"/> implementation type.</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/> being configured.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// Call this <b>before</b> exporter related Activity processors are added.
    /// </remarks>
    /// <returns>The instance of <see cref="IServiceCollection"/> to chain the calls.</returns>
#if NET
    public static IServiceCollection TryAddAspNetCoreTraceEnricher<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this IServiceCollection services)
#else
    public static IServiceCollection TryAddAspNetCoreTraceEnricher<T>(this IServiceCollection services)
#endif
        where T : AspNetCoreTraceEnricher
    {
        Guard.ThrowIfNull(services);

        services.TryAddEnumerable(ServiceDescriptor.Singleton<AspNetCoreTraceEnricher, T>());

        return services.TryAddAspNetCoreEnrichment();
    }

    /// <summary>
    /// Adds the specified <paramref name="enricher"/> implementation as a Singleton <see cref="AspNetCoreTraceEnricher"/> service
    /// to the <paramref name="services"/> if the same service and implementation does not already exist in <paramref name="services"/>.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> being configured.</param>
    /// <param name="enricher">The <see cref="AspNetCoreTraceEnricher"/> instance being added.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// Call this <b>before</b> exporter related Activity processors are added.
    /// </remarks>
    /// <returns>The instance of <see cref="IServiceCollection"/> to chain the calls.</returns>
    public static IServiceCollection TryAddAspNetCoreTraceEnricher(this IServiceCollection services, AspNetCoreTraceEnricher enricher)
    {
        Guard.ThrowIfNull(services);
        Guard.ThrowIfNull(enricher);

        services.TryAddEnumerable(ServiceDescriptor.Singleton(enricher));

        return services.TryAddAspNetCoreEnrichment();
    }

    /// <summary>
    /// Adds the specified <typeparamref name="T"/> implementation produced by the supplied factory as a Singleton <see cref="AspNetCoreTraceEnricher"/> service
    /// to the <paramref name="services"/> if the same service and implementation does not already exist in <paramref name="services"/>.
    /// </summary>
    /// <typeparam name="T">Concrete <see cref="AspNetCoreTraceEnricher"/> implementation type.</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/> being configured.</param>
    /// <param name="enricherImplementationFactory">Factory used to create the <typeparamref name="T"/> instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="enricherImplementationFactory"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// Call this <b>before</b> exporter related Activity processors are added.
    /// </remarks>
    /// <returns>The instance of <see cref="IServiceCollection"/> to chain the calls.</returns>
    public static IServiceCollection TryAddAspNetCoreTraceEnricher<T>(this IServiceCollection services, Func<IServiceProvider, T> enricherImplementationFactory)
        where T : AspNetCoreTraceEnricher
    {
        Guard.ThrowIfNull(services);
        Guard.ThrowIfNull(enricherImplementationFactory);

        services.TryAddEnumerable(ServiceDescriptor.Singleton<AspNetCoreTraceEnricher, T>((serviceProvider) => enricherImplementationFactory(serviceProvider)));

        return services.TryAddAspNetCoreEnrichment();
    }

    private static IServiceCollection TryAddAspNetCoreEnrichment(this IServiceCollection services)
    {
        services
            .AddOptions()
            .TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<AspNetCoreTraceInstrumentationOptions>, ConfigureAspNetCoreTraceInstrumentationOptions>());

        services.TryAddSingleton<AspNetCoreTraceEnrichmentProcessor>();

        return services;
    }
}
