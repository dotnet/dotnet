// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Globalization;

namespace OpenTelemetry.Exporter.InfluxDB.Tests.Utils;

internal class LineProtocolParser
{
    private static readonly DateTime UnixEpoch = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);

    public static PointData ParseLine(string line)
    {
        var segments = line.Split(' ');
        if (segments.Length is < 2 or > 3)
        {
            throw new ArgumentException("Invalid InfluxDB Line Protocol format.", nameof(line));
        }

        var measurementAndTags = segments[0];
        var fieldsSection = segments[1];
        var timestampSection = segments.Length == 3 ? segments[2] : null;

        var measurementAndTagsParts = measurementAndTags.Split(',');
        var measurement = measurementAndTagsParts[0];
        var tags = ParseTags(measurementAndTagsParts.Skip(1));
        var fields = ParseFields(fieldsSection);
        var timestamp = ParseTimestamp(timestampSection);

        return new PointData { Measurement = measurement, Tags = tags, Fields = fields, Timestamp = timestamp };
    }

    private static Dictionary<string, object> ParseFields(string fieldsSection)
    {
        return fieldsSection.Split(',')
            .Where(field => !string.IsNullOrEmpty(field))
            .Select(field =>
            {
                var kv = field.Split('=');
                var key = kv[0];
                var value = ParseFieldValue(kv[1]);
                return new KeyValuePair<string, object>(key, value);
            })
            .ToDictionary(x => x.Key, x => x.Value);
    }

    private static List<KeyValuePair<string, string>> ParseTags(IEnumerable<string> tagParts)
    {
        return tagParts
            .Select(tagPart =>
            {
                var kv = tagPart.Split('=');
                return new KeyValuePair<string, string>(kv[0], kv[1]);
            })
            .ToList();
    }

    private static object ParseFieldValue(string fieldValue)
    {
        if (bool.TryParse(fieldValue, out var boolValue))
        {
            return boolValue;
        }

#pragma warning disable CA1865 // Use char overload
        if (fieldValue.EndsWith("i", StringComparison.Ordinal)
            && long.TryParse(fieldValue.AsSpan(0, fieldValue.Length - 1).ToString(), out var intValue))
        {
            return intValue;
        }
#pragma warning restore CA1865 // Use char overload

        return double.TryParse(fieldValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var doubleValue)
            ? doubleValue
            : fieldValue;
    }

    private static DateTime ParseTimestamp(string? timestampSection)
    {
        if (string.IsNullOrEmpty(timestampSection) || !long.TryParse(timestampSection, out var unixTimeNanoseconds))
        {
            throw new ArgumentException("Invalid formatted timestamp.");
        }

        var ticks = unixTimeNanoseconds / 100;
        return UnixEpoch.AddTicks(ticks);
    }
}
