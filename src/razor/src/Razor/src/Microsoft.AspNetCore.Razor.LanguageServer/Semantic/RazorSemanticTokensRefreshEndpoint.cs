// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

[LanguageServerEndpoint(CustomMessageNames.RazorSemanticTokensRefreshEndpoint)]
internal sealed class RazorSemanticTokensRefreshEndpoint : IRazorNotificationHandler<SemanticTokensRefreshParams>
{
    private readonly WorkspaceSemanticTokensRefreshPublisher _semanticTokensRefreshPublisher;

    public bool MutatesSolutionState { get; } = false;

    public RazorSemanticTokensRefreshEndpoint(WorkspaceSemanticTokensRefreshPublisher semanticTokensRefreshPublisher)
    {
        _semanticTokensRefreshPublisher = semanticTokensRefreshPublisher ?? throw new ArgumentNullException(nameof(semanticTokensRefreshPublisher));
    }

    public Task HandleNotificationAsync(SemanticTokensRefreshParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        // We have to invalidate the tokens cache since it may no longer be up to date.
        _semanticTokensRefreshPublisher.EnqueueWorkspaceSemanticTokensRefresh();

        return Task.CompletedTask;
    }
}
