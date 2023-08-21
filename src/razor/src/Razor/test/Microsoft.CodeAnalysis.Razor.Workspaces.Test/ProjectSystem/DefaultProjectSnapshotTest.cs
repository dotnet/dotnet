// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Host;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class DefaultProjectSnapshotTest : WorkspaceTestBase
{
    private readonly HostDocument[] _documents;
    private readonly HostProject _hostProject;
    private readonly ProjectWorkspaceState _projectWorkspaceState;
    private readonly TestTagHelperResolver _tagHelperResolver;

    public DefaultProjectSnapshotTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _tagHelperResolver = new TestTagHelperResolver();

        _hostProject = new HostProject(TestProjectData.SomeProject.FilePath, TestProjectData.SomeProject.IntermediateOutputPath, FallbackRazorConfiguration.MVC_2_0, TestProjectData.SomeProject.RootNamespace);
        _projectWorkspaceState = new ProjectWorkspaceState(ImmutableArray.Create(
            TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly").Build()),
            csharpLanguageVersion: default);

        _documents = new HostDocument[]
        {
            TestProjectData.SomeProjectFile1,
            TestProjectData.SomeProjectFile2,

            // linked file
            TestProjectData.AnotherProjectNestedFile3,
        };
    }

    protected override void ConfigureWorkspaceServices(List<IWorkspaceService> services)
    {
        services.Add(_tagHelperResolver);
    }

    protected override void ConfigureProjectEngine(RazorProjectEngineBuilder builder)
    {
        builder.SetImportFeature(new TestImportProjectFeature());
    }

    [Fact]
    public void ProjectSnapshot_CachesDocumentSnapshots()
    {
        // Arrange
        var state = ProjectState.Create(Workspace.Services, _hostProject, _projectWorkspaceState)
            .WithAddedHostDocument(_documents[0], DocumentState.EmptyLoader)
            .WithAddedHostDocument(_documents[1], DocumentState.EmptyLoader)
            .WithAddedHostDocument(_documents[2], DocumentState.EmptyLoader);
        var snapshot = new ProjectSnapshot(state);

        // Act
        var documents = snapshot.DocumentFilePaths.ToDictionary(f => f, f => snapshot.GetDocument(f));

        // Assert
        Assert.Collection(
            documents,
            d => Assert.Same(d.Value, snapshot.GetDocument(d.Key)),
            d => Assert.Same(d.Value, snapshot.GetDocument(d.Key)),
            d => Assert.Same(d.Value, snapshot.GetDocument(d.Key)));
    }

    [Fact]
    public void IsImportDocument_NonImportDocument_ReturnsFalse()
    {
        // Arrange
        var state = ProjectState.Create(Workspace.Services, _hostProject, _projectWorkspaceState)
            .WithAddedHostDocument(_documents[0], DocumentState.EmptyLoader);
        var snapshot = new ProjectSnapshot(state);

        var document = snapshot.GetDocument(_documents[0].FilePath);

        // Act
        var result = snapshot.IsImportDocument(document);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsImportDocument_ImportDocument_ReturnsTrue()
    {
        // Arrange
        var state = ProjectState.Create(Workspace.Services, _hostProject, _projectWorkspaceState)
            .WithAddedHostDocument(_documents[0], DocumentState.EmptyLoader)
            .WithAddedHostDocument(TestProjectData.SomeProjectImportFile, DocumentState.EmptyLoader);
        var snapshot = new ProjectSnapshot(state);

        var document = snapshot.GetDocument(TestProjectData.SomeProjectImportFile.FilePath);

        // Act
        var result = snapshot.IsImportDocument(document);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetRelatedDocuments_NonImportDocument_ReturnsEmpty()
    {
        // Arrange
        var state = ProjectState.Create(Workspace.Services, _hostProject, _projectWorkspaceState)
            .WithAddedHostDocument(_documents[0], DocumentState.EmptyLoader);
        var snapshot = new ProjectSnapshot(state);

        var document = snapshot.GetDocument(_documents[0].FilePath);

        // Act
        var documents = snapshot.GetRelatedDocuments(document);

        // Assert
        Assert.Empty(documents);
    }

    [Fact]
    public void GetRelatedDocuments_ImportDocument_ReturnsRelated()
    {
        // Arrange
        var state = ProjectState.Create(Workspace.Services, _hostProject, _projectWorkspaceState)
            .WithAddedHostDocument(_documents[0], DocumentState.EmptyLoader)
            .WithAddedHostDocument(_documents[1], DocumentState.EmptyLoader)
            .WithAddedHostDocument(TestProjectData.SomeProjectImportFile, DocumentState.EmptyLoader);
        var snapshot = new ProjectSnapshot(state);

        var document = snapshot.GetDocument(TestProjectData.SomeProjectImportFile.FilePath);

        // Act
        var documents = snapshot.GetRelatedDocuments(document);

        // Assert
        Assert.Collection(
            documents.OrderBy(d => d.FilePath),
            d => Assert.Equal(_documents[0].FilePath, d.FilePath),
            d => Assert.Equal(_documents[1].FilePath, d.FilePath));
    }
}
