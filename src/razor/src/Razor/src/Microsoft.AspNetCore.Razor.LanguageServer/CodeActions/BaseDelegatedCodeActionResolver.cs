// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal abstract class BaseDelegatedCodeActionResolver : BaseCodeActionResolver
{
    protected readonly ClientNotifierServiceBase LanguageServer;

    public BaseDelegatedCodeActionResolver(ClientNotifierServiceBase languageServer)
    {
        if (languageServer is null)
        {
            throw new ArgumentNullException(nameof(languageServer));
        }

        LanguageServer = languageServer;
    }

    public abstract Task<CodeAction> ResolveAsync(CodeActionResolveParams resolveParams, CodeAction codeAction, CancellationToken cancellationToken);

    protected async Task<CodeAction?> ResolveCodeActionWithServerAsync(Uri razorFileUri, int hostDocumentVersion, RazorLanguageKind languageKind, CodeAction codeAction, CancellationToken cancellationToken)
    {
        var resolveCodeActionParams = new RazorResolveCodeActionParams(razorFileUri, hostDocumentVersion, languageKind, codeAction);

        var resolvedCodeAction = await LanguageServer.SendRequestAsync<RazorResolveCodeActionParams, CodeAction?>(
            RazorLanguageServerCustomMessageTargets.RazorResolveCodeActionsEndpoint,
            resolveCodeActionParams,
            cancellationToken).ConfigureAwait(false);

        return resolvedCodeAction;
    }
}
