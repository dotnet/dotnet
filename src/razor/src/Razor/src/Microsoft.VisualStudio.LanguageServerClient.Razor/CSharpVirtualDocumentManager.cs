// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Export(typeof(IProjectSnapshotChangeTrigger))]
[method: ImportingConstructor]
internal class CSharpVirtualDocumentManager(LSPDocumentManager lspDocumentManager) : IProjectSnapshotChangeTrigger
{
    private readonly LSPDocumentManager _lspDocumentManager = lspDocumentManager;

    public void Initialize(ProjectSnapshotManagerBase projectManager)
    {
        projectManager.Changed += ProjectManager_Changed;
    }

    private void ProjectManager_Changed(object sender, ProjectChangeEventArgs e)
    {
        if (e.SolutionIsClosing)
        {
            return;
        }

        switch (e.Kind)
        {
            case ProjectChangeKind.DocumentAdded:
            case ProjectChangeKind.DocumentRemoved:
            case ProjectChangeKind.ProjectChanged:
            case ProjectChangeKind.ProjectAdded:
            case ProjectChangeKind.ProjectRemoved:
                _lspDocumentManager.RefreshVirtualDocuments();
                break;
        }
    }
}
