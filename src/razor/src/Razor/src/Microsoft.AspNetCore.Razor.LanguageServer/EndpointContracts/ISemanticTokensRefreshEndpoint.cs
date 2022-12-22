// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;

[LanguageServerEndpoint(RazorLanguageServerCustomMessageTargets.RazorSemanticTokensRefreshEndpoint)]
internal interface ISemanticTokensRefreshEndpoint : IRazorNotificationHandler<SemanticTokensRefreshParams>,
    IRegistrationExtension
{
}
