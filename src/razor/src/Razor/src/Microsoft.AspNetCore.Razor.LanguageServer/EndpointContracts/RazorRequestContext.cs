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

#if DEBUG
    public readonly string? LspMethodName;
    public readonly Uri? Uri;
#endif

    public RazorRequestContext(
        VersionedDocumentContext? documentContext,
        IRazorLogger logger,
        ILspServices lspServices)
    {
        DocumentContext = documentContext;
        LspServices = lspServices;
        Logger = logger;
    }

#if DEBUG
    public RazorRequestContext(
        VersionedDocumentContext? documentContext,
        IRazorLogger logger,
        ILspServices lspServices,
        string lspMethodName,
        Uri? uri) : this(documentContext, logger, lspServices)
    {
        LspMethodName = lspMethodName;
        Uri = uri;
    }
#endif

    public VersionedDocumentContext GetRequiredDocumentContext()
    {
        if (DocumentContext is null)
        {
            throw new ArgumentNullException(nameof(DocumentContext)
#if DEBUG
                , $"Could not find a document context for '{LspMethodName}' on '{Uri}'"
#endif
                );
        }

        return DocumentContext;
    }

    public T GetRequiredService<T>() where T : class
    {
        return LspServices.GetRequiredService<T>();
    }
}
