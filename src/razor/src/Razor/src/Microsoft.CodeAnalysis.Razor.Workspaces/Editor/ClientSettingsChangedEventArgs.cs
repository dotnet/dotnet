// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.CodeAnalysis.Razor.Editor;

public sealed class ClientSettingsChangedEventArgs : EventArgs
{
    public ClientSettingsChangedEventArgs(ClientSettings settings)
    {
        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        Settings = settings;
    }

    public ClientSettings Settings { get; }
}
