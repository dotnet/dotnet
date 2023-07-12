// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal abstract class ProjectSnapshotManagerBase : ProjectSnapshotManager
{
    internal abstract Workspace Workspace { get; }

    internal abstract IErrorReporter ErrorReporter { get; }

    internal abstract IReadOnlyCollection<string> OpenDocuments { get; }

    internal abstract void DocumentAdded(ProjectKey projectKey, HostDocument hostDocument, TextLoader textLoader);

    internal abstract void DocumentOpened(ProjectKey projectKey, string documentFilePath, SourceText sourceText);

    internal abstract void DocumentClosed(ProjectKey projectKey, string documentFilePath, TextLoader textLoader);

    internal abstract void DocumentChanged(ProjectKey projectKey, string documentFilePath, TextLoader textLoader);

    internal abstract void DocumentChanged(ProjectKey projectKey, string documentFilePath, SourceText sourceText);

    internal abstract void DocumentRemoved(ProjectKey projectKey, HostDocument hostDocument);

    internal abstract void ProjectAdded(HostProject hostProject);

    internal abstract void ProjectConfigurationChanged(HostProject hostProject);

    internal abstract void ProjectWorkspaceStateChanged(ProjectKey projectKey, ProjectWorkspaceState projectWorkspaceState);

    internal abstract void ProjectRemoved(ProjectKey projectKey);

    internal abstract void ReportError(Exception exception);

    internal abstract void ReportError(Exception exception, IProjectSnapshot project);

    internal abstract void ReportError(Exception exception, ProjectKey projectKey);

    internal abstract void SolutionOpened();

    internal abstract void SolutionClosed();
}
