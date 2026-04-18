// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET
using System.Diagnostics.CodeAnalysis;
#endif
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Internal;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Extensions.Enrichment.AspNetCore;

/// <summary>
/// Extension methods to register telemetry enrichers.
/// </summary>
public static class AspNetCoreTraceEnrichmentProviderBuilderExtensions
{
    /// <summary>
    /// Adds the specified <typeparamref name="T"/> implementation as a Singleton <see cref="AspNetCoreTraceEnricher"/> service
    /// to the <paramref name="builder"/> if the same service and implementation does not already exist.
    /// </summary>
    /// <typeparam name="T">Concrete <see cref="AspNetCoreTraceEnricher"/> implementation type.</typeparam>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// Call this <b>before</b> exporter related Activity processors are added.
    /// </remarks>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
#if NET
    public static TracerProviderBuilder TryAddAspNetCoreTraceEnricher<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this TracerProviderBuilder builder)
#else
    public static TracerProviderBuilder TryAddAspNetCoreTraceEnricher<T>(this TracerProviderBuilder builder)
#endif
        where T : AspNetCoreTraceEnricher
    {
        Guard.ThrowIfNull(builder);

        return builder.ConfigureServices(services => services.TryAddAspNetCoreTraceEnricher<T>());
    }

    /// <summary>
    /// Adds the specified <paramref name="enricher"/> implementation as a Singleton <see cref="AspNetCoreTraceEnricher"/> service
    /// to the <paramref name="builder"/> if the same service and implementation does not already exist.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="enricher">The <see cref="AspNetCoreTraceEnricher"/> instance being added.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// Call this <b>before</b> exporter related Activity processors are added.
    /// </remarks>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder TryAddAspNetCoreTraceEnricher(this TracerProviderBuilder builder, AspNetCoreTraceEnricher enricher)
    {
        Guard.ThrowIfNull(builder);
        Guard.ThrowIfNull(enricher);

        return builder.ConfigureServices(services => services.TryAddAspNetCoreTraceEnricher(enricher));
    }

    /// <summary>
    /// Adds the specified <typeparamref name="T"/> implementation produced by the supplied factory as a Singleton <see cref="AspNetCoreTraceEnricher"/> service
    /// to the <paramref name="builder"/> if the same service and implementation does not already exist.
    /// </summary>
    /// <typeparam name="T">Concrete <see cref="AspNetCoreTraceEnricher"/> implementation type.</typeparam>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="enricherImplementationFactory">Factory used to create the <typeparamref name="T"/> instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> or <paramref name="enricherImplementationFactory"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// Call this <b>before</b> exporter related Activity processors are added.
    /// </remarks>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder TryAddAspNetCoreTraceEnricher<T>(this TracerProviderBuilder builder, Func<IServiceProvider, T> enricherImplementationFactory)
        where T : AspNetCoreTraceEnricher
    {
        Guard.ThrowIfNull(builder);
        Guard.ThrowIfNull(enricherImplementationFactory);

        return builder.ConfigureServices(services => services.TryAddAspNetCoreTraceEnricher(enricherImplementationFactory));
    }
}
