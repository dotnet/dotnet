// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Editor;

namespace Microsoft.VisualStudio.Editor.Razor;

internal interface IClientSettingsManager
{
    event EventHandler<ClientSettingsChangedEventArgs>? ClientSettingsChanged;

    void Update(ClientSpaceSettings updateSettings);

    void Update(ClientCompletionSettings updateSettings);

    void Update(ClientAdvancedSettings updateSettings);

    ClientSettings GetClientSettings();
}

internal interface IAdvancedSettingsStorage : IDisposable
{
    ClientAdvancedSettings GetAdvancedSettings();

    Task OnChangedAsync(Action<ClientAdvancedSettings> changed);
}

internal class ClientAdvancedSettingsChangedEventArgs(ClientAdvancedSettings advancedSettings) : EventArgs
{
    public ClientAdvancedSettings Settings { get; } = advancedSettings;
}
