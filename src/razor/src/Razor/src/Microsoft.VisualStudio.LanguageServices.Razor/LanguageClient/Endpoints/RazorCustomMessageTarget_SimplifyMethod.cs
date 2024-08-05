// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorSimplifyMethodEndpointName, UseSingleObjectParameterDeserialization = true)]
    public async Task<TextEdit[]?> SimplifyTypeAsync(DelegatedSimplifyMethodParams request, CancellationToken cancellationToken)
    {
        var identifier = request.Identifier.TextDocumentIdentifier;
        if (request.RequiresVirtualDocument)
        {
            var (synchronized, virtualDocument) = await TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                request.Identifier.Version,
                request.Identifier.TextDocumentIdentifier,
                cancellationToken).ConfigureAwait(false);
            if (!synchronized || virtualDocument is null)
            {
                return null;
            }

            identifier = identifier.WithUri(virtualDocument.Uri);
        }

        var simplifyTypeNamesParams = new SimplifyMethodParams()
        {
            TextDocument = identifier,
            TextEdit = request.TextEdit
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<SimplifyMethodParams, TextEdit[]?>(
            RazorLSPConstants.RoslynSimplifyMethodEndpointName,
            RazorLSPConstants.RazorCSharpLanguageServerName,
            simplifyTypeNamesParams,
            cancellationToken).ConfigureAwait(false);

        return response.Result;
    }
}
