// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

public static class FilePathNormalizer
{
    public static string NormalizeDirectory(string? directoryFilePath)
    {
        var normalized = Normalize(directoryFilePath);

        if (!normalized.EndsWith("/", StringComparison.Ordinal))
        {
            normalized += '/';
        }

        return normalized;
    }

    public static string Normalize(string? filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return "/";
        }

        var decodedPath = filePath.AssumeNotNull().Contains("%") ? WebUtility.UrlDecode(filePath) : filePath;
        var normalized = decodedPath.Replace('\\', '/');

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
            normalized[0] == '/' &&
            !normalized.StartsWith("//", StringComparison.OrdinalIgnoreCase))
        {
            // We've been provided a path that probably looks something like /C:/path/to
            normalized = normalized[1..];
        }
        else
        {
            // Already a valid path like C:/path or //path
        }

        return normalized;
    }

    public static string GetDirectory(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new InvalidOperationException(filePath);
        }

        var normalizedPath = Normalize(filePath);
        var lastSeparatorIndex = normalizedPath.LastIndexOf('/');

        var directory = normalizedPath[..(lastSeparatorIndex + 1)];
        return directory;
    }

    public static bool FilePathsEquivalent(string filePath1, string filePath2)
    {
        var normalizedFilePath1 = Normalize(filePath1);
        var normalizedFilePath2 = Normalize(filePath2);

        return FilePathComparer.Instance.Equals(normalizedFilePath1, normalizedFilePath2);
    }
}
