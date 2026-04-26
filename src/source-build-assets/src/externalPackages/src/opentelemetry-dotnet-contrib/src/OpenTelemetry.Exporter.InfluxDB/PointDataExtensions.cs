// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using InfluxDB.Client.Writes;

namespace OpenTelemetry.Exporter.InfluxDB;

internal static class PointDataExtensions
{
    public static PointData Tags(this PointData pointData, ReadOnlyTagCollection tags)
    {
        foreach (var tag in tags)
        {
            pointData = pointData.Tag(tag.Key, tag.Value?.ToString());
        }

        return pointData;
    }

    public static PointData Tags(this PointData pointData, IEnumerable<KeyValuePair<string, object>>? tags)
    {
        if (tags == null)
        {
            return pointData;
        }

        foreach (var tag in tags)
        {
            pointData = pointData.Tag(tag.Key, tag.Value.ToString());
        }

        return pointData;
    }
}
