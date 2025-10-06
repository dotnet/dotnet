// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal static class IDocumentContextFactoryExtensions
{
    public static bool TryCreate(
        this IDocumentContextFactory service,
        TextDocumentIdentifier documentIdentifier,
        [NotNullWhen(true)] out DocumentContext? context)
            => service.TryCreate(documentIdentifier.DocumentUri.GetRequiredParsedUri(), documentIdentifier.GetProjectContext(), out context);

    public static bool TryCreate(
        this IDocumentContextFactory service,
        Uri documentUri,
        [NotNullWhen(true)] out DocumentContext? context)
            => service.TryCreate(documentUri, projectContext: null, out context);
}
