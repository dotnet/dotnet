// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Threading;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor;

internal abstract class ProjectWorkspaceStateGenerator : ProjectSnapshotChangeTrigger
{
    public abstract void Update(Project workspaceProject, ProjectSnapshot projectSnapshot, CancellationToken cancellationToken);
}
