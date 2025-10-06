// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentPresentation;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;

[RazorLanguageServerEndpoint(VSInternalMethods.TextDocumentUriPresentationName)]
internal interface ITextDocumentUriPresentationHandler : IRazorRequestHandler<UriPresentationParams, WorkspaceEdit?>, ICapabilitiesProvider
{
}
