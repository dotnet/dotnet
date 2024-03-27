// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Razor;

internal static class Assumed
{
    public static void False(
        [DoesNotReturnIf(true)] bool condition,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
    {
        if (condition)
        {
            ThrowInvalidOperation(SR.Expected_condition_to_be_false, path, line);
        }
    }

    public static void True(
        [DoesNotReturnIf(false)] bool condition,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
    {
        if (!condition)
        {
            ThrowInvalidOperation(SR.Expected_condition_to_be_true, path, line);
        }
    }

    /// <summary>
    ///  Can be called at points that are assumed to be unreachable at runtime.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    [DoesNotReturn]
    public static void Unreachable([CallerFilePath] string? path = null, [CallerLineNumber] int line = 0)
        => ThrowInvalidOperation(SR.This_program_location_is_thought_to_be_unreachable, path, line);

    /// <summary>
    ///  Can be called at points that are assumed to be unreachable at runtime.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    [DoesNotReturn]
    public static T Unreachable<T>([CallerFilePath] string? path = null, [CallerLineNumber] int line = 0)
    {
        ThrowInvalidOperation(SR.This_program_location_is_thought_to_be_unreachable, path, line);
        return default;
    }

    [DebuggerHidden]
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowInvalidOperation(string message, string? path, int line)
    {
        throw new InvalidOperationException(message);
    }
}
