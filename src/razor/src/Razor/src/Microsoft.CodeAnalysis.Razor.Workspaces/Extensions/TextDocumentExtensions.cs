// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor;
using Microsoft.NET.Sdk.Razor.SourceGenerators;

namespace Microsoft.CodeAnalysis;

internal static class TextDocumentExtensions
{
    /// <summary>
    /// This method tries to compute the source generated hint name for a Razor document using only string manipulation
    /// </summary>
    /// <remarks>
    /// This should only be used in the devenv process. In OOP we can look at the actual generated run result to find this
    /// information.
    /// </remarks>
    public static bool TryComputeHintNameFromRazorDocument(this TextDocument razorDocument, [NotNullWhen(true)] out string? hintName)
    {
        if (razorDocument.FilePath is null)
        {
            hintName = null;
            return false;
        }

        var filePath = razorDocument.FilePath.AsSpanOrDefault();
        if (string.IsNullOrEmpty(razorDocument.Project.FilePath))
        {
            var fileName = filePath[filePath.LastIndexOfAny(['/', '\\'])..].TrimStart(['/', '\\']);
            hintName = RazorSourceGenerator.GetIdentifierFromPath(fileName);
            return hintName is not null;
        }

        var projectFilePath = razorDocument.Project.FilePath.AsSpanOrDefault();
        var projectBasePath = PathUtilities.GetDirectoryName(projectFilePath);
        if (filePath.Length <= projectBasePath.Length)
        {
            // File must be from outside the project directory
            hintName = null;
            return false;
        }

        var relativeDocumentPath = filePath[projectBasePath.Length..].TrimStart(['/', '\\']);

        hintName = RazorSourceGenerator.GetIdentifierFromPath(relativeDocumentPath);

        return hintName is not null;
    }
}
