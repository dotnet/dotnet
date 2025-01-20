// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Razor;

internal static class NullableExtensions
{
    [DebuggerStepThrough]
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static T AssumeNotNull<T>(
        [NotNull] this T? value,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
        where T : class
    {
        Assumed.NotNull(value, message, valueExpression, path, line);
        return value;
    }

    [DebuggerStepThrough]
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static T AssumeNotNull<T>(
        [NotNull] this T? value,
        [InterpolatedStringHandlerArgument(nameof(value))] Assumed.ThrowIfNullInterpolatedStringHandler<T> message,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
        where T : class
    {
        Assumed.NotNull(value, message, path, line);
        return value;
    }

    [DebuggerStepThrough]
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static T AssumeNotNull<T>(
        [NotNull] this T? value,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
        where T : struct
    {
        Assumed.NotNull(value, message, valueExpression, path, line);
        return value.GetValueOrDefault();
    }

    [DebuggerStepThrough]
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static T AssumeNotNull<T>(
        [NotNull] this T? value,
        [InterpolatedStringHandlerArgument(nameof(value))] Assumed.ThrowIfNullInterpolatedStringHandler<T> message,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
        where T : struct
    {
        Assumed.NotNull(value, message, path, line);
        return value.GetValueOrDefault();
    }
}
