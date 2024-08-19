// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentRenameName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostRenameEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal class CohostRenameEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
    LSPRequestInvoker requestInvoker)
    : AbstractRazorCohostDocumentRequestHandler<RenameParams, WorkspaceEdit?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public Registration? GetRegistration(VSInternalClientCapabilities clientCapabilities, DocumentFilter[] filter, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.Rename?.DynamicRegistration == true)
        {
            return new Registration
            {
                Method = Methods.TextDocumentRenameName,
                RegisterOptions = new RenameRegistrationOptions()
                {
                    DocumentSelector = filter,
                    PrepareProvider = false
                }
            };
        }

        return null;
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

        return await GetHtmlRenameEditAsync(request, razorDocument, cancellationToken).ConfigureAwait(false);
    }

    private async Task<WorkspaceEdit?> GetHtmlRenameEditAsync(RenameParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var htmlDocument = await _htmlDocumentSynchronizer.TryGetSynchronizedHtmlDocumentAsync(razorDocument, cancellationToken).ConfigureAwait(false);
        if (htmlDocument is null)
        {
            return null;
        }

        request.TextDocument.Uri = htmlDocument.Uri;

        var result = await _requestInvoker.ReinvokeRequestOnServerAsync<RenameParams, WorkspaceEdit?>(
            htmlDocument.Buffer,
            Methods.TextDocumentRenameName,
            RazorLSPConstants.HtmlLanguageServerName,
            request,
            cancellationToken).ConfigureAwait(false);

        return result?.Response;
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostRenameEndpoint instance)
    {
        public Task<WorkspaceEdit?> HandleRequestAsync(RenameParams request, TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}
