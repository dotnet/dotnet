// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Editor;

namespace Microsoft.VisualStudio.Editor.Razor;

internal class DefaultWorkspaceEditorSettings : WorkspaceEditorSettings
{
    private readonly IClientSettingsManager _clientSettingsManager;
    private readonly EventHandler<ClientSettingsChangedEventArgs> _onChanged;
    private EventHandler<ClientSettingsChangedEventArgs>? _changed;
    private int _listenerCount = 0;

    public DefaultWorkspaceEditorSettings(IClientSettingsManager clientSettingsManager)
    {
        if (clientSettingsManager is null)
        {
            throw new ArgumentNullException(nameof(clientSettingsManager));
        }

        _clientSettingsManager = clientSettingsManager;
        _onChanged = OnChanged;
    }

    public override event EventHandler<ClientSettingsChangedEventArgs> Changed
    {
        add
        {
            _listenerCount++;
            _changed += value;

            if (_listenerCount == 1)
            {
                // We bind to the editor settings manager only when we have listeners to avoid leaking memory.
                // Basically we're relying on anyone listening to us to have an understanding of when they're going
                // to be torn down. In Razor's case this will just be the document tracker factory (which does know).
                AttachToEditorSettingsManager();
            }
        }
        remove
        {
            _listenerCount--;
            _changed -= value;

            if (_listenerCount == 0)
            {
                // We detach from the editor settings manager when no one is listening to allow us to be garbage
                // collected in the case that the workspace is tearing down.
                DetachFromEditorSettingsManager();
            }
        }
    }

    // Internal for testing
    internal virtual void AttachToEditorSettingsManager()
    {
        _clientSettingsManager.ClientSettingsChanged += _onChanged;
    }

    // Internal for testing
    internal virtual void DetachFromEditorSettingsManager()
    {
        _clientSettingsManager.ClientSettingsChanged -= _onChanged;
    }

    public override ClientSettings Current => _clientSettingsManager.GetClientSettings();

    // Internal for testing
    internal void OnChanged(object sender, ClientSettingsChangedEventArgs e)
    {
        Assumes.True(_changed is not null, nameof(OnChanged) + " should not be invoked when there are no listeners.");

        var args = new ClientSettingsChangedEventArgs(Current);
        _changed.Invoke(this, args);
    }
}
