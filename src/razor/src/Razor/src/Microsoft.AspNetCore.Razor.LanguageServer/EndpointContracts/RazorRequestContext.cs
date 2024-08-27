// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;

internal readonly struct RazorRequestContext(DocumentContext? documentContext, ILspServices lspServices, string method, Uri? uri)
{
    public readonly DocumentContext? DocumentContext = documentContext;
    public readonly ILspServices LspServices = lspServices;
    public readonly string Method = method;
    public readonly Uri? Uri = uri;

    public T GetRequiredService<T>() where T : class
    {
        return LspServices.GetRequiredService<T>();
    }
}
