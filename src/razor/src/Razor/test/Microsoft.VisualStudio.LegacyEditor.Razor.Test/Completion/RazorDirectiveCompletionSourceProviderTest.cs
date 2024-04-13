// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;
using Microsoft.VisualStudio.Razor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Projection;
using Microsoft.VisualStudio.Utilities;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Completion;

public class RazorDirectiveCompletionSourceProviderTest(ITestOutputHelper testOutput) : VisualStudioTestBase(testOutput)
{
    private static readonly IContentType s_razorContentType = VsMocks.ContentTypes.Create(RazorLanguage.ContentType, RazorConstants.LegacyContentType);

    [Fact]
    public void CreateCompletionSource_ReturnsNullIfParserHasNotBeenAssociatedWithRazorBuffer()
    {
        // Arrange
        var expectedParser = StrictMock.Of<IVisualStudioRazorParser>();
        var properties = new PropertyCollection();
        properties.AddProperty(typeof(IVisualStudioRazorParser), expectedParser);
        var razorBuffer = VsMocks.CreateTextBuffer(s_razorContentType, properties);
        var completionFactsService = StrictMock.Of<IRazorCompletionFactsService>();
        var completionSourceProvider = new RazorDirectiveCompletionSourceProvider(completionFactsService);

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
        var razorBuffer = VsMocks.CreateTextBuffer(s_razorContentType);
        var completionFactsService = StrictMock.Of<IRazorCompletionFactsService>();
        var completionSourceProvider = new RazorDirectiveCompletionSourceProvider(completionFactsService);

        // Act
        var completionSource = completionSourceProvider.CreateCompletionSource(razorBuffer);

        // Assert
        Assert.Null(completionSource);
    }

    [Fact]
    public void GetOrCreate_ReturnsNullIfRazorBufferHasNotBeenAssociatedWithTextView()
    {
        // Arrange
        var textView = CreateTextView(VsMocks.ContentTypes.NonRazor, new PropertyCollection());
        var completionFactsService = StrictMock.Of<IRazorCompletionFactsService>();
        var completionSourceProvider = new RazorDirectiveCompletionSourceProvider(completionFactsService);

        // Act
        var completionSource = completionSourceProvider.GetOrCreate(textView);

        // Assert
        Assert.Null(completionSource);
    }

    [Fact]
    public void GetOrCreate_CachesCompletionSource()
    {
        // Arrange
        var expectedParser = StrictMock.Of<IVisualStudioRazorParser>();
        var properties = new PropertyCollection();
        properties.AddProperty(typeof(IVisualStudioRazorParser), expectedParser);
        var textView = CreateTextView(s_razorContentType, properties);
        var completionFactsService = StrictMock.Of<IRazorCompletionFactsService>();
        var completionSourceProvider = new RazorDirectiveCompletionSourceProvider(completionFactsService);

        // Act
        var completionSource1 = completionSourceProvider.GetOrCreate(textView);
        var completionSource2 = completionSourceProvider.GetOrCreate(textView);

        // Assert
        Assert.Same(completionSource1, completionSource2);
    }

    private static ITextView CreateTextView(IContentType contentType, PropertyCollection properties)
    {
        var bufferGraphMock = new StrictMock<IBufferGraph>();
        bufferGraphMock
            .Setup(graph => graph.GetTextBuffers(It.IsAny<Predicate<ITextBuffer>>()))
            .Returns(
            [
                VsMocks.CreateTextBuffer(contentType, properties)
            ]);
        var textView = StrictMock.Of<ITextView>(b =>
            b.BufferGraph == bufferGraphMock.Object);

        return textView;
    }
}
