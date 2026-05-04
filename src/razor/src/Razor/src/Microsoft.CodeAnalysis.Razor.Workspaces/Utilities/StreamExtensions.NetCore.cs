// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.Razor.Utilities;

internal static class StreamExtensions
{
    public static Task WriteStringAsync(this Stream stream, string text, Encoding? encoding, CancellationToken cancellationToken)
    {
        Debug.Assert(stream.CanWrite);
        encoding ??= Encoding.UTF8;

        var byteCount = encoding.GetMaxByteCount(text.Length);
        using var _ = ArrayPool<byte>.Shared.GetPooledArray(byteCount, out var byteArray);

        var usedBytes = encoding.GetBytes(text, byteArray);

        stream.WriteSize(usedBytes);
        return stream.WriteAsync(byteArray, 0, usedBytes, cancellationToken);
    }

    public static async Task<string> ReadStringAsync(this Stream stream, Encoding? encoding, CancellationToken cancellationToken)
    {
        Debug.Assert(stream.CanRead);
        encoding ??= Encoding.UTF8;

        var length = stream.ReadSize();

        using var _ = ArrayPool<byte>.Shared.GetPooledArray(length, out var encodedBytes);

        await stream.ReadExactlyAsync(encodedBytes, 0, length, cancellationToken).ConfigureAwait(false);
        return encoding.GetString(encodedBytes, 0, length);
    }

    public static void WriteSize(this Stream stream, int length)
    {
        Span<byte> sizeBytes = stackalloc byte[4];
        BitConverter.TryWriteBytes(sizeBytes, length);
        stream.Write(sizeBytes);
    }

    public unsafe static int ReadSize(this Stream stream)
    {
        Span<byte> bytes = stackalloc byte[4];
        stream.ReadExactly(bytes);
        return BitConverter.ToInt32(bytes);
    }
}
