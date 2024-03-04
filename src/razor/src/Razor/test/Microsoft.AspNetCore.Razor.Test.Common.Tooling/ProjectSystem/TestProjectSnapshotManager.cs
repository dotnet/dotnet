// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;

internal class TestProjectSnapshotManager(
    IProjectSnapshotChangeTrigger[] changeTriggers,
    IProjectEngineFactoryProvider projectEngineFactoryProvider,
    ProjectSnapshotManagerDispatcher dispatcher,
    IErrorReporter errorReporter)
    : DefaultProjectSnapshotManager(changeTriggers, projectEngineFactoryProvider, dispatcher, errorReporter)
{
    public TestProjectSnapshotManager(ProjectSnapshotManagerDispatcher dispatcher)
        : this(changeTriggers: [], ProjectEngineFactories.DefaultProvider, dispatcher, StrictMock.Of<IErrorReporter>())
    {
    }

    public TestProjectSnapshotManager(
        IProjectEngineFactoryProvider projectEngineFactoryProvider,
        ProjectSnapshotManagerDispatcher dispatcher)
        : this(changeTriggers: [], projectEngineFactoryProvider, dispatcher, StrictMock.Of<IErrorReporter>())
    {
    }

    public TestProjectSnapshotManager(
        IProjectSnapshotChangeTrigger[] changeTriggers,
        IProjectEngineFactoryProvider projectEngineFactoryProvider,
        ProjectSnapshotManagerDispatcher dispatcher)
        : this(changeTriggers, projectEngineFactoryProvider, dispatcher, StrictMock.Of<IErrorReporter>())
    {
    }

    public bool AllowNotifyListeners { get; set; }

    protected override void NotifyListeners(ProjectChangeEventArgs e)
    {
        if (AllowNotifyListeners)
        {
            base.NotifyListeners(e);
        }
    }
}
