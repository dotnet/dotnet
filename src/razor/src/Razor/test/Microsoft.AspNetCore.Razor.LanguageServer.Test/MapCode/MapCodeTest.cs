// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.MapCode;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.MapCode;

public class MapCodeTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    private const string RazorFilePath = "C:/path/to/file.razor";

    [Fact]
    public async Task HandleRazorSingleLineInsertionAsync()
    {
        var originalCode = """
                <h3>Component</h3>
                $$
                @code {

                }
                
                """;

        var codeToMap = """
            <PageTitle>Title</PageTitle>
            """;

        var expectedCode = """
            <h3>Component</h3>
            <PageTitle>Title</PageTitle>
            @code {

            }
            
            """;

        await VerifyCodeMappingAsync(originalCode, [codeToMap], expectedCode);
    }

    [Fact(Skip = "C# needs to implement + merge their LSP-based mapper before this test can pass")]
    public async Task HandleCSharpInsertionAsync()
    {
        var originalCode = """
                @code
                {
                    public string Title { get; set; }$$
                }

                """;

        var codeToMap = """
            @code
            {
                public string Title { get; set; }

                void M()
                {
                    var x = 1;
                }
            }

            """;

        var expectedCode = """
            @code
            {
                public string Title { get; set; }
            
                void M()
                {
                    var x = 1;
                }
            }
            
            """;

        await VerifyCodeMappingAsync(originalCode, [codeToMap], expectedCode);
    }

    [Fact]
    public async Task HandleHtmlMultiLineInsertionAsync()
    {
        var originalCode = """
                <h3>Component</h3>
                $$
                @code {

                }
                
                """;

        var codeToMap = """
            <h1>Title</h1>
            <h2>Subtitle</h2>
            """;

        var expectedCode = """
            <h3>Component</h3>
            <h1>Title</h1><h2>Subtitle</h2>
            @code {

            }
            
            """;

        await VerifyCodeMappingAsync(originalCode, [codeToMap], expectedCode);
    }

    [Fact]
    public async Task HandleIgnoreExistingCodeAsync()
    {
        var originalCode = """
                @page "/"

                <PageTitle>Index</PageTitle>

                <h1>Hello, world!</h1>

                $$

                Welcome to your new app.
                
                """;

        var codeToMap = """
            @page "/"

            <PageTitle>Index</PageTitle>

            <h1>Hello, world!</h1>

            <button>Click me</button>

            Welcome to your new app.
            
            """;

        var expectedCode = """
            @page "/"
            
            <PageTitle>Index</PageTitle>
            
            <h1>Hello, world!</h1>
            
            <button>Click me</button>
            
            Welcome to your new app.
            
            """;

        await VerifyCodeMappingAsync(originalCode, [codeToMap], expectedCode);
    }

    [Fact]
    public async Task HandleMultipleFocusLocationsAsync()
    {
        var originalCode = """
                <h3>Component</h3>
                $$
                @code {

                }
                
                """;

        var codeToMap = """
            <PageTitle>Title</PageTitle>
            """;

        var expectedCode = """
            <h3>Component</h3>
            <PageTitle>Title</PageTitle>
            @code {
            
            }
            
            """;

        await VerifyCodeMappingAsync(originalCode, [codeToMap], expectedCode);
    }

    [Fact]
    public async Task HandleFocusLocationInMiddleOfNodeAsync()
    {
        var originalCode = """
                <h3>Component</h$$3>

                @code {

                }
                
                """;

        var codeToMap = """
            <PageTitle>Title</PageTitle>
            """;

        // Code mapper isn't responsible for formatting
        var expectedCode = """
            <h3>Component</h3><PageTitle>Title</PageTitle>
            
            @code {
            
            }
            
            """;

        await VerifyCodeMappingAsync(originalCode, [codeToMap], expectedCode);
    }

    [Fact(Skip = "C# needs to implement + merge their LSP-based mapper before this test can pass")]
    public async Task HandleCodeBlockInsertionAsync()
    {
        var originalCode = """
                $$
                """;

        var codeToMap = """
            @code {
                public string Title { get; set; }
            }
            """;

        var expectedCode = """
            @code {
                public string Title { get; set; }
            }
            """;

        await VerifyCodeMappingAsync(originalCode, [codeToMap], expectedCode);
    }

    [Fact]
    public async Task HandleRazorDirectiveAttributeAsync()
    {
        var originalCode = """
                @page "/fetchdata"
                @using Microsoft.AspNetCore.Authorization
                $$
                """;

        var codeToMap = """
            @attribute [Authorize]

            """;

        var expectedCode = """
            @page "/fetchdata"
            @using Microsoft.AspNetCore.Authorization
            @attribute [Authorize]

            """;

        await VerifyCodeMappingAsync(originalCode, [codeToMap], expectedCode);
    }

    private async Task VerifyCodeMappingAsync(
        string originalCode,
        string[] codeToMap,
        string expectedCode,
        string razorFilePath = RazorFilePath,
        LspLocation[][]? locations = null)
    {
        // Arrange
        TestFileMarkupParser.GetPositionAndSpans(originalCode, out var output, out int cursorPosition, out ImmutableArray<TextSpan> spans);
        var codeDocument = CreateCodeDocument(output, filePath: razorFilePath);
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var csharpDocumentUri = new Uri(razorFilePath + "__virtual.g.cs");
        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, new VSInternalServerCapabilities(), razorMappingService: null, capabilitiesUpdater: null, DisposalToken);
        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString(), DisposalToken);

        var documentContextFactory = new TestDocumentContextFactory(razorFilePath, codeDocument);
        var languageServer = new MapCodeServer(csharpServer, csharpDocumentUri);
        var documentMappingService = new LspDocumentMappingService(FilePathService, documentContextFactory, LoggerFactory);

        var endpoint = new MapCodeEndpoint(documentMappingService, documentContextFactory, languageServer, NoOpTelemetryReporter.Instance);

        var capabilitiesProvider = Assert.IsAssignableFrom<ICapabilitiesProvider>(endpoint);

        // Make sure the capabilities are applied
        var serverCapabilities = new VSInternalServerCapabilities();
        capabilitiesProvider.ApplyCapabilities(serverCapabilities, new());
        Assert.True(serverCapabilities.MapCodeProvider);

        var sourceText = codeDocument.Source.Text;

        var mappings = new VSInternalMapCodeMapping[]
        {
            new() {
                TextDocument = new TextDocumentIdentifier
                {
                    DocumentUri = new(new Uri(razorFilePath))
                },
                FocusLocations = locations ??
                [
                    [
                        new LspLocation
                        {
                            Range = sourceText.GetZeroWidthRange(cursorPosition),
                            DocumentUri = new(new Uri(razorFilePath))
                        }
                    ]
                ],
                Contents = codeToMap
            }
        };
        var request = new VSInternalMapCodeParams
        {
            Mappings = mappings
        };

        Assert.True(documentContextFactory.TryCreate(request.Mappings[0].TextDocument!, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);

        var actualCode = ApplyWorkspaceEdit(result, new Uri(razorFilePath), sourceText);
        AssertEx.EqualOrDiff(expectedCode, actualCode.ToString());
    }

    private sealed class MapCodeServer(CSharpTestLspServer csharpServer, Uri csharpDocumentUri) : IClientConnection
    {
        public Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
            => throw new NotImplementedException();

        public Task SendNotificationAsync(string method, CancellationToken cancellationToken)
            => throw new NotImplementedException();

        public Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
        {
            Assert.Equal(CustomMessageNames.RazorMapCodeEndpoint, method);
            var delegatedMapCodeParams = Assert.IsType<DelegatedMapCodeParams>(@params);

            var mappings = new VSInternalMapCodeMapping[]
            {
                new() {
                    TextDocument = new TextDocumentIdentifier()
                    {
                        DocumentUri = new(csharpDocumentUri)
                    },
                    Contents = delegatedMapCodeParams.Contents,
                    FocusLocations = delegatedMapCodeParams.FocusLocations
                }
            };
            var mapCodeRequest = new VSInternalMapCodeParams()
            {
                Mappings = mappings
            };

            return csharpServer.ExecuteRequestAsync<VSInternalMapCodeParams, TResponse>(
                VSInternalMethods.WorkspaceMapCodeName, mapCodeRequest, cancellationToken);
        }
    }

    private static SourceText ApplyWorkspaceEdit(WorkspaceEdit workspaceEdit, Uri documentUri, SourceText sourceText)
    {
        Assert.NotNull(workspaceEdit.DocumentChanges);
        var edits = workspaceEdit.DocumentChanges.Value.First;

        foreach (var edit in edits)
        {
            Assert.Equal(documentUri, edit.TextDocument.DocumentUri.GetRequiredParsedUri());

            foreach (var currentEdit in edit.Edits)
            {
                sourceText = sourceText.WithChanges(sourceText.GetTextChange(((TextEdit)currentEdit)));
            }
        }

        return sourceText;
    }
}
