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
    private readonly int? _version;

    public TestDocumentContextFactory()
    {
    }

    public TestDocumentContextFactory(string filePath, RazorCodeDocument codeDocument, int? version = null)
    {
        FilePath = filePath;
        CodeDocument = codeDocument;
        _version = version;
    }

    public virtual bool TryCreate(
        Uri documentUri,
        VSProjectContext? projectContext,
        bool versioned,
        [NotNullWhen(true)] out DocumentContext? context)
    {
        if (FilePath is null || CodeDocument is null)
        {
            context = null;
            return false;
        }

        if (versioned)
        {
            if (_version is null)
            {
                context = null;
                return false;
            }

            context = TestDocumentContext.From(FilePath, CodeDocument, _version.Value);
            return true;
        }

        context = TestDocumentContext.From(FilePath, CodeDocument);
        return true;
    }
}
