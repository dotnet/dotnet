// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal static class IDocumentContextFactoryExtensions
{
    public static bool TryCreate(
        this IDocumentContextFactory service,
        TextDocumentIdentifier documentIdentifier,
        [NotNullWhen(true)] out DocumentContext? context)
        => service.TryCreate(documentIdentifier.Uri, documentIdentifier.GetProjectContext(), versioned: false, out context);

    public static bool TryCreate(
        this IDocumentContextFactory service,
        Uri documentUri,
        [NotNullWhen(true)] out DocumentContext? context)
        => service.TryCreate(documentUri, projectContext: null, versioned: false, out context);

    public static bool TryCreate(
        this IDocumentContextFactory service,
        Uri documentUri,
        VSProjectContext? projectContext,
        [NotNullWhen(true)] out DocumentContext? context)
        => service.TryCreate(documentUri, projectContext, versioned: false, out context);

    public static bool TryCreateForOpenDocument(
        this IDocumentContextFactory service,
        Uri documentUri,
        [NotNullWhen(true)] out VersionedDocumentContext? context)
    {
        if (service.TryCreate(documentUri, projectContext: null, versioned: true, out var documentContext))
        {
            context = (VersionedDocumentContext)documentContext;
            return true;
        }

        context = null;
        return false;
    }

    public static bool TryCreateForOpenDocument(
        this IDocumentContextFactory service,
        TextDocumentIdentifier documentIdentifier,
        [NotNullWhen(true)] out VersionedDocumentContext? context)
    {
        if (service.TryCreate(documentIdentifier.Uri, documentIdentifier.GetProjectContext(), versioned: true, out var documentContext))
        {
            context = (VersionedDocumentContext)documentContext;
            return true;
        }

        context = null;
        return false;
    }

    public static bool TryCreateForOpenDocument(
        this IDocumentContextFactory service,
        Uri documentUri,
        VSProjectContext? projectContext,
        [NotNullWhen(true)] out VersionedDocumentContext? context)
    {
        if (service.TryCreate(documentUri, projectContext, versioned: true, out var documentContext))
        {
            context = (VersionedDocumentContext)documentContext;
            return true;
        }

        context = null;
        return false;
    }
}
