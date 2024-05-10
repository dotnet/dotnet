// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
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
internal class CohostLinkedEditingRangeEndpoint(IRemoteClientProvider remoteClientProvider, ILoggerFactory loggerFactory)
    : AbstractRazorCohostDocumentRequestHandler<LinkedEditingRangeParams, LinkedEditingRanges?>, IDynamicRegistrationProvider
{
    private readonly IRemoteClientProvider _remoteClientProvider = remoteClientProvider;
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

    protected override async Task<LinkedEditingRanges?> HandleRequestAsync(LinkedEditingRangeParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
    {
        var razorDocument = context.TextDocument.AssumeNotNull();

        var remoteClient = await _remoteClientProvider.TryGetClientAsync(cancellationToken).ConfigureAwait(false);
        if (remoteClient is null)
        {
            _logger.LogWarning($"Couldn't get remote client");
            return null;
        }

        try
        {
            var data = await remoteClient.TryInvokeAsync<IRemoteLinkedEditingRangeService, LinePositionSpan[]?>(
                razorDocument.Project.Solution,
                (service, solutionInfo, cancellationToken) => service.GetRangesAsync(solutionInfo, razorDocument.Id, request.Position.ToLinePosition(), cancellationToken),
                cancellationToken).ConfigureAwait(false);

            if (data.Value is { } linkedRanges && linkedRanges.Length == 2)
            {
                var ranges = new Range[2] { linkedRanges[0].ToRange(), linkedRanges[1].ToRange() };

                return new LinkedEditingRanges
                {
                    Ranges = ranges,
                    WordPattern = LinkedEditingRangeHelper.WordPattern
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error calling remote");
            return null;
        }

        return null;
    }
}
