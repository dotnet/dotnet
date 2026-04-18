// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using InfluxDB.Client;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace OpenTelemetry.Exporter.InfluxDB;

internal interface IMetricsWriter
{
    void Write(Metric metric, Resource? resource, WriteApi writeApi);
}
