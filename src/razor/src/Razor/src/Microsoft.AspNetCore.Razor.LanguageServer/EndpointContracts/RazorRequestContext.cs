// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;

internal readonly struct RazorRequestContext
{
    public readonly VersionedDocumentContext? DocumentContext;
    public readonly IRazorLogger Logger;
    public readonly ILspServices LspServices;

    public RazorRequestContext(
        VersionedDocumentContext? documentContext,
        IRazorLogger logger,
        ILspServices lspServices)
    {
        DocumentContext = documentContext;
        LspServices = lspServices;
        Logger = logger;
    }

    public VersionedDocumentContext GetRequiredDocumentContext()
    {
        if (DocumentContext is null)
        {
            throw new ArgumentNullException(nameof(DocumentContext));
        }

        return DocumentContext;
    }

    public T GetRequiredService<T>() where T : class
    {
        return LspServices.GetRequiredService<T>();
    }
}
