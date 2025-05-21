// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#if !NET8_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

using System.Runtime.CompilerServices;

namespace System;

internal static class SpanExtensions
{
    public static unsafe void Replace(this ReadOnlySpan<char> source, Span<char> destination, char oldValue, char newValue)
    {
#if NET8_0_OR_GREATER
        source.Replace<char>(destination, oldValue, newValue);
#else
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
#endif
    }

    public static unsafe void Replace(this Span<char> span, char oldValue, char newValue)
    {
#if NET8_0_OR_GREATER
        span.Replace<char>(oldValue, newValue);
#else
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
#endif
    }

    /// <summary>
    /// Determines whether the specified value appears at the start of the span.
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="value">The value to compare.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool StartsWith<T>(this ReadOnlySpan<T> span, T value)
        where T : IEquatable<T>? =>
#if NET9_0_OR_GREATER
        MemoryExtensions.StartsWith(span, value);
#else
        span.Length != 0 && (span[0]?.Equals(value) ?? (object?)value is null);
#endif

    /// <summary>
    /// Determines whether the specified value appears at the end of the span.
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="value">The value to compare.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EndsWith<T>(this ReadOnlySpan<T> span, T value)
        where T : IEquatable<T>? =>
#if NET9_0_OR_GREATER
        MemoryExtensions.EndsWith(span, value);
#else
        span.Length != 0 && (span[^1]?.Equals(value) ?? (object?)value is null);
#endif
}
