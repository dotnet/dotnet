// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Test;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class WorkspaceSemanticTokensRefreshNotifierTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task PublishWorkspaceChanged_DoesNotSendWorkspaceRefreshRequest_WhenNotSupported()
    {
        // Arrange
        var clientCapabilitiesService = GetClientCapabilitiesService(semanticRefreshEnabled: false);
        var serverClient = new TestClient();
        var optionsMonitor = GetOptionsMonitor();
        using var notifier = new WorkspaceSemanticTokensRefreshNotifier(clientCapabilitiesService, serverClient, optionsMonitor);
        var testAccessor = notifier.GetTestAccessor();

        // Act
        notifier.NotifyWorkspaceSemanticTokensRefresh();
        await testAccessor.WaitForNotificationAsync();

        // Assert
        Assert.Empty(serverClient.Requests);
    }

    [Fact]
    public async Task PublishWorkspaceChanged_SendsWorkspaceRefreshRequest_WhenSupported()
    {
        // Arrange
        var clientCapabilitiesService = GetClientCapabilitiesService(semanticRefreshEnabled: true);
        var serverClient = new TestClient();
        var optionsMonitor = GetOptionsMonitor();
        using var notifier = new WorkspaceSemanticTokensRefreshNotifier(clientCapabilitiesService, serverClient, optionsMonitor);
        var testAccessor = notifier.GetTestAccessor();

        // Act
        notifier.NotifyWorkspaceSemanticTokensRefresh();
        await testAccessor.WaitForNotificationAsync();

        // Assert
        var request = Assert.Single(serverClient.Requests);
        Assert.Equal(Methods.WorkspaceSemanticTokensRefreshName, request.Method);
    }

    [Fact]
    public async Task PublishWorkspaceChanged_DebouncesWorkspaceRefreshRequest()
    {
        // Arrange
        var clientCapabilitiesService = GetClientCapabilitiesService(semanticRefreshEnabled: true);
        var serverClient = new TestClient();
        var optionsMonitor = GetOptionsMonitor();
        using var notifier = new WorkspaceSemanticTokensRefreshNotifier(clientCapabilitiesService, serverClient, optionsMonitor);
        var testAccessor = notifier.GetTestAccessor();

        // Act
        notifier.NotifyWorkspaceSemanticTokensRefresh();
        notifier.NotifyWorkspaceSemanticTokensRefresh();
        await testAccessor.WaitForNotificationAsync();
        notifier.NotifyWorkspaceSemanticTokensRefresh();
        notifier.NotifyWorkspaceSemanticTokensRefresh();
        await testAccessor.WaitForNotificationAsync();

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
        var optionsMonitor = GetOptionsMonitor(withCSharpBackground: true);
        using var notifier = new WorkspaceSemanticTokensRefreshNotifier(clientCapabilitiesService, serverClient, optionsMonitor);
        var testAccessor = notifier.GetTestAccessor();

        // Act
        await optionsMonitor.UpdateAsync(DisposalToken);
        await testAccessor.WaitForNotificationAsync();

        // Assert
        var request = Assert.Single(serverClient.Requests);
        Assert.Equal(Methods.WorkspaceSemanticTokensRefreshName, request.Method);
    }

    private static TestRazorLSPOptionsMonitor GetOptionsMonitor(bool withCSharpBackground = false)
    {
        var configurationSyncService = new StrictMock<IConfigurationSyncService>();

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

    private static TestClientCapabilitiesService GetClientCapabilitiesService(bool semanticRefreshEnabled)
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
}
