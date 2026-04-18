#nullable enable

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.LinuxTracepoints.Provider;

using System;
using System.Runtime.InteropServices;
using Interlocked = System.Threading.Interlocked;
using BinaryPrimitives = System.Buffers.Binary.BinaryPrimitives;

internal static class Utility
{
    /// <summary>
    /// Atomically: old = location; if (old != null) { return old; } else { location = value; return value; }
    /// </summary>
    public static T InterlockedInitSingleton<T>(ref T? location, T value)
        where T : class
    {
        return Interlocked.CompareExchange(ref location, value, null) ?? value;
    }

    public static void WriteGuidBigEndian(Span<byte> destination, Guid value)
    {
        if (BitConverter.IsLittleEndian)
        {
            unsafe
            {
                var p = (byte*)&value;
                var p0 = (uint*)p;
                var p1 = (ushort*)(p + 4);
                var p2 = (ushort*)(p + 6);
                *p0 = BinaryPrimitives.ReverseEndianness(*p0);
                *p1 = BinaryPrimitives.ReverseEndianness(*p1);
                *p2 = BinaryPrimitives.ReverseEndianness(*p2);
            }
        }
        MemoryMarshal.Write(destination, ref value);
    }
}
