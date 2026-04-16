// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter.OneCollector;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Logs;

/// <summary>
/// Contains methods for building <see cref="BaseProcessor{T}"/> and <see
/// cref="OneCollectorExporter{T}"/> instances for exporting <see
/// cref="LogRecord"/> telemetry data.
/// </summary>
public sealed class OneCollectorLogExportProcessorBuilder
{
    private readonly string? name;
    private readonly IServiceCollection services;
    private readonly bool ownsServices;

    internal OneCollectorLogExportProcessorBuilder(
        string? name,
        IServiceCollection? services,
        IConfiguration? configuration)
    {
        this.name = name;

        if (services == null)
        {
            this.services = new ServiceCollection();
            this.services.AddOptions();
            this.ownsServices = true;
        }
        else
        {
            this.services = services;
        }

        if (configuration != null)
        {
            this.services.Configure<OneCollectorLogExporterOptions>(this.name, configuration);
            this.services.Configure<BatchExportLogRecordProcessorOptions>(
                this.name,
                batchOptions => configuration.GetSection("BatchOptions").Bind(batchOptions));
        }
    }

    /// <summary>
    /// Register a callback action for configuring the batch options of the
    /// processor used to invoke the <see cref="OneCollectorExporter{T}"/>.
    /// </summary>
    /// <param name="configure">Callback action for configuring <see
    /// cref="BatchExportProcessorOptions{T}"/>.</param>
    /// <returns>The supplied <see
    /// cref="OneCollectorLogExportProcessorBuilder"/> for call
    /// chaining.</returns>
    public OneCollectorLogExportProcessorBuilder ConfigureBatchOptions(
        Action<BatchExportProcessorOptions<LogRecord>> configure)
    {
        Guard.ThrowIfNull(configure);

        this.services.Configure<BatchExportLogRecordProcessorOptions>(
            this.name,
            batchOptions => configure(batchOptions));

        return this;
    }

    /// <summary>
    /// Register a callback action for configuring the <see
    /// cref="OneCollectorExporter{T}"/> created by the builder.
    /// </summary>
    /// <param name="configure">Callback action for configuring <see
    /// cref="OneCollectorExporter{T}"/>.</param>
    /// <returns>The supplied <see
    /// cref="OneCollectorLogExportProcessorBuilder"/> for call
    /// chaining.</returns>
    public OneCollectorLogExportProcessorBuilder ConfigureExporter(
        Action<OneCollectorExporter<LogRecord>> configure)
    {
        Guard.ThrowIfNull(configure);

        this.services.AddSingleton(
            new ConfigureOneCollectorExporter(
                this.name,
                (sp, e) => configure(e)));

        return this;
    }

    /// <summary>
    /// Register a callback action for configuring the serialization options
    /// used by the <see cref="OneCollectorExporter{T}"/> created by the
    /// builder.
    /// </summary>
    /// <param name="configure">Callback action for configuring <see
    /// cref="OneCollectorLogExporterSerializationOptions"/>.</param>
    /// <returns>The supplied <see
    /// cref="OneCollectorLogExportProcessorBuilder"/> for call
    /// chaining.</returns>
    public OneCollectorLogExportProcessorBuilder ConfigureSerializationOptions(
        Action<OneCollectorLogExporterSerializationOptions> configure)
    {
        Guard.ThrowIfNull(configure);

        this.services.Configure<OneCollectorLogExporterOptions>(
            this.name,
            exporterOptions => configure(exporterOptions.SerializationOptions));

        return this;
    }

    /// <summary>
    /// Register a callback action for configuring the transport options used by
    /// the <see cref="OneCollectorExporter{T}"/> created by the builder.
    /// </summary>
    /// <param name="configure">Callback action for configuring <see
    /// cref="OneCollectorExporterTransportOptions"/>.</param>
    /// <returns>The supplied <see
    /// cref="OneCollectorLogExportProcessorBuilder"/> for call
    /// chaining.</returns>
    public OneCollectorLogExportProcessorBuilder ConfigureTransportOptions(
        Action<OneCollectorExporterTransportOptions> configure)
    {
        Guard.ThrowIfNull(configure);

        this.services.Configure<OneCollectorLogExporterOptions>(
            this.name,
            exporterOptions => configure(exporterOptions.TransportOptions));

        return this;
    }

    /// <summary>
    /// Sets the connection string used by the <see
    /// cref="OneCollectorExporter{T}"/> created by the builder.
    /// </summary>
    /// <remarks><inheritdoc
    /// cref="OneCollectorExporterOptions.ConnectionString"
    /// path="/remarks"/></remarks>
    /// <param name="connectionString">Connection string.</param>
    /// <returns>The supplied <see
    /// cref="OneCollectorLogExportProcessorBuilder"/> for call
    /// chaining.</returns>
    public OneCollectorLogExportProcessorBuilder SetConnectionString(
        string connectionString)
    {
        Guard.ThrowIfNullOrWhitespace(connectionString);

        this.services.Configure<OneCollectorLogExporterOptions>(
            this.name,
            exporterOptions => exporterOptions.ConnectionString = connectionString);

        return this;
    }

    /// <summary>
    /// Sets the event full name mappings used by the <see
    /// cref="OneCollectorExporter{T}"/> created by the builder.
    /// </summary>
    /// <remarks><inheritdoc
    /// cref="OneCollectorLogExporterOptions.EventFullNameMappings"
    /// path="/remarks"/></remarks>
    /// <param name="eventFullNameMappings">Event full name mappings.</param>
    /// <returns>The supplied <see
    /// cref="OneCollectorLogExportProcessorBuilder"/> for call
    /// chaining.</returns>
    public OneCollectorLogExportProcessorBuilder SetEventFullNameMappings(
        IReadOnlyDictionary<string, string>? eventFullNameMappings)
    {
        this.services.Configure<OneCollectorLogExporterOptions>(
            this.name,
            exporterOptions => exporterOptions.EventFullNameMappings = eventFullNameMappings);

        return this;
    }

    /// <summary>
    /// Sets the default event namespace used by the <see
    /// cref="OneCollectorExporter{T}"/> created by the builder. Default value:
    /// <c>OpenTelemetry.Logs</c>.
    /// </summary>
    /// <remarks><inheritdoc
    /// cref="OneCollectorLogExporterOptions.DefaultEventNamespace"
    /// path="/remarks"/></remarks>
    /// <param name="defaultEventNamespace">Default event namespace.</param>
    /// <returns>The supplied <see
    /// cref="OneCollectorLogExportProcessorBuilder"/> for call
    /// chaining.</returns>
    public OneCollectorLogExportProcessorBuilder SetDefaultEventNamespace(
        string defaultEventNamespace)
    {
        Guard.ThrowIfNullOrWhitespace(defaultEventNamespace);

        this.services.Configure<OneCollectorLogExporterOptions>(
            this.name,
            exporterOptions => exporterOptions.DefaultEventNamespace = defaultEventNamespace);

        return this;
    }

    /// <summary>
    /// Sets the default event name used by the <see
    /// cref="OneCollectorExporter{T}"/> created by the builder. Default value:
    /// <c>Log</c>.
    /// </summary>
    /// <remarks><inheritdoc
    /// cref="OneCollectorLogExporterOptions.DefaultEventName"
    /// path="/remarks"/></remarks>
    /// <param name="defaultEventName">Default event name.</param>
    /// <returns>The supplied <see
    /// cref="OneCollectorLogExportProcessorBuilder"/> for call
    /// chaining.</returns>
    public OneCollectorLogExportProcessorBuilder SetDefaultEventName(
        string defaultEventName)
    {
        Guard.ThrowIfNullOrWhitespace(defaultEventName);

        this.services.Configure<OneCollectorLogExporterOptions>(
            this.name,
            exporterOptions => exporterOptions.DefaultEventName = defaultEventName);

        return this;
    }

    internal BaseProcessor<LogRecord> BuildProcessor(
        IServiceProvider serviceProvider)
    {
        Debug.Assert(serviceProvider != null, "serviceProvider was null");

        ServiceProvider? ownedServiceProvider = null;
        if (this.ownsServices)
        {
            ownedServiceProvider = this.services.BuildServiceProvider();
        }

        var exporterOptions = (ownedServiceProvider ?? serviceProvider!).GetRequiredService<IOptionsMonitor<OneCollectorLogExporterOptions>>().Get(this.name);
        var batchOptions = (ownedServiceProvider ?? serviceProvider!).GetRequiredService<IOptionsMonitor<BatchExportLogRecordProcessorOptions>>().Get(this.name);

        try
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            return new BatchLogRecordExportProcessor(
                CreateExporter(this.name, serviceProvider!, exporterOptions, (ownedServiceProvider ?? serviceProvider!).GetServices<ConfigureOneCollectorExporter>()),
                batchOptions.MaxQueueSize,
                batchOptions.ScheduledDelayMilliseconds,
                batchOptions.ExporterTimeoutMilliseconds,
                batchOptions.MaxExportBatchSize);
#pragma warning restore CA2000 // Dispose objects before losing scope
        }
        finally
        {
            ownedServiceProvider?.Dispose();
        }
    }

    private static OneCollectorExporter<LogRecord> CreateExporter(
        string? name,
        IServiceProvider serviceProvider,
        OneCollectorLogExporterOptions exporterOptions,
        IEnumerable<ConfigureOneCollectorExporter> configurations)
    {
#pragma warning disable CA2000 // Dispose objects before losing scope
        var exporter = new OneCollectorExporter<LogRecord>(CreateSink(exporterOptions));
#pragma warning restore CA2000 // Dispose objects before losing scope

        try
        {
            foreach (var configuration in configurations)
            {
                if (name == configuration.Name)
                {
                    configuration.Configure(serviceProvider, exporter);
                }
            }
        }
        catch
        {
            exporter.Dispose();
            throw;
        }

        return exporter;
    }

    private static WriteDirectlyToTransportSink<LogRecord> CreateSink(OneCollectorLogExporterOptions exporterOptions)
    {
        exporterOptions.Validate();

        var transportOptions = exporterOptions.TransportOptions;

#pragma warning disable CA2000 // Dispose objects before losing scope
        return new WriteDirectlyToTransportSink<LogRecord>(
            new LogRecordCommonSchemaJsonSerializer(
                new EventNameManager(
                    exporterOptions.DefaultEventNamespace,
                    exporterOptions.DefaultEventName,
                    exporterOptions.ParsedEventFullNameMappings),
                exporterOptions.TenantToken!,
                exporterOptions.SerializationOptions.ExceptionStackTraceHandling,
                transportOptions.MaxPayloadSizeInBytes == -1 ? int.MaxValue : transportOptions.MaxPayloadSizeInBytes,
                transportOptions.MaxNumberOfItemsPerPayload == -1 ? int.MaxValue : transportOptions.MaxNumberOfItemsPerPayload),
            new HttpJsonPostTransport(
                exporterOptions.InstrumentationKey!,
                transportOptions.Endpoint,
                transportOptions.HttpCompression,
                new HttpClientWrapper(transportOptions.GetHttpClient())));
#pragma warning restore CA2000 // Dispose objects before losing scope
    }

    private sealed class ConfigureOneCollectorExporter
    {
        public ConfigureOneCollectorExporter(
            string? name,
            Action<IServiceProvider, OneCollectorExporter<LogRecord>> configure)
        {
            this.Name = name;
            this.Configure = configure;
        }

        public string? Name { get; }

        public Action<IServiceProvider, OneCollectorExporter<LogRecord>> Configure { get; }
    }
}
