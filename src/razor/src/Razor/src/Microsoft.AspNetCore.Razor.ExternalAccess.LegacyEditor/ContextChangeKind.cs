// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal enum ContextChangeKind
{
    ProjectChanged,
    EditorSettingsChanged,
    TagHelpersChanged,
    ImportsChanged,
}
