// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Buffers;

namespace OpenTelemetry.OpAmp.Client.Internal.Utils;

internal static class Varint64
{
    private const byte DataMask = 0b_0111_1111;                 // Lower 7 bits of a byte
    private const byte ContinuationBit = 0b_1000_0000;          // Signals more bytes follow
    private const int BitsPerByte = 7;                          // Varint encodes 7 bits per byte
    private const int MaxEncodedBytes = 10;                     // 64 bits / 7 bits per byte = max 10 bytes
    private const int MaxShift = BitsPerByte * MaxEncodedBytes; // 70 bits (safety limit)

    public static bool TryDecode(ReadOnlySequence<byte> sequence, out int bytesRead, out ulong result, out string errorMessage)
    {
        int shift = 0;
        result = 0;
        bytesRead = 0;
        errorMessage = string.Empty;

        foreach (var memory in sequence)
        {
            foreach (byte b in memory.Span)
            {
                ulong value = (ulong)(b & DataMask);
                result |= value << shift;
                bytesRead++;

                if ((b & ContinuationBit) == 0)
                {
                    return true;
                }

                shift += BitsPerByte;

                // 64 bits max + buffer
                if (shift >= MaxShift)
                {
                    errorMessage = "Varint is too long for 64-bit integer.";
                    return false;
                }
            }
        }

        errorMessage = "Incomplete varint data.";
        return false;
    }

    public static byte[] Encode(ulong value)
    {
        var bytes = new byte[MaxEncodedBytes];
        var index = 0;

        while (value > DataMask)
        {
            bytes[index++] = (byte)((value & DataMask) | ContinuationBit);
            value >>= BitsPerByte;
        }

        bytes[index++] = (byte)value; // Last byte without continuation bit

#if NET
        return bytes[..index];
#else
        var result = new byte[index];
        Array.Copy(bytes, result, index);
        return result;
#endif
    }
}
