// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Settings;

namespace Microsoft.VisualStudio.Editor.Razor.Settings;

internal class ClientAdvancedSettingsChangedEventArgs(ClientAdvancedSettings advancedSettings) : EventArgs
{
    public ClientAdvancedSettings Settings { get; } = advancedSettings;
}
