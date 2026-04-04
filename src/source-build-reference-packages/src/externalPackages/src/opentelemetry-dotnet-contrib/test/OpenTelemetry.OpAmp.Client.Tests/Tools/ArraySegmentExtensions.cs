// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Buffers;
using OpenTelemetry.Internal;

namespace OpenTelemetry.OpAmp.Client.Tests.Tools;

internal static class ArraySegmentExtensions
{
#if NETFRAMEWORK
    // A polyfill for ArraySegment.Slice for .NET Framework
    public static ArraySegment<T> Slice<T>(this ArraySegment<T> segment, int index, int count)
    {
        Guard.ThrowIfNull(segment.Array);

        if ((uint)index > (uint)segment.Count || (uint)count > (uint)(segment.Count - index))
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return new ArraySegment<T>(segment.Array, segment.Offset + index, count);
    }
#endif

    public static ReadOnlySequence<T> ToSequence<T>(this ArraySegment<T> segment)
        where T : struct
    {
        Guard.ThrowIfNull(segment.Array);

        return new ReadOnlySequence<T>(segment.Array, segment.Offset, segment.Count);
    }
}
