// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
