// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#if !NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#endif

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
}
