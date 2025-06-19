// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Razor;

namespace Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

internal static class Extensions
{
    private const string RazorExtension = ".razor";
    private const string CSHtmlExtension = ".cshtml";

    public static bool IsRazorFilePath(this string filePath)
    {
        var comparison = PathUtilities.OSSpecificPathComparison;

        return filePath.EndsWith(RazorExtension, comparison) ||
               filePath.EndsWith(CSHtmlExtension, comparison);
    }

    public static bool IsRazorDocument(this TextDocument document)
        => document is AdditionalDocument &&
           document.FilePath is string filePath &&
           filePath.IsRazorFilePath();

    public static bool ContainsRazorDocuments(this Project project)
        => project.AdditionalDocuments.Any(static d => d.IsRazorDocument());
}
