// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Text;
using Roslyn.LanguageServer.Protocol;
using VSLSP = Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(VSInternalMethods.TextDocumentValidateBreakableRangeName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostValidateBreakableRangeEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostValidateBreakableRangeEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker)
    : AbstractRazorCohostDocumentRequestHandler<VSInternalValidateBreakableRangeParams, Range?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<VSLSP.Registration> GetRegistrations(VSLSP.VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        return [new VSLSP.Registration
        {
            Method = VSInternalMethods.TextDocumentValidateBreakableRangeName,
            RegisterOptions = new VSLSP.TextDocumentRegistrationOptions()
        }];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(VSInternalValidateBreakableRangeParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<Range?> HandleRequestAsync(VSInternalValidateBreakableRangeParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(
            context.TextDocument.AssumeNotNull(),
            request.Range.ToLinePositionSpan(),
            cancellationToken);

    private async Task<Range?> HandleRequestAsync(TextDocument razorDocument, LinePositionSpan span, CancellationToken cancellationToken)
    {
        var response = await _remoteServiceInvoker
            .TryInvokeAsync<IRemoteDebugInfoService, LinePositionSpan?>(
                razorDocument.Project.Solution,
                (service, solutionInfo, cancellationToken) =>
                    service.ValidateBreakableRangeAsync(solutionInfo, razorDocument.Id, span, cancellationToken),
                cancellationToken)
            .ConfigureAwait(false);

        return response?.ToRange();
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostValidateBreakableRangeEndpoint instance)
    {
        public Task<Range?> HandleRequestAsync(TextDocument razorDocument, LinePositionSpan span, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(razorDocument, span, cancellationToken);
    }
}
