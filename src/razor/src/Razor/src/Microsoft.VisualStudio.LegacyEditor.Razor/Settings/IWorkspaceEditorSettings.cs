// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.VisualStudio.Razor.Settings;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Settings;

internal interface IWorkspaceEditorSettings
{
    ClientSettings Current { get; }

    event EventHandler<ClientSettingsChangedEventArgs> Changed;
}
