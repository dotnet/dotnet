// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#nullable disable

using System.Text;
using OpenTelemetry.Exporter.Geneva.Tld;
using Xunit;

namespace OpenTelemetry.Exporter.Geneva.Tests;

public static class JsonSerializerTests
{
    public static IEnumerable<object[]> Data =>
        [
            [null, "null"],
            [true, "true"],
            [false, "false"],
            [0, "0"],
            [123, "123"],
            [-123, "-123"],
            [0.0f, "0"],
            [1.0f, "1"],
            [3.14f, "3.14"],
            [-3.14f, "-3.14"],
            [0.0d, "0"],
            [3.14d, "3.14"],
            [3.1415926d, "3.1415926"],
            [-3.1415926d, "-3.1415926"],
            [string.Empty, "''".Replace("'", "\"")],
            ["Hello, World!", "'Hello, World!'".Replace("'", "\"")],
            ["\"", "'\\\"'".Replace("'", "\"")],
            ["\n", "'\\n'".Replace("'", "\"")],
            ["\t", "'\\t'".Replace("'", "\"")],
            ["\0", "'\\u0000'".Replace("'", "\"")],
            ["\u6768", "'\\u6768'".Replace("'", "\"")],
            [Array.Empty<object>(), "[]"],
            [new object[] { 1, 2, 3 }, "[1,2,3]"],
            [new Dictionary<string, object>(), "{}"],
            [new Dictionary<string, object> { ["foo"] = 1, ["bar"] = "baz", ["golden ratio"] = 0.6180340f, ["pi"] = 3.14159265358979d }, "{'foo':1,'bar':'baz','golden ratio':0.618034,'pi':3.14159265358979}".Replace("'", "\"")],
        ];

    [Theory]
    [MemberData(nameof(Data))]
    [Trait("Platform", "Any")]
    public static void TestSerialization(object value, string expected)
    {
        var buffer = new byte[65_536];
        var length = JsonSerializer.Serialize(buffer, 0, value);
        Assert.Equal(expected, Encoding.ASCII.GetString(buffer, 0, length));
    }

    [Fact]
    [Trait("Platform", "Any")]
    public static void JsonSerializer_Null()
    {
        TestSerialization(null, "null");
    }

    [Fact]
    [Trait("Platform", "Any")]
    public static void JsonSerializer_Boolean()
    {
        TestSerialization(true, "true");
        TestSerialization(false, "false");
    }

    [Fact]
    [Trait("Platform", "Any")]
    public static void JsonSerializer_Numeric()
    {
        TestSerialization(0, "0");
        TestSerialization(123, "123");
        TestSerialization(-123, "-123");
        TestSerialization(0.0f, "0");
        TestSerialization(1.0f, "1");
        TestSerialization(3.14f, "3.14");
        TestSerialization(-3.14f, "-3.14");
        TestSerialization(0.0d, "0");
        TestSerialization(3.14d, "3.14");
        TestSerialization(3.1415926d, "3.1415926");
        TestSerialization(-3.1415926d, "-3.1415926");
    }

    [Fact]
    [Trait("Platform", "Any")]
    public static void JsonSerializer_String()
    {
        TestSerialization(string.Empty, "''".Replace("'", "\""));
        TestSerialization("Hello, World!", "'Hello, World!'".Replace("'", "\""));
        TestSerialization("\"", "'\\\"'".Replace("'", "\""));
        TestSerialization("\n", "'\\n'".Replace("'", "\""));
        TestSerialization("\t", "'\\t'".Replace("'", "\""));
        TestSerialization("\0", "'\\u0000'".Replace("'", "\""));
        TestSerialization("\u6768", "'\\u6768'".Replace("'", "\""));
    }

    [Fact]
    [Trait("Platform", "Any")]
    public static void JsonSerializer_Array()
    {
        TestSerialization(Array.Empty<object>(), "[]");
        TestSerialization(new object[] { 1, 2, 3 }, "[1,2,3]");
    }

    [Fact]
    [Trait("Platform", "Any")]
    public static void JsonSerializer_Map()
    {
        TestSerialization(new Dictionary<string, object>(), "{}");
        TestSerialization(
            new Dictionary<string, object>
            {
                ["foo"] = 1,
                ["bar"] = "baz",
                ["golden ratio"] = 0.6180340f,
                ["pi"] = 3.14159265358979d,
            },
            "{'foo':1,'bar':'baz','golden ratio':0.618034,'pi':3.14159265358979}".Replace("'", "\""));
    }
}
