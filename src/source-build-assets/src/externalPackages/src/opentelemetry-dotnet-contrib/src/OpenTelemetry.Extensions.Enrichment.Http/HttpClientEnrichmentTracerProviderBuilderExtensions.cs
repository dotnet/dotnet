// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET
using System.Diagnostics.CodeAnalysis;
#endif
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Extensions.Enrichment.Http;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Trace;

/// <summary>
/// Extension methods to register telemetry enrichers.
/// </summary>
public static class HttpClientEnrichmentTracerProviderBuilderExtensions
{
    /// <summary>
    /// Adds the specified <typeparamref name="T"/> implementation as a Singleton <see cref="HttpClientTraceEnricher"/> service
    /// to the <paramref name="builder"/> if the same service and implementation does not already exist.
    /// </summary>
    /// <typeparam name="T">Concrete <see cref="HttpClientTraceEnricher"/> implementation type.</typeparam>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> is <see langword="null"/>.</exception>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
#if NET
    public static TracerProviderBuilder TryAddHttpClientTraceEnricher<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this TracerProviderBuilder builder)
#else
    public static TracerProviderBuilder TryAddHttpClientTraceEnricher<T>(this TracerProviderBuilder builder)
#endif
        where T : HttpClientTraceEnricher
    {
        Guard.ThrowIfNull(builder);

        return builder.ConfigureServices(services => services.TryAddHttpClientTraceEnricher<T>());
    }

    /// <summary>
    /// Adds the specified <paramref name="enricher"/> implementation as a Singleton <see cref="HttpClientTraceEnricher"/> service
    /// to the <paramref name="builder"/> if the same service and implementation does not already exist.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="enricher">The <see cref="HttpClientTraceEnricher"/> instance being added.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> is <see langword="null"/>.</exception>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder TryAddHttpClientTraceEnricher(this TracerProviderBuilder builder, HttpClientTraceEnricher enricher)
    {
        Guard.ThrowIfNull(builder);
        Guard.ThrowIfNull(enricher);

        return builder.ConfigureServices(services => services.TryAddHttpClientTraceEnricher(enricher));
    }

    /// <summary>
    /// Adds the specified <typeparamref name="T"/> implementation produced by the supplied factory as a Singleton <see cref="HttpClientTraceEnricher"/> service
    /// to the <paramref name="builder"/> if the same service and implementation does not already exist.
    /// </summary>
    /// <typeparam name="T">Concrete <see cref="HttpClientTraceEnricher"/> implementation type.</typeparam>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="enricherImplementationFactory">Factory used to create the <typeparamref name="T"/> instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> or <paramref name="enricherImplementationFactory"/> is <see langword="null"/>.</exception>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder TryAddHttpClientTraceEnricher<T>(this TracerProviderBuilder builder, Func<IServiceProvider, T> enricherImplementationFactory)
        where T : HttpClientTraceEnricher
    {
        Guard.ThrowIfNull(builder);
        Guard.ThrowIfNull(enricherImplementationFactory);

        return builder.ConfigureServices(services => services.TryAddHttpClientTraceEnricher(enricherImplementationFactory));
    }
}
