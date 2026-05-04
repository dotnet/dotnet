// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.CompilerServices;
using OpenTelemetry.Context;
using OpenTelemetry.Logs;

namespace OpenTelemetry.Exporter.Geneva;

/// <summary>
/// A processor that enriches log records with AFDCorrelationId if it's present in the RuntimeContext.
/// </summary>
internal class AFDCorrelationIdLogProcessor : BaseProcessor<LogRecord>
{
    private const string AFDCorrelationId = "AFDCorrelationId";
    private const string AFDSlotAccessTrackerId = "GenevaAfdCorrelationIdStateTracker";
    private static readonly RuntimeContextSlot<bool> GenevaAfdCorrelationIdStateTracker = RuntimeContext.RegisterSlot<bool>(AFDSlotAccessTrackerId);
    private bool disposed;

    public AFDCorrelationIdLogProcessor()
    {
        GenevaAfdCorrelationIdStateTracker.Set(false);
    }

    /// <inheritdoc/>
    public override void OnEnd(LogRecord data)
    {
        if (data == null)
        {
            return;
        }

        var afdCorrelationIdValue = GetRuntimeContextValue();
        if (string.IsNullOrEmpty(afdCorrelationIdValue))
        {
            base.OnEnd(data);
            return;
        }

        var capacity = data.Attributes?.Count + 1 ?? 1;
        var attributes = new List<KeyValuePair<string, object?>>(capacity);

        if (data.Attributes != null)
        {
            attributes.AddRange(data.Attributes);
        }

        attributes.Add(new KeyValuePair<string, object?>(AFDCorrelationId, afdCorrelationIdValue));
        data.Attributes = attributes;
        base.OnEnd(data);
    }

    protected override void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                GenevaAfdCorrelationIdStateTracker.Dispose();
            }

            this.disposed = true;
        }

        base.Dispose(disposing);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string? GetRuntimeContextValue()
    {
        if (GenevaAfdCorrelationIdStateTracker.Get() == true)
        {
            return null;
        }

        try
        {
            return RuntimeContext.GetValue<string>(AFDCorrelationId);
        }
        catch (Exception ex)
        {
            ExporterEventSource.Log.FailedToGetAFDCorrelationId(ex);
            GenevaAfdCorrelationIdStateTracker.Set(true);
            return null;
        }
    }
}
