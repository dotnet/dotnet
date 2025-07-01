// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.CodeAnalysis.Razor.Settings;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Settings;

internal interface IWorkspaceEditorSettings
{
    ClientSettings Current { get; }

    event EventHandler<EventArgs> Changed;
}
