// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

internal static class Extensions
{
    public static bool IsRazorDocument(this TextDocument document)
        => document is AdditionalDocument &&
           document.FilePath is string filePath &&
           filePath.IsRazorFilePath();

    public static bool ContainsRazorDocuments(this Project project)
        => project.AdditionalDocuments.Any(static d => d.IsRazorDocument());
}
