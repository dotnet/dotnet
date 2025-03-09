// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Remote.Razor;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;
using LspLocation = Microsoft.VisualStudio.LanguageServer.Protocol.Location;
using RoslynLspExtensions = Roslyn.LanguageServer.Protocol.RoslynLspExtensions;

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
                Uri = new Uri(document.CreateUri(), document.Name + FeatureOptions.HtmlVirtualDocumentSuffix),
                Range = inputText.GetRange(input.Span),
            },
        });

        var requestInvoker = new TestLSPRequestInvoker([(Methods.TextDocumentImplementationName, htmlResponse)]);

        await VerifyGoToImplementationResultAsync(input, document, requestInvoker);
    }

    private async Task VerifyCSharpGoToImplementationAsync(TestCode input)
    {
        var document = CreateProjectAndRazorDocument(input.Text);

        var requestInvoker = new TestLSPRequestInvoker();

        await VerifyGoToImplementationResultCoreAsync(input, document, requestInvoker);
    }

    private async Task VerifyGoToImplementationResultAsync(TestCode input, TextDocument document, TestLSPRequestInvoker requestInvoker)
    {
        await VerifyGoToImplementationResultCoreAsync(input, document, requestInvoker);
    }

    private async Task VerifyGoToImplementationResultCoreAsync(TestCode input, TextDocument document, TestLSPRequestInvoker requestInvoker)
    {
        var inputText = await document.GetTextAsync(DisposalToken);

        var filePathService = new RemoteFilePathService(FeatureOptions);
        var endpoint = new CohostGoToImplementationEndpoint(RemoteServiceInvoker, TestHtmlDocumentSynchronizer.Instance, requestInvoker, filePathService);

        var position = inputText.GetPosition(input.Position);
        var textDocumentPositionParams = new TextDocumentPositionParams
        {
            Position = position,
            TextDocument = new TextDocumentIdentifier { Uri = document.CreateUri() },
        };

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(textDocumentPositionParams, document, DisposalToken);

        if (result.Value.TryGetFirst(out var roslynLocations))
        {
            var expected = input.Spans.Select(s => inputText.GetRange(s).ToLinePositionSpan()).OrderBy(r => r.Start.Line).ToArray();
            var actual = roslynLocations.Select(l => RoslynLspExtensions.ToLinePositionSpan(l.Range)).OrderBy(r => r.Start.Line).ToArray();
            Assert.Equal(expected, actual);

            Assert.All(roslynLocations, l => l.Uri.Equals(document.CreateUri()));
        }
        else if (result.Value.TryGetSecond(out var vsLocations))
        {
            var expected = input.Spans.Select(s => inputText.GetRange(s).ToLinePositionSpan()).OrderBy(r => r.Start.Line).ToArray();
            var actual = vsLocations.Select(l => l.Range.ToLinePositionSpan()).OrderBy(r => r.Start.Line).ToArray();
            Assert.Equal(expected, actual);

            Assert.All(vsLocations, l => l.Uri.Equals(document.CreateUri()));
        }
        else
        {
            Assert.Fail($"Unsupported result type: {result.Value.GetType()}");
        }
    }
}
