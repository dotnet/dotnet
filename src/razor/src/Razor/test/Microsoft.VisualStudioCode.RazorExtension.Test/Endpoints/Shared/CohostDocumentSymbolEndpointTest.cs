// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;
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
            {|BuildRenderTree():|}@code {
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
            
            """,
            hierarchical);

    [Theory]
    [CombinatorialData]
    public Task DocumentSymbols_CSharpClassWithMethods_MiscFile(bool hierarchical)
    {
        // What the source generator would product for TestProjectData.SomeProjectPath
        var generatedNamespace = PlatformInformation.IsWindows
            ? "c_.users.example.src.SomeProject"
            : "home.example.SomeProject";
        return VerifyDocumentSymbolsAsync(
            $$"""
            {|BuildRenderTree():|}@code {
                class {|ASP.{{generatedNamespace}}.File1.C:C|}
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
            
            """,
            hierarchical,
            miscellaneousFile: true);
    }

    [Theory]
    [CombinatorialData]
    public Task DocumentSymbols_CSharpMethods(bool hierarchical)
        => VerifyDocumentSymbolsAsync(
            """
            {|BuildRenderTree():|}@code {
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
            
            """,
            hierarchical);

    [Theory]
    [CombinatorialData]
    public Task DocumentSymbols_CSharpMethods_Legacy(bool hierarchical)
        => VerifyDocumentSymbolsAsync(
            """
                {|ExecuteAsync():|}@functions {
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
            
                """,
            hierarchical,
            fileKind: RazorFileKind.Legacy);

    private async Task VerifyDocumentSymbolsAsync(TestCode input, bool hierarchical = false, bool miscellaneousFile = false, RazorFileKind? fileKind = null)
    {
        fileKind ??= RazorFileKind.Component;

        var document = CreateProjectAndRazorDocument(input.Text, fileKind, miscellaneousFile: miscellaneousFile);

        var endpoint = new CohostDocumentSymbolEndpoint(IncompatibleProjectService, RemoteServiceInvoker);

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(document, hierarchical, DisposalToken);

        // Roslyn's DocumentSymbol type has an annoying property that makes it hard to serialize
        Assert.NotNull(JsonSerializer.SerializeToDocument(result, JsonHelpers.JsonSerializerOptions));

        var spansDict = input.NamedSpans;
        var sourceText = SourceText.From(input.Text);

        if (hierarchical)
        {
            var documentSymbols = result.Value.First;
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
            Assert.True(spansDict.TryGetValue(symbol.Detail ?? symbol.Name, out var spans), $"Expected {symbol.Detail} to be in test provided markers");
            var expectedRange = sourceText.GetRange(Assert.Single(spans));
            Assert.Equal(expectedRange, symbol.SelectionRange);

            if (symbol.Children is not null)
            {
                VerifyDocumentSymbols(spansDict, symbol.Children, sourceText, ref seen);
            }
        }
    }
}
