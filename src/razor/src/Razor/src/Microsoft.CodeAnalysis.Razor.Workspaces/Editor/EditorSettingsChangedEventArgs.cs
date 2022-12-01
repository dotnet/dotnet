// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.CodeAnalysis.Razor.Editor;

internal sealed class EditorSettingsChangedEventArgs : EventArgs
{
    public EditorSettingsChangedEventArgs(EditorSettings settings)
    {
        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        Settings = settings;
    }

    public EditorSettings Settings { get; }
}
