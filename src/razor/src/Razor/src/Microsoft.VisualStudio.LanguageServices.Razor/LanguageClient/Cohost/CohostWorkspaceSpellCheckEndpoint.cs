// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(VSInternalMethods.WorkspaceSpellCheckableRangesName)]
[ExportCohostStatelessLspService(typeof(CohostWorkspaceSpellCheckEndpoint))]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostWorkspaceSpellCheckEndpoint : AbstractRazorCohostRequestHandler<VSInternalWorkspaceSpellCheckableParams, VSInternalWorkspaceSpellCheckableReport[]>
{
    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => false;

    // Razor files generally don't do anything at the workspace level

    protected override Task<VSInternalWorkspaceSpellCheckableReport[]> HandleRequestAsync(VSInternalWorkspaceSpellCheckableParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => SpecializedTasks.EmptyArray<VSInternalWorkspaceSpellCheckableReport>();
}
