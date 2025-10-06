// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class ClientSettingsManagerWrapper(IClientSettingsManager obj) : Wrapper<IClientSettingsManager>(obj), IRazorEditorSettingsManager
    {
        public void Update(bool indentWithTabs, int indentSize)
        {
            var updatedSettings = new ClientSpaceSettings(indentWithTabs, indentSize);
            Object.Update(updatedSettings);
        }
    }
}
