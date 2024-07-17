// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.LinkedEditingRange;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.LanguageClient.Extensions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentLinkedEditingRangeName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostLinkedEditingRangeEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal class CohostLinkedEditingRangeEndpoint(IRemoteServiceInvoker remoteServiceInvoker, ILoggerFactory loggerFactory)
    : AbstractRazorCohostDocumentRequestHandler<LinkedEditingRangeParams, LinkedEditingRanges?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<CohostLinkedEditingRangeEndpoint>();

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public Registration? GetRegistration(VSInternalClientCapabilities clientCapabilities, DocumentFilter[] filter, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.LinkedEditingRange?.DynamicRegistration == true)
        {
            return new Registration
            {
                Method = Methods.TextDocumentLinkedEditingRangeName,
                RegisterOptions = new LinkedEditingRangeRegistrationOptions()
                {
                    DocumentSelector = filter
                }
            };
        }

        return null;
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(LinkedEditingRangeParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<LinkedEditingRanges?> HandleRequestAsync(LinkedEditingRangeParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(request, context.TextDocument.AssumeNotNull(), cancellationToken);

    private async Task<LinkedEditingRanges?> HandleRequestAsync(LinkedEditingRangeParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var linkedRanges = await _remoteServiceInvoker.TryInvokeAsync<IRemoteLinkedEditingRangeService, LinePositionSpan[]?>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetRangesAsync(solutionInfo, razorDocument.Id, request.Position.ToLinePosition(), cancellationToken),
            cancellationToken).ConfigureAwait(false);

        if (linkedRanges is [{ } span1, { } span2])
        {
            return new LinkedEditingRanges
            {
                Ranges = [span1.ToRange(), span2.ToRange()],
                WordPattern = LinkedEditingRangeHelper.WordPattern
            };
        }

        return null;
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostLinkedEditingRangeEndpoint instance)
    {
        public Task<LinkedEditingRanges?> HandleRequestAsync(LinkedEditingRangeParams request, TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}
