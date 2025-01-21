// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
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
        var state = ProjectState
            .Create(_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .AddEmptyDocument(_hostDocument);

        var output = await GetGeneratedOutputAsync(state, _hostDocument);

        // Act
        var newState = state.AddEmptyDocument(TestProjectData.AnotherProjectFile1);
        var newOutput = await GetGeneratedOutputAsync(newState, _hostDocument);

        // Assert
        Assert.Same(output, newOutput);
    }

    [Fact]
    public async Task AddDocument_Import_DoesNotCacheOutput()
    {
        // Arrange
        var state = ProjectState
            .Create(_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .AddEmptyDocument(_hostDocument);

        var output = await GetGeneratedOutputAsync(state, _hostDocument);

        // Act
        var newState = state.AddEmptyDocument(TestProjectData.SomeProjectImportFile);
        var newOutput = await GetGeneratedOutputAsync(newState, _hostDocument);

        // Assert
        Assert.NotSame(output, newOutput);
    }

    [Fact]
    public async Task WithDocumentText_DoesNotCacheOutput()
    {
        // Arrange
        var state = ProjectState
            .Create(_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .AddEmptyDocument(_hostDocument)
            .AddEmptyDocument(TestProjectData.SomeProjectImportFile);

        var output = await GetGeneratedOutputAsync(state, _hostDocument);

        // Act
        var newState = state.WithDocumentText(_hostDocument.FilePath, TestMocks.CreateTextLoader("@using System"));
        var newOutput = await GetGeneratedOutputAsync(newState, _hostDocument);

        // Assert
        Assert.NotSame(output, newOutput);
    }

    [Fact]
    public async Task WithDocumentText_Import_DoesNotCacheOutput()
    {
        // Arrange
        var state = ProjectState
            .Create(_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .AddEmptyDocument(_hostDocument)
            .AddEmptyDocument(TestProjectData.SomeProjectImportFile);

        var output = await GetGeneratedOutputAsync(state, _hostDocument);

        // Act
        var newState = state.WithDocumentText(TestProjectData.SomeProjectImportFile.FilePath, TestMocks.CreateTextLoader("@using System"));
        var newOutput = await GetGeneratedOutputAsync(newState, _hostDocument);

        // Assert
        Assert.NotSame(output, newOutput);
    }

    [Fact]
    public async Task RemoveDocument_Import_DoesNotCacheOutput()
    {
        // Arrange
        var state = ProjectState
            .Create(_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .AddEmptyDocument(_hostDocument)
            .AddEmptyDocument(TestProjectData.SomeProjectImportFile);

        var output = await GetGeneratedOutputAsync(state, _hostDocument);

        // Act
        var newState = state.RemoveDocument(TestProjectData.SomeProjectImportFile.FilePath);
        var newOutput = await GetGeneratedOutputAsync(newState, _hostDocument);

        // Assert
        Assert.NotSame(output, newOutput);
    }

    [Fact]
    public async Task WithProjectWorkspaceState_CachesOutput_EvenWhenNewerProjectWorkspaceState()
    {
        // Arrange
        var state = ProjectState
            .Create(_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .AddEmptyDocument(_hostDocument);

        var output = await GetGeneratedOutputAsync(state, _hostDocument);

        // Act
        var newState = state.WithProjectWorkspaceState(ProjectWorkspaceState.Default);
        var newOutput = await GetGeneratedOutputAsync(newState, _hostDocument);

        // Assert
        Assert.Same(output, newOutput);
    }

    [Fact]
    public async Task WithProjectWorkspaceState_TagHelperChange_DoesNotCacheOutput()
    {
        // Arrange
        var state = ProjectState
            .Create(_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .AddEmptyDocument(_hostDocument);

        var output = await GetGeneratedOutputAsync(state, _hostDocument);

        // Act
        var newState = state.WithProjectWorkspaceState(ProjectWorkspaceState.Create(_someTagHelpers));
        var newOutput = await GetGeneratedOutputAsync(newState, _hostDocument);

        // Assert
        Assert.NotSame(output, newOutput);
    }

    [Fact]
    public async Task WithHostProject_DoesNotCacheOutput()
    {
        // Arrange
        var state = ProjectState
            .Create(_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .AddEmptyDocument(_hostDocument);

        var output = await GetGeneratedOutputAsync(state, _hostDocument);

        // Act
        var newState = state.WithHostProject(_hostProjectWithConfigurationChange);
        var newOutput = await GetGeneratedOutputAsync(newState, _hostDocument);

        // Assert
        Assert.NotSame(output, newOutput);
    }

    private ValueTask<RazorCodeDocument> GetGeneratedOutputAsync(ProjectState project, HostDocument hostDocument)
    {
        var document = project.Documents[hostDocument.FilePath];

        var projectSnapshot = new ProjectSnapshot(project);
        var documentSnapshot = new DocumentSnapshot(projectSnapshot, document);

        return documentSnapshot.GetGeneratedOutputAsync(DisposalToken);
    }
}
