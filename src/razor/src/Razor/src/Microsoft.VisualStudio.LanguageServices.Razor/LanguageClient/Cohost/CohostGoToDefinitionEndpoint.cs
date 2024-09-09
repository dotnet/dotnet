// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using static Roslyn.LanguageServer.Protocol.RoslynLspExtensions;
using RoslynDocumentLink = Roslyn.LanguageServer.Protocol.DocumentLink;
using RoslynLocation = Roslyn.LanguageServer.Protocol.Location;
using RoslynLspFactory = Roslyn.LanguageServer.Protocol.RoslynLspFactory;
using VsLspLocation = Microsoft.VisualStudio.LanguageServer.Protocol.Location;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentDefinitionName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostGoToDefinitionEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostGoToDefinitionEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
    LSPRequestInvoker requestInvoker,
    IFilePathService filePathService)
    : AbstractRazorCohostDocumentRequestHandler<TextDocumentPositionParams, SumType<RoslynLocation, RoslynLocation[], RoslynDocumentLink[]>?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;
    private readonly IFilePathService _filePathService = filePathService;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public Registration? GetRegistration(VSInternalClientCapabilities clientCapabilities, DocumentFilter[] filter, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.Definition?.DynamicRegistration == true)
        {
            return new Registration
            {
                Method = Methods.TextDocumentDefinitionName,
                RegisterOptions = new DefinitionOptions()
            };
        }

        return null;
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(TextDocumentPositionParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<SumType<RoslynLocation, RoslynLocation[], RoslynDocumentLink[]>?> HandleRequestAsync(TextDocumentPositionParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(
            request,
            context.TextDocument.AssumeNotNull(),
            cancellationToken);

    private async Task<SumType<RoslynLocation, RoslynLocation[], RoslynDocumentLink[]>?> HandleRequestAsync(TextDocumentPositionParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var position = RoslynLspFactory.CreatePosition(request.Position.ToLinePosition());

        var response = await _remoteServiceInvoker
            .TryInvokeAsync<IRemoteGoToDefinitionService, RemoteResponse<RoslynLocation[]?>>(
                razorDocument.Project.Solution,
                (service, solutionInfo, cancellationToken) =>
                    service.GetDefinitionAsync(solutionInfo, razorDocument.Id, position, cancellationToken),
                cancellationToken)
            .ConfigureAwait(false);

        if (response.Result is RoslynLocation[] locations)
        {
            return locations;
        }

        if (response.StopHandling)
        {
            return null;
        }

        return await GetHtmlDefinitionsAsync(request, razorDocument, cancellationToken).ConfigureAwait(false);
    }

    private async Task<SumType<RoslynLocation, RoslynLocation[], RoslynDocumentLink[]>?> GetHtmlDefinitionsAsync(TextDocumentPositionParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var htmlDocument = await _htmlDocumentSynchronizer.TryGetSynchronizedHtmlDocumentAsync(razorDocument, cancellationToken).ConfigureAwait(false);
        if (htmlDocument is null)
        {
            return null;
        }

        request.TextDocument.Uri = htmlDocument.Uri;

        var result = await _requestInvoker
            .ReinvokeRequestOnServerAsync<TextDocumentPositionParams, SumType<VsLspLocation, VsLspLocation[], DocumentLink[]>?>(
                htmlDocument.Buffer,
                Methods.TextDocumentDefinitionName,
                RazorLSPConstants.HtmlLanguageServerName,
                request,
                cancellationToken)
            .ConfigureAwait(false);

        if (result is not { Response: { } response })
        {
            return null;
        }

        if (response.TryGetFirst(out var singleLocation))
        {
            return RoslynLspFactory.CreateLocation(RemapVirtualHtmlUri(singleLocation.Uri), singleLocation.Range.ToLinePositionSpan());
        }
        else if (response.TryGetSecond(out var multipleLocations))
        {
            return Array.ConvertAll(multipleLocations, l => RoslynLspFactory.CreateLocation(RemapVirtualHtmlUri(l.Uri), l.Range.ToLinePositionSpan()));
        }
        else if (response.TryGetThird(out var documentLinks))
        {
            using var builder = new PooledArrayBuilder<RoslynDocumentLink>(capacity: documentLinks.Length);

            foreach (var documentLink in documentLinks)
            {
                if (documentLink.Target is Uri target)
                {
                    builder.Add(RoslynLspFactory.CreateDocumentLink(RemapVirtualHtmlUri(target), documentLink.Range.ToLinePositionSpan()));
                }
            }

            return builder.ToArray();
        }

        return null;
    }

    private Uri RemapVirtualHtmlUri(Uri uri)
    {
        if (_filePathService.IsVirtualHtmlFile(uri))
        {
            return _filePathService.GetRazorDocumentUri(uri);
        }

        return uri;
    }


    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostGoToDefinitionEndpoint instance)
    {
        public Task<SumType<RoslynLocation, RoslynLocation[], RoslynDocumentLink[]>?> HandleRequestAsync(
            TextDocumentPositionParams request, TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}
