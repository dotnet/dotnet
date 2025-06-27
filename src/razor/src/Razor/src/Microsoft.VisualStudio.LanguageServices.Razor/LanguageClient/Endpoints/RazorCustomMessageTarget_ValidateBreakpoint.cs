// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorValidateBreakpointRangeName, UseSingleObjectParameterDeserialization = true)]
    public async Task<LspRange?> ValidateBreakpointRangeAsync(DelegatedValidateBreakpointRangeParams request, CancellationToken cancellationToken)
    {
        var delegationDetails = await GetProjectedRequestDetailsAsync(request, cancellationToken).ConfigureAwait(false);
        if (delegationDetails is null)
        {
            // Normally when things are out of sync (eg C# is ahead due to fast typing) we return null so the LSP client will
            // ask us again. For breakpoint range though, null means "remove this breakpoint" which is bad for the user. Instead
            // we signal to our server what is going on with an undefined range.
            return LspFactory.UndefinedRange;
        }

        var validateBreakpointRangeParams = new VSInternalValidateBreakableRangeParams
        {
            TextDocument = request.Identifier.TextDocumentIdentifier.WithUri(delegationDetails.Value.ProjectedUri),
            Range = request.ProjectedRange
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalValidateBreakableRangeParams, LspRange?>(
            delegationDetails.Value.TextBuffer,
            VSInternalMethods.TextDocumentValidateBreakableRangeName,
            delegationDetails.Value.LanguageServerName,
            validateBreakpointRangeParams,
            cancellationToken).ConfigureAwait(false);
        return response?.Response;
    }
}
