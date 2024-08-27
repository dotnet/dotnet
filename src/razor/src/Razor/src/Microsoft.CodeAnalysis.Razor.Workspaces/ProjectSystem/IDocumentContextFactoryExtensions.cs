// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal static class IDocumentContextFactoryExtensions
{
    public static bool TryCreate(
        this IDocumentContextFactory service,
        TextDocumentIdentifier documentIdentifier,
        [NotNullWhen(true)] out DocumentContext? context)
            => service.TryCreate(documentIdentifier.Uri, documentIdentifier.GetProjectContext(), out context);

    public static bool TryCreate(
        this IDocumentContextFactory service,
        Uri documentUri,
        [NotNullWhen(true)] out DocumentContext? context)
            => service.TryCreate(documentUri, projectContext: null, out context);
}
