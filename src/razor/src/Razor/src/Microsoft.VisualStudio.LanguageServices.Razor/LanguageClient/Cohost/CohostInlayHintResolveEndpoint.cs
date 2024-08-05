// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol.InlayHints;
using Microsoft.CodeAnalysis.Razor.Remote;
using Roslyn.LanguageServer.Protocol;
using VSLSP = Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.InlayHintResolveName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostInlayHintResolveEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal class CohostInlayHintResolveEndpoint(IRemoteServiceInvoker remoteServiceInvoker, ILoggerFactory loggerFactory)
    : AbstractRazorCohostDocumentRequestHandler<InlayHint, InlayHint?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<CohostInlayHintResolveEndpoint>();

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public VSLSP.Registration? GetRegistration(VSLSP.VSInternalClientCapabilities clientCapabilities, VSLSP.DocumentFilter[] filter, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.InlayHint?.DynamicRegistration == true)
        {
            return new VSLSP.Registration
            {
                Method = Methods.TextDocumentInlayHintName,
                RegisterOptions = new VSLSP.InlayHintRegistrationOptions()
                {
                    DocumentSelector = filter
                }
            };
        }

        return null;
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(InlayHint request)
        => GetTextDocumentIdentifier(request)?.ToRazorTextDocumentIdentifier() ?? null;

    private TextDocumentIdentifier? GetTextDocumentIdentifier(InlayHint request)
    {
        var data = GetInlayHintResolveData(request);
        if (data is null)
        {
            _logger.LogError($"Got a resolve request for an inlay hint but couldn't extract the data object. Raw data is: {request.Data}");
            return null;
        }

        return data.TextDocument;
    }

    protected override Task<InlayHint?> HandleRequestAsync(InlayHint request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(request, context.TextDocument.AssumeNotNull(), cancellationToken);

    private async Task<InlayHint?> HandleRequestAsync(InlayHint request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var razorData = GetInlayHintResolveData(request).AssumeNotNull();
        var razorPosition = request.Position;
        request.Data = razorData.OriginalData;
        request.Position = razorData.OriginalPosition;

        var hint = await _remoteServiceInvoker.TryInvokeAsync<IRemoteInlayHintService, InlayHint>(
           razorDocument.Project.Solution,
           (service, solutionInfo, cancellationToken) => service.ResolveHintAsync(solutionInfo, razorDocument.Id, request, cancellationToken),
           cancellationToken).ConfigureAwait(false);

        if (hint is null)
        {
            return null;
        }

        Debug.Assert(request.Position == hint.Position, "Resolving inlay hints should not change the position of them.");
        hint.Position = razorPosition;

        return hint;
    }

    private static InlayHintDataWrapper? GetInlayHintResolveData(InlayHint inlayHint)
    {
        if (inlayHint.Data is InlayHintDataWrapper wrapper)
        {
            return wrapper;
        }

        if (inlayHint.Data is JsonElement json)
        {
            return JsonSerializer.Deserialize<InlayHintDataWrapper>(json);
        }

        return null;
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostInlayHintResolveEndpoint instance)
    {
        public TextDocumentIdentifier? GetTextDocumentIdentifier(InlayHint request)
            => instance.GetTextDocumentIdentifier(request);

        public Task<InlayHint?> HandleRequestAsync(InlayHint request, TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}
