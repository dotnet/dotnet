// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

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
        if (!synchronized)
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
            SupportsSpellCheck,
            spellCheckParams,
            cancellationToken).ConfigureAwait(false);

        return response?.Response ?? Array.Empty<VSInternalSpellCheckableRangeReport>();
    }

    private static bool SupportsSpellCheck(JToken token)
    {
        var serverCapabilities = token.ToObject<VSInternalServerCapabilities>();

        return serverCapabilities?.SpellCheckingProvider ?? false;
    }
}
