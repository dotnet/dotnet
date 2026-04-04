// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#nullable disable

using System.Globalization;
using System.Text;
using OpenTelemetry.Exporter.Geneva.MsgPack;
using Xunit;
using Xunit.Sdk;

namespace OpenTelemetry.Exporter.Geneva.Tests;

public class MessagePackSerializerTests
{
    private void AssertBytes(byte[] expected, byte[] actual, int length)
    {
        Assert.Equal(expected.Length, length);
        for (var i = 0; i < length; i++)
        {
            var expectedByte = expected[i];
            var actualByte = actual[i];

            Assert.Equal(expectedByte, actualByte);
        }
    }

    private void MessagePackSerializer_TestSerialization<T>(T value)
    {
        var buffer = new byte[64 * 1024];
        var length = MessagePackSerializer.Serialize(buffer, 0, value);
        this.AssertBytes(MessagePack.MessagePackSerializer.Serialize(value), buffer, length);
    }

    private void MessagePackSerializer_TestASCIIStringSerialization(string input, int sizeLimit = (1 << 14) - 1)
    {
        // sizeLimit is the max length of string allowed
        var buffer = new byte[64 * 1024];
        var length = MessagePackSerializer.SerializeAsciiString(buffer, 0, input, sizeLimit);
        var deserializedString = MessagePack.MessagePackSerializer.Deserialize<string>(buffer);
        if (!string.IsNullOrEmpty(input) && input.Length > sizeLimit)
        {
            // We truncate the string using `.` in the last three characters which takes 3 bytes of memory
#pragma warning disable CA1846 // Prefer 'AsSpan' over 'Substring'
            var byteCount = Encoding.ASCII.GetByteCount(input.Substring(0, sizeLimit - 3)) + 3;
#pragma warning restore CA1846 // Prefer 'AsSpan' over 'Substring'
            Assert.Equal(0xDA, buffer[0]);
            Assert.Equal(byteCount, (buffer[1] << 8) | buffer[2]);
            Assert.Equal(byteCount, length - 3); // First three bytes are metadata

            Assert.NotEqual(input, deserializedString);

            int i;
            for (i = 0; i < sizeLimit - 3; i++)
            {
                Assert.Equal(input[i], deserializedString[i]);
            }

            Assert.Equal('.', deserializedString[i++]);
            Assert.Equal('.', deserializedString[i++]);
            Assert.Equal('.', deserializedString[i++]);
        }
        else
        {
            if (input != null)
            {
                var byteCount = Encoding.ASCII.GetByteCount(input);
                if (input.Length <= 31)
                {
                    Assert.Equal(0xA0 | byteCount, buffer[0]);
                    Assert.Equal(byteCount, length - 1); // First one byte is metadata
                }
                else if (input.Length <= 255)
                {
                    Assert.Equal(0xD9, buffer[0]);
                    Assert.Equal(byteCount, buffer[1]);
                    Assert.Equal(byteCount, length - 2); // First two bytes are metadata
                }
                else if (input.Length <= sizeLimit)
                {
                    Assert.Equal(0xDA, buffer[0]);
                    Assert.Equal(byteCount, (buffer[1] << 8) | buffer[2]);
                    Assert.Equal(byteCount, length - 3); // First three bytes are metadata
                }
            }

            Assert.Equal(input, deserializedString);
        }
    }

    private void MessagePackSerializer_TestUnicodeStringSerialization(string input, int sizeLimit = (1 << 14) - 1)
    {
        // sizeLimit is the max length of string allowed
        var buffer = new byte[64 * 1024];
        var length = MessagePackSerializer.SerializeUnicodeString(buffer, 0, input, sizeLimit);

        var deserializedString = MessagePack.MessagePackSerializer.Deserialize<string>(buffer);
        if (!string.IsNullOrEmpty(input) && input.Length > sizeLimit)
        {
            // We truncate the string using `.` in the last three characters which takes 3 bytes of memory
#pragma warning disable CA1846 // Prefer 'AsSpan' over 'Substring'
            var byteCount = Encoding.UTF8.GetByteCount(input.Substring(0, sizeLimit - 3)) + 3;
#pragma warning restore CA1846 // Prefer 'AsSpan' over 'Substring'
            Assert.Equal(0xDA, buffer[0]);
            Assert.Equal(byteCount, (buffer[1] << 8) | buffer[2]);
            Assert.Equal(byteCount, length - 3); // First three bytes are metadata

            Assert.NotEqual(input, deserializedString);

            int i;
            for (i = 0; i < sizeLimit - 3; i++)
            {
                Assert.Equal(input[i], deserializedString[i]);
            }

            Assert.Equal('.', deserializedString[i++]);
            Assert.Equal('.', deserializedString[i++]);
            Assert.Equal('.', deserializedString[i++]);
        }
        else
        {
            Assert.Equal(input, deserializedString);

            if (input != null)
            {
                var byteCount = Encoding.UTF8.GetByteCount(input);
                Assert.Equal(0xDA, buffer[0]);
                Assert.Equal(byteCount, (buffer[1] << 8) | buffer[2]);
                Assert.Equal(byteCount, length - 3); // First three bytes are metadata
            }
        }
    }

    [Fact]
    public void MessagePackSerializer_Null()
    {
        this.MessagePackSerializer_TestSerialization<object>(null);
    }

    [Fact]
    public void MessagePackSerializer_Boolean()
    {
        this.MessagePackSerializer_TestSerialization(true);
        this.MessagePackSerializer_TestSerialization(false);
    }

    [Fact]
    public void MessagePackSerializer_Int()
    {
        // 8 bits
        for (var value = sbyte.MinValue; value < sbyte.MaxValue; value++)
        {
            this.MessagePackSerializer_TestSerialization(value);
        }

        this.MessagePackSerializer_TestSerialization(sbyte.MaxValue);

        // 16 bits
        for (var value = short.MinValue; value < short.MaxValue; value++)
        {
            this.MessagePackSerializer_TestSerialization(value);
        }

        this.MessagePackSerializer_TestSerialization(short.MaxValue);

        // 32 bits
        this.MessagePackSerializer_TestSerialization(int.MinValue);
        this.MessagePackSerializer_TestSerialization(int.MinValue + 1);
        this.MessagePackSerializer_TestSerialization(short.MinValue - 1);
        this.MessagePackSerializer_TestSerialization((int)short.MinValue);
        this.MessagePackSerializer_TestSerialization(short.MinValue + 1);
        this.MessagePackSerializer_TestSerialization(sbyte.MinValue - 1);
        for (var value = sbyte.MinValue; value < sbyte.MaxValue; value++)
        {
            this.MessagePackSerializer_TestSerialization((int)value);
        }

        this.MessagePackSerializer_TestSerialization((int)sbyte.MaxValue);
        this.MessagePackSerializer_TestSerialization(sbyte.MaxValue + 1);
        this.MessagePackSerializer_TestSerialization(short.MaxValue - 1);
        this.MessagePackSerializer_TestSerialization((int)short.MaxValue);
        this.MessagePackSerializer_TestSerialization(short.MaxValue + 1);
        this.MessagePackSerializer_TestSerialization(int.MaxValue - 1);
        this.MessagePackSerializer_TestSerialization(int.MaxValue);

        // 64 bits
        this.MessagePackSerializer_TestSerialization(long.MinValue);
        this.MessagePackSerializer_TestSerialization(long.MinValue + 1);
        this.MessagePackSerializer_TestSerialization((long)int.MinValue - 1);
        this.MessagePackSerializer_TestSerialization((long)int.MinValue);
        this.MessagePackSerializer_TestSerialization((long)int.MinValue + 1);
        this.MessagePackSerializer_TestSerialization((long)short.MinValue - 1);
        this.MessagePackSerializer_TestSerialization((long)short.MinValue);
        this.MessagePackSerializer_TestSerialization((long)short.MinValue + 1);
        this.MessagePackSerializer_TestSerialization((long)sbyte.MinValue - 1);
        for (var value = sbyte.MinValue; value < sbyte.MaxValue; value++)
        {
            this.MessagePackSerializer_TestSerialization((long)value);
        }

        this.MessagePackSerializer_TestSerialization((long)sbyte.MaxValue);
        this.MessagePackSerializer_TestSerialization((long)sbyte.MaxValue + 1);
        this.MessagePackSerializer_TestSerialization((long)short.MaxValue - 1);
        this.MessagePackSerializer_TestSerialization((long)short.MaxValue);
        this.MessagePackSerializer_TestSerialization((long)short.MaxValue + 1);
        this.MessagePackSerializer_TestSerialization((long)int.MaxValue - 1);
        this.MessagePackSerializer_TestSerialization((long)int.MaxValue);
        this.MessagePackSerializer_TestSerialization((long)int.MaxValue + 1);
        this.MessagePackSerializer_TestSerialization(long.MaxValue - 1);
        this.MessagePackSerializer_TestSerialization(long.MaxValue);
    }

    [Fact]
    public void MessagePackSerializer_UInt()
    {
        // 8 bits
        for (var value = byte.MinValue; value < byte.MaxValue; value++)
        {
            this.MessagePackSerializer_TestSerialization(value);
        }

        this.MessagePackSerializer_TestSerialization(byte.MaxValue);

        // 16 bits
        for (var value = ushort.MinValue; value < ushort.MaxValue; value++)
        {
            this.MessagePackSerializer_TestSerialization(value);
        }

        this.MessagePackSerializer_TestSerialization(ushort.MaxValue);

        // 32 bits
        this.MessagePackSerializer_TestSerialization(uint.MinValue);
        this.MessagePackSerializer_TestSerialization((uint)byte.MaxValue - 1);
        this.MessagePackSerializer_TestSerialization((uint)byte.MaxValue);
        this.MessagePackSerializer_TestSerialization((uint)byte.MaxValue + 1);
        this.MessagePackSerializer_TestSerialization((uint)ushort.MaxValue - 1);
        this.MessagePackSerializer_TestSerialization((uint)ushort.MaxValue);
        this.MessagePackSerializer_TestSerialization((uint)ushort.MaxValue + 1);
        this.MessagePackSerializer_TestSerialization(uint.MaxValue - 1);
        this.MessagePackSerializer_TestSerialization(uint.MaxValue);

        // 64 bits
        this.MessagePackSerializer_TestSerialization(ulong.MinValue);
        this.MessagePackSerializer_TestSerialization((ulong)byte.MaxValue - 1);
        this.MessagePackSerializer_TestSerialization((ulong)byte.MaxValue);
        this.MessagePackSerializer_TestSerialization((ulong)byte.MaxValue + 1);
        this.MessagePackSerializer_TestSerialization((ulong)ushort.MaxValue - 1);
        this.MessagePackSerializer_TestSerialization((ulong)ushort.MaxValue);
        this.MessagePackSerializer_TestSerialization((ulong)ushort.MaxValue + 1);
        this.MessagePackSerializer_TestSerialization((ulong)uint.MaxValue - 1);
        this.MessagePackSerializer_TestSerialization((ulong)uint.MaxValue);
        this.MessagePackSerializer_TestSerialization((ulong)uint.MaxValue + 1);
        this.MessagePackSerializer_TestSerialization(ulong.MaxValue - 1);
        this.MessagePackSerializer_TestSerialization(ulong.MaxValue);
    }

    [Fact]
    public void MessagePackSerializer_Float()
    {
        this.MessagePackSerializer_TestSerialization(0.0f);
        this.MessagePackSerializer_TestSerialization(1.0f);
        this.MessagePackSerializer_TestSerialization(-123.45f);
        this.MessagePackSerializer_TestSerialization(float.MaxValue);
        this.MessagePackSerializer_TestSerialization(float.MinValue);
        this.MessagePackSerializer_TestSerialization(float.PositiveInfinity);
        this.MessagePackSerializer_TestSerialization(float.NegativeInfinity);

        this.MessagePackSerializer_TestSerialization(0.0d);
        this.MessagePackSerializer_TestSerialization(3.1415926d);
        this.MessagePackSerializer_TestSerialization(-67.89f);
        this.MessagePackSerializer_TestSerialization(double.MaxValue);
        this.MessagePackSerializer_TestSerialization(double.MinValue);
        this.MessagePackSerializer_TestSerialization(double.PositiveInfinity);
        this.MessagePackSerializer_TestSerialization(double.NegativeInfinity);
    }

    [Fact]
    public void MessagePackSerializer_SerializeAsciiString()
    {
        this.MessagePackSerializer_TestASCIIStringSerialization(null);
        this.MessagePackSerializer_TestASCIIStringSerialization(string.Empty);
        this.MessagePackSerializer_TestASCIIStringSerialization("Hello world!");

        // fixstr stores a byte array whose length is upto 31 bytes
        this.MessagePackSerializer_TestASCIIStringSerialization("1234567890123456789012345678901");

        // str 8 stores a byte array whose length is upto (2^8)-1 bytes
        this.MessagePackSerializer_TestASCIIStringSerialization("12345678901234567890123456789012");
        this.MessagePackSerializer_TestASCIIStringSerialization(new string('A', byte.MaxValue));
        this.MessagePackSerializer_TestASCIIStringSerialization(new string('B', byte.MaxValue + 1));
        this.MessagePackSerializer_TestASCIIStringSerialization(new string('Z', (1 << 14) - 1));
        this.MessagePackSerializer_TestASCIIStringSerialization(new string('Z', 1 << 14));

        this.MessagePackSerializer_TestASCIIStringSerialization(new string('Z', (1 << 15) - 1));
        this.MessagePackSerializer_TestASCIIStringSerialization(new string('Z', 1 << 15));

        // Unicode special characters
        // SerializeAsciiString will encode non-ASCII characters with '?'
        Assert.Throws<EqualException>(() => this.MessagePackSerializer_TestASCIIStringSerialization("\u0418"));
    }

    [Fact]
    public void MessagePackSerializer_SerializeUnicodeString()
    {
        this.MessagePackSerializer_TestUnicodeStringSerialization(null);
        this.MessagePackSerializer_TestUnicodeStringSerialization(string.Empty);
        this.MessagePackSerializer_TestUnicodeStringSerialization("Hello world!");

        // fixstr stores a byte array whose length is upto 31 bytes
        this.MessagePackSerializer_TestUnicodeStringSerialization("1234567890123456789012345678901");

        // str 8 stores a byte array whose length is upto (2^8)-1 bytes
        this.MessagePackSerializer_TestUnicodeStringSerialization("12345678901234567890123456789012");
        this.MessagePackSerializer_TestUnicodeStringSerialization(new string('A', byte.MaxValue));
        this.MessagePackSerializer_TestUnicodeStringSerialization(new string('B', byte.MaxValue + 1));
        this.MessagePackSerializer_TestUnicodeStringSerialization(new string('Z', (1 << 14) - 1));
        this.MessagePackSerializer_TestUnicodeStringSerialization(new string('Z', 1 << 14));

        // ill-formed UTF-8 sequence
        // This is replaced by `U+FFFD REPLACEMENT CHARACTER` in the returned string instance constructed from the byte array
        // TODO: Update this test case once the serializer starts to throw exception for ill-formed UTF-8 sequence.
        Assert.Throws<EqualException>(() => this.MessagePackSerializer_TestUnicodeStringSerialization("\uD801\uD802"));

        // Unicode special characters
        this.MessagePackSerializer_TestUnicodeStringSerialization("\u0418");
        this.MessagePackSerializer_TestUnicodeStringSerialization(new string('\u0418', 31));
        this.MessagePackSerializer_TestUnicodeStringSerialization(new string('\u0418', 50));
        this.MessagePackSerializer_TestUnicodeStringSerialization(new string('\u0418', (1 << 8) - 1));
        this.MessagePackSerializer_TestUnicodeStringSerialization(new string('\u0418', 1 << 10));
        this.MessagePackSerializer_TestUnicodeStringSerialization(new string('\u0418', (1 << 14) - 1));
        this.MessagePackSerializer_TestUnicodeStringSerialization(new string('\u0418', 1 << 14));

        this.MessagePackSerializer_TestUnicodeStringSerialization(new string('\u0418', (1 << 15) - 1));
        this.MessagePackSerializer_TestUnicodeStringSerialization(new string('\u0418', 1 << 15));

        // Unicode regular and special characters
        this.MessagePackSerializer_TestUnicodeStringSerialization("\u0418TestString");
        this.MessagePackSerializer_TestUnicodeStringSerialization("TestString\u0418");
        this.MessagePackSerializer_TestUnicodeStringSerialization("Test\u0418String");
    }

    [Fact]
    public void MessagePackSerializer_Array()
    {
        this.MessagePackSerializer_TestSerialization((object[])null);
        this.MessagePackSerializer_TestSerialization(Array.Empty<object>());

        // This object array has a custom string which will be serialized as STR16
        var objectArrayWithString = new object[]
        {
            "foo",
            1,
            0.6180340f,
            3.14159265358979323846264d,
        };

        var buffer = new byte[64 * 1024];
        _ = MessagePackSerializer.Serialize(buffer, 0, objectArrayWithString);
        var objectArrayWithStringDeserialized = MessagePack.MessagePackSerializer.Deserialize<object[]>(buffer);
        Assert.Equal(objectArrayWithString.Length, objectArrayWithStringDeserialized.Length);
        Assert.Equal(objectArrayWithString[0], objectArrayWithStringDeserialized[0]);
        Assert.Equal(objectArrayWithString[1], Convert.ToInt32(objectArrayWithStringDeserialized[1]));
        Assert.Equal(objectArrayWithString[2], objectArrayWithStringDeserialized[2]);
        Assert.Equal(objectArrayWithString[3], objectArrayWithStringDeserialized[3]);
    }

    [Fact]
    public void MessagePackSerializer_Map()
    {
        this.MessagePackSerializer_TestSerialization((Dictionary<string, object>)null);
        this.MessagePackSerializer_TestSerialization(new Dictionary<string, object>());

        // This dictionary has custom strings which will be serialized as STR16
        var dictionaryWithStrings = new Dictionary<string, object>
        {
            ["foo"] = 1,
            ["bar"] = "baz",
            ["golden ratio"] = 0.6180340f,
            ["pi"] = 3.14159265358979323846264d,
        };
        var buffer = new byte[64 * 1024];
        _ = MessagePackSerializer.Serialize(buffer, 0, dictionaryWithStrings);
        var dictionaryWithStringsDeserialized = MessagePack.MessagePackSerializer.Deserialize<Dictionary<string, object>>(buffer);
        Assert.Equal(dictionaryWithStrings.Count, dictionaryWithStringsDeserialized.Count);
        Assert.Equal(dictionaryWithStrings["foo"], Convert.ToInt32(dictionaryWithStringsDeserialized["foo"], CultureInfo.InvariantCulture));
        Assert.Equal(dictionaryWithStrings["bar"], dictionaryWithStringsDeserialized["bar"]);
        Assert.Equal(dictionaryWithStrings["golden ratio"], dictionaryWithStringsDeserialized["golden ratio"]);
        Assert.Equal(dictionaryWithStrings["pi"], dictionaryWithStringsDeserialized["pi"]);
    }
}
