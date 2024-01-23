// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

[System.Composition.Shared]
[Export(typeof(IProjectSnapshotManagerAccessor))]
[method: ImportingConstructor]
internal sealed class VisualStudioProjectSnapshotManagerAccessor(
    [Import(typeof(VisualStudioWorkspace))] Workspace workspace)
    : IProjectSnapshotManagerAccessor
{
    private readonly Workspace _workspace = workspace;
    private ProjectSnapshotManagerBase? _projectManager;

    public ProjectSnapshotManagerBase Instance
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
        _projectManager ??= (ProjectSnapshotManagerBase)_workspace.Services
            .GetLanguageServices(RazorLanguage.Name)
            .GetRequiredService<ProjectSnapshotManager>();
    }
}
