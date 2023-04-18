// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;

[LanguageServerEndpoint(VSInternalMethods.WorkspacePullDiagnosticName)]
internal class WorkspacePullDiagnosticsEndpoint : IRazorDocumentlessRequestHandler<VSInternalWorkspaceDiagnosticsParams, VSInternalWorkspaceDiagnosticReport[]>
{
    public bool MutatesSolutionState => false;

    // We don't actually support workspace diagnostics, but by registering the capability in DocumentPullDiagnosticsEndpoint we will get requests

    public Task<VSInternalWorkspaceDiagnosticReport[]> HandleRequestAsync(VSInternalWorkspaceDiagnosticsParams request, RazorRequestContext context, CancellationToken cancellationToken)
        => Task.FromResult(Array.Empty<VSInternalWorkspaceDiagnosticReport>());
}
