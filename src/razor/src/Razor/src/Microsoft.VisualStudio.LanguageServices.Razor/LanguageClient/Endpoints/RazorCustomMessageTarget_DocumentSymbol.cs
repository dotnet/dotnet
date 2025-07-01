// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorDocumentSymbolEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<SumType<DocumentSymbol[], SymbolInformation[]>?> DocumentSymbolsAsync(DelegatedDocumentSymbolParams request, CancellationToken cancellationToken)
    {
        var hostDocument = request.Identifier.TextDocumentIdentifier;
        var (synchronized, virtualDocument) = await TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
            request.Identifier.Version,
            hostDocument,
        cancellationToken).ConfigureAwait(false);

        if (!synchronized || virtualDocument is null)
        {
            return null;
        }

        var documentSymbolParams = new DocumentSymbolParams()
        {
            TextDocument = hostDocument.WithUri(virtualDocument.Uri)
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<DocumentSymbolParams, SumType<DocumentSymbol[], SymbolInformation[]>?>(
            virtualDocument.Snapshot.TextBuffer,
            Methods.TextDocumentDocumentSymbolName,
            RazorLSPConstants.RazorCSharpLanguageServerName,
            documentSymbolParams,
            cancellationToken).ConfigureAwait(false);

        return response?.Response;
    }
}
