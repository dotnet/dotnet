// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;

internal partial class TestProjectSnapshotManager(
    IProjectEngineFactoryProvider projectEngineFactoryProvider,
    ProjectSnapshotManagerDispatcher dispatcher)
    : ProjectSnapshotManager(projectEngineFactoryProvider, dispatcher)
{
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
