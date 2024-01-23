// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class ProjectStateGeneratedOutputTest : WorkspaceTestBase
{
    private readonly HostDocument _hostDocument;
    private readonly HostProject _hostProject;
    private readonly HostProject _hostProjectWithConfigurationChange;
    private readonly TestTagHelperResolver _tagHelperResolver;
    private readonly ImmutableArray<TagHelperDescriptor> _someTagHelpers;
    private readonly SourceText _text;

    public ProjectStateGeneratedOutputTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _hostProject = new HostProject(TestProjectData.SomeProject.FilePath, TestProjectData.SomeProject.IntermediateOutputPath, FallbackRazorConfiguration.MVC_2_0, TestProjectData.SomeProject.RootNamespace);
        _hostProjectWithConfigurationChange = new HostProject(TestProjectData.SomeProject.FilePath, TestProjectData.SomeProject.IntermediateOutputPath, FallbackRazorConfiguration.MVC_1_0, TestProjectData.SomeProject.RootNamespace);

        _tagHelperResolver = new TestTagHelperResolver();

        _someTagHelpers = ImmutableArray.Create(
            TagHelperDescriptorBuilder.Create("Test1", "TestAssembly").Build());

        _hostDocument = TestProjectData.SomeProjectFile1;

        _text = SourceText.From("Hello, world!");
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
    public async Task HostDocumentAdded_CachesOutput()
    {
        // Arrange
        var original =
            ProjectState.Create(ProjectEngineFactoryProvider, _hostProject, ProjectWorkspaceState.Default)
            .WithAddedHostDocument(_hostDocument, DocumentState.EmptyLoader);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument);

        // Act
        var state = original.WithAddedHostDocument(TestProjectData.AnotherProjectFile1, DocumentState.EmptyLoader);

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument);
        Assert.Same(originalOutput, actualOutput);
        Assert.Equal(originalInputVersion, actualInputVersion);
    }

    [Fact]
    public async Task HostDocumentAdded_Import_DoesNotCacheOutput()
    {
        // Arrange
        var original =
            ProjectState.Create(ProjectEngineFactoryProvider, _hostProject, ProjectWorkspaceState.Default)
            .WithAddedHostDocument(_hostDocument, DocumentState.EmptyLoader);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument);

        // Act
        var state = original.WithAddedHostDocument(TestProjectData.SomeProjectImportFile, DocumentState.EmptyLoader);

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument);
        Assert.NotSame(originalOutput, actualOutput);
        Assert.NotEqual(originalInputVersion, actualInputVersion);
        Assert.Equal(state.DocumentCollectionVersion, actualInputVersion);
    }

    [Fact]
    public async Task HostDocumentChanged_DoesNotCacheOutput()
    {
        // Arrange
        var original =
            ProjectState.Create(ProjectEngineFactoryProvider, _hostProject, ProjectWorkspaceState.Default)
            .WithAddedHostDocument(_hostDocument, DocumentState.EmptyLoader)
            .WithAddedHostDocument(TestProjectData.SomeProjectImportFile, DocumentState.EmptyLoader);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument);

        // Act
        var version = VersionStamp.Create();
        var state = original.WithChangedHostDocument(_hostDocument, () => Task.FromResult(TextAndVersion.Create(SourceText.From("@using System"), version)));

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument);
        Assert.NotSame(originalOutput, actualOutput);
        Assert.NotEqual(originalInputVersion, actualInputVersion);
        Assert.Equal(version, actualInputVersion);
    }

    [Fact]
    public async Task HostDocumentChanged_Import_DoesNotCacheOutput()
    {
        // Arrange
        var original =
            ProjectState.Create(ProjectEngineFactoryProvider, _hostProject, ProjectWorkspaceState.Default)
            .WithAddedHostDocument(_hostDocument, DocumentState.EmptyLoader)
            .WithAddedHostDocument(TestProjectData.SomeProjectImportFile, DocumentState.EmptyLoader);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument);

        // Act
        var version = VersionStamp.Create();
        var state = original.WithChangedHostDocument(TestProjectData.SomeProjectImportFile, () => Task.FromResult(TextAndVersion.Create(SourceText.From("@using System"), version)));

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument);
        Assert.NotSame(originalOutput, actualOutput);
        Assert.NotEqual(originalInputVersion, actualInputVersion);
        Assert.Equal(version, actualInputVersion);
    }

    [Fact]
    public async Task HostDocumentRemoved_Import_DoesNotCacheOutput()
    {
        // Arrange
        var original =
            ProjectState.Create(ProjectEngineFactoryProvider, _hostProject, ProjectWorkspaceState.Default)
            .WithAddedHostDocument(_hostDocument, DocumentState.EmptyLoader)
            .WithAddedHostDocument(TestProjectData.SomeProjectImportFile, DocumentState.EmptyLoader);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument);

        // Act
        var state = original.WithRemovedHostDocument(TestProjectData.SomeProjectImportFile);

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument);
        Assert.NotSame(originalOutput, actualOutput);
        Assert.NotEqual(originalInputVersion, actualInputVersion);
        Assert.Equal(state.DocumentCollectionVersion, actualInputVersion);
    }

    [Fact]
    public async Task ProjectWorkspaceStateChange_CachesOutput_EvenWhenNewerProjectWorkspaceState()
    {
        // Arrange
        var original =
            ProjectState.Create(ProjectEngineFactoryProvider, _hostProject, ProjectWorkspaceState.Default)
            .WithAddedHostDocument(_hostDocument, DocumentState.EmptyLoader);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument);
        var changed = ProjectWorkspaceState.Default;

        // Act
        var state = original.WithProjectWorkspaceState(changed);

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument);
        Assert.Same(originalOutput, actualOutput);
        Assert.Equal(originalInputVersion, actualInputVersion);
    }

    // The generated code's text doesn't change as a result, so the output version does not change
    [Fact]
    public async Task ProjectWorkspaceStateChange_WithTagHelperChange_DoesNotCacheOutput()
    {
        // Arrange
        var original =
            ProjectState.Create(ProjectEngineFactoryProvider, _hostProject, ProjectWorkspaceState.Default)
            .WithAddedHostDocument(_hostDocument, DocumentState.EmptyLoader);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument);
        var changed = ProjectWorkspaceState.Create(_someTagHelpers);

        // Act
        var state = original.WithProjectWorkspaceState(changed);

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument);
        Assert.NotSame(originalOutput, actualOutput);
        Assert.NotEqual(originalInputVersion, actualInputVersion);
        Assert.Equal(state.ProjectWorkspaceStateVersion, actualInputVersion);
    }

    [Fact]
    public async Task ProjectWorkspaceStateChange_WithProjectWorkspaceState_CSharpLanguageVersionChange_DoesNotCacheOutput()
    {
        // Arrange
        var csharp8ValidConfiguration = RazorConfiguration.Create(RazorLanguageVersion.Version_3_0, _hostProject.Configuration.ConfigurationName, _hostProject.Configuration.Extensions);
        var hostProject = new HostProject(TestProjectData.SomeProject.FilePath, TestProjectData.SomeProject.IntermediateOutputPath, csharp8ValidConfiguration, TestProjectData.SomeProject.RootNamespace);
        var originalWorkspaceState = ProjectWorkspaceState.Create(_someTagHelpers, LanguageVersion.CSharp7);
        var original =
            ProjectState.Create(ProjectEngineFactoryProvider, hostProject, originalWorkspaceState)
            .WithAddedHostDocument(_hostDocument, () => Task.FromResult(TextAndVersion.Create(SourceText.From("@DateTime.Now"), VersionStamp.Default)));
        var changedWorkspaceState = ProjectWorkspaceState.Create(_someTagHelpers, LanguageVersion.CSharp8);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument);

        // Act
        var state = original.WithProjectWorkspaceState(changedWorkspaceState);

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument);
        Assert.NotSame(originalOutput, actualOutput);
        Assert.NotEqual(originalInputVersion, actualInputVersion);
        Assert.Equal(state.ProjectWorkspaceStateVersion, actualInputVersion);
    }

    [Fact]
    public async Task ConfigurationChange_DoesNotCacheOutput()
    {
        // Arrange
        var original =
            ProjectState.Create(ProjectEngineFactoryProvider, _hostProject, ProjectWorkspaceState.Default)
            .WithAddedHostDocument(_hostDocument, DocumentState.EmptyLoader);

        var (originalOutput, originalInputVersion) = await GetOutputAsync(original, _hostDocument);

        // Act
        var state = original.WithHostProject(_hostProjectWithConfigurationChange);

        // Assert
        var (actualOutput, actualInputVersion) = await GetOutputAsync(state, _hostDocument);
        Assert.NotSame(originalOutput, actualOutput);
        Assert.NotEqual(originalInputVersion, actualInputVersion);
        Assert.NotEqual(state.ProjectWorkspaceStateVersion, actualInputVersion);
    }

    private static Task<(RazorCodeDocument, VersionStamp)> GetOutputAsync(ProjectState project, HostDocument hostDocument)
    {
        var document = project.Documents[hostDocument.FilePath];
        return GetOutputAsync(project, document);
    }

    private static Task<(RazorCodeDocument, VersionStamp)> GetOutputAsync(ProjectState project, DocumentState document)
    {

        var projectSnapshot = new ProjectSnapshot(project);
        var documentSnapshot = new DocumentSnapshot(projectSnapshot, document);
        return document.GetGeneratedOutputAndVersionAsync(projectSnapshot, documentSnapshot);
    }
}
