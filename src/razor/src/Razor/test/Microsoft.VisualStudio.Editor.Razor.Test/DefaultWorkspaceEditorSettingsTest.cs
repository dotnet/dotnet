// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.CodeAnalysis.Razor.Editor;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Editor.Razor;

public class DefaultWorkspaceEditorSettingsTest : ProjectSnapshotManagerDispatcherTestBase
{
    public DefaultWorkspaceEditorSettingsTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void InitialSettingsAreEditorSettingsManagerDefault()
    {
        // Arrange
        var settings = new ClientSettings(new ClientSpaceSettings(true, 123), ClientAdvancedSettings.Default);
        var editorSettingsManager = Mock.Of<IClientSettingsManager>(m => m.GetClientSettings() == settings, MockBehavior.Strict);

        // Act
        var manager = new DefaultWorkspaceEditorSettings(editorSettingsManager);

        // Assert
        Assert.Equal(settings, manager.Current);
    }

    [Fact]
    public void OnChanged_TriggersChanged()
    {
        // Arrange
        var editorSettingsManager = new Mock<IClientSettingsManager>(MockBehavior.Strict);
        editorSettingsManager.Setup(m => m.GetClientSettings()).Returns(ClientSettings.Default);
        var manager = new DefaultWorkspaceEditorSettings(editorSettingsManager.Object);
        var called = false;
        manager.Changed += (caller, args) => called = true;

        // Act
        manager.OnChanged(null, null);

        // Assert
        Assert.True(called);
    }

    [Fact]
    public void Attach_CalledOnceForMultipleListeners()
    {
        // Arrange
        var manager = new TestEditorSettingsManagerInternal();

        // Act
        manager.Changed += (caller, args) => { };
        manager.Changed += (caller, args) => { };

        // Assert
        Assert.Equal(1, manager.AttachCount);
    }

    [Fact]
    public void Detach_CalledOnceWhenNoMoreListeners()
    {
        // Arrange
        var manager = new TestEditorSettingsManagerInternal();
        static void Listener1(object caller, ClientSettingsChangedEventArgs args)
        {
        }

        static void Listener2(object caller, ClientSettingsChangedEventArgs args)
        {
        }

        manager.Changed += Listener1;
        manager.Changed += Listener2;

        // Act
        manager.Changed -= Listener1;
        manager.Changed -= Listener2;

        // Assert
        Assert.Equal(1, manager.DetachCount);
    }

    private class TestEditorSettingsManagerInternal : DefaultWorkspaceEditorSettings
    {
        public TestEditorSettingsManagerInternal()
            : base(Mock.Of<IClientSettingsManager>(MockBehavior.Strict))
        {
        }

        public int AttachCount { get; private set; }

        public int DetachCount { get; private set; }

        internal override void AttachToEditorSettingsManager()
        {
            AttachCount++;
        }

        internal override void DetachFromEditorSettingsManager()
        {
            DetachCount++;
        }
    }
}
