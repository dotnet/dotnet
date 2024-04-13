// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.VisualStudio.Razor.Settings;

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
