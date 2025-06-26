// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentColorPresentationName)]
[ExportRazorStatelessLspService(typeof(CohostColorPresentationEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostColorPresentationEndpoint(
    IHtmlRequestInvoker requestInvoker)
    : AbstractRazorCohostDocumentRequestHandler<ColorPresentationParams, ColorPresentation[]?>
{
    private readonly IHtmlRequestInvoker _requestInvoker = requestInvoker;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(ColorPresentationParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<ColorPresentation[]?> HandleRequestAsync(ColorPresentationParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(request, context.TextDocument.AssumeNotNull(), cancellationToken);

    private async Task<ColorPresentation[]?> HandleRequestAsync(ColorPresentationParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        return await _requestInvoker.MakeHtmlLspRequestAsync<ColorPresentationParams, ColorPresentation[]>(
            razorDocument,
            Methods.TextDocumentColorPresentationName,
            request,
            cancellationToken).ConfigureAwait(false);
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostColorPresentationEndpoint instance)
    {
        public Task<ColorPresentation[]?> HandleRequestAsync(ColorPresentationParams request, TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}
