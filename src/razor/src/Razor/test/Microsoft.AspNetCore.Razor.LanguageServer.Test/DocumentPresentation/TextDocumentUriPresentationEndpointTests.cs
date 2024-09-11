// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentPresentation;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;

public class TextDocumentUriPresentationEndpointTests(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [OSSkipConditionFact(["OSX", "Linux"])]
    public async Task Handle_SimpleComponent_ReturnsResult()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        var project = await projectManager.UpdateAsync(updater =>
        {
            return updater.CreateAndAddProject("c:/path/project.csproj");
        });

        await projectManager.CreateAndAddDocumentAsync(project, "c:/path/index.razor");
        await projectManager.CreateAndAddDocumentAsync(project, "c:/path/MyTagHelper.razor");

        var droppedUri = new Uri("file:///c:/path/MyTagHelper.razor");
        var builder = TagHelperDescriptorBuilder.Create("MyTagHelper", "MyAssembly");
        builder.SetMetadata(TypeNameIdentifier("MyTagHelper"), TypeNamespace("TestRootNamespace"));

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectWorkspaceStateChanged(project.Key, ProjectWorkspaceState.Create([builder.Build()]));
        });

        var razorFilePath = "c:/path/index.razor";
        var uri = new Uri(razorFilePath);

        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentOpened(project.Key, razorFilePath, SourceText.From("<div></div>"));
        });

        var documentContextFactory = new DocumentContextFactory(projectManager, LoggerFactory);
        Assert.True(documentContextFactory.TryCreate(uri, projectContext: null, out var documentContext));

        var endpoint = CreateEndpoint(documentContextFactory);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new() { Uri = uri },
            Range = VsLspFactory.CreateSingleLineRange(line: 0, character: 1, length: 1),
            Uris = [droppedUri]
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.DocumentChanges);

        var documentChanges = result.DocumentChanges.GetValueOrDefault();
        Assert.True(documentChanges.TryGetFirst(out var documentEdits));

        Assert.Equal("<MyTagHelper />", documentEdits[0].Edits[0].NewText);
    }

    [OSSkipConditionFact(["OSX", "Linux"])]
    public async Task Handle_SimpleComponentWithChildFile_ReturnsResult()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        var project = await projectManager.UpdateAsync(updater =>
        {
            return updater.CreateAndAddProject("c:/path/project.csproj");
        });

        await projectManager.CreateAndAddDocumentAsync(project, "c:/path/index.razor");
        await projectManager.CreateAndAddDocumentAsync(project, "c:/path/MyTagHelper.razor");

        var droppedUri = new Uri("file:///c:/path/MyTagHelper.razor");
        var builder = TagHelperDescriptorBuilder.Create("MyTagHelper", "MyAssembly");
        builder.SetMetadata(TypeNameIdentifier("MyTagHelper"), TypeNamespace("TestRootNamespace"));

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectWorkspaceStateChanged(project.Key, ProjectWorkspaceState.Create([builder.Build()]));
        });

        var razorFilePath = "c:/path/index.razor";
        var uri = new Uri(razorFilePath);

        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentOpened(project.Key, razorFilePath, SourceText.From("<div></div>"));
        });

        var documentContextFactory = new DocumentContextFactory(projectManager, LoggerFactory);
        Assert.True(documentContextFactory.TryCreate(uri, null, out var documentContext));

        var endpoint = CreateEndpoint(documentContextFactory);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new() { Uri = uri },
            Range = VsLspFactory.CreateSingleLineRange(line: 0, character: 1, length: 1),
            Uris =
            [
                new Uri("file:///c:/path/MyTagHelper.razor.cs"),
                new Uri("file:///c:/path/MyTagHelper.razor.css"),
                droppedUri,
            ]
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.DocumentChanges);

        var documentChanges = result.DocumentChanges.GetValueOrDefault();
        Assert.True(documentChanges.TryGetFirst(out var documentEdits));

        Assert.Equal("<MyTagHelper />", documentEdits[0].Edits[0].NewText);
    }

    [OSSkipConditionFact(["OSX", "Linux"])]
    public async Task Handle_ComponentWithRequiredAttribute_ReturnsResult()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        var project = await projectManager.UpdateAsync(updater =>
        {
            return updater.CreateAndAddProject("c:/path/project.csproj");
        });

        await projectManager.CreateAndAddDocumentAsync(project, "c:/path/index.razor");
        await projectManager.CreateAndAddDocumentAsync(project, "c:/path/fetchdata.razor");

        var droppedUri = new Uri("file:///c:/path/fetchdata.razor");
        var builder = TagHelperDescriptorBuilder.Create("FetchData", "MyAssembly");
        builder.SetMetadata(TypeNameIdentifier("FetchData"), TypeNamespace("TestRootNamespace"));
        builder.BindAttribute(b =>
        {
            b.IsEditorRequired = true;
            b.Name = "MyAttribute";
        });
        builder.BindAttribute(b => b.Name = "MyNonRequiredAttribute");

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectWorkspaceStateChanged(project.Key, ProjectWorkspaceState.Create([builder.Build()]));
        });

        var razorFilePath = "c:/path/index.razor";
        var uri = new Uri(razorFilePath);

        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentOpened(project.Key, razorFilePath, SourceText.From("<div></div>"));
        });

        var documentContextFactory = new DocumentContextFactory(projectManager, LoggerFactory);
        Assert.True(documentContextFactory.TryCreate(uri, null, out var documentContext));

        var endpoint = CreateEndpoint(documentContextFactory);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new() { Uri = uri },
            Range = VsLspFactory.CreateSingleLineRange(line: 0, character: 1, length: 1),
            Uris = [droppedUri]
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.DocumentChanges);

        var documentChanges = result.DocumentChanges.GetValueOrDefault();
        Assert.True(documentChanges.TryGetFirst(out var documentEdits));

        Assert.Equal("<FetchData MyAttribute=\"\" />", documentEdits[0].Edits[0].NewText);
    }

    [Fact]
    public async Task Handle_NoTypeNameIdentifier_ReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("<div></div>");
        var uri = new Uri("file://path/test.razor");
        var droppedUri = new Uri("file:///c:/path/MyTagHelper.razor");

        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);
        var documentContext = CreateDocumentContext(uri, codeDocument);

        var clientConnection = CreateClientConnection(response: null);
        var endpoint = CreateEndpoint(documentContextFactory, clientConnection);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new() { Uri = uri },
            Range = VsLspFactory.CreateSingleLineRange(line: 0, character: 1, length: 1),
            Uris = [droppedUri]
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
        var codeDocument = CreateCodeDocument("<div></div>");

        var uri = new Uri("file://path/test.razor");
        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);
        var documentContext = CreateDocumentContext(uri, codeDocument);

        var clientConnection = CreateClientConnection(response: null);
        var endpoint = CreateEndpoint(documentContextFactory, clientConnection);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new() { Uri = uri },
            Range = VsLspFactory.CreateSingleLineRange(line: 0, character: 1, length: 1),
            Uris =
            [
                new Uri("file:///c:/path/SomeOtherFile.cs"),
                new Uri("file:///c:/path/Bar.Foo"),
                new Uri("file:///c:/path/MyTagHelper.razor"),
            ]
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
        var codeDocument = CreateCodeDocument("<div></div>");

        var droppedUri = new Uri("file:///c:/path/MyTagHelper.cshtml");
        var uri = new Uri("file://path/test.razor");
        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);
        var documentContext = CreateDocumentContext(uri, codeDocument);

        var clientConnection = CreateClientConnection(response: null);
        var endpoint = CreateEndpoint(documentContextFactory, clientConnection);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new() { Uri = uri },
            Range = VsLspFactory.CreateSingleLineRange(line: 0, character: 1, length: 1),
            Uris = [droppedUri]
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [OSSkipConditionFact(["OSX", "Linux"])]
    public async Task Handle_ComponentWithNestedFiles_ReturnsResult()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        var project = await projectManager.UpdateAsync(updater =>
        {
            return updater.CreateAndAddProject("c:/path/project.csproj");
        });

        await projectManager.CreateAndAddDocumentAsync(project, "c:/path/index.razor");
        await projectManager.CreateAndAddDocumentAsync(project, "c:/path/fetchdata.razor");

        var droppedUri1 = new Uri("file:///c:/path/fetchdata.razor.cs");
        var droppedUri2 = new Uri("file:///c:/path/fetchdata.razor");
        var builder = TagHelperDescriptorBuilder.Create("FetchData", "MyAssembly");
        builder.SetMetadata(TypeNameIdentifier("FetchData"), TypeNamespace("TestRootNamespace"));

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectWorkspaceStateChanged(project.Key, ProjectWorkspaceState.Create([builder.Build()]));
        });

        var razorFilePath = "c:/path/index.razor";
        var uri = new Uri(razorFilePath);

        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentOpened(project.Key, razorFilePath, SourceText.From("<div></div>"));
        });

        var documentSnapshot = projectManager
            .GetLoadedProject(project.Key)
            .GetDocument(razorFilePath);
        Assert.NotNull(documentSnapshot);

        var documentContextFactory = new DocumentContextFactory(projectManager, LoggerFactory);
        Assert.True(documentContextFactory.TryCreate(uri, projectContext: null, out var documentContext));

        var endpoint = CreateEndpoint(documentContextFactory);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new() { Uri = uri },
            Range = VsLspFactory.CreateSingleLineRange(line: 0, character: 1, length: 1),
            Uris = [droppedUri1, droppedUri2]
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.DocumentChanges);

        var documentChanges = result.DocumentChanges.GetValueOrDefault();
        Assert.True(documentChanges.TryGetFirst(out var documentEdits));

        Assert.Equal("<FetchData />", documentEdits[0].Edits[0].NewText);
    }

    [Fact]
    public async Task Handle_CSharp_ReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("@counter");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);

        var clientConnection = CreateClientConnection(response: null);
        var endpoint = CreateEndpoint(documentContextFactory, clientConnection);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new() { Uri = uri },
            Range = VsLspFactory.CreateSingleLineRange(line: 0, character: 1, length: 1)
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
        var codeDocument = CreateCodeDocument("<div></div>");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);

        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);

        var clientConnection = CreateClientConnection(response: null);
        var endpoint = CreateEndpoint(documentContextFactory, clientConnection);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new() { Uri = uri },
            Range = VsLspFactory.CreateSingleLineRange(line: 0, character: 1, length: 1)
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
        var codeDocument = CreateCodeDocument("<div></div>");
        codeDocument.SetUnsupported();
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);

        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);

        var clientConnection = CreateClientConnection(response: new WorkspaceEdit());
        var endpoint = CreateEndpoint(documentContextFactory, clientConnection);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new() { Uri = uri },
            Range = VsLspFactory.CreateSingleLineRange(line: 0, character: 1, length: 1)
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
        var codeDocument = CreateCodeDocument("<div></div>");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);

        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);

        var clientConnection = CreateClientConnection(response: null);
        var endpoint = CreateEndpoint(documentContextFactory, clientConnection);

        var parameters = new UriPresentationParams()
        {
            TextDocument = new() { Uri = uri },
            Range = VsLspFactory.CreateSingleLineRange(line: 0, character: 1, length: 1)
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    private TextDocumentUriPresentationEndpoint CreateEndpoint(
        IDocumentContextFactory documentContextFactory,
        IClientConnection? clientConnection = null)
    {
        return new TextDocumentUriPresentationEndpoint(
            StrictMock.Of<IDocumentMappingService>(),
            clientConnection ?? StrictMock.Of<IClientConnection>(),
            FilePathService,
            documentContextFactory,
            LoggerFactory);
    }

    private static IClientConnection CreateClientConnection(WorkspaceEdit? response)
        => TestMocks.CreateClientConnection(builder =>
        {
            builder.SetupSendRequest<IRazorPresentationParams, WorkspaceEdit?>(CustomMessageNames.RazorUriPresentationEndpoint, response);
        });
}
