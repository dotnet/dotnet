// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NET8_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

#if !NET9_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace System;

internal static class SpanExtensions
{
#if !NET8_0_OR_GREATER
    public static unsafe void Replace(this ReadOnlySpan<char> source, Span<char> destination, char oldValue, char newValue)
    {
        var length = source.Length;
        if (length == 0)
        {
            return;
        }

        if (length > destination.Length)
        {
            throw new ArgumentException(SR.Destination_is_too_short, nameof(destination));
        }

        ref var src = ref MemoryMarshal.GetReference(source);
        ref var dst = ref MemoryMarshal.GetReference(destination);

        for (var i = 0; i < length; i++)
        {
            var original = Unsafe.Add(ref src, i);
            Unsafe.Add(ref dst, i) = original == oldValue ? newValue : original;
        }
    }

    public static unsafe void Replace(this Span<char> span, char oldValue, char newValue)
    {
        var length = span.Length;
        if (length == 0)
        {
            return;
        }

        ref var src = ref MemoryMarshal.GetReference(span);

        for (var i = 0; i < length; i++)
        {
            ref var slot = ref Unsafe.Add(ref src, i);

            if (slot == oldValue)
            {
                slot = newValue;
            }
        }
    }
#endif

#if !NET9_0_OR_GREATER
    /// <summary>
    /// Determines whether the specified value appears at the start of the span.
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="value">The value to compare.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool StartsWith<T>(this ReadOnlySpan<T> span, T value)
        where T : IEquatable<T>? =>
        span.Length != 0 && (span[0]?.Equals(value) ?? (object?)value is null);

    /// <summary>
    /// Determines whether the specified value appears at the end of the span.
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="value">The value to compare.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EndsWith<T>(this ReadOnlySpan<T> span, T value)
        where T : IEquatable<T>? =>
        span.Length != 0 && (span[^1]?.Equals(value) ?? (object?)value is null);
#endif
}
