// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using RoslynLspFactory = Roslyn.LanguageServer.Protocol.RoslynLspFactory;
using RoslynLspLocation = Roslyn.LanguageServer.Protocol.Location;
using VsLspLocation = Microsoft.VisualStudio.LanguageServer.Protocol.Location;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentImplementationName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostGoToImplementationEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostGoToImplementationEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
    LSPRequestInvoker requestInvoker,
    IFilePathService filePathService)
    : AbstractRazorCohostDocumentRequestHandler<TextDocumentPositionParams, SumType<RoslynLspLocation[], VsLspLocation[], VSInternalReferenceItem[]>?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;
    private readonly IFilePathService _filePathService = filePathService;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public Registration? GetRegistration(VSInternalClientCapabilities clientCapabilities, DocumentFilter[] filter, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.Implementation?.DynamicRegistration == true)
        {
            return new Registration
            {
                Method = Methods.TextDocumentImplementationName,
                RegisterOptions = new ImplementationOptions()
            };
        }

        return null;
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(TextDocumentPositionParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<SumType<RoslynLspLocation[], VsLspLocation[], VSInternalReferenceItem[]>?> HandleRequestAsync(TextDocumentPositionParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(
            request,
            context.TextDocument.AssumeNotNull(),
            cancellationToken);

    private async Task<SumType<RoslynLspLocation[], VsLspLocation[], VSInternalReferenceItem[]>?> HandleRequestAsync(TextDocumentPositionParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var position = RoslynLspFactory.CreatePosition(request.Position.ToLinePosition());

        var response = await _remoteServiceInvoker
            .TryInvokeAsync<IRemoteGoToImplementationService, RemoteResponse<RoslynLspLocation[]?>>(
                razorDocument.Project.Solution,
                (service, solutionInfo, cancellationToken) =>
                    service.GetImplementationAsync(solutionInfo, razorDocument.Id, position, cancellationToken),
                cancellationToken)
            .ConfigureAwait(false);

        if (response.Result is RoslynLspLocation[] locations)
        {
            return locations;
        }

        if (response.StopHandling)
        {
            return null;
        }

        return await GetHtmlImplementationsAsync(request, razorDocument, cancellationToken).ConfigureAwait(false);
    }

    private async Task<SumType<RoslynLspLocation[], VsLspLocation[], VSInternalReferenceItem[]>?> GetHtmlImplementationsAsync(TextDocumentPositionParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var htmlDocument = await _htmlDocumentSynchronizer.TryGetSynchronizedHtmlDocumentAsync(razorDocument, cancellationToken).ConfigureAwait(false);
        if (htmlDocument is null)
        {
            return null;
        }

        request.TextDocument = request.TextDocument.WithUri(htmlDocument.Uri);

        var result = await _requestInvoker
            .ReinvokeRequestOnServerAsync<TextDocumentPositionParams, SumType<VsLspLocation[], VSInternalReferenceItem[]>?>(
                htmlDocument.Buffer,
                Methods.TextDocumentImplementationName,
                RazorLSPConstants.HtmlLanguageServerName,
                request,
                cancellationToken)
            .ConfigureAwait(false);

        if (result is not { Response: { } response })
        {
            return null;
        }

        if (response.TryGetFirst(out var locations))
        {
            foreach (var location in locations)
            {
                RemapVirtualHtmlUri(location);
            }

            return locations;
        }
        else if (response.TryGetSecond(out var referenceItems))
        {
            foreach (var referenceItem in referenceItems)
            {
                RemapVirtualHtmlUri(referenceItem.Location);
            }

            return referenceItems;
        }

        return null;
    }

    private void RemapVirtualHtmlUri(VsLspLocation location)
    {
        if (_filePathService.IsVirtualHtmlFile(location.Uri))
        {
            location.Uri = _filePathService.GetRazorDocumentUri(location.Uri);
        }
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostGoToImplementationEndpoint instance)
    {
        public Task<SumType<RoslynLspLocation[], VsLspLocation[], VSInternalReferenceItem[]>?> HandleRequestAsync(
            TextDocumentPositionParams request, TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}
