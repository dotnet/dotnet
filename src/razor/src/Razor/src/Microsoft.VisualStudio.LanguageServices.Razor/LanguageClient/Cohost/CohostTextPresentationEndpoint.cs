// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(VSInternalMethods.TextDocumentTextPresentationName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostTextPresentationEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal class CohostTextPresentationEndpoint(
    IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
    IFilePathService filePathService,
    LSPRequestInvoker requestInvoker)
    : AbstractRazorCohostDocumentRequestHandler<VSInternalTextPresentationParams, WorkspaceEdit?>, IDynamicRegistrationProvider
{
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
    private readonly IFilePathService _filePathService = filePathService;
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.SupportsVisualStudioExtensions)
        {
            return [new Registration
            {
                Method = VSInternalMethods.TextDocumentTextPresentationName,
                RegisterOptions = new TextDocumentRegistrationOptions()
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(VSInternalTextPresentationParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<WorkspaceEdit?> HandleRequestAsync(VSInternalTextPresentationParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(request, context.TextDocument.AssumeNotNull(), cancellationToken);

    private async Task<WorkspaceEdit?> HandleRequestAsync(VSInternalTextPresentationParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var htmlDocument = await _htmlDocumentSynchronizer.TryGetSynchronizedHtmlDocumentAsync(razorDocument, cancellationToken).ConfigureAwait(false);
        if (htmlDocument is null)
        {
            return null;
        }

        request.TextDocument = request.TextDocument.WithUri(htmlDocument.Uri);

        var result = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalTextPresentationParams, WorkspaceEdit?>(
            htmlDocument.Buffer,
            VSInternalMethods.TextDocumentTextPresentationName,
            RazorLSPConstants.HtmlLanguageServerName,
            request,
            cancellationToken).ConfigureAwait(false);

        if (result?.Response is not { } workspaceEdit)
        {
            return null;
        }

        if (!workspaceEdit.TryGetTextDocumentEdits(out var edits))
        {
            return null;
        }

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

    internal readonly struct TestAccessor(CohostTextPresentationEndpoint instance)
    {
        public Task<WorkspaceEdit?> HandleRequestAsync(VSInternalTextPresentationParams request, TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}
