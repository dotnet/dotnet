// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.SignatureHelp;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.SignatureHelp;

public class SignatureHelpEndpointTest : SingleServerDelegatingEndpointTestBase
{
    public SignatureHelpEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task Handle_SingleServer_CSharpSignature()
    {
        var input = """
                <div></div>

                @{
                    string M1(int i) => throw new NotImplementedException();

                    void Act()
                    {
                        M1($$);
                    }
                }
                """;

        await VerifySignatureHelpAsync(input, "string M1(int i)");
    }

    [Fact]
    public async Task Handle_SingleServer_CSharpSignature_Razor()
    {
        var input = """
                <div>@GetDiv($$)</div>

                @{
                    string GetDiv() => "";
                }
                """;

        await VerifySignatureHelpAsync(input, "string GetDiv()");
    }

    [Fact]
    public async Task Handle_SingleServer_ReturnNull()
    {
        var input = """
                <div>@GetDiv($$)</div>

                @{
                }
                """;

        await VerifySignatureHelpAsync(input);
    }

    private async Task VerifySignatureHelpAsync(string input, params string[] signatures)
    {
        // Arrange
        TestFileMarkupParser.GetPositionAndSpans(input, out var output, out int cursorPosition, out ImmutableArray<TextSpan> _);
        var codeDocument = CreateCodeDocument(output);
        var razorFilePath = "C:/path/to/file.razor";

        await CreateLanguageServerAsync(codeDocument, razorFilePath);

        var endpoint = new SignatureHelpEndpoint(
            LanguageServerFeatureOptions, DocumentMappingService, LanguageServer, LoggerFactory);

        codeDocument.GetSourceText().GetLineAndOffset(cursorPosition, out var line, out var offset);
        var request = new SignatureHelpParamsBridge
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = new Uri(razorFilePath)
            },
            Position = new Position(line, offset)
        };

        var documentContext = await DocumentContextFactory.TryCreateAsync(request.TextDocument.Uri, DisposalToken);

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        if (signatures.Length == 0)
        {
            Assert.Null(result);
            return;
        }

        Assert.Equal(signatures.Length, result.Signatures.Length);
        for (var i = 0; i < signatures.Length; i++)
        {
            var expected = signatures[i];
            var actual = result.Signatures[i];

            Assert.Equal(expected, actual.Label);
        }
    }
}
