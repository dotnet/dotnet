// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

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
            if (!synchronized)
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
            SupportsSimplifyMethod,
            simplifyTypeNamesParams,
            cancellationToken).ConfigureAwait(false);

        return response.Result;
    }

    private static bool SupportsSimplifyMethod(JToken token)
    {
        var serverCapabilities = token.ToObject<VSInternalServerCapabilities>();

        return serverCapabilities?.Experimental is JObject experimental
            && experimental.TryGetValue(RazorLSPConstants.RoslynSimplifyMethodEndpointName, out var supportsSimplifyMethod)
            && supportsSimplifyMethod.ToObject<bool>();
    }
}
