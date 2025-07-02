// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentSynchronization;

public class DocumentDidChangeEndpointTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    private readonly IRazorProjectService _projectService = Mock.Of<IRazorProjectService>(MockBehavior.Strict);

    [Fact]
    public void ApplyContentChanges_SingleChange()
    {
        // Arrange
        var endpoint = new DocumentDidChangeEndpoint(_projectService, LoggerFactory);
        var sourceText = SourceText.From("Hello World");
        var change = new TextDocumentContentChangeEvent()
        {
            Range = LspFactory.CreateZeroWidthRange(0, 5),
            RangeLength = 0,
            Text = "!"
        };

        // Act
        var result = endpoint.ApplyContentChanges([change], sourceText);

        // Assert
        Assert.Equal("Hello! World", result.ToString());
    }

    [Fact]
    public void ApplyContentChanges_MultipleChanges()
    {
        // Arrange
        var endpoint = new DocumentDidChangeEndpoint(_projectService, LoggerFactory);
        var sourceText = SourceText.From("Hello World");
        var changes = new[] {
            new TextDocumentContentChangeEvent()
            {
                Range = LspFactory.CreateZeroWidthRange(0, 5),
                RangeLength = 0,
                Text = Environment.NewLine
            },
            // Hello
            //  World

            new TextDocumentContentChangeEvent()
            {
                Range = LspFactory.CreateZeroWidthRange(1, 0),
                RangeLength = 0,
                Text = "!"
            },
            // Hello
            // ! World

            new TextDocumentContentChangeEvent()
            {
                Range = LspFactory.CreateZeroWidthRange(0, 1),
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
        var result = endpoint.ApplyContentChanges(changes, sourceText);

        // Assert
        Assert.Equal(@"Hi!

! World", result.ToString());
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
        var projectService = new StrictMock<IRazorProjectService>();
        projectService
            .Setup(service => service.UpdateDocumentAsync(It.IsAny<string>(), It.IsAny<SourceText>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Callback((string path, SourceText text, CancellationToken cancellationToken) =>
            {
                Assert.Equal("<p></p>", text.ToString());
                Assert.Equal(documentPath.OriginalString, path);
            });
        var endpoint = new DocumentDidChangeEndpoint(projectService.Object, LoggerFactory);
        var change = new TextDocumentContentChangeEvent()
        {
            Range = LspFactory.CreateZeroWidthRange(0, 3),
            RangeLength = 0,
            Text = "</p>"
        };
        var request = new DidChangeTextDocumentParams()
        {
            ContentChanges = [change],
            TextDocument = new VersionedTextDocumentIdentifier()
            {
                DocumentUri = new(documentPath),
                Version = 1337,
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        await endpoint.HandleNotificationAsync(request, requestContext, DisposalToken);

        // Assert
        projectService.VerifyAll();
    }
}
