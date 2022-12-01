// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Semantic;

public class SemanticTokensRefreshEndpointTest : TestBase
{
    public SemanticTokensRefreshEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task Handle_QueuesRefresh()
    {
        // Arrange
        var clientSettings = GetInitializedParams(semanticRefreshEnabled: true);
        var clientSettingsManager = new Mock<IInitializeManager<InitializeParams, InitializeResult>>(MockBehavior.Strict);
        clientSettingsManager.Setup(m => m.GetInitializeParams()).Returns(clientSettings);
        var serverClient = new TestClient();
        var errorReporter = new TestErrorReporter();
        using var semanticTokensRefreshPublisher = new DefaultWorkspaceSemanticTokensRefreshPublisher(clientSettingsManager.Object, serverClient, errorReporter);
        var refreshEndpoint = new SemanticTokensRefreshEndpoint(semanticTokensRefreshPublisher);
        var refreshParams = new SemanticTokensRefreshParams();
        var requestContext = new RazorRequestContext();

        // Act
        await refreshEndpoint.HandleNotificationAsync(refreshParams, requestContext, DisposalToken);
        semanticTokensRefreshPublisher.GetTestAccessor().WaitForEmpty();

        // Assert
        Assert.Equal(Methods.WorkspaceSemanticTokensRefreshName, serverClient.Requests.Single().Method);
    }

    private static InitializeParams GetInitializedParams(bool semanticRefreshEnabled)
    {
        return new InitializeParams
        {
            Capabilities = new ClientCapabilities
            {
                Workspace = new WorkspaceClientCapabilities
                {
                    SemanticTokens = new SemanticTokensWorkspaceSetting
                    {
                        RefreshSupport = semanticRefreshEnabled
                    },
                },
            },
        };
    }

    private class TestErrorReporter : ErrorReporter
    {
        public override void ReportError(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override void ReportError(Exception exception, ProjectSnapshot? project)
        {
            throw new NotImplementedException();
        }

        public override void ReportError(Exception exception, Project workspaceProject)
        {
            throw new NotImplementedException();
        }
    }
}
