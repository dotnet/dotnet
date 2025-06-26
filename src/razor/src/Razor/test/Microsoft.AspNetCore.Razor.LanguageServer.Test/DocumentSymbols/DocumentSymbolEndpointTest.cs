// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentSymbols;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentSymbols;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.DocumentSymbols;

public class DocumentSymbolEndpointTest(ITestOutputHelper testOutput) : SingleServerDelegatingEndpointTestBase(testOutput)
{
    [Theory]
    [CombinatorialData]
    public Task DocumentSymbols_CSharpClassWithMethods(bool hierarchical)
        => VerifyDocumentSymbolsAsync(
            """
            @functions {
                class {|AspNetCoreGeneratedDocument.test.C:C|}
                {
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
            }
            
            """, hierarchical);

    [Theory]
    [CombinatorialData]
    public Task DocumentSymbols_CSharpMethods(bool hierarchical)
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
            
            """, hierarchical);

    [Fact]
    public async Task DocumentSymbols_DisabledWhenNotSingleServer()
    {
        var input = """
            <p> Hello World </p>
            """;

        var codeDocument = CreateCodeDocument(input);
        var razorFilePath = "C:/path/to/file.razor";

        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);

        // This test requires the SingleServerSupport to be disabled
        Assert.False(TestLanguageServerFeatureOptions.Instance.SingleServerSupport);
        var documentSymbolService = new DocumentSymbolService(DocumentMappingService);
        var endpoint = new DocumentSymbolEndpoint(languageServer, documentSymbolService, TestLanguageServerFeatureOptions.Instance);

        var serverCapabilities = new VSInternalServerCapabilities();
        var clientCapabilities = new VSInternalClientCapabilities();

        endpoint.ApplyCapabilities(serverCapabilities, clientCapabilities);

        Assert.Null(serverCapabilities.DocumentSymbolProvider?.Value);
    }

    private async Task VerifyDocumentSymbolsAsync(string input, bool hierarchical = false)
    {
        TestFileMarkupParser.GetSpans(input, out input, out ImmutableDictionary<string, ImmutableArray<TextSpan>> spansDict);
        var codeDocument = CreateCodeDocument(input);
        var razorFilePath = "C:/path/to/file.razor";

        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath,
            capabilitiesUpdater: c => c.TextDocument!.DocumentSymbol = new DocumentSymbolSetting() { HierarchicalDocumentSymbolSupport = hierarchical });

        var documentSymbolService = new DocumentSymbolService(DocumentMappingService);
        var endpoint = new DocumentSymbolEndpoint(languageServer, documentSymbolService, TestLanguageServerFeatureOptions.Instance);

        var request = new DocumentSymbolParams()
        {
            TextDocument = new VSTextDocumentIdentifier
            {
                DocumentUri = new(new Uri(razorFilePath)),
                ProjectContext = new VSProjectContext()
                {
                    Label = "test",
                    Kind = VSProjectKind.CSharp,
                    Id = "test"
                }
            }
        };
        Assert.True(DocumentContextFactory.TryCreate(request.TextDocument, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);
        Assert.NotNull(result);

        if (hierarchical)
        {
            var documentSymbols = result.Value.First;
            var sourceText = SourceText.From(input);
            var seen = 0;

            VerifyDocumentSymbols(spansDict, documentSymbols, sourceText, ref seen);

            Assert.Equal(spansDict.Values.Count(), seen);
        }
        else
        {
            var symbolsInformations = result.Value.Second;
            Assert.Equal(spansDict.Values.Count(), symbolsInformations.Length);

#pragma warning disable CS0618 // Type or member is obsolete
            // SymbolInformation is obsolete, but things still return it so we have to handle it
            var sourceText = SourceText.From(input);
            foreach (var symbolInformation in symbolsInformations)
            {
                Assert.True(spansDict.TryGetValue(symbolInformation.Name, out var spans), $"Expected {symbolInformation.Name} to be in test provided markers");
                var expectedRange = sourceText.GetRange(Assert.Single(spans));
                Assert.Equal(expectedRange, symbolInformation.Location.Range);
            }
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }

    private static void VerifyDocumentSymbols(ImmutableDictionary<string, ImmutableArray<TextSpan>> spansDict, DocumentSymbol[] documentSymbols, SourceText sourceText, ref int seen)
    {
        foreach (var symbol in documentSymbols)
        {
            seen++;
            Assert.True(spansDict.TryGetValue(symbol.Detail.AssumeNotNull(), out var spans), $"Expected {symbol.Name} to be in test provided markers");
            var expectedRange = sourceText.GetRange(Assert.Single(spans));
            Assert.Equal(expectedRange, symbol.SelectionRange);

            if (symbol.Children is not null)
            {
                VerifyDocumentSymbols(spansDict, symbol.Children, sourceText, ref seen);
            }
        }
    }
}
