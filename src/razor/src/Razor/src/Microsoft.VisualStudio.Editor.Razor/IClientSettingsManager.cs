// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Editor;

namespace Microsoft.VisualStudio.Editor.Razor;

public interface IClientSettingsManager
{
    event EventHandler<ClientSettingsChangedEventArgs>? ClientSettingsChanged;

    void Update(ClientSpaceSettings updateSettings);

    ClientSettings GetClientSettings();
}

public interface IAdvancedSettingsStorage
{
    ClientAdvancedSettings GetAdvancedSettings();

    event EventHandler<ClientAdvancedSettingsChangedEventArgs>? Changed;
}

public class ClientAdvancedSettingsChangedEventArgs : EventArgs
{
    public ClientAdvancedSettingsChangedEventArgs(ClientAdvancedSettings advancedSettings)
    {
        Settings = advancedSettings;
    }

    public ClientAdvancedSettings Settings { get; }
}
