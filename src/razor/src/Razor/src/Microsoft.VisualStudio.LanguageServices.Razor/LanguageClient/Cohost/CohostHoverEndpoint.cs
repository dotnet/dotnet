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
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using RoslynHover = Roslyn.LanguageServer.Protocol.Hover;
using RoslynLspFactory = Roslyn.LanguageServer.Protocol.RoslynLspFactory;
using VsHover = Microsoft.VisualStudio.LanguageServer.Protocol.Hover;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentHoverName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostHoverEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostHoverEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
    LSPRequestInvoker requestInvoker)
    : AbstractRazorCohostDocumentRequestHandler<TextDocumentPositionParams, SumType<RoslynHover, VsHover>?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.Hover?.DynamicRegistration == true)
        {
            return [new Registration
            {
                Method = Methods.TextDocumentHoverName,
                RegisterOptions = new HoverRegistrationOptions()
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(TextDocumentPositionParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<SumType<RoslynHover, VsHover>?> HandleRequestAsync(TextDocumentPositionParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(
            request,
            context.TextDocument.AssumeNotNull(),
            cancellationToken);

    private async Task<SumType<RoslynHover, VsHover>?> HandleRequestAsync(TextDocumentPositionParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var position = RoslynLspFactory.CreatePosition(request.Position.ToLinePosition());

        var response = await _remoteServiceInvoker
            .TryInvokeAsync<IRemoteHoverService, RemoteResponse<RoslynHover?>>(
                razorDocument.Project.Solution,
                (service, solutionInfo, cancellationToken) =>
                    service.GetHoverAsync(solutionInfo, razorDocument.Id, position, cancellationToken),
                cancellationToken)
            .ConfigureAwait(false);

        if (response.Result is RoslynHover hover)
        {
            return hover;
        }

        if (response.StopHandling)
        {
            return null;
        }

        return await GetHtmlHoverAsync(request, razorDocument, cancellationToken).ConfigureAwait(false);
    }

    private async Task<VsHover?> GetHtmlHoverAsync(TextDocumentPositionParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var htmlDocument = await _htmlDocumentSynchronizer.TryGetSynchronizedHtmlDocumentAsync(razorDocument, cancellationToken).ConfigureAwait(false);
        if (htmlDocument is null)
        {
            return null;
        }

        request.TextDocument = request.TextDocument.WithUri(htmlDocument.Uri);

        var result = await _requestInvoker
            .ReinvokeRequestOnServerAsync<TextDocumentPositionParams, VsHover?>(
                htmlDocument.Buffer,
                Methods.TextDocumentHoverName,
                RazorLSPConstants.HtmlLanguageServerName,
                request,
                cancellationToken)
            .ConfigureAwait(false);

        return result?.Response;
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostHoverEndpoint instance)
    {
        public Task<SumType<RoslynHover, VsHover>?> HandleRequestAsync(
            TextDocumentPositionParams request, TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}
