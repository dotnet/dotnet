// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorSpellCheckEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<VSInternalSpellCheckableRangeReport[]> SpellCheckAsync(DelegatedSpellCheckParams request, CancellationToken cancellationToken)
    {
        var hostDocument = request.Identifier.TextDocumentIdentifier;
        var (synchronized, virtualDocument) = await TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
            request.Identifier.Version,
            hostDocument,
            cancellationToken).ConfigureAwait(false);
        if (!synchronized || virtualDocument is null)
        {
            return Array.Empty<VSInternalSpellCheckableRangeReport>();
        }

        var spellCheckParams = new VSInternalDocumentSpellCheckableParams
        {
            TextDocument = hostDocument.WithUri(virtualDocument.Uri),
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalDocumentSpellCheckableParams, VSInternalSpellCheckableRangeReport[]>(
            virtualDocument.Snapshot.TextBuffer,
            VSInternalMethods.TextDocumentSpellCheckableRangesName,
            RazorLSPConstants.RazorCSharpLanguageServerName,
            spellCheckParams,
            cancellationToken).ConfigureAwait(false);

        return response?.Response ?? Array.Empty<VSInternalSpellCheckableRangeReport>();
    }
}
