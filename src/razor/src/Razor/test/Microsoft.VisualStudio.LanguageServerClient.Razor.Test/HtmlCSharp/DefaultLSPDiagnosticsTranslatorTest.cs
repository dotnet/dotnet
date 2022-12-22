// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Test;
using Microsoft.VisualStudio.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

public class DefaultLSPDiagnosticsTranslatorTest : HandlerTestBase
{
    public DefaultLSPDiagnosticsTranslatorTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task ProcessDiagnosticsAsync_ReturnsResponse()
    {
        // Arrange
        var response = new RazorDiagnosticsResponse()
        {
            HostDocumentVersion = 5
        };

        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(ri => ri.ReinvokeRequestOnServerAsync<RazorDiagnosticsParams, RazorDiagnosticsResponse>(
                It.IsAny<ITextBuffer>(),
                LanguageServerConstants.RazorTranslateDiagnosticsEndpoint,
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<RazorDiagnosticsParams>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReinvocationResponse<RazorDiagnosticsResponse>("TestLanguageClient", response));
        var documentUri = new Uri("file://C:/path/to/file.razor");
        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(documentUri, new TestLSPDocumentSnapshot(documentUri, version: 0, "The content"));
        var diagnosticsProvider = new DefaultLSPDiagnosticsTranslator(documentManager, requestInvoker.Object, LoggerProvider);

        // Act
        var diagnosticsResponse = await diagnosticsProvider.TranslateAsync(
            RazorLanguageKind.CSharp,
            documentUri,
            Array.Empty<Diagnostic>(),
            DisposalToken);

        // Assert
        Assert.Equal(5, diagnosticsResponse.HostDocumentVersion);
    }
}
