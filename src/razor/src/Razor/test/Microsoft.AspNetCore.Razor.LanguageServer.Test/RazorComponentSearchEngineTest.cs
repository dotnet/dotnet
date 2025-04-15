// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

public class RazorComponentSearchEngineTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    private static readonly string s_project1BasePath = TestPathUtilities.CreateRootedPath("First");
    private static readonly string s_project2BasePath = TestPathUtilities.CreateRootedPath("Second");

    private static readonly string s_projectFilePath1 = Path.Combine(s_project1BasePath, "First.csproj");
    private static readonly string s_projectFilePath2 = Path.Combine(s_project2BasePath, "Second.csproj");

    private static readonly string s_intermediateOutputPath1 = Path.Combine(s_project1BasePath, "obj");
    private static readonly string s_intermediateOutputPath2 = Path.Combine(s_project2BasePath, "obj");

    private const string RootNamespace1 = "First.Components";
    private const string RootNamespace2 = "Second.Components";

    private static readonly string s_componentFilePath1 = Path.Combine(s_project1BasePath, "Component1.razor");
    private static readonly string s_componentFilePath2 = Path.Combine(s_project1BasePath, "Component2.razor");
    private static readonly string s_componentFilePath3 = Path.Combine(s_project2BasePath, "Component3.razor");

#nullable disable
    private ProjectSnapshotManager _projectManager;
#nullable enable

    protected override async Task InitializeAsync()
    {
        _projectManager = CreateProjectSnapshotManager();

        var remoteTextLoaderFactoryMock = new StrictMock<RemoteTextLoaderFactory>();
        remoteTextLoaderFactoryMock
            .Setup(x => x.Create(It.IsAny<string>()))
            .Returns((string filePath) =>
            {
                var textLoaderMock = new StrictMock<TextLoader>();
                textLoaderMock
                    .Setup(x => x.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(TextAndVersion.Create(SourceText.From(""), VersionStamp.Create(), filePath));

                return textLoaderMock.Object;
            });

        var projectService = new TestRazorProjectService(
            remoteTextLoaderFactoryMock.Object,
            _projectManager,
            LoggerFactory);
        AddDisposable(projectService);

        await projectService.GetTestAccessor().AddProjectAsync(
            s_projectFilePath1,
            s_intermediateOutputPath1,
            RazorConfiguration.Default,
            RootNamespace1,
            displayName: "",
            DisposalToken);

        await projectService.AddDocumentToPotentialProjectsAsync(s_componentFilePath1, DisposalToken);
        await projectService.UpdateDocumentAsync(s_componentFilePath1, SourceText.From(""), DisposalToken);

        await projectService.AddDocumentToPotentialProjectsAsync(s_componentFilePath2, DisposalToken);
        await projectService.UpdateDocumentAsync(s_componentFilePath2, SourceText.From("@namespace Test"), DisposalToken);

        await projectService.GetTestAccessor().AddProjectAsync(
            s_projectFilePath2,
            s_intermediateOutputPath2,
            RazorConfiguration.Default,
            RootNamespace2,
            displayName: "",
            DisposalToken);

        await projectService.AddDocumentToPotentialProjectsAsync(s_componentFilePath3, DisposalToken);
        await projectService.UpdateDocumentAsync(s_componentFilePath3, SourceText.From(""), DisposalToken);
    }

    [Fact]
    public async Task Handle_SearchFound_GenericComponent()
    {
        // Arrange
        var tagHelperDescriptor1 = CreateRazorComponentTagHelperDescriptor("First", RootNamespace1, "Component1", typeName: "Component1<TItem>");
        var tagHelperDescriptor2 = CreateRazorComponentTagHelperDescriptor("Second", RootNamespace2, "Component3", typeName: "Component3<TItem>");
        var searchEngine = new RazorComponentSearchEngine(LoggerFactory);

        // Act
        var documentSnapshot1 = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor1, _projectManager.GetQueryOperations(), DisposalToken);
        var documentSnapshot2 = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor2, _projectManager.GetQueryOperations(), DisposalToken);

        // Assert
        Assert.NotNull(documentSnapshot1);
        TestPathUtilities.AssertEquivalent(s_componentFilePath1, documentSnapshot1.FilePath);
        Assert.NotNull(documentSnapshot2);
        TestPathUtilities.AssertEquivalent(s_componentFilePath3, documentSnapshot2.FilePath);
    }

    [Fact]
    public async Task Handle_SearchFound()
    {
        // Arrange
        var tagHelperDescriptor1 = CreateRazorComponentTagHelperDescriptor("First", RootNamespace1, "Component1");
        var tagHelperDescriptor2 = CreateRazorComponentTagHelperDescriptor("Second", RootNamespace2, "Component3");
        var searchEngine = new RazorComponentSearchEngine(LoggerFactory);

        // Act
        var documentSnapshot1 = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor1, _projectManager.GetQueryOperations(), DisposalToken);
        var documentSnapshot2 = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor2, _projectManager.GetQueryOperations(), DisposalToken);

        // Assert
        Assert.NotNull(documentSnapshot1);
        TestPathUtilities.AssertEquivalent(s_componentFilePath1, documentSnapshot1.FilePath);
        Assert.NotNull(documentSnapshot2);
        TestPathUtilities.AssertEquivalent(s_componentFilePath3, documentSnapshot2.FilePath);
    }

    [Fact]
    public async Task Handle_SearchFound_SetNamespace()
    {
        // Arrange
        var tagHelperDescriptor = CreateRazorComponentTagHelperDescriptor("First", "Test", "Component2");
        var searchEngine = new RazorComponentSearchEngine(LoggerFactory);

        // Act
        var documentSnapshot = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor, _projectManager.GetQueryOperations(), DisposalToken);

        // Assert
        Assert.NotNull(documentSnapshot);
        TestPathUtilities.AssertEquivalent(s_componentFilePath2, documentSnapshot.FilePath);
    }

    [Fact]
    public async Task Handle_SearchMissing_IncorrectAssembly()
    {
        // Arrange
        var tagHelperDescriptor = CreateRazorComponentTagHelperDescriptor("Third", RootNamespace1, "Component3");
        var searchEngine = new RazorComponentSearchEngine(LoggerFactory);

        // Act
        var documentSnapshot = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor, _projectManager.GetQueryOperations(), DisposalToken);

        // Assert
        Assert.Null(documentSnapshot);
    }

    [Fact]
    public async Task Handle_SearchMissing_IncorrectNamespace()
    {
        // Arrange
        var tagHelperDescriptor = CreateRazorComponentTagHelperDescriptor("First", RootNamespace1, "Component2");
        var searchEngine = new RazorComponentSearchEngine(LoggerFactory);

        // Act
        var documentSnapshot = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor, _projectManager.GetQueryOperations(), DisposalToken);

        // Assert
        Assert.Null(documentSnapshot);
    }

    [Fact]
    public async Task Handle_SearchMissing_IncorrectComponent()
    {
        // Arrange
        var tagHelperDescriptor = CreateRazorComponentTagHelperDescriptor("First", RootNamespace1, "Component3");
        var searchEngine = new RazorComponentSearchEngine(LoggerFactory);

        // Act
        var documentSnapshot = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor, _projectManager.GetQueryOperations(), DisposalToken);

        // Assert
        Assert.Null(documentSnapshot);
    }

    [Fact]
    public async Task Handle_FilePathAndAssemblyNameDifferent()
    {
        // Arrange
        var tagHelperDescriptor = CreateRazorComponentTagHelperDescriptor("AssemblyName", "Test", "Component2");
        var searchEngine = new RazorComponentSearchEngine(LoggerFactory);

        // Act
        var documentSnapshot = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor, _projectManager.GetQueryOperations(), DisposalToken);

        // Assert
        Assert.NotNull(documentSnapshot);
        TestPathUtilities.AssertEquivalent(s_componentFilePath2, documentSnapshot.FilePath);
    }

    internal static TagHelperDescriptor CreateRazorComponentTagHelperDescriptor(string assemblyName, string namespaceName, string tagName, string? typeName = null)
    {
        typeName ??= tagName;
        var fullyQualifiedName = $"{namespaceName}.{typeName}";
        var builder1 = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, fullyQualifiedName, assemblyName);
        builder1.TagMatchingRule(rule => rule.TagName = tagName);
        builder1.SetMetadata(
            TypeNameIdentifier(typeName),
            TypeNamespace(namespaceName));

        return builder1.Build();
    }
}
