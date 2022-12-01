// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Editor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.VisualStudio.Editor.Razor;

[System.Composition.Shared]
[Export(typeof(EditorSettingsManager))]
internal class DefaultEditorSettingsManager : EditorSettingsManager
{
    public override event EventHandler<EditorSettingsChangedEventArgs>? Changed;

    private readonly object _settingsAccessorLock = new();
    private readonly RazorGlobalOptions? _globalOptions;
    private EditorSettings _settings;

    [ImportingConstructor]
    public DefaultEditorSettingsManager(            
        [ImportMany] IEnumerable<EditorSettingsChangedTrigger> editorSettingsChangeTriggers,
        RazorGlobalOptions? globalOptions = null)
    {
        _settings = EditorSettings.Default;

        // update Roslyn's global options (null in tests):
        if (globalOptions is not null)
        {
            globalOptions.TabSize = _settings.IndentSize;
            globalOptions.UseTabs = _settings.IndentWithTabs;
        }

        foreach (var changeTrigger in editorSettingsChangeTriggers)
        {
            changeTrigger.Initialize(this);
        }

        _globalOptions = globalOptions;
    }

    public override EditorSettings Current
    {
        get
        {
            lock (_settingsAccessorLock)
            {
                return _settings;
            }
        }
    }

    public override void Update(EditorSettings updatedSettings)
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

        lock (_settingsAccessorLock)
        {
            if (!_settings.Equals(updatedSettings))
            {
                _settings = updatedSettings;
                OnChanged();
            }
        }
    }

    private void OnChanged()
    {
        var args = new EditorSettingsChangedEventArgs(Current);
        Changed?.Invoke(this, args);
    }
}
