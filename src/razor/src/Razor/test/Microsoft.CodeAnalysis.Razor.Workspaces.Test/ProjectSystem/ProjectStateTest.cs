// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor.Utilities;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Test.Common.TestProjectData;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class ProjectStateTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    private static readonly IProjectEngineFactoryProvider s_projectEngineFactoryProvider =
        TestProjectEngineFactoryProvider.Instance.AddConfigure(
            static b => b.SetImportFeature(new TestImportProjectFeature(HierarchicalImports.Legacy)));

    private static readonly HostProject s_hostProject =
        SomeProject with { Configuration = FallbackRazorConfiguration.MVC_2_0 };

    private static readonly ProjectWorkspaceState s_projectWorkspaceState =
        ProjectWorkspaceState.Create([TagHelperDescriptorBuilder.CreateTagHelper("TestTagHelper", "TestAssembly").Build()]);

    private static readonly SourceText s_text = SourceText.From("Hello, world!");
    private static readonly TextLoader s_textLoader = TestMocks.CreateTextLoader(s_text);

    [Fact]
    public void GetImportDocumentTargetPaths_DoesNotIncludeCurrentImport()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState);

        // Act
        var paths = state.GetImportDocumentTargetPaths(SomeProjectComponentImportFile1);

        // Assert
        Assert.DoesNotContain(SomeProjectComponentImportFile1.TargetPath, paths);
    }

    [Fact]
    public void ProjectState_ConstructedNew()
    {
        // Arrange & Act
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState);

        // Assert
        Assert.Empty(state.Documents);
    }

    [Fact]
    public void ProjectState_AddDocument_ToEmpty()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState);

        // Act
        var newState = state.AddEmptyDocument(SomeProjectFile1);

        // Assert
        var documentState = Assert.Single(newState.Documents.Values);
        Assert.Same(SomeProjectFile1, documentState.HostDocument);
    }

    [Fact]
    public async Task ProjectState_AddDocument_DocumentIsEmpty()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState);

        // Act
        var newState = state.AddEmptyDocument(SomeProjectFile1);

        // Assert
        var text = await newState.Documents[SomeProjectFile1.FilePath].GetTextAsync(DisposalToken);
        Assert.Equal(0, text.Length);
    }

    [Fact]
    public void ProjectState_AddDocument_ToProjectWithDocuments()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.AddEmptyDocument(SomeProjectFile1);

        // Assert
        Assert.Collection(
            newState.Documents.OrderBy(static kvp => kvp.Key),
            d => Assert.Same(AnotherProjectNestedFile3, d.Value.HostDocument),
            d => Assert.Same(SomeProjectFile1, d.Value.HostDocument),
            d => Assert.Same(SomeProjectFile2, d.Value.HostDocument));
    }

    [Fact]
    public void ProjectState_AddDocument_TracksImports()
    {
        // Arrange & Act
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(SomeProjectFile1)
            .AddEmptyDocument(SomeProjectFile2)
            .AddEmptyDocument(SomeProjectNestedFile3)
            .AddEmptyDocument(AnotherProjectNestedFile4);

        // Assert
        Assert.Collection(
            state.ImportsToRelatedDocuments.OrderBy(kvp => kvp.Key),
            kvp =>
            {
                Assert.Equal(SomeProjectImportFile.TargetPath, kvp.Key);
                Assert.Equal(
                    [
                        AnotherProjectNestedFile4.FilePath,
                        SomeProjectFile1.FilePath,
                        SomeProjectFile2.FilePath,
                        SomeProjectNestedFile3.FilePath,
                    ],
                    kvp.Value.OrderBy(f => f));
            },
            kvp =>
            {
                Assert.Equal(SomeProjectNestedImportFile.TargetPath, kvp.Key);
                Assert.Equal(
                    [
                        AnotherProjectNestedFile4.FilePath,
                        SomeProjectNestedFile3.FilePath,
                    ],
                    kvp.Value.OrderBy(f => f));
            });
    }

    [Fact]
    public void ProjectState_AddDocument_TracksImports_AddImportFile()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(SomeProjectFile1)
            .AddEmptyDocument(SomeProjectFile2)
            .AddEmptyDocument(SomeProjectNestedFile3)
            .AddEmptyDocument(AnotherProjectNestedFile4);

        // Act
        var newState = state.AddEmptyDocument(AnotherProjectImportFile);

        // Assert
        Assert.Collection(
            newState.ImportsToRelatedDocuments.OrderBy(kvp => kvp.Key),
            kvp =>
            {
                Assert.Equal(SomeProjectImportFile.TargetPath, kvp.Key);
                Assert.Equal(
                    [
                        AnotherProjectNestedFile4.FilePath,
                        SomeProjectFile1.FilePath,
                        SomeProjectFile2.FilePath,
                        SomeProjectNestedFile3.FilePath,
                    ],
                    kvp.Value.OrderBy(f => f));
            },
            kvp =>
            {
                Assert.Equal(SomeProjectNestedImportFile.TargetPath, kvp.Key);
                Assert.Equal(
                    [
                        AnotherProjectNestedFile4.FilePath,
                        SomeProjectNestedFile3.FilePath,
                    ],
                    kvp.Value.OrderBy(f => f));
            });
    }

    [Fact]
    public void ProjectState_AddDocument_RetainsComputedState()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.AddEmptyDocument(SomeProjectFile1);

        // Assert
        Assert.Same(state.ProjectEngine, newState.ProjectEngine);
        AssertSameTagHelpers(state.TagHelpers, newState.TagHelpers);
        Assert.Same(state.Documents[SomeProjectFile2.FilePath], newState.Documents[SomeProjectFile2.FilePath]);
        Assert.Same(state.Documents[AnotherProjectNestedFile3.FilePath], newState.Documents[AnotherProjectNestedFile3.FilePath]);
    }

    [Fact]
    public void ProjectState_AddDocument_DuplicateIgnored()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.AddEmptyDocument(new HostDocument(SomeProjectFile2.FilePath, "SomePath.cshtml"));

        // Assert
        Assert.Same(state, newState);
    }

    [Fact]
    public async Task ProjectState_WithDocumentText_Loader()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.WithDocumentText(SomeProjectFile2.FilePath, s_textLoader);

        // Assert
        var text = await newState.Documents[SomeProjectFile2.FilePath].GetTextAsync(DisposalToken);
        Assert.Same(s_text, text);
    }

    [Fact]
    public async Task ProjectState_WithDocumentText_Snapshot()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.WithDocumentText(SomeProjectFile2.FilePath, s_text);

        // Assert
        var text = await newState.Documents[SomeProjectFile2.FilePath].GetTextAsync(DisposalToken);
        Assert.Same(s_text, text);
    }

    [Fact]
    public void ProjectState_WithDocumentText_Loader_RetainsComputedState()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.WithDocumentText(SomeProjectFile2.FilePath, s_textLoader);

        // Assert
        Assert.Same(state.ProjectEngine, newState.ProjectEngine);
        AssertSameTagHelpers(state.TagHelpers, newState.TagHelpers);
        Assert.NotSame(state.Documents[SomeProjectFile2.FilePath], newState.Documents[SomeProjectFile2.FilePath]);
    }

    [Fact]
    public void ProjectState_WithDocumentText_Snapshot_RetainsComputedState()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.WithDocumentText(SomeProjectFile2.FilePath, s_text);

        // Assert
        Assert.Same(state.ProjectEngine, newState.ProjectEngine);
        AssertSameTagHelpers(state.TagHelpers, newState.TagHelpers);
        Assert.NotSame(state.Documents[SomeProjectFile2.FilePath], newState.Documents[SomeProjectFile2.FilePath]);
    }

    [Fact]
    public void ProjectState_WithDocumentText_Loader_NotFoundIgnored()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.WithDocumentText(SomeProjectFile1.FilePath, s_textLoader);

        // Assert
        Assert.Same(state, newState);
    }

    [Fact]
    public void ProjectState_WithDocumentText_Snapshot_NotFoundIgnored()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.WithDocumentText(SomeProjectFile1.FilePath, s_text);

        // Assert
        Assert.Same(state, newState);
    }

    [Fact]
    public void ProjectState_RemoveDocument_FromProjectWithDocuments()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.RemoveDocument(SomeProjectFile2.FilePath);

        // Assert
        var documentState = Assert.Single(newState.Documents.Values);
        Assert.Same(AnotherProjectNestedFile3, documentState.HostDocument);
    }

    [Fact]
    public void ProjectState_RemoveDocument_TracksImports()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(SomeProjectFile1)
            .AddEmptyDocument(SomeProjectFile2)
            .AddEmptyDocument(SomeProjectNestedFile3)
            .AddEmptyDocument(AnotherProjectNestedFile4);

        // Act
        var newState = state.RemoveDocument(SomeProjectNestedFile3.FilePath);

        // Assert
        Assert.Collection(
            newState.ImportsToRelatedDocuments.OrderBy(kvp => kvp.Key),
            kvp =>
            {
                Assert.Equal(SomeProjectImportFile.TargetPath, kvp.Key);
                Assert.Equal(
                    [
                        AnotherProjectNestedFile4.FilePath,
                        SomeProjectFile1.FilePath,
                        SomeProjectFile2.FilePath,
                    ],
                    kvp.Value.OrderBy(f => f));
            },
            kvp =>
            {
                Assert.Equal(SomeProjectNestedImportFile.TargetPath, kvp.Key);
                Assert.Equal(
                    [
                        AnotherProjectNestedFile4.FilePath,
                    ],
                    kvp.Value.OrderBy(f => f));
            });
    }

    [Fact]
    public void ProjectState_RemoveDocument_TracksImports_RemoveAllDocuments()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(SomeProjectFile1)
            .AddEmptyDocument(SomeProjectFile2)
            .AddEmptyDocument(SomeProjectNestedFile3)
            .AddEmptyDocument(AnotherProjectNestedFile4);

        // Act
        var newState = state
            .RemoveDocument(SomeProjectFile1.FilePath)
            .RemoveDocument(SomeProjectFile2.FilePath)
            .RemoveDocument(SomeProjectNestedFile3.FilePath)
            .RemoveDocument(AnotherProjectNestedFile4.FilePath);

        // Assert
        Assert.Empty(newState.Documents);
        Assert.Empty(newState.ImportsToRelatedDocuments);
    }

    [Fact]
    public void ProjectState_RemoveDocument_RetainsComputedState()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.RemoveDocument(AnotherProjectNestedFile3.FilePath);

        // Assert
        Assert.Same(state.ProjectEngine, newState.ProjectEngine);
        AssertSameTagHelpers(state.TagHelpers, newState.TagHelpers);
        Assert.Same(state.Documents[SomeProjectFile2.FilePath], newState.Documents[SomeProjectFile2.FilePath]);
    }

    [Fact]
    public void ProjectState_RemoveDocument_NotFoundIgnored()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.RemoveDocument(SomeProjectFile1.FilePath);

        // Assert
        Assert.Same(state, newState);
    }

    [Fact]
    public void ProjectState_WithHostProject_ConfigurationChange_UpdatesConfigurationState()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.WithHostProject(s_hostProject with { Configuration = FallbackRazorConfiguration.MVC_1_0 });

        // Assert
        Assert.Same(FallbackRazorConfiguration.MVC_1_0, newState.HostProject.Configuration);

        Assert.NotSame(state.ProjectEngine, newState.ProjectEngine);
        AssertSameTagHelpers(state.TagHelpers, newState.TagHelpers);

        Assert.NotSame(state.Documents[SomeProjectFile2.FilePath], newState.Documents[SomeProjectFile2.FilePath]);
        Assert.NotSame(state.Documents[AnotherProjectNestedFile3.FilePath], newState.Documents[AnotherProjectNestedFile3.FilePath]);
    }

    [Fact]
    public void ProjectState_WithHostProject_RootNamespaceChange_UpdatesConfigurationState()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.WithHostProject(s_hostProject with { RootNamespace = "ChangedRootNamespace" });

        // Assert
        Assert.NotSame(state, newState);
        Assert.Equal("ChangedRootNamespace", newState.HostProject.RootNamespace);
    }

    [Fact]
    public void ProjectState_WithHostProject_NoConfigurationChange_Ignored()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.WithHostProject(s_hostProject);

        // Assert
        Assert.Same(state, newState);
    }

    [Fact]
    public void ProjectState_WithConfiguration_Change_UpdatesAllDocuments()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(SomeProjectFile2)
            .AddEmptyDocument(AnotherProjectNestedFile3);

        var documentPathSet = state.Documents.Keys.ToHashSet(FilePathNormalizingComparer.Instance);

        // Act
        var newState = state.WithHostProject(s_hostProject with { Configuration = FallbackRazorConfiguration.MVC_1_0 });

        // Assert
        Assert.Equal(FallbackRazorConfiguration.MVC_1_0, newState.HostProject.Configuration);

        // all documents were updated
        foreach (var filePath in documentPathSet.ToArray())
        {
            AssertDocumentUpdated(filePath, state, newState);
            documentPathSet.Remove(filePath);
        }

        // no other documents - everything was a related document
        Assert.Empty(documentPathSet);
    }

    [Fact]
    public void ProjectState_WithConfiguration_Change_ResetsImportMap()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(SomeProjectFile1);

        // Act
        var newState = state.WithHostProject(s_hostProject with { Configuration = FallbackRazorConfiguration.MVC_1_0 });

        // Assert
        var importMap = Assert.Single(newState.ImportsToRelatedDocuments);
        var documentFilePath = Assert.Single(importMap.Value);
        Assert.Equal(SomeProjectFile1.FilePath, documentFilePath);
    }

    [Fact]
    public void ProjectState_WithProjectWorkspaceState_Changed()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        var newWorkspaceState = ProjectWorkspaceState.Create([]);

        // Act
        var newState = state.WithProjectWorkspaceState(newWorkspaceState);

        // Assert
        Assert.Same(newWorkspaceState, newState.ProjectWorkspaceState);

        // The the tag helpers didn't change
        Assert.Same(state.ProjectEngine, newState.ProjectEngine);

        Assert.NotSame(state.Documents[SomeProjectFile2.FilePath], newState.Documents[SomeProjectFile2.FilePath]);
        Assert.NotSame(state.Documents[AnotherProjectNestedFile3.FilePath], newState.Documents[AnotherProjectNestedFile3.FilePath]);
    }

    [Fact]
    public void ProjectState_WithProjectWorkspaceState_Changed_TagHelpersChanged()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.WithProjectWorkspaceState(ProjectWorkspaceState.Default);

        // Assert
        Assert.Same(ProjectWorkspaceState.Default, newState.ProjectWorkspaceState);

        // The configuration didn't change, but the tag helpers did
        Assert.Same(state.ProjectEngine, newState.ProjectEngine);
        Assert.NotEqual(state.TagHelpers, newState.TagHelpers);
        Assert.NotSame(state.Documents[SomeProjectFile2.FilePath], newState.Documents[SomeProjectFile2.FilePath]);
        Assert.NotSame(state.Documents[AnotherProjectNestedFile3.FilePath], newState.Documents[AnotherProjectNestedFile3.FilePath]);
    }

    [Fact]
    public void ProjectState_WithProjectWorkspaceState_IdenticalState_Caches()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(AnotherProjectNestedFile3)
            .AddEmptyDocument(SomeProjectFile2);

        // Act
        var newState = state.WithProjectWorkspaceState(ProjectWorkspaceState.Create(state.TagHelpers));

        // Assert
        Assert.Same(state, newState);
    }

    [Fact]
    public void ProjectState_WithProjectWorkspaceState_UpdatesAllDocuments()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(SomeProjectFile2)
            .AddEmptyDocument(AnotherProjectNestedFile3);

        var documentPathSet = state.Documents.Keys.ToHashSet(FilePathNormalizingComparer.Instance);

        // Act
        var newState = state.WithProjectWorkspaceState(ProjectWorkspaceState.Default);

        // Assert
        Assert.NotEqual(state.ProjectWorkspaceState, newState.ProjectWorkspaceState);
        Assert.Same(ProjectWorkspaceState.Default, newState.ProjectWorkspaceState);

        // all documents were updated
        foreach (var filePath in documentPathSet.ToArray())
        {
            AssertDocumentUpdated(filePath, state, newState);
            documentPathSet.Remove(filePath);
        }

        // no other documents - everything was a related document
        Assert.Empty(documentPathSet);
    }

    [Fact]
    public void ProjectState_AddImportDocument_UpdatesRelatedDocuments()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(SomeProjectFile1)
            .AddEmptyDocument(SomeProjectFile2)
            .AddEmptyDocument(SomeProjectNestedFile3)
            .AddEmptyDocument(AnotherProjectNestedFile4);

        var documentPathSet = state.Documents.Keys.ToHashSet(FilePathNormalizingComparer.Instance);

        // Act
        var newState = state.AddEmptyDocument(AnotherProjectImportFile);

        // Assert

        // related documents were updated
        var relatedDocumentPaths = newState.ImportsToRelatedDocuments[AnotherProjectImportFile.TargetPath];

        foreach (var filePath in relatedDocumentPaths)
        {
            AssertDocumentUpdated(filePath, state, newState);
            documentPathSet.Remove(filePath);
        }

        // no other documents - everything was a related document

        Assert.Empty(documentPathSet);
    }

    [Fact]
    public void ProjectState_AddImportDocument_UpdatesRelatedDocuments_Nested()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(SomeProjectFile1)
            .AddEmptyDocument(SomeProjectFile2)
            .AddEmptyDocument(SomeProjectNestedFile3)
            .AddEmptyDocument(AnotherProjectNestedFile4);

        var documentPathSet = state.Documents.Keys.ToHashSet(FilePathNormalizingComparer.Instance);

        // Act
        var newState = state.AddEmptyDocument(AnotherProjectNestedImportFile);

        // Assert

        // related documents were updated
        var relatedDocumentPaths = newState.ImportsToRelatedDocuments[AnotherProjectNestedImportFile.TargetPath];

        foreach (var filePath in relatedDocumentPaths)
        {
            AssertDocumentUpdated(filePath, state, newState);
            documentPathSet.Remove(filePath);
        }

        // other documents were not updated
        foreach (var filePath in documentPathSet.ToArray())
        {
            AssertDocumentNotUpdated(filePath, state, newState);
            documentPathSet.Remove(filePath);
        }

        Assert.Empty(documentPathSet);
    }

    [Fact]
    public void ProjectState_WithImportDocumentText_UpdatesRelatedDocuments_TextLoader()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(SomeProjectFile1)
            .AddEmptyDocument(SomeProjectFile2)
            .AddEmptyDocument(SomeProjectNestedFile3)
            .AddEmptyDocument(AnotherProjectNestedFile4)
            .AddEmptyDocument(AnotherProjectNestedImportFile);

        var documentPathSet = state.Documents.Keys.ToHashSet(FilePathNormalizingComparer.Instance);

        // Act
        var newState = state.WithDocumentText(AnotherProjectNestedImportFile.FilePath, s_textLoader);

        // Assert

        // document was updated
        AssertDocumentUpdated(AnotherProjectNestedImportFile.FilePath, state, newState);
        documentPathSet.Remove(AnotherProjectNestedImportFile.FilePath);

        // related documents were updated
        var relatedDocumentPaths = newState.ImportsToRelatedDocuments[AnotherProjectNestedImportFile.TargetPath];

        foreach (var filePath in relatedDocumentPaths)
        {
            AssertDocumentUpdated(filePath, state, newState);
            documentPathSet.Remove(filePath);
        }

        // other documents were not updated
        foreach (var filePath in documentPathSet.ToArray())
        {
            AssertDocumentNotUpdated(filePath, state, newState);
            documentPathSet.Remove(filePath);
        }

        Assert.Empty(documentPathSet);
    }

    [Fact]
    public void ProjectState_WithImportDocumentText_UpdatesRelatedDocuments_Snapshot()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(SomeProjectFile1)
            .AddEmptyDocument(SomeProjectFile2)
            .AddEmptyDocument(SomeProjectNestedFile3)
            .AddEmptyDocument(AnotherProjectNestedFile4)
            .AddEmptyDocument(AnotherProjectNestedImportFile);

        var documentPathSet = state.Documents.Keys.ToHashSet(FilePathNormalizingComparer.Instance);

        // Act
        var newState = state.WithDocumentText(AnotherProjectNestedImportFile.FilePath, s_text);

        // Assert

        // document was updated
        AssertDocumentUpdated(AnotherProjectNestedImportFile.FilePath, state, newState);
        documentPathSet.Remove(AnotherProjectNestedImportFile.FilePath);

        // related documents were updated
        var relatedDocumentPaths = newState.ImportsToRelatedDocuments[AnotherProjectNestedImportFile.TargetPath];

        foreach (var filePath in relatedDocumentPaths)
        {
            AssertDocumentUpdated(filePath, state, newState);
            documentPathSet.Remove(filePath);
        }

        // other documents were not updated
        foreach (var filePath in documentPathSet.ToArray())
        {
            AssertDocumentNotUpdated(filePath, state, newState);
            documentPathSet.Remove(filePath);
        }

        Assert.Empty(documentPathSet);
    }

    [Fact]
    public void ProjectState_RemoveImportDocument_UpdatesRelatedDocuments()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, s_projectEngineFactoryProvider)
            .WithProjectWorkspaceState(s_projectWorkspaceState)
            .AddEmptyDocument(SomeProjectFile1)
            .AddEmptyDocument(SomeProjectFile2)
            .AddEmptyDocument(SomeProjectNestedFile3)
            .AddEmptyDocument(AnotherProjectNestedFile4)
            .AddEmptyDocument(AnotherProjectNestedImportFile);

        var documentPathSet = state.Documents.Keys.ToHashSet(FilePathNormalizingComparer.Instance);
        var relatedDocumentPaths = state.ImportsToRelatedDocuments[AnotherProjectNestedImportFile.TargetPath];

        // Act
        var newState = state.RemoveDocument(AnotherProjectNestedImportFile.FilePath);

        // Assert

        // document was removed
        Assert.False(newState.Documents.ContainsKey(AnotherProjectNestedImportFile.FilePath));
        // TODO: Fix this bug. newState.ImportsToRelatedDocuments should contain an import document after it's been removed.
        Assert.True(newState.ImportsToRelatedDocuments.ContainsKey(AnotherProjectNestedImportFile.TargetPath));
        documentPathSet.Remove(AnotherProjectNestedImportFile.FilePath);

        // related documents were updated
        foreach (var filePath in relatedDocumentPaths)
        {
            AssertDocumentUpdated(filePath, state, newState);
            documentPathSet.Remove(filePath);
        }

        // other documents were not updated
        foreach (var filePath in documentPathSet.ToArray())
        {
            AssertDocumentNotUpdated(filePath, state, newState);
            documentPathSet.Remove(filePath);
        }

        Assert.Empty(documentPathSet);
    }

    private static void AssertSameTagHelpers(TagHelperCollection expected, TagHelperCollection actual)
    {
        Assert.Equal(expected.Count, actual.Count);

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Same(expected[i], actual[i]);
        }
    }

    private static void AssertDocumentUpdated(string filePath, ProjectState oldState, ProjectState newState)
    {
        Assert.True(oldState.Documents.TryGetValue(filePath, out var document));
        Assert.True(newState.Documents.TryGetValue(filePath, out var newDocument));

        Assert.NotSame(document, newDocument);
        Assert.Same(document.HostDocument, newDocument.HostDocument);
        Assert.Equal(document.Version + 1, newDocument.Version);
    }

    private static void AssertDocumentNotUpdated(string filePath, ProjectState oldState, ProjectState newState)
    {
        Assert.True(oldState.Documents.TryGetValue(filePath, out var document));
        Assert.True(newState.Documents.TryGetValue(filePath, out var newDocument));

        Assert.Same(document, newDocument);
    }
}
