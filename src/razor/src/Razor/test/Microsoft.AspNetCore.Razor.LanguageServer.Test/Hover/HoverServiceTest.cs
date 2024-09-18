// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.Hover;
using Microsoft.AspNetCore.Razor.LanguageServer.Tooltip;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text.Adornments;
using Moq;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.LanguageServer.Tooltip.DefaultVSLSPTagHelperTooltipFactory;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Hover;

public class HoverServiceTest(ITestOutputHelper testOutput) : TagHelperServiceTestBase(testOutput)
{
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
                    ContentFormat = [markupKind],
                }
            }
        };

    [Fact]
    public async Task GetHoverInfo_TagHelper_Element()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <$$test1></test1>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);
        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreateMarkDownCapabilities(), DisposalToken);

        // Assert
        Assert.NotNull(hover);
        Assert.NotNull(hover.Contents);
        Assert.Contains("**Test1TagHelper**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 1, length: 5);
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_Element_WithParent()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <test1>
                <Som$$eChild></SomeChild>
            </test1>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);
        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreateMarkDownCapabilities(), DisposalToken);

        // Assert
        Assert.NotNull(hover);
        Assert.NotNull(hover.Contents);
        Assert.Contains("**SomeChild**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 2, character: 5, length: 9);
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_Attribute_WithParent()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <test1>
                <SomeChild [|att$$ribute|]="test"></SomeChild>
            </test1>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);
        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreateMarkDownCapabilities(), DisposalToken);

        // Assert
        Assert.NotNull(hover);
        Assert.NotNull(hover.Contents);
        Assert.Contains("**Attribute**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = codeDocument.Source.Text.GetRange(code.Span);
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_Element_EndTag()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <test1></$$test1>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);
        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreateMarkDownCapabilities(), DisposalToken);

        // Assert
        Assert.NotNull(hover);
        Assert.NotNull(hover.Contents);
        Assert.Contains("**Test1TagHelper**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 9, length: 5);
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_Attribute()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <test1 $$bool-val='true'></test1>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);
        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreateMarkDownCapabilities(), DisposalToken);

        // Assert
        Assert.NotNull(hover);
        Assert.NotNull(hover.Contents);
        Assert.Contains("**BoolVal**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.DoesNotContain("**IntVal**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 7, length: 8);
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_AttributeTrailingEdge()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <test1 bool-val$$ minimized></test1>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);
        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var edgeLocation = code.Position;
        var location = new SourceLocation(edgeLocation, lineIndex: 0, edgeLocation);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreateMarkDownCapabilities(), DisposalToken);

        // Assert
        Assert.NotNull(hover);
        Assert.NotNull(hover.Contents);
        Assert.Contains("**BoolVal**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.DoesNotContain("**IntVal**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 7, length: 8);
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_AttributeValue_ReturnsNull()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <test1 bool-val='$$true'></test1>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);
        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreateMarkDownCapabilities(), DisposalToken);

        // Assert
        Assert.Null(hover);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_AfterAttributeEquals_ReturnsNull()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <test1 bool-val=$$'true'></test1>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);
        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreateMarkDownCapabilities(), DisposalToken);

        // Assert
        Assert.Null(hover);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_AttributeEnd_ReturnsNull()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <test1 bool-val='true'$$></test1>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);
        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreateMarkDownCapabilities(), DisposalToken);

        // Assert
        Assert.Null(hover);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_MinimizedAttribute()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <test1 $$bool-val></test1>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);
        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreateMarkDownCapabilities(), DisposalToken);

        // Assert
        Assert.NotNull(hover);
        Assert.NotNull(hover.Contents);
        Assert.Contains("**BoolVal**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.DoesNotContain("**IntVal**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 7, length: 8);
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public async Task GetHoverInfo_DirectiveAttribute_HasResult()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <any @t$$est="Increment" />
            @code{
                public void Increment(){
                }
            }
            """;

        var codeDocument = CreateCodeDocument(code.Text, "text.razor", DefaultTagHelpers);
        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreateMarkDownCapabilities(), DisposalToken);

        // Assert
        Assert.NotNull(hover);
        Assert.NotNull(hover.Contents);
        Assert.Contains("**Test**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 5, length: 5);
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_MalformedElement()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <$$test1<hello
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);
        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreateMarkDownCapabilities(), DisposalToken);

        // Assert
        Assert.NotNull(hover);
        Assert.NotNull(hover.Contents);
        Assert.Contains("**Test1TagHelper**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 1, length: 5);
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_MalformedAttribute()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <test1 $$bool-val=\"aslj alsk<strong>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);
        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreateMarkDownCapabilities(), DisposalToken);

        // Assert
        Assert.NotNull(hover);
        Assert.NotNull(hover.Contents);
        Assert.Contains("**BoolVal**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.DoesNotContain("**IntVal**", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 7, length: 8);
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public async Task GetHoverInfo_HTML_MarkupElement()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <p><$$strong></strong></p>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false);
        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreateMarkDownCapabilities(), DisposalToken);

        // Assert
        Assert.Null(hover);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_PlainTextElement()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <$$test1></test1>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);

        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreatePlainTextCapabilities(), DisposalToken);

        // Assert
        Assert.NotNull(hover);
        Assert.NotNull(hover.Contents);
        Assert.Contains("Test1TagHelper", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.Equal(MarkupKind.PlainText, ((MarkupContent)hover.Contents).Kind);
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 1, length: 5);
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_PlainTextElement_EndTag()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <test1></$$test1>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);

        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreatePlainTextCapabilities(), DisposalToken);

        // Assert
        Assert.NotNull(hover);
        Assert.NotNull(hover.Contents);
        Assert.Contains("Test1TagHelper", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.Equal(MarkupKind.PlainText, ((MarkupContent)hover.Contents).Kind);
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 9, length: 5);
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_TextComponent()
    {
        // Arrange
        TestCode code = """
            <$$Text></Text>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: true, DefaultTagHelpers);

        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.razor", codeDocument, location, CreatePlainTextCapabilities(), DisposalToken);

        // Assert
        Assert.NotNull(hover);
        Assert.NotNull(hover.Contents);
        Assert.Contains("Text", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.Equal(MarkupKind.PlainText, ((MarkupContent)hover.Contents).Kind);
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 0, character: 1, length: 4);
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_TextComponent_NestedInHtml()
    {
        // Arrange
        TestCode code = """
            <div>
                <$$Text></Text>
            </div>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: true, DefaultTagHelpers);

        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.razor", codeDocument, location, CreatePlainTextCapabilities(), DisposalToken);

        // Assert
        Assert.NotNull(hover);
        Assert.NotNull(hover.Contents);
        Assert.Contains("Text", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.Equal(MarkupKind.PlainText, ((MarkupContent)hover.Contents).Kind);
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 5, length: 4);
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_TextComponent_NestedInCSharp()
    {
        // Arrange
        TestCode code = """
            @if (true)
            {
                <$$Text></Text>
            }
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: true, DefaultTagHelpers);

        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.razor", codeDocument, location, CreatePlainTextCapabilities(), DisposalToken);

        // Assert
        Assert.Null(hover);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_TextComponent_NestedInCSharpAndText()
    {
        // Arrange
        TestCode code = """
            @if (true)
            {
                <text>
                    <$$Text></Text>
                </text>
            }
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: true, DefaultTagHelpers);

        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.razor", codeDocument, location, CreatePlainTextCapabilities(), DisposalToken);

        // Assert
        Assert.NotNull(hover);
        Assert.NotNull(hover.Contents);
        Assert.Contains("Text", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.Equal(MarkupKind.PlainText, ((MarkupContent)hover.Contents).Kind);
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 3, character: 9, length: 4);
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_PlainTextAttribute()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <test1 $$bool-val></test1>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);

        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreatePlainTextCapabilities(), DisposalToken);

        // Assert
        Assert.NotNull(hover);
        Assert.NotNull(hover.Contents);
        Assert.Contains("BoolVal", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.DoesNotContain("IntVal", ((MarkupContent)hover.Contents).Value, StringComparison.Ordinal);
        Assert.Equal(MarkupKind.PlainText, ((MarkupContent)hover.Contents).Kind);
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 7, length: 8);
        Assert.Equal(expectedRange, hover.Range);
    }

    [Fact]
    public async Task GetHoverInfo_HTML_PlainTextElement()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <p><$$strong></strong></p>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false);

        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreatePlainTextCapabilities(), DisposalToken);

        // Assert
        Assert.Null(hover);
    }

    [Fact]
    public async Task GetHoverInfo_HTML_PlainTextAttribute()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <p><strong class="$$weak"></strong></p>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false);

        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();

        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);

        // Act
        var hover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, CreatePlainTextCapabilities(), DisposalToken);

        // Assert
        Assert.Null(hover);
    }

    [Fact]
    public async Task GetHoverInfo_TagHelper_Element_VSClient_ReturnVSHover()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <$$test1></test1>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);
        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);
        var clientCapabilities = CreateMarkDownCapabilities();
        clientCapabilities.SupportsVisualStudioExtensions = true;

        // Act
        var vsHover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, clientCapabilities, DisposalToken);

        // Assert
        Assert.NotNull(vsHover);
        Assert.NotNull(vsHover.Contents);
        Assert.False(vsHover.Contents.Value.TryGetFourth(out var _));
        Assert.True(vsHover.Contents.Value.TryGetThird(out var _) && !vsHover.Contents.Value.Third.Any());
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 1, length: 5);
        Assert.Equal(expectedRange, vsHover.Range);

        Assert.NotNull(vsHover.RawContent);
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
    public async Task GetHoverInfo_TagHelper_Attribute_VSClient_ReturnVSHover()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, TestAssembly
            <test1 $$bool-val='true'></test1>
            """;

        var codeDocument = CreateCodeDocument(code.Text, isRazorFile: false, DefaultTagHelpers);

        var service = GetHoverService();
        var serviceAccessor = service.GetTestAccessor();
        var location = new SourceLocation(code.Position, lineIndex: -1, characterIndex: -1);
        var clientCapabilities = CreateMarkDownCapabilities();
        clientCapabilities.SupportsVisualStudioExtensions = true;

        // Act
        var vsHover = await serviceAccessor.GetHoverInfoAsync("file.cshtml", codeDocument, location, clientCapabilities, DisposalToken);

        // Assert
        Assert.NotNull(vsHover);
        Assert.NotNull(vsHover.Contents);
        Assert.False(vsHover.Contents.Value.TryGetFourth(out _));
        Assert.True(vsHover.Contents.Value.TryGetThird(out var markedStrings) && !markedStrings.Any());
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 7, length: 8);
        Assert.Equal(expectedRange, vsHover.Range);

        Assert.NotNull(vsHover.RawContent);
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
        var languageServerFeatureOptions = StrictMock.Of<LanguageServerFeatureOptions>(options =>
            options.SingleServerSupport == true &&
            options.UseRazorCohostServer == false);

        var delegatedHover = new VSInternalHover();

        var clientConnection = TestMocks.CreateClientConnection(builder =>
        {
            builder.SetupSendRequest<IDelegatedParams, VSInternalHover>(CustomMessageNames.RazorHoverEndpointName, response: delegatedHover);
        });

        var documentMappingServiceMock = new StrictMock<IDocumentMappingService>();

        var outRange = new LinePositionSpan();
        documentMappingServiceMock
            .Setup(x => x.TryMapToGeneratedDocumentRange(It.IsAny<IRazorGeneratedDocument>(), It.IsAny<LinePositionSpan>(), out outRange))
            .Returns(true);

        var projectedPosition = new LinePosition(1, 1);
        var projectedIndex = 1;
        documentMappingServiceMock
            .Setup(x => x.TryMapToGeneratedDocumentPosition(It.IsAny<IRazorGeneratedDocument>(), It.IsAny<int>(), out projectedPosition, out projectedIndex))
            .Returns(true);

        var endpoint = CreateEndpoint(languageServerFeatureOptions, documentMappingServiceMock.Object, clientConnection);

        var (documentContext, position) = CreateDefaultDocumentContext();
        var requestContext = CreateRazorRequestContext(documentContext);

        var request = new TextDocumentPositionParams
        {
            TextDocument = new() { Uri = new Uri("C:/text.razor") },
            Position = position,
        };

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Same(delegatedHover, result);
    }

    [Fact]
    public async Task Handle_Hover_SingleServer_CSharpVariable()
    {
        // Arrange
        TestCode code = """
            <div></div>

            @{
                var $$myVariable = "Hello";

                var length = myVariable.Length;
            }
            """;

        // Act
        var result = await GetResultFromSingleServerEndpointAsync(code);

        // Assert
        Assert.NotNull(result);
        var range = result.Range;
        var expected = VsLspFactory.CreateSingleLineRange(line: 3, character: 8, length: 10);

        Assert.Equal(expected, range);

        Assert.NotNull(result.RawContent);
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
        TestCode code = """
            @addTagHelper *, TestAssembly

            <$$test1></test1>
            """;

        // Act
        var result = await GetResultFromSingleServerEndpointAsync(code);

        // Assert
        Assert.NotNull(result);
        var range = result.Range;
        var expected = VsLspFactory.CreateSingleLineRange(line: 2, character: 1, length: 5);

        Assert.Equal(expected, range);

        Assert.NotNull(result.RawContent);
        var rawContainer = (ContainerElement)result.RawContent;
        var embeddedContainerElement = (ContainerElement)rawContainer.Elements.Single();

        var classifiedText = (ClassifiedTextElement)embeddedContainerElement.Elements.ElementAt(1);
        var text = string.Join("", classifiedText.Runs.Select(r => r.Text));
        Assert.Equal("Test1TagHelper", text);
    }

    [Fact]
    public async Task Handle_Hover_SingleServer_AddTagHelper()
    {
        // Arrange
        TestCode code = """
            @addTagHelper *, Test$$Assembly

            <test1></test1>
            """;

        // Act
        var result = await GetResultFromSingleServerEndpointAsync(code);

        // Assert

        // Roslyn returns us a range that is outside of our source mappings, so we expect the endpoint
        // to return null, so as not to confuse the client
        Assert.NotNull(result);
        Assert.Null(result.Range);

        Assert.NotNull(result.RawContent);
        var rawContainer = (ContainerElement)result.RawContent;
        var embeddedContainerElement = (ContainerElement)rawContainer.Elements.Single();

        if (embeddedContainerElement.Elements.FirstOrDefault() is ContainerElement headerContainer)
        {
            embeddedContainerElement = headerContainer;
        }

        var classifiedText = (ClassifiedTextElement)embeddedContainerElement.Elements.ElementAt(1);
        var text = string.Join("", classifiedText.Runs.Select(r => r.Text));
        // Hover info is for a string
        Assert.StartsWith("class System.String", text);
    }

    private async Task<VSInternalHover?> GetResultFromSingleServerEndpointAsync(TestCode code)
    {
        var codeDocument = CreateCodeDocument(code.Text, DefaultTagHelpers);
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var csharpDocumentUri = new Uri("C:/path/to/file.razor__virtual.g.cs");
        var serverCapabilities = new VSInternalServerCapabilities()
        {
            HoverProvider = true
        };

        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, serverCapabilities, razorSpanMappingService: null, capabilitiesUpdater: null, DisposalToken);
        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString());

        var razorFilePath = "C:/path/to/file.razor";
        var documentContextFactory = new TestDocumentContextFactory(razorFilePath, codeDocument);
        var languageServerFeatureOptions = StrictMock.Of<LanguageServerFeatureOptions>(options =>
            options.SupportsFileManipulation == true &&
            options.SingleServerSupport == true &&
            options.CSharpVirtualDocumentSuffix == ".g.cs" &&
            options.HtmlVirtualDocumentSuffix == ".g.html" &&
            options.UseRazorCohostServer == false);
        var languageServer = new HoverLanguageServer(csharpServer, csharpDocumentUri, DisposalToken);
        var documentMappingService = new LspDocumentMappingService(FilePathService, documentContextFactory, LoggerFactory);

        var service = GetHoverService(documentMappingService);

        var endpoint = new HoverEndpoint(
            service,
            languageServerFeatureOptions,
            documentMappingService,
            languageServer,
            LoggerFactory);

        var razorFileUri = new Uri(razorFilePath);
        var request = new TextDocumentPositionParams
        {
            TextDocument = new() { Uri = razorFileUri, },
            Position = codeDocument.Source.Text.GetPosition(code.Position)
        };

        var documentContext = CreateDocumentContext(razorFileUri, codeDocument);
        var requestContext = CreateRazorRequestContext(documentContext: documentContext);

        return await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);
    }

    private (DocumentContext, Position) CreateDefaultDocumentContext()
    {
        TestCode code = """
            @addTagHelper *, TestAssembly
            <any @test="Increment" />
            @code{
                public void $$Increment(){
                }
            }
            """;

        var path = "C:/text.razor";
        var codeDocument = CreateCodeDocument(code.Text, path, DefaultTagHelpers);
        var projectWorkspaceState = ProjectWorkspaceState.Create(DefaultTagHelpers);
        var projectSnapshot = TestProjectSnapshot.Create("C:/project.csproj", projectWorkspaceState);

        var documentSnapshotMock = new StrictMock<IDocumentSnapshot>();
        documentSnapshotMock
            .Setup(x => x.GetGeneratedOutputAsync(It.IsAny<bool>()))
            .ReturnsAsync(codeDocument);
        documentSnapshotMock
            .Setup(x => x.GetTextAsync())
            .ReturnsAsync(codeDocument.Source.Text);
        documentSnapshotMock
            .SetupGet(x => x.FilePath)
            .Returns(path);
        documentSnapshotMock
            .SetupGet(x => x.FileKind)
            .Returns(FileKinds.Component);
        documentSnapshotMock
            .SetupGet(x => x.Version)
            .Returns(0);
        documentSnapshotMock
            .SetupGet(x => x.Project)
            .Returns(projectSnapshot);

        var documentContext = new DocumentContext(new Uri(path), documentSnapshotMock.Object, projectContext: null);
        var position = codeDocument.Source.Text.GetPosition(code.Position);

        return (documentContext, position);
    }

    private HoverEndpoint CreateEndpoint(
        LanguageServerFeatureOptions? languageServerFeatureOptions = null,
        IDocumentMappingService? documentMappingService = null,
        IClientConnection? clientConnection = null)
    {
        languageServerFeatureOptions ??= StrictMock.Of<LanguageServerFeatureOptions>(options =>
            options.SupportsFileManipulation == true &&
            options.SingleServerSupport == false);

        documentMappingService ??= StrictMock.Of<IDocumentMappingService>();

        clientConnection ??= StrictMock.Of<IClientConnection>();

        var service = GetHoverService();

        var endpoint = new HoverEndpoint(
            service,
            languageServerFeatureOptions,
            documentMappingService,
            clientConnection,
            LoggerFactory);

        return endpoint;
    }

    private HoverService GetHoverService(IDocumentMappingService? mappingService = null)
    {
        var projectManager = CreateProjectSnapshotManager();
        var lspTagHelperTooltipFactory = new DefaultLSPTagHelperTooltipFactory(projectManager);
        var vsLspTagHelperTooltipFactory = new DefaultVSLSPTagHelperTooltipFactory(projectManager);

        var clientCapabilities = CreateMarkDownCapabilities();
        clientCapabilities.SupportsVisualStudioExtensions = true;
        var clientCapabilitiesService = new TestClientCapabilitiesService(clientCapabilities);

        mappingService ??= StrictMock.Of<IDocumentMappingService>();

        return new HoverService(lspTagHelperTooltipFactory, vsLspTagHelperTooltipFactory, mappingService, clientCapabilitiesService);
    }

    private class HoverLanguageServer : IClientConnection
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

        public Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SendNotificationAsync(string method, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
        {
            Assert.Equal(CustomMessageNames.RazorHoverEndpointName, method);
            var hoverParams = Assert.IsType<DelegatedPositionParams>(@params);

            var hoverRequest = new TextDocumentPositionParams()
            {
                TextDocument = new() { Uri = _csharpDocumentUri, },
                Position = hoverParams.ProjectedPosition
            };

            var result = await _csharpServer.ExecuteRequestAsync<TextDocumentPositionParams, TResponse>(
                Methods.TextDocumentHoverName, hoverRequest, _cancellationToken);

            return result;
        }
    }
}
