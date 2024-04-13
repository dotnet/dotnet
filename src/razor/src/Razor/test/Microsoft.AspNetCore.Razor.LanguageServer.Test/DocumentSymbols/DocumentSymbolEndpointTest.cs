// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentSymbol;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.DocumentSymbols;

public class DocumentSymbolEndpointTest(ITestOutputHelper testOutput) : SingleServerDelegatingEndpointTestBase(testOutput)
{
    [Fact]
    public Task DocumentSymbols_CSharpMethods()
        => VerifyDocumentSymbolsAsync(
            """
            @functions {
                private void {|HandleString(string s):HandleString|}(string s)
                {
                    s += "Hello";
                }

                private void {|M(int i):M|}(int i)
                {
                    i++;
                }

                private string {|ObjToString(object o):ObjToString|}(object o)
                {
                    return o.ToString();
                }
            }
            
            """);

    [Fact]
    public async Task DocumentSymbols_DisabledWhenNotSingleServer()
    {
        var input = """
            <p> Hello World </p>
            """;

        var codeDocument = CreateCodeDocument(input);
        var razorFilePath = "C:/path/to/file.razor";

        var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);

        // This test requires the SingleServerSupport to be disabled
        Assert.False(TestLanguageServerFeatureOptions.Instance.SingleServerSupport);
        var endpoint = new DocumentSymbolEndpoint(languageServer, DocumentMappingService, TestLanguageServerFeatureOptions.Instance);

        var serverCapabilities = new VSInternalServerCapabilities();
        var clientCapabilities = new VSInternalClientCapabilities();

        endpoint.ApplyCapabilities(serverCapabilities, clientCapabilities);

        Assert.Null(serverCapabilities.DocumentSymbolProvider?.Value);
    }

    private async Task VerifyDocumentSymbolsAsync(string input)
    {
        TestFileMarkupParser.GetSpans(input, out input, out ImmutableDictionary<string, ImmutableArray<TextSpan>>  spansDict);
        var codeDocument = CreateCodeDocument(input);
        var razorFilePath = "C:/path/to/file.razor";

        var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);

        var endpoint = new DocumentSymbolEndpoint(languageServer, DocumentMappingService, TestLanguageServerFeatureOptions.Instance);

        var request = new DocumentSymbolParams()
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
        var documentContext = await DocumentContextFactory.TryCreateForOpenDocumentAsync(request.TextDocument, DisposalToken);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var symbolsInformations = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(symbolsInformations);
        Assert.Equal(spansDict.Values.Count(), symbolsInformations.Length);

        var sourceText = SourceText.From(input);
        foreach (var symbolInformation in symbolsInformations)
        {
            Assert.True(spansDict.TryGetValue(symbolInformation.Name, out var spans), $"Expected {symbolInformation.Name} to be in test provided markers");
            Assert.Single(spans);
            var expectedRange = spans.Single().ToRange(sourceText);
            Assert.Equal(expectedRange, symbolInformation.Location.Range);
        }
    }
}
