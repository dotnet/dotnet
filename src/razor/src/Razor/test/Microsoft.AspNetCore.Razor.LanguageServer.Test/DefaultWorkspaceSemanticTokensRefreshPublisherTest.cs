// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class DefaultWorkspaceSemanticTokensRefreshPublisherTest : LanguageServerTestBase
{
    public DefaultWorkspaceSemanticTokensRefreshPublisherTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void PublishWorkspaceChanged_DoesNotSendWorkspaceRefreshRequest_WhenNotSupported()
    {
        // Arrange
        var settingManager = GetServerSettingsManager(semanticRefreshEnabled: false);
        var serverClient = new TestClient();
        var errorReporter = new TestErrorReporter();
        using var defaultWorkspaceChangedPublisher = new DefaultWorkspaceSemanticTokensRefreshPublisher(settingManager, serverClient, errorReporter);
        var testAccessor = defaultWorkspaceChangedPublisher.GetTestAccessor();

        // Act
        defaultWorkspaceChangedPublisher.EnqueueWorkspaceSemanticTokensRefresh();
        testAccessor.WaitForEmpty();

        // Assert
        Assert.Equal(0, serverClient.Requests.Count);
    }

    [Fact]
    public void PublishWorkspaceChanged_SendsWorkspaceRefreshRequest_WhenSupported()
    {
        // Arrange
        var settingManager = GetServerSettingsManager(semanticRefreshEnabled: true);
        var serverClient = new TestClient();
        var errorReporter = new TestErrorReporter();
        using var defaultWorkspaceChangedPublisher = new DefaultWorkspaceSemanticTokensRefreshPublisher(settingManager, serverClient, errorReporter);
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
        var settingManager = GetServerSettingsManager(semanticRefreshEnabled: true);
        var serverClient = new TestClient();
        var errorReporter = new TestErrorReporter();
        using var defaultWorkspaceChangedPublisher = new DefaultWorkspaceSemanticTokensRefreshPublisher(settingManager, serverClient, errorReporter);
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

    private static IInitializeManager<InitializeParams, InitializeResult> GetServerSettingsManager(bool semanticRefreshEnabled)
    {
        var initializedParams = GetInitializedParams(semanticRefreshEnabled);

        var settingsManager = new Mock<IInitializeManager<InitializeParams, InitializeResult>>(MockBehavior.Strict);
        settingsManager.Setup(s => s.GetInitializeParams()).Returns(initializedParams);

        return settingsManager.Object;
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
                }
            }
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
