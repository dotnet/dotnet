// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
#if !NET8_0_OR_GREATER
using System.Collections.Generic;
#endif
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Razor;

internal static class ArgHelper
{
    public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(argument, paramName);
#else
        if (argument is null)
        {
            ThrowArgumentNullException(paramName);
        }
#endif
    }

    public static unsafe void ThrowIfNull([NotNull] void* argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(argument, paramName);
#else
        if (argument is null)
        {
            ThrowArgumentNullException(paramName);
        }
#endif
    }

#if !NET8_0_OR_GREATER
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowArgumentNullException(string? paramName)
    {
        throw new ArgumentNullException(paramName);
    }
#endif

    public static void ThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrEmpty(argument, paramName);
#else
        if (argument.IsNullOrEmpty())
        {
            ThrowException(argument, paramName);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ThrowException(string? argument, string? paramName)
        {
            ThrowIfNull(argument, paramName);
            throw new ArgumentException(SR.The_value_cannot_be_an_empty_string, paramName);
        }
#endif
    }

    public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(argument, paramName);
#else

        if (argument.IsNullOrWhiteSpace())
        {
            ThrowException(argument, paramName);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ThrowException(string? argument, string? paramName)
        {
            ThrowIfNull(argument, paramName);
            throw new ArgumentException(SR.The_value_cannot_be_an_empty_string_composed_entirely_of_whitespace, paramName);
        }
#endif
    }

    public static void ThrowIfZero(int value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
#if NET8_0_OR_GREATER
        ArgumentOutOfRangeException.ThrowIfZero(value, paramName);
#else
        if (value == 0)
        {
            ThrowException(value, paramName);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ThrowException(int value, string? paramName)
        {
            throw new ArgumentOutOfRangeException(paramName, value, SR.Format0_1_must_be_a_non_zero_value(paramName, value));
        }
#endif
    }

    public static void ThrowIfNegative(int value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
#if NET8_0_OR_GREATER
        ArgumentOutOfRangeException.ThrowIfNegative(value, paramName);
#else
        if (value < 0)
        {
            ThrowException(value, paramName);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ThrowException(int value, string? paramName)
        {
            throw new ArgumentOutOfRangeException(paramName, value, SR.Format0_1_must_be_a_non_negative_value(paramName, value));
        }
#endif
    }

    public static void ThrowIfNegativeOrZero(int value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
#if NET8_0_OR_GREATER
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value, paramName);
#else
        if (value <= 0)
        {
            ThrowException(value, paramName);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ThrowException(int value, string? paramName)
        {
            throw new ArgumentOutOfRangeException(paramName, value, SR.Format0_1_must_be_a_non_negative_and_non_zero_value(paramName, value));
        }
#endif
    }

    public static void ThrowIfEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IEquatable<T>?
    {
#if NET8_0_OR_GREATER
        ArgumentOutOfRangeException.ThrowIfEqual(value, other, paramName);
#else
        if (EqualityComparer<T>.Default.Equals(value, other))
        {
            ThrowException(value, other, paramName);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ThrowException(T value, T other, string? paramName)
        {
            throw new ArgumentOutOfRangeException(paramName, value, SR.Format0_1_must_not_be_equal_to_2(paramName, (object?)value ?? "null", (object?)other ?? "null"));
        }
#endif
    }

    public static void ThrowIfNotEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IEquatable<T>?
    {
#if NET8_0_OR_GREATER
        ArgumentOutOfRangeException.ThrowIfNotEqual(value, other, paramName);
#else
        if (!EqualityComparer<T>.Default.Equals(value, other))
        {
            ThrowException(value, other, paramName);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ThrowException(T value, T other, string? paramName)
        {
            throw new ArgumentOutOfRangeException(paramName, value, SR.Format0_1_must_be_equal_to_2(paramName, (object?)value ?? "null", (object?)other ?? "null"));
        }
#endif
    }

    public static void ThrowIfGreaterThan<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IComparable<T>
    {
#if NET8_0_OR_GREATER
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, other, paramName);
#else
        if (value.CompareTo(other) > 0)
        {
            ThrowException(value, other, paramName);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ThrowException(T value, T other, string? paramName)
        {
            throw new ArgumentOutOfRangeException(paramName, value, SR.Format0_1_must_be_less_than_or_equal_to_2(paramName, value, other));
        }
#endif
    }

    public static void ThrowIfGreaterThanOrEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IComparable<T>
    {
#if NET8_0_OR_GREATER
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, other, paramName);
#else
        if (value.CompareTo(other) >= 0)
        {
            ThrowException(value, other, paramName);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ThrowException(T value, T other, string? paramName)
        {
            throw new ArgumentOutOfRangeException(paramName, value, SR.Format0_1_must_be_less_than_2(paramName, value, other));
        }
#endif
    }

    public static void ThrowIfLessThan<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IComparable<T>
    {
#if NET8_0_OR_GREATER
        ArgumentOutOfRangeException.ThrowIfLessThan(value, other, paramName);
#else
        if (value.CompareTo(other) < 0)
        {
            ThrowException(value, other, paramName);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ThrowException(T value, T other, string? paramName)
        {
            throw new ArgumentOutOfRangeException(paramName, value, SR.Format0_1_must_be_greater_than_or_equal_to_2(paramName, value, other));
        }
#endif
    }

    public static void ThrowIfLessThanOrEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IComparable<T>
    {
#if NET8_0_OR_GREATER
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, other, paramName);
#else
        if (value.CompareTo(other) <= 0)
        {
            ThrowException(value, other, paramName);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ThrowException(T value, T other, string? paramName)
        {
            throw new ArgumentOutOfRangeException(paramName, value, SR.Format0_1_must_be_greater_than_2(paramName, value, other));
        }
#endif
    }
}
