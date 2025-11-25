// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Rename;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Refactoring;

public class RenameEndpointTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
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
    private const string ComponentText1 = $"""
        @namespace {RootNamespace1}
        @using Test
        <Component2></Component2>
        """;

    private static readonly string s_componentFilePath2 = Path.Combine(s_project1BasePath, "Component2.razor");
    private const string ComponentText2 = """
        @namespace Test
        """;

    private static readonly string s_componentFilePath1337 = Path.Combine(s_project1BasePath, "Component1337.razor");
    private const string ComponentText1337 = """
        @namespace Test
        """;

    private static readonly string s_componentFilePath3 = Path.Combine(s_project2BasePath, "Component3.razor");
    private const string ComponentText3 = $"""
        @namespace {RootNamespace2}
        <Component3></Component3>
        """;

    private static readonly string s_componentFilePath4 = Path.Combine(s_project2BasePath, "Component4.razor");
    private const string ComponentText4 = $"""
        @namespace {RootNamespace2}
        <Component3></Component3>
        <Component3></Component3>
        """;

    private static readonly string s_componentFilePath5 = Path.Combine(s_project2BasePath, "Component5.razor");

    private static readonly string s_componentWithParamFilePath = Path.Combine(s_project2BasePath, "ComponentWithParam.razor");
    private const string ComponentWithParamText = $"""
        @namespace {RootNamespace2}
        <Component3 Title="Something"></Component3>
        """;

    private static readonly string s_indexFilePath1 = Path.Combine(s_project1BasePath, "Index.razor");
    private const string IndexText1 = $"""
        @namespace {RootNamespace1}
        @using Test
        <Component1337></Component1337>
        <Test.Component1337></Test.Component1337>
        """;

    private static readonly string s_directoryFilePath1 = Path.Combine(s_project1BasePath, "Directory1.razor");
    private const string DirectoryText1 = """
        @namespace Test.Components
        <Directory2></Directory2>
        """;

    private static readonly string s_directoryFilePath2 = Path.Combine(s_project1BasePath, "Directory2.razor");
    private const string DirectoryText2 = """
        @namespace Test.Components
        <Directory1></Directory1>
        """;

    [Fact]
    public async Task Handle_Rename_FileManipulationNotSupported_ReturnsNull()
    {
        // Arrange
        var options = StrictMock.Of<LanguageServerFeatureOptions>(static o =>
            o.SupportsFileManipulation == false &&
            o.ReturnCodeActionAndRenamePathsWithPrefixedSlash == false);
        var (endpoint, documentContextFactory) = await CreateEndpointAndDocumentContextFactoryAsync(options);
        var uri = TestPathUtilities.GetUri(s_componentFilePath1);
        var request = new RenameParams
        {
            TextDocument = new() { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(2, 1),
            NewName = "Component5"
        };

        Assert.True(documentContextFactory.TryCreate(uri, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_Rename_WithNamespaceDirective()
    {
        // Arrange
        var (endpoint, documentContextFactory) = await CreateEndpointAndDocumentContextFactoryAsync();
        var uri = TestPathUtilities.GetUri(s_componentFilePath1);
        var request = new RenameParams
        {
            TextDocument = new() { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(2, 1),
            NewName = "Component5"
        };

        Assert.True(documentContextFactory.TryCreate(uri, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        var documentChanges = result.DocumentChanges.AssumeNotNull();
        Assert.Equal(2, documentChanges.Count());

        // We renamed Component2 to Component5, so ensure we received a file rename.
        var renameChange = documentChanges.ElementAt(0);
        Assert.True(renameChange.TryGetThird(out var renameFile));
        Assert.Equal(TestPathUtilities.GetUri(s_componentFilePath2), renameFile.OldDocumentUri.GetRequiredParsedUri());
        Assert.Equal(TestPathUtilities.GetUri(s_project1BasePath, "Component5.razor"), renameFile.NewDocumentUri.GetRequiredParsedUri());

        // Next, we should get a series of text edits to Component1 that rename
        // "Component2" to "Component5".
        var editChange = documentChanges.ElementAt(1);
        Assert.True(editChange.TryGetFirst(out var textDocumentEdit));
        Assert.Equal(uri, textDocumentEdit.TextDocument.DocumentUri.GetRequiredParsedUri());
        Assert.Collection(
            textDocumentEdit.Edits,
            AssertTextEdit("Component5", startLine: 2, startCharacter: 1, endLine: 2, endCharacter: 11),
            AssertTextEdit("Component5", startLine: 2, startCharacter: 14, endLine: 2, endCharacter: 24));
    }

    [Fact]
    public async Task Handle_Rename_OnComponentParameter_ReturnsNull()
    {
        // Arrange
        var (endpoint, documentContextFactory) = await CreateEndpointAndDocumentContextFactoryAsync();
        var uri = TestPathUtilities.GetUri(s_componentWithParamFilePath);
        var request = new RenameParams
        {
            TextDocument = new() { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 14),
            NewName = "Test2"
        };

        Assert.True(documentContextFactory.TryCreate(uri, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_Rename_OnOpeningBrace_ReturnsNull()
    {
        // Arrange
        var (endpoint, documentContextFactory) = await CreateEndpointAndDocumentContextFactoryAsync();
        var uri = TestPathUtilities.GetUri(s_componentWithParamFilePath);
        var request = new RenameParams
        {
            TextDocument = new() { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 0),
            NewName = "Test2"
        };

        Assert.True(documentContextFactory.TryCreate(uri, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_Rename_OnComponentNameLeadingEdge_ReturnsResult()
    {
        // Arrange
        var (endpoint, documentContextFactory) = await CreateEndpointAndDocumentContextFactoryAsync();
        var uri = TestPathUtilities.GetUri(s_componentWithParamFilePath);
        var request = new RenameParams
        {
            TextDocument = new() { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 1),
            NewName = "Test2"
        };

        Assert.True(documentContextFactory.TryCreate(uri, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Handle_Rename_OnComponentName_ReturnsResult()
    {
        // Arrange
        var (endpoint, documentContextFactory) = await CreateEndpointAndDocumentContextFactoryAsync();
        var uri = TestPathUtilities.GetUri(s_componentWithParamFilePath);
        var request = new RenameParams
        {
            TextDocument = new() { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 3),
            NewName = "Test2"
        };

        Assert.True(documentContextFactory.TryCreate(uri, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Handle_Rename_OnComponentEndTag_ReturnsResult()
    {
        // Arrange
        var (endpoint, documentContextFactory) = await CreateEndpointAndDocumentContextFactoryAsync();
        var uri = TestPathUtilities.GetUri(s_componentWithParamFilePath);
        var request = new RenameParams
        {
            TextDocument = new() { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 36),
            NewName = "Test2"
        };

        Assert.True(documentContextFactory.TryCreate(uri, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Handle_Rename_OnComponentNameTrailingEdge_ReturnsResult()
    {
        // Arrange
        var (endpoint, documentContextFactory) = await CreateEndpointAndDocumentContextFactoryAsync();
        var uri = TestPathUtilities.GetUri(s_componentWithParamFilePath);
        var request = new RenameParams
        {
            TextDocument = new() { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 10),
            NewName = "Test2"
        };

        Assert.True(documentContextFactory.TryCreate(uri, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/8541")]
    public async Task Handle_Rename_ComponentInSameFile()
    {
        // Arrange
        var (endpoint, documentContextFactory) = await CreateEndpointAndDocumentContextFactoryAsync();
        var uri = TestPathUtilities.GetUri(s_componentFilePath4);
        var request = new RenameParams
        {
            TextDocument = new() { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 1),
            NewName = "Component5"
        };

        Assert.True(documentContextFactory.TryCreate(uri, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        var documentChanges = result.DocumentChanges.AssumeNotNull();
        Assert.Equal(4, documentChanges.Count());

        // We renamed Component3 to Component5, so we should expect file rename.
        var renameChange = documentChanges.ElementAt(0);
        Assert.True(renameChange.TryGetThird(out var renameFile));
        Assert.Equal(TestPathUtilities.GetUri(s_componentFilePath3), renameFile.OldDocumentUri.GetRequiredParsedUri());
        Assert.Equal(TestPathUtilities.GetUri(s_componentFilePath5), renameFile.NewDocumentUri.GetRequiredParsedUri());

        Assert.Collection(GetTextDocumentEdits(result, startIndex: 1, endIndex: 3),
            textDocumentEdit =>
            {
                Assert.Equal(TestPathUtilities.GetUri(s_componentFilePath4), textDocumentEdit.TextDocument.DocumentUri.GetRequiredParsedUri());
                Assert.Collection(
                    textDocumentEdit.Edits,
                    AssertTextEdit("Component5", startLine: 1, startCharacter: 1, endLine: 1, endCharacter: 11),
                    AssertTextEdit("Component5", startLine: 1, startCharacter: 14, endLine: 1, endCharacter: 24),
                    AssertTextEdit("Component5", startLine: 2, startCharacter: 1, endLine: 2, endCharacter: 11),
                    AssertTextEdit("Component5", startLine: 2, startCharacter: 14, endLine: 2, endCharacter: 24));
            },
            textDocumentEdit =>
            {
                Assert.Equal(TestPathUtilities.GetUri(s_componentFilePath5), textDocumentEdit.TextDocument.DocumentUri.GetRequiredParsedUri());
                Assert.Collection(
                    textDocumentEdit.Edits,
                    AssertTextEdit("Component5", startLine: 1, startCharacter: 1, endLine: 1, endCharacter: 11),
                    AssertTextEdit("Component5", startLine: 1, startCharacter: 14, endLine: 1, endCharacter: 24));
            },
            textDocumentEdit =>
            {
                Assert.Equal(TestPathUtilities.GetUri(s_componentWithParamFilePath), textDocumentEdit.TextDocument.DocumentUri.GetRequiredParsedUri());
                Assert.Collection(
                    textDocumentEdit.Edits,
                    AssertTextEdit("Component5", startLine: 1, startCharacter: 1, endLine: 1, endCharacter: 11),
                    AssertTextEdit("Component5", startLine: 1, startCharacter: 32, endLine: 1, endCharacter: 42));
            });

        static IEnumerable<TextDocumentEdit> GetTextDocumentEdits(WorkspaceEdit workspaceEdit, int startIndex, int endIndex)
        {
            var documentChanges = workspaceEdit.DocumentChanges.AssumeNotNull();

            using var builder = new PooledArrayBuilder<TextDocumentEdit>();

            for (var i = startIndex; i <= endIndex; i++)
            {
                var change = documentChanges.ElementAt(i);
                Assert.True(change.TryGetFirst(out var textDocumentEdit));

                builder.Add(textDocumentEdit);
            }

            return builder
                .ToArray()
                .OrderBy(x => x.TextDocument.DocumentUri.GetRequiredParsedUri().ToString())
                .ThenBy(x => ((TextEdit)x.Edits.First()).Range.Start.Line)
                .ThenBy(x => ((TextEdit)x.Edits.First()).Range.Start.Character);
        }
    }

    [Fact]
    public async Task Handle_Rename_FullyQualifiedAndNot()
    {
        // Arrange
        var (endpoint, documentContextFactory) = await CreateEndpointAndDocumentContextFactoryAsync();
        var uri = TestPathUtilities.GetUri(s_indexFilePath1);
        var request = new RenameParams
        {
            TextDocument = new() { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(2, 1),
            NewName = "Component5"
        };

        Assert.True(documentContextFactory.TryCreate(uri, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        var documentChanges = result.DocumentChanges.AssumeNotNull();
        Assert.Equal(2, documentChanges.Count());

        var renameChange = documentChanges.ElementAt(0);
        Assert.True(renameChange.TryGetThird(out var renameFile));
        Assert.Equal(TestPathUtilities.GetUri(s_componentFilePath1337), renameFile.OldDocumentUri.GetRequiredParsedUri());
        Assert.Equal(TestPathUtilities.GetUri(s_project1BasePath, "Component5.razor"), renameFile.NewDocumentUri.GetRequiredParsedUri());

        var editChange1 = documentChanges.ElementAt(1);
        Assert.True(editChange1.TryGetFirst(out var textDocumentEdit));
        Assert.Equal(TestPathUtilities.GetUri(s_indexFilePath1), textDocumentEdit.TextDocument.DocumentUri.GetRequiredParsedUri());
        Assert.Collection(
            textDocumentEdit.Edits,
            AssertTextEdit("Component5", startLine: 2, startCharacter: 1, endLine: 2, endCharacter: 14),
            AssertTextEdit("Component5", startLine: 2, startCharacter: 17, endLine: 2, endCharacter: 30),
            AssertTextEdit("Test.Component5", startLine: 3, startCharacter: 1, endLine: 3, endCharacter: 19),
            AssertTextEdit("Test.Component5", startLine: 3, startCharacter: 22, endLine: 3, endCharacter: 40));
    }

    [Fact]
    public async Task Handle_Rename_MultipleFileUsages()
    {
        // Arrange
        var (endpoint, documentContextFactory) = await CreateEndpointAndDocumentContextFactoryAsync();
        var uri = TestPathUtilities.GetUri(s_componentFilePath3);
        var request = new RenameParams
        {
            TextDocument = new() { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 1),
            NewName = "Component5"
        };

        Assert.True(documentContextFactory.TryCreate(uri, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        var documentChanges = result.DocumentChanges.AssumeNotNull();
        Assert.Equal(4, documentChanges.Count());

        var renameChange = documentChanges.ElementAt(0);
        Assert.True(renameChange.TryGetThird(out var renameFile));
        Assert.Equal(TestPathUtilities.GetUri(s_componentFilePath3), renameFile.OldDocumentUri.GetRequiredParsedUri());
        Assert.Equal(TestPathUtilities.GetUri(s_componentFilePath5), renameFile.NewDocumentUri.GetRequiredParsedUri());

        var editChange1 = documentChanges.ElementAt(1);
        Assert.True(editChange1.TryGetFirst(out var textDocumentEdit));
        Assert.Equal(TestPathUtilities.GetUri(s_componentFilePath5), textDocumentEdit.TextDocument.DocumentUri.GetRequiredParsedUri());
        Assert.Collection(
            textDocumentEdit.Edits,
            AssertTextEdit("Component5", 1, 1, 1, 11),
            AssertTextEdit("Component5", 1, 14, 1, 24));

        var editChange2 = documentChanges.ElementAt(2);
        Assert.True(editChange2.TryGetFirst(out var textDocumentEdit2));
        Assert.Equal(TestPathUtilities.GetUri(s_componentFilePath4), textDocumentEdit2.TextDocument.DocumentUri.GetRequiredParsedUri());
        Assert.Collection(
            textDocumentEdit.Edits,
            AssertTextEdit("Component5", 1, 1, 1, 11),
            AssertTextEdit("Component5", 1, 14, 1, 24));

        var editChange3 = documentChanges.ElementAt(3);
        Assert.True(editChange3.TryGetFirst(out var textDocumentEdit3));
        Assert.Equal(TestPathUtilities.GetUri(s_componentWithParamFilePath), textDocumentEdit3.TextDocument.DocumentUri.GetRequiredParsedUri());
        Assert.Collection(
            textDocumentEdit.Edits,
            AssertTextEdit("Component5", 1, 1, 1, 11),
            AssertTextEdit("Component5", 1, 14, 1, 24));
    }

    [Fact]
    public async Task Handle_Rename_DifferentDirectories()
    {
        // Arrange
        var (endpoint, documentContextFactory) = await CreateEndpointAndDocumentContextFactoryAsync();
        var uri = TestPathUtilities.GetUri(s_directoryFilePath1);
        var request = new RenameParams
        {
            TextDocument = new() { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 1),
            NewName = "TestComponent"
        };

        Assert.True(documentContextFactory.TryCreate(uri, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        var documentChanges = result.DocumentChanges.AssumeNotNull();
        Assert.Equal(2, documentChanges.Count());

        var renameChange = documentChanges.ElementAt(0);
        Assert.True(renameChange.TryGetThird(out var renameFile));
        Assert.Equal(TestPathUtilities.GetUri(s_directoryFilePath2), renameFile.OldDocumentUri.GetRequiredParsedUri());
        Assert.Equal(TestPathUtilities.GetUri(s_project1BasePath, "TestComponent.razor"), renameFile.NewDocumentUri.GetRequiredParsedUri());

        var editChange = documentChanges.ElementAt(1);
        Assert.True(editChange.TryGetFirst(out var textDocumentEdit));
        Assert.Equal(TestPathUtilities.GetUri(s_directoryFilePath1), textDocumentEdit.TextDocument.DocumentUri.GetRequiredParsedUri());
        Assert.Collection(
            textDocumentEdit.Edits,
            AssertTextEdit("TestComponent", startLine: 1, startCharacter: 1, endLine: 1, endCharacter: 11),
            AssertTextEdit("TestComponent", startLine: 1, startCharacter: 14, endLine: 1, endCharacter: 24));
    }

    [Fact]
    public async Task Handle_Rename_SingleServer_CallsDelegatedLanguageServer()
    {
        // Arrange
        var options = StrictMock.Of<LanguageServerFeatureOptions>(static o =>
            o.SupportsFileManipulation == true &&
            o.SingleServerSupport == true &&
            o.ReturnCodeActionAndRenamePathsWithPrefixedSlash == false);

        var delegatedEdit = new WorkspaceEdit();

        var clientConnection = TestMocks.CreateClientConnection(builder =>
        {
            builder.SetupSendRequest<IDelegatedParams, WorkspaceEdit>(CustomMessageNames.RazorRenameEndpointName, response: delegatedEdit);
        });

        var documentMappingServiceMock = new StrictMock<IDocumentMappingService>();

        var projectedPosition = new LinePosition(1, 1);
        var projectedIndex = 1;
        documentMappingServiceMock
            .Setup(x => x.TryMapToCSharpDocumentPosition(It.IsAny<RazorCSharpDocument>(), It.IsAny<int>(), out projectedPosition, out projectedIndex))
            .Returns(true);

        var editMappingServiceMock = new StrictMock<IEditMappingService>();
        editMappingServiceMock
            .Setup(x => x.RemapWorkspaceEditAsync(It.IsAny<IDocumentSnapshot>(), It.IsAny<WorkspaceEdit>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(delegatedEdit);

        var (endpoint, documentContextFactory) = await CreateEndpointAndDocumentContextFactoryAsync(
            options,
            documentMappingServiceMock.Object,
            editMappingServiceMock.Object,
            clientConnection);

        var uri = TestPathUtilities.GetUri(s_componentWithParamFilePath);
        var request = new RenameParams
        {
            TextDocument = new() { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 0),
            NewName = "Test2"
        };

        Assert.True(documentContextFactory.TryCreate(uri, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Same(delegatedEdit, result);
    }

    [Fact]
    public async Task Handle_Rename_SingleServer_DoesNotDelegateForRazor()
    {
        // Arrange
        var options = StrictMock.Of<LanguageServerFeatureOptions>(static o =>
            o.SupportsFileManipulation == true &&
            o.SingleServerSupport == true &&
            o.ReturnCodeActionAndRenamePathsWithPrefixedSlash == false);

        var documentMappingService = StrictMock.Of<IDocumentMappingService>();

        var (endpoint, documentContextFactory) = await CreateEndpointAndDocumentContextFactoryAsync(options, documentMappingService);

        var request = new RenameParams
        {
            TextDocument = new() { DocumentUri = new(TestPathUtilities.GetUri(s_componentWithParamFilePath)) },
            Position = LspFactory.CreatePosition(0, 1), // This is right after the '@' in '@namespace'
            NewName = "Test2"
        };

        Assert.True(documentContextFactory.TryCreate(request.TextDocument.DocumentUri.GetRequiredParsedUri(), out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    private async Task<(RenameEndpoint, IDocumentContextFactory)> CreateEndpointAndDocumentContextFactoryAsync(
        LanguageServerFeatureOptions? options = null,
        IDocumentMappingService? documentMappingService = null,
        IEditMappingService? editMappingService = null,
        IClientConnection? clientConnection = null)
    {
        var tagHelpers = CreateRazorComponentTagHelpers();
        var projectManager = CreateProjectSnapshotManager();

        var documentContextFactory = new DocumentContextFactory(projectManager, LoggerFactory);

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

        var projectService = AddDisposable(
            new TestRazorProjectService(
                remoteTextLoaderFactoryMock.Object,
                projectManager,
                LoggerFactory));

        var projectKey1 = await projectService.GetTestAccessor().AddProjectAsync(
            s_projectFilePath1, s_intermediateOutputPath1, RazorConfiguration.Default, RootNamespace1, displayName: null, DisposalToken);

        await projectManager.UpdateAsync(updater =>
        {
            updater.UpdateProjectWorkspaceState(projectKey1, ProjectWorkspaceState.Create(tagHelpers));
        });

        await projectService.AddDocumentToPotentialProjectsAsync(s_componentFilePath1, DisposalToken);
        await projectService.AddDocumentToPotentialProjectsAsync(s_componentFilePath2, DisposalToken);
        await projectService.AddDocumentToPotentialProjectsAsync(s_directoryFilePath1, DisposalToken);
        await projectService.AddDocumentToPotentialProjectsAsync(s_directoryFilePath2, DisposalToken);
        await projectService.AddDocumentToPotentialProjectsAsync(s_componentFilePath1337, DisposalToken);
        await projectService.AddDocumentToPotentialProjectsAsync(s_indexFilePath1, DisposalToken);

        await projectService.UpdateDocumentAsync(s_componentFilePath1, SourceText.From(ComponentText1), DisposalToken);
        await projectService.UpdateDocumentAsync(s_componentFilePath2, SourceText.From(ComponentText2), DisposalToken);
        await projectService.UpdateDocumentAsync(s_directoryFilePath1, SourceText.From(DirectoryText1), DisposalToken);
        await projectService.UpdateDocumentAsync(s_directoryFilePath2, SourceText.From(DirectoryText2), DisposalToken);
        await projectService.UpdateDocumentAsync(s_componentFilePath1337, SourceText.From(ComponentText1337), DisposalToken);
        await projectService.UpdateDocumentAsync(s_indexFilePath1, SourceText.From(IndexText1), DisposalToken);

        var projectKey2 = await projectService.GetTestAccessor().AddProjectAsync(
            s_projectFilePath2, s_intermediateOutputPath2, RazorConfiguration.Default, RootNamespace2, displayName: null, DisposalToken);

        await projectManager.UpdateAsync(updater =>
        {
            updater.UpdateProjectWorkspaceState(projectKey2, ProjectWorkspaceState.Create(tagHelpers));
        });

        await projectService.AddDocumentToPotentialProjectsAsync(s_componentFilePath3, DisposalToken);
        await projectService.AddDocumentToPotentialProjectsAsync(s_componentFilePath4, DisposalToken);
        await projectService.AddDocumentToPotentialProjectsAsync(s_componentWithParamFilePath, DisposalToken);

        await projectService.UpdateDocumentAsync(s_componentFilePath3, SourceText.From(ComponentText3), DisposalToken);
        await projectService.UpdateDocumentAsync(s_componentFilePath4, SourceText.From(ComponentText4), DisposalToken);
        await projectService.UpdateDocumentAsync(s_componentWithParamFilePath, SourceText.From(ComponentWithParamText), DisposalToken);

        var searchEngine = new RazorComponentSearchEngine(LoggerFactory);
        options ??= StrictMock.Of<LanguageServerFeatureOptions>(static o =>
            o.SupportsFileManipulation == true &&
            o.SingleServerSupport == false &&
            o.ReturnCodeActionAndRenamePathsWithPrefixedSlash == false);

        if (documentMappingService == null)
        {
            var documentMappingServiceMock = new StrictMock<IDocumentMappingService>();

            var projectedPosition = new LinePosition(1, 1);
            var projectedIndex = 1;
            documentMappingServiceMock
                .Setup(c => c.TryMapToCSharpDocumentPosition(It.IsAny<RazorCSharpDocument>(), It.IsAny<int>(), out projectedPosition, out projectedIndex))
                .Returns(true);
            documentMappingService = documentMappingServiceMock.Object;
        }

        editMappingService ??= StrictMock.Of<IEditMappingService>();

        clientConnection ??= StrictMock.Of<IClientConnection>();

        var renameService = new RenameService(searchEngine, new FileSystem(), options);
        var endpoint = new RenameEndpoint(
            renameService,
            options,
            documentMappingService,
            editMappingService,
            projectManager,
            clientConnection,
            LoggerFactory);

        return (endpoint, documentContextFactory);
    }

    private static TagHelperCollection CreateRazorComponentTagHelpers()
    {
        using var builder = new TagHelperCollection.RefBuilder();
        builder.AddRange(CreateRazorComponentTagHelpersCore("First", RootNamespace1, "Component1"));
        builder.AddRange(CreateRazorComponentTagHelpersCore("First", "Test", "Component2"));
        builder.AddRange(CreateRazorComponentTagHelpersCore("Second", RootNamespace2, "Component3"));
        builder.AddRange(CreateRazorComponentTagHelpersCore("Second", RootNamespace2, "Component4"));
        builder.AddRange(CreateRazorComponentTagHelpersCore("First", "Test", "Component1337"));
        builder.AddRange(CreateRazorComponentTagHelpersCore("First", "Test.Components", "Directory1"));
        builder.AddRange(CreateRazorComponentTagHelpersCore("First", "Test.Components", "Directory2"));

        return builder.ToCollection();

        static IEnumerable<TagHelperDescriptor> CreateRazorComponentTagHelpersCore(
            string assemblyName, string namespaceName, string tagName)
        {
            var fullyQualifiedName = $"{namespaceName}.{tagName}";
            var builder = TagHelperDescriptorBuilder.CreateComponent(fullyQualifiedName, assemblyName);
            builder.SetTypeName(fullyQualifiedName, namespaceName, tagName);
            builder.TagMatchingRule(rule => rule.TagName = tagName);

            yield return builder.Build();

            var fullyQualifiedBuilder = TagHelperDescriptorBuilder.CreateComponent(fullyQualifiedName, assemblyName);
            fullyQualifiedBuilder.SetTypeName(fullyQualifiedName, namespaceName, tagName);
            fullyQualifiedBuilder.TagMatchingRule(rule => rule.TagName = fullyQualifiedName);
            fullyQualifiedBuilder.IsFullyQualifiedNameMatch = true;

            yield return fullyQualifiedBuilder.Build();
        }
    }

    private static Action<SumType<TextEdit, AnnotatedTextEdit>> AssertTextEdit(string fileName, int startLine, int startCharacter, int endLine, int endCharacter)
        => edit =>
        {
            Assert.Equal(fileName, ((TextEdit)edit).NewText);

            var range = ((TextEdit)edit).Range;
            Assert.Equal(startLine, range.Start.Line);
            Assert.Equal(startCharacter, range.Start.Character);
            Assert.Equal(endLine, range.End.Line);
            Assert.Equal(endCharacter, range.End.Character);
        };
}
