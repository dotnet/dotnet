// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;

internal interface IRazorRequestHandler<RequestType, ResponseType> : IRequestHandler<RequestType, ResponseType, RazorRequestContext>, ITextDocumentIdentifierHandler<RequestType, TextDocumentIdentifier>
{
}

internal interface IRazorDocumentlessRequestHandler<RequestType, ResponseType> : IRequestHandler<RequestType, ResponseType, RazorRequestContext>
{
}
