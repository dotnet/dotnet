// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace System;

internal static class StringExtensions
{
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? s)
        => string.IsNullOrEmpty(s);

    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? s)
        => string.IsNullOrWhiteSpace(s);

    public static ReadOnlySpan<char> AsSpanOrDefault(this string? s)
        => s is not null ? s.AsSpan() : default;

    public static ReadOnlyMemory<char> AsMemoryOrDefault(this string? s)
        => s is not null ? s.AsMemory() : default;

    // This method doesn't exist on .NET Framework, but it does on .NET Core.
    public static bool Contains(this string s, char ch)
        => s.IndexOf(ch) >= 0;
}
