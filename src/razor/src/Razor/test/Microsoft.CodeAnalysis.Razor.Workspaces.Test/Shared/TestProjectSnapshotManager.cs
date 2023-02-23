// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Linq;
using System.Threading.Tasks;
using Moq;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class TestProjectSnapshotManager : DefaultProjectSnapshotManager
{
    public TestProjectSnapshotManager(Workspace workspace)
        : base(CreateProjectSnapshotManagerDispatcher(), Mock.Of<IErrorReporter>(MockBehavior.Strict), Enumerable.Empty<ProjectSnapshotChangeTrigger>(), workspace)
    {
    }

    public TestProjectSnapshotManager(ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher, Workspace workspace)
        : base(projectSnapshotManagerDispatcher, Mock.Of<IErrorReporter>(MockBehavior.Strict), Enumerable.Empty<ProjectSnapshotChangeTrigger>(), workspace)
    {
    }

    public bool AllowNotifyListeners { get; set; }

    private static ProjectSnapshotManagerDispatcher CreateProjectSnapshotManagerDispatcher()
    {
        var dispatcher = new Mock<ProjectSnapshotManagerDispatcher>(MockBehavior.Strict);
        dispatcher.Setup(d => d.AssertDispatcherThread(It.IsAny<string>())).Verifiable();
        dispatcher.Setup(d => d.IsDispatcherThread).Returns(true);
        dispatcher.Setup(d => d.DispatcherScheduler).Returns(TaskScheduler.FromCurrentSynchronizationContext());
        return dispatcher.Object;
    }

    public ProjectSnapshot GetSnapshot(HostProject hostProject)
    {
        return Projects.Cast<ProjectSnapshot>().FirstOrDefault(s => s.FilePath == hostProject.FilePath);
    }

    public ProjectSnapshot GetSnapshot(Project workspaceProject)
    {
        return Projects.Cast<ProjectSnapshot>().FirstOrDefault(s => s.FilePath == workspaceProject.FilePath);
    }

    protected override void NotifyListeners(ProjectChangeEventArgs e)
    {
        if (AllowNotifyListeners)
        {
            base.NotifyListeners(e);
        }
    }
}
