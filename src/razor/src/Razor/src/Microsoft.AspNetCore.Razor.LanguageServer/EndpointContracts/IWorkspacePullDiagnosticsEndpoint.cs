// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;

[LanguageServerEndpoint(VSInternalMethods.WorkspacePullDiagnosticName)]
internal interface IWorkspacePullDiagnosticsEndpoint : IRazorDocumentlessRequestHandler<VSInternalWorkspaceDiagnosticsParams, VSInternalWorkspaceDiagnosticReport[]>
{
}
