// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.VisualStudio.Editor.Razor.Settings;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Settings;

[Export(typeof(IWorkspaceEditorSettings))]
internal sealed class WorkspaceEditorSettings : IWorkspaceEditorSettings
{
    private readonly IClientSettingsManager _clientSettingsManager;
    private readonly EventHandler<ClientSettingsChangedEventArgs> _onClientSettingsChanged;

    private EventHandler<ClientSettingsChangedEventArgs>? _changedHandler;
    private int _listenerCount = 0;

    [ImportingConstructor]
    public WorkspaceEditorSettings(IClientSettingsManager clientSettingsManager)
    {
        _clientSettingsManager = clientSettingsManager;
        _onClientSettingsChanged = OnClientSettingsChanged;
    }

    public ClientSettings Current => _clientSettingsManager.GetClientSettings();

    private void OnClientSettingsChanged(object sender, ClientSettingsChangedEventArgs e)
    {
        Assumes.True(_changedHandler is not null, $"{nameof(OnClientSettingsChanged)} should not be invoked when there are no listeners.");

        var args = new ClientSettingsChangedEventArgs(Current);
        _changedHandler.Invoke(this, args);
    }

    public event EventHandler<ClientSettingsChangedEventArgs> Changed
    {
        add
        {
            _listenerCount++;
            _changedHandler += value;

            if (_listenerCount == 1)
            {
                // We bind to the client settings manager only when we have listeners to avoid leaking memory.
                // Basically we're relying on anyone listening to us to have an understanding of when they're going
                // to be torn down. In Razor's case this will just be the document tracker factory (which does know).
                _clientSettingsManager.ClientSettingsChanged += _onClientSettingsChanged;
            }
        }
        remove
        {
            _listenerCount--;
            _changedHandler -= value;

            if (_listenerCount == 0)
            {
                // We detach from the client settings manager when no one is listening to allow us to be garbage
                // collected in the case that the workspace is tearing down.
                _clientSettingsManager.ClientSettingsChanged -= _onClientSettingsChanged;
            }
        }
    }
}
