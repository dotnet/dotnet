// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Linq;
using Moq;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class TestProjectSnapshotManager : DefaultProjectSnapshotManager
{
    public TestProjectSnapshotManager(Workspace workspace, ProjectSnapshotManagerDispatcher dispatcher)
        : base(Mock.Of<IErrorReporter>(MockBehavior.Strict), Enumerable.Empty<IProjectSnapshotChangeTrigger>(), workspace, dispatcher)
    {
    }

    public bool AllowNotifyListeners { get; set; }

    public ProjectSnapshot GetSnapshot(HostProject hostProject)
    {
        return GetProjects().Cast<ProjectSnapshot>().FirstOrDefault(s => s.FilePath == hostProject.FilePath);
    }

    public ProjectSnapshot GetSnapshot(Project workspaceProject)
    {
        return GetProjects().Cast<ProjectSnapshot>().FirstOrDefault(s => s.FilePath == workspaceProject.FilePath);
    }

    protected override void NotifyListeners(ProjectChangeEventArgs e)
    {
        if (AllowNotifyListeners)
        {
            base.NotifyListeners(e);
        }
    }
}
