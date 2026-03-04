// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentSymbols;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentSymbols;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.DocumentSymbols;

public class DocumentSymbolEndpointTest(ITestOutputHelper testOutput) : SingleServerDelegatingEndpointTestBase(testOutput)
{
    [Fact]
    public Task DocumentSymbols_CSharpClassWithMethods()
        => VerifySymbolInformationsAsync(
            """
            {|ExecuteAsync():|}@functions {
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
            
            """);

    [Fact]
    public async Task DocumentSymbols_CSharpClassWithMethods_Hierarchical()
    {
        TestCode input = """
            @functions {
                class {|C:C|}
                {
                    private void {|HandleString:HandleString|}(string s)
                    {
                        s += "Hello";
                    }

                    private void {|M:M|}(int i)
                    {
                        i++;
                    }
            
                    private string {|ObjToString:ObjToString|}(object o)
                    {
                        return o.ToString();
                    }
                }
            }
            
            """;

        var documentSymbols = await GetDocumentSymbolsAsync(input);
        var sourceText = SourceText.From(input.Text);

        // Expect: 1 class C containing HandleString, M, ObjToString methods
        var classC = Assert.Single(documentSymbols);
        Assert.Equal("C", classC.Name);
        Assert.Equal(SymbolKind.Class, classC.Kind);
        Assert.Equal(sourceText.GetRange(Assert.Single(input.NamedSpans["C"])), classC.SelectionRange);
        Assert.NotNull(classC.Children);
        Assert.Equal(3, classC.Children!.Length);

        var handleString = classC.Children[0];
        Assert.Equal("HandleString(string) : void", handleString.Name);
        Assert.Equal(SymbolKind.Method, handleString.Kind);
        Assert.Equal(sourceText.GetRange(Assert.Single(input.NamedSpans["HandleString"])), handleString.SelectionRange);

        var m = classC.Children[1];
        Assert.Equal("M(int) : void", m.Name);
        Assert.Equal(SymbolKind.Method, m.Kind);
        Assert.Equal(sourceText.GetRange(Assert.Single(input.NamedSpans["M"])), m.SelectionRange);

        var objToString = classC.Children[2];
        Assert.Equal("ObjToString(object) : string", objToString.Name);
        Assert.Equal(SymbolKind.Method, objToString.Kind);
        Assert.Equal(sourceText.GetRange(Assert.Single(input.NamedSpans["ObjToString"])), objToString.SelectionRange);
    }

    [Fact]
    public Task DocumentSymbols_CSharpMethods()
        => VerifySymbolInformationsAsync(
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
            
            """);

    [Fact]
    public async Task DocumentSymbols_CSharpMethods_Hierarchical()
    {
        TestCode input = """
            @functions {
                private void {|HandleString:HandleString|}(string s)
                {
                    s += "Hello";
                }

                private void {|M:M|}(int i)
                {
                    i++;
                }

                private string {|ObjToString:ObjToString|}(object o)
                {
                    return o.ToString();
                }
            }
            
            """;

        var documentSymbols = await GetDocumentSymbolsAsync(input);
        var sourceText = SourceText.From(input.Text);

        // Expect: HandleString, M, ObjToString methods at top level
        Assert.Equal(3, documentSymbols.Length);

        var handleString = documentSymbols[0];
        Assert.Equal("HandleString(string) : void", handleString.Name);
        Assert.Equal(SymbolKind.Method, handleString.Kind);
        Assert.Equal(sourceText.GetRange(Assert.Single(input.NamedSpans["HandleString"])), handleString.SelectionRange);

        var m = documentSymbols[1];
        Assert.Equal("M(int) : void", m.Name);
        Assert.Equal(SymbolKind.Method, m.Kind);
        Assert.Equal(sourceText.GetRange(Assert.Single(input.NamedSpans["M"])), m.SelectionRange);

        var objToString = documentSymbols[2];
        Assert.Equal("ObjToString(object) : string", objToString.Name);
        Assert.Equal(SymbolKind.Method, objToString.Kind);
        Assert.Equal(sourceText.GetRange(Assert.Single(input.NamedSpans["ObjToString"])), objToString.SelectionRange);
    }

    private async Task VerifySymbolInformationsAsync(string input)
    {
        TestFileMarkupParser.GetSpans(input, out input, out ImmutableDictionary<string, ImmutableArray<TextSpan>> spansDict);
        var codeDocument = CreateCodeDocument(input);
        var razorFilePath = "C:/path/to/file.razor";

        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath,
            capabilitiesUpdater: c => c.TextDocument!.DocumentSymbol = new DocumentSymbolSetting() { HierarchicalDocumentSymbolSupport = false });

        var documentSymbolService = new DocumentSymbolService(DocumentMappingService);
        var endpoint = new DocumentSymbolEndpoint(languageServer, documentSymbolService);

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

    private async Task<DocumentSymbol[]> GetDocumentSymbolsAsync(TestCode input)
    {
        var codeDocument = CreateCodeDocument(input.Text);
        var razorFilePath = "C:/path/to/file.razor";

        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath,
            capabilitiesUpdater: c => c.TextDocument!.DocumentSymbol = new DocumentSymbolSetting() { HierarchicalDocumentSymbolSupport = true });

        var documentSymbolService = new DocumentSymbolService(DocumentMappingService);
        var endpoint = new DocumentSymbolEndpoint(languageServer, documentSymbolService);

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

        Assert.True(result.Value.TryGetFirst(out var documentSymbols));
        return documentSymbols;
    }
}
