// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal class SemanticTokensRefreshEndpoint : ISemanticTokensRefreshEndpoint
{
    private readonly WorkspaceSemanticTokensRefreshPublisher _semanticTokensRefreshPublisher;

    public bool MutatesSolutionState { get; } = false;

    public SemanticTokensRefreshEndpoint(WorkspaceSemanticTokensRefreshPublisher semanticTokensRefreshPublisher)
    {
        if (semanticTokensRefreshPublisher is null)
        {
            throw new ArgumentNullException(nameof(semanticTokensRefreshPublisher));
        }

        _semanticTokensRefreshPublisher = semanticTokensRefreshPublisher;
    }

    public Task HandleNotificationAsync(SemanticTokensRefreshParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        // We have to invalidate the tokens cache since it may no longer be up to date.
        _semanticTokensRefreshPublisher.EnqueueWorkspaceSemanticTokensRefresh();

        return Task.CompletedTask;
    }

    public RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities)
    {
        const string ServerCapability = "workspace.semanticTokens";

        return new RegistrationExtensionResult(ServerCapability, new SemanticTokensWorkspaceSetting
        {
            RefreshSupport = true,
        });
    }
}
