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
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

public class DefaultRazorComponentSearchEngineTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    private static readonly string s_project1BasePath = PathUtilities.CreateRootedPath("First");
    private static readonly string s_project2BasePath = PathUtilities.CreateRootedPath("Second");

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
    private TestProjectSnapshotManager _projectManager;
#nullable enable

    protected override async Task InitializeAsync()
    {
        _projectManager = CreateProjectSnapshotManager();

        var snapshotResolver = new SnapshotResolver(_projectManager, LoggerFactory);
        var documentVersionCache = new DocumentVersionCache(_projectManager);

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

        var projectService = new RazorProjectService(
            Dispatcher,
            remoteTextLoaderFactoryMock.Object,
            snapshotResolver,
            documentVersionCache,
            _projectManager,
            LoggerFactory);

        await RunOnDispatcherAsync(() =>
        {
            projectService.AddProject(
                s_projectFilePath1,
                s_intermediateOutputPath1,
                RazorConfiguration.Default,
                RootNamespace1,
                displayName: "");

            projectService.AddDocument(s_componentFilePath1);
            projectService.UpdateDocument(s_componentFilePath1, SourceText.From(""), version: 1);

            projectService.AddDocument(s_componentFilePath2);
            projectService.UpdateDocument(s_componentFilePath2, SourceText.From("@namespace Test"), version: 1);

            projectService.AddProject(
                s_projectFilePath2,
                s_intermediateOutputPath2,
                RazorConfiguration.Default,
                RootNamespace2,
                displayName: "");

            projectService.AddDocument(s_componentFilePath3);
            projectService.UpdateDocument(s_componentFilePath3, SourceText.From(""), version: 1);
        });
    }

    [Fact]
    public async Task Handle_SearchFound_GenericComponent()
    {
        // Arrange
        var tagHelperDescriptor1 = CreateRazorComponentTagHelperDescriptor("First", RootNamespace1, "Component1", typeName: "Component1<TItem>");
        var tagHelperDescriptor2 = CreateRazorComponentTagHelperDescriptor("Second", RootNamespace2, "Component3", typeName: "Component3<TItem>");
        var searchEngine = new DefaultRazorComponentSearchEngine(_projectManager, LoggerFactory);

        // Act
        var documentSnapshot1 = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor1);
        var documentSnapshot2 = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor2);

        // Assert
        Assert.NotNull(documentSnapshot1);
        PathUtilities.AssertEquivalent(s_componentFilePath1, documentSnapshot1.FilePath);
        Assert.NotNull(documentSnapshot2);
        PathUtilities.AssertEquivalent(s_componentFilePath3, documentSnapshot2.FilePath);
    }

    [Fact]
    public async Task Handle_SearchFound()
    {
        // Arrange
        var tagHelperDescriptor1 = CreateRazorComponentTagHelperDescriptor("First", RootNamespace1, "Component1");
        var tagHelperDescriptor2 = CreateRazorComponentTagHelperDescriptor("Second", RootNamespace2, "Component3");
        var searchEngine = new DefaultRazorComponentSearchEngine(_projectManager, LoggerFactory);

        // Act
        var documentSnapshot1 = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor1);
        var documentSnapshot2 = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor2);

        // Assert
        Assert.NotNull(documentSnapshot1);
        PathUtilities.AssertEquivalent(s_componentFilePath1, documentSnapshot1.FilePath);
        Assert.NotNull(documentSnapshot2);
        PathUtilities.AssertEquivalent(s_componentFilePath3, documentSnapshot2.FilePath);
    }

    [Fact]
    public async Task Handle_SearchFound_SetNamespace()
    {
        // Arrange
        var tagHelperDescriptor = CreateRazorComponentTagHelperDescriptor("First", "Test", "Component2");
        var searchEngine = new DefaultRazorComponentSearchEngine(_projectManager, LoggerFactory);

        // Act
        var documentSnapshot = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor);

        // Assert
        Assert.NotNull(documentSnapshot);
        PathUtilities.AssertEquivalent(s_componentFilePath2, documentSnapshot.FilePath);
    }

    [Fact]
    public async Task Handle_SearchMissing_IncorrectAssembly()
    {
        // Arrange
        var tagHelperDescriptor = CreateRazorComponentTagHelperDescriptor("Third", RootNamespace1, "Component3");
        var searchEngine = new DefaultRazorComponentSearchEngine(_projectManager, LoggerFactory);

        // Act
        var documentSnapshot = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor);

        // Assert
        Assert.Null(documentSnapshot);
    }

    [Fact]
    public async Task Handle_SearchMissing_IncorrectNamespace()
    {
        // Arrange
        var tagHelperDescriptor = CreateRazorComponentTagHelperDescriptor("First", RootNamespace1, "Component2");
        var searchEngine = new DefaultRazorComponentSearchEngine(_projectManager, LoggerFactory);

        // Act
        var documentSnapshot = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor);

        // Assert
        Assert.Null(documentSnapshot);
    }

    [Fact]
    public async Task Handle_SearchMissing_IncorrectComponent()
    {
        // Arrange
        var tagHelperDescriptor = CreateRazorComponentTagHelperDescriptor("First", RootNamespace1, "Component3");
        var searchEngine = new DefaultRazorComponentSearchEngine(_projectManager, LoggerFactory);

        // Act
        var documentSnapshot = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor);

        // Assert
        Assert.Null(documentSnapshot);
    }

    [Fact]
    public async Task Handle_FilePathAndAssemblyNameDifferent()
    {
        // Arrange
        var tagHelperDescriptor = CreateRazorComponentTagHelperDescriptor("AssemblyName", "Test", "Component2");
        var searchEngine = new DefaultRazorComponentSearchEngine(_projectManager, LoggerFactory);

        // Act
        var documentSnapshot = await searchEngine.TryLocateComponentAsync(tagHelperDescriptor);

        // Assert
        Assert.NotNull(documentSnapshot);
        PathUtilities.AssertEquivalent(s_componentFilePath2, documentSnapshot.FilePath);
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
