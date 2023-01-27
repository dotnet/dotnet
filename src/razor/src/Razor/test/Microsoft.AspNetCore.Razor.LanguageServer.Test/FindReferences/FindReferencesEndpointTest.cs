// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.FindAllReferences;

public class FindReferencesEndpointTest : SingleServerDelegatingEndpointTestBase
{
    public FindReferencesEndpointTest(ITestOutputHelper testOutput) : base(testOutput)
    {
    }

    [Fact]
    public Task FindCSharpReferences()
        => VerifyCSharpFindAllReferencesAsyncAsync("""

                @{
                    const string [|$$S|] = "";

                    string M()
                    {
                        return [|S|];
                    }

                    string N()
                    {
                        return [|S|];
                    }
                }

                <p>@[|S|]</p>
                """);

    private async Task VerifyCSharpFindAllReferencesAsyncAsync(string input)
    {
        // Arrange
        TestFileMarkupParser.GetPositionAndSpans(input, out var output, out int cursorPosition, out ImmutableArray<TextSpan> expectedSpans);

        var codeDocument = CreateCodeDocument(output);
        var razorFilePath = "C:/path/to/file.razor";

        await CreateLanguageServerAsync(codeDocument, razorFilePath);

        var endpoint = new FindAllReferencesEndpoint(
            LanguageServerFeatureOptions, DocumentMappingService, LanguageServer, LoggerFactory, LanguageServerFeatureOptions);

        var sourceText = codeDocument.GetSourceText();
        sourceText.GetLineAndOffset(cursorPosition, out var line, out var offset);

        var completedTokenSource = new CancellationTokenSource();
        var progressToken = new ProgressWithCompletion<object>((val) =>
        {
            var results = Assert.IsType<VSInternalReferenceItem[]>(val);
            completedTokenSource.CancelAfter(0);
        });

        var request = new ReferenceParamsBridge
        {
            Context = new ReferenceContext()
            {
                IncludeDeclaration = true
            },
            PartialResultToken = progressToken,
            TextDocument = new TextDocumentIdentifier
            {
                Uri = new Uri(razorFilePath)
            },
            Position = new Position(line, offset)
        };
        var documentContext = await DocumentContextFactory.TryCreateForOpenDocumentAsync(request.TextDocument.Uri, DisposalToken);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(expectedSpans.Length, result.Length);

        var i = 0;
        foreach (var referenceItem in result.OrderBy(l => l.Location.Range.Start.Line))
        {
            Assert.Equal(new Uri(razorFilePath), referenceItem.Location.Uri);

            var expectedRange = expectedSpans[i].AsRange(codeDocument.GetSourceText());
            Assert.Equal(expectedRange, referenceItem.Location.Range);

            i++;
        }
    }
}
