// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Extensions.Version2_X;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Test;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Refactoring.Test;

[UseExportProvider]
public class RenameEndpointTest : LanguageServerTestBase
{
    private readonly RenameEndpoint _endpoint;
    private DocumentContextFactory _documentContextFactory;

    public RenameEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _endpoint = CreateEndpoint();
    }

    [Fact]
    public async Task Handle_Rename_FileManipulationNotSupported_ReturnsNull()
    {
        // Arrange
        var languageServerFeatureOptions = Mock.Of<LanguageServerFeatureOptions>(options => options.SupportsFileManipulation == false, MockBehavior.Strict);
        var endpoint = CreateEndpoint(languageServerFeatureOptions);
        var uri = new Uri("file:///c:/First/Component1.razor");
        var request = new RenameParamsBridge
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri,
            },
            Position = new Position(2, 1),
            NewName = "Component5"
        };
        var documentContext = await GetDocumentContextAsync(uri);
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
        var uri = new Uri("file:///c:/First/Component1.razor");
        var request = new RenameParamsBridge
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri,
            },
            Position = new Position(2, 1),
            NewName = "Component5"
        };
        var documentContext = await GetDocumentContextAsync(uri);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await _endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        var documentChanges = result.DocumentChanges.Value;
        Assert.Equal(2, documentChanges.Count());
        var renameChange = documentChanges.ElementAt(0);
        Assert.True(renameChange.TryGetThird(out var renameFile));
        Assert.Equal(new Uri("file:///c:/First/Component2.razor"), renameFile.OldUri);
        Assert.Equal(new Uri("file:///c:/First/Component5.razor"), renameFile.NewUri);
        var editChange = documentChanges.ElementAt(1);
        Assert.True(editChange.TryGetFirst(out var textDocumentEdit));
        Assert.Equal("file:///c:/First/Component1.razor", textDocumentEdit.TextDocument.Uri.ToString());
        Assert.Collection(
            textDocumentEdit.Edits,
            edit =>
            {
                Assert.Equal("Component5", edit.NewText);
                Assert.Equal(2, edit.Range.Start.Line);
                Assert.Equal(1, edit.Range.Start.Character);
                Assert.Equal(2, edit.Range.End.Line);
                Assert.Equal(11, edit.Range.End.Character);
            },
            edit =>
            {
                Assert.Equal("Component5", edit.NewText);
                Assert.Equal(2, edit.Range.Start.Line);
                Assert.Equal(14, edit.Range.Start.Character);
                Assert.Equal(2, edit.Range.End.Line);
                Assert.Equal(24, edit.Range.End.Character);
            });
    }

    [Fact]
    public async Task Handle_Rename_OnComponentParameter_ReturnsNull()
    {
        // Arrange
        var uri = new Uri("file:///c:/Second/ComponentWithParam.razor");
        var request = new RenameParamsBridge
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri,
            },
            Position = new Position(1, 14),
            NewName = "Test2"
        };
        var documentContext = await GetDocumentContextAsync(uri);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await _endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_Rename_OnOpeningBrace_ReturnsNull()
    {
        // Arrange
        var uri = new Uri("file:///c:/Second/ComponentWithParam.razor");
        var request = new RenameParamsBridge
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri,
            },
            Position = new Position(1, 0),
            NewName = "Test2"
        };
        var documentContext = await GetDocumentContextAsync(uri);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await _endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_Rename_OnComponentNameLeadingEdge_ReturnsResult()
    {
        // Arrange
        var uri = new Uri("file:///c:/Second/ComponentWithParam.razor");
        var request = new RenameParamsBridge
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri,
            },
            Position = new Position(1, 1),
            NewName = "Test2"
        };
        var documentContext = await GetDocumentContextAsync(uri);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await _endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Handle_Rename_OnComponentName_ReturnsResult()
    {
        // Arrange
        var uri = new Uri("file:///c:/Second/ComponentWithParam.razor");
        var request = new RenameParamsBridge
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri,
            },
            Position = new Position(1, 3),
            NewName = "Test2"
        };
        var documentContext = await GetDocumentContextAsync(uri);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await _endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Handle_Rename_OnComponentNameTrailingEdge_ReturnsResult()
    {
        // Arrange
        var uri = new Uri("file:///c:/Second/ComponentWithParam.razor");
        var request = new RenameParamsBridge
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri,
            },
            Position = new Position(1, 10),
            NewName = "Test2"
        };
        var documentContext = await GetDocumentContextAsync(uri);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await _endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Handle_Rename_FullyQualifiedAndNot()
    {
        // Arrange
        var uri = new Uri("file:///c:/First/Index.razor");
        var request = new RenameParamsBridge
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri,
            },
            Position = new Position(2, 1),
            NewName = "Component5"
        };
        var documentContext = await GetDocumentContextAsync(uri);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await _endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        var documentChanges = result.DocumentChanges.Value;
        Assert.Equal(3, documentChanges.Count());
        var renameChange = documentChanges.ElementAt(0);
        Assert.True(renameChange.TryGetThird(out var renameFile));
        Assert.Equal(new Uri("file:///c:/First/Component1337.razor"), renameFile.OldUri);
        Assert.Equal(new Uri("file:///c:/First/Component5.razor"), renameFile.NewUri);
        var editChange1 = documentChanges.ElementAt(1);
        Assert.True(editChange1.TryGetFirst(out var textDocumentEdit));
        Assert.Equal("file:///c:/First/Index.razor", textDocumentEdit.TextDocument.Uri.ToString());
        Assert.Collection(
            textDocumentEdit.Edits,
            edit =>
            {
                Assert.Equal("Component5", edit.NewText);
                Assert.Equal(2, edit.Range.Start.Line);
                Assert.Equal(1, edit.Range.Start.Character);
                Assert.Equal(2, edit.Range.End.Line);
                Assert.Equal(14, edit.Range.End.Character);
            },
            edit =>
            {
                Assert.Equal("Component5", edit.NewText);
                Assert.Equal(2, edit.Range.Start.Line);
                Assert.Equal(17, edit.Range.Start.Character);
                Assert.Equal(2, edit.Range.End.Line);
                Assert.Equal(30, edit.Range.End.Character);
            });

        var editChange2 = result.DocumentChanges.Value.ElementAt(2);
        Assert.True(editChange2.TryGetFirst(out var textDocumentEdit2));
        Assert.Equal("file:///c:/First/Index.razor", textDocumentEdit2.TextDocument.Uri.ToString());
        Assert.Collection(
            textDocumentEdit2.Edits,
            edit =>
            {
                Assert.Equal("Test.Component5", edit.NewText);
                Assert.Equal(3, edit.Range.Start.Line);
                Assert.Equal(1, edit.Range.Start.Character);
                Assert.Equal(3, edit.Range.End.Line);
                Assert.Equal(19, edit.Range.End.Character);
            },
            edit =>
            {
                Assert.Equal("Test.Component5", edit.NewText);
                Assert.Equal(3, edit.Range.Start.Line);
                Assert.Equal(22, edit.Range.Start.Character);
                Assert.Equal(3, edit.Range.End.Line);
                Assert.Equal(40, edit.Range.End.Character);
            });
    }

    [Fact]
    public async Task Handle_Rename_MultipleFileUsages()
    {
        // Arrange
        var uri = new Uri("file:///c:/Second/Component3.razor");
        var request = new RenameParamsBridge
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri,
            },
            Position = new Position(1, 1),
            NewName = "Component5"
        };
        var documentContext = await GetDocumentContextAsync(uri);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await _endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.DocumentChanges.Value.Count());
        var renameChange = result.DocumentChanges.Value.ElementAt(0);
        Assert.True(renameChange.TryGetThird(out var renameFile));
        Assert.Equal(new Uri("file:///c:/Second/Component3.razor"), renameFile.OldUri);
        Assert.Equal(new Uri("file:///c:/Second/Component5.razor"), renameFile.NewUri);
        var editChange1 = result.DocumentChanges.Value.ElementAt(1);
        Assert.True(editChange1.TryGetFirst(out var textDocumentEdit));
        Assert.Equal("file:///c:/Second/Component3.razor", textDocumentEdit.TextDocument.Uri.ToString());
        Assert.Collection(
            textDocumentEdit.Edits,
            edit =>
            {
                Assert.Equal("Component5", edit.NewText);
                Assert.Equal(1, edit.Range.Start.Line);
                Assert.Equal(1, edit.Range.Start.Character);
                Assert.Equal(1, edit.Range.End.Line);
                Assert.Equal(11, edit.Range.End.Character);
            },
            edit =>
            {
                Assert.Equal("Component5", edit.NewText);
                Assert.Equal(1, edit.Range.Start.Line);
                Assert.Equal(14, edit.Range.Start.Character);
                Assert.Equal(1, edit.Range.End.Line);
                Assert.Equal(24, edit.Range.End.Character);
            });
        var editChange2 = result.DocumentChanges.Value.ElementAt(2);
        Assert.True(editChange2.TryGetFirst(out var textDocumentEdit2));
        Assert.Equal("file:///c:/Second/Component4.razor", textDocumentEdit2.TextDocument.Uri.ToString());
        Assert.Equal(2, textDocumentEdit2.Edits.Length);
    }

    [Fact]
    public async Task Handle_Rename_DifferentDirectories()
    {
        // Arrange
        var uri = new Uri("file:///c:/Dir1/Directory1.razor");
        var request = new RenameParamsBridge
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri,
            },
            Position = new Position(1, 1),
            NewName = "TestComponent"
        };
        var documentContext = await GetDocumentContextAsync(uri);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await _endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.DocumentChanges.Value.Count());
        var renameChange = result.DocumentChanges.Value.ElementAt(0);
        Assert.True(renameChange.TryGetThird(out var renameFile));
        Assert.Equal(new Uri("file:///c:/Dir2/Directory2.razor"), renameFile.OldUri);
        Assert.Equal(new Uri("file:///c:/Dir2/TestComponent.razor"), renameFile.NewUri);
        var editChange = result.DocumentChanges.Value.ElementAt(1);
        Assert.True(editChange.TryGetFirst(out var textDocumentEdit));
        Assert.Equal("file:///c:/Dir1/Directory1.razor", textDocumentEdit.TextDocument.Uri.ToString());
        Assert.Collection(
            textDocumentEdit.Edits,
            edit =>
            {
                Assert.Equal("TestComponent", edit.NewText);
                Assert.Equal(1, edit.Range.Start.Line);
                Assert.Equal(1, edit.Range.Start.Character);
                Assert.Equal(1, edit.Range.End.Line);
                Assert.Equal(11, edit.Range.End.Character);
            },
            edit =>
            {
                Assert.Equal("TestComponent", edit.NewText);
                Assert.Equal(1, edit.Range.Start.Line);
                Assert.Equal(14, edit.Range.Start.Character);
                Assert.Equal(1, edit.Range.End.Line);
                Assert.Equal(24, edit.Range.End.Character);
            });
    }

    [Fact]
    public async Task Handle_Rename_SingleServer_CallsDelegatedLanguageServer()
    {
        // Arrange
        var languageServerFeatureOptions = Mock.Of<LanguageServerFeatureOptions>(options => options.SupportsFileManipulation == true && options.SingleServerSupport == true, MockBehavior.Strict);

        var delegatedEdit = new WorkspaceEdit();

        var languageServerMock = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServerMock
            .Setup(c => c.SendRequestAsync<IDelegatedParams, WorkspaceEdit>(RazorLanguageServerCustomMessageTargets.RazorRenameEndpointName, It.IsAny<DelegatedRenameParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(delegatedEdit);

        var documentMappingServiceMock = new Mock<RazorDocumentMappingService>(MockBehavior.Strict);
        documentMappingServiceMock
            .Setup(c => c.GetLanguageKind(It.IsAny<RazorCodeDocument>(), It.IsAny<int>(), It.IsAny<bool>()))
            .Returns(RazorLanguageKind.CSharp);
        documentMappingServiceMock
            .Setup(c => c.RemapWorkspaceEditAsync(It.IsAny<WorkspaceEdit>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(delegatedEdit);

        var projectedPosition = new Position(1, 1);
        var projectedIndex = 1;
        documentMappingServiceMock.Setup(c => c.TryMapToProjectedDocumentPosition(It.IsAny<RazorCodeDocument>(), It.IsAny<int>(), out projectedPosition, out projectedIndex)).Returns(true);

        var endpoint = CreateEndpoint(languageServerFeatureOptions, documentMappingServiceMock.Object, languageServerMock.Object);

        var uri = new Uri("file:///c:/Second/ComponentWithParam.razor");
        var request = new RenameParamsBridge
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri,
            },
            Position = new Position(1, 0),
            NewName = "Test2"
        };

        var documentContext = await GetDocumentContextAsync(uri);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Same(delegatedEdit, result);
    }

    [Fact]
    public async Task Handle_Rename_SingleServer_DoesntDelegateForRazor()
    {
        // Arrange
        var languageServerFeatureOptions = Mock.Of<LanguageServerFeatureOptions>(options => options.SupportsFileManipulation == true && options.SingleServerSupport == true, MockBehavior.Strict);
        var languageServerMock = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        var documentMappingServiceMock = new Mock<RazorDocumentMappingService>(MockBehavior.Strict);
        documentMappingServiceMock
            .Setup(c => c.GetLanguageKind(It.IsAny<RazorCodeDocument>(), It.IsAny<int>(), It.IsAny<bool>()))
            .Returns(RazorLanguageKind.Razor);

        var endpoint = CreateEndpoint(languageServerFeatureOptions, documentMappingServiceMock.Object, languageServerMock.Object);

        var request = new RenameParamsBridge
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = new Uri("file:///c:/Second/ComponentWithParam.razor")
            },
            Position = new Position(1, 0),
            NewName = "Test2"
        };

        var documentContext = await GetDocumentContextAsync(request.TextDocument.Uri);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    private Task<DocumentContext> GetDocumentContextAsync(Uri file)
    {
        return _documentContextFactory.TryCreateAsync(file, DisposalToken);
    }

    private static IEnumerable<TagHelperDescriptor> CreateRazorComponentTagHelperDescriptors(string assemblyName, string namespaceName, string tagName)
    {
        var fullyQualifiedName = $"{namespaceName}.{tagName}";
        var builder = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, fullyQualifiedName, assemblyName);
        builder.TagMatchingRule(rule => rule.TagName = tagName);
        builder.SetTypeName(fullyQualifiedName);
        builder.SetTypeNameIdentifier(tagName);
        builder.SetTypeNamespace(namespaceName);
        yield return builder.Build();

        var fullyQualifiedBuilder = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, fullyQualifiedName, assemblyName);
        fullyQualifiedBuilder.TagMatchingRule(rule => rule.TagName = fullyQualifiedName);
        fullyQualifiedBuilder.SetTypeName(fullyQualifiedName);
        fullyQualifiedBuilder.SetTypeNameIdentifier(tagName);
        fullyQualifiedBuilder.SetTypeNamespace(namespaceName);
        fullyQualifiedBuilder.AddMetadata(ComponentMetadata.Component.NameMatchKey, ComponentMetadata.Component.FullyQualifiedNameMatch);
        yield return fullyQualifiedBuilder.Build();
    }

    private static TestRazorProjectItem CreateProjectItem(string text, string filePath)
    {
        return new TestRazorProjectItem(filePath, fileKind: FileKinds.Component)
        {
            Content = text
        };
    }

    private static DocumentContext CreateRazorDocumentContext(RazorProjectEngine projectEngine, TestRazorProjectItem item, string rootNamespaceName, IReadOnlyList<TagHelperDescriptor> tagHelpers)
    {
        var codeDocument = projectEngine.ProcessDesignTime(item);

        var namespaceNode = (NamespaceDeclarationIntermediateNode)codeDocument
            .GetDocumentIntermediateNode()
            .FindDescendantNodes<IntermediateNode>()
            .FirstOrDefault(n => n is NamespaceDeclarationIntermediateNode);
        namespaceNode.Content = rootNamespaceName;

        var sourceText = SourceText.From(new string(item.Content));
        var projectWorkspaceState = new ProjectWorkspaceState(tagHelpers, LanguageVersion.Default);
        var projectSnapshot = TestProjectSnapshot.Create("C:/project.csproj", projectWorkspaceState);
        var snapshot = Mock.Of<DocumentSnapshot>(d =>
            d.GetGeneratedOutputAsync() == Task.FromResult(codeDocument) &&
            d.FilePath == item.FilePath &&
            d.FileKind == FileKinds.Component &&
            d.GetTextAsync() == Task.FromResult(sourceText) &&
            d.Project == projectSnapshot, MockBehavior.Strict);
        var version = 1337;
        var documentSnapshot = new DocumentContext(new Uri(item.FilePath), snapshot, version);

        return documentSnapshot;
    }

    private RenameEndpoint CreateEndpoint(LanguageServerFeatureOptions languageServerFeatureOptions = null, RazorDocumentMappingService documentMappingService = null, ClientNotifierServiceBase languageServer = null)
    {
        var tagHelperDescriptors = new List<TagHelperDescriptor>();
        tagHelperDescriptors.AddRange(CreateRazorComponentTagHelperDescriptors("First", "First.Components", "Component1"));
        tagHelperDescriptors.AddRange(CreateRazorComponentTagHelperDescriptors("First", "Test", "Component2"));
        tagHelperDescriptors.AddRange(CreateRazorComponentTagHelperDescriptors("Second", "Second.Components", "Component3"));
        tagHelperDescriptors.AddRange(CreateRazorComponentTagHelperDescriptors("Second", "Second.Components", "Component4"));
        tagHelperDescriptors.AddRange(CreateRazorComponentTagHelperDescriptors("First", "Test", "Component1337"));
        tagHelperDescriptors.AddRange(CreateRazorComponentTagHelperDescriptors("First", "Test.Components", "Directory1"));
        tagHelperDescriptors.AddRange(CreateRazorComponentTagHelperDescriptors("First", "Test.Components", "Directory2"));

        var item1 = CreateProjectItem("@namespace First.Components\n@using Test\n<Component2></Component2>", "c:/First/Component1.razor");
        var item2 = CreateProjectItem("@namespace Test", "c:/First/Component2.razor");
        var item3 = CreateProjectItem("@namespace Second.Components\n<Component3></Component3>", "c:/Second/Component3.razor");
        var item4 = CreateProjectItem("@namespace Second.Components\n<Component3></Component3>\n<Component3></Component3>", "c:/Second/Component4.razor");
        var itemComponentParam = CreateProjectItem("@namespace Second.Components\n<Component3 Title=\"Something\"></Component3>", "c:/Second/Component5.razor");
        var item1337 = CreateProjectItem(string.Empty, "c:/First/Component1337.razor");
        var indexItem = CreateProjectItem("@namespace First.Components\n@using Test\n<Component1337></Component1337>\n<Test.Component1337></Test.Component1337>", "c:/First/Index.razor");

        var itemDirectory1 = CreateProjectItem("@namespace Test.Components\n<Directory2></Directory2>", "c:/Dir1/Directory1.razor");
        var itemDirectory2 = CreateProjectItem("@namespace Test.Components\n<Directory1></Directory1>", "c:/Dir2/Directory2.razor");

        var fileSystem = new TestRazorProjectFileSystem(new[] { item1, item2, item3, item4, itemComponentParam, indexItem, itemDirectory1, itemDirectory2 });

        var projectEngine = RazorProjectEngine.Create(RazorConfiguration.Default, fileSystem, builder =>
        {
            builder.AddDirective(NamespaceDirective.Directive);
            builder.AddTagHelpers(tagHelperDescriptors);
        });

        var component1 = CreateRazorDocumentContext(projectEngine, item1, "First.Components", tagHelperDescriptors);
        var component2 = CreateRazorDocumentContext(projectEngine, item2, "Test", tagHelperDescriptors);
        var component3 = CreateRazorDocumentContext(projectEngine, item3, "Second.Components", tagHelperDescriptors);
        var component4 = CreateRazorDocumentContext(projectEngine, item4, "Second.Components", tagHelperDescriptors);
        var componentWithParam = CreateRazorDocumentContext(projectEngine, itemComponentParam, "Second.Components", tagHelperDescriptors);
        var component1337 = CreateRazorDocumentContext(projectEngine, item1337, "Test", tagHelperDescriptors);
        var index = CreateRazorDocumentContext(projectEngine, indexItem, "First.Components", tagHelperDescriptors);
        var directory1Component = CreateRazorDocumentContext(projectEngine, itemDirectory1, "Test.Components", tagHelperDescriptors);
        var directory2Component = CreateRazorDocumentContext(projectEngine, itemDirectory2, "Test.Components", tagHelperDescriptors);

        _documentContextFactory = Mock.Of<DocumentContextFactory>(d =>
            d.TryCreateAsync(new Uri("c:/First/Component1.razor"), It.IsAny<CancellationToken>()) == Task.FromResult(component1) &&
            d.TryCreateAsync(new Uri("c:/First/Component2.razor"), It.IsAny<CancellationToken>()) == Task.FromResult(component2) &&
            d.TryCreateAsync(new Uri("c:/Second/Component3.razor"), It.IsAny<CancellationToken>()) == Task.FromResult(component3) &&
            d.TryCreateAsync(new Uri("c:/Second/Component4.razor"), It.IsAny<CancellationToken>()) == Task.FromResult(component4) &&
            d.TryCreateAsync(new Uri("c:/Second/ComponentWithParam.razor"), It.IsAny<CancellationToken>()) == Task.FromResult(componentWithParam) &&
            d.TryCreateAsync(new Uri(index.FilePath), It.IsAny<CancellationToken>()) == Task.FromResult(index) &&
            d.TryCreateAsync(new Uri(component1337.FilePath), It.IsAny<CancellationToken>()) == Task.FromResult(component1337) &&
            d.TryCreateAsync(new Uri(itemDirectory1.FilePath), It.IsAny<CancellationToken>()) == Task.FromResult(directory1Component) &&
            d.TryCreateAsync(new Uri(itemDirectory2.FilePath), It.IsAny<CancellationToken>()) == Task.FromResult(directory2Component), MockBehavior.Strict);

        var firstProject = Mock.Of<ProjectSnapshot>(p =>
            p.FilePath == "c:/First/First.csproj" &&
            p.DocumentFilePaths == new[] { "c:/First/Component1.razor", "c:/First/Component2.razor", itemDirectory1.FilePath, itemDirectory2.FilePath, component1337.FilePath } &&
            p.GetDocument("c:/First/Component1.razor") == component1.Snapshot &&
            p.GetDocument("c:/First/Component2.razor") == component2.Snapshot &&
            p.GetDocument(itemDirectory1.FilePath) == directory1Component.Snapshot &&
            p.GetDocument(itemDirectory2.FilePath) == directory2Component.Snapshot &&
            p.GetDocument(component1337.FilePath) == component1337.Snapshot, MockBehavior.Strict);

        var secondProject = Mock.Of<ProjectSnapshot>(p =>
            p.FilePath == "c:/Second/Second.csproj" &&
            p.DocumentFilePaths == new[] { "c:/Second/Component3.razor", "c:/Second/Component4.razor", index.FilePath } &&
            p.GetDocument("c:/Second/Component3.razor") == component3.Snapshot &&
            p.GetDocument("c:/Second/Component4.razor") == component4.Snapshot &&
            p.GetDocument("c:/Second/ComponentWithParam.razor") == componentWithParam.Snapshot &&
            p.GetDocument(index.FilePath) == index.Snapshot, MockBehavior.Strict);

        var projectSnapshotManager = Mock.Of<ProjectSnapshotManagerBase>(p => p.Projects == new[] { firstProject, secondProject }, MockBehavior.Strict);
        var projectSnapshotManagerAccessor = new TestProjectSnapshotManagerAccessor(projectSnapshotManager);

        var projectSnapshotManagerDispatcher = new LSPProjectSnapshotManagerDispatcher(LoggerFactory);

        _documentContextFactory = Mock.Of<DocumentContextFactory>(d =>
            d.TryCreateAsync(new Uri("c:/First/Component1.razor"), It.IsAny<CancellationToken>()) == Task.FromResult(component1) &&
            d.TryCreateAsync(new Uri("c:/First/Component2.razor"), It.IsAny<CancellationToken>()) == Task.FromResult(component2) &&
            d.TryCreateAsync(new Uri("c:/Second/Component3.razor"), It.IsAny<CancellationToken>()) == Task.FromResult(component3) &&
            d.TryCreateAsync(new Uri("c:/Second/Component4.razor"), It.IsAny<CancellationToken>()) == Task.FromResult(component4) &&
            d.TryCreateAsync(new Uri("c:/Second/ComponentWithParam.razor"), It.IsAny<CancellationToken>()) == Task.FromResult(componentWithParam) &&
            d.TryCreateAsync(new Uri(index.FilePath), It.IsAny<CancellationToken>()) == Task.FromResult(index) &&
            d.TryCreateAsync(new Uri(component1337.FilePath), It.IsAny<CancellationToken>()) == Task.FromResult(component1337) &&
            d.TryCreateAsync(new Uri(itemDirectory1.FilePath), It.IsAny<CancellationToken>()) == Task.FromResult(directory1Component) &&
            d.TryCreateAsync(new Uri(itemDirectory2.FilePath), It.IsAny<CancellationToken>()) == Task.FromResult(directory2Component), MockBehavior.Strict);

        var searchEngine = new DefaultRazorComponentSearchEngine(Dispatcher, projectSnapshotManagerAccessor, LoggerFactory);
        languageServerFeatureOptions ??= Mock.Of<LanguageServerFeatureOptions>(options => options.SupportsFileManipulation == true && options.SingleServerSupport == false, MockBehavior.Strict);

        var documentMappingServiceMock = new Mock<RazorDocumentMappingService>(MockBehavior.Strict);
        documentMappingServiceMock
            .Setup(c => c.GetLanguageKind(It.IsAny<RazorCodeDocument>(), It.IsAny<int>(), It.IsAny<bool>()))
            .Returns(Protocol.RazorLanguageKind.Html);
        documentMappingService ??= documentMappingServiceMock.Object;

        languageServer ??= Mock.Of<ClientNotifierServiceBase>(MockBehavior.Strict);

        var endpoint = new RenameEndpoint(
            projectSnapshotManagerDispatcher,
            _documentContextFactory,
            searchEngine,
            projectSnapshotManagerAccessor,
            languageServerFeatureOptions,
            documentMappingService,
            languageServer,
            LoggerFactory);

        return endpoint;
    }
}
