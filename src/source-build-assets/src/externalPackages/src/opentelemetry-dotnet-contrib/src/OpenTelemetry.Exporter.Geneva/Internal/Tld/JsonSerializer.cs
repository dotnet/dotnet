// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace OpenTelemetry.Exporter.Geneva.Tld;

internal static class JsonSerializer
{
    private const byte ASCII_QUOTATION_MARK = 0x22;
    private const byte ASCII_REVERSE_SOLIDUS = 0x5C;
    private const byte ASCII_SOLIDUS = 0x2F;
    private const byte ASCII_BACKSPACE = 0x08;
    private const byte ASCII_FORMFEED = 0x0C;
    private const byte ASCII_LINEFEED = 0x0A;
    private const byte ASCII_CARRIAGE_RETURN = 0x0D;
    private const byte ASCII_HORIZONTAL_TAB = 0x09;

#if NET
    private const int MAX_STACK_ALLOC_SIZE_IN_BYTES = 256;
#endif

    private static readonly byte[] HEX_CODE = InitializeHexCodeLookup();
    private static readonly ThreadLocal<byte[]> ThreadLocalBuffer = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SerializeNull()
    {
        var buffer = ThreadLocalBuffer.Value;
        if (buffer == null)
        {
            buffer = new byte[65360];
            ThreadLocalBuffer.Value = buffer;
        }

        var count = WriteString(buffer, 0, "null");
        return Encoding.UTF8.GetString(buffer, 0, count);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SerializeNull(byte[] buffer, int cursor)
    {
        return WriteString(buffer, cursor, "null");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SerializeString(string? value)
    {
        var buffer = ThreadLocalBuffer.Value;
        if (buffer == null)
        {
            buffer = new byte[65360];
            ThreadLocalBuffer.Value = buffer;
        }

        var count = SerializeString(buffer, 0, value);
        return Encoding.UTF8.GetString(buffer, 0, count);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SerializeString(byte[] buffer, int cursor, string? value)
    {
        if (value == null)
        {
            return SerializeNull(buffer, cursor);
        }

        buffer[cursor++] = ASCII_QUOTATION_MARK;
        cursor = WriteString(buffer, cursor, value);
        buffer[cursor++] = ASCII_QUOTATION_MARK;
        return cursor;
    }

#if NET
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SerializeString(byte[] buffer, int cursor, ReadOnlySpan<char> value)
    {
        if (value.IsEmpty)
        {
            return SerializeNull(buffer, cursor);
        }

        buffer[cursor++] = ASCII_QUOTATION_MARK;
        cursor = WriteString(buffer, cursor, value);
        buffer[cursor++] = ASCII_QUOTATION_MARK;
        return cursor;
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SerializeArray<T>(T[]? array)
    {
        var buffer = ThreadLocalBuffer.Value;
        if (buffer == null)
        {
            buffer = new byte[65360];
            ThreadLocalBuffer.Value = buffer;
        }

        var count = SerializeArray(buffer, 0, array);
        return Encoding.UTF8.GetString(buffer, 0, count);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SerializeArray<T>(byte[] buffer, int cursor, T[]? array)
    {
        if (array == null)
        {
            return SerializeNull(buffer, cursor);
        }

        buffer[cursor++] = unchecked((byte)'[');
        var length = array.Length;
        if (length >= 1)
        {
            cursor = Serialize(buffer, cursor, array[0]);
            for (var i = 1; i < length; i++)
            {
                buffer[cursor++] = unchecked((byte)',');
                cursor = Serialize(buffer, cursor, array[i]);
            }
        }

        buffer[cursor++] = unchecked((byte)']');
        return cursor;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SerializeMap(IEnumerable<KeyValuePair<string, object?>>? map)
    {
        var buffer = ThreadLocalBuffer.Value;
        if (buffer == null)
        {
            buffer = new byte[65360];
            ThreadLocalBuffer.Value = buffer;
        }

        var count = SerializeMap(buffer, 0, map);
        return Encoding.UTF8.GetString(buffer, 0, count);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] SerializeKeyValuePairsListAsBytes(List<KeyValuePair<string, object?>>? listKVp, out int count)
    {
        var buffer = ThreadLocalBuffer.Value;
        if (buffer == null)
        {
            buffer = new byte[65360];
            ThreadLocalBuffer.Value = buffer;
        }

        count = SerializeKeyValuePairList(buffer, 0, listKVp);
        return buffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SerializeMap(byte[] buffer, int cursor, IEnumerable<KeyValuePair<string, object?>>? map)
    {
        if (map == null)
        {
            return SerializeNull(buffer, cursor);
        }

        buffer[cursor++] = unchecked((byte)'{');
        var count = 0;
        foreach (var entry in map)
        {
            if (count > 0)
            {
                buffer[cursor++] = unchecked((byte)',');
            }

            cursor = SerializeString(buffer, cursor, entry.Key);
            buffer[cursor++] = unchecked((byte)':');
            cursor = Serialize(buffer, cursor, entry.Value);
            count++;
        }

        buffer[cursor++] = unchecked((byte)'}');
        return cursor;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SerializeKeyValuePairList(byte[] buffer, int cursor, List<KeyValuePair<string, object?>>? listKvp)
    {
        if (listKvp == null)
        {
            return SerializeNull(buffer, cursor);
        }

        buffer[cursor++] = unchecked((byte)'{');
        var count = 0;
        for (var i = 0; i < listKvp.Count; i++)
        {
            if (count > 0)
            {
                buffer[cursor++] = unchecked((byte)',');
            }

            cursor = SerializeString(buffer, cursor, listKvp[i].Key);
            buffer[cursor++] = unchecked((byte)':');
            cursor = Serialize(buffer, cursor, listKvp[i].Value);
            count++;
        }

        buffer[cursor++] = unchecked((byte)'}');
        return cursor;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Serialize(object? obj)
    {
        var buffer = ThreadLocalBuffer.Value;
        if (buffer == null)
        {
            buffer = new byte[65360];
            ThreadLocalBuffer.Value = buffer;
        }

        var count = Serialize(buffer, 0, obj);
        return Encoding.UTF8.GetString(buffer, 0, count);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Serialize(byte[] buffer, int cursor, object? obj)
    {
        if (obj == null)
        {
            return SerializeNull(buffer, cursor);
        }

        switch (obj)
        {
            case bool v:
                return WriteString(buffer, cursor, v ? "true" : "false");
#if NET
            case byte:
            case sbyte:
            case short:
            case ushort:
            case int:
            case uint:
            case long:
            case ulong:
            case float:
            case double:
                Span<char> tmp = stackalloc char[MAX_STACK_ALLOC_SIZE_IN_BYTES / sizeof(char)];
                ((ISpanFormattable)obj).TryFormat(tmp, out var charsWritten, default, CultureInfo.InvariantCulture);
                return WriteString(buffer, cursor, tmp.Slice(0, charsWritten));
            case DateTime dt:
                tmp = stackalloc char[MAX_STACK_ALLOC_SIZE_IN_BYTES / sizeof(char)];
                dt = dt.ToUniversalTime();
                dt.TryFormat(tmp, out var count, default, CultureInfo.InvariantCulture);
                return WriteString(buffer, cursor, tmp.Slice(0, count));
#else
            case byte:
            case sbyte:
            case short:
            case ushort:
            case int:
            case uint:
            case long:
            case ulong:
            case float:
            case double:
                return WriteString(buffer, cursor, Convert.ToString(obj, CultureInfo.InvariantCulture));
            case DateTime dt:
                return WriteString(buffer, cursor, Convert.ToString(dt.ToUniversalTime(), CultureInfo.InvariantCulture));
#endif
            case bool[] vbarray:
                return SerializeArray(buffer, cursor, vbarray);
            case byte[] vui8array:
                return SerializeArray(buffer, cursor, vui8array);
            case sbyte[] vi8array:
                return SerializeArray(buffer, cursor, vi8array);
            case short[] vi16array:
                return SerializeArray(buffer, cursor, vi16array);
            case ushort[] vui16array:
                return SerializeArray(buffer, cursor, vui16array);
            case int[] vi32array:
                return SerializeArray(buffer, cursor, vi32array);
            case uint[] vui32array:
                return SerializeArray(buffer, cursor, vui32array);
            case long[] vi64array:
                return SerializeArray(buffer, cursor, vi64array);
            case ulong[] vui64array:
                return SerializeArray(buffer, cursor, vui64array);
            case float[] vfarray:
                return SerializeArray(buffer, cursor, vfarray);
            case double[] vdarray:
                return SerializeArray(buffer, cursor, vdarray);
            case string[] vsarray:
                return SerializeArray(buffer, cursor, vsarray);
            case DateTime[] vdtarray:
                return SerializeArray(buffer, cursor, vdtarray);
            case string v:
                return SerializeString(buffer, cursor, v);
            case IEnumerable<KeyValuePair<string, object?>> v:
                return SerializeMap(buffer, cursor, v);
            case object[] v:
                return SerializeArray(buffer, cursor, v);

#if NET
            case ISpanFormattable v:
                tmp = stackalloc char[MAX_STACK_ALLOC_SIZE_IN_BYTES / sizeof(char)];
                if (v.TryFormat(tmp, out charsWritten, default, CultureInfo.InvariantCulture))
                {
                    return SerializeString(buffer, cursor, tmp.Slice(0, charsWritten));
                }

                goto default;
#endif

            default:
                string? repr;
                try
                {
                    repr = Convert.ToString(obj, CultureInfo.InvariantCulture);
                }
                catch
                {
                    repr = $"ERROR: type {obj.GetType().FullName} is not supported";
                }

                return SerializeString(buffer, cursor, repr);
        }
    }

    private static byte[] InitializeHexCodeLookup()
    {
        var mapping = "0123456789abcdef"u8.ToArray();

        var hexCodeLookup = new byte[512];
        for (var i = 0; i < 256; i++)
        {
            hexCodeLookup[i] = mapping[i >> 4];
            hexCodeLookup[i + 256] = mapping[i & 0x0F];
        }

        return hexCodeLookup;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int WriteString(byte[] buffer, int cursor, string value)
    {
        for (var i = 0; i < value.Length; i++)
        {
            var ordinal = (ushort)value[i];
            switch (ordinal)
            {
                case ASCII_QUOTATION_MARK:
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    buffer[cursor++] = ASCII_QUOTATION_MARK;
                    break;
                case ASCII_REVERSE_SOLIDUS:
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    break;
                case ASCII_SOLIDUS:
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    buffer[cursor++] = ASCII_SOLIDUS;
                    break;
                case ASCII_BACKSPACE:
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    buffer[cursor++] = unchecked((byte)'b');
                    break;
                case ASCII_FORMFEED:
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    buffer[cursor++] = unchecked((byte)'f');
                    break;
                case ASCII_LINEFEED:
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    buffer[cursor++] = unchecked((byte)'n');
                    break;
                case ASCII_CARRIAGE_RETURN:
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    buffer[cursor++] = unchecked((byte)'r');
                    break;
                case ASCII_HORIZONTAL_TAB:
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    buffer[cursor++] = unchecked((byte)'t');
                    break;
                default:
                    // ASCII printable characters
                    if (ordinal is >= 32 and < 127)
                    {
                        buffer[cursor++] = unchecked((byte)ordinal);
                    }

                    // ASCII control characters, extended ASCII codes or UNICODE
                    else
                    {
                        buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                        var high = unchecked(ordinal >> 8);
                        var low = ordinal & 0xFF;
                        buffer[cursor++] = unchecked((byte)'u');
                        buffer[cursor++] = HEX_CODE[high];
                        buffer[cursor++] = HEX_CODE[high + 256];
                        buffer[cursor++] = HEX_CODE[low];
                        buffer[cursor++] = HEX_CODE[low + 256];
                    }

                    break;
            }
        }

        return cursor;
    }

#if NET
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int WriteString(byte[] buffer, int cursor, ReadOnlySpan<char> value)
    {
        for (var i = 0; i < value.Length; i++)
        {
            var ordinal = (ushort)value[i];
            switch (ordinal)
            {
                case ASCII_QUOTATION_MARK:
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    buffer[cursor++] = ASCII_QUOTATION_MARK;
                    break;
                case ASCII_REVERSE_SOLIDUS:
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    break;
                case ASCII_SOLIDUS:
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    buffer[cursor++] = ASCII_SOLIDUS;
                    break;
                case ASCII_BACKSPACE:
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    buffer[cursor++] = unchecked((byte)'b');
                    break;
                case ASCII_FORMFEED:
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    buffer[cursor++] = unchecked((byte)'f');
                    break;
                case ASCII_LINEFEED:
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    buffer[cursor++] = unchecked((byte)'n');
                    break;
                case ASCII_CARRIAGE_RETURN:
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    buffer[cursor++] = unchecked((byte)'r');
                    break;
                case ASCII_HORIZONTAL_TAB:
                    buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                    buffer[cursor++] = unchecked((byte)'t');
                    break;
                default:
                    // ASCII printable characters
                    if (ordinal is >= 32 and < 127)
                    {
                        buffer[cursor++] = unchecked((byte)ordinal);
                    }

                    // ASCII control characters, extended ASCII codes or UNICODE
                    else
                    {
                        buffer[cursor++] = ASCII_REVERSE_SOLIDUS;
                        var high = unchecked(ordinal >> 8);
                        var low = ordinal & 0xFF;
                        buffer[cursor++] = unchecked((byte)'u');
                        buffer[cursor++] = HEX_CODE[high];
                        buffer[cursor++] = HEX_CODE[high + 256];
                        buffer[cursor++] = HEX_CODE[low];
                        buffer[cursor++] = HEX_CODE[low + 256];
                    }

                    break;
            }
        }

        return cursor;
    }
#endif
}
