// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentHighlight;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentDocumentHighlightName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostDocumentHighlightEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal class CohostDocumentHighlightEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
    LSPRequestInvoker requestInvoker)
    : AbstractRazorCohostDocumentRequestHandler<DocumentHighlightParams, DocumentHighlight[]?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public Registration? GetRegistration(VSInternalClientCapabilities clientCapabilities, DocumentFilter[] filter, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.SupportsVisualStudioExtensions)
        {
            return new Registration
            {
                Method = Methods.TextDocumentDocumentHighlightName,
                RegisterOptions = new DocumentHighlightRegistrationOptions()
                {
                    DocumentSelector = filter
                }
            };
        }

        return null;
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(DocumentHighlightParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<DocumentHighlight[]?> HandleRequestAsync(DocumentHighlightParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(request, context.TextDocument.AssumeNotNull(), cancellationToken);

    private async Task<DocumentHighlight[]?> HandleRequestAsync(DocumentHighlightParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var csharpResult = await _remoteServiceInvoker.TryInvokeAsync<IRemoteDocumentHighlightService, RemoteResponse<RemoteDocumentHighlight[]?>>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetHighlightsAsync(solutionInfo, razorDocument.Id, request.Position.ToLinePosition(), cancellationToken),
            cancellationToken).ConfigureAwait(false);

        // If we got a response back, then either Razor or C# wants to do something with this, so we're good to go
        if (csharpResult.Result is { } highlights)
        {
            return highlights.Select(RemoteDocumentHighlight.ToVsDocumentHighlight).ToArray();
        }

        if (csharpResult.StopHandling)
        {
            return null;
        }

        // If we didn't get anything from Razor or Roslyn, lets ask Html what they want to do
        var htmlDocument = await _htmlDocumentSynchronizer.TryGetSynchronizedHtmlDocumentAsync(razorDocument, cancellationToken).ConfigureAwait(false);
        if (htmlDocument is null)
        {
            return null;
        }

        request.TextDocument = request.TextDocument.WithUri(htmlDocument.Uri);

        var result = await _requestInvoker.ReinvokeRequestOnServerAsync<DocumentHighlightParams, DocumentHighlight[]?>(
            htmlDocument.Buffer,
            Methods.TextDocumentDocumentHighlightName,
            RazorLSPConstants.HtmlLanguageServerName,
            request,
            cancellationToken).ConfigureAwait(false);

        // Since we don't need to map positions in Html, and document highlight results don't have Uri's, we can return these results directly
        return result?.Response;
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostDocumentHighlightEndpoint instance)
    {
        public Task<DocumentHighlight[]?> HandleRequestAsync(DocumentHighlightParams request, TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}
