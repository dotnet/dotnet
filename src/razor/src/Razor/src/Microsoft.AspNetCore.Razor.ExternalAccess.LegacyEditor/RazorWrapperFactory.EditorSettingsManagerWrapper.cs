// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Editor;
using Microsoft.VisualStudio.Editor.Razor;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class EditorSettingsManagerWrapper(EditorSettingsManager obj) : Wrapper<EditorSettingsManager>(obj), IRazorEditorSettingsManager
    {
        public void Update(bool indentWithTabs, int indentSize)
        {
            var updatedSettings = new EditorSettings(indentWithTabs, indentSize);
            Object.Update(updatedSettings);
        }
    }
}
