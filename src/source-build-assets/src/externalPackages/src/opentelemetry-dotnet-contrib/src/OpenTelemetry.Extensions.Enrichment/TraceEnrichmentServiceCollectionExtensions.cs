// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET
using System.Diagnostics.CodeAnalysis;
#endif
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenTelemetry.Extensions.Enrichment;
using OpenTelemetry.Internal;
using OpenTelemetry.Trace;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to register telemetry enrichers.
/// </summary>
public static class TraceEnrichmentServiceCollectionExtensions
{
    /// <summary>
    /// Adds a <see cref="TraceEnricher"/> implementation of type <typeparamref name="T"/> as a Singleton to the specified <see cref="IServiceCollection"/> if one has not already been registered.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> being configured.</param>
    /// <typeparam name="T">Enricher object type.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="services"/> is <see langword="null" />.</exception>
    /// <returns>The instance of <see cref="IServiceCollection"/> to chain the calls.</returns>
    /// <remarks>
    /// Add this enricher *before* exporter related Activity processors.
    /// </remarks>
#if NET
    public static IServiceCollection TryAddTraceEnricher<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this IServiceCollection services)
#else
    public static IServiceCollection TryAddTraceEnricher<T>(this IServiceCollection services)
#endif
        where T : TraceEnricher
    {
        Guard.ThrowIfNull(services);

        services.TryAddEnumerable(ServiceDescriptor.Singleton<TraceEnricher, T>());

        return services.TryAddEnrichment();
    }

    /// <summary>
    /// Adds a <see cref="TraceEnricher"/> implementation of type <paramref name="enricher"/> as a Singleton to the specified <see cref="IServiceCollection"/> if one has not already been registered.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> being configured.</param>
    /// <param name="enricher">The <see cref="TraceEnricher"/> object being added.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="services"/> or <paramref name="enricher"/> is <see langword="null" />.</exception>
    /// <returns>The instance of <see cref="IServiceCollection"/> to chain the calls.</returns>
    /// <remarks>
    /// Add this enricher *before* exporter related Activity processors.
    /// </remarks>
    public static IServiceCollection TryAddTraceEnricher(this IServiceCollection services, TraceEnricher enricher)
    {
        Guard.ThrowIfNull(services);
        Guard.ThrowIfNull(enricher);

        services.TryAddEnumerable(ServiceDescriptor.Singleton(enricher));

        return services.TryAddEnrichment();
    }

    /// <summary>
    /// Adds a trace enricher delegate to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> being configured.</param>
    /// <param name="enrichmentAction">The <see cref="TraceEnrichmentBag"/> delegate to enrich traces.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="services"/> or <paramref name="enrichmentAction"/> is <see langword="null" />.</exception>
    /// <returns>The instance of <see cref="IServiceCollection"/> to chain the calls.</returns>
    /// <remarks>
    /// Add this enricher *before* exporter related Activity processors.
    /// </remarks>
    public static IServiceCollection AddTraceEnricher(this IServiceCollection services, Action<TraceEnrichmentBag> enrichmentAction)
    {
        Guard.ThrowIfNull(services);
        Guard.ThrowIfNull(enrichmentAction);

        services.TryAddSingleton<TraceEnricher, TraceEnrichmentActions>();

        return services
            .TryAddEnrichment()
            .AddSingleton(enrichmentAction);
    }

    /// <summary>
    /// Adds a trace enricher factory to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> being configured.</param>
    /// <param name="enricherImplementationFactory">The <see cref="TraceEnricher"/> object being added using implementation factory.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="services"/> or <paramref name="enricherImplementationFactory"/> is <see langword="null" />.</exception>
    /// <returns>The instance of <see cref="IServiceCollection"/> to chain the calls.</returns>
    /// <remarks>
    /// Add this enricher *before* exporter related Activity processors.
    /// </remarks>
    public static IServiceCollection AddTraceEnricher(this IServiceCollection services, Func<IServiceProvider, TraceEnricher> enricherImplementationFactory)
    {
        Guard.ThrowIfNull(services);
        Guard.ThrowIfNull(enricherImplementationFactory);

        return services
            .TryAddEnrichment()
            .AddSingleton(enricherImplementationFactory);
    }

    private static IServiceCollection TryAddEnrichment(this IServiceCollection services)
    {
        if (!services.Any(x => x.ServiceType == typeof(TraceEnrichmentProcessor)))
        {
            services
                .AddSingleton<TraceEnrichmentProcessor>()
                .ConfigureOpenTelemetryTracerProvider((sp, builder) =>
                {
                    var proc = sp.GetRequiredService<TraceEnrichmentProcessor>();
                    builder.AddProcessor(proc);
                });
        }

        return services;
    }
}
