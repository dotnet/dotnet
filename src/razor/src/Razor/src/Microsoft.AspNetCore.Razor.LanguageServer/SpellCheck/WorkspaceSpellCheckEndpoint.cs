// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.SpellCheck;

[LanguageServerEndpoint(VSInternalMethods.WorkspaceSpellCheckableRangesName)]
internal sealed class WorkspaceSpellCheckEndpoint : IRazorDocumentlessRequestHandler<VSInternalWorkspaceSpellCheckableParams, VSInternalWorkspaceSpellCheckableReport[]>
{
    public bool MutatesSolutionState => false;

    // Razor files generally don't do anything at the workspace level, so continuing that tradition for spell checking

    public Task<VSInternalWorkspaceSpellCheckableReport[]> HandleRequestAsync(VSInternalWorkspaceSpellCheckableParams request, RazorRequestContext context, CancellationToken cancellationToken)
        => Task.FromResult(Array.Empty<VSInternalWorkspaceSpellCheckableReport>());
}
