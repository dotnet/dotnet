// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;

internal class TestDocumentContextFactory : DocumentContextFactory
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

    public override Task<DocumentContext?> TryCreateAsync(Uri documentUri, CancellationToken cancellationToken)
    {
        if (FilePath is null || CodeDocument is null)
        {
            return Task.FromResult<DocumentContext?>(null);
        }

        return Task.FromResult<DocumentContext?>(TestDocumentContext.From(FilePath, CodeDocument));
    }

    public override Task<VersionedDocumentContext?> TryCreateForOpenDocumentAsync(Uri documentUri, CancellationToken cancellationToken)
    {
        if (FilePath is null || CodeDocument is null || _version is null)
        {
            return Task.FromResult<VersionedDocumentContext?>(null);
        }

        return Task.FromResult<VersionedDocumentContext?>(TestDocumentContext.From(FilePath, CodeDocument, _version.Value));
    }
}
