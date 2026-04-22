// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenTelemetry.Exporter.Geneva;

#pragma warning disable SA1649 // File name should match first type name

internal enum MetricEventType
{
    ULongMetric = 50,
    DoubleMetric = 55,
    ExternallyAggregatedULongDistributionMetric = 56,
    TLV = 70,
}

internal enum PayloadType
{
    AccountName = 1,
    Namespace = 2,
    MetricName = 3,
    Dimensions = 4,
    ULongMetric = 5,
    DoubleMetric = 6,
    ExternallyAggregatedULongDistributionMetric = 8,
    HistogramULongValueCountPairs = 12,
    Exemplars = 15,
}

[Flags]
internal enum ExemplarFlags : byte
{
    None = 0x0,
    IsMetricValueDoubleStoredAsLong = 0x1,
    IsTimestampAvailable = 0x2,
    SpanIdExists = 0x4,
    TraceIdExists = 0x8,
    SampleCountExists = 0x10,
}

/// <summary>
/// Represents the binary header for non-ETW transmitted metrics.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
internal struct BinaryHeader
{
    /// <summary>
    /// The event ID that represents how it will be processed.
    /// </summary>
    [FieldOffset(0)]
    public ushort EventId;

    /// <summary>
    /// The length of the body following the header.
    /// </summary>
    [FieldOffset(2)]
    public ushort BodyLength;
}

/// <summary>
/// Represents the fixed payload of a standard metric.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
internal struct MetricPayload
{
    /// <summary>
    /// The dimension count.
    /// </summary>
    [FieldOffset(0)]
    public ushort CountDimension;

    /// <summary>
    /// Reserved for alignment.
    /// </summary>
    [FieldOffset(2)]
    public ushort ReservedWord; // for 8-byte aligned

    /// <summary>
    /// Reserved for alignment.
    /// </summary>
    [FieldOffset(4)]
    public uint ReservedDword;

    /// <summary>
    /// The UTC timestamp of the metric.
    /// </summary>
    [FieldOffset(8)]
    public ulong TimestampUtc;

    /// <summary>
    /// The value of the metric.
    /// </summary>
    [FieldOffset(16)]
    public MetricData Data;
}

/// <summary>
/// Represents the fixed payload of an externally aggregated metric.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
internal struct ExternalPayload
{
    /// <summary>
    /// The dimension count.
    /// </summary>
    [FieldOffset(0)]
    public ushort CountDimension;

    /// <summary>
    /// Reserved for alignment.
    /// </summary>
    [FieldOffset(2)]
    public ushort ReservedWord; // for alignment

    /// <summary>
    /// The number of samples produced in the period.
    /// </summary>
    [FieldOffset(4)]
    public uint Count;

    /// <summary>
    /// The UTC timestamp of the metric.
    /// </summary>
    [FieldOffset(8)]
    public ulong TimestampUtc;

    /// <summary>
    /// The sum of the samples produced in the period.
    /// </summary>
    [FieldOffset(16)]
    public MetricData Sum;

    /// <summary>
    /// The minimum value of the samples produced in the period.
    /// </summary>
    [FieldOffset(24)]
    public MetricData Min;

    /// <summary>
    /// The maximum value of the samples produced in the period.
    /// </summary>
    [FieldOffset(32)]
    public MetricData Max;
}

/// <summary>
/// Represents the value of a metric.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
internal struct MetricData
{
    /// <summary>
    /// The value represented as an integer.
    /// </summary>
    [FieldOffset(0)]
    public ulong UInt64Value;

    /// <summary>
    /// The value represented as a double.
    /// </summary>
    [FieldOffset(0)]
    public double DoubleValue;
}

internal static class MetricSerializer
{
    /// <summary>
    /// Writes the string to buffer.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <param name="bufferIndex">Index of the buffer.</param>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeString(byte[] buffer, ref int bufferIndex, string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            if (bufferIndex + value!.Length + sizeof(short) >= buffer.Length)
            {
                // TODO: What should we do when the data is invalid?
            }

#if NETSTANDARD2_1_OR_GREATER
            Span<byte> bufferSpan = new Span<byte>(buffer);
            bufferSpan = bufferSpan.Slice(bufferIndex);
            Span<byte> stringSpan = bufferSpan.Slice(2);
            var lengthWritten = (short)Encoding.UTF8.GetBytes(value, stringSpan);
            MemoryMarshal.Write(bufferSpan, ref lengthWritten);
            bufferIndex += sizeof(short) + lengthWritten;
#else
            // Advanced the buffer to account for the length, we will write it back after encoding.
            var currentIndex = bufferIndex;
            bufferIndex += sizeof(short);
            var lengthWritten = Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, bufferIndex);
            bufferIndex += lengthWritten;

            // Write the length now that it is known
            SerializeInt16(buffer, ref currentIndex, (short)lengthWritten);
#endif
        }
        else
        {
            SerializeInt16(buffer, ref bufferIndex, 0);
        }
    }

    /// <summary>
    /// Writes the encoded string to buffer.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <param name="bufferIndex">Index of the buffer.</param>
    /// <param name="encodedValue">The encoded value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeEncodedString(byte[] buffer, ref int bufferIndex, byte[] encodedValue)
    {
        if (bufferIndex + encodedValue.Length + sizeof(short) >= buffer.Length)
        {
            // TODO: What should we do when the data is invalid?
        }

#if NETSTANDARD2_1_OR_GREATER
        Span<byte> sourceSpan = new Span<byte>(encodedValue);
        Span<byte> bufferSpan = new Span<byte>(buffer);
        bufferSpan = bufferSpan.Slice(bufferIndex);
        sourceSpan.CopyTo(bufferSpan.Slice(2));
        short encodedLength = (short)encodedValue.Length;
        MemoryMarshal.Write(bufferSpan, ref encodedLength);
        bufferIndex += sizeof(short) + encodedLength;
#else
        SerializeInt16(buffer, ref bufferIndex, (short)encodedValue.Length);
        Array.Copy(encodedValue, 0, buffer, bufferIndex, encodedValue.Length);
        bufferIndex += encodedValue.Length;
#endif
    }

    /// <summary>
    /// Writes the byte to buffer.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <param name="bufferIndex">Index of the buffer.</param>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeByte(byte[] buffer, ref int bufferIndex, byte value)
    {
        if (bufferIndex + sizeof(byte) >= buffer.Length)
        {
            // TODO: What should we do when the data is invalid?
        }

        buffer[bufferIndex] = value;
        bufferIndex += sizeof(byte);
    }

    /// <summary>
    /// Writes the unsigned short to buffer.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <param name="bufferIndex">Index of the buffer.</param>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeUInt16(byte[] buffer, ref int bufferIndex, ushort value)
    {
        if (bufferIndex + sizeof(ushort) >= buffer.Length)
        {
            // TODO: What should we do when the data is invalid?
        }

        buffer[bufferIndex] = (byte)value;
        buffer[bufferIndex + 1] = (byte)(value >> 8);
        bufferIndex += sizeof(ushort);
    }

    /// <summary>
    /// Writes the short to buffer.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <param name="bufferIndex">Index of the buffer.</param>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeInt16(byte[] buffer, ref int bufferIndex, short value)
    {
        if (bufferIndex + sizeof(short) >= buffer.Length)
        {
            // TODO: What should we do when the data is invalid?
        }

        buffer[bufferIndex] = (byte)value;
        buffer[bufferIndex + 1] = (byte)(value >> 8);
        bufferIndex += sizeof(short);
    }

    /// <summary>
    /// Writes the unsigned int to buffer.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <param name="bufferIndex">Index of the buffer.</param>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeUInt32(byte[] buffer, ref int bufferIndex, uint value)
    {
        if (bufferIndex + sizeof(uint) >= buffer.Length)
        {
            // TODO: What should we do when the data is invalid?
        }

        buffer[bufferIndex] = (byte)value;
        buffer[bufferIndex + 1] = (byte)(value >> 8);
        buffer[bufferIndex + 2] = (byte)(value >> 0x10);
        buffer[bufferIndex + 3] = (byte)(value >> 0x18);
        bufferIndex += sizeof(uint);
    }

    /// <summary>
    /// Writes the ulong to buffer.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <param name="bufferIndex">Index of the buffer.</param>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeUInt64(byte[] buffer, ref int bufferIndex, ulong value)
    {
        if (bufferIndex + sizeof(ulong) >= buffer.Length)
        {
            // TODO: What should we do when the data is invalid?
        }

        buffer[bufferIndex] = (byte)value;
        buffer[bufferIndex + 1] = (byte)(value >> 8);
        buffer[bufferIndex + 2] = (byte)(value >> 0x10);
        buffer[bufferIndex + 3] = (byte)(value >> 0x18);
        buffer[bufferIndex + 4] = (byte)(value >> 0x20);
        buffer[bufferIndex + 5] = (byte)(value >> 0x28);
        buffer[bufferIndex + 6] = (byte)(value >> 0x30);
        buffer[bufferIndex + 7] = (byte)(value >> 0x38);
        bufferIndex += sizeof(ulong);
    }

    /// <summary>
    /// Writes the long to buffer.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <param name="bufferIndex">Index of the buffer.</param>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeInt64(byte[] buffer, ref int bufferIndex, long value)
    {
        if (bufferIndex + sizeof(long) >= buffer.Length)
        {
        }

        buffer[bufferIndex] = (byte)value;
        buffer[bufferIndex + 1] = (byte)(value >> 8);
        buffer[bufferIndex + 2] = (byte)(value >> 0x10);
        buffer[bufferIndex + 3] = (byte)(value >> 0x18);
        buffer[bufferIndex + 4] = (byte)(value >> 0x20);
        buffer[bufferIndex + 5] = (byte)(value >> 0x28);
        buffer[bufferIndex + 6] = (byte)(value >> 0x30);
        buffer[bufferIndex + 7] = (byte)(value >> 0x38);
        bufferIndex += sizeof(long);
    }

    /// <summary>
    /// Writes the double to buffer.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <param name="bufferIndex">Index of the buffer.</param>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SerializeFloat64(byte[] buffer, ref int bufferIndex, double value)
    {
        if (bufferIndex + sizeof(double) >= buffer.Length)
        {
            // TODO: What should we do when the data is invalid?
        }

        fixed (byte* bp = buffer)
        {
            *(double*)(bp + bufferIndex) = value;
        }

        bufferIndex += sizeof(double);
    }

    /// <summary>
    /// Writes the base128 string to buffer.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <param name="bufferIndex">Index of the buffer.</param>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeBase128String(byte[] buffer, ref int bufferIndex, string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            if (bufferIndex + value!.Length + sizeof(short) >= buffer.Length)
            {
                // TODO: What should we do when the data is invalid?
            }

            var encodedValue = Encoding.UTF8.GetBytes(value);
            SerializeUInt64AsBase128(buffer, ref bufferIndex, (ulong)encodedValue.Length);
            Array.Copy(encodedValue, 0, buffer, bufferIndex, encodedValue.Length);
            bufferIndex += encodedValue.Length;
        }
        else
        {
            SerializeInt16(buffer, ref bufferIndex, 0);
        }
    }

    /// <summary>
    /// Writes unsigned int value Base-128 encoded.
    /// </summary>
    /// <param name="buffer">Buffer used for writing.</param>
    /// <param name="offset">Offset to start with. Will be moved to the next byte after written.</param>
    /// <param name="value">Value to write.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeUInt32AsBase128(byte[] buffer, ref int offset, uint value)
    {
        SerializeUInt64AsBase128(buffer, ref offset, value);
    }

    /// <summary>
    /// Writes ulong value Base-128 encoded to the buffer starting from the specified offset.
    /// </summary>
    /// <param name="buffer">Buffer used for writing.</param>
    /// <param name="offset">Offset to start with. Will be moved to the next byte after written.</param>
    /// <param name="value">Value to write.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeUInt64AsBase128(byte[] buffer, ref int offset, ulong value)
    {
        var t = value;
        do
        {
            var b = (byte)(t & 0x7f);
            t >>= 7;
            if (t > 0)
            {
                b |= 0x80;
            }

            buffer[offset++] = b;
        }
        while (t > 0);
    }

    /// <summary>
    /// Writes int value Base-128 encoded.
    /// </summary>
    /// <param name="buffer">Buffer used for writing.</param>
    /// <param name="offset">Offset to start with. Will be moved to the next byte after written.</param>
    /// <param name="value">Value to write.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeInt32AsBase128(byte[] buffer, ref int offset, int value)
    {
        SerializeInt64AsBase128(buffer, ref offset, value);
    }

    /// <summary>
    /// Writes long value Base-128 encoded.
    /// </summary>
    /// <param name="buffer">Buffer used for writing.</param>
    /// <param name="offset">Offset to start with. Will be moved to the next byte after written.</param>
    /// <param name="value">Value to write.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeInt64AsBase128(byte[] buffer, ref int offset, long value)
    {
        var negative = value < 0;
        var t = negative ? -value : value;
        var first = true;
        do
        {
            byte b;
            if (first)
            {
                b = (byte)(t & 0x3f);
                t >>= 6;
                if (negative)
                {
                    b = (byte)(b | 0x40);
                }

                first = false;
            }
            else
            {
                b = (byte)(t & 0x7f);
                t >>= 7;
            }

            if (t > 0)
            {
                b |= 0x80;
            }

            buffer[offset++] = b;
        }
        while (t > 0);
    }

    /// <summary>
    /// Writes the encoded string to buffer.
    /// </summary>
    /// <param name="buffer">The buffer to write data into.</param>
    /// <param name="bufferIndex">Index of the buffer.</param>
    /// <param name="data">Source data.</param>
    /// <param name="dataLength"> Number of bytes to copy.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeSpanOfBytes(byte[] buffer, ref int bufferIndex, Span<byte> data, int dataLength)
    {
        if (bufferIndex + dataLength + sizeof(short) >= buffer.Length)
        {
        }

        ReadOnlySpan<byte> source = data.Slice(0, dataLength);
        var target = new Span<byte>(buffer, bufferIndex, dataLength);

        source.CopyTo(target);
        bufferIndex += dataLength;
    }
}
