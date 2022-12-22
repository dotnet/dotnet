// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.VisualStudio.LanguageServer.ContainedLanguage.DefaultLSPDocumentSynchronizer;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

public class DefaultLSPProjectionProviderTest : HandlerTestBase
{
    private readonly LSPDocumentSnapshot _documentSnapshot;
    private readonly HtmlVirtualDocumentSnapshot _htmlVirtualDocumentSnapshot;
    private readonly CSharpVirtualDocumentSnapshot _csharpVirtualDocumentSnapshot;

    public DefaultLSPProjectionProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var htmlUri = new Uri("file:///some/folder/to/file.razor__virtual.html");
        _htmlVirtualDocumentSnapshot = new HtmlVirtualDocumentSnapshot(htmlUri, new StringTextSnapshot(string.Empty), 1);

        var csharpUri = new Uri("file:///some/folder/to/file.razor__virtual.cs");
        _csharpVirtualDocumentSnapshot = new CSharpVirtualDocumentSnapshot(csharpUri, new StringTextSnapshot(string.Empty), 1);

        var uri = new Uri("file:///some/folder/to/file.razor");
        _documentSnapshot = new TestLSPDocumentSnapshot(uri, version: 0, "Some Content", _htmlVirtualDocumentSnapshot, _csharpVirtualDocumentSnapshot);
    }

    [Fact]
    public async Task GetProjectionAsync_RazorProjection_ReturnsNull()
    {
        // Arrange
        var response = new RazorLanguageQueryResponse()
        {
            Kind = RazorLanguageKind.Razor,
            Position = null
        };

        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<RazorLanguageQueryParams, RazorLanguageQueryResponse>(
                It.IsAny<ITextBuffer>(),
                It.IsAny<string>(),
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<Func<JToken, bool>>(),
                It.IsAny<RazorLanguageQueryParams>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReinvocationResponse<RazorLanguageQueryResponse>("LanguageClient", response));

        var documentSynchronizer = new Mock<LSPDocumentSynchronizer>(MockBehavior.Strict);

        var projectionProvider = new DefaultLSPProjectionProvider(requestInvoker.Object, documentSynchronizer.Object, TestRazorLogger.Instance, LoggerProvider);

        // Act
        var result = await projectionProvider.GetProjectionAsync(_documentSnapshot, new Position(), DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetProjectionAsync_HtmlProjection_Synchronizes_ReturnsProjection()
    {
        // Arrange
        var expectedPosition = new Position(0, 0);
        var response = new RazorLanguageQueryResponse()
        {
            Kind = RazorLanguageKind.Html,
            HostDocumentVersion = 1,
            Position = new Position(expectedPosition.Line, expectedPosition.Character)
        };
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<RazorLanguageQueryParams, RazorLanguageQueryResponse>(
                It.IsAny<ITextBuffer>(),
                It.IsAny<string>(),
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<Func<JToken, bool>>(),
                It.IsAny<RazorLanguageQueryParams>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReinvocationResponse<RazorLanguageQueryResponse>("LanguageClient", response));

        var documentSynchronizer = new Mock<LSPDocumentSynchronizer>(MockBehavior.Strict);
        documentSynchronizer
            .Setup(d => d.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(_documentSnapshot.Version, _documentSnapshot.Uri, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DefaultLSPDocumentSynchronizer.SynchronizedResult<HtmlVirtualDocumentSnapshot>(true, _htmlVirtualDocumentSnapshot));

        var projectionProvider = new DefaultLSPProjectionProvider(requestInvoker.Object, documentSynchronizer.Object, TestRazorLogger.Instance, LoggerProvider);

        // Act
        var result = await projectionProvider.GetProjectionAsync(_documentSnapshot, new Position(), DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_htmlVirtualDocumentSnapshot.Uri, result.Uri);
        Assert.Equal(RazorLanguageKind.Html, result.LanguageKind);
        Assert.Equal(expectedPosition, result.Position);
    }

    [Fact]
    public async Task GetProjectionAsync_CSharpProjection_Synchronizes_ReturnsProjection()
    {
        // Arrange
        var expectedPosition = new Position(0, 0);
        var response = new RazorLanguageQueryResponse()
        {
            Kind = RazorLanguageKind.CSharp,
            HostDocumentVersion = 1,
            Position = new Position(expectedPosition.Line, expectedPosition.Character)
        };
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<RazorLanguageQueryParams, RazorLanguageQueryResponse>(
                It.IsAny<ITextBuffer>(),
                It.IsAny<string>(),
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<Func<JToken, bool>>(),
                It.IsAny<RazorLanguageQueryParams>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReinvocationResponse<RazorLanguageQueryResponse>("LanguageClient", response));

        var documentSynchronizer = new Mock<LSPDocumentSynchronizer>(MockBehavior.Strict);
        documentSynchronizer
            .Setup(d => d.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(_documentSnapshot.Version, _documentSnapshot.Uri, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SynchronizedResult<CSharpVirtualDocumentSnapshot>(true, _csharpVirtualDocumentSnapshot));

        var projectionProvider = new DefaultLSPProjectionProvider(requestInvoker.Object, documentSynchronizer.Object, TestRazorLogger.Instance, LoggerProvider);

        // Act
        var result = await projectionProvider.GetProjectionAsync(_documentSnapshot, new Position(), DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_csharpVirtualDocumentSnapshot.Uri, result.Uri);
        Assert.Equal(RazorLanguageKind.CSharp, result.LanguageKind);
        Assert.Equal(expectedPosition, result.Position);
    }

    [Fact]
    public async Task GetProjectionAsync_UndefinedHostDocumentVersionResponse_ReturnsProjection()
    {
        // Arrange
        var expectedPosition = new Position(0, 0);
        var response = new RazorLanguageQueryResponse()
        {
            Kind = RazorLanguageKind.Html,
            HostDocumentVersion = null,
            Position = new Position(expectedPosition.Line, expectedPosition.Character)
        };
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<RazorLanguageQueryParams, RazorLanguageQueryResponse>(
                It.IsAny<ITextBuffer>(),
                It.IsAny<string>(),
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<Func<JToken, bool>>(),
                It.IsAny<RazorLanguageQueryParams>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReinvocationResponse<RazorLanguageQueryResponse>("LanguageClient", response));

        var documentSynchronizer = new Mock<LSPDocumentSynchronizer>(MockBehavior.Strict);
        documentSynchronizer
            .Setup(d => d.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(_documentSnapshot.Version, _documentSnapshot.Uri, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SynchronizedResult<HtmlVirtualDocumentSnapshot>(true, _htmlVirtualDocumentSnapshot));

        var logger = new Mock<RazorLogger>(MockBehavior.Strict);
        logger.Setup(l => l.LogVerbose(It.IsAny<string>())).Verifiable();
        var projectionProvider = new DefaultLSPProjectionProvider(requestInvoker.Object, documentSynchronizer.Object, logger.Object, LoggerProvider);

        // Act
        var result = await projectionProvider.GetProjectionAsync(_documentSnapshot, new Position(), DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_htmlVirtualDocumentSnapshot.Uri, result.Uri);
        Assert.Equal(RazorLanguageKind.Html, result.LanguageKind);
        Assert.Equal(expectedPosition, result.Position);
    }

    [Fact]
    public async Task GetProjectionForCompletionAsync_CSharpProjection_ReturnsProjection()
    {
        // Arrange
        var expectedPosition = new Position(0, 0);
        var response = new RazorLanguageQueryResponse()
        {
            Kind = RazorLanguageKind.CSharp,
            HostDocumentVersion = 1,
            Position = new Position(expectedPosition.Line, expectedPosition.Character)
        };
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<RazorLanguageQueryParams, RazorLanguageQueryResponse>(
                It.IsAny<ITextBuffer>(),
                It.IsAny<string>(),
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<Func<JToken, bool>>(),
                It.IsAny<RazorLanguageQueryParams>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReinvocationResponse<RazorLanguageQueryResponse>("LanguageClient", response));

        var documentSynchronizer = new Mock<LSPDocumentSynchronizer>(MockBehavior.Strict);
        documentSynchronizer
            .Setup(d => d.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                _documentSnapshot.Version, _documentSnapshot.Uri, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SynchronizedResult<CSharpVirtualDocumentSnapshot>(true, _csharpVirtualDocumentSnapshot));

        var projectionProvider = new DefaultLSPProjectionProvider(requestInvoker.Object, documentSynchronizer.Object, TestRazorLogger.Instance, LoggerProvider);

        // Act
        var result = await projectionProvider.GetProjectionForCompletionAsync(_documentSnapshot, new Position(), DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_csharpVirtualDocumentSnapshot.Uri, result.Uri);
        Assert.Equal(RazorLanguageKind.CSharp, result.LanguageKind);
        Assert.Equal(expectedPosition, result.Position);
    }

    [Fact]
    public async Task GetProjectionAsync_SynchronizationFails_ReturnsNull()
    {
        // Arrange
        var expectedPosition = new Position(0, 0);
        var response = new RazorLanguageQueryResponse()
        {
            Kind = RazorLanguageKind.CSharp,
            HostDocumentVersion = 1,
            Position = new Position(expectedPosition.Line, expectedPosition.Character)
        };
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<RazorLanguageQueryParams, RazorLanguageQueryResponse>(
                It.IsAny<ITextBuffer>(),
                It.IsAny<string>(),
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<Func<JToken, bool>>(),
                It.IsAny<RazorLanguageQueryParams>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReinvocationResponse<RazorLanguageQueryResponse>("LanguageClient", response));

        var documentSynchronizer = new Mock<LSPDocumentSynchronizer>(MockBehavior.Strict);
        documentSynchronizer
            .Setup(d => d.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                _documentSnapshot.Version, _documentSnapshot.Uri, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SynchronizedResult<CSharpVirtualDocumentSnapshot>(false, VirtualSnapshot: null));

        var projectionProvider = new DefaultLSPProjectionProvider(requestInvoker.Object, documentSynchronizer.Object, TestRazorLogger.Instance, LoggerProvider);

        // Act
        var result = await projectionProvider.GetProjectionAsync(_documentSnapshot, new Position(), DisposalToken);

        // Assert
        Assert.Null(result);
    }
}
