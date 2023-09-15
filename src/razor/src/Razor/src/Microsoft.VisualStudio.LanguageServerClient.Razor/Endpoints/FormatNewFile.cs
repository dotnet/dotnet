// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorFormatNewFileEndpointName, UseSingleObjectParameterDeserialization = true)]
    public async Task<string?> FormatNewFileAsync(FormatNewFileParams request, CancellationToken cancellationToken)
    {
        // This endpoint is special because it deals with a file that doesn't exist yet, so there is no document syncing necessary!
        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<FormatNewFileParams, string?>(
            RazorLSPConstants.RoslynFormatNewFileEndpointName,
            RazorLSPConstants.RazorCSharpLanguageServerName,
            SupportsFormatNewFile,
            request,
            cancellationToken).ConfigureAwait(false);

        return response.Result;
    }

    private static bool SupportsFormatNewFile(JToken token)
    {
        var serverCapabilities = token.ToObject<VSInternalServerCapabilities>();

        return serverCapabilities?.Experimental is JObject experimental
            && experimental.TryGetValue(RazorLSPConstants.RoslynFormatNewFileEndpointName, out var supportsFormatNewFile)
            && supportsFormatNewFile.ToObject<bool>();
    }
}
