// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol.SemanticTokens;

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
