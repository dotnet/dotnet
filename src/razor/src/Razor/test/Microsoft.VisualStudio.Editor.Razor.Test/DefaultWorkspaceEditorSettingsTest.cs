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
        var editorSettings = new EditorSettings(true, 123);
        var editorSettingsManager = Mock.Of<EditorSettingsManager>(m => m.Current == editorSettings, MockBehavior.Strict);

        // Act
        var manager = new DefaultWorkspaceEditorSettings(editorSettingsManager);

        // Assert
        Assert.Equal(editorSettings, manager.Current);
    }

    [Fact]
    public void OnChanged_TriggersChanged()
    {
        // Arrange
        var editorSettingsManager = new Mock<EditorSettingsManager>(MockBehavior.Strict);
        editorSettingsManager.SetupGet(m => m.Current).Returns(EditorSettings.Default);
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
        static void Listener1(object caller, EditorSettingsChangedEventArgs args)
        {
        }

        static void Listener2(object caller, EditorSettingsChangedEventArgs args)
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
            : base(Mock.Of<EditorSettingsManager>(MockBehavior.Strict))
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
