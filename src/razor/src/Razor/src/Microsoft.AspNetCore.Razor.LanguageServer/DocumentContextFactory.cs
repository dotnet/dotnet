// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal abstract class DocumentContextFactory
{
    public Task<DocumentContext?> TryCreateAsync(TextDocumentIdentifier documentIdentifier, CancellationToken cancellationToken)
        => TryCreateCoreAsync(documentIdentifier.Uri, documentIdentifier.GetProjectContext(), versioned: false, cancellationToken);

    public Task<DocumentContext?> TryCreateAsync(Uri documentUri, CancellationToken cancellationToken)
        => TryCreateCoreAsync(documentUri, projectContext: null, versioned: false, cancellationToken);

    public Task<DocumentContext?> TryCreateAsync(Uri documentUri, VSProjectContext? projectContext, CancellationToken cancellationToken)
        => TryCreateCoreAsync(documentUri, projectContext, versioned: false, cancellationToken);

    public async Task<VersionedDocumentContext?> TryCreateForOpenDocumentAsync(Uri documentUri, CancellationToken cancellationToken)
        => (VersionedDocumentContext?) await TryCreateCoreAsync(documentUri, projectContext: null, versioned: true, cancellationToken).ConfigureAwait(false);

    public async Task<VersionedDocumentContext?> TryCreateForOpenDocumentAsync(TextDocumentIdentifier documentIdentifier, CancellationToken cancellationToken)
        => (VersionedDocumentContext?)await TryCreateCoreAsync(documentIdentifier.Uri, documentIdentifier.GetProjectContext(), versioned: true, cancellationToken).ConfigureAwait(false);

    public async Task<VersionedDocumentContext?> TryCreateForOpenDocumentAsync(Uri documentUri, VSProjectContext? projectContext, CancellationToken cancellationToken)
        => (VersionedDocumentContext?)await TryCreateCoreAsync(documentUri, projectContext, versioned: true, cancellationToken).ConfigureAwait(false);

    protected abstract Task<DocumentContext?> TryCreateCoreAsync(Uri documentUri, VSProjectContext? projectContext, bool versioned, CancellationToken cancellationToken);
}
