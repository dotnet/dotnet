// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Implementation;

public class ImplementationEndpointTest(ITestOutputHelper testOutput) : SingleServerDelegatingEndpointTestBase(testOutput)
{
    [Fact]
    public async Task Handle_SingleServer_CSharp_Method()
    {
        var input = """
                <div></div>

                @{
                    var x = Ge$$tX();
                }

                @functions
                {
                    void [|GetX|]()
                    {
                    }
                }
                """;

        await VerifyCSharpGoToImplementationAsync(input);
    }

    [Fact]
    public async Task Handle_SingleServer_CSharp_Local()
    {
        var input = """
                <div></div>

                @{
                    var x = GetX();
                }

                @functions
                {
                    private string [|_name|];

                    string GetX()
                    {
                        return _na$$me;
                    }
                }
                """;

        await VerifyCSharpGoToImplementationAsync(input);
    }

    [Fact]
    public async Task Handle_SingleServer_CSharp_MultipleResults()
    {
        var input = """
                <div></div>

                @functions
                {
                    class Base { }
                    class [|Derived1|] : Base { }
                    class [|Derived2|] : Base { }

                    void M(Ba$$se b)
                    {
                    }
                }
                """;

        await VerifyCSharpGoToImplementationAsync(input);
    }

    private async Task VerifyCSharpGoToImplementationAsync(string input)
    {
        // Arrange
        TestFileMarkupParser.GetPositionAndSpans(input, out var output, out int cursorPosition, out ImmutableArray<TextSpan> expectedSpans);

        var codeDocument = CreateCodeDocument(output);
        var razorFilePath = "C:/path/to/file.razor";

        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);

        var endpoint = new ImplementationEndpoint(
            LanguageServerFeatureOptions, DocumentMappingService, languageServer, LoggerFactory);

        var request = new TextDocumentPositionParams
        {
            TextDocument = new TextDocumentIdentifier
            {
                DocumentUri = new(new Uri(razorFilePath))
            },
            Position = codeDocument.Source.Text.GetPosition(cursorPosition)
        };
        Assert.True(DocumentContextFactory.TryCreate(request.TextDocument, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result.First);
        var locations = result.First;

        Assert.Equal(expectedSpans.Length, locations.Length);

        var i = 0;
        foreach (var location in locations.OrderBy(l => l.Range.Start.Line))
        {
            Assert.Equal(new Uri(razorFilePath), location.DocumentUri.GetRequiredParsedUri());

            var expectedRange = codeDocument.Source.Text.GetRange(expectedSpans[i]);
            Assert.Equal(expectedRange, location.Range);

            i++;
        }
    }
}
