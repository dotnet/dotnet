// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Razor;

internal static class EnumExtensions
{
    // Note: This is written to allow the JIT to inline only the correct branch depending on the size of T.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void SetFlag<T>(ref this T value, T flag)
        where T : unmanaged, Enum
    {
        var v = (T*)Unsafe.AsPointer(ref value);

        if (sizeof(T) == sizeof(byte))
        {
            *(byte*)v |= *(byte*)&flag;
            return;
        }
        else if (sizeof(T) == sizeof(ushort))
        {
            *(ushort*)v |= *(ushort*)&flag;
            return;
        }
        else if (sizeof(T) == sizeof(uint))
        {
            *(uint*)v |= *(uint*)&flag;
            return;
        }
        else if (sizeof(T) == sizeof(ulong))
        {
            *(ulong*)v |= *(ulong*)&flag;
            return;
        }

        Debug.Fail("Unexpected enum underlying type.");
    }

    // Note: This is written to allow the JIT to inline only the correct branch depending on the size of T.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void ClearFlag<T>(ref this T value, T flag)
        where T : unmanaged, Enum
    {
        var v = (T*)Unsafe.AsPointer(ref value);

        if (sizeof(T) == sizeof(byte))
        {
            *(byte*)v &= (byte)~*(byte*)&flag;
            return;
        }
        else if (sizeof(T) == sizeof(ushort))
        {
            *(ushort*)v &= (ushort)~*(ushort*)&flag;
            return;
        }
        else if (sizeof(T) == sizeof(uint))
        {
            *(uint*)v &= ~*(uint*)&flag;
            return;
        }
        else if (sizeof(T) == sizeof(ulong))
        {
            *(ulong*)v &= ~*(ulong*)&flag;
            return;
        }

        Debug.Fail("Unexpected enum underlying type.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void UpdateFlag<T>(ref this T value, T flag, bool set)
        where T : unmanaged, Enum
    {
        if (set)
        {
            value.SetFlag(flag);
        }
        else
        {
            value.ClearFlag(flag);
        }
    }

    // Note: This is written to allow the JIT to inline only the correct branch depending on the size of T.
    // This is somewhat faster than Enum.HasFlag(...) when running on .NET Framework.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe bool IsFlagSet<T>(this T value, T flags)
        where T : unmanaged, Enum
    {
        if (sizeof(T) == sizeof(byte))
        {
            var f = *(byte*)&flags;
            return (*(byte*)&value & f) == f;
        }
        else if (sizeof(T) == sizeof(ushort))
        {
            var f = *(ushort*)&flags;
            return (*(ushort*)&value & f) == f;
        }
        else if (sizeof(T) == sizeof(uint))
        {
            var f = *(uint*)&flags;
            return (*(uint*)&value & f) == f;
        }
        else if (sizeof(T) == sizeof(ulong))
        {
            var f = *(ulong*)&flags;
            return (*(ulong*)&value & f) == f;
        }

        Debug.Fail("Unexpected enum underlying type.");
        return false;
    }

    // Note: This is written to allow the JIT to inline only the correct branch depending on the size of T.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe bool IsFlagClear<T>(this T value, T flags)
        where T : unmanaged, Enum
    {
        if (sizeof(T) == sizeof(byte))
        {
            var f = *(byte*)&flags;
            return (*(byte*)&value & f) == 0;
        }
        else if (sizeof(T) == sizeof(ushort))
        {
            var f = *(ushort*)&flags;
            return (*(ushort*)&value & f) == 0;
        }
        else if (sizeof(T) == sizeof(uint))
        {
            var f = *(uint*)&flags;
            return (*(uint*)&value & f) == 0;
        }
        else if (sizeof(T) == sizeof(ulong))
        {
            var f = *(ulong*)&flags;
            return (*(ulong*)&value & f) == 0;
        }

        Debug.Fail("Unexpected enum underlying type.");
        return false;
    }
}
