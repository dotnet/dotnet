// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;
using Microsoft.CodeAnalysis.Razor.LinkedEditingRange;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentLinkedEditingRangeName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportRazorStatelessLspService(typeof(CohostLinkedEditingRangeEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostLinkedEditingRangeEndpoint(IRemoteServiceInvoker remoteServiceInvoker)
    : AbstractRazorCohostDocumentRequestHandler<LinkedEditingRangeParams, LinkedEditingRanges?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.LinkedEditingRange?.DynamicRegistration == true)
        {
            return [new Registration
            {
                Method = Methods.TextDocumentLinkedEditingRangeName,
                RegisterOptions = new LinkedEditingRangeRegistrationOptions()
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(LinkedEditingRangeParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<LinkedEditingRanges?> HandleRequestAsync(LinkedEditingRangeParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(request, context.TextDocument.AssumeNotNull(), cancellationToken);

    private async Task<LinkedEditingRanges?> HandleRequestAsync(LinkedEditingRangeParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var linkedRanges = await _remoteServiceInvoker.TryInvokeAsync<IRemoteLinkedEditingRangeService, LinePositionSpan[]?>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetRangesAsync(solutionInfo, razorDocument.Id, request.Position.ToLinePosition(), cancellationToken),
            cancellationToken).ConfigureAwait(false);

        if (linkedRanges is [{ } span1, { } span2])
        {
            return new LinkedEditingRanges
            {
                Ranges = [span1.ToRange(), span2.ToRange()],
                WordPattern = LinkedEditingRangeHelper.WordPattern
            };
        }

        return null;
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostLinkedEditingRangeEndpoint instance)
    {
        public Task<LinkedEditingRanges?> HandleRequestAsync(LinkedEditingRangeParams request, TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}
