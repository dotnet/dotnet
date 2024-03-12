// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;

internal partial class TestProjectSnapshotManager(
    IProjectSnapshotChangeTrigger[] triggers,
    IProjectEngineFactoryProvider projectEngineFactoryProvider,
    ProjectSnapshotManagerDispatcher dispatcher,
    IErrorReporter errorReporter)
    : DefaultProjectSnapshotManager(triggers, projectEngineFactoryProvider, dispatcher, errorReporter)
{
    private IProjectSnapshotManagerAccessor? _accessor;

    public IProjectSnapshotManagerAccessor GetAccessor()
    {
        return _accessor ?? InterlockedOperations.Initialize(ref _accessor, CreateAccessor(this));

        static IProjectSnapshotManagerAccessor CreateAccessor(ProjectSnapshotManagerBase @this)
        {
            var mock = new StrictMock<IProjectSnapshotManagerAccessor>();

            mock.SetupGet(x => x.Instance)
                .Returns(@this);

            var instance = @this;
            mock.Setup(x => x.TryGetInstance(out instance))
                .Returns(true);

            return mock.Object;
        }
    }

    public TestDocumentSnapshot CreateAndAddDocument(ProjectSnapshot projectSnapshot, string filePath)
    {
        var documentSnapshot = TestDocumentSnapshot.Create(projectSnapshot, filePath);
        DocumentAdded(projectSnapshot.Key, documentSnapshot.HostDocument, new DocumentSnapshotTextLoader(documentSnapshot));

        return documentSnapshot;
    }

    internal TestProjectSnapshot CreateAndAddProject(string filePath)
    {
        var projectSnapshot = TestProjectSnapshot.Create(filePath);
        ProjectAdded(projectSnapshot.HostProject);

        return projectSnapshot;
    }

    public Listener ListenToNotifications() => new(this);
}
