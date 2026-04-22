// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.Configuration;
using OpenTelemetry.Exporter.OneCollector;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Logs;

/// <summary>
/// Contains extension methods to register the OneCollector log exporter.
/// </summary>
public static class OneCollectorOpenTelemetryLoggerOptionsExtensions
{
    /// <summary>
    /// Add OneCollector exporter to the <see
    /// cref="OpenTelemetryLoggerOptions"/>.
    /// </summary>
    /// <param name="options"><see cref="OpenTelemetryLoggerOptions"/>.</param>
    /// <param name="configure">Callback action for configuring <see
    /// cref="OneCollectorLogExportProcessorBuilder"/>.</param>
    /// <returns>The supplied <see cref="OpenTelemetryLoggerOptions"/> for call
    /// chaining.</returns>
    public static OpenTelemetryLoggerOptions AddOneCollectorExporter(
        this OpenTelemetryLoggerOptions options,
        Action<OneCollectorLogExportProcessorBuilder> configure)
    {
        Guard.ThrowIfNull(configure);

        return AddOneCollectorExporter(options, connectionString: null, configuration: null, configure);
    }

    /// <summary>
    /// Add OneCollector exporter to the <see
    /// cref="OpenTelemetryLoggerOptions"/>.
    /// </summary>
    /// <param name="options"><see cref="OpenTelemetryLoggerOptions"/>.</param>
    /// <param name="connectionString">OneCollector connection string.</param>
    /// <returns>The supplied <see cref="OpenTelemetryLoggerOptions"/> for call
    /// chaining.</returns>
    public static OpenTelemetryLoggerOptions AddOneCollectorExporter(
        this OpenTelemetryLoggerOptions options,
        string connectionString)
    {
        Guard.ThrowIfNullOrWhitespace(connectionString);

        return AddOneCollectorExporter(options, connectionString, configuration: null, configure: null);
    }

    /// <summary>
    /// Add OneCollector exporter to the <see
    /// cref="OpenTelemetryLoggerOptions"/>.
    /// </summary>
    /// <param name="options"><see cref="OpenTelemetryLoggerOptions"/>.</param>
    /// <param name="connectionString">OneCollector connection string.</param>
    /// <param name="configure">Callback action for configuring <see
    /// cref="OneCollectorLogExportProcessorBuilder"/>.</param>
    /// <returns>The supplied <see cref="OpenTelemetryLoggerOptions"/> for call
    /// chaining.</returns>
    public static OpenTelemetryLoggerOptions AddOneCollectorExporter(
        this OpenTelemetryLoggerOptions options,
        string connectionString,
        Action<OneCollectorLogExportProcessorBuilder> configure)
    {
        Guard.ThrowIfNullOrWhitespace(connectionString);

        return AddOneCollectorExporter(options, connectionString, configuration: null, configure);
    }

    /// <summary>
    /// Add OneCollector exporter to the <see
    /// cref="OpenTelemetryLoggerOptions"/>.
    /// </summary>
    /// <remarks>Note: Batch options (<see
    /// cref="BatchExportProcessorOptions{T}"/>) are bound to the "BatchOptions"
    /// sub-section of the <see cref="IConfiguration"/> supplied in the
    /// <paramref name="configuration"/> parameter.</remarks>
    /// <param name="options"><see cref="OpenTelemetryLoggerOptions"/>.</param>
    /// <param name="configuration">Configuration used to build <see
    /// cref="OneCollectorLogExporterOptions"/> and <see
    /// cref="BatchExportProcessorOptions{T}"/>.</param>
    /// <returns>The supplied <see cref="OpenTelemetryLoggerOptions"/> for call
    /// chaining.</returns>
    public static OpenTelemetryLoggerOptions AddOneCollectorExporter(
        this OpenTelemetryLoggerOptions options,
        IConfiguration configuration)
    {
        Guard.ThrowIfNull(configuration);

        return AddOneCollectorExporter(options, connectionString: null, configuration, configure: null);
    }

    /// <summary>
    /// Add OneCollector exporter to the <see
    /// cref="OpenTelemetryLoggerOptions"/>.
    /// </summary>
    /// <remarks><inheritdoc
    /// cref="AddOneCollectorExporter(OpenTelemetryLoggerOptions,
    /// IConfiguration)" path="/remarks"/></remarks>
    /// <param name="options"><see cref="OpenTelemetryLoggerOptions"/>.</param>
    /// <param name="configuration"><inheritdoc
    /// cref="AddOneCollectorExporter(OpenTelemetryLoggerOptions,
    /// IConfiguration)" path="/param[@name='configuration']"/></param>
    /// <param name="configure">Callback action for configuring <see
    /// cref="OneCollectorLogExportProcessorBuilder"/>.</param>
    /// <returns>The supplied <see cref="OpenTelemetryLoggerOptions"/> for call
    /// chaining.</returns>
    public static OpenTelemetryLoggerOptions AddOneCollectorExporter(
        this OpenTelemetryLoggerOptions options,
        IConfiguration configuration,
        Action<OneCollectorLogExportProcessorBuilder> configure)
    {
        Guard.ThrowIfNull(configuration);

        return AddOneCollectorExporter(options, connectionString: null, configuration, configure);
    }

    private static OpenTelemetryLoggerOptions AddOneCollectorExporter(
        this OpenTelemetryLoggerOptions options,
        string? connectionString,
        IConfiguration? configuration,
        Action<OneCollectorLogExportProcessorBuilder>? configure)
    {
        Guard.ThrowIfNull(options);

        var builder = new OneCollectorLogExportProcessorBuilder(name: null, services: null, configuration);

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            builder.SetConnectionString(connectionString!);
        }

        configure?.Invoke(builder);

        options.AddProcessor(builder.BuildProcessor);

        return options;
    }
}
