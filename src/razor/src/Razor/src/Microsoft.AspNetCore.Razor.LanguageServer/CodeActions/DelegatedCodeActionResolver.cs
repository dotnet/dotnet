// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal sealed class DelegatedCodeActionResolver(IClientConnection clientConnection) : IDelegatedCodeActionResolver
{
    private readonly IClientConnection _clientConnection = clientConnection;

    public async Task<CodeAction?> ResolveCodeActionAsync(TextDocumentIdentifier razorFileIdentifier, int hostDocumentVersion, RazorLanguageKind languageKind, CodeAction codeAction, CancellationToken cancellationToken)
    {
        var resolveCodeActionParams = new RazorResolveCodeActionParams(razorFileIdentifier, hostDocumentVersion, languageKind, codeAction);

        var resolvedCodeAction = await _clientConnection.SendRequestAsync<RazorResolveCodeActionParams, CodeAction?>(
            CustomMessageNames.RazorResolveCodeActionsEndpoint,
            resolveCodeActionParams,
            cancellationToken).ConfigureAwait(false);

        return resolvedCodeAction;
    }
}
