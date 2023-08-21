// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal abstract class DocumentContextFactory
{
    public DocumentContext? TryCreate(TextDocumentIdentifier documentIdentifier)
        => TryCreateCore(documentIdentifier.Uri, documentIdentifier.GetProjectContext(), versioned: false);

    public DocumentContext? TryCreate(Uri documentUri)
        => TryCreateCore(documentUri, projectContext: null, versioned: false);

    public DocumentContext? TryCreate(Uri documentUri, VSProjectContext? projectContext)
        => TryCreateCore(documentUri, projectContext, versioned: false);

    public VersionedDocumentContext? TryCreateForOpenDocument(Uri documentUri)
        => (VersionedDocumentContext?) TryCreateCore(documentUri, projectContext: null, versioned: true);

    public VersionedDocumentContext? TryCreateForOpenDocument(TextDocumentIdentifier documentIdentifier)
        => (VersionedDocumentContext?)TryCreateCore(documentIdentifier.Uri, documentIdentifier.GetProjectContext(), versioned: true);

    public VersionedDocumentContext? TryCreateForOpenDocument(Uri documentUri, VSProjectContext? projectContext)
        => (VersionedDocumentContext?)TryCreateCore(documentUri, projectContext, versioned: true);

    protected abstract DocumentContext? TryCreateCore(Uri documentUri, VSProjectContext? projectContext, bool versioned);
}
