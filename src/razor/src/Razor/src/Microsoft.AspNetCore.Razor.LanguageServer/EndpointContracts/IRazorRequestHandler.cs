// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;

internal interface IRazorRequestHandler<RequestType, ResponseType> : IRequestHandler<RequestType, ResponseType, RazorRequestContext>, ITextDocumentIdentifierHandler<RequestType, TextDocumentIdentifier>
{
}

internal interface IRazorDocumentlessRequestHandler<RequestType, ResponseType> : IRequestHandler<RequestType, ResponseType, RazorRequestContext>
{
}
