// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;

internal class TestDocumentContextFactory : IDocumentContextFactory
{
    private protected readonly string? FilePath;
    private protected readonly RazorCodeDocument? CodeDocument;

    public TestDocumentContextFactory()
    {
    }

    public TestDocumentContextFactory(string filePath, RazorCodeDocument codeDocument)
    {
        FilePath = filePath;
        CodeDocument = codeDocument;
    }

    public virtual bool TryCreate(
        Uri documentUri,
        VSProjectContext? projectContext,
        [NotNullWhen(true)] out DocumentContext? context)
    {
        if (FilePath is null || CodeDocument is null)
        {
            context = null;
            return false;
        }

        context = TestDocumentContext.Create(FilePath, CodeDocument);
        return true;
    }
}
