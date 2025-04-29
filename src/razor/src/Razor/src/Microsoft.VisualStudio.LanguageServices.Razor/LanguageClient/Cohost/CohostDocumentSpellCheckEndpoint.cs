// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(VSInternalMethods.TextDocumentSpellCheckableRangesName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostDocumentSpellCheckEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostDocumentSpellCheckEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker)
    : AbstractRazorCohostDocumentRequestHandler<VSInternalDocumentSpellCheckableParams, VSInternalSpellCheckableRangeReport[]>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.SupportsVisualStudioExtensions)
        {
            return [new Registration
            {
                Method = VSInternalMethods.TextDocumentSpellCheckableRangesName,
                RegisterOptions = new TextDocumentRegistrationOptions()
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(VSInternalDocumentSpellCheckableParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<VSInternalSpellCheckableRangeReport[]> HandleRequestAsync(VSInternalDocumentSpellCheckableParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(context.TextDocument.AssumeNotNull(), cancellationToken);

    private async Task<VSInternalSpellCheckableRangeReport[]> HandleRequestAsync(TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var data = await _remoteServiceInvoker.TryInvokeAsync<IRemoteSpellCheckService, int[]>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetSpellCheckRangeTriplesAsync(solutionInfo, razorDocument.Id, cancellationToken),
            cancellationToken).ConfigureAwait(false);

        return
            [
                new VSInternalSpellCheckableRangeReport
                    {
                        Ranges = data,
                        ResultId = Guid.NewGuid().ToString()
                    }
            ];
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostDocumentSpellCheckEndpoint instance)
    {
        public Task<VSInternalSpellCheckableRangeReport[]> HandleRequestAsync(TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(razorDocument, cancellationToken);
    }
}
