// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;

#if !NET
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#endif

namespace Microsoft.AspNetCore.Razor.Utilities;

internal static class StreamExtensions
{
    public static Task WriteStringAsync(this Stream stream, string text, Encoding? encoding, CancellationToken cancellationToken)
    {
        Debug.Assert(stream.CanWrite);
        encoding ??= Encoding.UTF8;

        var byteCount = encoding.GetMaxByteCount(text.Length);
        using var _ = ArrayPool<byte>.Shared.GetPooledArray(byteCount, out var byteArray);

#if NET
        var usedBytes = encoding.GetBytes(text, byteArray);
#else
        var usedBytes = GetBytes(text, encoding, byteArray);
#endif

        WriteSize(stream, usedBytes);
        return stream.WriteAsync(byteArray, 0, usedBytes, cancellationToken);

#if !NET
        static unsafe int GetBytes(string text, Encoding encoding, byte[] byteArray)
        {
            fixed (char* c = text)
            fixed (byte* b = byteArray)
            {
                return encoding.GetBytes(c, text.Length, b, byteArray.Length);
            }
        }
#endif
    }

    public static async Task<string> ReadStringAsync(this Stream stream, Encoding? encoding, CancellationToken cancellationToken)
    {
        Debug.Assert(stream.CanRead);
        encoding ??= Encoding.UTF8;

        var length = ReadSize(stream);

        using var _ = ArrayPool<byte>.Shared.GetPooledArray(length, out var encodedBytes);

        await ReadExactlyAsync(stream, encodedBytes, length, cancellationToken).ConfigureAwait(false);
        return encoding.GetString(encodedBytes, 0, length);
    }

    public static ProjectInfoAction ReadProjectInfoAction(this Stream stream)
    {
        var action = stream.ReadByte();
        return action switch
        {
            0 => ProjectInfoAction.Update,
            1 => ProjectInfoAction.Remove,
            _ => throw Assumes.NotReachable()
        };
    }

    public static void WriteProjectInfoAction(this Stream stream, ProjectInfoAction projectInfoAction)
    {
        stream.WriteByte(projectInfoAction switch
        {
            ProjectInfoAction.Update => 0,
            ProjectInfoAction.Remove => 1,
            _ => throw Assumes.NotReachable()
        });
    }

    public static Task WriteProjectInfoRemovalAsync(this Stream stream, string intermediateOutputPath, CancellationToken cancellationToken)
    {
        WriteProjectInfoAction(stream, ProjectInfoAction.Remove);
        return stream.WriteStringAsync(intermediateOutputPath, encoding: null, cancellationToken);
    }

    public static Task<string> ReadProjectInfoRemovalAsync(this Stream stream, CancellationToken cancellationToken)
    {
        return stream.ReadStringAsync(encoding: null, cancellationToken);
    }

    public static async Task WriteProjectInfoAsync(this Stream stream, RazorProjectInfo projectInfo, CancellationToken cancellationToken)
    {
        var bytes = projectInfo.Serialize();
        WriteSize(stream, bytes.Length);
        await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
    }

    public static async Task<RazorProjectInfo?> ReadProjectInfoAsync(this Stream stream, CancellationToken cancellationToken)
    {
        var sizeToRead = ReadSize(stream);

        using var _ = ArrayPool<byte>.Shared.GetPooledArray(sizeToRead, out var projectInfoBytes);
        await ReadExactlyAsync(stream, projectInfoBytes, sizeToRead, cancellationToken).ConfigureAwait(false);

        // The array may be larger than the bytes read so make sure to trim accordingly.
        var projectInfoMemory = projectInfoBytes.AsMemory(0, sizeToRead);

        return RazorProjectInfo.DeserializeFrom(projectInfoMemory);
    }

    public static void WriteSize(this Stream stream, int length)
    {
#if NET
        Span<byte> sizeBytes = stackalloc byte[4];
        BitConverter.TryWriteBytes(sizeBytes, length);
        stream.Write(sizeBytes);
#else
        using var _ = ArrayPool<byte>.Shared.GetPooledArray(4, out var sizeBytes);
        // Pulled from https://github.com/dotnet/runtime/blob/4b9a1b2d956f4a10a28b8f5f3f725e76eb6fb826/src/libraries/System.Private.CoreLib/src/System/BitConverter.cs#L158C13-L158C87
        Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(sizeBytes.AsSpan()), length);
        stream.Write(sizeBytes, 0, 4);
#endif

    }

    public unsafe static int ReadSize(this Stream stream)
    {
#if NET
        Span<byte> bytes = stackalloc byte[4];
        stream.Read(bytes);
        return BitConverter.ToInt32(bytes);
#else
        using var _  = ArrayPool<byte>.Shared.GetPooledArray(4, out var bytes);
        stream.Read(bytes, 0, 4);
        return BitConverter.ToInt32(bytes, 0);
#endif
    }

#if NET
    private static ValueTask ReadExactlyAsync(Stream stream, byte[] buffer, int length, CancellationToken cancellationToken)
        => stream.ReadExactlyAsync(buffer, 0, length, cancellationToken);
#else
    private static async ValueTask ReadExactlyAsync(Stream stream, byte[] buffer, int length, CancellationToken cancellationToken)
    {
        var bytesRead = 0;
        while (bytesRead < length)
        {
            var read = await stream.ReadAsync(buffer, bytesRead, length - bytesRead, cancellationToken);
            if (read == 0)
            {
                throw new EndOfStreamException();
            }

            bytesRead += read;
        }
    }
#endif
}
