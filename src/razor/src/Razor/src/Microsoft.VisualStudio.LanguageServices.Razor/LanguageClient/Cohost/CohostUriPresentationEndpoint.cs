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
using Microsoft.VisualStudio.Razor.LanguageClient.Extensions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(VSInternalMethods.TextDocumentUriPresentationName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostUriPresentationEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal class CohostUriPresentationEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
    IFilePathService filePathService,
    LSPRequestInvoker requestInvoker)
    : AbstractRazorCohostDocumentRequestHandler<VSInternalUriPresentationParams, WorkspaceEdit?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
    private readonly IFilePathService _filePathService = filePathService;
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public Registration? GetRegistration(VSInternalClientCapabilities clientCapabilities, DocumentFilter[] filter, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.SupportsVisualStudioExtensions)
        {
            return new Registration
            {
                Method = VSInternalMethods.TextDocumentUriPresentationName,
                RegisterOptions = new TextDocumentRegistrationOptions()
                {
                    DocumentSelector = filter
                }
            };
        }

        return null;
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(VSInternalUriPresentationParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<WorkspaceEdit?> HandleRequestAsync(VSInternalUriPresentationParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(request, context.TextDocument.AssumeNotNull(), cancellationToken);

    private async Task<WorkspaceEdit?> HandleRequestAsync(VSInternalUriPresentationParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var data = await _remoteServiceInvoker.TryInvokeAsync<IRemoteUriPresentationService, IRemoteUriPresentationService.Response>(
                    razorDocument.Project.Solution,
                    (service, solutionInfo, cancellationToken) => service.GetPresentationAsync(solutionInfo, razorDocument.Id, request.Range.ToLinePositionSpan(), request.Uris, cancellationToken),
                    cancellationToken).ConfigureAwait(false);

        // If we got a response back, then we're good to go
        if (data.TextChange is { } textChange)
        {
            var sourceText = await razorDocument.GetTextAsync(cancellationToken).ConfigureAwait(false);

            return new WorkspaceEdit
            {
                DocumentChanges = new TextDocumentEdit[]
                {
                    new TextDocumentEdit
                    {
                        TextDocument = new()
                        {
                            Uri = request.TextDocument.Uri
                        },
                        Edits = [textChange.ToTextEdit(sourceText)]
                    }
                }
            };
        }

        // If we didn't get anything from our logic, we might need to go and ask Html, but we also might have determined not to
        if (!data.CallHtml)
        {
            return null;
        }

        var htmlDocument = await _htmlDocumentSynchronizer.TryGetSynchronizedHtmlDocumentAsync(razorDocument, cancellationToken).ConfigureAwait(false);
        if (htmlDocument is null)
        {
            return null;
        }

        var presentationParams = new VSInternalUriPresentationParams
        {
            Range = request.Range,
            Uris = request.Uris,
            TextDocument = new TextDocumentIdentifier { Uri = htmlDocument.Uri }
        };

        var result = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalUriPresentationParams, WorkspaceEdit?>(
            htmlDocument.Buffer,
            VSInternalMethods.TextDocumentUriPresentationName,
            RazorLSPConstants.HtmlLanguageServerName,
            presentationParams,
            cancellationToken).ConfigureAwait(false);

        // TODO: We _really_ should go back to OOP to remap the response to razor, but the fact is, Razor and Html are 1:1 mappings, so we're
        //       just adjusting Uris, and we don't need OOP for that. If we ever do proper Html mapping then this will not be true.

        if (result?.Response is not { } workspaceEdit)
        {
            return null;
        }

        if (!workspaceEdit.TryGetDocumentChanges(out var edits))
        {
            return null;
        }

        // TODO: We could have a helper service for this, because RazorDocumentMappingService used to do it, but we can't use that in devenv,
        //       but if we move this all to OOP, per the above TODO, then that point is moot.
        foreach (var edit in edits)
        {
            if (_filePathService.IsVirtualHtmlFile(edit.TextDocument.Uri))
            {
                edit.TextDocument = new OptionalVersionedTextDocumentIdentifier { Uri = _filePathService.GetRazorDocumentUri(edit.TextDocument.Uri) };
            }
        }

        return workspaceEdit;
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostUriPresentationEndpoint instance)
    {
        public Task<WorkspaceEdit?> HandleRequestAsync(VSInternalUriPresentationParams request, TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}
