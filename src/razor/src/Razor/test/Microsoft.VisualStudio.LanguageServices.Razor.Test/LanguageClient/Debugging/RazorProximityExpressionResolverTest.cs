// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.Debugging;
using Microsoft.VisualStudio.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Debugging;

public class RazorProximityExpressionResolverTest : ToolingTestBase
{
    private readonly string _validProximityExpressionRoot;
    private readonly string _invalidProximityExpressionRoot;
    private readonly ITextBuffer _csharpTextBuffer;
    private readonly Uri _documentUri;
    private readonly Uri _csharpDocumentUri;
    private readonly ITextBuffer _hostTextBuffer;

    public RazorProximityExpressionResolverTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _documentUri = new Uri("file://C:/path/to/file.razor", UriKind.Absolute);
        _csharpDocumentUri = new Uri(_documentUri.OriginalString + ".ide.g.cs", UriKind.Absolute);

        _validProximityExpressionRoot = "var abc = 123;";
        _invalidProximityExpressionRoot = "private int bar;";
        var csharpTextSnapshot = new StringTextSnapshot($$"""
            public class SomeRazorFile
            {
                {{_invalidProximityExpressionRoot}}

                public void Render()
                {
                    {{_validProximityExpressionRoot}}
                }
            }
            """);
        _csharpTextBuffer = new TestTextBuffer(csharpTextSnapshot);

        var textBufferSnapshot = new StringTextSnapshot($$"""@{{{_invalidProximityExpressionRoot}}} @code {{{_validProximityExpressionRoot}}}""");
        _hostTextBuffer = new TestTextBuffer(textBufferSnapshot);
    }

    [Fact]
    public async Task TryResolveProximityExpressionsAsync_UnaddressableTextBuffer_ReturnsNull()
    {
        // Arrange
        var differentTextBuffer = Mock.Of<ITextBuffer>(MockBehavior.Strict);
        var resolver = CreateResolverWith();

        // Act
        var expressions = await resolver.TryResolveProximityExpressionsAsync(differentTextBuffer, lineIndex: 0, characterIndex: 1, DisposalToken);

        // Assert
        Assert.Null(expressions);
    }

    [Fact]
    public async Task TryResolveProximityExpressionsAsync_UnknownRazorDocument_ReturnsNull()
    {
        // Arrange
        var documentManager = new Mock<LSPDocumentManager>(MockBehavior.Strict).Object;
        Mock.Get(documentManager)
            .Setup(m => m.TryGetDocument(_documentUri, out It.Ref<LSPDocumentSnapshot>.IsAny))
            .Returns(false);
        var resolver = CreateResolverWith(documentManager: documentManager);

        // Act
        var result = await resolver.TryResolveProximityExpressionsAsync(_hostTextBuffer, lineIndex: 0, characterIndex: 1, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task TryResolveProximityExpressionsAsync_UnsynchronizedCSharpDocument_ReturnsNull()
    {
        // Arrange
        var documentManager = new TestDocumentManager();
        var testCSharpDocument = new CSharpVirtualDocumentSnapshot(projectKey: default, _csharpDocumentUri, _csharpTextBuffer.CurrentSnapshot, hostDocumentSyncVersion: 1);
        var document = new TestLSPDocumentSnapshot(_documentUri, version: (int)(testCSharpDocument.HostDocumentSyncVersion.Value + 1), testCSharpDocument);
        documentManager.AddDocument(document.Uri, document);
        var resolver = CreateResolverWith(documentManager: documentManager);

        // Act
        var expressions = await resolver.TryResolveProximityExpressionsAsync(_hostTextBuffer, lineIndex: 0, characterIndex: 1, DisposalToken);

        // Assert
        Assert.Null(expressions);
    }

    private IRazorProximityExpressionResolver CreateResolverWith(
        FileUriProvider uriProvider = null,
        LSPDocumentManager documentManager = null)
    {
        var documentUri = _documentUri;
        uriProvider ??= Mock.Of<FileUriProvider>(provider => provider.TryGet(_hostTextBuffer, out documentUri) == true && provider.TryGet(It.IsNotIn(_hostTextBuffer), out It.Ref<Uri>.IsAny) == false, MockBehavior.Strict);
        var csharpVirtualDocumentSnapshot = new CSharpVirtualDocumentSnapshot(projectKey: default, _csharpDocumentUri, _csharpTextBuffer.CurrentSnapshot, hostDocumentSyncVersion: 0);
        LSPDocumentSnapshot documentSnapshot = new TestLSPDocumentSnapshot(_documentUri, 0, csharpVirtualDocumentSnapshot);
        documentManager ??= Mock.Of<LSPDocumentManager>(
            manager => manager.TryGetDocument(_documentUri, out documentSnapshot) == true,
            MockBehavior.Strict);
        var remoteServiceInvoker = StrictMock.Of<IRemoteServiceInvoker>();

        var razorProximityExpressionResolver = new RazorProximityExpressionResolver(
            uriProvider,
            documentManager,
            TestLSPProximityExpressionProvider.Instance,
            TestLanguageServerFeatureOptions.Instance,
            remoteServiceInvoker);

        return razorProximityExpressionResolver;
    }

    private class TestLSPProximityExpressionProvider : ILSPProximityExpressionsProvider
    {
        public static readonly TestLSPProximityExpressionProvider Instance = new();

        private TestLSPProximityExpressionProvider()
        {
        }

        public Task<IReadOnlyList<string>> GetProximityExpressionsAsync(LSPDocumentSnapshot documentSnapshot, long hostDocumentSyncVersion, Position position, CancellationToken cancellationToken)
        {
            return SpecializedTasks.Null<IReadOnlyList<string>>();
        }
    }
}
