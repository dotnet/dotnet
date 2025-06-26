// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.VisualStudio.LegacyEditor.Razor.Indentation;
using Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;
using Microsoft.VisualStudio.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

public class RazorEditorFactoryServiceTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public void TryGetDocumentTracker_ForRazorTextBuffer_ReturnsTrue()
    {
        // Arrange
        var expectedDocumentTracker = StrictMock.Of<IVisualStudioDocumentTracker>();
        var factoryService = CreateFactoryService(expectedDocumentTracker);
        var textBuffer = VsMocks.CreateTextBuffer(core: true);

        // Act
        var result = factoryService.TryGetDocumentTracker(textBuffer, out var documentTracker);

        // Assert
        Assert.True(result);
        Assert.Same(expectedDocumentTracker, documentTracker);
    }

    [Fact]
    public void TryGetDocumentTracker_NonRazorBuffer_ReturnsFalse()
    {
        // Arrange
        var factoryService = CreateFactoryService();
        var textBuffer = VsMocks.CreateTextBuffer(core: false);

        // Act
        var result = factoryService.TryGetDocumentTracker(textBuffer, out var documentTracker);

        // Assert
        Assert.False(result);
        Assert.Null(documentTracker);
    }

    [Fact]
    public void TryInitializeTextBuffer_StoresTracker_ReturnsTrue()
    {
        // Arrange
        var expectedDocumentTracker = StrictMock.Of<IVisualStudioDocumentTracker>();
        var factoryService = CreateFactoryService(expectedDocumentTracker);
        var textBuffer = VsMocks.CreateTextBuffer(core: true);

        // Act
        var result = factoryService.TryInitializeTextBuffer(textBuffer);

        // Assert
        Assert.True(result);
        Assert.True(textBuffer.Properties.TryGetProperty(typeof(IVisualStudioDocumentTracker), out IVisualStudioDocumentTracker documentTracker));
        Assert.Same(expectedDocumentTracker, documentTracker);
    }

    [Fact]
    public void TryInitializeTextBuffer_OnlyStoresTrackerOnTextBufferOnce_ReturnsTrue()
    {
        // Arrange
        var factoryService = CreateFactoryService();
        var textBuffer = VsMocks.CreateTextBuffer(core: true);
        factoryService.TryInitializeTextBuffer(textBuffer);
        var expectedDocumentTracker = textBuffer.Properties[typeof(IVisualStudioDocumentTracker)];

        // Create a second factory service so it generates a different tracker
        factoryService = CreateFactoryService();

        // Act
        var result = factoryService.TryInitializeTextBuffer(textBuffer);

        // Assert
        Assert.True(result);
        Assert.True(textBuffer.Properties.TryGetProperty(typeof(IVisualStudioDocumentTracker), out IVisualStudioDocumentTracker documentTracker));
        Assert.Same(expectedDocumentTracker, documentTracker);
    }

    [Fact]
    public void TryGetParser_ForRazorTextBuffer_ReturnsTrue()
    {
        // Arrange
        var expectedParser = StrictMock.Of<IVisualStudioRazorParser>();
        var factoryService = CreateFactoryService(parser: expectedParser);
        var textBuffer = VsMocks.CreateTextBuffer(core: true);

        // Act
        var result = factoryService.TryGetParser(textBuffer, out var parser);

        // Assert
        Assert.True(result);
        Assert.Same(expectedParser, parser);
    }

    [Fact]
    public void TryGetParser_NonRazorBuffer_ReturnsFalse()
    {
        // Arrange
        var factoryService = CreateFactoryService();
        var textBuffer = VsMocks.CreateTextBuffer(core: false);

        // Act
        var result = factoryService.TryGetParser(textBuffer, out var parser);

        // Assert
        Assert.False(result);
        Assert.Null(parser);
    }

    [Fact]
    public void TryInitializeTextBuffer_StoresParser_ReturnsTrue()
    {
        // Arrange
        var expectedParser = StrictMock.Of<IVisualStudioRazorParser>();
        var factoryService = CreateFactoryService(parser: expectedParser);
        var textBuffer = VsMocks.CreateTextBuffer(core: true);

        // Act
        var result = factoryService.TryInitializeTextBuffer(textBuffer);

        // Assert
        Assert.True(result);
        Assert.True(textBuffer.Properties.TryGetProperty(typeof(IVisualStudioRazorParser), out IVisualStudioRazorParser parser));
        Assert.Same(expectedParser, parser);
    }

    [Fact]
    public void TryInitializeTextBuffer_OnlyStoresParserOnTextBufferOnce_ReturnsTrue()
    {
        // Arrange
        var factoryService = CreateFactoryService();
        var textBuffer = VsMocks.CreateTextBuffer(core: true);
        factoryService.TryInitializeTextBuffer(textBuffer);
        var expectedParser = textBuffer.Properties[typeof(IVisualStudioRazorParser)];

        // Create a second factory service so it generates a different parser
        factoryService = CreateFactoryService();

        // Act
        var result = factoryService.TryInitializeTextBuffer(textBuffer);

        // Assert
        Assert.True(result);
        Assert.True(textBuffer.Properties.TryGetProperty(typeof(IVisualStudioRazorParser), out IVisualStudioRazorParser parser));
        Assert.Same(expectedParser, parser);
    }

    [Fact]
    public void TryGetSmartIndenter_ForRazorTextBuffer_ReturnsTrue()
    {
        // Arrange
        var expectedSmartIndenter = StrictMock.Of<BraceSmartIndenter>();
        var factoryService = CreateFactoryService(smartIndenter: expectedSmartIndenter);
        var textBuffer = VsMocks.CreateTextBuffer(core: true);

        // Act
        var result = factoryService.TryGetSmartIndenter(textBuffer, out var smartIndenter);

        // Assert
        Assert.True(result);
        Assert.Same(expectedSmartIndenter, smartIndenter);
    }

    [Fact]
    public void TryGetSmartIndenter_NonRazorBuffer_ReturnsFalse()
    {
        // Arrange
        var factoryService = CreateFactoryService();
        var textBuffer = VsMocks.CreateTextBuffer(core: false);

        // Act
        var result = factoryService.TryGetSmartIndenter(textBuffer, out var smartIndenter);

        // Assert
        Assert.False(result);
        Assert.Null(smartIndenter);
    }

    [Fact]
    public void TryInitializeTextBuffer_StoresSmartIndenter_ReturnsTrue()
    {
        // Arrange
        var expectedSmartIndenter = StrictMock.Of<BraceSmartIndenter>();
        var factoryService = CreateFactoryService(smartIndenter: expectedSmartIndenter);
        var textBuffer = VsMocks.CreateTextBuffer(core: true);

        // Act
        var result = factoryService.TryInitializeTextBuffer(textBuffer);

        // Assert
        Assert.True(result);
        Assert.True(textBuffer.Properties.TryGetProperty(typeof(BraceSmartIndenter), out BraceSmartIndenter smartIndenter));
        Assert.Same(expectedSmartIndenter, smartIndenter);
    }

    [Fact]
    public void TryInitializeTextBuffer_OnlyStoresSmartIndenterOnTextBufferOnce_ReturnsTrue()
    {
        // Arrange
        var factoryService = CreateFactoryService();
        var textBuffer = VsMocks.CreateTextBuffer(core: true);
        factoryService.TryInitializeTextBuffer(textBuffer);
        var expectedSmartIndenter = textBuffer.Properties[typeof(BraceSmartIndenter)];

        // Create a second factory service so it generates a different smart indenter
        factoryService = CreateFactoryService();

        // Act
        var result = factoryService.TryInitializeTextBuffer(textBuffer);

        // Assert
        Assert.True(result);
        Assert.True(textBuffer.Properties.TryGetProperty(typeof(BraceSmartIndenter), out BraceSmartIndenter smartIndenter));
        Assert.Same(expectedSmartIndenter, smartIndenter);
    }

    private static RazorEditorFactoryService CreateFactoryService(
        IVisualStudioDocumentTracker? documentTracker = null,
        IVisualStudioRazorParser? parser = null,
        BraceSmartIndenter? smartIndenter = null)
    {
        documentTracker ??= StrictMock.Of<IVisualStudioDocumentTracker>();
        parser ??= StrictMock.Of<IVisualStudioRazorParser>();
        smartIndenter ??= StrictMock.Of<BraceSmartIndenter>();

        var documentTrackerFactory = StrictMock.Of<IVisualStudioDocumentTrackerFactory>(f =>
            f.Create(It.IsAny<ITextBuffer>()) == documentTracker);
        var parserFactory = StrictMock.Of<IVisualStudioRazorParserFactory>(f =>
            f.Create(It.IsAny<IVisualStudioDocumentTracker>()) == parser);
        var smartIndenterFactory = StrictMock.Of<IBraceSmartIndenterFactory>(f =>
            f.Create(It.IsAny<IVisualStudioDocumentTracker>()) == smartIndenter);

        var factoryService = new RazorEditorFactoryService(documentTrackerFactory, parserFactory, smartIndenterFactory);

        return factoryService;
    }
}
