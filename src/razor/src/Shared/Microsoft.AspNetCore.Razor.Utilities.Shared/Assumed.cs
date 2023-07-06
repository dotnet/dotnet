// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Razor;

internal static class Assumed
{
    /// <summary>
    ///  Can be called at points that are assumed to be unreachable at runtime.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    [DoesNotReturn]
    public static void Unreachable([CallerFilePath] string? path = null, [CallerLineNumber] int line = 0)
        => throw new InvalidOperationException(
            SR.FormatThis_program_location_is_thought_to_be_unreachable_File_0_Line_1(path, line));

    /// <summary>
    ///  Can be called at points that are assumed to be unreachable at runtime.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    [DoesNotReturn]
    public static T Unreachable<T>([CallerFilePath] string? path = null, [CallerLineNumber] int line = 0)
        => throw new InvalidOperationException(
            SR.FormatThis_program_location_is_thought_to_be_unreachable_File_0_Line_1(path, line));
}
