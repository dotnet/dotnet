// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Host;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal abstract class ProjectSnapshotManager : ILanguageService
{
    public abstract event EventHandler<ProjectChangeEventArgs> Changed;

    public abstract ImmutableArray<IProjectSnapshot> GetProjects();

    public abstract bool IsDocumentOpen(string documentFilePath);

    public abstract IProjectSnapshot GetLoadedProject(ProjectKey projectKey);

    public abstract ImmutableArray<ProjectKey> GetAllProjectKeys(string projectFileName);
}
