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
    [JsonRpcMethod(CustomMessageNames.RazorValidateBreakpointRangeName, UseSingleObjectParameterDeserialization = true)]
    public async Task<Range?> ValidateBreakpointRangeAsync(DelegatedValidateBreakpointRangeParams request, CancellationToken cancellationToken)
    {
        var delegationDetails = await GetProjectedRequestDetailsAsync(request, cancellationToken).ConfigureAwait(false);
        if (delegationDetails is null)
        {
            return default;
        }

        var validateBreakpointRangeParams = new VSInternalValidateBreakableRangeParams
        {
            TextDocument = request.Identifier.TextDocumentIdentifier.WithUri(delegationDetails.Value.ProjectedUri),
            Range = request.ProjectedRange
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalValidateBreakableRangeParams, Range?>(
            delegationDetails.Value.TextBuffer,
            VSInternalMethods.TextDocumentValidateBreakableRangeName,
            delegationDetails.Value.LanguageServerName,
            validateBreakpointRangeParams,
            cancellationToken).ConfigureAwait(false);
        return response?.Response;
    }
}
