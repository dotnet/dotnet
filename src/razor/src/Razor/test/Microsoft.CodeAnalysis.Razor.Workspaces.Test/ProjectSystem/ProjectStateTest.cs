// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class ProjectStateTest : WorkspaceTestBase
{
    private readonly HostDocument[] _documents;
    private readonly HostProject _hostProject;
    private readonly HostProject _hostProjectWithConfigurationChange;
    private readonly ProjectWorkspaceState _projectWorkspaceState;
    private readonly TextLoader _textLoader;
    private readonly SourceText _text;

    public ProjectStateTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _hostProject = TestProjectData.SomeProject with { Configuration = FallbackRazorConfiguration.MVC_2_0 };
        _hostProjectWithConfigurationChange = TestProjectData.SomeProject with { Configuration = FallbackRazorConfiguration.MVC_1_0 };
        _projectWorkspaceState = ProjectWorkspaceState.Create(
            ImmutableArray.Create(
                TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly").Build()));

        _documents = new HostDocument[]
        {
            TestProjectData.SomeProjectFile1,
            TestProjectData.SomeProjectFile2,

            // linked file
            TestProjectData.AnotherProjectNestedFile3,
        };

        _text = SourceText.From("Hello, world!");
        _textLoader = TestMocks.CreateTextLoader(_text);
    }

    protected override void ConfigureProjectEngine(RazorProjectEngineBuilder builder)
    {
        builder.SetImportFeature(new TestImportProjectFeature());
    }

    [Fact]
    public void GetImportDocumentTargetPaths_DoesNotIncludeCurrentImport()
    {
        // Arrange
        var state = ProjectState.Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState);

        // Act
        var paths = state.GetImportDocumentTargetPaths(TestProjectData.SomeProjectComponentImportFile1);

        // Assert
        Assert.DoesNotContain(TestProjectData.SomeProjectComponentImportFile1.TargetPath, paths);
    }

    [Fact]
    public void ProjectState_ConstructedNew()
    {
        // Arrange

        // Act
        var state = ProjectState.Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState);

        // Assert
        Assert.Empty(state.Documents);
        Assert.NotEqual(VersionStamp.Default, state.Version);
    }

    [Fact]
    public void ProjectState_AddDocument_ToEmpty()
    {
        // Arrange
        var original = ProjectState.Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState);

        // Act
        var state = original.AddDocument(_documents[0], DocumentState.EmptyLoader);

        // Assert
        Assert.NotEqual(original.Version, state.Version);

        Assert.Collection(
            state.Documents.OrderBy(kvp => kvp.Key),
            d => Assert.Same(_documents[0], d.Value.HostDocument));
        Assert.NotEqual(original.DocumentCollectionVersion, state.DocumentCollectionVersion);
    }

    [Fact]
    public async Task ProjectState_AddDocument_DocumentIsEmpty()
    {
        // Arrange
        var original = ProjectState.Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState);

        // Act
        var state = original.AddDocument(_documents[0], DocumentState.EmptyLoader);

        // Assert
        var text = await state.Documents[_documents[0].FilePath].GetTextAsync(DisposalToken);
        Assert.Equal(0, text.Length);
    }

    [Fact]
    public void ProjectState_AddDocument_ToProjectWithDocuments()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Act
        var state = original.AddDocument(_documents[0], DocumentState.EmptyLoader);

        // Assert
        Assert.NotEqual(original.Version, state.Version);

        Assert.Collection(
            state.Documents.OrderBy(kvp => kvp.Key),
            d => Assert.Same(_documents[2], d.Value.HostDocument),
            d => Assert.Same(_documents[0], d.Value.HostDocument),
            d => Assert.Same(_documents[1], d.Value.HostDocument));
        Assert.NotEqual(original.DocumentCollectionVersion, state.DocumentCollectionVersion);
    }

    [Fact]
    public void ProjectState_AddDocument_TracksImports()
    {
        // Arrange

        // Act
        var state = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(TestProjectData.SomeProjectFile1, DocumentState.EmptyLoader)
            .AddDocument(TestProjectData.SomeProjectFile2, DocumentState.EmptyLoader)
            .AddDocument(TestProjectData.SomeProjectNestedFile3, DocumentState.EmptyLoader)
            .AddDocument(TestProjectData.AnotherProjectNestedFile4, DocumentState.EmptyLoader);

        // Assert
        Assert.Collection(
            state.ImportsToRelatedDocuments.OrderBy(kvp => kvp.Key),
            kvp =>
            {
                Assert.Equal(TestProjectData.SomeProjectImportFile.TargetPath, kvp.Key);
                Assert.Equal(
                    new string[]
                    {
                        TestProjectData.AnotherProjectNestedFile4.FilePath,
                        TestProjectData.SomeProjectFile1.FilePath,
                        TestProjectData.SomeProjectFile2.FilePath,
                        TestProjectData.SomeProjectNestedFile3.FilePath,
                    },
                    kvp.Value.OrderBy(f => f));
            },
            kvp =>
            {
                Assert.Equal(TestProjectData.SomeProjectNestedImportFile.TargetPath, kvp.Key);
                Assert.Equal(
                    new string[]
                    {
                        TestProjectData.AnotherProjectNestedFile4.FilePath,
                        TestProjectData.SomeProjectNestedFile3.FilePath,
                    },
                    kvp.Value.OrderBy(f => f));
            });
    }

    [Fact]
    public void ProjectState_AddDocument_TracksImports_AddImportFile()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(TestProjectData.SomeProjectFile1, DocumentState.EmptyLoader)
            .AddDocument(TestProjectData.SomeProjectFile2, DocumentState.EmptyLoader)
            .AddDocument(TestProjectData.SomeProjectNestedFile3, DocumentState.EmptyLoader)
            .AddDocument(TestProjectData.AnotherProjectNestedFile4, DocumentState.EmptyLoader);

        // Act
        var state = original
            .AddDocument(TestProjectData.AnotherProjectImportFile, DocumentState.EmptyLoader);

        // Assert
        Assert.Collection(
            state.ImportsToRelatedDocuments.OrderBy(kvp => kvp.Key),
            kvp =>
            {
                Assert.Equal(TestProjectData.SomeProjectImportFile.TargetPath, kvp.Key);
                Assert.Equal(
                    new string[]
                    {
                        TestProjectData.AnotherProjectNestedFile4.FilePath,
                        TestProjectData.SomeProjectFile1.FilePath,
                        TestProjectData.SomeProjectFile2.FilePath,
                        TestProjectData.SomeProjectNestedFile3.FilePath,
                    },
                    kvp.Value.OrderBy(f => f));
            },
            kvp =>
            {
                Assert.Equal(TestProjectData.SomeProjectNestedImportFile.TargetPath, kvp.Key);
                Assert.Equal(
                    new string[]
                    {
                        TestProjectData.AnotherProjectNestedFile4.FilePath,
                        TestProjectData.SomeProjectNestedFile3.FilePath,
                    },
                    kvp.Value.OrderBy(f => f));
            });
    }

    [Fact]
    public void ProjectState_AddDocument_RetainsComputedState()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Force init
        var originalTagHelpers = original.TagHelpers;
        var originalProjectWorkspaceStateVersion = original.ProjectWorkspaceStateVersion;

        // Act
        var state = original.AddDocument(_documents[0], DocumentState.EmptyLoader);

        // Assert
        var actualTagHelpers = state.TagHelpers;
        var actualProjectWorkspaceStateVersion = state.ProjectWorkspaceStateVersion;

        Assert.Same(original.ProjectEngine, state.ProjectEngine);

        Assert.Equal(originalTagHelpers.Length, actualTagHelpers.Length);
        for (var i = 0; i < originalTagHelpers.Length; i++)
        {
            Assert.Same(originalTagHelpers[i], actualTagHelpers[i]);
        }

        Assert.Equal(originalProjectWorkspaceStateVersion, actualProjectWorkspaceStateVersion);

        Assert.Same(original.Documents[_documents[1].FilePath], state.Documents[_documents[1].FilePath]);
        Assert.Same(original.Documents[_documents[2].FilePath], state.Documents[_documents[2].FilePath]);
    }

    [Fact]
    public void ProjectState_AddDocument_DuplicateIgnored()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Act
        var state = original.AddDocument(new HostDocument(_documents[1].FilePath, "SomePath.cshtml"), DocumentState.EmptyLoader);

        // Assert
        Assert.Same(original, state);
    }

    [Fact]
    public async Task ProjectState_WithDocumentText_Loader()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Act
        var state = original.WithDocumentText(_documents[1].FilePath, _textLoader);

        // Assert
        Assert.NotEqual(original.Version, state.Version);

        var text = await state.Documents[_documents[1].FilePath].GetTextAsync(DisposalToken);
        Assert.Same(_text, text);

        Assert.Equal(original.DocumentCollectionVersion, state.DocumentCollectionVersion);
    }

    [Fact]
    public async Task ProjectState_WithDocumentText_Snapshot()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Act
        var state = original.WithDocumentText(_documents[1].FilePath, _text);

        // Assert
        Assert.NotEqual(original.Version, state.Version);

        var text = await state.Documents[_documents[1].FilePath].GetTextAsync(DisposalToken);
        Assert.Same(_text, text);

        Assert.Equal(original.DocumentCollectionVersion, state.DocumentCollectionVersion);
    }

    [Fact]
    public void ProjectState_WithDocumentText_Loader_RetainsComputedState()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Force init
        var originalTagHelpers = original.TagHelpers;
        var originalProjectWorkspaceStateVersion = original.ProjectWorkspaceStateVersion;

        // Act
        var state = original.WithDocumentText(_documents[1].FilePath, _textLoader);

        // Assert
        var actualTagHelpers = state.TagHelpers;
        var actualProjectWorkspaceStateVersion = state.ProjectWorkspaceStateVersion;

        Assert.Same(original.ProjectEngine, state.ProjectEngine);

        Assert.Equal(originalTagHelpers.Length, actualTagHelpers.Length);
        for (var i = 0; i < originalTagHelpers.Length; i++)
        {
            Assert.Same(originalTagHelpers[i], actualTagHelpers[i]);
        }

        Assert.Equal(originalProjectWorkspaceStateVersion, actualProjectWorkspaceStateVersion);

        Assert.NotSame(original.Documents[_documents[1].FilePath], state.Documents[_documents[1].FilePath]);
    }

    [Fact]
    public void ProjectState_WithDocumentText_Snapshot_RetainsComputedState()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Force init
        var originalTagHelpers = original.TagHelpers;
        var originalProjectWorkspaceStateVersion = original.ProjectWorkspaceStateVersion;

        // Act
        var state = original.WithDocumentText(_documents[1].FilePath, _text);

        // Assert
        var actualTagHelpers = state.TagHelpers;
        var actualProjectWorkspaceStateVersion = state.ProjectWorkspaceStateVersion;

        Assert.Same(original.ProjectEngine, state.ProjectEngine);

        Assert.Equal(originalTagHelpers.Length, actualTagHelpers.Length);
        for (var i = 0; i < originalTagHelpers.Length; i++)
        {
            Assert.Same(originalTagHelpers[i], actualTagHelpers[i]);
        }

        Assert.Equal(originalProjectWorkspaceStateVersion, actualProjectWorkspaceStateVersion);

        Assert.NotSame(original.Documents[_documents[1].FilePath], state.Documents[_documents[1].FilePath]);
    }

    [Fact]
    public void ProjectState_WithDocumentText_Loader_NotFoundIgnored()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Act
        var state = original.WithDocumentText(_documents[0].FilePath, _textLoader);

        // Assert
        Assert.Same(original, state);
    }

    [Fact]
    public void ProjectState_WithDocumentText_Snapshot_NotFoundIgnored()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Act
        var state = original.WithDocumentText(_documents[0].FilePath, _text);

        // Assert
        Assert.Same(original, state);
    }

    [Fact]
    public void ProjectState_RemoveDocument_FromProjectWithDocuments()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Act
        var state = original.RemoveDocument(_documents[1].FilePath);

        // Assert
        Assert.NotEqual(original.Version, state.Version);

        Assert.Collection(
            state.Documents.OrderBy(kvp => kvp.Key),
            d => Assert.Same(_documents[2], d.Value.HostDocument));

        Assert.NotEqual(original.DocumentCollectionVersion, state.DocumentCollectionVersion);
    }

    [Fact]
    public void ProjectState_RemoveDocument_TracksImports()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(TestProjectData.SomeProjectFile1, DocumentState.EmptyLoader)
            .AddDocument(TestProjectData.SomeProjectFile2, DocumentState.EmptyLoader)
            .AddDocument(TestProjectData.SomeProjectNestedFile3, DocumentState.EmptyLoader)
            .AddDocument(TestProjectData.AnotherProjectNestedFile4, DocumentState.EmptyLoader);

        // Act
        var state = original.RemoveDocument(TestProjectData.SomeProjectNestedFile3.FilePath);

        // Assert
        Assert.Collection(
            state.ImportsToRelatedDocuments.OrderBy(kvp => kvp.Key),
            kvp =>
            {
                Assert.Equal(TestProjectData.SomeProjectImportFile.TargetPath, kvp.Key);
                Assert.Equal(
                    new string[]
                    {
                        TestProjectData.AnotherProjectNestedFile4.FilePath,
                        TestProjectData.SomeProjectFile1.FilePath,
                        TestProjectData.SomeProjectFile2.FilePath,
                    },
                    kvp.Value.OrderBy(f => f));
            },
            kvp =>
            {
                Assert.Equal(TestProjectData.SomeProjectNestedImportFile.TargetPath, kvp.Key);
                Assert.Equal(
                    new string[]
                    {
                        TestProjectData.AnotherProjectNestedFile4.FilePath,
                    },
                    kvp.Value.OrderBy(f => f));
            });
    }

    [Fact]
    public void ProjectState_RemoveDocument_TracksImports_RemoveAllDocuments()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(TestProjectData.SomeProjectFile1, DocumentState.EmptyLoader)
            .AddDocument(TestProjectData.SomeProjectFile2, DocumentState.EmptyLoader)
            .AddDocument(TestProjectData.SomeProjectNestedFile3, DocumentState.EmptyLoader)
            .AddDocument(TestProjectData.AnotherProjectNestedFile4, DocumentState.EmptyLoader);

        // Act
        var state = original
            .RemoveDocument(TestProjectData.SomeProjectFile1.FilePath)
            .RemoveDocument(TestProjectData.SomeProjectFile2.FilePath)
            .RemoveDocument(TestProjectData.SomeProjectNestedFile3.FilePath)
            .RemoveDocument(TestProjectData.AnotherProjectNestedFile4.FilePath);

        // Assert
        Assert.Empty(state.Documents);
        Assert.Empty(state.ImportsToRelatedDocuments);
    }

    [Fact]
    public void ProjectState_RemoveDocument_RetainsComputedState()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Force init
        var originalTagHelpers = original.TagHelpers;
        var originalProjectWorkspaceStateVersion = original.ProjectWorkspaceStateVersion;

        // Act
        var state = original.RemoveDocument(_documents[2].FilePath);

        // Assert
        var actualTagHelpers = state.TagHelpers;
        var actualProjectWorkspaceStateVersion = state.ProjectWorkspaceStateVersion;

        Assert.Same(original.ProjectEngine, state.ProjectEngine);

        Assert.Equal(originalTagHelpers.Length, actualTagHelpers.Length);
        for (var i = 0; i < originalTagHelpers.Length; i++)
        {
            Assert.Same(originalTagHelpers[i], actualTagHelpers[i]);
        }

        Assert.Equal(originalProjectWorkspaceStateVersion, actualProjectWorkspaceStateVersion);

        Assert.Same(original.Documents[_documents[1].FilePath], state.Documents[_documents[1].FilePath]);
    }

    [Fact]
    public void ProjectState_RemoveDocument_NotFoundIgnored()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Act
        var state = original.RemoveDocument(_documents[0].FilePath);

        // Assert
        Assert.Same(original, state);
    }

    [Fact]
    public void ProjectState_WithHostProject_ConfigurationChange_UpdatesConfigurationState()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Force init
        var originalTagHelpers = original.TagHelpers;
        var originalProjectWorkspaceStateVersion = original.ConfigurationVersion;

        // Act
        var state = original.WithHostProject(_hostProjectWithConfigurationChange);

        // Assert
        Assert.NotEqual(original.Version, state.Version);
        Assert.Same(_hostProjectWithConfigurationChange, state.HostProject);

        var actualTagHelpers = state.TagHelpers;
        var actualProjectWorkspaceStateVersion = state.ConfigurationVersion;

        Assert.NotSame(original.ProjectEngine, state.ProjectEngine);

        Assert.Equal(originalTagHelpers.Length, actualTagHelpers.Length);
        for (var i = 0; i < originalTagHelpers.Length; i++)
        {
            Assert.Same(originalTagHelpers[i], actualTagHelpers[i]);
        }

        Assert.NotEqual(originalProjectWorkspaceStateVersion, actualProjectWorkspaceStateVersion);

        Assert.NotSame(original.Documents[_documents[1].FilePath], state.Documents[_documents[1].FilePath]);
        Assert.NotSame(original.Documents[_documents[2].FilePath], state.Documents[_documents[2].FilePath]);

        Assert.NotEqual(original.DocumentCollectionVersion, state.DocumentCollectionVersion);
    }

    [Fact]
    public void ProjectState_WithHostProject_RootNamespaceChange_UpdatesConfigurationState()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);
        var hostProjectWithRootNamespaceChange = original.HostProject with { RootNamespace = "ChangedRootNamespace" };

        // Force init
        _ = original.TagHelpers;
        _ = original.ConfigurationVersion;

        // Act
        var state = original.WithHostProject(hostProjectWithRootNamespaceChange);

        // Assert
        Assert.NotSame(original, state);
    }

    [Fact]
    public void ProjectState_WithHostProject_NoConfigurationChange_Ignored()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Force init
        _ = original.ProjectWorkspaceStateVersion;

        // Act
        var state = original.WithHostProject(_hostProject);

        // Assert
        Assert.Same(original, state);
    }

    [Fact]
    public void ProjectState_WithHostProject_CallsConfigurationChangeOnDocumentState()
    {
        // Arrange
        var callCount = 0;

        var documents = ImmutableDictionary.CreateBuilder<string, DocumentState>(FilePathComparer.Instance);
        documents[_documents[1].FilePath] = TestDocumentState.Create(_documents[1], onConfigurationChange: () => callCount++);
        documents[_documents[2].FilePath] = TestDocumentState.Create(_documents[2], onConfigurationChange: () => callCount++);

        var original = ProjectState.Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState);
        original.Documents = documents.ToImmutable();

        // Act
        var state = original.WithHostProject(_hostProjectWithConfigurationChange);

        // Assert
        Assert.NotEqual(original.Version, state.Version);
        Assert.Same(_hostProjectWithConfigurationChange, state.HostProject);
        Assert.Equal(2, callCount);
    }

    [Fact]
    public void ProjectState_WithHostProject_ResetsImportedDocuments()
    {
        // Arrange
        var original = ProjectState.Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState);
        original = original.AddDocument(TestProjectData.SomeProjectFile1, DocumentState.EmptyLoader);

        // Act
        var state = original.WithHostProject(_hostProjectWithConfigurationChange);

        // Assert
        var importMap = Assert.Single(state.ImportsToRelatedDocuments);
        var documentFilePath = Assert.Single(importMap.Value);
        Assert.Equal(TestProjectData.SomeProjectFile1.FilePath, documentFilePath);
    }

    [Fact]
    public void ProjectState_WithProjectWorkspaceState_Changed()
    {
        // Arrange
        var original = ProjectState.Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Force init
        var originalTagHelpers = original.TagHelpers;
        var originalProjectWorkspaceStateVersion = original.ProjectWorkspaceStateVersion;

        var changed = ProjectWorkspaceState.Create(_projectWorkspaceState.TagHelpers, LanguageVersion.CSharp6);

        // Act
        var state = original.WithProjectWorkspaceState(changed);

        // Assert
        Assert.NotEqual(original.Version, state.Version);
        Assert.Same(changed, state.ProjectWorkspaceState);

        var actualTagHelpers = state.TagHelpers;
        var actualProjectWorkspaceStateVersion = state.ProjectWorkspaceStateVersion;

        // The C# language version changed, and the tag helpers didn't change
        Assert.NotSame(original.ProjectEngine, state.ProjectEngine);

        Assert.Equal(originalTagHelpers.Length, actualTagHelpers.Length);
        for (var i = 0; i < originalTagHelpers.Length; i++)
        {
            Assert.Same(originalTagHelpers[i], actualTagHelpers[i]);
        }

        Assert.NotEqual(originalProjectWorkspaceStateVersion, actualProjectWorkspaceStateVersion);

        Assert.NotSame(original.Documents[_documents[1].FilePath], state.Documents[_documents[1].FilePath]);
        Assert.NotSame(original.Documents[_documents[2].FilePath], state.Documents[_documents[2].FilePath]);
    }

    [Fact]
    public void ProjectState_WithProjectWorkspaceState_Changed_TagHelpersChanged()
    {
        // Arrange
        var original = ProjectState.Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Force init
        var originalTagHelpers = original.TagHelpers;
        var originalProjectWorkspaceStateVersion = original.ProjectWorkspaceStateVersion;

        var changed = ProjectWorkspaceState.Default;

        // Act
        var state = original.WithProjectWorkspaceState(changed);

        // Assert
        Assert.NotEqual(original.Version, state.Version);
        Assert.Same(changed, state.ProjectWorkspaceState);

        var actualTagHelpers = state.TagHelpers;
        var actualProjectWorkspaceStateVersion = state.ProjectWorkspaceStateVersion;

        // The configuration didn't change, but the tag helpers did
        Assert.Same(original.ProjectEngine, state.ProjectEngine);
        Assert.NotEqual(originalTagHelpers, actualTagHelpers);
        Assert.NotEqual(originalProjectWorkspaceStateVersion, actualProjectWorkspaceStateVersion);
        Assert.Equal(state.Version, actualProjectWorkspaceStateVersion);

        Assert.NotSame(original.Documents[_documents[1].FilePath], state.Documents[_documents[1].FilePath]);
        Assert.NotSame(original.Documents[_documents[2].FilePath], state.Documents[_documents[2].FilePath]);
    }

    [Fact]
    public void ProjectState_WithProjectWorkspaceState_IdenticalState_Caches()
    {
        // Arrange
        var original = ProjectState.Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState)
            .AddDocument(_documents[2], DocumentState.EmptyLoader)
            .AddDocument(_documents[1], DocumentState.EmptyLoader);

        // Force init
        _ = original.TagHelpers;
        _ = original.ProjectWorkspaceStateVersion;

        // Act
        var state = original.WithProjectWorkspaceState(ProjectWorkspaceState.Create(original.TagHelpers, original.CSharpLanguageVersion));

        // Assert
        Assert.Same(original, state);
    }

    [Fact]
    public void ProjectState_WithProjectWorkspaceState_CallsWorkspaceProjectChangeOnDocumentState()
    {
        // Arrange
        var callCount = 0;

        var documents = ImmutableDictionary.CreateBuilder<string, DocumentState>(FilePathComparer.Instance);
        documents[_documents[1].FilePath] = TestDocumentState.Create(_documents[1], onProjectWorkspaceStateChange: () => callCount++);
        documents[_documents[2].FilePath] = TestDocumentState.Create(_documents[2], onProjectWorkspaceStateChange: () => callCount++);

        var original = ProjectState.Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState);
        original.Documents = documents.ToImmutable();

        // Act
        var state = original.WithProjectWorkspaceState(ProjectWorkspaceState.Default);

        // Assert
        Assert.NotEqual(original.Version, state.Version);
        Assert.Equal(2, callCount);
    }

    [Fact]
    public void ProjectState_AddDocument_Import_CallsImportsChanged()
    {
        // Arrange
        var callCount = 0;

        var document1 = TestProjectData.SomeProjectFile1;
        var document2 = TestProjectData.SomeProjectFile2;
        var document3 = TestProjectData.SomeProjectNestedFile3;
        var document4 = TestProjectData.AnotherProjectNestedFile4;

        var documents = ImmutableDictionary.CreateBuilder<string, DocumentState>(FilePathComparer.Instance);
        documents[document1.FilePath] = TestDocumentState.Create(document1, onImportsChange: () => callCount++);
        documents[document2.FilePath] = TestDocumentState.Create(document2, onImportsChange: () => callCount++);
        documents[document3.FilePath] = TestDocumentState.Create(document3, onImportsChange: () => callCount++);
        documents[document4.FilePath] = TestDocumentState.Create(document4, onImportsChange: () => callCount++);

        var importsToRelatedDocuments = ImmutableDictionary.CreateBuilder<string, ImmutableArray<string>>(FilePathComparer.Instance);
        importsToRelatedDocuments.Add(
            TestProjectData.SomeProjectImportFile.TargetPath,
            ImmutableArray.Create(
                TestProjectData.SomeProjectFile1.FilePath,
                TestProjectData.SomeProjectFile2.FilePath,
                TestProjectData.SomeProjectNestedFile3.FilePath,
                TestProjectData.AnotherProjectNestedFile4.FilePath));
        importsToRelatedDocuments.Add(
            TestProjectData.SomeProjectNestedImportFile.TargetPath,
            ImmutableArray.Create(
                TestProjectData.SomeProjectNestedFile3.FilePath,
                TestProjectData.AnotherProjectNestedFile4.FilePath));

        var original = ProjectState.Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState);
        original.Documents = documents.ToImmutable();
        original.ImportsToRelatedDocuments = importsToRelatedDocuments.ToImmutable();

        // Act
        var state = original.AddDocument(TestProjectData.AnotherProjectImportFile, DocumentState.EmptyLoader);

        // Assert
        Assert.NotEqual(original.Version, state.Version);
        Assert.Equal(4, callCount);
    }

    [Fact]
    public void ProjectState_AddDocument_Import_CallsImportsChanged_Nested()
    {
        // Arrange
        var callCount = 0;

        var document1 = TestProjectData.SomeProjectFile1;
        var document2 = TestProjectData.SomeProjectFile2;
        var document3 = TestProjectData.SomeProjectNestedFile3;
        var document4 = TestProjectData.AnotherProjectNestedFile4;

        var documents = ImmutableDictionary.CreateBuilder<string, DocumentState>(FilePathComparer.Instance);
        documents[document1.FilePath] = TestDocumentState.Create(document1, onImportsChange: () => callCount++);
        documents[document2.FilePath] = TestDocumentState.Create(document2, onImportsChange: () => callCount++);
        documents[document3.FilePath] = TestDocumentState.Create(document3, onImportsChange: () => callCount++);
        documents[document4.FilePath] = TestDocumentState.Create(document4, onImportsChange: () => callCount++);

        var importsToRelatedDocuments = ImmutableDictionary.CreateBuilder<string, ImmutableArray<string>>(FilePathComparer.Instance);
        importsToRelatedDocuments.Add(
            TestProjectData.SomeProjectImportFile.TargetPath,
            ImmutableArray.Create(
                TestProjectData.SomeProjectFile1.FilePath,
                TestProjectData.SomeProjectFile2.FilePath,
                TestProjectData.SomeProjectNestedFile3.FilePath,
                TestProjectData.AnotherProjectNestedFile4.FilePath));
        importsToRelatedDocuments.Add(
            TestProjectData.SomeProjectNestedImportFile.TargetPath,
            ImmutableArray.Create(
                TestProjectData.SomeProjectNestedFile3.FilePath,
                TestProjectData.AnotherProjectNestedFile4.FilePath));

        var original = ProjectState.Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState);
        original.Documents = documents.ToImmutable();
        original.ImportsToRelatedDocuments = importsToRelatedDocuments.ToImmutable();

        // Act
        var state = original.AddDocument(TestProjectData.AnotherProjectNestedImportFile, DocumentState.EmptyLoader);

        // Assert
        Assert.NotEqual(original.Version, state.Version);
        Assert.Equal(2, callCount);
    }

    [Fact]
    public void ProjectState_WithDocumentText_Loader_Import_CallsImportsChanged()
    {
        // Arrange
        var callCount = 0;

        var document1 = TestProjectData.SomeProjectFile1;
        var document2 = TestProjectData.SomeProjectFile2;
        var document3 = TestProjectData.SomeProjectNestedFile3;
        var document4 = TestProjectData.AnotherProjectNestedFile4;
        var document5 = TestProjectData.AnotherProjectNestedImportFile;

        var documents = ImmutableDictionary.CreateBuilder<string, DocumentState>(FilePathComparer.Instance);
        documents[document1.FilePath] = TestDocumentState.Create(document1, onImportsChange: () => callCount++);
        documents[document2.FilePath] = TestDocumentState.Create(document2, onImportsChange: () => callCount++);
        documents[document3.FilePath] = TestDocumentState.Create(document3, onImportsChange: () => callCount++);
        documents[document4.FilePath] = TestDocumentState.Create(document4, onImportsChange: () => callCount++);
        documents[document5.FilePath] = TestDocumentState.Create(document5, onImportsChange: () => callCount++);

        var importsToRelatedDocuments = ImmutableDictionary.CreateBuilder<string, ImmutableArray<string>>(FilePathComparer.Instance);
        importsToRelatedDocuments.Add(
            TestProjectData.SomeProjectImportFile.TargetPath,
            ImmutableArray.Create(
                TestProjectData.SomeProjectFile1.FilePath,
                TestProjectData.SomeProjectFile2.FilePath,
                TestProjectData.SomeProjectNestedFile3.FilePath,
                TestProjectData.AnotherProjectNestedFile4.FilePath,
                TestProjectData.AnotherProjectNestedImportFile.FilePath));
        importsToRelatedDocuments.Add(
            TestProjectData.SomeProjectNestedImportFile.TargetPath,
            ImmutableArray.Create(
                TestProjectData.SomeProjectNestedFile3.FilePath,
                TestProjectData.AnotherProjectNestedFile4.FilePath));

        var original = ProjectState.Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState);
        original.Documents = documents.ToImmutable();
        original.ImportsToRelatedDocuments = importsToRelatedDocuments.ToImmutable();

        // Act
        var state = original.WithDocumentText(document5.FilePath, DocumentState.EmptyLoader);

        // Assert
        Assert.NotEqual(original.Version, state.Version);
        Assert.Equal(2, callCount);
    }

    [Fact]
    public void ProjectState_WithDocumentText_Snapshot_Import_CallsImportsChanged()
    {
        // Arrange
        var callCount = 0;

        var document1 = TestProjectData.SomeProjectFile1;
        var document2 = TestProjectData.SomeProjectFile2;
        var document3 = TestProjectData.SomeProjectNestedFile3;
        var document4 = TestProjectData.AnotherProjectNestedFile4;
        var document5 = TestProjectData.AnotherProjectNestedImportFile;

        var documents = ImmutableDictionary.CreateBuilder<string, DocumentState>(FilePathComparer.Instance);
        documents[document1.FilePath] = TestDocumentState.Create(document1, onImportsChange: () => callCount++);
        documents[document2.FilePath] = TestDocumentState.Create(document2, onImportsChange: () => callCount++);
        documents[document3.FilePath] = TestDocumentState.Create(document3, onImportsChange: () => callCount++);
        documents[document4.FilePath] = TestDocumentState.Create(document4, onImportsChange: () => callCount++);
        documents[document5.FilePath] = TestDocumentState.Create(document5, onImportsChange: () => callCount++);

        var importsToRelatedDocuments = ImmutableDictionary.CreateBuilder<string, ImmutableArray<string>>(FilePathComparer.Instance);
        importsToRelatedDocuments.Add(
            TestProjectData.SomeProjectImportFile.TargetPath,
            ImmutableArray.Create(
                TestProjectData.SomeProjectFile1.FilePath,
                TestProjectData.SomeProjectFile2.FilePath,
                TestProjectData.SomeProjectNestedFile3.FilePath,
                TestProjectData.AnotherProjectNestedFile4.FilePath,
                TestProjectData.AnotherProjectNestedImportFile.FilePath));
        importsToRelatedDocuments.Add(
            TestProjectData.SomeProjectNestedImportFile.TargetPath,
            ImmutableArray.Create(
                TestProjectData.SomeProjectNestedFile3.FilePath,
                TestProjectData.AnotherProjectNestedFile4.FilePath));

        var original = ProjectState.Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState);
        original.Documents = documents.ToImmutable();
        original.ImportsToRelatedDocuments = importsToRelatedDocuments.ToImmutable();

        // Act
        var state = original.WithDocumentText(document5.FilePath, _text);

        // Assert
        Assert.NotEqual(original.Version, state.Version);
        Assert.Equal(2, callCount);
    }

    [Fact]
    public void ProjectState_RemoveDocument_Import_CallsImportsChanged()
    {
        // Arrange
        var callCount = 0;

        var document1 = TestProjectData.SomeProjectFile1;
        var document2 = TestProjectData.SomeProjectFile2;
        var document3 = TestProjectData.SomeProjectNestedFile3;
        var document4 = TestProjectData.AnotherProjectNestedFile4;
        var document5 = TestProjectData.AnotherProjectNestedImportFile;

        var documents = ImmutableDictionary.CreateBuilder<string, DocumentState>(FilePathComparer.Instance);
        documents[document1.FilePath] = TestDocumentState.Create(document1, onImportsChange: () => callCount++);
        documents[document2.FilePath] = TestDocumentState.Create(document2, onImportsChange: () => callCount++);
        documents[document3.FilePath] = TestDocumentState.Create(document3, onImportsChange: () => callCount++);
        documents[document4.FilePath] = TestDocumentState.Create(document4, onImportsChange: () => callCount++);
        documents[document5.FilePath] = TestDocumentState.Create(document5, onImportsChange: () => callCount++);

        var importsToRelatedDocuments = ImmutableDictionary.CreateBuilder<string, ImmutableArray<string>>(FilePathComparer.Instance);
        importsToRelatedDocuments.Add(
            TestProjectData.SomeProjectImportFile.TargetPath,
            ImmutableArray.Create(
                TestProjectData.SomeProjectFile1.FilePath,
                TestProjectData.SomeProjectFile2.FilePath,
                TestProjectData.SomeProjectNestedFile3.FilePath,
                TestProjectData.AnotherProjectNestedFile4.FilePath,
                TestProjectData.AnotherProjectNestedImportFile.FilePath));
        importsToRelatedDocuments.Add(
            TestProjectData.SomeProjectNestedImportFile.TargetPath,
            ImmutableArray.Create(
                TestProjectData.SomeProjectNestedFile3.FilePath,
                TestProjectData.AnotherProjectNestedFile4.FilePath));

        var original = ProjectState.Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, _projectWorkspaceState);
        original.Documents = documents.ToImmutable();
        original.ImportsToRelatedDocuments = importsToRelatedDocuments.ToImmutable();

        // Act
        var state = original.RemoveDocument(document5.FilePath);

        // Assert
        Assert.NotEqual(original.Version, state.Version);
        Assert.Equal(2, callCount);
    }

    private class TestDocumentState : DocumentState
    {
        public static TestDocumentState Create(
            HostDocument hostDocument,
            TextLoader? loader = null,
            Action? onTextChange = null,
            Action? onTextLoaderChange = null,
            Action? onConfigurationChange = null,
            Action? onImportsChange = null,
            Action? onProjectWorkspaceStateChange = null)
        {
            return new TestDocumentState(
                hostDocument,
                loader,
                onTextChange,
                onTextLoaderChange,
                onConfigurationChange,
                onImportsChange,
                onProjectWorkspaceStateChange);
        }

        private readonly Action? _onTextChange;
        private readonly Action? _onTextLoaderChange;
        private readonly Action? _onConfigurationChange;
        private readonly Action? _onImportsChange;
        private readonly Action? _onProjectWorkspaceStateChange;

        private TestDocumentState(
            HostDocument hostDocument,
            TextLoader? loader,
            Action? onTextChange,
            Action? onTextLoaderChange,
            Action? onConfigurationChange,
            Action? onImportsChange,
            Action? onProjectWorkspaceStateChange)
            : base(hostDocument, version: 1, loader ?? EmptyLoader)
        {
            _onTextChange = onTextChange;
            _onTextLoaderChange = onTextLoaderChange;
            _onConfigurationChange = onConfigurationChange;
            _onImportsChange = onImportsChange;
            _onProjectWorkspaceStateChange = onProjectWorkspaceStateChange;
        }

        public override DocumentState WithText(SourceText sourceText, VersionStamp textVersion)
        {
            _onTextChange?.Invoke();
            return base.WithText(sourceText, textVersion);
        }

        public override DocumentState WithTextLoader(TextLoader loader)
        {
            _onTextLoaderChange?.Invoke();
            return base.WithTextLoader(loader);
        }

        public override DocumentState WithConfigurationChange()
        {
            _onConfigurationChange?.Invoke();
            return base.WithConfigurationChange();
        }

        public override DocumentState WithImportsChange()
        {
            _onImportsChange?.Invoke();
            return base.WithImportsChange();
        }

        public override DocumentState WithProjectWorkspaceStateChange()
        {
            _onProjectWorkspaceStateChange?.Invoke();
            return base.WithProjectWorkspaceStateChange();
        }
    }
}
