// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public class RazorCompletionEndpointTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task Handle_NoDocumentContext_NoCompletionItems()
    {
        // Arrange
        var documentPath = "C:/path/to/document.cshtml";
        var optionsMonitor = GetOptionsMonitor();
        var completionEndpoint = new RazorCompletionEndpoint(completionListProvider: null, triggerAndCommitCharacters: null, NoOpTelemetryReporter.Instance, optionsMonitor);
        var request = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier()
            {
                DocumentUri = new(new Uri(documentPath))
            },
            Position = LspFactory.CreatePosition(0, 1),
            Context = new VSInternalCompletionContext(),
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var completionList = await Task.Run(() => completionEndpoint.HandleRequestAsync(request, requestContext, default));

        // Assert
        Assert.Null(completionList);
    }

    [Fact]
    public async Task Handle_AutoShowCompletionDisabled_NoCompletionItems()
    {
        // Arrange
        var codeDocument = CreateCodeDocument();
        var documentPath = "C:/path/to/document.cshtml";
        var uri = new Uri(documentPath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var optionsMonitor = GetOptionsMonitor(autoShowCompletion: false);
        var completionEndpoint = new RazorCompletionEndpoint(completionListProvider: null, triggerAndCommitCharacters: null, NoOpTelemetryReporter.Instance, optionsMonitor);
        var request = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier()
            {
                DocumentUri = new(uri)
            },
            Position = LspFactory.CreatePosition(0, 1),
            Context = new VSInternalCompletionContext() { InvokeKind = VSInternalCompletionInvokeKind.Typing },
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var completionList = await Task.Run(() => completionEndpoint.HandleRequestAsync(request, requestContext, default));

        // Assert
        Assert.Null(completionList);
    }

    private static RazorCodeDocument CreateCodeDocument()
    {
        return CreateCodeDocument("""

            @{ }
            """);
    }
}
