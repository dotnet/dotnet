// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Remote.Razor;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class CohostGoToImplementationEndpointTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public async Task CSharp_Method()
    {
        var input = """
            <div></div>

            @{
                var x = Ge$$tX();
            }

            @code
            {
                void [|GetX|]()
                {
                }
            }
            """;

        await VerifyCSharpGoToImplementationAsync(input);
    }

    [Fact]
    public async Task CSharp_Field()
    {
        var input = """
            <div></div>

            @{
                var x = GetX();
            }

            @code
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
    public async Task CSharp_Multiple()
    {
        var input = """
            <div></div>

            @code
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

    [Fact]
    public async Task Html()
    {
        // This really just validates Uri remapping, the actual response is largely arbitrary

        TestCode input = """
            <div></div>

            <script>
                function [|foo|]() {
                    f$$oo();
                }
            </script>
            """;

        var document = CreateProjectAndRazorDocument(input.Text);
        var inputText = await document.GetTextAsync(DisposalToken);

        var htmlResponse = new SumType<LspLocation[], VSInternalReferenceItem[]>?(new LspLocation[]
        {
            new LspLocation
            {
                DocumentUri = new(new Uri(document.CreateUri(), document.Name + FeatureOptions.HtmlVirtualDocumentSuffix)),
                Range = inputText.GetRange(input.Span),
            },
        });

        var requestInvoker = new TestHtmlRequestInvoker([(Methods.TextDocumentImplementationName, htmlResponse)]);

        await VerifyGoToImplementationResultAsync(input, document, requestInvoker);
    }

    private async Task VerifyCSharpGoToImplementationAsync(TestCode input)
    {
        var document = CreateProjectAndRazorDocument(input.Text);

        var requestInvoker = new TestHtmlRequestInvoker();

        await VerifyGoToImplementationResultCoreAsync(input, document, requestInvoker);
    }

    private async Task VerifyGoToImplementationResultAsync(TestCode input, TextDocument document, IHtmlRequestInvoker requestInvoker)
    {
        await VerifyGoToImplementationResultCoreAsync(input, document, requestInvoker);
    }

    private async Task VerifyGoToImplementationResultCoreAsync(TestCode input, TextDocument document, IHtmlRequestInvoker requestInvoker)
    {
        var inputText = await document.GetTextAsync(DisposalToken);

        var filePathService = new RemoteFilePathService(FeatureOptions);
        var endpoint = new CohostGoToImplementationEndpoint(IncompatibleProjectService, RemoteServiceInvoker, requestInvoker, filePathService);

        var position = inputText.GetPosition(input.Position);
        var textDocumentPositionParams = new TextDocumentPositionParams
        {
            Position = position,
            TextDocument = new TextDocumentIdentifier { DocumentUri = document.CreateDocumentUri() },
        };

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(textDocumentPositionParams, document, DisposalToken);

        if (result.Value.TryGetFirst(out var roslynLocations))
        {
            var expected = input.Spans.Select(s => inputText.GetRange(s).ToLinePositionSpan()).OrderBy(r => r.Start.Line).ToArray();
            var actual = roslynLocations.Select(l => l.Range.ToLinePositionSpan()).OrderBy(r => r.Start.Line).ToArray();
            Assert.Equal(expected, actual);

            Assert.All(roslynLocations, l => l.DocumentUri.GetRequiredParsedUri().Equals(document.CreateUri()));
        }
        else
        {
            Assert.Fail($"Unsupported result type: {result.Value.GetType()}");
        }
    }
}
