// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.InfluxDB.Tests.Utils;

internal class PointData
{
    public string Measurement { get; init; } = null!;

    public IReadOnlyList<KeyValuePair<string, string>> Tags { get; init; } = null!;

    public IReadOnlyDictionary<string, object> Fields { get; init; } = null!;

    public DateTime Timestamp { get; init; }
}
