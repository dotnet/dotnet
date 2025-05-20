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
                    : [];
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsValidDriveChar(char value)
        => (uint)((value | 0x20) - 'a') <= (uint)('z' - 'a');
#endif

    public static bool IsPathFullyQualified(string path)
    {
        ArgHelper.ThrowIfNull(path);

        return IsPathFullyQualified(path.AsSpan());
    }

    public static bool IsPathFullyQualified(ReadOnlySpan<char> path)
    {
#if NET
        return Path.IsPathFullyQualified(path);
#else
        if (PlatformInformation.IsWindows)
        {
            // Derived from .NET Runtime:
            // - https://github.com/dotnet/runtime/blob/c7c961a330395152e5ec4000032cd3204ceb4a10/src/libraries/Common/src/System/IO/PathInternal.Windows.cs#L250-L274

            if (path.Length < 2)
            {
                // It isn't fixed, it must be relative.  There is no way to specify a fixed
                // path with one character (or less).
                return false;
            }

            if (IsDirectorySeparator(path[0]))
            {
                // There is no valid way to specify a relative path with two initial slashes or
                // \? as ? isn't valid for drive relative paths and \??\ is equivalent to \\?\
                return path[1] == '?' || IsDirectorySeparator(path[1]);
            }

            // The only way to specify a fixed path that doesn't begin with two slashes
            // is the drive, colon, slash format- i.e. C:\
            return (path.Length >= 3)
                && (path[1] == Path.VolumeSeparatorChar)
                && IsDirectorySeparator(path[2])
                // To match old behavior we'll check the drive character for validity as the path is technically
                // not qualified if you don't have a valid drive. "=:\" is the "=" file's default data stream.
                && IsValidDriveChar(path[0]);
        }
        else
        {
            // Derived from .NET Runtime:
            // - https://github.com/dotnet/runtime/blob/c7c961a330395152e5ec4000032cd3204ceb4a10/src/libraries/Common/src/System/IO/PathInternal.Unix.cs#L77-L82

            // This is much simpler than Windows where paths can be rooted, but not fully qualified (such as Drive Relative)
            // As long as the path is rooted in Unix it doesn't use the current directory and therefore is fully qualified.
            return IsPathRooted(path);
        }
#endif
    }

    public static bool IsPathRooted(string path)
    {
#if NET
        return Path.IsPathRooted(path);
#else
        return IsPathRooted(path.AsSpan());
#endif
    }

    public static bool IsPathRooted(ReadOnlySpan<char> path)
    {
#if NET
        return Path.IsPathRooted(path);

#else
        if (PlatformInformation.IsWindows)
        {
            // Derived from .NET Runtime
            // - https://github.com/dotnet/runtime/blob/850c0ab4519b904a28f2d67abdaba1ac78c955ff/src/libraries/System.Private.CoreLib/src/System/IO/Path.Windows.cs#L271-L276

            var length = path.Length;
            return (length >= 1 && IsDirectorySeparator(path[0]))
                || (length >= 2 && IsValidDriveChar(path[0]) && path[1] == Path.VolumeSeparatorChar);
        }
        else
        {
            // Derived from .NET Runtime
            // - https://github.com/dotnet/runtime/blob/850c0ab4519b904a28f2d67abdaba1ac78c955ff/src/libraries/System.Private.CoreLib/src/System/IO/Path.Unix.cs#L132-L135

            return path.StartsWith(Path.DirectorySeparatorChar);
        }
#endif
    }
}
