// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;

internal class TestProjectSnapshot : ProjectSnapshot
{
    public static TestProjectSnapshot Create(string filePath, ProjectWorkspaceState? projectWorkspaceState = null)
        => Create(filePath, [], projectWorkspaceState);

    public static TestProjectSnapshot Create(string filePath, string[] documentFilePaths, ProjectWorkspaceState? projectWorkspaceState = null)
        => Create(filePath, Path.Combine(Path.GetDirectoryName(filePath) ?? @"\\path", "obj"), documentFilePaths, RazorConfiguration.Default, projectWorkspaceState);

    public static TestProjectSnapshot Create(
        string filePath,
        string intermediateOutputPath,
        string[] documentFilePaths,
        RazorConfiguration configuration,
        ProjectWorkspaceState? projectWorkspaceState = null,
        string? displayName = null)
    {
        var projectEngineFactory = new TestProjectSnapshotProjectEngineFactory();
        var hostProject = new HostProject(filePath, intermediateOutputPath, configuration, "TestRootNamespace", displayName);
        var state = ProjectState.Create(projectEngineFactory, hostProject, projectWorkspaceState ?? ProjectWorkspaceState.Default);

        foreach (var documentFilePath in documentFilePaths)
        {
            var hostDocument = new HostDocument(documentFilePath, documentFilePath);
            state = state.WithAddedHostDocument(hostDocument, () => Task.FromResult(TextAndVersion.Create(SourceText.From(string.Empty), VersionStamp.Default)));
        }

        var testProject = new TestProjectSnapshot(state);

        return testProject;
    }

    private TestProjectSnapshot(ProjectState state)
        : base(state)
    {
    }

    public override VersionStamp Version => throw new NotImplementedException();

    public override IDocumentSnapshot? GetDocument(string filePath)
    {
        return base.GetDocument(filePath);
    }

    public override RazorProjectEngine GetProjectEngine()
    {
        return RazorProjectEngine.Create(RazorConfiguration.Default, RazorProjectFileSystem.Create("C:/"));
    }
}
