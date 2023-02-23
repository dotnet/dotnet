// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
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
        [NotNull] this T? obj,
        [CallerArgumentExpression(nameof(obj))] string? objExpression = null)
        where T : class
        => obj ?? throw new InvalidOperationException($"Expected '{objExpression}' to be non-null.");

    [DebuggerStepThrough]
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static T AssumeNotNull<T>(
        [NotNull] this T? obj,
        [CallerArgumentExpression(nameof(obj))] string? objExpression = null)
        where T : struct
        => obj ?? throw new InvalidOperationException($"Expected '{objExpression}' to be non-null.");
}
