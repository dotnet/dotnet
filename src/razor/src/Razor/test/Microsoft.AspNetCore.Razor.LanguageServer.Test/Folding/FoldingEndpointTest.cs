// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Folding;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.FindAllReferences;

public class FoldingEndpointTest : SingleServerDelegatingEndpointTestBase
{
    public FoldingEndpointTest(ITestOutputHelper testOutput) : base(testOutput)
    {
    }

    [Fact]
    public Task FoldRazorUsings()
        => VerifyRazorFoldsAsync("""
                @using System
                @using System.Text

                <p>hello!</p>

                @using System.Buffers
                @using System.Drawing
                @using System.CodeDom

                @code {
                    var helloWorld = "";
                }

                <p>hello!</p>
                """, new List<(int StartLine, int EndLine)> { (0, 1), (5, 7) });

    private async Task VerifyRazorFoldsAsync(string input, List<(int StartLine, int EndLine)> expected)
    {
        var codeDocument = CreateCodeDocument(input);
        var razorFilePath = "C:/path/to/file.razor";

        await CreateLanguageServerAsync(codeDocument, razorFilePath);

        var endpoint = new FoldingRangeEndpoint(
            DocumentMappingService, LanguageServer, new List<IRazorFoldingRangeProvider> { new UsingsFoldingRangeProvider(), new RazorCodeBlockFoldingProvider() }, LoggerFactory);

        var request = new FoldingRangeParams()
        {
            TextDocument = new VSTextDocumentIdentifier
            {
                Uri = new Uri(razorFilePath),
                ProjectContext = new VSProjectContext()
                {
                    Label = "test",
                    Kind = VSProjectKind.CSharp,
                    Id = "test"
                }
            }
        };
        var documentContext = DocumentContextFactory.TryCreateForOpenDocument(request.TextDocument);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected, result.Select(foldingRange => (foldingRange.StartLine, foldingRange.EndLine)));

    }
}
