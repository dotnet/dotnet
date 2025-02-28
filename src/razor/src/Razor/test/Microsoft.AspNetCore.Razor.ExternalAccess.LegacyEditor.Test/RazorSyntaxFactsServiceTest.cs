// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor.Test;

public class RazorSyntaxFactsServiceTest(ITestOutputHelper testOutput) : RazorToolingProjectEngineTestBase(testOutput)
{
    protected override RazorLanguageVersion Version => RazorLanguageVersion.Latest;

    [Fact]
    public void GetClassifiedSpans_ReturnsExpectedSpans()
    {
        // Arrange
        var expectedSpans = new[]
        {
            new ClassifiedSpan(new RazorSourceSpan("test.cshtml", 0, 0, 0, 5), new RazorSourceSpan("test.cshtml", 0, 0, 0, 5), SpanKind.Markup, BlockKind.Tag, AcceptedCharacters.Any),
            new ClassifiedSpan(new RazorSourceSpan("test.cshtml", 5, 0, 5, 6), new RazorSourceSpan("test.cshtml", 0, 0, 0, 42), SpanKind.Markup, BlockKind.Markup, AcceptedCharacters.Any),
            new ClassifiedSpan(new RazorSourceSpan("test.cshtml", 34, 1, 27, 2), new RazorSourceSpan("test.cshtml", 0, 0, 0, 42), SpanKind.Markup, BlockKind.Markup, AcceptedCharacters.Any),
            new ClassifiedSpan(new RazorSourceSpan("test.cshtml", 36, 2, 0, 6), new RazorSourceSpan("test.cshtml", 36, 2, 0, 6), SpanKind.Markup, BlockKind.Tag, AcceptedCharacters.Any),
        };

        var codeDocument = GetCodeDocument(
@"<div>
    <taghelper></taghelper>
</div>");

        // Act
        var spans = codeDocument.GetClassifiedSpans();

        // Assert
        Assert.Equal(expectedSpans, spans);
    }

    [Fact]
    public void GetClassifiedSpans_ReturnsAttributeSpansInDocumentOrder()
    {
        // Arrange
        var expectedSpans = new[]
        {
            new ClassifiedSpan(new RazorSourceSpan("test.cshtml", 14, 0, 14, 1), new RazorSourceSpan("test.cshtml", 0, 0, 0, 49), SpanKind.Code, BlockKind.Tag, AcceptedCharacters.AnyExceptNewline),
            new ClassifiedSpan(new RazorSourceSpan("test.cshtml", 23, 0, 23, 2), new RazorSourceSpan("test.cshtml", 0, 0, 0, 49), SpanKind.Markup, BlockKind.Tag, AcceptedCharacters.Any),
            new ClassifiedSpan(new RazorSourceSpan("test.cshtml", 32, 0, 32, 4), new RazorSourceSpan("test.cshtml", 0, 0, 0, 49), SpanKind.Code, BlockKind.Tag, AcceptedCharacters.AnyExceptNewline),
        };

        var codeDocument = GetCodeDocument(
@"<taghelper id=1 class=""th"" show=true></taghelper>");

        // Act
        var spans = codeDocument.GetClassifiedSpans();

        // Assert
        Assert.Equal(expectedSpans, spans);
    }

    [Fact]
    public void GetTagHelperSpans_ReturnsExpectedSpans()
    {
        // Arrange
        var codeDocument = GetCodeDocument(
@"<div>
    <taghelper></taghelper>
</div>");

        var tagHelperContext = codeDocument.GetTagHelperContext();
        var expectedSourceSpan = new RazorSourceSpan("test.cshtml", 11, 1, 4, 23);

        // Act
        var spans = codeDocument.GetTagHelperSpans();

        // Assert
        var actualSpan = Assert.Single(spans);
        Assert.Equal(expectedSourceSpan, actualSpan.Span);
        Assert.Equal<IRazorTagHelperDescriptor>(tagHelperContext.TagHelpers, actualSpan.TagHelpers);
    }

    private IRazorCodeDocument GetCodeDocument(string source)
    {
        var taghelper = TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly")
            .BoundAttributeDescriptor(attr => attr.Name("show").TypeName("System.Boolean"))
            .BoundAttributeDescriptor(attr => attr.Name("id").TypeName("System.Int32"))
            .TagMatchingRuleDescriptor(rule => rule.RequireTagName("taghelper"))
            .Metadata(TypeName("TestTagHelper"))
            .Build();

        var engine = CreateProjectEngine(builder =>
        {
            builder.ConfigureParserOptions(builder =>
            {
                builder.EnableSpanEditHandlers = true;
            });
        });

        var sourceDocument = TestRazorSourceDocument.Create(source, normalizeNewLines: true);
        var importDocument = TestRazorSourceDocument.Create("@addTagHelper *, TestAssembly", filePath: "import.cshtml", relativePath: "import.cshtml");

        var codeDocument = engine.ProcessDesignTime(sourceDocument, FileKinds.Legacy, importSources: ImmutableArray.Create(importDocument), new []{ taghelper });

        return RazorWrapperFactory.WrapCodeDocument(codeDocument);
    }
}
