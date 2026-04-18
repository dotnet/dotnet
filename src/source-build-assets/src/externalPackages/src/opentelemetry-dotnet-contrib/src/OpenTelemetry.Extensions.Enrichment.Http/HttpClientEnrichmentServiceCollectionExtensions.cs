// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET
using System.Diagnostics.CodeAnalysis;
#endif
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OpenTelemetry.Extensions.Enrichment.Http;
using OpenTelemetry.Instrumentation.Http;
using OpenTelemetry.Internal;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to register HTTP Client specific telemetry trace enrichers.
/// </summary>
public static class HttpClientEnrichmentServiceCollectionExtensions
{
    /// <summary>
    /// Adds the specified <typeparamref name="T"/> implementation as a Singleton <see cref="HttpClientTraceEnricher"/> service
    /// to the <paramref name="services"/> if the same service and implementation does not already exist in <paramref name="services"/>.
    /// </summary>
    /// <typeparam name="T">Concrete <see cref="HttpClientTraceEnricher"/> implementation type.</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/> being configured.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is <see langword="null"/>.</exception>
    /// <returns>The instance of <see cref="IServiceCollection"/> to chain the calls.</returns>
#if NET
    public static IServiceCollection TryAddHttpClientTraceEnricher<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this IServiceCollection services)
#else
    public static IServiceCollection TryAddHttpClientTraceEnricher<T>(this IServiceCollection services)
#endif
        where T : HttpClientTraceEnricher
    {
        Guard.ThrowIfNull(services);

        services.TryAddEnumerable(ServiceDescriptor.Singleton<HttpClientTraceEnricher, T>());

        return services.TryAddHttpClientEnrichment();
    }

    /// <summary>
    /// Adds the specified <paramref name="enricher"/> implementation as a Singleton <see cref="HttpClientTraceEnricher"/> service
    /// to the <paramref name="services"/> if the same service and implementation does not already exist in <paramref name="services"/>.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> being configured.</param>
    /// <param name="enricher">The <see cref="HttpClientTraceEnricher"/> instance being added.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is <see langword="null"/>.</exception>
    /// <returns>The instance of <see cref="IServiceCollection"/> to chain the calls.</returns>
    public static IServiceCollection TryAddHttpClientTraceEnricher(this IServiceCollection services, HttpClientTraceEnricher enricher)
    {
        Guard.ThrowIfNull(services);
        Guard.ThrowIfNull(enricher);

        services.TryAddEnumerable(ServiceDescriptor.Singleton(enricher));

        return services.TryAddHttpClientEnrichment();
    }

    /// <summary>
    /// Adds the specified <typeparamref name="T"/> implementation produced by the supplied factory as a Singleton <see cref="HttpClientTraceEnricher"/> service
    /// to the <paramref name="services"/> if the same service and implementation does not already exist in <paramref name="services"/>.
    /// </summary>
    /// <typeparam name="T">Concrete <see cref="HttpClientTraceEnricher"/> implementation type.</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/> being configured.</param>
    /// <param name="enricherImplementationFactory">Factory used to create the <typeparamref name="T"/> instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="enricherImplementationFactory"/> is <see langword="null"/>.</exception>
    /// <returns>The instance of <see cref="IServiceCollection"/> to chain the calls.</returns>
    public static IServiceCollection TryAddHttpClientTraceEnricher<T>(this IServiceCollection services, Func<IServiceProvider, T> enricherImplementationFactory)
        where T : HttpClientTraceEnricher
    {
        Guard.ThrowIfNull(services);
        Guard.ThrowIfNull(enricherImplementationFactory);

        services.TryAddEnumerable(ServiceDescriptor.Singleton<HttpClientTraceEnricher, T>((serviceProvider) => enricherImplementationFactory(serviceProvider)));

        return services.TryAddHttpClientEnrichment();
    }

    private static IServiceCollection TryAddHttpClientEnrichment(this IServiceCollection services)
    {
        services
            .AddOptions()
            .TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<HttpClientTraceInstrumentationOptions>, ConfigureHttpClientTraceInstrumentationOptions>());

        services.TryAddSingleton<HttpClientTraceEnrichmentProcessor>();

        return services;
    }
}
