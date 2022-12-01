// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using DefinitionResult = Microsoft.VisualStudio.LanguageServer.Protocol.SumType<
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalLocation,
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalLocation[],
    Microsoft.VisualStudio.LanguageServer.Protocol.DocumentLink[]>;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;

[LanguageServerEndpoint(Methods.TextDocumentDefinitionName)]
internal interface IDefinitionEndpoint : IRazorRequestHandler<TextDocumentPositionParamsBridge, DefinitionResult?>,
    IRegistrationExtension
{
}
