// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Linq;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

public class DefaultLSPDocumentFactoryTest : TestBase
{
    public DefaultLSPDocumentFactoryTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void Create_BuildsLSPDocumentWithTextBufferURI()
    {
        // Arrange
        var textBuffer = Mock.Of<ITextBuffer>(MockBehavior.Strict);
        var uri = new Uri("C:/path/to/file.razor");
        var uriProvider = Mock.Of<FileUriProvider>(p => p.GetOrCreate(textBuffer) == uri, MockBehavior.Strict);
        var factory = new DefaultLSPDocumentFactory(uriProvider, Enumerable.Empty<Lazy<VirtualDocumentFactory, IContentTypeMetadata>>());

        // Act
        var lspDocument = factory.Create(textBuffer);

        // Assert
        Assert.Same(uri, lspDocument.Uri);
    }

    [Fact]
    public void Create_MultipleFactories_CreatesLSPDocumentWithVirtualDocuments()
    {
        // Arrange
        var contentType = Mock.Of<IContentType>(ct =>
            ct.TypeName == "text" &&
            ct.IsOfType("text"),
            MockBehavior.Strict
        );
        var metadata = Mock.Of<IContentTypeMetadata>(md =>
            md.ContentTypes == new[] { contentType.TypeName },
            MockBehavior.Strict);
        var textBuffer = Mock.Of<ITextBuffer>(b =>
            b.ContentType == contentType,
            MockBehavior.Strict);
        var uri = new Uri("C:/path/to/file.razor");
        var uriProvider = Mock.Of<FileUriProvider>(p => p.GetOrCreate(textBuffer) == uri, MockBehavior.Strict);
        var virtualDocument1 = Mock.Of<VirtualDocument>(MockBehavior.Strict);
        var factory1 = Mock.Of<VirtualDocumentFactory>(f => f.TryCreateFor(textBuffer, out virtualDocument1) == true, MockBehavior.Strict);
        var factory1Lazy = new Lazy<VirtualDocumentFactory, IContentTypeMetadata>(() => factory1, metadata);
        var virtualDocument2 = Mock.Of<VirtualDocument>(MockBehavior.Strict);
        var factory2 = Mock.Of<VirtualDocumentFactory>(f => f.TryCreateFor(textBuffer, out virtualDocument2) == true, MockBehavior.Strict);
        var factory2Lazy = new Lazy<VirtualDocumentFactory, IContentTypeMetadata>(() => factory2, metadata);
        var factory = new DefaultLSPDocumentFactory(uriProvider, new[] { factory1Lazy, factory2Lazy });

        // Act
        var lspDocument = factory.Create(textBuffer);

        // Assert
        Assert.Collection(
            lspDocument.VirtualDocuments,
            virtualDocument => Assert.Same(virtualDocument1, virtualDocument),
            virtualDocument => Assert.Same(virtualDocument2, virtualDocument));
    }

    [Fact]
    public void Create_FiltersFactoriesByContentType()
    {
        // Arrange
        var contentType = Mock.Of<IContentType>(ct =>
            ct.TypeName == "text" &&
            ct.IsOfType("NotText") == false,
            MockBehavior.Strict
        );
        var metadata = Mock.Of<IContentTypeMetadata>(md =>
            md.ContentTypes == new[] { "NotText" },
            MockBehavior.Strict);
        var textBuffer = Mock.Of<ITextBuffer>(b =>
            b.ContentType == contentType,
            MockBehavior.Strict);
        var uri = new Uri("C:/path/to/file.razor");
        var uriProvider = Mock.Of<FileUriProvider>(p => p.GetOrCreate(textBuffer) == uri, MockBehavior.Strict);
        var factory1 = Mock.Of<VirtualDocumentFactory>(MockBehavior.Strict);
        var factory1Lazy = new Lazy<VirtualDocumentFactory, IContentTypeMetadata>(() => factory1, metadata);
        var factory = new DefaultLSPDocumentFactory(uriProvider, new[] { factory1Lazy, });

        // Act
        var lspDocument = factory.Create(textBuffer);

        // Assert
        Assert.Equal(0, lspDocument.VirtualDocuments.Count);
    }
}
