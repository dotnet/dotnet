// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using System;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts.Debugging;

[LanguageServerEndpoint(LanguageServerConstants.RazorProximityExpressionsEndpoint)]
internal interface IRazorProximityExpressionsEndpoint : IRazorDocumentlessRequestHandler<RazorProximityExpressionsParams, RazorProximityExpressionsResponse?>,
    ITextDocumentIdentifierHandler<RazorProximityExpressionsParams, Uri>
{
}
