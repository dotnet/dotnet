// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Editor;

namespace Microsoft.VisualStudio.Editor.Razor;

internal interface IClientSettingsManager
{
    event EventHandler<ClientSettingsChangedEventArgs>? ClientSettingsChanged;

    void Update(ClientSpaceSettings updateSettings);

    void Update(ClientAdvancedSettings updateSettings);

    ClientSettings GetClientSettings();
}

internal interface IAdvancedSettingsStorage
{
    ClientAdvancedSettings GetAdvancedSettings();

    event EventHandler<ClientAdvancedSettingsChangedEventArgs>? Changed;
}

internal class ClientAdvancedSettingsChangedEventArgs(ClientAdvancedSettings advancedSettings) : EventArgs
{
    public ClientAdvancedSettings Settings { get; } = advancedSettings;
}
