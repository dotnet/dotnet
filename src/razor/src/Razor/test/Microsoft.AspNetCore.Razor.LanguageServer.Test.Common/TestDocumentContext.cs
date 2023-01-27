// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;

internal static class TestDocumentContext
{
    public static DocumentContext Create(Uri uri) => Create(uri, string.Empty);

    public static DocumentContext Create(Uri uri, string text)
    {
        var snapshot = TestDocumentSnapshot.Create(uri.GetAbsoluteOrUNCPath(), text);
        return new DocumentContext(uri, snapshot);
    }

    public static VersionedDocumentContext From(string filePath, RazorCodeDocument codeDocument, int hostDocumentVersion)
    {
        var content = codeDocument.GetSourceText().ToString();
        var documentSnapshot = TestDocumentSnapshot.Create(filePath, content);
        documentSnapshot.With(codeDocument);
        var uri = new Uri(filePath);
        return new VersionedDocumentContext(uri, documentSnapshot, hostDocumentVersion);
    }

    public static DocumentContext From(string filePath, RazorCodeDocument codeDocument)
    {
        var content = codeDocument.GetSourceText().ToString();
        var documentSnapshot = TestDocumentSnapshot.Create(filePath, content);
        documentSnapshot.With(codeDocument);
        var uri = new Uri(filePath);
        return new DocumentContext(uri, documentSnapshot);
    }

    public static DocumentContext From(string filePath)
    {
        var properties = new RazorSourceDocumentProperties(filePath, filePath);
        var sourceDocument = RazorSourceDocument.Create(content: string.Empty, properties);
        var codeDocument = RazorCodeDocument.Create(sourceDocument);
        return From(filePath, codeDocument);
    }

    public static VersionedDocumentContext From(string filePath, int hostDocumentVersion)
    {
        var properties = new RazorSourceDocumentProperties(filePath, filePath);
        var sourceDocument = RazorSourceDocument.Create(content: string.Empty, properties);
        var codeDocument = RazorCodeDocument.Create(sourceDocument);
        return From(filePath, codeDocument, hostDocumentVersion);
    }
}
