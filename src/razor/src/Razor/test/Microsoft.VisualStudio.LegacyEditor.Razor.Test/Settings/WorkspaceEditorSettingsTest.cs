// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Accessors;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Settings;

public class WorkspaceEditorSettingsTest(ITestOutputHelper testOutput) : VisualStudioTestBase(testOutput)
{
    [Fact]
    public void InitialSettingsAreClientSettingsManagerDefault()
    {
        // Arrange
        var expectedSettings = new ClientSettings(
            new ClientSpaceSettings(IndentWithTabs: true, IndentSize: 123),
            ClientCompletionSettings.Default,
            ClientAdvancedSettings.Default);

        var settingsManagerMock = new StrictMock<IClientSettingsManager>();
        settingsManagerMock
            .Setup(x => x.GetClientSettings())
            .Returns(expectedSettings);

        // Act
        var editorSettings = new WorkspaceEditorSettings(settingsManagerMock.Object);

        // Assert
        Assert.Equal(expectedSettings, editorSettings.Current);
    }

    [Fact]
    public void ClientSettingsChangedTriggersChangedEvent()
    {
        // Arrange
        var settingsManagerMock = new StrictMock<IClientSettingsManager>();
        settingsManagerMock
            .Setup(x => x.GetClientSettings())
            .Returns(ClientSettings.Default);

        var editorSettings = new WorkspaceEditorSettings(settingsManagerMock.Object);
        var settingsAccessor = new TestAccessor(editorSettings);

        // Act
        var called = false;
        editorSettings.Changed += (caller, args) => called = true;

        settingsManagerMock.Raise(
            x => x.ClientSettingsChanged += null,
            EventArgs.Empty);

        // Assert
        Assert.True(called);
    }

    [Fact]
    public void ClientSettingsChangedIsOnlyHookedOnceForMultipleListeners()
    {
        // Arrange
        var settingsManagerMock = new StrictMock<IClientSettingsManager>();
        settingsManagerMock
            .SetupAdd(x => x.ClientSettingsChanged += delegate { });
        settingsManagerMock
            .SetupRemove(x => x.ClientSettingsChanged -= delegate { });

        var editorSettings = new WorkspaceEditorSettings(settingsManagerMock.Object);
        var accessor = new TestAccessor(editorSettings);

        static void Listener1(object caller, EventArgs args)
        {
        }

        static void Listener2(object caller, EventArgs args)
        {
        }

        // Act 1
        editorSettings.Changed += Listener1;
        editorSettings.Changed += Listener2;

        // Assert 1 - There should be two listeners and ClientSettingsChanged should be attached.
        Assert.Equal(2, accessor._listenerCount);

        settingsManagerMock.VerifyAdd(
            x => x.ClientSettingsChanged += It.IsAny<EventHandler<EventArgs>>(),
            Times.Once());

        // Act 2
        editorSettings.Changed -= Listener1;
        editorSettings.Changed -= Listener2;

        // Assert 2 - There should be no more listeners and ClientSettingsChanged should be detached
        Assert.Equal(0, accessor._listenerCount);

        settingsManagerMock.VerifyRemove(
            x => x.ClientSettingsChanged -= It.IsAny<EventHandler<EventArgs>>(),
            Times.Once());
    }

    private class TestAccessor(WorkspaceEditorSettings instance) : TestAccessor<WorkspaceEditorSettings>(instance)
    {
        public int _listenerCount
        {
            get => Dynamic._listenerCount;
            set => Dynamic._listenerCount = value;
        }
    }
}
