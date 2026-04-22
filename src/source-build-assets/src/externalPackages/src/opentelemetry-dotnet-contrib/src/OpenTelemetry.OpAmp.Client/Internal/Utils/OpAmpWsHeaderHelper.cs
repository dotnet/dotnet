// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Buffers;
using System.Diagnostics;

namespace OpenTelemetry.OpAmp.Client.Internal.Utils;

internal static class OpAmpWsHeaderHelper
{
    public const int OpAmpProtocolHeader = 0x00;
    public const int MaxHeaderLength = 10; // Maximum length for varint64 encoding

    private static readonly byte[] EncodedHeader = Varint64.Encode(OpAmpProtocolHeader);

    public static int WriteHeader(ArraySegment<byte> buffer)
    {
        Debug.Assert(buffer.Count >= EncodedHeader.Length, $"Ensure {EncodedHeader.Length} bytes for the buffer.");

        Buffer.BlockCopy(EncodedHeader, 0, buffer.Array!, buffer.Offset, EncodedHeader.Length);

        return EncodedHeader.Length;
    }

    public static bool TryVerifyHeader(ReadOnlySequence<byte> sequence, out int headerSize, out string errorMessage)
    {
        var result = Varint64.TryDecode(sequence, out headerSize, out ulong header, out errorMessage);
        if (!result)
        {
            return false;
        }

        if (header != OpAmpProtocolHeader)
        {
            errorMessage = $"Invalid OpAmp WebSocket header: {header}.";
            return false;
        }

        return true;
    }
}
