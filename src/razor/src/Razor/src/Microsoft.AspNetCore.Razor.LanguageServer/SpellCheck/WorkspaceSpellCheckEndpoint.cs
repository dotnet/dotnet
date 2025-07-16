// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.Threading;

namespace Microsoft.AspNetCore.Razor.LanguageServer.SpellCheck;

[RazorLanguageServerEndpoint(VSInternalMethods.WorkspaceSpellCheckableRangesName)]
internal sealed class WorkspaceSpellCheckEndpoint : IRazorDocumentlessRequestHandler<VSInternalWorkspaceSpellCheckableParams, VSInternalWorkspaceSpellCheckableReport[]>
{
    public bool MutatesSolutionState => false;

    // Razor files generally don't do anything at the workspace level, so continuing that tradition for spell checking

    public Task<VSInternalWorkspaceSpellCheckableReport[]> HandleRequestAsync(VSInternalWorkspaceSpellCheckableParams request, RazorRequestContext context, CancellationToken cancellationToken)
        => SpecializedTasks.EmptyArray<VSInternalWorkspaceSpellCheckableReport>();
}
