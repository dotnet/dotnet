// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorFormatNewFileEndpointName, UseSingleObjectParameterDeserialization = true)]
    public async Task<string?> FormatNewFileAsync(FormatNewFileParams request, CancellationToken cancellationToken)
    {
        // This endpoint is special because it deals with a file that doesn't exist yet, so there is no document syncing necessary!
        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<FormatNewFileParams, string?>(
            RazorLSPConstants.RoslynFormatNewFileEndpointName,
            RazorLSPConstants.RazorCSharpLanguageServerName,
            request,
            cancellationToken).ConfigureAwait(false);

        return response.Result;
    }
}
