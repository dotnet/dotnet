// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Internal;
using OpenTelemetry.Resources;

namespace OpenTelemetry.Exporter.OneCollector;

/// <summary>
/// OpenTelemetry exporter implementation for sending telemetry data to
/// Microsoft OneCollector.
/// </summary>
/// <typeparam name="T">Item type.</typeparam>
public sealed class OneCollectorExporter<T> : BaseExporter<T>
    where T : class
{
    private readonly string typeName;
    private readonly ISink<T> sink;
    private Resource? resource;

    /// <summary>
    /// Initializes a new instance of the <see cref="OneCollectorExporter{T}"/> class.
    /// </summary>
    /// <param name="sink"><see cref="ISink{T}"/>.</param>
    internal OneCollectorExporter(ISink<T> sink)
    {
        Guard.ThrowIfNull(sink);

        this.typeName = typeof(T).Name;
        this.sink = sink;
    }

    /// <inheritdoc/>
    public sealed override ExportResult Export(in Batch<T> batch)
    {
        try
        {
            var resource = this.resource ??= this.ParentProvider?.GetResource() ?? Resource.Empty;

            var numberOfRecordsWritten = this.sink.Write(resource, in batch);

            return numberOfRecordsWritten > 0
                ? ExportResult.Success
                : ExportResult.Failure;
        }
        catch (Exception ex)
        {
            OneCollectorExporterEventSource.Log.WriteExportExceptionThrownEventIfEnabled(this.typeName, ex);

            return ExportResult.Failure;
        }
    }

    /// <summary>
    /// Register a callback action that will be triggered any time a payload is
    /// successfully transmitted by the exporter.
    /// </summary>
    /// <remarks>
    /// Success or failure of a transmission depends on the transport being
    /// used. In the case of HTTP transport, success is driven by the HTTP
    /// response status code (anything in the 200-range indicates success) and
    /// any other result (connection failure, timeout, non-200 response code,
    /// etc.) is considered a failure.
    /// </remarks>
    /// <param name="callback"><see
    /// cref="OneCollectorExporterPayloadTransmittedCallbackAction"/>.</param>
    /// <returns><see langword="null"/> if no transport is tied to the exporter
    /// or an <see cref="IDisposable"/> representing the registered callback.
    /// Call <see cref="IDisposable.Dispose"/> on the returned instance to
    /// cancel the registration.</returns>
    public IDisposable? RegisterPayloadTransmittedCallback(OneCollectorExporterPayloadTransmittedCallbackAction callback)
        => this.RegisterPayloadTransmittedCallback(callback, includeFailures: false);

    /// <summary>
    /// Register a callback action that will be triggered any time a payload is
    /// transmitted by the exporter.
    /// </summary>
    /// <remarks><inheritdoc cref="RegisterPayloadTransmittedCallback(OneCollectorExporterPayloadTransmittedCallbackAction)" path="/remarks"/></remarks>
    /// <param name="callback"><see
    /// cref="OneCollectorExporterPayloadTransmittedCallbackAction"/>.</param>
    /// <param name="includeFailures">Specify <see langword="true"/> to receive
    /// callbacks when transmission fails. See <see
    /// cref="OneCollectorExporterPayloadTransmittedCallbackArguments.Succeeded"/>
    /// for details about how a success or failure is determined.</param>
    /// <returns><see langword="null"/> if no transport is tied to the exporter
    /// or an <see cref="IDisposable"/> representing the registered callback.
    /// Call <see cref="IDisposable.Dispose"/> on the returned instance to
    /// cancel the registration.</returns>
    public IDisposable? RegisterPayloadTransmittedCallback(OneCollectorExporterPayloadTransmittedCallbackAction callback, bool includeFailures)
        => this.sink.Transport?.RegisterPayloadTransmittedCallback(callback, includeFailures);

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            (this.sink as IDisposable)?.Dispose();
        }

        base.Dispose(disposing);
    }
}
