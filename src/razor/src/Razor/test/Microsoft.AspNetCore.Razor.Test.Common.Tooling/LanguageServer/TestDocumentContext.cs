// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;

internal static class TestDocumentContext
{
    public static DocumentContext Create(Uri uri) => Create(uri, string.Empty);

    public static DocumentContext Create(Uri uri, string text)
    {
        var snapshot = TestDocumentSnapshot.Create(uri.GetAbsoluteOrUNCPath(), text);
        return new DocumentContext(uri, snapshot, projectContext: null);
    }

    public static DocumentContext Create(string filePath, RazorCodeDocument codeDocument)
    {
        var documentSnapshot = TestDocumentSnapshot.Create(filePath, codeDocument);
        var uri = new Uri(filePath);
        return new DocumentContext(uri, documentSnapshot, projectContext: null);
    }

    public static DocumentContext Create(string filePath)
    {
        var properties = RazorSourceDocumentProperties.Create(filePath, filePath);
        var source = RazorSourceDocument.Create(content: string.Empty, properties);
        var codeDocument = RazorCodeDocument.Create(source);

        return Create(filePath, codeDocument);
    }
}
