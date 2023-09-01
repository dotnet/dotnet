// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal abstract class ProjectSnapshotManagerBase : ProjectSnapshotManager
{
    internal abstract Workspace Workspace { get; }

    internal abstract IErrorReporter ErrorReporter { get; }

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

    internal abstract void ReportError(Exception exception);

    internal abstract void ReportError(Exception exception, IProjectSnapshot project);

    internal abstract void ReportError(Exception exception, ProjectKey projectKey);

    internal abstract void SolutionOpened();

    internal abstract void SolutionClosed();

    /// <summary>
    /// Gets a project if it's already loaded, or calls <see cref="ProjectAdded(HostProject)" /> with a new host project
    /// </summary>
    internal abstract IProjectSnapshot GetOrAddLoadedProject(ProjectKey projectKey, Func<HostProject> createHostProjectFunc);

    internal abstract bool TryRemoveLoadedProject(ProjectKey projectKey, [NotNullWhen(true)] out IProjectSnapshot? project);
}
