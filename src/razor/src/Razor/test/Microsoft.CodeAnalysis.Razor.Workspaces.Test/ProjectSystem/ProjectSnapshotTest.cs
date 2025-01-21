// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
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
        builder.SetImportFeature(new TestImportProjectFeature());
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
        var state = ProjectState.Create(s_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(s_documents[0]);

        var snapshot = new ProjectSnapshot(state);

        var document = snapshot.GetRequiredDocument(s_documents[0].FilePath);

        // Act
        var documents = snapshot.GetRelatedDocuments(document);

        // Assert
        Assert.Empty(documents);
    }

    [Fact]
    public void GetRelatedDocuments_ImportDocument_ReturnsRelated()
    {
        // Arrange
        var state = ProjectState.Create(s_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(s_documents[0])
            .AddEmptyDocument(s_documents[1])
            .AddEmptyDocument(TestProjectData.SomeProjectImportFile);

        var snapshot = new ProjectSnapshot(state);

        var document = snapshot.GetRequiredDocument(TestProjectData.SomeProjectImportFile.FilePath);

        // Act
        var documents = snapshot.GetRelatedDocuments(document);

        // Assert
        Assert.Collection(
            documents.OrderBy(d => d.FilePath),
            d => Assert.Equal(s_documents[0].FilePath, d.FilePath),
            d => Assert.Equal(s_documents[1].FilePath, d.FilePath));
    }
}
