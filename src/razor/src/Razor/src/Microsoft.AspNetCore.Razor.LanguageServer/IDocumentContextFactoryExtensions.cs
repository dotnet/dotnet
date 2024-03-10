// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal static class IDocumentContextFactoryExtensions
{
    public static DocumentContext? TryCreate(this IDocumentContextFactory service, TextDocumentIdentifier documentIdentifier)
        => service.TryCreate(documentIdentifier.Uri, documentIdentifier.GetProjectContext(), versioned: false);

    public static DocumentContext? TryCreate(this IDocumentContextFactory service, Uri documentUri)
        => service.TryCreate(documentUri, projectContext: null, versioned: false);

    public static DocumentContext? TryCreate(this IDocumentContextFactory service, Uri documentUri, VSProjectContext? projectContext)
        => service.TryCreate(documentUri, projectContext, versioned: false);

    public static VersionedDocumentContext? TryCreateForOpenDocument(this IDocumentContextFactory service, Uri documentUri)
        => (VersionedDocumentContext?)service.TryCreate(documentUri, projectContext: null, versioned: true);

    public static VersionedDocumentContext? TryCreateForOpenDocument(this IDocumentContextFactory service, TextDocumentIdentifier documentIdentifier)
        => (VersionedDocumentContext?)service.TryCreate(documentIdentifier.Uri, documentIdentifier.GetProjectContext(), versioned: true);

    public static VersionedDocumentContext? TryCreateForOpenDocument(this IDocumentContextFactory service, Uri documentUri, VSProjectContext? projectContext)
        => (VersionedDocumentContext?)service.TryCreate(documentUri, projectContext, versioned: true);

}
