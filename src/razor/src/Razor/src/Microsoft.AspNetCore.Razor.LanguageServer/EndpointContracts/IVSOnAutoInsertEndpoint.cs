// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;

[LanguageServerEndpoint(VSInternalMethods.OnAutoInsertName)]
internal interface IVSOnAutoInsertEndpoint : IRazorRequestHandler<OnAutoInsertParamsBridge, VSInternalDocumentOnAutoInsertResponseItem?>, IRegistrationExtension
{
}
