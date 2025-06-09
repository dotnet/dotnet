// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class ProjectSnapshotTest(ITestOutputHelper testOutput) : WorkspaceTestBase(testOutput)
{
    private static readonly HostProject s_hostProject = TestProjectData.SomeProject with { Configuration = FallbackRazorConfiguration.MVC_2_0 };
    private static readonly ProjectWorkspaceState s_projectWorkspaceState = ProjectWorkspaceState.Create([TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly").Build()]);

    private static readonly HostDocument[] s_documents =
    [
        TestProjectData.SomeProjectFile1,
        TestProjectData.SomeProjectFile2,

        // linked file
        TestProjectData.AnotherProjectNestedFile3,
    ];

    protected override void ConfigureProjectEngine(RazorProjectEngineBuilder builder)
    {
        builder.SetImportFeature(new TestImportProjectFeature(HierarchicalImports.Legacy));
    }

    [Fact]
    public void ProjectSnapshot_CachesDocumentSnapshots()
    {
        // Arrange
        var state = ProjectState.Create(s_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(s_documents[0])
            .AddEmptyDocument(s_documents[1])
            .AddEmptyDocument(s_documents[2]);

        var snapshot = new ProjectSnapshot(state);

        // Act
        var documents = snapshot.DocumentFilePaths.ToDictionary(f => f, snapshot.GetRequiredDocument);

        // Assert
        Assert.Collection(
            documents,
            d => Assert.Same(d.Value, snapshot.GetRequiredDocument(d.Key)),
            d => Assert.Same(d.Value, snapshot.GetRequiredDocument(d.Key)),
            d => Assert.Same(d.Value, snapshot.GetRequiredDocument(d.Key)));
    }

    [Fact]
    public void GetRelatedDocuments_NonImportDocument_ReturnsEmpty()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(s_documents[0]);

        var project = new ProjectSnapshot(state);

        // Act
        var documents = project.GetRelatedDocumentFilePaths(s_documents[0].FilePath);

        // Assert
        Assert.Empty(documents);
    }

    [Fact]
    public void GetRelatedDocuments_ImportDocument_ReturnsRelated()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(s_documents[0])
            .AddEmptyDocument(s_documents[1])
            .AddEmptyDocument(TestProjectData.SomeProjectImportFile);

        var project = new ProjectSnapshot(state);

        // Act
        var relatedDocumentFilePaths = project.GetRelatedDocumentFilePaths(TestProjectData.SomeProjectImportFile.FilePath);

        // Assert
        Assert.Collection(
            relatedDocumentFilePaths.Sort(),
            path => Assert.Equal(s_documents[0].FilePath, path),
            path => Assert.Equal(s_documents[1].FilePath, path));
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/11712")]
    public async Task GetImportSources_ResultsHaveCorrectFilePaths()
    {
        var basePath = TestPathUtilities.CreateRootedPath("my", "project", "path");
        var hostProject = TestHostProject.Create(Path.Combine(basePath, "project.csproj"));

        var importHostDocument = TestHostDocument.Create(
            hostProject,
            Path.Combine(basePath, "_ViewImports.cshtml"));

        var hostDocument = TestHostDocument.Create(
            hostProject,
            Path.Combine(basePath, "Products", "Index.cshtml"));

        var state = ProjectState
            .Create(hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .AddEmptyDocument(importHostDocument)
            .AddEmptyDocument(hostDocument);

        var project = new ProjectSnapshot(state);
        var importDocument = project.GetRequiredDocument(importHostDocument.FilePath);
        var document = project.GetRequiredDocument(hostDocument.FilePath);

        var importSources = await CompilationHelpers.GetImportSourcesAsync(document, project.ProjectEngine, DisposalToken);

        // Note: The only import returned is the one we added. There aren't any default imports
        // because of the ConfigureProjectEngine override above.
        var importSource = Assert.Single(importSources);

        // The RazorSourceDocument for the import should use the paths from the document.
        Assert.Equal(importDocument.FilePath, importSource.FilePath, FilePathComparer.Instance);
        Assert.Equal(importDocument.TargetPath, importSource.RelativePath, FilePathComparer.Instance);
    }
}
