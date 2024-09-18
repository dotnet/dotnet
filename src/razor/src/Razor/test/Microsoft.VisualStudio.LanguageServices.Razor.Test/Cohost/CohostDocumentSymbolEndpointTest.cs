// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class CohostDocumentSymbolEndpointTest(ITestOutputHelper testOutput) : CohostEndpointTestBase(testOutput)
{
    [Theory]
    [CombinatorialData]
    public Task DocumentSymbols_CSharpClassWithMethods(bool hierarchical)
        => VerifyDocumentSymbolsAsync(
            """
            @functions {
                class {|SomeProject.File1.C:C|}
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

    private async Task VerifyDocumentSymbolsAsync(string input, bool hierarchical = false)
    {
        TestFileMarkupParser.GetSpans(input, out input, out ImmutableDictionary<string, ImmutableArray<TextSpan>> spansDict);
        var document = await CreateProjectAndRazorDocumentAsync(input);

        var endpoint = new CohostDocumentSymbolEndpoint(RemoteServiceInvoker);

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(document, hierarchical, DisposalToken);

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

            var sourceText = SourceText.From(input);
            foreach (var symbolInformation in symbolsInformations)
            {
                Assert.True(spansDict.TryGetValue(symbolInformation.Name, out var spans), $"Expected {symbolInformation.Name} to be in test provided markers");
                var expectedRange = sourceText.GetRange(Assert.Single(spans));
                Assert.Equal(expectedRange, symbolInformation.Location.Range);
            }
        }
    }

    private static void VerifyDocumentSymbols(ImmutableDictionary<string, ImmutableArray<TextSpan>> spansDict, DocumentSymbol[] documentSymbols, SourceText sourceText, ref int seen)
    {
        foreach (var symbol in documentSymbols)
        {
            seen++;
            Assert.True(spansDict.TryGetValue(symbol.Detail.AssumeNotNull(), out var spans), $"Expected {symbol.Detail} to be in test provided markers");
            var expectedRange = sourceText.GetRange(Assert.Single(spans));
            Assert.Equal(expectedRange, symbol.SelectionRange);

            if (symbol.Children is not null)
            {
                VerifyDocumentSymbols(spansDict, symbol.Children, sourceText, ref seen);
            }
        }
    }
}
