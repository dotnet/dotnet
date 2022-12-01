// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public class RazorCompletionEndpointTest : LanguageServerTestBase
{
    public RazorCompletionEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task Handle_NoDocumentContext_NoCompletionItems()
    {
        // Arrange
        var documentPath = "C:/path/to/document.cshtml";
        var completionEndpoint = new RazorCompletionEndpoint(completionListProvider: null);
        var request = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier()
            {
                Uri = new Uri(documentPath)
            },
            Position = new Position(0, 1),
            Context = new VSInternalCompletionContext(),
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var completionList = await Task.Run(() => completionEndpoint.HandleRequestAsync(request, requestContext, default));

        // Assert
        Assert.Null(completionList);
    }
}
