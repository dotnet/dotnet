// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text;
using System.Text.Json;
using Xunit;

namespace OpenTelemetry.Exporter.OneCollector.Tests;

public class CommonSchemaJsonSerializationHelperTests
{
    [Fact]
    public void SerializeKeyValueToJsonTest()
    {
        var actualJson = GetJson(key: "key1", value: "value1");

        Assert.Equal("\"key1\":\"value1\"", actualJson);
    }

    [Theory]
    [InlineData("stringValue1", "\"stringValue1\"")]
    [InlineData(18, "18")]
    [InlineData(18L, "18")]
    [InlineData((short)18, "18")]
    [InlineData(18U, "18")]
    [InlineData(18UL, "18")]
    [InlineData((ushort)18, "18")]
    [InlineData((byte)18, "18")]
    [InlineData((sbyte)18, "18")]
    [InlineData(true, "true")]
    [InlineData(false, "false")]
    [InlineData(null, "null")]
    [InlineData(1.01D, "1.01")]
#if NETFRAMEWORK
    // Note: There seems to be some kind of round-tripping bug in .NET
    // Framework. See:
    // https://stackoverflow.com/questions/24299692/why-is-a-round-trip-conversion-via-a-string-not-safe-for-a-double
    [InlineData(2.1099999F, "2.1099999")]
#else
    [InlineData(1.02F, "1.02")]
#endif
    public void SerializeValueToJsonTest(object? value, string expectedJson)
    {
        var actualJson = GetJson(value);

        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public void SerializeComplexValueToJsonTest()
    {
        this.SerializeValueToJsonTest(18.99M, "18.99");

        /* Begin note: STJ does some trimming of DateTime\Offsets so we manually
         * construct these values. See:
         * https://github.com/dotnet/runtime/blob/78ed4438a42acab80541e9bde1910abaa8841db2/src/libraries/System.Text.Json/src/System/Text/Json/Writer/JsonWriterHelper.Date.cs#L43
         */
        var dt = DateTime.SpecifyKind(new DateTime(2023, 1, 18, 10, 18, 0), DateTimeKind.Utc);
        this.SerializeValueToJsonTest(dt, "\"2023-01-18T10:18:00Z\"");
        var dto = new DateTimeOffset(new DateTime(2023, 1, 18, 10, 18, 0), new TimeSpan(1, 0, 0));
        this.SerializeValueToJsonTest(dto, "\"2023-01-18T10:18:00+01:00\"");
        /* End note. */

        var byteArray = new byte[] { 0, 0xff };
        this.SerializeValueToJsonTest(byteArray, "\"AP8=\"");
        this.SerializeValueToJsonTest(new ArraySegment<byte>(byteArray, 0, byteArray.Length), "\"AP8=\"");
        this.SerializeValueToJsonTest(new Memory<byte>(byteArray, 0, byteArray.Length), "\"AP8=\"");

        var array = new[] { 0, 1, 18 };
        this.SerializeValueToJsonTest(array, "[0,1,18]");

        var listMap = new List<KeyValuePair<string, object?>> { new("key1", "value1") };
        this.SerializeValueToJsonTest(listMap, "{\"key1\":\"value1\"}");

        var dictMap = new Dictionary<string, object?> { ["key1"] = "value1" };
        this.SerializeValueToJsonTest(dictMap, "{\"key1\":\"value1\"}");

        var typeWithToString = new TypeWithToString();
        this.SerializeValueToJsonTest(typeWithToString, "\"Hello world\"");

        var typeWithThrowingToString = new TypeWithThrowingToString();
        this.SerializeValueToJsonTest(typeWithThrowingToString, $"\"ERROR: type {typeof(CommonSchemaJsonSerializationHelperTests).FullName}\\u002B{nameof(TypeWithThrowingToString)} is not supported\"");

        var ts = new TimeSpan(0, 10, 18, 59, 1);
        this.SerializeValueToJsonTest(ts, "\"10:18:59.0010000\"");

        var guid = Guid.NewGuid();
        this.SerializeValueToJsonTest(guid, $"\"{guid}\"");

        var uri = new Uri("http://www.localhost.com");
        this.SerializeValueToJsonTest(uri, "\"http://www.localhost.com\"");

        var version = new Version("1.4.0");
        this.SerializeValueToJsonTest(version, "\"1.4.0\"");

#if NET
        var typeWithISpanFormattable = new TypeWithISpanFormattable(overflow: false);
        this.SerializeValueToJsonTest(typeWithISpanFormattable, "\"hello\"");

        var typeWithISpanFormattableOverflow = new TypeWithISpanFormattable(overflow: true);
        this.SerializeValueToJsonTest(typeWithISpanFormattableOverflow, "\"Overflow\"");

        var dateOnly = DateOnly.FromDateTime(dt);
        this.SerializeValueToJsonTest(dateOnly, $"\"{dateOnly:O}\"");

        var to = TimeOnly.FromTimeSpan(ts);
        this.SerializeValueToJsonTest(to, "\"10:18:59.0010000\"");
#endif
    }

    private static string GetJson(object? value, string? key = null)
    {
        using var stream = new MemoryStream();

        using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { SkipValidation = true }))
        {
            if (key is not null)
            {
                CommonSchemaJsonSerializationHelper.SerializeKeyValueToJson(key, value, writer);
            }
            else
            {
                CommonSchemaJsonSerializationHelper.SerializeValueToJson(value, writer);
            }
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private sealed class TypeWithToString
    {
        public override string ToString() => "Hello world";
    }

    private sealed class TypeWithThrowingToString
    {
        public override string ToString() => throw new NotImplementedException();
    }

#if NET
    private sealed class TypeWithISpanFormattable : ISpanFormattable
    {
        private readonly bool overflow;

        public TypeWithISpanFormattable(bool overflow)
        {
            this.overflow = overflow;
        }

        public override string ToString()
            => "Overflow";

        public string ToString(string? format, IFormatProvider? formatProvider)
            => this.ToString();

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            if (!this.overflow && destination.Length >= 5)
            {
                destination[0] = 'h';
                destination[1] = 'e';
                destination[2] = 'l';
                destination[3] = 'l';
                destination[4] = 'o';
                charsWritten = 5;
                return true;
            }

            charsWritten = 0;
            return false;
        }
    }
#endif
}
