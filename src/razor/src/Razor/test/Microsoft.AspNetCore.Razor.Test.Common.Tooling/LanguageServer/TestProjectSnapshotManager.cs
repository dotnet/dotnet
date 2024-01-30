// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;

internal class TestProjectSnapshotManager : DefaultProjectSnapshotManager
{
    private TestProjectSnapshotManager(
        IProjectEngineFactoryProvider projectEngineFactoryProvider,
        ProjectSnapshotManagerDispatcher dispatcher,
        IErrorReporter errorReporter)
        : base(triggers: [], projectEngineFactoryProvider, dispatcher, errorReporter)
    {
    }

    public static TestProjectSnapshotManager Create(ProjectSnapshotManagerDispatcher dispatcher, IErrorReporter errorReporter)
        => new TestProjectSnapshotManager(ProjectEngineFactories.DefaultProvider, dispatcher, errorReporter);

    public bool AllowNotifyListeners { get; set; }

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

    protected override void NotifyListeners(ProjectChangeEventArgs e)
    {
        if (AllowNotifyListeners)
        {
            base.NotifyListeners(e);
        }
    }

    private sealed class TestWorkspaceProvider(Workspace workspace) : IWorkspaceProvider
    {
        public Workspace GetWorkspace() => workspace;
    }
}
