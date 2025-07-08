// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.CodeAnalysis.Razor.GoToDefinition;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Definition;

public class RazorComponentDefinitionHelpersTest(ITestOutputHelper testOutput) : TagHelperServiceTestBase(testOutput)
{
    [Fact]
    public void TryGetBoundTagHelpers_TagHelper_Element()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <te$$st1></test1>
            """;

        VerifyTryGetBoundTagHelpers(content, "Test1TagHelper", isRazorFile: false);
    }

    [Fact]
    public void TryGetBoundTagHelpers_TagHelper_StartTag_WithAttribute()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <Co$$mponent1 @test="Increment"></Component1>
            @code {
                public void Increment()
                {
                }
            }
            """;

        VerifyTryGetBoundTagHelpers(content, "Component1TagHelper");
    }

    [Fact]
    public void TryGetBoundTagHelpers_TagHelper_EndTag_WithAttribute()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <Component1 @test="Increment"></Comp$$onent1>
            @code {
                public void Increment()
                {
                }
            }
            """;

        VerifyTryGetBoundTagHelpers(content, "Component1TagHelper");
    }

    [Fact]
    public void TryGetBoundTagHelpers_TagHelper_Attribute_ReturnsNull()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <Component1 @te$$st="Increment"></Component1>
            @code {
                public void Increment()
                {
                }
            }
            """;

        VerifyTryGetBoundTagHelpers(content);
    }

    [Fact]
    public void TryGetBoundTagHelpers_TagHelper_AttributeValue_ReturnsNull()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <Component1 @test="Increm$$ent"></Component1>
            @code {
                public void Increment()
                {
                }
            }
            """;

        VerifyTryGetBoundTagHelpers(content);
    }

    [Fact]
    public void TryGetBoundTagHelpers_TagHelper_AfterAttributeEquals_ReturnsNull()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <Component1 @test="$$Increment"></Component1>
            @code {
                public void Increment()
                {
                }
            }
            """;

        VerifyTryGetBoundTagHelpers(content);
    }

    [Fact]
    public void TryGetBoundTagHelpers_TagHelper_AttributeEnd_ReturnsNull()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <Component1 @test="Increment">$$</Component1>
            @code {
                public void Increment()
                {
                }
            }
            """;

        VerifyTryGetBoundTagHelpers(content);
    }

    [Fact]
    public void TryGetBoundTagHelpers_TagHelper_MultipleAttributes()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <Co$$mponent1 @test="Increment" @minimized></Component1>
            @code {
                public void Increment()
                {
                }
            }
            """;

        VerifyTryGetBoundTagHelpers(content, "Component1TagHelper");
    }

    [Fact]
    public void TryGetBoundTagHelpers_TagHelper_MalformedElement()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <Co$$mponent1</Component1>
            @code {
                public void Increment()
                {
                }
            }
            """;

        VerifyTryGetBoundTagHelpers(content, "Component1TagHelper");
    }

    [Fact]
    public void TryGetBoundTagHelpers_TagHelper_MalformedAttribute()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <Co$$mponent1 @test="Increment></Component1>
            @code {
                public void Increment()
                {
                }
            }
            """;

        VerifyTryGetBoundTagHelpers(content, "Component1TagHelper");
    }

    [Fact]
    public void TryGetBoundTagHelpers_HTML_MarkupElement()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <p>
                <str$$ong></strong>
            </p>
            """;

        VerifyTryGetBoundTagHelpers(content);
    }

    [Fact]
    public void TryGetBoundTagHelpers_IgnoreAttribute_PropertyAttribute()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <Component1 boo$$l-val="true"></Component1>
            @code {
                public void Increment()
                {
                }
            }
            """;

        VerifyTryGetBoundTagHelpers(content, ignoreAttributes: true);
    }

    [Fact]
    public void TryGetBoundTagHelpers_TagHelper_PropertyAttribute()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <Component1 boo$$l-val="true"></Component1>
            @code {
                public void Increment()
                {
                }
            }
            """;

        VerifyTryGetBoundTagHelpers(content, "Component1TagHelper", "BoolVal");
    }

    [Fact]
    public void TryGetBoundTagHelpers_TagHelper_MinimizedPropertyAttribute()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <Component1 boo$$l-val></Component1>
            @code {
                public void Increment()
                {
                }
            }
            """;

        VerifyTryGetBoundTagHelpers(content, "Component1TagHelper", "BoolVal");
    }

    [Fact]
    public void TryGetBoundTagHelpers_TagHelper_MinimizedPropertyAttributeEdge1()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <Component1 $$bool-val></Component1>
            @code {
                public void Increment()
                {
                }
            }
            """;

        VerifyTryGetBoundTagHelpers(content, "Component1TagHelper", "BoolVal");
    }

    [Fact]
    public void TryGetBoundTagHelpers_TagHelper_MinimizedPropertyAttributeEdge2()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <Component1 bool-val$$></Component1>
            @code {
                public void Increment()
                {
                }
            }
            """;

        VerifyTryGetBoundTagHelpers(content, "Component1TagHelper", "BoolVal");
    }

    [Fact, WorkItem("https://github.com/dotnet/razor-tooling/issues/6775")]
    public void TryGetBoundTagHelpers_TagHelper_PropertyAttributeEdge()
    {
        var content = """
            @addTagHelper *, TestAssembly
            <Component1 bool-val$$="true"></Component1>
            @code {
                public void Increment()
                {
                }
            }
            """;

        VerifyTryGetBoundTagHelpers(content, "Component1TagHelper", "BoolVal");
    }

    [Fact]
    public async Task TryGetPropertyRangeAsync_TagHelperProperty_CorrectRange1()
    {
        var content = """
            <div>@Title</div>

            @code
            {
                [Parameter]
                public string NotTitle { get; set; }

                [Parameter]
                public string [|Title|] { get; set; }
            }
            """;

        await VerifyTryGetPropertyRangeAsync(content, "Title");
    }

    [Fact]
    public async Task TryGetPropertyRangeAsync_TagHelperProperty_CorrectRange2()
    {
        var content = """
            <div>@Title</div>

            @code
            {
                [Microsoft.AspNetCore.Components.Parameter]
                public string [|Title|] { get; set; }
            }
            """;

        await VerifyTryGetPropertyRangeAsync(content, "Title");
    }

    [Fact]
    public async Task TryGetPropertyRangeAsync_TagHelperProperty_CorrectRange3()
    {
        var content = """
            <div>@Title</div>

            @code
            {
                [Components.ParameterAttribute]
                public string [|Title|] { get; set; }
            }
            """;

        await VerifyTryGetPropertyRangeAsync(content, "Title");
    }

    [Fact]
    public async Task TryGetPropertyRangeAsync_TagHelperProperty_IgnoreInnerProperty()
    {
        var content = """
            <div>@Title</div>

            @code
            {
                private class NotTheDroidsYoureLookingFor
                {
                    public string Title { get; set; }
                }

                public string [|Title|] { get; set; }
            }
            """;

        await VerifyTryGetPropertyRangeAsync(content, "Title");
    }

    private void VerifyTryGetBoundTagHelpers(
        string content,
        string? tagHelperDescriptorName = null,
        string? attributeDescriptorPropertyName = null,
        bool isRazorFile = true,
        bool ignoreAttributes = false)
    {
        TestFileMarkupParser.GetPosition(content, out content, out var position);

        var codeDocument = CreateCodeDocument(content, isRazorFile);

        var result = RazorComponentDefinitionHelpers.TryGetBoundTagHelpers(codeDocument, position, ignoreAttributes, Logger, out var boundTagHelper, out var boundAttribute);

        if (tagHelperDescriptorName is null)
        {
            Assert.False(result);
        }
        else
        {
            Assert.True(result);
            Assert.NotNull(boundTagHelper);
            Assert.Equal(tagHelperDescriptorName, boundTagHelper.Name);
        }

        if (attributeDescriptorPropertyName is not null)
        {
            Assert.True(result);
            Assert.NotNull(boundAttribute);
            Assert.Equal(attributeDescriptorPropertyName, boundAttribute.GetPropertyName());
        }
    }

    private async Task VerifyTryGetPropertyRangeAsync(string content, string propertyName)
    {
        TestFileMarkupParser.GetSpan(content, out content, out var selection);

        var codeDocument = CreateCodeDocument(content);
        var expectedRange = codeDocument.Source.Text.GetRange(selection);
        var snapshot = TestDocumentSnapshot.Create("test.razor", codeDocument);

        var documentMappingService = new LspDocumentMappingService(FilePathService, new TestDocumentContextFactory(), LoggerFactory);

        var range = await RazorComponentDefinitionHelpers.TryGetPropertyRangeAsync(snapshot, propertyName, documentMappingService, Logger, DisposalToken);
        Assert.NotNull(range);
        Assert.Equal(expectedRange, range);
    }

    private RazorCodeDocument CreateCodeDocument(string content, bool isRazorFile = true)
        => CreateCodeDocument(content, isRazorFile, DefaultTagHelpers);
}
