// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.NET.Sdk.Razor.SourceGenerators;

namespace Microsoft.VisualStudio.Razor.Extensions;

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

        var projectBasePath = Path.GetDirectoryName(razorDocument.Project.FilePath);
        var relativeDocumentPath = razorDocument.FilePath[projectBasePath.Length..].TrimStart('/', '\\');
        hintName = RazorSourceGenerator.GetIdentifierFromPath(relativeDocumentPath);

        return hintName is not null;
    }
}
