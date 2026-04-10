// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Internal;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Exporter.Geneva;

/// <summary>
/// Contains extension methods to register the Geneva trace exporter.
/// </summary>
public static class GenevaExporterHelperExtensions
{
    /// <summary>
    /// Adds <see cref="GenevaTraceExporter"/> to the <see cref="TracerProviderBuilder"/>.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> builder to use.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddGenevaTraceExporter(this TracerProviderBuilder builder)
        => AddGenevaTraceExporter(builder, name: null, configure: null);

    /// <summary>
    /// Adds <see cref="GenevaTraceExporter"/> to the <see cref="TracerProviderBuilder"/>.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> builder to use.</param>
    /// <param name="configure">Callback action for configuring <see cref="GenevaExporterOptions"/>.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddGenevaTraceExporter(this TracerProviderBuilder builder, Action<GenevaExporterOptions> configure)
        => AddGenevaTraceExporter(builder, name: null, configure);

    /// <summary>
    /// Adds <see cref="GenevaTraceExporter"/> to the <see cref="TracerProviderBuilder"/>.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> builder to use.</param>
    /// <param name="name">Optional name which is used when retrieving options.</param>
    /// <param name="configure">Optional callback action for configuring <see cref="GenevaExporterOptions"/>.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddGenevaTraceExporter(
        this TracerProviderBuilder builder,
        string? name,
        Action<GenevaExporterOptions>? configure)
    {
        Guard.ThrowIfNull(builder);

        var finalOptionsName = name ?? Options.DefaultName;

        builder.ConfigureServices(services =>
        {
            if (name != null && configure != null)
            {
                // If we are using named options we register the
                // configuration delegate into options pipeline.
                services.Configure(finalOptionsName, configure);
            }
        });

        return builder.AddProcessor(sp =>
        {
            GenevaExporterOptions exporterOptions;

            BatchExportActivityProcessorOptions batchExportActivityProcessorOptions;

            if (name == null)
            {
                // If we are NOT using named options we create a new
                // instance always. The reason for this is
                // GenevaExporterOptions is shared by tracing and logging signals. Without a
                // name, delegates for all signals will mix together. See:
                // https://github.com/open-telemetry/opentelemetry-dotnet/issues/4043
                exporterOptions = sp.GetRequiredService<IOptionsFactory<GenevaExporterOptions>>().Create(finalOptionsName);

                batchExportActivityProcessorOptions = sp.GetRequiredService<IOptionsFactory<BatchExportActivityProcessorOptions>>().Create(finalOptionsName);

                // Configuration delegate is executed inline on the fresh instance.
                configure?.Invoke(exporterOptions);
            }
            else
            {
                // When using named options we can properly utilize Options
                // API to create or reuse an instance.
                exporterOptions = sp.GetRequiredService<IOptionsMonitor<GenevaExporterOptions>>().Get(finalOptionsName);

                batchExportActivityProcessorOptions = sp.GetRequiredService<IOptionsMonitor<BatchExportActivityProcessorOptions>>().Get(finalOptionsName);
            }

            return BuildGenevaTraceExporter(
                exporterOptions,
                batchExportActivityProcessorOptions);
        });
    }

    private static BaseProcessor<Activity> BuildGenevaTraceExporter(
        GenevaExporterOptions options,
        BatchExportActivityProcessorOptions batchActivityExportProcessor)
    {
#pragma warning disable CA2000 // Dispose objects before losing scope
        var exporter = new GenevaTraceExporter(options);
#pragma warning restore CA2000 // Dispose objects before losing scope

        if (exporter.IsUsingUnixDomainSocket)
        {
            return new BatchActivityExportProcessor(
                exporter,
                batchActivityExportProcessor.MaxQueueSize,
                batchActivityExportProcessor.ScheduledDelayMilliseconds,
                batchActivityExportProcessor.ExporterTimeoutMilliseconds,
                batchActivityExportProcessor.MaxExportBatchSize);
        }
        else
        {
            return new ReentrantActivityExportProcessor(exporter);
        }
    }
}
