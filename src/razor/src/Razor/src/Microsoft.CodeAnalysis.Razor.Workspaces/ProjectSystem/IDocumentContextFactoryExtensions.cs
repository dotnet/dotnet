// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal static class IDocumentContextFactoryExtensions
{
    public static Task<DocumentContext?> TryCreateAsync(this IDocumentContextFactory service, TextDocumentIdentifier documentIdentifier, CancellationToken cancellationToken)
        => service.TryCreateAsync(documentIdentifier.Uri, documentIdentifier.GetProjectContext(), versioned: false, cancellationToken);

    public static Task<DocumentContext?> TryCreateAsync(this IDocumentContextFactory service, Uri documentUri, CancellationToken cancellationToken)
        => service.TryCreateAsync(documentUri, projectContext: null, versioned: false, cancellationToken);

    public static Task<DocumentContext?> TryCreateAsync(this IDocumentContextFactory service, Uri documentUri, VSProjectContext? projectContext, CancellationToken cancellationToken)
        => service.TryCreateAsync(documentUri, projectContext, versioned: false, cancellationToken);

    public static async Task<VersionedDocumentContext?> TryCreateForOpenDocumentAsync(this IDocumentContextFactory service, Uri documentUri, CancellationToken cancellationToken)
        => (VersionedDocumentContext?)await service.TryCreateAsync(documentUri, projectContext: null, versioned: true, cancellationToken).ConfigureAwait(false);

    public static async Task<VersionedDocumentContext?> TryCreateForOpenDocumentAsync(this IDocumentContextFactory service, TextDocumentIdentifier documentIdentifier, CancellationToken cancellationToken)
        => (VersionedDocumentContext?)await service.TryCreateAsync(documentIdentifier.Uri, documentIdentifier.GetProjectContext(), versioned: true, cancellationToken).ConfigureAwait(false);

    public static async Task<VersionedDocumentContext?> TryCreateForOpenDocumentAsync(this IDocumentContextFactory service, Uri documentUri, VSProjectContext? projectContext, CancellationToken cancellationToken)
        => (VersionedDocumentContext?)await service.TryCreateAsync(documentUri, projectContext, versioned: true, cancellationToken).ConfigureAwait(false);
}
