// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;

[LanguageServerEndpoint(Methods.TextDocumentReferencesName)]
internal interface IVSFindAllReferencesEndpoint : IRazorRequestHandler<ReferenceParamsBridge, VSInternalReferenceItem[]?>, IRegistrationExtension
{
}
