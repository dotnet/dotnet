// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;

internal readonly struct RazorRequestContext(VersionedDocumentContext? documentContext, ILspServices lspServices, string method, Uri? uri)
{
    public readonly VersionedDocumentContext? DocumentContext = documentContext;
    public readonly ILspServices LspServices = lspServices;
    public readonly string Method = method;
    public readonly Uri? Uri = uri;

    public VersionedDocumentContext GetRequiredDocumentContext()
    {
        if (DocumentContext is null)
        {
            throw new ArgumentNullException(nameof(DocumentContext), $"Could not find a document context for '{Method}' on '{Uri}'");
        }

        return DocumentContext;
    }

    public T GetRequiredService<T>() where T : class
    {
        return LspServices.GetRequiredService<T>();
    }
}
