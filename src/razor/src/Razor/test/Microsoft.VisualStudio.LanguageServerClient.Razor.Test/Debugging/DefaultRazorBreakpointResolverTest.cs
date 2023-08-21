// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.Editor.Razor.Debugging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Test;
using Microsoft.VisualStudio.Test;
using Microsoft.VisualStudio.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Debugging;

public class DefaultRazorBreakpointResolverTest : TestBase
{
    private const string ValidBreakpointCSharp = "private int foo = 123;";
    private const string InvalidBreakpointCSharp = "private int bar;";

    private readonly ITextBuffer _csharpTextBuffer;
    private readonly Uri _documentUri;
    private readonly Uri _csharpDocumentUri;
    private readonly ITextBuffer _hostTextbuffer;

    public DefaultRazorBreakpointResolverTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _documentUri = new Uri("file://C:/path/to/file.razor", UriKind.Absolute);
        _csharpDocumentUri = new Uri(_documentUri.OriginalString + ".g.cs", UriKind.Absolute);

        var csharpTextSnapshot = new StringTextSnapshot(
$@"public class SomeRazorFile
{{
    {ValidBreakpointCSharp}
    {InvalidBreakpointCSharp}
}}");
        _csharpTextBuffer = new TestTextBuffer(csharpTextSnapshot);

        var textBufferSnapshot = new StringTextSnapshot(@$"
@code
{{
    {ValidBreakpointCSharp}
    {InvalidBreakpointCSharp}
}}
");
        _hostTextbuffer = new TestTextBuffer(textBufferSnapshot);
    }

    [Fact]
    public async Task TryResolveBreakpointRangeAsync_UnaddressableTextBuffer_ReturnsNull()
    {
        // Arrange
        var differentTextBuffer = Mock.Of<ITextBuffer>(MockBehavior.Strict);
        var resolver = CreateResolverWith();

        // Act
        var breakpointRange = await resolver.TryResolveBreakpointRangeAsync(differentTextBuffer, lineIndex: 0, characterIndex: 1, DisposalToken);

        // Assert
        Assert.Null(breakpointRange);
    }

    [Fact]
    public async Task TryResolveBreakpointRangeAsync_UnknownRazorDocument_ReturnsNull()
    {
        // Arrange
        var documentManager = new Mock<LSPDocumentManager>(MockBehavior.Strict).Object;
        Mock.Get(documentManager).Setup(m => m.TryGetDocument(_documentUri, out It.Ref<LSPDocumentSnapshot>.IsAny)).Returns(false);
        var resolver = CreateResolverWith(documentManager: documentManager);

        // Act
        var result = await resolver.TryResolveBreakpointRangeAsync(_hostTextbuffer, lineIndex: 0, characterIndex: 1, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task TryResolveBreakpointRangeAsync_UnsynchronizedCSharpDocument_ReturnsNull()
    {
        // Arrange
        var documentManager = new TestDocumentManager();
        var testCSharpDocument = new CSharpVirtualDocumentSnapshot(projectKey: default, _csharpDocumentUri, _csharpTextBuffer.CurrentSnapshot, hostDocumentSyncVersion: 1);
        var document = new TestLSPDocumentSnapshot(_documentUri, version: (int)(testCSharpDocument.HostDocumentSyncVersion.Value + 1), testCSharpDocument);
        documentManager.AddDocument(document.Uri, document);
        var resolver = CreateResolverWith(documentManager: documentManager);

        // Act
        var expressions = await resolver.TryResolveBreakpointRangeAsync(_hostTextbuffer, lineIndex: 0, characterIndex: 1, DisposalToken);

        // Assert
        Assert.Null(expressions);
    }

    [Fact]
    public async Task TryResolveBreakpointRangeAsync_UnprojectedLocation_ReturnsNull()
    {
        // Arrange
        var resolver = CreateResolverWith();

        // Act
        var breakpointRange = await resolver.TryResolveBreakpointRangeAsync(_hostTextbuffer, lineIndex: 0, characterIndex: 1, DisposalToken);

        // Assert
        Assert.Null(breakpointRange);
    }

    [Fact]
    public async Task TryResolveBreakpointRangeAsync_NotValidBreakpointLocation_ReturnsNull()
    {
        // Arrange
        var hostDocumentPosition = GetPosition(InvalidBreakpointCSharp, _hostTextbuffer);
        var resolver = CreateResolverWith();

        // Act
        var breakpointRange = await resolver.TryResolveBreakpointRangeAsync(_hostTextbuffer, hostDocumentPosition.Line, hostDocumentPosition.Character, DisposalToken);

        // Assert
        Assert.Null(breakpointRange);
    }

    [Fact]
    public async Task TryResolveBreakpointRangeAsync_MappableCSharpBreakpointLocation_ReturnsHostBreakpointLocation()
    {
        // Arrange
        var hostDocumentPosition = GetPosition(ValidBreakpointCSharp, _hostTextbuffer);
        var hostBreakpointRange = new Range()
        {
            Start = hostDocumentPosition,
            End = new Position(hostDocumentPosition.Line, hostDocumentPosition.Character + ValidBreakpointCSharp.Length),
        };
        var projectionProvider = new TestLSPBreakpointSpanProvider(
            _documentUri,
            new Dictionary<Position, Range>()
            {
                [hostDocumentPosition] = hostBreakpointRange
            });
        var resolver = CreateResolverWith(projectionProvider: projectionProvider);

        // Act
        var breakpointRange = await resolver.TryResolveBreakpointRangeAsync(_hostTextbuffer, hostDocumentPosition.Line, hostDocumentPosition.Character, DisposalToken);

        // Assert
        Assert.Equal(hostBreakpointRange, breakpointRange);
    }

    private RazorBreakpointResolver CreateResolverWith(
        FileUriProvider uriProvider = null,
        LSPDocumentManager documentManager = null,
        LSPBreakpointSpanProvider projectionProvider = null)
    {
        var documentUri = _documentUri;
        uriProvider ??= Mock.Of<FileUriProvider>(provider => provider.TryGet(_hostTextbuffer, out documentUri) == true && provider.TryGet(It.IsNotIn(_hostTextbuffer), out It.Ref<Uri>.IsAny) == false, MockBehavior.Strict);
        var csharpVirtualDocumentSnapshot = new CSharpVirtualDocumentSnapshot(projectKey: default, _csharpDocumentUri, _csharpTextBuffer.CurrentSnapshot, hostDocumentSyncVersion: 0);
        LSPDocumentSnapshot documentSnapshot = new TestLSPDocumentSnapshot(_documentUri, 0, csharpVirtualDocumentSnapshot);
        documentManager ??= Mock.Of<LSPDocumentManager>(manager => manager.TryGetDocument(_documentUri, out documentSnapshot) == true, MockBehavior.Strict);
        if (projectionProvider is null)
        {
            projectionProvider = new Mock<LSPBreakpointSpanProvider>(MockBehavior.Strict).Object;
            Mock.Get(projectionProvider)
                .Setup(projectionProvider => projectionProvider.GetBreakpointSpanAsync(
                    It.IsAny<LSPDocumentSnapshot>(),
                    It.IsAny<Position>(),
                    DisposalToken))
                .ReturnsAsync(value: null);
        }

        var razorBreakpointResolver = new DefaultRazorBreakpointResolver(uriProvider, documentManager, projectionProvider);

        return razorBreakpointResolver;
    }

    private static Position GetPosition(string content, ITextBuffer textBuffer)
    {
        var index = textBuffer.CurrentSnapshot.GetText().IndexOf(content, StringComparison.Ordinal);
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(content));
        }

        var line = textBuffer.CurrentSnapshot.GetLineFromPosition(index);
        return new Position(line.LineNumber, index - line.Start.Position);
    }
}
