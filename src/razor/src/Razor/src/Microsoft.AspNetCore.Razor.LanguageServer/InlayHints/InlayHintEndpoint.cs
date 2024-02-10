// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.InlayHints;

[LanguageServerEndpoint(Methods.TextDocumentInlayHintName)]
internal sealed class InlayHintEndpoint(LanguageServerFeatureOptions featureOptions, IInlayHintService inlayHintService, IClientConnection clientConnection)
    : IRazorRequestHandler<InlayHintParams, InlayHint[]?>, ICapabilitiesProvider
{
    private readonly LanguageServerFeatureOptions _featureOptions = featureOptions;
    private readonly IInlayHintService _inlayHintService = inlayHintService;
    private readonly IClientConnection _clientConnection = clientConnection;

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.EnableInlayHints();
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(InlayHintParams request)
        => request.TextDocument;

    public Task<InlayHint[]?> HandleRequestAsync(InlayHintParams request, RazorRequestContext context, CancellationToken cancellationToken)
    {
        var documentContext = context.GetRequiredDocumentContext();
        return _inlayHintService.GetInlayHintsAsync(_clientConnection, documentContext, request.Range, cancellationToken);
    }
}
