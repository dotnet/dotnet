// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Language;

public static class RazorCodeDocumentExtensions
{
    public static bool TryComputeClassName(this RazorCodeDocument codeDocument, [NotNullWhen(true)] out string? className)
    {
        var filePath = codeDocument.Source.RelativePath ?? codeDocument.Source.FilePath;
        if (filePath.IsNullOrEmpty())
        {
            className = null;
            return false;
        }

        className = CSharpIdentifier.GetClassNameFromPath(filePath);
        return className is not null;
    }

    public static bool TryGetNamespace(
        this RazorCodeDocument codeDocument,
        bool fallbackToRootNamespace,
        [NotNullWhen(true)] out string? @namespace)
        => codeDocument.TryGetNamespace(fallbackToRootNamespace, out @namespace, out _);

    public static bool TryGetNamespace(
        this RazorCodeDocument codeDocument,
        bool fallbackToRootNamespace,
        [NotNullWhen(true)] out string? @namespace,
        out SourceSpan? namespaceSpan)
        => codeDocument.TryGetNamespace(fallbackToRootNamespace, considerImports: true, out @namespace, out namespaceSpan);
}
