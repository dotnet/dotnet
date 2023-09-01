// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

public class RazorSemanticTokensRefreshEndpointTest : TestBase
{
    public RazorSemanticTokensRefreshEndpointTest(ITestOutputHelper testOutput)
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
        var optionsMonitor = GetOptionsMonitor();
        using var semanticTokensRefreshPublisher = new DefaultWorkspaceSemanticTokensRefreshPublisher(clientSettingsManager.Object, serverClient, errorReporter, optionsMonitor);
        var refreshEndpoint = new RazorSemanticTokensRefreshEndpoint(semanticTokensRefreshPublisher);
        var refreshParams = new SemanticTokensRefreshParams();
        var requestContext = new RazorRequestContext();

        // Act
        await refreshEndpoint.HandleNotificationAsync(refreshParams, requestContext, DisposalToken);
        semanticTokensRefreshPublisher.GetTestAccessor().WaitForEmpty();

        // Assert
        Assert.Equal(Methods.WorkspaceSemanticTokensRefreshName, serverClient.Requests.Single().Method);
    }

    private static RazorLSPOptionsMonitor GetOptionsMonitor()
    {
        var configurationSyncService = new Mock<IConfigurationSyncService>(MockBehavior.Strict);

        var options = RazorLSPOptions.Default;
        configurationSyncService
            .Setup(c => c.GetLatestOptionsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<RazorLSPOptions?>(options));

        var optionsMonitorCache = new OptionsCache<RazorLSPOptions>();

        var optionsMonitor = TestRazorLSPOptionsMonitor.Create(
            configurationSyncService.Object,
            optionsMonitorCache);

        return optionsMonitor;
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

    private class TestErrorReporter : IErrorReporter
    {
        public void ReportError(Exception exception) => throw new NotImplementedException();
        public void ReportError(Exception exception, IProjectSnapshot? project) => throw new NotImplementedException();
        public void ReportError(Exception exception, Project workspaceProject) => throw new NotImplementedException();
    }
}
