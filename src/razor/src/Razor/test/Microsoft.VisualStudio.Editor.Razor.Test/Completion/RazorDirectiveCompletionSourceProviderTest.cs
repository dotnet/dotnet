// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.ObjectModel;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Projection;
using Microsoft.VisualStudio.Utilities;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Editor.Razor.Completion;

public class RazorDirectiveCompletionSourceProviderTest : ProjectSnapshotManagerDispatcherTestBase
{
    private readonly IContentType _razorContentType;
    private readonly IContentType _nonRazorContentType;
    private readonly RazorCompletionFactsService _completionFactsService;

    public RazorDirectiveCompletionSourceProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _razorContentType = Mock.Of<IContentType>(
            c => c.IsOfType(RazorLanguage.ContentType) && c.IsOfType(RazorConstants.LegacyContentType),
            MockBehavior.Strict);

        _nonRazorContentType = Mock.Of<IContentType>(
            c => c.IsOfType(It.IsAny<string>()) == false,
            MockBehavior.Strict);

        _completionFactsService = Mock.Of<RazorCompletionFactsService>(MockBehavior.Strict);
    }

    [Fact]
    public void CreateCompletionSource_ReturnsNullIfParserHasNotBeenAssocitedWithRazorBuffer()
    {
        // Arrange
        var expectedParser = Mock.Of<VisualStudioRazorParser>(MockBehavior.Strict);
        var properties = new PropertyCollection();
        properties.AddProperty(typeof(VisualStudioRazorParser), expectedParser);
        var razorBuffer = Mock.Of<ITextBuffer>(buffer => buffer.ContentType == _razorContentType && buffer.Properties == properties, MockBehavior.Strict);
        var completionSourceProvider = new RazorDirectiveCompletionSourceProvider(_completionFactsService);

        // Act
        var completionSource = completionSourceProvider.CreateCompletionSource(razorBuffer);

        // Assert
        var completionSourceImpl = Assert.IsType<RazorDirectiveCompletionSource>(completionSource);
        Assert.Same(expectedParser, completionSourceImpl.Parser);
    }

    [Fact]
    public void CreateCompletionSource_CreatesACompletionSourceWithTextBuffersParser()
    {
        // Arrange
        var razorBuffer = Mock.Of<ITextBuffer>(buffer => buffer.ContentType == _razorContentType && buffer.Properties == new PropertyCollection(), MockBehavior.Strict);
        var completionSourceProvider = new RazorDirectiveCompletionSourceProvider(_completionFactsService);

        // Act
        var completionSource = completionSourceProvider.CreateCompletionSource(razorBuffer);

        // Assert
        Assert.Null(completionSource);
    }

    [Fact]
    public void GetOrCreate_ReturnsNullIfRazorBufferHasNotBeenAssociatedWithTextView()
    {
        // Arrange
        var textView = CreateTextView(_nonRazorContentType, new PropertyCollection());
        var completionSourceProvider = new RazorDirectiveCompletionSourceProvider(_completionFactsService);

        // Act
        var completionSource = completionSourceProvider.GetOrCreate(textView);

        // Assert
        Assert.Null(completionSource);
    }

    [Fact]
    public void GetOrCreate_CachesCompletionSource()
    {
        // Arrange
        var expectedParser = Mock.Of<VisualStudioRazorParser>(MockBehavior.Strict);
        var properties = new PropertyCollection();
        properties.AddProperty(typeof(VisualStudioRazorParser), expectedParser);
        var textView = CreateTextView(_razorContentType, properties);
        var completionSourceProvider = new RazorDirectiveCompletionSourceProvider(_completionFactsService);

        // Act
        var completionSource1 = completionSourceProvider.GetOrCreate(textView);
        var completionSource2 = completionSourceProvider.GetOrCreate(textView);

        // Assert
        Assert.Same(completionSource1, completionSource2);
    }

    private static ITextView CreateTextView(IContentType contentType, PropertyCollection properties)
    {
        var bufferGraph = new Mock<IBufferGraph>(MockBehavior.Strict);
        bufferGraph.Setup(graph => graph.GetTextBuffers(It.IsAny<Predicate<ITextBuffer>>()))
            .Returns(new Collection<ITextBuffer>()
            {
                Mock.Of<ITextBuffer>(buffer => buffer.ContentType == contentType && buffer.Properties == properties, MockBehavior.Strict)
            });
        var textView = Mock.Of<ITextView>(view => view.BufferGraph == bufferGraph.Object, MockBehavior.Strict);

        return textView;
    }
}
