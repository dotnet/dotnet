// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

namespace Microsoft.VisualStudioCode.RazorExtension.Endpoints;

[Shared]
[ExportRazorStatelessLspService(typeof(RazorDocumentClosedEndpoint))]
[RazorEndpoint("razor/documentClosed")]
[method: ImportingConstructor]
internal class RazorDocumentClosedEndpoint(IHtmlDocumentSynchronizer htmlDocumentSynchronizer) : AbstractRazorCohostDocumentRequestHandler<TextDocumentIdentifier, VoidResult>
{
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(TextDocumentIdentifier request)
        => request.ToRazorTextDocumentIdentifier();

    protected override Task<VoidResult> HandleRequestAsync(TextDocumentIdentifier textDocument, RazorCohostRequestContext requestContext, CancellationToken cancellationToken)
    {
        _htmlDocumentSynchronizer.DocumentRemoved(requestContext.DocumentUri.AssumeNotNull().GetRequiredParsedUri(), cancellationToken);
        return SpecializedTasks.Default<VoidResult>();
    }
}
