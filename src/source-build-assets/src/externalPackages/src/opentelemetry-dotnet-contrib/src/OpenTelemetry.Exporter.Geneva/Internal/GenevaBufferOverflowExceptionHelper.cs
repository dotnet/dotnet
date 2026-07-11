// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET
using Microsoft.LinuxTracepoints.Provider;
#endif

namespace OpenTelemetry.Exporter.Geneva;

internal static class GenevaBufferOverflowExceptionHelper
{
    internal const string MetricBufferTooSmallMessage = "The buffer is too small to write a value at the specified index.";

    internal static bool TryReportTraceBufferOverflow(Exception ex)
    {
        if (!IsTraceBufferOverflow(ex))
        {
            return false;
        }

        ExporterEventSource.Log.TraceSerializationBufferFull();
        return true;
    }

    internal static bool TryReportLogBufferOverflow(Exception ex)
    {
        if (!IsLogBufferOverflow(ex))
        {
            return false;
        }

        ExporterEventSource.Log.LogSerializationBufferFull();
        return true;
    }

    internal static bool TryReportMetricBufferOverflow(string metricName, Exception ex)
    {
        if (!IsMetricBufferOverflow(ex))
        {
            return false;
        }

        ExporterEventSource.Log.MetricSerializationBufferFull(metricName, GenevaMetricExporter.BufferSize);
        return true;
    }

    internal static bool TryReportLogBufferOverflow(int errorCode)
    {
        if (!IsEventHeaderBufferOverflow(errorCode))
        {
            return false;
        }

        ExporterEventSource.Log.LogSerializationBufferFull();
        return true;
    }

    internal static bool IsTraceBufferOverflow(Exception ex) =>
        IsMsgPackBufferOverflow(ex) || IsEventBuilderBufferOverflow(ex);

    internal static bool IsLogBufferOverflow(Exception ex) =>
        IsMsgPackBufferOverflow(ex) || IsEventBuilderBufferOverflow(ex);

    internal static bool IsMetricBufferOverflow(Exception ex) =>
        IsMsgPackBufferOverflow(ex) || IsMetricSerializerBufferOverflow(ex);

    internal static bool IsMsgPackBufferOverflow(Exception ex) =>
        ex is IndexOutOfRangeException || ex.GetType() == typeof(ArgumentException);

    internal static bool IsMetricSerializerBufferOverflow(Exception ex) =>
        ex is ArgumentOutOfRangeException argumentOutOfRangeException
        && argumentOutOfRangeException.Message.Contains(MetricBufferTooSmallMessage, StringComparison.Ordinal);

    internal static bool IsEventBuilderBufferOverflow(Exception ex) =>
        ex is InvalidOperationException { Message: "Event too large" };

    internal static bool IsEventHeaderBufferOverflow(int errorCode) =>
#if NET
        errorCode == TracepointHandle.EventTooBigError;
#else
        false;
#endif
}
