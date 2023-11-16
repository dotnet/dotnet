// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentSynchronization;

public class DocumentDidChangeEndpointTest : LanguageServerTestBase
{
    private readonly RazorProjectService _projectService;

    public DocumentDidChangeEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _projectService = Mock.Of<RazorProjectService>(MockBehavior.Strict);
    }

    [Fact]
    public void ApplyContentChanges_SingleChange()
    {
        // Arrange
        var endpoint = new DocumentDidChangeEndpoint(Dispatcher, _projectService);
        var sourceText = SourceText.From("Hello World");
        var change = new TextDocumentContentChangeEvent()
        {
            Range = new Range
            {
                Start = new Position(0, 5),
                End = new Position(0, 5),
            },
            RangeLength = 0,
            Text = "!"
        };

        // Act
        var result = endpoint.ApplyContentChanges(new[] { change }, sourceText, Logger);

        // Assert
        var resultString = GetString(result);
        Assert.Equal("Hello! World", resultString);
    }

    [Fact]
    public void ApplyContentChanges_MultipleChanges()
    {
        // Arrange
        var endpoint = new DocumentDidChangeEndpoint(Dispatcher, _projectService);
        var sourceText = SourceText.From("Hello World");
        var changes = new[] {
            new TextDocumentContentChangeEvent()
            {
                Range = new Range{
                    Start = new Position(0, 5),
                    End = new Position(0, 5)
                },
                RangeLength = 0,
                Text = Environment.NewLine
            },
            // Hello
            //  World

            new TextDocumentContentChangeEvent()
            {
                Range = new Range{
                    Start = new Position(1, 0),
                    End = new Position(1, 0),
                },
                RangeLength = 0,
                Text = "!"
            },
            // Hello
            // ! World

            new TextDocumentContentChangeEvent()
            {
                Range = new Range{
                    Start = new Position(0, 1),
                    End = new Position(0, 1)
                },
                RangeLength = 4,
                Text = """
                    i!

                    """
            },
            // Hi!
            //
            // ! World
        };

        // Act
        var result = endpoint.ApplyContentChanges(changes, sourceText, Logger);

        // Assert
        var resultString = GetString(result);
        Assert.Equal(@"Hi!

! World", resultString);
    }

    // This is more of an integration test to validate that all the pieces work together
    [Fact]
    public async Task Handle_DidChangeTextDocument_UpdatesDocument()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var sourceText = "<p>";
        var codeDocument = CreateCodeDocument(sourceText);
        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var projectService = new Mock<RazorProjectService>(MockBehavior.Strict);
        projectService.Setup(service => service.UpdateDocument(It.IsAny<string>(), It.IsAny<SourceText>(), It.IsAny<int>()))
            .Callback<string, SourceText, int>((path, text, version) =>
            {
                var resultString = GetString(text);
                Assert.Equal("<p></p>", resultString);
                Assert.Equal(documentPath.OriginalString, path);
                Assert.Equal(1337, version);
            });
        var endpoint = new DocumentDidChangeEndpoint(Dispatcher, projectService.Object);
        var change = new TextDocumentContentChangeEvent()
        {
            Range = new Range
            {
                Start = new Position(0, 3),
                End = new Position(0, 3),
            },
            RangeLength = 0,
            Text = "</p>"
        };
        var request = new DidChangeTextDocumentParams()
        {
            ContentChanges = new TextDocumentContentChangeEvent[] { change },
            TextDocument = new VersionedTextDocumentIdentifier()
            {
                Uri = documentPath,
                Version = 1337,
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        await endpoint.HandleNotificationAsync(request, requestContext, default);

        // Assert
        projectService.VerifyAll();
    }
}
