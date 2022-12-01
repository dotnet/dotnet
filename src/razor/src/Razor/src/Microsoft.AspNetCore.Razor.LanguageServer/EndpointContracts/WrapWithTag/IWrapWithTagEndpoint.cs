// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts.WrapWithTag;

[LanguageServerEndpoint(LanguageServerConstants.RazorWrapWithTagEndpoint)]
internal interface IWrapWithTagEndpoint : IRazorRequestHandler<WrapWithTagParams, WrapWithTagResponse?>
{
}
