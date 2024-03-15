// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal abstract class ProjectSnapshotManagerBase : IProjectSnapshotManager
{
    public abstract event EventHandler<ProjectChangeEventArgs> PriorityChanged;
    public abstract event EventHandler<ProjectChangeEventArgs> Changed;

    public abstract ImmutableArray<IProjectSnapshot> GetProjects();

    public abstract bool IsDocumentOpen(string documentFilePath);

    public abstract IProjectSnapshot GetLoadedProject(ProjectKey projectKey);

    public abstract bool TryGetLoadedProject(ProjectKey projectKey, [NotNullWhen(true)] out IProjectSnapshot? project);

    public abstract ImmutableArray<ProjectKey> GetAllProjectKeys(string projectFileName);

    internal abstract ImmutableArray<string> GetOpenDocuments();

    internal abstract void DocumentAdded(ProjectKey projectKey, HostDocument hostDocument, TextLoader textLoader);

    internal abstract void DocumentOpened(ProjectKey projectKey, string documentFilePath, SourceText sourceText);

    internal abstract void DocumentClosed(ProjectKey projectKey, string documentFilePath, TextLoader textLoader);

    internal abstract void DocumentChanged(ProjectKey projectKey, string documentFilePath, TextLoader textLoader);

    internal abstract void DocumentChanged(ProjectKey projectKey, string documentFilePath, SourceText sourceText);

    internal abstract void DocumentRemoved(ProjectKey projectKey, HostDocument hostDocument);

    internal abstract void ProjectAdded(HostProject hostProject);

    internal abstract void ProjectConfigurationChanged(HostProject hostProject);

    internal abstract void ProjectWorkspaceStateChanged(ProjectKey projectKey, ProjectWorkspaceState? projectWorkspaceState);

    internal abstract void ProjectRemoved(ProjectKey projectKey);

    internal abstract void SolutionOpened();

    internal abstract void SolutionClosed();
}
