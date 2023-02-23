// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Project;

public class OmniSharpProjectWorkspaceStateGenerator : IOmniSharpProjectSnapshotManagerChangeTrigger
{
    // Internal for testing
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    internal OmniSharpProjectWorkspaceStateGenerator()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public OmniSharpProjectWorkspaceStateGenerator(OmniSharpProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        InternalWorkspaceStateGenerator = new DefaultProjectWorkspaceStateGenerator(projectSnapshotManagerDispatcher.InternalDispatcher);
    }

    internal DefaultProjectWorkspaceStateGenerator InternalWorkspaceStateGenerator { get; }

    public void Initialize(OmniSharpProjectSnapshotManager projectManager) => InternalWorkspaceStateGenerator.Initialize(projectManager.InternalProjectSnapshotManager);

    public virtual void Update(CodeAnalysis.Project workspaceProject, OmniSharpProjectSnapshot projectSnapshot) => InternalWorkspaceStateGenerator.Update(workspaceProject, projectSnapshot.InternalProjectSnapshot, CancellationToken.None);
}
