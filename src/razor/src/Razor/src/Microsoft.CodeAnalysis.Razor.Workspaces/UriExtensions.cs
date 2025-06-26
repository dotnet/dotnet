// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Utilities;

namespace Microsoft.CodeAnalysis.Razor;

internal static class UriExtensions
{
    /// <summary>
    ///  Converts the specified <see cref="Uri"/> into a file path that matches
    ///  a Roslyn <see cref="TextDocument.FilePath"/>.
    /// </summary>
    public static string GetDocumentFilePath(this Uri uri)
        => RazorUri.GetDocumentFilePathFromUri(uri);

    public static Uri GetRequiredParsedUri(this DocumentUri uri)
        => uri.ParsedUri.AssumeNotNull();

    public static string GetAbsoluteOrUNCPath(this DocumentUri uri)
        => GetAbsoluteOrUNCPath(uri.GetRequiredParsedUri());

    public static string GetAbsoluteOrUNCPath(this Uri uri)
    {
        ArgHelper.ThrowIfNull(uri, nameof(uri));

        if (uri.IsUnc)
        {
            // For UNC paths, AbsolutePath doesn't include the host name `//COMPUTERNAME/` part. So we need to use LocalPath instead.
            return uri.LocalPath;
        }

        // Absolute paths are usually encoded.
        var absolutePath = uri.AbsolutePath.Contains("%") ? WebUtility.UrlDecode(uri.AbsolutePath) : uri.AbsolutePath;

        if (string.Equals(uri.Scheme, "git", StringComparison.OrdinalIgnoreCase))
        {
            // return a normalized path when we want to add to a fake git directory path (hacky fix for https://github.com/dotnet/razor/issues/9365)
            return AddToFakeGitDirectoryAtRoot(absolutePath);
        }

        return absolutePath;
    }

    private static string AddToFakeGitDirectoryAtRoot(string decodedAbsolutePath)
    {
        var normalizedPath = FilePathNormalizer.Normalize(decodedAbsolutePath);
        var firstSeparatorIndex = normalizedPath.IndexOf('/');
        if (firstSeparatorIndex < 0)
        {
            // no-op
            return decodedAbsolutePath;
        }

        return normalizedPath.Insert(firstSeparatorIndex + 1, "_git_/");
    }
}
