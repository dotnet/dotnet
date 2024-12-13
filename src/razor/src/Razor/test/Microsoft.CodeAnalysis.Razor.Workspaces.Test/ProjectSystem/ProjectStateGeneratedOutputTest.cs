// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class ProjectStateGeneratedOutputTest : WorkspaceTestBase
{
    private readonly HostDocument _hostDocument;
    private readonly HostProject _hostProject;
    private readonly HostProject _hostProjectWithConfigurationChange;
    private readonly ImmutableArray<TagHelperDescriptor> _someTagHelpers;

    public ProjectStateGeneratedOutputTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _hostProject = TestProjectData.SomeProject with { Configuration = FallbackRazorConfiguration.MVC_2_0 };
        _hostProjectWithConfigurationChange = TestProjectData.SomeProject with { Configuration = FallbackRazorConfiguration.MVC_1_0 };

        _someTagHelpers = ImmutableArray.Create(
            TagHelperDescriptorBuilder.Create("Test1", "TestAssembly").Build());

        _hostDocument = TestProjectData.SomeProjectFile1;
    }

    protected override void ConfigureProjectEngine(RazorProjectEngineBuilder builder)
    {
        builder.SetImportFeature(new TestImportProjectFeature());
    }

    [Fact]
    public async Task AddDocument_CachesOutput()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, ProjectWorkspaceState.Default)
            .AddDocument(_hostDocument, DocumentState.EmptyLoader);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument, DisposalToken);

        // Act
        var state = original.AddDocument(TestProjectData.AnotherProjectFile1, DocumentState.EmptyLoader);

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument, DisposalToken);
        Assert.Same(originalOutput, actualOutput);
        Assert.Equal(originalInputVersion, actualInputVersion);
    }

    [Fact]
    public async Task AddDocument_Import_DoesNotCacheOutput()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, ProjectWorkspaceState.Default)
            .AddDocument(_hostDocument, DocumentState.EmptyLoader);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument, DisposalToken);

        // Act
        var state = original.AddDocument(TestProjectData.SomeProjectImportFile, DocumentState.EmptyLoader);

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument, DisposalToken);
        Assert.NotSame(originalOutput, actualOutput);
        Assert.NotEqual(originalInputVersion, actualInputVersion);
        Assert.Equal(state.DocumentCollectionVersion, actualInputVersion);
    }

    [Fact]
    public async Task WithDocumentText_DoesNotCacheOutput()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, ProjectWorkspaceState.Default)
            .AddDocument(_hostDocument, DocumentState.EmptyLoader)
            .AddDocument(TestProjectData.SomeProjectImportFile, DocumentState.EmptyLoader);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument, DisposalToken);

        // Act
        var version = VersionStamp.Create();
        var state = original.WithDocumentText(_hostDocument.FilePath, TestMocks.CreateTextLoader("@using System", version));

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument, DisposalToken);
        Assert.NotSame(originalOutput, actualOutput);
        Assert.NotEqual(originalInputVersion, actualInputVersion);
        Assert.Equal(version, actualInputVersion);
    }

    [Fact]
    public async Task WithDocumentText_Import_DoesNotCacheOutput()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, ProjectWorkspaceState.Default)
            .AddDocument(_hostDocument, DocumentState.EmptyLoader)
            .AddDocument(TestProjectData.SomeProjectImportFile, DocumentState.EmptyLoader);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument, DisposalToken);

        // Act
        var version = VersionStamp.Create();
        var state = original.WithDocumentText(TestProjectData.SomeProjectImportFile.FilePath, TestMocks.CreateTextLoader("@using System", version));

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument, DisposalToken);
        Assert.NotSame(originalOutput, actualOutput);
        Assert.NotEqual(originalInputVersion, actualInputVersion);
        Assert.Equal(version, actualInputVersion);
    }

    [Fact]
    public async Task RemoveDocument_Import_DoesNotCacheOutput()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, ProjectWorkspaceState.Default)
            .AddDocument(_hostDocument, DocumentState.EmptyLoader)
            .AddDocument(TestProjectData.SomeProjectImportFile, DocumentState.EmptyLoader);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument, DisposalToken);

        // Act
        var state = original.RemoveDocument(TestProjectData.SomeProjectImportFile.FilePath);

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument, DisposalToken);
        Assert.NotSame(originalOutput, actualOutput);
        Assert.NotEqual(originalInputVersion, actualInputVersion);
        Assert.Equal(state.DocumentCollectionVersion, actualInputVersion);
    }

    [Fact]
    public async Task WithProjectWorkspaceState_CachesOutput_EvenWhenNewerProjectWorkspaceState()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, ProjectWorkspaceState.Default)
            .AddDocument(_hostDocument, DocumentState.EmptyLoader);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument, DisposalToken);

        // Act
        var state = original.WithProjectWorkspaceState(ProjectWorkspaceState.Default);

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument, DisposalToken);
        Assert.Same(originalOutput, actualOutput);
        Assert.Equal(originalInputVersion, actualInputVersion);
    }

    // The generated code's text doesn't change as a result, so the output version does not change
    [Fact]
    public async Task WithProjectWorkspaceState_TagHelperChange_DoesNotCacheOutput()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, ProjectWorkspaceState.Default)
            .AddDocument(_hostDocument, DocumentState.EmptyLoader);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument, DisposalToken);

        // Act
        var state = original.WithProjectWorkspaceState(
            ProjectWorkspaceState.Create(_someTagHelpers));

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument, DisposalToken);
        Assert.NotSame(originalOutput, actualOutput);
        Assert.NotEqual(originalInputVersion, actualInputVersion);
        Assert.Equal(state.ProjectWorkspaceStateVersion, actualInputVersion);
    }

    [Fact]
    public async Task WithProjectWorkspaceState_CSharpLanguageVersionChange_DoesNotCacheOutput()
    {
        // Arrange
        var csharp8ValidConfiguration = new RazorConfiguration(RazorLanguageVersion.Version_3_0, _hostProject.Configuration.ConfigurationName, _hostProject.Configuration.Extensions);
        var hostProject = TestProjectData.SomeProject with { Configuration = csharp8ValidConfiguration };
        var originalWorkspaceState = ProjectWorkspaceState.Create(_someTagHelpers, LanguageVersion.CSharp7);
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, hostProject, originalWorkspaceState)
            .AddDocument(_hostDocument, TestMocks.CreateTextLoader("@DateTime.Now", VersionStamp.Default));

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument, DisposalToken);

        // Act
        var state = original.WithProjectWorkspaceState(
            ProjectWorkspaceState.Create(_someTagHelpers, LanguageVersion.CSharp8));

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument, DisposalToken);
        Assert.NotSame(originalOutput, actualOutput);
        Assert.NotEqual(originalInputVersion, actualInputVersion);
        Assert.Equal(state.ProjectWorkspaceStateVersion, actualInputVersion);
    }

    [Fact]
    public async Task WithHostProject_DoesNotCacheOutput()
    {
        // Arrange
        var original = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, ProjectWorkspaceState.Default)
            .AddDocument(_hostDocument, DocumentState.EmptyLoader);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument, DisposalToken);

        // Act
        var state = original.WithHostProject(_hostProjectWithConfigurationChange);

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument, DisposalToken);
        Assert.NotSame(originalOutput, actualOutput);
        Assert.NotEqual(originalInputVersion, actualInputVersion);
        Assert.NotEqual(state.ProjectWorkspaceStateVersion, actualInputVersion);
    }

    private static Task<(RazorCodeDocument, VersionStamp)> GetOutputAsync(ProjectState project, HostDocument hostDocument, CancellationToken cancellationToken)
    {
        var document = project.Documents[hostDocument.FilePath];
        return GetOutputAsync(project, document, cancellationToken);
    }

    private static Task<(RazorCodeDocument, VersionStamp)> GetOutputAsync(ProjectState project, DocumentState document, CancellationToken cancellationToken)
    {
        var projectSnapshot = new ProjectSnapshot(project);
        var documentSnapshot = new DocumentSnapshot(projectSnapshot, document);
        return document.GetGeneratedOutputAndVersionAsync(projectSnapshot, documentSnapshot, cancellationToken);
    }
}
