// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.Configuration;
using OpenTelemetry.Exporter.OneCollector;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Logs;

/// <summary>
/// Contains extension methods to register the OneCollector log exporter.
/// </summary>
public static class OneCollectorLoggerProviderBuilderExtensions
{
    /// <summary>
    /// Add OneCollector exporter to the <see
    /// cref="LoggerProviderBuilder"/>.
    /// </summary>
    /// <param name="builder"><see cref="LoggerProviderBuilder"/>.</param>
    /// <returns>The supplied <see cref="LoggerProviderBuilder"/> for call
    /// chaining.</returns>
    public static LoggerProviderBuilder AddOneCollectorExporter(
        this LoggerProviderBuilder builder)
    {
        return AddOneCollectorExporter(builder, name: null, connectionString: null, configuration: null, configure: null);
    }

    /// <summary>
    /// Add OneCollector exporter to the <see
    /// cref="LoggerProviderBuilder"/>.
    /// </summary>
    /// <param name="builder"><see cref="LoggerProviderBuilder"/>.</param>
    /// <param name="configure">Callback action for configuring <see
    /// cref="OneCollectorLogExportProcessorBuilder"/>.</param>
    /// <returns>The supplied <see cref="LoggerProviderBuilder"/> for call
    /// chaining.</returns>
    public static LoggerProviderBuilder AddOneCollectorExporter(
        this LoggerProviderBuilder builder,
        Action<OneCollectorLogExportProcessorBuilder> configure)
    {
        Guard.ThrowIfNull(configure);

        return AddOneCollectorExporter(builder, name: null, connectionString: null, configuration: null, configure);
    }

    /// <summary>
    /// Add OneCollector exporter to the <see
    /// cref="LoggerProviderBuilder"/>.
    /// </summary>
    /// <param name="builder"><see cref="LoggerProviderBuilder"/>.</param>
    /// <param name="connectionString">OneCollector connection string.</param>
    /// <returns>The supplied <see cref="LoggerProviderBuilder"/> for call
    /// chaining.</returns>
    public static LoggerProviderBuilder AddOneCollectorExporter(
        this LoggerProviderBuilder builder,
        string connectionString)
    {
        Guard.ThrowIfNullOrWhitespace(connectionString);

        return AddOneCollectorExporter(builder, name: null, connectionString, configuration: null, configure: null);
    }

    /// <summary>
    /// Add OneCollector exporter to the <see
    /// cref="LoggerProviderBuilder"/>.
    /// </summary>
    /// <param name="builder"><see cref="LoggerProviderBuilder"/>.</param>
    /// <param name="connectionString">OneCollector connection string.</param>
    /// <param name="configure">Callback action for configuring <see
    /// cref="OneCollectorLogExportProcessorBuilder"/>.</param>
    /// <returns>The supplied <see cref="LoggerProviderBuilder"/> for call
    /// chaining.</returns>
    public static LoggerProviderBuilder AddOneCollectorExporter(
        this LoggerProviderBuilder builder,
        string connectionString,
        Action<OneCollectorLogExportProcessorBuilder> configure)
    {
        Guard.ThrowIfNullOrWhitespace(connectionString);
        Guard.ThrowIfNull(configure);

        return AddOneCollectorExporter(builder, name: null, connectionString, configuration: null, configure);
    }

    /// <summary>
    /// Add OneCollector exporter to the <see
    /// cref="LoggerProviderBuilder"/>.
    /// </summary>
    /// <remarks>Note: Batch options (<see
    /// cref="BatchExportProcessorOptions{T}"/>) are bound to the "BatchOptions"
    /// sub-section of the <see cref="IConfiguration"/> supplied in the
    /// <paramref name="configuration"/> parameter.</remarks>
    /// <param name="builder"><see cref="LoggerProviderBuilder"/>.</param>
    /// <param name="configuration">Configuration used to build <see
    /// cref="OneCollectorLogExporterOptions"/> and <see
    /// cref="BatchExportProcessorOptions{T}"/>.</param>
    /// <returns>The supplied <see cref="LoggerProviderBuilder"/> for call
    /// chaining.</returns>
    public static LoggerProviderBuilder AddOneCollectorExporter(
        this LoggerProviderBuilder builder,
        IConfiguration configuration)
    {
        Guard.ThrowIfNull(configuration);

        return AddOneCollectorExporter(builder, name: null, connectionString: null, configuration, configure: null);
    }

    /// <summary>
    /// Add OneCollector exporter to the <see
    /// cref="LoggerProviderBuilder"/>.
    /// </summary>
    /// <remarks><inheritdoc
    /// cref="AddOneCollectorExporter(LoggerProviderBuilder,
    /// IConfiguration)" path="/remarks"/></remarks>
    /// <param name="builder"><see cref="LoggerProviderBuilder"/>.</param>
    /// <param name="configuration"><inheritdoc
    /// cref="AddOneCollectorExporter(LoggerProviderBuilder,
    /// IConfiguration)" path="/param[@name='configuration']"/></param>
    /// <param name="configure">Callback action for configuring <see
    /// cref="OneCollectorLogExportProcessorBuilder"/>.</param>
    /// <returns>The supplied <see cref="LoggerProviderBuilder"/> for call
    /// chaining.</returns>
    public static LoggerProviderBuilder AddOneCollectorExporter(
        this LoggerProviderBuilder builder,
        IConfiguration configuration,
        Action<OneCollectorLogExportProcessorBuilder> configure)
    {
        Guard.ThrowIfNull(configuration);
        Guard.ThrowIfNull(configure);

        return AddOneCollectorExporter(builder, name: null, connectionString: null, configuration, configure);
    }

    /// <summary>
    /// Add OneCollector exporter to the <see cref="LoggerProviderBuilder"/>.
    /// </summary>
    /// <remarks><inheritdoc
    /// cref="AddOneCollectorExporter(LoggerProviderBuilder, IConfiguration)"
    /// path="/remarks"/></remarks>
    /// <param name="builder"><see cref="LoggerProviderBuilder"/>.</param>
    /// <param name="name">Optional name which is used when retrieving
    /// options.</param>
    /// <param name="connectionString">Optional OneCollector connection
    /// string.</param>
    /// <param name="configuration">Optional configuration used to build <see
    /// cref="OneCollectorLogExporterOptions"/> and <see
    /// cref="BatchExportProcessorOptions{T}"/>.</param>
    /// <param name="configure">Optional callback action for configuring <see
    /// cref="OneCollectorLogExportProcessorBuilder"/>.</param>
    /// <returns>The supplied <see cref="LoggerProviderBuilder"/> for call
    /// chaining.</returns>
    public static LoggerProviderBuilder AddOneCollectorExporter(
        this LoggerProviderBuilder builder,
        string? name,
        string? connectionString,
        IConfiguration? configuration,
        Action<OneCollectorLogExportProcessorBuilder>? configure)
    {
        Guard.ThrowIfNull(builder);

        return builder.ConfigureServices(services =>
        {
            var processorBuilder = new OneCollectorLogExportProcessorBuilder(name, services, configuration);

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                processorBuilder.SetConnectionString(connectionString!);
            }

            configure?.Invoke(processorBuilder);

            builder.AddProcessor(processorBuilder.BuildProcessor);
        });
    }
}
