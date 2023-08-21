// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage.MessageInterception;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Utilities;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Export(typeof(MessageInterceptor))]
[InterceptMethod(Methods.WorkspaceSemanticTokensRefreshName)]
[ContentType(RazorLSPConstants.CSharpContentTypeName)]
internal class RazorCSharpSemanticTokensInterceptor : MessageInterceptor
{
    private readonly LSPRequestInvoker _requestInvoker;

    [ImportingConstructor]
    public RazorCSharpSemanticTokensInterceptor(LSPRequestInvoker requestInvoker)
    {
        if (requestInvoker is null)
        {
            throw new ArgumentNullException(nameof(requestInvoker));
        }

        _requestInvoker = requestInvoker;
    }

    public async override Task<InterceptionResult> ApplyChangesAsync(
        JToken message, string containedLanguageName, CancellationToken cancellationToken)
    {
        var refreshParams = new SemanticTokensRefreshParams();
        await _requestInvoker.ReinvokeRequestOnServerAsync<SemanticTokensRefreshParams, Unit>(
            CustomMessageNames.RazorSemanticTokensRefreshEndpoint,
            RazorLSPConstants.RazorLanguageServerName,
            refreshParams,
            cancellationToken).ConfigureAwait(false);

        return InterceptionResult.NoChange;
    }

    // A basic POCO which will handle the lack of data in the response.
    private class Unit
    {

    }
}
