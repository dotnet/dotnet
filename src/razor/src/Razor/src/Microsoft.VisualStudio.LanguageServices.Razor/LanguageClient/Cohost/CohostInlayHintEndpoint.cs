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
using Roslyn.LanguageServer.Protocol;
using VSLSP = Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentInlayHintName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostInlayHintEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal class CohostInlayHintEndpoint(IRemoteServiceInvoker remoteServiceInvoker)
    : AbstractRazorCohostDocumentRequestHandler<InlayHintParams, InlayHint[]?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<VSLSP.Registration> GetRegistrations(VSLSP.VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.InlayHint?.DynamicRegistration == true)
        {
            return [new VSLSP.Registration
            {
                Method = Methods.TextDocumentInlayHintName,
                RegisterOptions = new VSLSP.InlayHintRegistrationOptions()
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(InlayHintParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<InlayHint[]?> HandleRequestAsync(InlayHintParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
    {
        // TODO: Once the platform team have finished the work, check the "Show inlay hints while key pressed" option, and pass it along
        return HandleRequestAsync(request, context.TextDocument.AssumeNotNull(), displayAllOverride: false, cancellationToken);
    }

    private async Task<InlayHint[]?> HandleRequestAsync(InlayHintParams request, TextDocument razorDocument, bool displayAllOverride, CancellationToken cancellationToken)
    {
        // Normally we could remove the await here, but in this case it neatly converts from ValueTask to Task for us,
        // and more importantly this method is essentially a public API entry point (via LSP) so having it appear in
        // call stacks is desirable
        return await _remoteServiceInvoker.TryInvokeAsync<IRemoteInlayHintService, InlayHint[]?>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetInlayHintsAsync(solutionInfo, razorDocument.Id, request, displayAllOverride, cancellationToken),
            cancellationToken).ConfigureAwait(false);
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostInlayHintEndpoint instance)
    {
        public Task<InlayHint[]?> HandleRequestAsync(InlayHintParams request, TextDocument razorDocument, bool displayAllOverride, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(request, razorDocument, displayAllOverride, cancellationToken);
    }
}
