// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Moq;

namespace Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;

internal class TestProjectSnapshotManager(
    Workspace workspace,
    IProjectEngineFactoryProvider projectEngineFactoryProvider,
    ProjectSnapshotManagerDispatcher dispatcher)
    : DefaultProjectSnapshotManager(
        Mock.Of<IErrorReporter>(MockBehavior.Strict),
        Array.Empty<IProjectSnapshotChangeTrigger>(),
        workspace,
        projectEngineFactoryProvider,
        dispatcher)
{
    public bool AllowNotifyListeners { get; set; }

    public ProjectSnapshot? GetSnapshot(HostProject hostProject)
    {
        return GetProjects().Cast<ProjectSnapshot>().FirstOrDefault(s => s.FilePath == hostProject.FilePath);
    }

    public ProjectSnapshot? GetSnapshot(Project workspaceProject)
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
