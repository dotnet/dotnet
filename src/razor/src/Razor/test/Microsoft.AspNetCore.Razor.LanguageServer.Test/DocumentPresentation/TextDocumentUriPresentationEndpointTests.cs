// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;

public class TextDocumentUriPresentationEndpointTests : LanguageServerTestBase
{
    public TextDocumentUriPresentationEndpointTests(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task Handle_SimpleComponent_ReturnsResult()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.Create("<div></div>");
        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);

        var componentCodeDocument = TestRazorCodeDocument.Create("<div></div>");
        var droppedUri = new Uri("file:///c:/path/MyTagHelper.razor");
        var builder = TagHelperDescriptorBuilder.Create("MyTagHelper", "MyAssembly");
        builder.Metadata(TypeNameIdentifier("MyTagHelper"));
        var tagHelperDescriptor = builder.Build();

        var uri = new Uri("file://path/test.razor");

        var documentSnapshot = Mock.Of<IDocumentSnapshot>(s => s.GetGeneratedOutputAsync() == Task.FromResult(componentCodeDocument), MockBehavior.Strict);
        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);

        var documentContext = CreateDocumentContext(uri, codeDocument);
        var searchEngine = Mock.Of<RazorComponentSearchEngine>(
            s => s.TryGetTagHelperDescriptorAsync(It.IsAny<IDocumentSnapshot>(), It.IsAny<CancellationToken>()) == Task.FromResult(tagHelperDescriptor),
            MockBehavior.Strict);

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);

        var endpoint = new TextDocumentUriPresentationEndpoint(
            documentMappingService,
            searchEngine,
            languageServer.Object,
            FilePathService,
            documentContextFactory,
            Dispatcher,
            LoggerFactory);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri
            },
            Range = new Range
            {
                Start = new Position(0, 1),
                End = new Position(0, 2)
            },
            Uris = new[]
            {
                droppedUri
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("<MyTagHelper />", result!.DocumentChanges!.Value.First[0].Edits[0].NewText);
    }

    [Fact]
    public async Task Handle_SimpleComponentWithChildFile_ReturnsResult()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.Create("<div></div>");
        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);

        var componentCodeDocument = TestRazorCodeDocument.Create("<div></div>");
        var droppedUri = new Uri("file:///c:/path/MyTagHelper.razor");
        var builder = TagHelperDescriptorBuilder.Create("MyTagHelper", "MyAssembly");
        builder.Metadata(TypeNameIdentifier("MyTagHelper"));
        var tagHelperDescriptor = builder.Build();

        var documentSnapshot = Mock.Of<IDocumentSnapshot>(s => s.GetGeneratedOutputAsync() == Task.FromResult(componentCodeDocument), MockBehavior.Strict);

        var uri = new Uri("file://path/test.razor");
        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var searchEngine = Mock.Of<RazorComponentSearchEngine>(
            s => s.TryGetTagHelperDescriptorAsync(It.IsAny<IDocumentSnapshot>(), It.IsAny<CancellationToken>()) == Task.FromResult(tagHelperDescriptor),
            MockBehavior.Strict);

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);

        var endpoint = new TextDocumentUriPresentationEndpoint(
            documentMappingService,
            searchEngine,
            languageServer.Object,
            FilePathService,
            documentContextFactory,
            Dispatcher,
            LoggerFactory);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri
            },
            Range = new Range
            {
                Start = new Position(0, 1),
                End = new Position(0, 2)
            },
            Uris = new[]
            {
                new Uri("file:///c:/path/MyTagHelper.razor.cs"),
                new Uri("file:///c:/path/MyTagHelper.razor.css"),
                droppedUri,
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("<MyTagHelper />", result!.DocumentChanges!.Value.First[0].Edits[0].NewText);
    }

    [Fact]
    public async Task Handle_ComponentWithRequiredAttribute_ReturnsResult()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.Create("<div></div>");
        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);

        var componentCodeDocument = TestRazorCodeDocument.Create("<div></div>");
        var droppedUri = new Uri("file:///c:/path/MyTagHelper.razor");
        var builder = TagHelperDescriptorBuilder.Create("MyTagHelper", "MyAssembly");
        builder.Metadata(TypeNameIdentifier("MyTagHelper"));
        builder.BindAttribute(b =>
        {
            b.IsEditorRequired = true;
            b.Name = "MyAttribute";
        });
        builder.BindAttribute(b => b.Name = "MyNonRequiredAttribute");
        var tagHelperDescriptor = builder.Build();

        var documentSnapshot = Mock.Of<IDocumentSnapshot>(s => s.GetGeneratedOutputAsync() == Task.FromResult(componentCodeDocument), MockBehavior.Strict);

        var uri = new Uri("file://path/test.razor");
        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var searchEngine = Mock.Of<RazorComponentSearchEngine>(
            s => s.TryGetTagHelperDescriptorAsync(It.IsAny<IDocumentSnapshot>(), It.IsAny<CancellationToken>()) == Task.FromResult(tagHelperDescriptor),
            MockBehavior.Strict);

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);

        var endpoint = new TextDocumentUriPresentationEndpoint(
            documentMappingService,
            searchEngine,
            languageServer.Object,
            FilePathService,
            documentContextFactory,
            Dispatcher,
            LoggerFactory);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri
            },
            Range = new Range
            {
                Start = new Position(0, 1),
                End = new Position(0, 2)
            },
            Uris = new[]
            {
                droppedUri
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("<MyTagHelper MyAttribute=\"\" />", result!.DocumentChanges!.Value.First[0].Edits[0].NewText);
    }

    [Fact]
    public async Task Handle_NoTypeNameIdentifier_ReturnsNull()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.Create("<div></div>");
        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);

        var componentCodeDocument = TestRazorCodeDocument.Create("<div></div>");
        var droppedUri = new Uri("file:///c:/path/MyTagHelper.razor");
        var builder = TagHelperDescriptorBuilder.Create("MyTagHelper", "MyAssembly");
        var tagHelperDescriptor = builder.Build();

        var documentSnapshot = Mock.Of<IDocumentSnapshot>(s => s.GetGeneratedOutputAsync() == Task.FromResult(componentCodeDocument), MockBehavior.Strict);

        var uri = new Uri("file://path/test.razor");
        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var searchEngine = Mock.Of<RazorComponentSearchEngine>(
            s => s.TryGetTagHelperDescriptorAsync(It.IsAny<IDocumentSnapshot>(), It.IsAny<CancellationToken>()) == Task.FromResult(tagHelperDescriptor),
            MockBehavior.Strict);

        var response = (WorkspaceEdit?)null;

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<IRazorPresentationParams, WorkspaceEdit?>(CustomMessageNames.RazorUriPresentationEndpoint, It.IsAny<IRazorPresentationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var endpoint = new TextDocumentUriPresentationEndpoint(
            documentMappingService,
            searchEngine,
            languageServer.Object,
            FilePathService,
            documentContextFactory,
            Dispatcher,
            LoggerFactory);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri
            },
            Range = new Range
            {
                Start = new Position(0, 1),
                End = new Position(0, 2)
            },
            Uris = new[]
            {
                droppedUri
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_MultipleUris_ReturnsNull()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.Create("<div></div>");
        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);

        var documentSnapshot = Mock.Of<IDocumentSnapshot>(s => s.GetGeneratedOutputAsync() == Task.FromResult(codeDocument), MockBehavior.Strict);

        var uri = new Uri("file://path/test.razor");
        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var searchEngine = Mock.Of<RazorComponentSearchEngine>(MockBehavior.Strict);

        var response = (WorkspaceEdit?)null;

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<IRazorPresentationParams, WorkspaceEdit?>(CustomMessageNames.RazorUriPresentationEndpoint, It.IsAny<IRazorPresentationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var endpoint = new TextDocumentUriPresentationEndpoint(
            documentMappingService,
            searchEngine,
            languageServer.Object,
            FilePathService,
            documentContextFactory,
            Dispatcher,
            LoggerFactory);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri
            },
            Range = new Range
            {
                Start = new Position(0, 1),
                End = new Position(0, 2)
            },
            Uris = new[]
            {
                new Uri("file:///c:/path/SomeOtherFile.cs"),
                new Uri("file:///c:/path/Bar.Foo"),
                new Uri("file:///c:/path/MyTagHelper.razor"),
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_NotComponent_ReturnsNull()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.Create("<div></div>");
        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);

        var documentSnapshot = Mock.Of<IDocumentSnapshot>(s => s.GetGeneratedOutputAsync() == Task.FromResult(codeDocument), MockBehavior.Strict);

        var droppedUri = new Uri("file:///c:/path/MyTagHelper.cshtml");
        var uri = new Uri("file://path/test.razor");
        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var searchEngine = Mock.Of<RazorComponentSearchEngine>(MockBehavior.Strict);

        var response = (WorkspaceEdit?)null;

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<IRazorPresentationParams, WorkspaceEdit?>(CustomMessageNames.RazorUriPresentationEndpoint, It.IsAny<IRazorPresentationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var endpoint = new TextDocumentUriPresentationEndpoint(
            documentMappingService,
            searchEngine,
            languageServer.Object,
            FilePathService,
            documentContextFactory,
            Dispatcher,
            LoggerFactory);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri
            },
            Range = new Range
            {
                Start = new Position(0, 1),
                End = new Position(0, 2)
            },
            Uris = new[]
            {
                droppedUri
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_ComponentWithNestedFiles_ReturnsResult()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.Create(@"<FetchData\>");
        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);

        var documentSnapshot = Mock.Of<IDocumentSnapshot>(s => s.GetGeneratedOutputAsync() == Task.FromResult(codeDocument), MockBehavior.Strict);

        var droppedUri1 = new Uri("file:///c:/path/fetchdata.razor.cs");
        var droppedUri2 = new Uri("file:///c:/path/fetchdata.razor");
        var builder = TagHelperDescriptorBuilder.Create("FetchData", "MyAssembly");
        builder.SetMetadata(TypeNameIdentifier("FetchData"));
        var tagHelperDescriptor = builder.Build();

        var uri = new Uri("file://path/index.razor");
        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var searchEngine = Mock.Of<RazorComponentSearchEngine>(
            s => s.TryGetTagHelperDescriptorAsync(It.IsAny<IDocumentSnapshot>(), It.IsAny<CancellationToken>()) == Task.FromResult(tagHelperDescriptor),
            MockBehavior.Strict);

        var response = (WorkspaceEdit?)null;

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<IRazorPresentationParams, WorkspaceEdit?>(CustomMessageNames.RazorUriPresentationEndpoint, It.IsAny<IRazorPresentationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var endpoint = new TextDocumentUriPresentationEndpoint(
            documentMappingService,
            searchEngine,
            languageServer.Object,
            FilePathService,
            documentContextFactory,
            Dispatcher,
            LoggerFactory);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri
            },
            Range = new Range
            {
                Start = new Position(0, 1),
                End = new Position(0, 2)
            },
            Uris = new[]
            {
                droppedUri1,
                droppedUri2,
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("<FetchData />", result!.DocumentChanges!.Value.First[0].Edits[0].NewText);
    }

    [Fact]
    public async Task Handle_CSharp_ReturnsNull()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.Create("@counter");
        var csharpDocument = codeDocument.GetCSharpDocument();
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var projectedRange = It.IsAny<LinePositionSpan>();
        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.CSharp &&
            s.TryMapToGeneratedDocumentRange(csharpDocument, It.IsAny<LinePositionSpan>(), out projectedRange) == true, MockBehavior.Strict);
        var searchEngine = Mock.Of<RazorComponentSearchEngine>(MockBehavior.Strict);

        var documentSnapshot = Mock.Of<IDocumentSnapshot>(s => s.GetGeneratedOutputAsync() == Task.FromResult(codeDocument), MockBehavior.Strict);
        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);

        var response = (WorkspaceEdit?)null;

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<IRazorPresentationParams, WorkspaceEdit?>(CustomMessageNames.RazorUriPresentationEndpoint, It.IsAny<IRazorPresentationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var endpoint = new TextDocumentUriPresentationEndpoint(
            documentMappingService,
            searchEngine,
            languageServer.Object,
            FilePathService,
            documentContextFactory,
            Dispatcher,
            LoggerFactory);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri
            },
            Range = new Range
            {
                Start = new Position(0, 1),
                End = new Position(0, 2)
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_DocumentNotFound_ReturnsNull()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.Create("<div></div>");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);
        var searchEngine = Mock.Of<RazorComponentSearchEngine>(MockBehavior.Strict);

        var documentSnapshot = Mock.Of<IDocumentSnapshot>(s => s.GetGeneratedOutputAsync() == Task.FromResult(codeDocument), MockBehavior.Strict);
        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);

        var response = (WorkspaceEdit?)null;

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<IRazorPresentationParams, WorkspaceEdit?>(CustomMessageNames.RazorUriPresentationEndpoint, It.IsAny<IRazorPresentationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var endpoint = new TextDocumentUriPresentationEndpoint(
            documentMappingService,
            searchEngine,
            languageServer.Object,
            FilePathService,
            documentContextFactory,
            Dispatcher,
            LoggerFactory);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri
            },
            Range = new Range
            {
                Start = new Position(0, 1),
                End = new Position(0, 2)
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_UnsupportedCodeDocument_ReturnsNull()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.Create("<div></div>");
        codeDocument.SetUnsupported();
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);
        var searchEngine = Mock.Of<RazorComponentSearchEngine>(MockBehavior.Strict);

        var documentSnapshot = Mock.Of<IDocumentSnapshot>(s => s.GetGeneratedOutputAsync() == Task.FromResult(codeDocument), MockBehavior.Strict);
        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);

        var response = new WorkspaceEdit();

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<IRazorPresentationParams, WorkspaceEdit?>(CustomMessageNames.RazorUriPresentationEndpoint, It.IsAny<IRazorPresentationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var endpoint = new TextDocumentUriPresentationEndpoint(
            documentMappingService,
            searchEngine,
            languageServer.Object,
            FilePathService,
            documentContextFactory,
            Dispatcher,
            LoggerFactory);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri
            },
            Range = new Range
            {
                Start = new Position(0, 1),
                End = new Position(0, 2)
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_NoUris_ReturnsNull()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.Create("<div></div>");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);
        var searchEngine = Mock.Of<RazorComponentSearchEngine>(MockBehavior.Strict);

        var documentSnapshot = Mock.Of<IDocumentSnapshot>(s => s.GetGeneratedOutputAsync() == Task.FromResult(codeDocument), MockBehavior.Strict);
        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);

        var response = (WorkspaceEdit?)null;

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<IRazorPresentationParams, WorkspaceEdit?>(CustomMessageNames.RazorUriPresentationEndpoint, It.IsAny<IRazorPresentationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var endpoint = new TextDocumentUriPresentationEndpoint(
            documentMappingService,
            searchEngine,
            languageServer.Object,
            FilePathService,
            documentContextFactory,
            Dispatcher,
            LoggerFactory);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri
            },
            Range = new Range
            {
                Start = new Position(0, 1),
                End = new Position(0, 2)
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }
}
