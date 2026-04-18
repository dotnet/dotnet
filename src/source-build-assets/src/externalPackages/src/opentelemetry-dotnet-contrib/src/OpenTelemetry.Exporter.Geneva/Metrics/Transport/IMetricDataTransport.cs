// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.Geneva;

internal interface IMetricDataTransport : IDisposable
{
    /// <summary>
    /// Writes a standard metric event containing only a single value.
    /// </summary>
    /// <param name="eventType">Type of the event.</param>
    /// <param name="body">The byte array containing the serialized data.</param>
    /// <param name="size">Length of the payload (fixed + variable).</param>
    void Send(
        MetricEventType eventType,
        byte[] body,
        int size);

    /// <summary>
    /// Writes a standard metric event containing only a single value.
    /// </summary>
    /// <param name="body">The byte array containing the serialized data.</param>
    /// <param name="size">Length of the payload (fixed + variable).</param>
    void SendOtlpProtobufEvent(
        byte[] body,
        int size);
}
