// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.CodeAnalysis.Razor.Editor;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Editor.Razor;

public class ClientSettingsManagerTest(ITestOutputHelper testOutput) : ProjectSnapshotManagerDispatcherTestBase(testOutput)
{
    private readonly IEnumerable<ClientSettingsChangedTrigger> _editorSettingsChangeTriggers = Array.Empty<ClientSettingsChangedTrigger>();

    [Fact]
    public void ChangeTriggersGetInitialized()
    {
        // Act
        var triggers = new TestChangeTrigger[]
        {
            new TestChangeTrigger(),
            new TestChangeTrigger(),
        };
        var manager = new ClientSettingsManager(triggers);

        // Assert
        Assert.All(triggers, (trigger) => Assert.True(trigger.Initialized));
    }

    [Fact]
    public void InitialSettingsAreDefault()
    {
        // Act
        var manager = new ClientSettingsManager(_editorSettingsChangeTriggers);

        // Assert
        Assert.Equal(ClientSettings.Default, manager.GetClientSettings());
    }

    [Fact]
    public void Update_TriggersChangedIfEditorSettingsAreDifferent()
    {
        // Arrange
        var manager = new ClientSettingsManager(_editorSettingsChangeTriggers);
        var called = false;
        manager.Changed += (caller, args) => called = true;
        var settings = new ClientSpaceSettings(IndentWithTabs: true, IndentSize: 7);

        // Act
        manager.Update(settings);

        // Assert
        Assert.True(called);
        Assert.Equal(settings, manager.GetClientSettings().ClientSpaceSettings);
    }

    [Fact]
    public void Update_DoesNotTriggerChangedIfEditorSettingsAreSame()
    {
        // Arrange
        var manager = new ClientSettingsManager(_editorSettingsChangeTriggers);
        var called = false;
        manager.Changed += (caller, args) => called = true;
        var originalSettings = manager.GetClientSettings();

        // Act
        manager.Update(ClientSpaceSettings.Default);

        // Assert
        Assert.False(called);
        Assert.Same(originalSettings, manager.GetClientSettings());
    }

    [Fact]
    public void Update_TriggersChangedIfAdvancedSettingsAreDifferent()
    {
        // Arrange
        var manager = new ClientSettingsManager(_editorSettingsChangeTriggers);
        var called = false;
        manager.Changed += (caller, args) => called = true;
        var settings = new ClientAdvancedSettings(FormatOnType: false, AutoClosingTags: true, AutoInsertAttributeQuotes: true, ColorBackground: true, CommitElementsWithSpace: false, SnippetSetting: default, LogLevel: default);

        // Act
        manager.Update(settings);

        // Assert
        Assert.True(called);
        Assert.Equal(settings, manager.GetClientSettings().AdvancedSettings);
    }

    [Fact]
    public void Update_DoesNotTriggerChangedIfAdvancedSettingsAreSame()
    {
        // Arrange
        var manager = new ClientSettingsManager(_editorSettingsChangeTriggers);
        var called = false;
        manager.Changed += (caller, args) => called = true;
        var originalSettings = manager.GetClientSettings();

        // Act
        manager.Update(ClientAdvancedSettings.Default);

        // Assert
        Assert.False(called);
        Assert.Same(originalSettings, manager.GetClientSettings());
    }

    [Fact]
    public void InitialSettingsStored()
    {
        var defaultSettings = ClientAdvancedSettings.Default;
        var expectedSettings = defaultSettings with
        {
            FormatOnType = !defaultSettings.FormatOnType
        };

        var manager = new ClientSettingsManager(_editorSettingsChangeTriggers, new AdvancedSettingsStorage(expectedSettings));

        Assert.Same(expectedSettings, manager.GetClientSettings().AdvancedSettings);
    }

    private class TestChangeTrigger : ClientSettingsChangedTrigger
    {
        public bool Initialized { get; private set; }

        public override void Initialize(IClientSettingsManager clientSettingsManager)
        {
            Initialized = true;
        }
    }

    private class AdvancedSettingsStorage : IAdvancedSettingsStorage
    {
        private readonly ClientAdvancedSettings _settings;

        public AdvancedSettingsStorage(ClientAdvancedSettings settings)
        {
            _settings = settings;
        }

#pragma warning disable CS0067 // The event 'ClientSettingsManagerTest.AdvancedSettingsStorage.Changed' is never used
        public event EventHandler<ClientAdvancedSettingsChangedEventArgs> Changed;
#pragma warning restore CS0067 // The event 'ClientSettingsManagerTest.AdvancedSettingsStorage.Changed' is never used

        public ClientAdvancedSettings GetAdvancedSettings()
        {
            return _settings;
        }

        public void Dispose()
        {
        }
    }
}
