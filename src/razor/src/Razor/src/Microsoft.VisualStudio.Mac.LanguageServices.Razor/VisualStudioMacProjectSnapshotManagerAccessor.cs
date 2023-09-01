// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using MonoDevelop.Ide;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

[System.Composition.Shared]
[Export(typeof(ProjectSnapshotManagerAccessor))]
[method: ImportingConstructor]
internal class VisualStudioMacProjectSnapshotManagerAccessor() : ProjectSnapshotManagerAccessor
{
    private ProjectSnapshotManagerBase? _projectManager;

    public override ProjectSnapshotManagerBase Instance
    {
        get
        {
            EnsureInitialized();

            return _projectManager;
        }
    }

    [MemberNotNull(nameof(_projectManager))]
    private void EnsureInitialized()
    {
        _projectManager ??= (ProjectSnapshotManagerBase)IdeServices.TypeSystemService.Workspace.Services.GetLanguageServices(RazorLanguage.Name).GetRequiredService<ProjectSnapshotManager>();
    }
}
