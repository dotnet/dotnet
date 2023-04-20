// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal abstract class ProjectSnapshotManagerBase : ProjectSnapshotManager
{
    internal abstract Workspace Workspace { get; }

    internal abstract IErrorReporter ErrorReporter { get; }

    internal abstract IReadOnlyCollection<string> OpenDocuments { get; }

    internal abstract void DocumentAdded(HostProject hostProject, HostDocument hostDocument, TextLoader textLoader);

    internal abstract void DocumentOpened(string projectFilePath, string documentFilePath, SourceText sourceText);

    internal abstract void DocumentClosed(string projectFilePath, string documentFilePath, TextLoader textLoader);

    internal abstract void DocumentChanged(string projectFilePath, string documentFilePath, TextLoader textLoader);

    internal abstract void DocumentChanged(string projectFilePath, string documentFilePath, SourceText sourceText);

    internal abstract void DocumentRemoved(HostProject hostProject, HostDocument hostDocument);

    internal abstract void ProjectAdded(HostProject hostProject);

    internal abstract void ProjectConfigurationChanged(HostProject hostProject);

    internal abstract void ProjectWorkspaceStateChanged(string projectFilePath, ProjectWorkspaceState projectWorkspaceState);

    internal abstract void ProjectRemoved(HostProject hostProject);

    internal abstract void ReportError(Exception exception);

    internal abstract void ReportError(Exception exception, IProjectSnapshot project);

    internal abstract void ReportError(Exception exception, HostProject hostProject);

    internal abstract void SolutionOpened();

    internal abstract void SolutionClosed();
}
