// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal interface IProjectSnapshotManager
{
    event EventHandler<ProjectChangeEventArgs> PriorityChanged;
    event EventHandler<ProjectChangeEventArgs> Changed;

    ImmutableArray<IProjectSnapshot> GetProjects();

    bool IsDocumentOpen(string documentFilePath);

    IProjectSnapshot GetLoadedProject(ProjectKey projectKey);
    bool TryGetLoadedProject(ProjectKey projectKey, [NotNullWhen(true)] out IProjectSnapshot? project);

    ImmutableArray<ProjectKey> GetAllProjectKeys(string projectFileName);
}
