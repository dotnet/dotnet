// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Test;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class WorkspaceSemanticTokensRefreshPublisherTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public void PublishWorkspaceChanged_DoesNotSendWorkspaceRefreshRequest_WhenNotSupported()
    {
        // Arrange
        var clientCapabilitiesService = GetClientCapabilitiesService(semanticRefreshEnabled: false);
        var serverClient = new TestClient();
        var errorReporter = new TestErrorReporter();
        var optionsMonitor = GetOptionsMonitor();
        using var defaultWorkspaceChangedPublisher = new WorkspaceSemanticTokensRefreshPublisher(clientCapabilitiesService, serverClient, errorReporter, optionsMonitor);
        var testAccessor = defaultWorkspaceChangedPublisher.GetTestAccessor();

        // Act
        defaultWorkspaceChangedPublisher.EnqueueWorkspaceSemanticTokensRefresh();
        testAccessor.WaitForEmpty();

        // Assert
        Assert.Empty(serverClient.Requests);
    }

    [Fact]
    public void PublishWorkspaceChanged_SendsWorkspaceRefreshRequest_WhenSupported()
    {
        // Arrange
        var clientCapabilitiesService = GetClientCapabilitiesService(semanticRefreshEnabled: true);
        var serverClient = new TestClient();
        var errorReporter = new TestErrorReporter();
        var optionsMonitor = GetOptionsMonitor();
        using var defaultWorkspaceChangedPublisher = new WorkspaceSemanticTokensRefreshPublisher(clientCapabilitiesService, serverClient, errorReporter, optionsMonitor);
        var testAccessor = defaultWorkspaceChangedPublisher.GetTestAccessor();

        // Act
        defaultWorkspaceChangedPublisher.EnqueueWorkspaceSemanticTokensRefresh();
        testAccessor.WaitForEmpty();

        // Assert
        Assert.Collection(serverClient.Requests,
            r => Assert.Equal(Methods.WorkspaceSemanticTokensRefreshName, r.Method));
    }

    [Fact]
    public void PublishWorkspaceChanged_DebouncesWorkspaceRefreshRequest()
    {
        // Arrange
        var clientCapabilitiesService = GetClientCapabilitiesService(semanticRefreshEnabled: true);
        var serverClient = new TestClient();
        var errorReporter = new TestErrorReporter();
        var optionsMonitor = GetOptionsMonitor();
        using var defaultWorkspaceChangedPublisher = new WorkspaceSemanticTokensRefreshPublisher(clientCapabilitiesService, serverClient, errorReporter, optionsMonitor);
        var testAccessor = defaultWorkspaceChangedPublisher.GetTestAccessor();

        // Act
        defaultWorkspaceChangedPublisher.EnqueueWorkspaceSemanticTokensRefresh();
        defaultWorkspaceChangedPublisher.EnqueueWorkspaceSemanticTokensRefresh();
        testAccessor.WaitForEmpty();
        defaultWorkspaceChangedPublisher.EnqueueWorkspaceSemanticTokensRefresh();
        defaultWorkspaceChangedPublisher.EnqueueWorkspaceSemanticTokensRefresh();
        testAccessor.WaitForEmpty();

        // Assert
        Assert.Collection(serverClient.Requests,
            r => Assert.Equal(Methods.WorkspaceSemanticTokensRefreshName, r.Method),
            r => Assert.Equal(Methods.WorkspaceSemanticTokensRefreshName, r.Method));
    }

    [Fact]
    public async Task PublishWorkspaceChanged_SendsWorkspaceRefreshRequest_WhenOptionChanges()
    {
        // Arrange
        var clientCapabilitiesService = GetClientCapabilitiesService(semanticRefreshEnabled: true);
        var serverClient = new TestClient();
        var errorReporter = new TestErrorReporter();
        var optionsMonitor = GetOptionsMonitor(withCSharpBackground: true);
        using var defaultWorkspaceChangedPublisher = new WorkspaceSemanticTokensRefreshPublisher(clientCapabilitiesService, serverClient, errorReporter, optionsMonitor);
        var testAccessor = defaultWorkspaceChangedPublisher.GetTestAccessor();

        // Act
        await optionsMonitor.UpdateAsync(DisposalToken);
        testAccessor.WaitForEmpty();

        // Assert
        Assert.Collection(serverClient.Requests,
            r => Assert.Equal(Methods.WorkspaceSemanticTokensRefreshName, r.Method));
    }

    private static RazorLSPOptionsMonitor GetOptionsMonitor(bool withCSharpBackground = false)
    {
        var configurationSyncService = new Mock<IConfigurationSyncService>(MockBehavior.Strict);

        var options = RazorLSPOptions.Default with { ColorBackground = withCSharpBackground };
        configurationSyncService
            .Setup(c => c.GetLatestOptionsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<RazorLSPOptions?>(options));

        var optionsMonitorCache = new OptionsCache<RazorLSPOptions>();

        var optionsMonitor = TestRazorLSPOptionsMonitor.Create(
            configurationSyncService.Object,
            optionsMonitorCache);

        return optionsMonitor;
    }

    private static IClientCapabilitiesService GetClientCapabilitiesService(bool semanticRefreshEnabled)
    {
        var clientCapabilities = new VSInternalClientCapabilities
        {
            Workspace = new WorkspaceClientCapabilities
            {
                SemanticTokens = new SemanticTokensWorkspaceSetting
                {
                    RefreshSupport = semanticRefreshEnabled
                },
            }
        };

        return new TestClientCapabilitiesService(clientCapabilities);
    }

    private class TestErrorReporter : IErrorReporter
    {
        public void ReportError(Exception exception) => throw new NotImplementedException();
        public void ReportError(Exception exception, IProjectSnapshot? project) => throw new NotImplementedException();
        public void ReportError(Exception exception, Project workspaceProject) => throw new NotImplementedException();
    }
}
