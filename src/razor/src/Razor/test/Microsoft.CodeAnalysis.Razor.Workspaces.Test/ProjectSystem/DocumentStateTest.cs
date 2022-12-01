// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class DocumentStateTest : WorkspaceTestBase
{
    private readonly HostDocument _hostDocument;
    private readonly HostProject _hostProject;
    private readonly HostProject _hostProjectWithConfigurationChange;
    private readonly ProjectWorkspaceState _projectWorkspaceState;
    private readonly TestTagHelperResolver _tagHelperResolver;
    private readonly Func<Task<TextAndVersion>> _textLoader;
    private readonly SourceText _text;

    public DocumentStateTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _tagHelperResolver = new TestTagHelperResolver();

        _hostProject = new HostProject(TestProjectData.SomeProject.FilePath, FallbackRazorConfiguration.MVC_2_0, TestProjectData.SomeProject.RootNamespace);
        _hostProjectWithConfigurationChange = new HostProject(TestProjectData.SomeProject.FilePath, FallbackRazorConfiguration.MVC_1_0, TestProjectData.SomeProject.RootNamespace);
        _projectWorkspaceState = new ProjectWorkspaceState(new[]
        {
            TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly").Build(),
        }, default);

        _hostDocument = TestProjectData.SomeProjectFile1;

        _text = SourceText.From("Hello, world!");
        _textLoader = () => Task.FromResult(TextAndVersion.Create(_text, VersionStamp.Create()));
    }

    protected override void ConfigureWorkspaceServices(List<IWorkspaceService> services)
    {
        services.Add(_tagHelperResolver);
    }

    [Fact]
    public async Task DocumentState_CreatedNew_HasEmptyText()
    {
        // Arrange & Act
        var state = DocumentState.Create(Workspace.Services, _hostDocument, DocumentState.EmptyLoader);

        // Assert
        var text = await state.GetTextAsync();
        Assert.Equal(0, text.Length);
    }

    [Fact]
    public async Task DocumentState_WithText_CreatesNewState()
    {
        // Arrange
        var original = DocumentState.Create(Workspace.Services, _hostDocument, DocumentState.EmptyLoader);

        // Act
        var state = original.WithText(_text, VersionStamp.Create());

        // Assert
        var text = await state.GetTextAsync();
        Assert.Same(_text, text);
    }

    [Fact]
    public async Task DocumentState_WithTextLoader_CreatesNewState()
    {
        // Arrange
        var original = DocumentState.Create(Workspace.Services, _hostDocument, DocumentState.EmptyLoader);

        // Act
        var state = original.WithTextLoader(_textLoader);

        // Assert
        var text = await state.GetTextAsync();
        Assert.Same(_text, text);
    }

    [Fact]
    public void DocumentState_WithConfigurationChange_CachesSnapshotText()
    {
        // Arrange
        var original = DocumentState.Create(Workspace.Services, _hostDocument, DocumentState.EmptyLoader)
            .WithText(_text, VersionStamp.Create());

        // Act
        var state = original.WithConfigurationChange();

        // Assert
        Assert.True(state.TryGetText(out _));
        Assert.True(state.TryGetTextVersion(out _));
    }

    [Fact]
    public async Task DocumentState_WithConfigurationChange_CachesLoadedText()
    {
        // Arrange
        var original = DocumentState.Create(Workspace.Services, _hostDocument, DocumentState.EmptyLoader)
            .WithTextLoader(_textLoader);

        await original.GetTextAsync();

        // Act
        var state = original.WithConfigurationChange();

        // Assert
        Assert.True(state.TryGetText(out _));
        Assert.True(state.TryGetTextVersion(out _));
    }

    [Fact]
    public void DocumentState_WithImportsChange_CachesSnapshotText()
    {
        // Arrange
        var original = DocumentState.Create(Workspace.Services, _hostDocument, DocumentState.EmptyLoader)
            .WithText(_text, VersionStamp.Create());

        // Act
        var state = original.WithImportsChange();

        // Assert
        Assert.True(state.TryGetText(out _));
        Assert.True(state.TryGetTextVersion(out _));
    }

    [Fact]
    public async Task DocumentState_WithImportsChange_CachesLoadedText()
    {
        // Arrange
        var original = DocumentState.Create(Workspace.Services, _hostDocument, DocumentState.EmptyLoader)
            .WithTextLoader(_textLoader);

        await original.GetTextAsync();

        // Act
        var state = original.WithImportsChange();

        // Assert
        Assert.True(state.TryGetText(out _));
        Assert.True(state.TryGetTextVersion(out _));
    }

    [Fact]
    public void DocumentState_WithProjectWorkspaceStateChange_CachesSnapshotText()
    {
        // Arrange
        var original = DocumentState.Create(Workspace.Services, _hostDocument, DocumentState.EmptyLoader)
            .WithText(_text, VersionStamp.Create());

        // Act
        var state = original.WithProjectWorkspaceStateChange();

        // Assert
        Assert.True(state.TryGetText(out _));
        Assert.True(state.TryGetTextVersion(out _));
    }

    [Fact]
    public async Task DocumentState_WithProjectWorkspaceStateChange_CachesLoadedText()
    {
        // Arrange
        var original = DocumentState.Create(Workspace.Services, _hostDocument, DocumentState.EmptyLoader)
            .WithTextLoader(_textLoader);

        await original.GetTextAsync();

        // Act
        var state = original.WithProjectWorkspaceStateChange();

        // Assert
        Assert.True(state.TryGetText(out _));
        Assert.True(state.TryGetTextVersion(out _));
    }
}
