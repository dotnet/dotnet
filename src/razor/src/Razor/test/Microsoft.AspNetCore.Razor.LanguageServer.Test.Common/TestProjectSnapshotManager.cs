// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Linq;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Test.Common;

internal class TestProjectSnapshotManager : DefaultProjectSnapshotManager
{
    private TestProjectSnapshotManager(IErrorReporter errorReporter, Workspace workspace, ProjectSnapshotManagerDispatcher dispatcher)
        : base(errorReporter, Array.Empty<IProjectSnapshotChangeTrigger>(), workspace, dispatcher)
    {
    }

    public static TestProjectSnapshotManager Create(IErrorReporter errorReporter, ProjectSnapshotManagerDispatcher dispatcher)
    {
        var services = TestServices.Create(
            workspaceServices: new[]
            {
                new DefaultProjectSnapshotProjectEngineFactory(new FallbackProjectEngineFactory(), MefProjectEngineFactories.Factories)
            },
            razorLanguageServices: Enumerable.Empty<ILanguageService>());
        var workspace = TestWorkspace.Create(services);
        var testProjectManager = new TestProjectSnapshotManager(errorReporter, workspace, dispatcher);

        return testProjectManager;
    }

    public bool AllowNotifyListeners { get; set; }

    public TestDocumentSnapshot CreateAndAddDocument(ProjectSnapshot projectSnapshot, string filePath)
    {
        var documentSnapshot = TestDocumentSnapshot.Create(Workspace, projectSnapshot, filePath);
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
}
