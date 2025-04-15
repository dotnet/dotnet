// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

#if !NET
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Razor.Utilities;
#endif

namespace Microsoft.AspNetCore.Razor;

internal static class PathUtilities
{
    [return: NotNullIfNotNull(nameof(path))]
    public static string? GetExtension(string? path)
        => Path.GetExtension(path);

    public static ReadOnlySpan<char> GetExtension(ReadOnlySpan<char> path)
    {
#if NET
        return Path.GetExtension(path);
#else
        // Derived the .NET Runtime:
        // - https://github.com/dotnet/runtime/blob/850c0ab4519b904a28f2d67abdaba1ac78c955ff/src/libraries/System.Private.CoreLib/src/System/IO/Path.cs#L189-L213

        var length = path.Length;

        for (var i = length - 1; i >= 0; i--)
        {
            var ch = path[i];

            if (ch == '.')
            {
                return i != length - 1
                    ? path[i..length]
                    :  [];
            }

            if (IsDirectorySeparator(ch))
            {
                break;
            }
        }

        return [];
#endif
    }

    public static bool HasExtension([NotNullWhen(true)] string? path)
        => Path.HasExtension(path);

    public static bool HasExtension(ReadOnlySpan<char> path)
    {
#if NET
        return Path.HasExtension(path);
#else
        return !GetExtension(path).IsEmpty;
#endif
    }

#if !NET
    // Derived from the .NET Runtime:
    // - https://github.com/dotnet/runtime/blob/850c0ab4519b904a28f2d67abdaba1ac78c955ff/src/libraries/Common/src/System/IO/PathInternal.Unix.cs#L27-L32
    // - https://github.com/dotnet/runtime/blob/850c0ab4519b904a28f2d67abdaba1ac78c955ff/src/libraries/Common/src/System/IO/PathInternal.Windows.cs#L280-L283

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsDirectorySeparator(char ch)
        => ch == Path.DirectorySeparatorChar ||
          (PlatformInformation.IsWindows && ch == Path.AltDirectorySeparatorChar);
#endif
    }
