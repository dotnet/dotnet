// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hover;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Tooltip;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text.Adornments;
using Moq;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.LanguageServer.Extensions.SourceTextExtensions;
using static Microsoft.AspNetCore.Razor.LanguageServer.Tooltip.DefaultVSLSPTagHelperTooltipFactory;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Hover;

[UseExportProvider]
public class DefaultRazorHoverInfoServiceTest : TagHelperServiceTestBase
{
    public DefaultRazorHoverInfoServiceTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    private static VSInternalClientCapabilities CreateMarkDownCapabilities()
        => CreateCapabilities(MarkupKind.Markdown);

    private static VSInternalClientCapabilities CreatePlainTextCapabilities()
        => CreateCapabilities(MarkupKind.PlainText);

    private static VSInternalClientCapabilities CreateCapabilities(MarkupKind markupKind)
        => new()
        {
            TextDocument = new()
            {
                Hover = new()
                {
                    ContentFormat = new[] { markupKind },
                }
            }
        };

    [Fact]
    public void GetHoverInfo_TagHelper_Element()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <$$test1></test1>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreateMarkDownCapabilities());

        // Assert
        Assert.Contains("**Test1TagHelper**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = new Range
        {
            Start = new Position(1, 1),
            End = new Position(1, 6),
        };
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public void GetHoverInfo_TagHelper_Element_EndTag()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <test1></$$test1>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreateMarkDownCapabilities());

        // Assert
        Assert.Contains("**Test1TagHelper**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = new Range
        {
            Start = new Position(1, 9),
            End = new Position(1, 14),
        };
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public void GetHoverInfo_TagHelper_Attribute()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <test1 $$bool-val='true'></test1>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreateMarkDownCapabilities());

        // Assert
        Assert.Contains("**BoolVal**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.DoesNotContain("**IntVal**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = new Range
        {
            Start = new Position(1, 7),
            End = new Position(1, 15),
        };
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public void GetHoverInfo_TagHelper_AttributeTrailingEdge()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <test1 bool-val$$ minimized></test1>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var service = GetDefaultRazorHoverInfoService();
        var edgeLocation = cursorPosition;
        var location = new SourceLocation(edgeLocation, 0, edgeLocation);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreateMarkDownCapabilities());

        // Assert
        Assert.Contains("**BoolVal**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.DoesNotContain("**IntVal**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = new Range
        {
            Start = new Position(1, 7),
            End = new Position(1, 15),
        };
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public void GetHoverInfo_TagHelper_AttributeValue_ReturnsNull()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <test1 bool-val='$$true'></test1>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreateMarkDownCapabilities());

        // Assert
        Assert.Null(hover);
    }

    [Fact]
    public void GetHoverInfo_TagHelper_AfterAttributeEquals_ReturnsNull()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <test1 bool-val=$$'true'></test1>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreateMarkDownCapabilities());

        // Assert
        Assert.Null(hover);
    }

    [Fact]
    public void GetHoverInfo_TagHelper_AttributeEnd_ReturnsNull()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <test1 bool-val='true'$$></test1>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreateMarkDownCapabilities());

        // Assert
        Assert.Null(hover);
    }

    [Fact]
    public void GetHoverInfo_TagHelper_MinimizedAttribute()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <test1 $$bool-val></test1>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreateMarkDownCapabilities());

        // Assert
        Assert.Contains("**BoolVal**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.DoesNotContain("**IntVal**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = new Range
        {
            Start = new Position(1, 7),
            End = new Position(1, 15),
        };
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public void GetHoverInfo_DirectiveAttribute_HasResult()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <any @t$$est="Increment" />
                @code{
                    public void Increment(){
                    }
                }
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, "text.razor", DefaultTagHelpers);
        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreateMarkDownCapabilities());

        // Assert
        Assert.NotNull(hover);
        Assert.Contains("**Test**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = new Range
        {
            Start = new Position(1, 5),
            End = new Position(1, 10),
        };
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public void GetHoverInfo_TagHelper_MalformedElement()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <$$test1<hello
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreateMarkDownCapabilities());

        // Assert
        Assert.Contains("**Test1TagHelper**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = new Range
        {
            Start = new Position(1, 1),
            End = new Position(1, 6),
        };
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public void GetHoverInfo_TagHelper_MalformedAttribute()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <test1 $$bool-val=\"aslj alsk<strong>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreateMarkDownCapabilities());

        // Assert
        Assert.Contains("**BoolVal**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.DoesNotContain("**IntVal**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = new Range
        {
            Start = new Position(1, 7),
            End = new Position(1, 15),
        };
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public void GetHoverInfo_HTML_MarkupElement()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <p><$$strong></strong></p>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false);
        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreateMarkDownCapabilities());

        // Assert
        Assert.Null(hover);
    }

    [Fact]
    public void GetHoverInfo_TagHelper_PlainTextElement()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <$$test1></test1>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);

        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreatePlainTextCapabilities());

        // Assert
        Assert.Contains("Test1TagHelper", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.Equal(MarkupKind.PlainText, ((MarkupContent)hover.Contents).Kind);
        var expectedRange = new Range
        {
            Start = new Position(1, 1),
            End = new Position(1, 6),
        };
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public void GetHoverInfo_TagHelper_PlainTextElement_EndTag()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <test1></$$test1>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);

        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreatePlainTextCapabilities());

        // Assert
        Assert.Contains("Test1TagHelper", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.Equal(MarkupKind.PlainText, ((MarkupContent)hover.Contents).Kind);
        var expectedRange = new Range
        {
            Start = new Position(1, 9),
            End = new Position(1, 14),
        };
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public void GetHoverInfo_TagHelper_PlainTextAttribute()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <test1 $$bool-val></test1>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);

        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreatePlainTextCapabilities());

        // Assert
        Assert.Contains("BoolVal", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.DoesNotContain("IntVal", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.Equal(MarkupKind.PlainText, ((MarkupContent)hover.Contents).Kind);
        var expectedRange = new Range
        {
            Start = new Position(1, 7),
            End = new Position(1, 15),
        };
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public void GetHoverInfo_HTML_PlainTextElement()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <p><$$strong></strong></p>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false);

        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreatePlainTextCapabilities());

        // Assert
        Assert.Null(hover);
    }

    [Fact]
    public void GetHoverInfo_HTML_PlainTextAttribute()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <p><strong class="$$weak"></strong></p>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false);

        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);

        // Act
        var hover = service.GetHoverInfo(codeDocument, location, CreatePlainTextCapabilities());

        // Assert
        Assert.Null(hover);
    }

    [Fact]
    public void GetHoverInfo_TagHelper_Element_VSClient_ReturnVSHover()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <$$test1></test1>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);
        var clientCapabilities = CreateMarkDownCapabilities();
        clientCapabilities.SupportsVisualStudioExtensions = true;

        // Act
        var vsHover = service.GetHoverInfo(codeDocument, location, clientCapabilities);

        // Assert
        Assert.False(vsHover.Contents.Value.TryGetFourth(out var _));
        Assert.True(vsHover.Contents.Value.TryGetThird(out var _) && !vsHover.Contents.Value.Third.Any());
        var expectedRange = new Range
        {
            Start = new Position(1, 1),
            End = new Position(1, 6),
        };
        Assert.Equal(expectedRange, vsHover.Range);

        var container = (ContainerElement)vsHover.RawContent;
        var containerElements = container.Elements.ToList();
        Assert.Equal(ContainerElementStyle.Stacked, container.Style);
        Assert.Single(containerElements);

        // [TagHelper Glyph] Test1TagHelper
        var innerContainer = ((ContainerElement)containerElements[0]).Elements.ToList();
        var classifiedTextElement = (ClassifiedTextElement)innerContainer[1];
        Assert.Equal(2, innerContainer.Count);
        Assert.Equal(ClassGlyph, innerContainer[0]);
        Assert.Collection(classifiedTextElement.Runs,
            run => DefaultVSLSPTagHelperTooltipFactoryTest.AssertExpectedClassification(run, "Test1TagHelper", VSPredefinedClassificationTypeNames.Type));
    }

    [Fact]
    public void GetHoverInfo_TagHelper_Attribute_VSClient_ReturnVSHover()
    {
        // Arrange
        var txt = """
                @addTagHelper *, TestAssembly
                <test1 $$bool-val='true'></test1>
                """;
        TestFileMarkupParser.GetPosition(txt, out txt, out var cursorPosition);

        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var service = GetDefaultRazorHoverInfoService();
        var location = new SourceLocation(cursorPosition, -1, -1);
        var clientCapabilities = CreateMarkDownCapabilities();
        clientCapabilities.SupportsVisualStudioExtensions = true;

        // Act
        var vsHover = service.GetHoverInfo(codeDocument, location, clientCapabilities);

        // Assert
        Assert.False(vsHover.Contents.Value.TryGetFourth(out var _));
        Assert.True(vsHover.Contents.Value.TryGetThird(out var markedStrings) && !markedStrings.Any());
        var expectedRange = new Range
        {
            Start = new Position(1, 7),
            End = new Position(1, 15),
        };
        Assert.Equal(expectedRange, vsHover.Range);

        var container = (ContainerElement)vsHover.RawContent;
        var containerElements = container.Elements.ToList();
        Assert.Equal(ContainerElementStyle.Stacked, container.Style);
        Assert.Single(containerElements);

        // [TagHelper Glyph] bool Test1TagHelper.BoolVal
        var innerContainer = ((ContainerElement)containerElements[0]).Elements.ToList();
        var classifiedTextElement = (ClassifiedTextElement)innerContainer[1];
        Assert.Equal(2, innerContainer.Count);
        Assert.Equal(PropertyGlyph, innerContainer[0]);
        Assert.Collection(classifiedTextElement.Runs,
            run => DefaultVSLSPTagHelperTooltipFactoryTest.AssertExpectedClassification(run, "bool", VSPredefinedClassificationTypeNames.Keyword),
            run => DefaultVSLSPTagHelperTooltipFactoryTest.AssertExpectedClassification(run, " ", VSPredefinedClassificationTypeNames.WhiteSpace),
            run => DefaultVSLSPTagHelperTooltipFactoryTest.AssertExpectedClassification(run, "Test1TagHelper", VSPredefinedClassificationTypeNames.Type),
            run => DefaultVSLSPTagHelperTooltipFactoryTest.AssertExpectedClassification(run, ".", VSPredefinedClassificationTypeNames.Punctuation),
            run => DefaultVSLSPTagHelperTooltipFactoryTest.AssertExpectedClassification(run, "BoolVal", VSPredefinedClassificationTypeNames.Identifier));
    }

    [Fact]
    public async Task Handle_Hover_SingleServer_CallsDelegatedLanguageServer()
    {
        // Arrange
        var languageServerFeatureOptions = Mock.Of<LanguageServerFeatureOptions>(options => options.SingleServerSupport == true, MockBehavior.Strict);

        var delegatedHover = new VSInternalHover();

        var languageServerMock = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServerMock
            .Setup(c => c.SendRequestAsync<IDelegatedParams, VSInternalHover>(RazorLanguageServerCustomMessageTargets.RazorHoverEndpointName, It.IsAny<DelegatedPositionParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(delegatedHover);

        var documentMappingServiceMock = new Mock<RazorDocumentMappingService>(MockBehavior.Strict);
        documentMappingServiceMock
            .Setup(c => c.GetLanguageKind(It.IsAny<RazorCodeDocument>(), It.IsAny<int>(), It.IsAny<bool>()))
            .Returns(RazorLanguageKind.CSharp);

        var outRange = new Range();
        documentMappingServiceMock
            .Setup(c => c.TryMapToProjectedDocumentRange(It.IsAny<RazorCodeDocument>(), It.IsAny<Range>(), out outRange))
            .Returns(true);

        var projectedPosition = new Position(1, 1);
        var projectedIndex = 1;
        documentMappingServiceMock.Setup(
            c => c.TryMapToProjectedDocumentPosition(It.IsAny<RazorCodeDocument>(), It.IsAny<int>(), out projectedPosition, out projectedIndex))
            .Returns(true);

        var endpoint = CreateEndpoint(languageServerFeatureOptions, documentMappingServiceMock.Object, languageServerMock.Object);

        var request = new TextDocumentPositionParamsBridge
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = new Uri("C:/text.razor")
            },
            Position = new Position(1, 0),
        };
        var documentContext = CreateDefaultDocumentContext();
        var requestContext = CreateRazorRequestContext(documentContext: documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Same(delegatedHover, result);
    }

    [Fact]
    public async Task Handle_Hover_SingleServer_CSharpVariable()
    {
        // Arrange
        var input = """
                <div></div>

                @{
                    var $$myVariable = "Hello";

                    var length = myVariable.Length;
                }
                """;

        // Act
        var result = await GetResultFromSingleServerEndpointAsync(input);

        // Assert
        var range = result.Range;
        var expected = new Range()
        {
            Start = new Position(line: 3, character: 8),
            End = new Position(line: 3, character: 18)
        };

        Assert.Equal(expected, range);

        var rawContainer = (ContainerElement)result.RawContent;
        var embeddedContainerElement = (ContainerElement)rawContainer.Elements.Single();

        var classifiedText = (ClassifiedTextElement)embeddedContainerElement.Elements.ElementAt(1);
        var text = string.Join("", classifiedText.Runs.Select(r => r.Text));

        // No need to validate exact text, in case the c# language server changes. Just
        // verify that the expected variable is represented within the hover text
        Assert.Contains("myVariable", text);
    }

    [Fact]
    public async Task Handle_Hover_SingleServer_Component()
    {
        // Arrange
        var input = """
                @addTagHelper *, TestAssembly

                <$$test1></test1>
                """;

        // Act
        var result = await GetResultFromSingleServerEndpointAsync(input);

        // Assert
        var range = result.Range;
        var expected = new Range()
        {
            Start = new Position(line: 2, character: 1),
            End = new Position(line: 2, character: 6)
        };

        Assert.Equal(expected, range);

        var rawContainer = (ContainerElement)result.RawContent;
        var embeddedContainerElement = (ContainerElement)rawContainer.Elements.Single();

        var classifiedText = (ClassifiedTextElement)embeddedContainerElement.Elements.ElementAt(1);
        var text = string.Join("", classifiedText.Runs.Select(r => r.Text));
        Assert.Equal("Test1TagHelper", text);
    }

    private async Task<VSInternalHover> GetResultFromSingleServerEndpointAsync(string input)
    {
        TestFileMarkupParser.GetPosition(input, out var output, out var cursorPosition);
        var codeDocument = CreateCodeDocument(output, DefaultTagHelpers);
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var csharpDocumentUri = new Uri("C:/path/to/file.razor__virtual.g.cs");
        var serverCapabilities = new ServerCapabilities()
        {
            HoverProvider = true
        };

        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, serverCapabilities, razorSpanMappingService: null, DisposalToken);
        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString());

        var razorFilePath = "C:/path/to/file.razor";
        var documentContextFactory = new TestDocumentContextFactory(razorFilePath, codeDocument, version: 1337);
        var languageServerFeatureOptions = Mock.Of<LanguageServerFeatureOptions>(options =>
            options.SupportsFileManipulation == true &&
            options.SingleServerSupport == true &&
            options.CSharpVirtualDocumentSuffix == ".g.cs" &&
            options.HtmlVirtualDocumentSuffix == ".g.html"
            , MockBehavior.Strict);
        var languageServer = new HoverLanguageServer(csharpServer, csharpDocumentUri, DisposalToken);
        var documentMappingService = new DefaultRazorDocumentMappingService(languageServerFeatureOptions, documentContextFactory, LoggerFactory);
        var projectSnapshotManager = Mock.Of<ProjectSnapshotManagerBase>(p => p.Projects == new[] { Mock.Of<ProjectSnapshot>(MockBehavior.Strict) }, MockBehavior.Strict);
        var hoverInfoService = GetDefaultRazorHoverInfoService();

        var endpoint = new RazorHoverEndpoint(
            hoverInfoService,
            languageServerFeatureOptions,
            documentMappingService,
            languageServer,
            LoggerFactory);

        var clientCapabilities = CreateMarkDownCapabilities();
        clientCapabilities.SupportsVisualStudioExtensions = true;

        _ = endpoint.GetRegistration(clientCapabilities);

        codeDocument.GetSourceText().GetLineAndOffset(cursorPosition, out var line, out var offset);
        var razorFileUri = new Uri(razorFilePath);
        var request = new TextDocumentPositionParamsBridge
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = razorFileUri,
            },
            Position = new Position(line, offset)
        };
        var documentContext = CreateDocumentContext(razorFileUri, codeDocument);
        var requestContext = CreateRazorRequestContext(documentContext: documentContext);

        return await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);
    }

    private DocumentContext CreateDefaultDocumentContext()
    {
        var txt = """
                @addTagHelper *, TestAssembly
                <any @test="Increment" />
                @code{
                    public void Increment(){
                    }
                }
                """;
        var path = "C:/text.razor";
        var codeDocument = CreateCodeDocument(txt, path, DefaultTagHelpers);
        var projectWorkspaceState = new ProjectWorkspaceState(DefaultTagHelpers, LanguageVersion.Default);
        var projectSnapshot = TestProjectSnapshot.Create("C:/project.csproj", projectWorkspaceState);
        var sourceText = SourceText.From(txt);

        var snapshot = Mock.Of<DocumentSnapshot>(d =>
            d.GetGeneratedOutputAsync() == Task.FromResult(codeDocument) &&
            d.FilePath == path &&
            d.FileKind == FileKinds.Component &&
            d.GetTextAsync() == Task.FromResult(sourceText) &&
            d.Project == projectSnapshot, MockBehavior.Strict);

        var documentContext = new DocumentContext(new Uri(path), snapshot, 1337);

        return documentContext;
    }

    private RazorHoverEndpoint CreateEndpoint(LanguageServerFeatureOptions languageServerFeatureOptions = null,
        RazorDocumentMappingService documentMappingService = null,
        ClientNotifierServiceBase languageServer = null)
    {

        languageServerFeatureOptions ??= Mock.Of<LanguageServerFeatureOptions>(options => options.SupportsFileManipulation == true && options.SingleServerSupport == false, MockBehavior.Strict);

        var documentMappingServiceMock = new Mock<RazorDocumentMappingService>(MockBehavior.Strict);
        documentMappingServiceMock
            .Setup(c => c.GetLanguageKind(It.IsAny<RazorCodeDocument>(), It.IsAny<int>(), It.IsAny<bool>()))
            .Returns(Protocol.RazorLanguageKind.Html);
        documentMappingService ??= documentMappingServiceMock.Object;

        languageServer ??= Mock.Of<ClientNotifierServiceBase>(MockBehavior.Strict);

        var endpoint = new RazorHoverEndpoint(
            GetDefaultRazorHoverInfoService(),
            languageServerFeatureOptions,
            documentMappingService,
            languageServer,
            LoggerFactory);

        return endpoint;
    }

    private DefaultRazorHoverInfoService GetDefaultRazorHoverInfoService()
    {
        var lspTagHelperTooltipFactory = new DefaultLSPTagHelperTooltipFactory();
        var vsLspTagHelperTooltipFactory = new DefaultVSLSPTagHelperTooltipFactory();
        return new DefaultRazorHoverInfoService(TagHelperFactsService, lspTagHelperTooltipFactory, vsLspTagHelperTooltipFactory, HtmlFactsService);
    }

    private class HoverLanguageServer : ClientNotifierServiceBase
    {
        private readonly CSharpTestLspServer _csharpServer;
        private readonly Uri _csharpDocumentUri;
        private readonly CancellationToken _cancellationToken;

        public HoverLanguageServer(
            CSharpTestLspServer csharpServer,
            Uri csharpDocumentUri,
            CancellationToken cancellationToken)
        {
            _csharpServer = csharpServer;
            _csharpDocumentUri = csharpDocumentUri;
            _cancellationToken = cancellationToken;
        }

        public override Task OnInitializedAsync(VSInternalClientCapabilities clientCapabilities, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public override Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task SendNotificationAsync(string method, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
        {
            Assert.Equal(RazorLanguageServerCustomMessageTargets.RazorHoverEndpointName, method);
            var hoverParams = Assert.IsType<DelegatedPositionParams>(@params);

            var hoverRequest = new TextDocumentPositionParams()
            {
                TextDocument = new TextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri
                },
                Position = hoverParams.ProjectedPosition
            };

            var result = await _csharpServer.ExecuteRequestAsync<TextDocumentPositionParams, TResponse>(
                Methods.TextDocumentHoverName, hoverRequest, _cancellationToken);

            return result;
        }
    }
}
