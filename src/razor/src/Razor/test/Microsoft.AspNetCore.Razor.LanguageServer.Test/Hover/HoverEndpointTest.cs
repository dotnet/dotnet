// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.Hover;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text.Adornments;
using Moq;
using Xunit;
using Xunit.Abstractions;
using LspHover = Microsoft.VisualStudio.LanguageServer.Protocol.Hover;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Hover;

public class HoverEndpointTest(ITestOutputHelper testOutput) : TagHelperServiceTestBase(testOutput)
{
    [Fact]
    public async Task Handle_Hover_SingleServer_CallsDelegatedLanguageServer()
    {
        // Arrange
        var languageServerFeatureOptions = StrictMock.Of<LanguageServerFeatureOptions>(options =>
            options.SingleServerSupport == true &&
            options.UseRazorCohostServer == false);

        var delegatedHover = new LspHover();

        var clientConnection = TestMocks.CreateClientConnection(builder =>
        {
            builder.SetupSendRequest<IDelegatedParams, LspHover>(CustomMessageNames.RazorHoverEndpointName, response: delegatedHover);
        });

        var documentMappingServiceMock = new StrictMock<IDocumentMappingService>();

        var outRange = new LinePositionSpan();
        documentMappingServiceMock
            .Setup(x => x.TryMapToGeneratedDocumentRange(It.IsAny<IRazorGeneratedDocument>(), It.IsAny<LinePositionSpan>(), out outRange))
            .Returns(true);

        var projectedPosition = new LinePosition(1, 1);
        var projectedIndex = 1;
        documentMappingServiceMock
            .Setup(x => x.TryMapToGeneratedDocumentPosition(It.IsAny<IRazorGeneratedDocument>(), It.IsAny<int>(), out projectedPosition, out projectedIndex))
            .Returns(true);

        var endpoint = CreateEndpoint(languageServerFeatureOptions, documentMappingServiceMock.Object, clientConnection);

        var (documentContext, position) = CreateDefaultDocumentContext();
        var requestContext = CreateRazorRequestContext(documentContext);

        var request = new TextDocumentPositionParams
        {
            TextDocument = new() { Uri = new Uri("C:/text.razor") },
            Position = position,
        };

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Same(delegatedHover, result);
    }

    [Fact]
    public async Task Handle_Hover_SingleServer_CSharpVariable()
    {
        // Arrange
        TestCode code = """
            <div></div>

            @{
                var $$myVariable = "Hello";

                var length = myVariable.Length;
            }
            """;

        // Act
        var result = await GetResultFromSingleServerEndpointAsync(code);

        // Assert
        Assert.NotNull(result);
        var range = result.Range;
        var expected = VsLspFactory.CreateSingleLineRange(line: 3, character: 8, length: 10);

        Assert.Equal(expected, range);

        Assert.NotNull(result.RawContent);
        var rawContainer = (ContainerElement)result.RawContent;
        var embeddedContainerElement = (ContainerElement)rawContainer.Elements.Single();

        var classifiedText = (ClassifiedTextElement)embeddedContainerElement.Elements.ElementAt(1);
        var text = string.Join("", classifiedText.Runs.Select(r => r.Text));

        // No need to validate exact text, in case the c# language server changes. Just
        // verify that the expected variable is represented within the hover text
        Assert.Contains("myVariable", text);
    }

    [Fact]
    public async Task Handle_Hover_SingleServer_Component()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly

            <$$test1></test1>
            """;

        // Act
        var result = await GetResultFromSingleServerEndpointAsync(code);

        // Assert
        Assert.NotNull(result);
        var range = result.Range;
        var expected = VsLspFactory.CreateSingleLineRange(line: 2, character: 1, length: 5);

        Assert.Equal(expected, range);

        Assert.NotNull(result.RawContent);
        var rawContainer = (ContainerElement)result.RawContent;
        var embeddedContainerElement = (ContainerElement)rawContainer.Elements.Single();

        var classifiedText = (ClassifiedTextElement)embeddedContainerElement.Elements.ElementAt(1);
        var text = string.Join("", classifiedText.Runs.Select(r => r.Text));
        Assert.Equal("Test1TagHelper", text);
    }

    [Fact]
    public async Task Handle_Hover_SingleServer_AddTagHelper()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, Test$$Assembly

            <test1></test1>
            """;

        // Act
        var result = await GetResultFromSingleServerEndpointAsync(code);

        // Assert

        // Roslyn returns us a range that is outside of our source mappings, so we expect the endpoint
        // to return null, so as not to confuse the client
        Assert.NotNull(result);
        Assert.Null(result.Range);

        Assert.NotNull(result.RawContent);
        var rawContainer = (ContainerElement)result.RawContent;
        var embeddedContainerElement = (ContainerElement)rawContainer.Elements.Single();

        if (embeddedContainerElement.Elements.FirstOrDefault() is ContainerElement headerContainer)
        {
            embeddedContainerElement = headerContainer;
        }

        var classifiedText = (ClassifiedTextElement)embeddedContainerElement.Elements.ElementAt(1);
        var text = string.Join("", classifiedText.Runs.Select(r => r.Text));
        // Hover info is for a string
        Assert.StartsWith("class System.String", text);
    }

    private async Task<VSInternalHover?> GetResultFromSingleServerEndpointAsync(TestCode code)
    {
        var codeDocument = CreateCodeDocument(code.Text, DefaultTagHelpers);
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var csharpDocumentUri = new Uri("C:/path/to/file.razor__virtual.g.cs");
        var serverCapabilities = new VSInternalServerCapabilities()
        {
            HoverProvider = true
        };

        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, serverCapabilities, razorMappingService: null, capabilitiesUpdater: null, DisposalToken);
        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString());

        var razorFilePath = "C:/path/to/file.razor";
        var documentContextFactory = new TestDocumentContextFactory(razorFilePath, codeDocument);
        var languageServerFeatureOptions = StrictMock.Of<LanguageServerFeatureOptions>(options =>
            options.SupportsFileManipulation == true &&
            options.SingleServerSupport == true &&
            options.CSharpVirtualDocumentSuffix == ".g.cs" &&
            options.HtmlVirtualDocumentSuffix == ".g.html" &&
            options.UseRazorCohostServer == false);
        var languageServer = new HoverLanguageServer(csharpServer, csharpDocumentUri, DisposalToken);
        var documentMappingService = new LspDocumentMappingService(FilePathService, documentContextFactory, LoggerFactory);

        var projectManager = CreateProjectSnapshotManager();
        var componentAvailabilityService = new ComponentAvailabilityService(projectManager);

        var clientCapabilities = new VSInternalClientCapabilities()
        {
            TextDocument = new() { Hover = new() { ContentFormat = [MarkupKind.PlainText, MarkupKind.Markdown] } },
            SupportsVisualStudioExtensions = true
        };

        var clientCapabilitiesService = new TestClientCapabilitiesService(clientCapabilities);

        var endpoint = new HoverEndpoint(
            componentAvailabilityService,
            clientCapabilitiesService,
            languageServerFeatureOptions,
            documentMappingService,
            languageServer,
            LoggerFactory);

        var razorFileUri = new Uri(razorFilePath);
        var request = new TextDocumentPositionParams
        {
            TextDocument = new() { Uri = razorFileUri, },
            Position = codeDocument.Source.Text.GetPosition(code.Position)
        };

        var documentContext = CreateDocumentContext(razorFileUri, codeDocument);
        var requestContext = CreateRazorRequestContext(documentContext: documentContext);

        var hover = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Note: This should always be a VSInternalHover because
        // VSInternalClientCapabilities.SupportsVisualStudioExtensions is set to true above.
        return Assert.IsType<VSInternalHover>(hover);
    }

    private static (DocumentContext, Position) CreateDefaultDocumentContext()
    {
        TestCode code = """
            @addTagHelper *, TestAssembly
            <any @test="Increment" />
            @code{
                public void $$Increment(){
                }
            }
            """;

        var path = "C:/text.razor";
        var codeDocument = CreateCodeDocument(code.Text, path, DefaultTagHelpers);
        var projectWorkspaceState = ProjectWorkspaceState.Create(DefaultTagHelpers);

        var hostProject = TestHostProject.Create("C:/project.csproj");
        var projectSnapshot = TestMocks.CreateProjectSnapshot(hostProject, projectWorkspaceState);

        var documentSnapshotMock = new StrictMock<IDocumentSnapshot>();
        documentSnapshotMock
            .Setup(x => x.GetGeneratedOutputAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument);
        documentSnapshotMock
            .Setup(x => x.GetTextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument.Source.Text);
        documentSnapshotMock
            .SetupGet(x => x.FilePath)
            .Returns(path);
        documentSnapshotMock
            .SetupGet(x => x.FileKind)
            .Returns(FileKinds.Component);
        documentSnapshotMock
            .SetupGet(x => x.Version)
            .Returns(0);
        documentSnapshotMock
            .SetupGet(x => x.Project)
            .Returns(projectSnapshot);

        var documentContext = new DocumentContext(new Uri(path), documentSnapshotMock.Object, projectContext: null);
        var position = codeDocument.Source.Text.GetPosition(code.Position);

        return (documentContext, position);
    }

    private HoverEndpoint CreateEndpoint(
        LanguageServerFeatureOptions? languageServerFeatureOptions = null,
        IDocumentMappingService? documentMappingService = null,
        IClientConnection? clientConnection = null)
    {
        languageServerFeatureOptions ??= StrictMock.Of<LanguageServerFeatureOptions>(options =>
            options.SupportsFileManipulation == true &&
            options.SingleServerSupport == false);

        documentMappingService ??= StrictMock.Of<IDocumentMappingService>();

        clientConnection ??= StrictMock.Of<IClientConnection>();

        var projectManager = CreateProjectSnapshotManager();
        var componentAvailabilityService = new ComponentAvailabilityService(projectManager);

        var clientCapabilities = new VSInternalClientCapabilities()
        {
            TextDocument = new() { Hover = new() { ContentFormat = [MarkupKind.PlainText, MarkupKind.Markdown] } },
            SupportsVisualStudioExtensions = true
        };

        var clientCapabilitiesService = new TestClientCapabilitiesService(clientCapabilities);

        var endpoint = new HoverEndpoint(
            componentAvailabilityService,
            clientCapabilitiesService,
            languageServerFeatureOptions,
            documentMappingService,
            clientConnection,
            LoggerFactory);

        return endpoint;
    }

    private class HoverLanguageServer : IClientConnection
    {
        private readonly CSharpTestLspServer _csharpServer;
        private readonly Uri _csharpDocumentUri;
        private readonly CancellationToken _cancellationToken;

        public HoverLanguageServer(
            CSharpTestLspServer csharpServer,
            Uri csharpDocumentUri,
            CancellationToken cancellationToken)
        {
            _csharpServer = csharpServer;
            _csharpDocumentUri = csharpDocumentUri;
            _cancellationToken = cancellationToken;
        }

        public Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SendNotificationAsync(string method, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
        {
            Assert.Equal(CustomMessageNames.RazorHoverEndpointName, method);
            var hoverParams = Assert.IsType<DelegatedPositionParams>(@params);

            var hoverRequest = new TextDocumentPositionParams()
            {
                TextDocument = new() { Uri = _csharpDocumentUri, },
                Position = hoverParams.ProjectedPosition
            };

            var result = await _csharpServer.ExecuteRequestAsync<TextDocumentPositionParams, TResponse>(
                Methods.TextDocumentHoverName, hoverRequest, _cancellationToken);

            return result;
        }
    }
}
