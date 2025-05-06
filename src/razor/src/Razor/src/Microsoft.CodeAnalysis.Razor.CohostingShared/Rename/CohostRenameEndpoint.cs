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
using Microsoft.CodeAnalysis.Razor.Remote;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentRenameName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportRazorStatelessLspService(typeof(CohostRenameEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostRenameEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IHtmlRequestInvoker requestInvoker)
    : AbstractRazorCohostDocumentRequestHandler<RenameParams, WorkspaceEdit?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IHtmlRequestInvoker _requestInvoker = requestInvoker;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.Rename?.DynamicRegistration == true)
        {
            return [new Registration
            {
                Method = Methods.TextDocumentRenameName,
                RegisterOptions = new RenameRegistrationOptions()
                {
                    PrepareProvider = false
                }
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(RenameParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<WorkspaceEdit?> HandleRequestAsync(RenameParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(request, context.TextDocument.AssumeNotNull(), cancellationToken);

    private async Task<WorkspaceEdit?> HandleRequestAsync(RenameParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var result = await _remoteServiceInvoker.TryInvokeAsync<IRemoteRenameService, RemoteResponse<WorkspaceEdit?>>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetRenameEditAsync(solutionInfo, razorDocument.Id, request.Position, request.NewName, cancellationToken),
            cancellationToken).ConfigureAwait(false);

        if (result.Result is { } edit)
        {
            return edit;
        }

        if (result.StopHandling)
        {
            return null;
        }

        return await _requestInvoker.MakeHtmlLspRequestAsync<RenameParams, WorkspaceEdit>(
            razorDocument,
            Methods.TextDocumentRenameName,
            request,
            cancellationToken).ConfigureAwait(false);
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostRenameEndpoint instance)
    {
        public Task<WorkspaceEdit?> HandleRequestAsync(RenameParams request, TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}
