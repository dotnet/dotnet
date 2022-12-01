// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentSynchronization;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class RazorDidCloseTextDocumentEndpointTest : LanguageServerTestBase
{
    public RazorDidCloseTextDocumentEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    // This is more of an integration test to validate that all the pieces work together
    [Fact]
    public async Task Handle_DidCloseTextDocument_ClosesDocument()
    {
        // Arrange
        var documentPath = "C:/path/to/document.cshtml";
        var projectService = new Mock<RazorProjectService>(MockBehavior.Strict);
        projectService.Setup(service => service.CloseDocument(It.IsAny<string>()))
            .Callback<string>((path) => Assert.Equal(documentPath, path));
        var endpoint = new RazorDidCloseTextDocumentEndpoint(Dispatcher, projectService.Object);
        var request = new DidCloseTextDocumentParams()
        {
            TextDocument = new TextDocumentIdentifier()
            {
                Uri = new Uri(documentPath)
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        await endpoint.HandleNotificationAsync(request, requestContext, default);

        // Assert
        projectService.VerifyAll();
    }
}
