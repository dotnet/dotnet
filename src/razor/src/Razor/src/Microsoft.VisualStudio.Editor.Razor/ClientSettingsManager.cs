// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Editor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.VisualStudio.Editor.Razor;

[System.Composition.Shared]
[Export(typeof(IClientSettingsManager))]
[Export(typeof(EditorSettingsManager))]
internal class ClientSettingsManager : EditorSettingsManager, IClientSettingsManager
{
    public event EventHandler<ClientSettingsChangedEventArgs>? ClientSettingsChanged;
    public override event EventHandler<EditorSettingsChangedEventArgs>? Changed;

    private readonly object _settingsUpdateLock = new();
    private readonly IAdvancedSettingsStorage? _advancedSettingsStorage;
    private readonly RazorGlobalOptions? _globalOptions;
    private ClientSettings _settings;

    public override EditorSettings Current => new(_settings.ClientSpaceSettings.IndentWithTabs, _settings.ClientSpaceSettings.IndentSize);

    [ImportingConstructor]
    public ClientSettingsManager(
        [ImportMany] IEnumerable<ClientSettingsChangedTrigger> editorSettingsChangeTriggers,
        [Import(AllowDefault = true)] IAdvancedSettingsStorage? advancedSettingsStorage = null,
        RazorGlobalOptions? globalOptions = null)
    {
        _settings = ClientSettings.Default;

        // update Roslyn's global options (null in tests):
        if (globalOptions is not null)
        {
            globalOptions.TabSize = _settings.ClientSpaceSettings.IndentSize;
            globalOptions.UseTabs = _settings.ClientSpaceSettings.IndentWithTabs;
        }

        foreach (var changeTrigger in editorSettingsChangeTriggers)
        {
            changeTrigger.Initialize(this);
        }

        _advancedSettingsStorage = advancedSettingsStorage;
        _globalOptions = globalOptions;

        if (_advancedSettingsStorage is not null)
        {
            Update(_advancedSettingsStorage.GetAdvancedSettings());
            _advancedSettingsStorage.Changed += AdvancedSettingsChanged;
        }
    }

    public void Update(ClientSpaceSettings updatedSettings)
    {
        if (updatedSettings is null)
        {
            throw new ArgumentNullException(nameof(updatedSettings));
        }

        // update Roslyn's global options (null in tests):
        if (_globalOptions is not null)
        {
            _globalOptions.TabSize = updatedSettings.IndentSize;
            _globalOptions.UseTabs = updatedSettings.IndentWithTabs;
        }

        lock (_settingsUpdateLock)
        {
            UpdateSettings_NoLock(_settings with { ClientSpaceSettings = updatedSettings });
        }
    }

    public ClientSettings GetClientSettings() => _settings;

    public void Update(ClientAdvancedSettings advancedSettings)
    {
        if (advancedSettings is null)
        {
            throw new ArgumentNullException(nameof(advancedSettings));
        }

        lock (_settingsUpdateLock)
        {
            UpdateSettings_NoLock(_settings with { AdvancedSettings = advancedSettings });
        }
    }

    private void AdvancedSettingsChanged(object sender, ClientAdvancedSettingsChangedEventArgs e) => Update(e.Settings);

    private void UpdateSettings_NoLock(ClientSettings settings)
    {
        if (!_settings.Equals(settings))
        {
            _settings = settings;

            var args = new ClientSettingsChangedEventArgs(_settings);
            ClientSettingsChanged?.Invoke(this, args);
            Changed?.Invoke(this,
                new(new EditorSettings(_settings.ClientSpaceSettings.IndentWithTabs, _settings.ClientSpaceSettings.IndentSize)));
        }
    }

    public override void Update(EditorSettings updateSettings)
    {
        Update(new ClientSpaceSettings(updateSettings.IndentWithTabs, updateSettings.IndentSize));
    }
}
