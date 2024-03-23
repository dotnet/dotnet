// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;

internal static class Extensions
{
    public static TestDocumentSnapshot CreateAndAddDocument(this ProjectSnapshotManager.Updater updater, ProjectSnapshot projectSnapshot, string filePath)
    {
        var documentSnapshot = TestDocumentSnapshot.Create(projectSnapshot, filePath);
        updater.DocumentAdded(projectSnapshot.Key, documentSnapshot.HostDocument, new DocumentSnapshotTextLoader(documentSnapshot));

        return documentSnapshot;
    }

    public static TestProjectSnapshot CreateAndAddProject(this ProjectSnapshotManager.Updater updater, string filePath)
    {
        var projectSnapshot = TestProjectSnapshot.Create(filePath);
        updater.ProjectAdded(projectSnapshot.HostProject);

        return projectSnapshot;
    }
}
