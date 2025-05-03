// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;

namespace Microsoft.VisualStudioCode.RazorExtension.Configuration;

[Shared]
[Export(typeof(IClientSettingsManager))]
internal class ClientSettingsManager : IClientSettingsManager
{
    private ClientSettings _currentSettings = ClientSettings.Default;

    public event EventHandler<EventArgs>? ClientSettingsChanged;

    public ClientSettings GetClientSettings()
    {
        return _currentSettings;
    }

    public void Update(ClientAdvancedSettings updateSettings)
    {
        _currentSettings = _currentSettings with
        {
            AdvancedSettings = updateSettings
        };

        ClientSettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Update(ClientSpaceSettings updateSettings)
    {
        _currentSettings = _currentSettings with
        {
            ClientSpaceSettings = updateSettings
        };

        ClientSettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Update(ClientCompletionSettings updateSettings)
    {
        _currentSettings = _currentSettings with
        {
            ClientCompletionSettings = updateSettings
        };

        ClientSettingsChanged?.Invoke(this, EventArgs.Empty);
    }
}
