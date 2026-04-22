// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Instrumentation.ElasticsearchClient;
using OpenTelemetry.Instrumentation.ElasticsearchClient.Implementation;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Trace;

/// <summary>
/// Extension methods to simplify registering of dependency instrumentation.
/// </summary>
public static class TracerProviderBuilderExtensions
{
    /// <summary>
    /// Enables Elasticsearch client Instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddElasticsearchClientInstrumentation(
        this TracerProviderBuilder builder) =>
        AddElasticsearchClientInstrumentation(builder, name: null, configure: null);

    /// <summary>
    /// Enables Elasticsearch client Instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="configure">Elasticsearch client configuration options.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddElasticsearchClientInstrumentation(
        this TracerProviderBuilder builder,
        Action<ElasticsearchClientInstrumentationOptions>? configure) =>
        AddElasticsearchClientInstrumentation(builder, name: null, configure);

    /// <summary>
    /// Enables Elasticsearch client Instrumentation.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="name">Name which is used when retrieving options.</param>
    /// <param name="configure">Elasticsearch client configuration options.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddElasticsearchClientInstrumentation(
        this TracerProviderBuilder builder,
        string? name,
        Action<ElasticsearchClientInstrumentationOptions>? configure)
    {
        Guard.ThrowIfNull(builder);

        name ??= Options.DefaultName;

        if (configure != null)
        {
            builder.ConfigureServices(services => services.Configure(name, configure));
        }

        builder.AddInstrumentation(sp =>
        {
            var options = sp.GetRequiredService<IOptionsMonitor<ElasticsearchClientInstrumentationOptions>>().Get(name);
            return new ElasticsearchClientInstrumentation(options);
        });

        builder.AddSource(ElasticsearchRequestPipelineDiagnosticListener.ActivitySourceName);
        builder.AddLegacySource("CallElasticsearch");

        return builder;
    }
}
