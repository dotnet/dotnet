// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;

[LanguageServerEndpoint(LanguageServerConstants.RazorTranslateDiagnosticsEndpoint)]
internal interface IRazorTranslateDiagnosticsEndpoint : IRazorRequestHandler<RazorDiagnosticsParams, RazorDiagnosticsResponse>
{
}
