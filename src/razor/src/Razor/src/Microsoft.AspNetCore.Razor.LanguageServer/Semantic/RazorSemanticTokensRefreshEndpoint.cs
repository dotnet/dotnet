// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

[RazorLanguageServerEndpoint(CustomMessageNames.RazorSemanticTokensRefreshEndpoint)]
internal sealed class RazorSemanticTokensRefreshEndpoint(IWorkspaceSemanticTokensRefreshNotifier semanticTokensRefreshPublisher) : IRazorNotificationHandler<SemanticTokensRefreshParams>
{
    private readonly IWorkspaceSemanticTokensRefreshNotifier _semanticTokensRefreshPublisher = semanticTokensRefreshPublisher;

    public bool MutatesSolutionState { get; } = false;

    public Task HandleNotificationAsync(SemanticTokensRefreshParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        // We have to invalidate the tokens cache since it may no longer be up to date.
        _semanticTokensRefreshPublisher.NotifyWorkspaceSemanticTokensRefresh();

        return Task.CompletedTask;
    }
}
