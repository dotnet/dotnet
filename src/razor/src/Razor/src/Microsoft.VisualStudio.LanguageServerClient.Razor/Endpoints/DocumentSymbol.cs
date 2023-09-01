// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

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

        if (!synchronized)
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
