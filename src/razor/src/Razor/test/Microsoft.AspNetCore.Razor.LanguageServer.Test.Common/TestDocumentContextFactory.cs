// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;

internal class TestDocumentContextFactory : DocumentContextFactory
{
    private readonly string? _filePath;
    private readonly RazorCodeDocument? _codeDocument;
    private readonly int? _version;

    public TestDocumentContextFactory()
    {
    }

    public TestDocumentContextFactory(string filePath, RazorCodeDocument codeDocument, int? version = null)
    {
        _filePath = filePath;
        _codeDocument = codeDocument;
        _version = version;
    }

    public override Task<DocumentContext?> TryCreateAsync(Uri documentUri, CancellationToken cancellationToken)
    {
        if (_filePath is null || _codeDocument is null)
        {
            return Task.FromResult<DocumentContext?>(null);
        }

        return Task.FromResult<DocumentContext?>(TestDocumentContext.From(_filePath, _codeDocument));
    }

    public override Task<VersionedDocumentContext?> TryCreateForOpenDocumentAsync(Uri documentUri, CancellationToken cancellationToken)
    {
        if (_filePath is null || _codeDocument is null || _version is null)
        {
            return Task.FromResult<VersionedDocumentContext?>(null);
        }

        return Task.FromResult<VersionedDocumentContext?>(TestDocumentContext.From(_filePath, _codeDocument, _version.Value));
    }
}
