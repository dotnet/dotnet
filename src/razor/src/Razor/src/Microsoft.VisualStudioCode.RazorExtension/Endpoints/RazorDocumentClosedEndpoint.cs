// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;
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
        _htmlDocumentSynchronizer.DocumentRemoved(requestContext.Uri.AssumeNotNull());
        return SpecializedTasks.Default<VoidResult>();
    }
}
