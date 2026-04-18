// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.OneCollector;

internal interface ITransport
{
    string Description { get; }

    bool Send(in TransportSendRequest sendRequest);

    IDisposable RegisterPayloadTransmittedCallback(
        OneCollectorExporterPayloadTransmittedCallbackAction callback,
        bool includeFailures);
}
