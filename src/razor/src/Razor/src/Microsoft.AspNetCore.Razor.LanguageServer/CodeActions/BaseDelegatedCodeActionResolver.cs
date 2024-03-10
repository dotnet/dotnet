// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal abstract class BaseDelegatedCodeActionResolver : ICodeActionResolver
{
    protected readonly IClientConnection ClientConnection;

    public BaseDelegatedCodeActionResolver(IClientConnection clientConnection)
    {
        ClientConnection = clientConnection ?? throw new ArgumentNullException(nameof(clientConnection));
    }

    public abstract string Action { get; }

    public abstract Task<CodeAction> ResolveAsync(CodeActionResolveParams resolveParams, CodeAction codeAction, CancellationToken cancellationToken);

    protected async Task<CodeAction?> ResolveCodeActionWithServerAsync(TextDocumentIdentifier razorFileIdentifier, int hostDocumentVersion, RazorLanguageKind languageKind, CodeAction codeAction, CancellationToken cancellationToken)
    {
        var resolveCodeActionParams = new RazorResolveCodeActionParams(razorFileIdentifier, hostDocumentVersion, languageKind, codeAction);

        var resolvedCodeAction = await ClientConnection.SendRequestAsync<RazorResolveCodeActionParams, CodeAction?>(
            CustomMessageNames.RazorResolveCodeActionsEndpoint,
            resolveCodeActionParams,
            cancellationToken).ConfigureAwait(false);

        return resolvedCodeAction;
    }
}
