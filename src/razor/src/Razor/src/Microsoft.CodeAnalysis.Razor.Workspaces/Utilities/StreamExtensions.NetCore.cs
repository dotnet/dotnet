// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

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

    public static RazorProjectInfoAction ReadProjectInfoAction(this Stream stream)
    {
        var action = stream.ReadByte();
        return action switch
        {
            0 => RazorProjectInfoAction.Update,
            1 => RazorProjectInfoAction.Remove,
            _ => throw Assumes.NotReachable()
        };
    }

    public static void WriteProjectInfoAction(this Stream stream, RazorProjectInfoAction projectInfoAction)
    {
        stream.WriteByte(projectInfoAction switch
        {
            RazorProjectInfoAction.Update => 0,
            RazorProjectInfoAction.Remove => 1,
            _ => throw Assumes.NotReachable()
        });
    }

    public static Task WriteProjectInfoRemovalAsync(this Stream stream, string intermediateOutputPath, CancellationToken cancellationToken)
    {
        stream.WriteProjectInfoAction(RazorProjectInfoAction.Remove);
        return stream.WriteStringAsync(intermediateOutputPath, encoding: null, cancellationToken);
    }

    public static Task<string> ReadProjectInfoRemovalAsync(this Stream stream, CancellationToken cancellationToken)
    {
        return stream.ReadStringAsync(encoding: null, cancellationToken);
    }

    public static async Task WriteProjectInfoAsync(this Stream stream, RazorProjectInfo projectInfo, CancellationToken cancellationToken)
    {
        var bytes = projectInfo.Serialize();
        stream.WriteSize(bytes.Length);
        await stream.WriteAsync(bytes, cancellationToken).ConfigureAwait(false);
    }

    public static async Task<RazorProjectInfo?> ReadProjectInfoAsync(this Stream stream, CancellationToken cancellationToken)
    {
        var sizeToRead = stream.ReadSize();

        using var _ = ArrayPool<byte>.Shared.GetPooledArray(sizeToRead, out var projectInfoBytes);
        await stream.ReadExactlyAsync(projectInfoBytes, 0, sizeToRead, cancellationToken).ConfigureAwait(false);

        // The array may be larger than the bytes read so make sure to trim accordingly.
        var projectInfoMemory = projectInfoBytes.AsMemory(0, sizeToRead);

        return RazorProjectInfo.DeserializeFrom(projectInfoMemory);
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
