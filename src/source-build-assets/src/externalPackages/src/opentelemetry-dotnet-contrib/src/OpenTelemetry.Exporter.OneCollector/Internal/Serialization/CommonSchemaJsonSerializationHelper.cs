// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace OpenTelemetry.Exporter.OneCollector;

internal static class CommonSchemaJsonSerializationHelper
{
#if NET
    public const int MaximumStackAllocSizeInBytes = 256;
#endif

    public const char OneCollectorTenancySymbol = 'o';

    public static readonly byte[] NewLine = "\n"u8.ToArray();

    public static readonly JsonEncodedText VersionProperty = JsonEncodedText.Encode("ver");
    public static readonly JsonEncodedText Version4Value = JsonEncodedText.Encode("4.0");
    public static readonly JsonEncodedText NameProperty = JsonEncodedText.Encode("name");
    public static readonly JsonEncodedText TimeProperty = JsonEncodedText.Encode("time");
    public static readonly JsonEncodedText IKeyProperty = JsonEncodedText.Encode("iKey");
    public static readonly JsonEncodedText ExtensionsProperty = JsonEncodedText.Encode("ext");
    public static readonly JsonEncodedText DataProperty = JsonEncodedText.Encode("data");

    public static void SerializeKeyValueToJson(string key, object? value, Utf8JsonWriter writer)
    {
        writer.WritePropertyName(key);
        SerializeValueToJson(value, writer);
    }

    public static void SerializeValueToJson(object? value, Utf8JsonWriter writer)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        switch (value)
        {
            case bool v:
                writer.WriteBooleanValue(v);
                return;

            case byte v:
                writer.WriteNumberValue(v);
                return;

            case sbyte v:
                writer.WriteNumberValue(v);
                return;

            case short v:
                writer.WriteNumberValue(v);
                return;

            case ushort v:
                writer.WriteNumberValue(v);
                return;

            case int v:
                writer.WriteNumberValue(v);
                return;

            case uint v:
                writer.WriteNumberValue(v);
                return;

            case long v:
                writer.WriteNumberValue(v);
                return;

            case ulong v:
                writer.WriteNumberValue(v);
                return;

            case float v:
                writer.WriteNumberValue(v);
                return;

            case double v:
                writer.WriteNumberValue(v);
                return;

            case decimal v:
                writer.WriteNumberValue(v);
                return;

            case string v:
                writer.WriteStringValue(v);
                return;

            case DateTime v:
                writer.WriteStringValue(v.ToUniversalTime());
                return;

            case DateTimeOffset v:
                writer.WriteStringValue(v);
                return;

#if NET
            case DateOnly v:
                JsonMetadataServices.DateOnlyConverter.Write(writer, v, null!);
                return;
#endif

            case TimeSpan v:
                JsonMetadataServices.TimeSpanConverter.Write(writer, v, null!);
                return;

#if NET
            case TimeOnly v:
                JsonMetadataServices.TimeOnlyConverter.Write(writer, v, null!);
                return;
#endif

            case Guid v:
                writer.WriteStringValue(v);
                return;

            case Uri v:
                JsonMetadataServices.UriConverter.Write(writer, v, null!);
                return;

            case Version v:
                JsonMetadataServices.VersionConverter.Write(writer, v, null!);
                return;

            case byte[] v:
                writer.WriteBase64StringValue(v);
                return;

            case Memory<byte> v:
                writer.WriteBase64StringValue(v.Span);
                return;

            case ArraySegment<byte> v:
                writer.WriteBase64StringValue(v);
                return;

            case Array v:
                SerializeArrayValueToJson(v, writer);
                return;

            case IReadOnlyList<KeyValuePair<string, object?>> v:
                SerializeMapValueToJson(v, writer);
                return;

            case IEnumerable<KeyValuePair<string, object?>> v:
                SerializeMapValueToJson(v, writer);
                return;

            default:
                SerializeObjectValueToJson(value, writer);
                return;
        }
    }

    private static void SerializeArrayValueToJson(Array value, Utf8JsonWriter writer)
    {
        writer.WriteStartArray();

        foreach (var element in value)
        {
            SerializeValueToJson(element, writer);
        }

        writer.WriteEndArray();
    }

    private static void SerializeMapValueToJson(IReadOnlyList<KeyValuePair<string, object?>> value, Utf8JsonWriter writer)
    {
        writer.WriteStartObject();

        for (var i = 0; i < value.Count; i++)
        {
            var element = value[i];

            if (string.IsNullOrEmpty(element.Key))
            {
                continue;
            }

            SerializeKeyValueToJson(element.Key, element.Value, writer);
        }

        writer.WriteEndObject();
    }

    private static void SerializeMapValueToJson(IEnumerable<KeyValuePair<string, object?>> value, Utf8JsonWriter writer)
    {
        writer.WriteStartObject();

        foreach (var element in value)
        {
            if (string.IsNullOrEmpty(element.Key))
            {
                continue;
            }

            SerializeKeyValueToJson(element.Key, element.Value, writer);
        }

        writer.WriteEndObject();
    }

    private static void SerializeObjectValueToJson(object value, Utf8JsonWriter writer)
    {
#if NET
        if (value is ISpanFormattable spanFormattable)
        {
            Span<char> destination = stackalloc char[MaximumStackAllocSizeInBytes / 2];
            if (spanFormattable.TryFormat(destination, out var charsWritten, string.Empty, CultureInfo.InvariantCulture))
            {
                writer.WriteStringValue(destination.Slice(0, charsWritten));
                return;
            }
        }
#endif

        string v;

        try
        {
            v = Convert.ToString(value, CultureInfo.InvariantCulture)!;
        }
        catch
        {
            v = $"ERROR: type {value.GetType().FullName} is not supported";
        }

        writer.WriteStringValue(v);
    }
}
