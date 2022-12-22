// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Editor;

namespace Microsoft.VisualStudio.Editor.Razor;

[Shared]
[ExportLanguageServiceFactory(typeof(WorkspaceEditorSettings), RazorLanguage.Name)]
internal class DefaultWorkspaceEditorSettingsFactory : ILanguageServiceFactory
{
    private readonly EditorSettingsManager _editorSettingsManager;

    [ImportingConstructor]
    public DefaultWorkspaceEditorSettingsFactory(EditorSettingsManager editorSettingsManager)
    {
        if (editorSettingsManager is null)
        {
            throw new ArgumentNullException(nameof(editorSettingsManager));
        }

        _editorSettingsManager = editorSettingsManager;
    }

    public ILanguageService CreateLanguageService(HostLanguageServices languageServices)
    {
        if (languageServices is null)
        {
            throw new ArgumentNullException(nameof(languageServices));
        }

        return new DefaultWorkspaceEditorSettings(_editorSettingsManager);
    }
}
