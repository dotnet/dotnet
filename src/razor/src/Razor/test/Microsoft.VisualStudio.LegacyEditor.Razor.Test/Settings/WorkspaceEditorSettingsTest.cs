// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Accessors;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.VisualStudio.Editor.Razor.Settings;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Settings;

public class WorkspaceEditorSettingsTest(ITestOutputHelper testOutput) : ProjectSnapshotManagerDispatcherTestBase(testOutput)
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
            new ClientSettingsChangedEventArgs(ClientSettings.Default));

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

        static void Listener1(object caller, ClientSettingsChangedEventArgs args)
        {
        }

        static void Listener2(object caller, ClientSettingsChangedEventArgs args)
        {
        }

        // Act 1
        editorSettings.Changed += Listener1;
        editorSettings.Changed += Listener2;

        // Assert 1 - There should be two listeners and ClientSettingsChanged should be attached.
        Assert.Equal(2, accessor._listenerCount);

        settingsManagerMock.VerifyAdd(
            x => x.ClientSettingsChanged += It.IsAny<EventHandler<ClientSettingsChangedEventArgs>>(),
            Times.Once());

        // Act 2
        editorSettings.Changed -= Listener1;
        editorSettings.Changed -= Listener2;

        // Assert 2 - There should be no more listeners and ClientSettingsChanged should be detached
        Assert.Equal(0, accessor._listenerCount);

        settingsManagerMock.VerifyRemove(
            x => x.ClientSettingsChanged -= It.IsAny<EventHandler<ClientSettingsChangedEventArgs>>(),
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
