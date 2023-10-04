// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Definition;

public class DefinitionEndpointTest : TagHelperServiceTestBase
{
    public DefinitionEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task GetOriginTagHelperBindingAsync_TagHelper_Element()
    {
        var content = """
                @addTagHelper *, TestAssembly
                <te$$st1></test1>
                """;

        await VerifyOriginTagHelperBindingAsync(content, "Test1TagHelper", isRazorFile: false);
    }

    [Fact]
    public async Task GetOriginTagHelperBindingAsync_TagHelper_StartTag_WithAttribute()
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

        await VerifyOriginTagHelperBindingAsync(content, "Component1TagHelper");
    }

    [Fact]
    public async Task GetOriginTagHelperBindingAsync_TagHelper_EndTag_WithAttribute()
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

        await VerifyOriginTagHelperBindingAsync(content, "Component1TagHelper");
    }

    [Fact]
    public async Task GetOriginTagHelperBindingAsync_TagHelper_Attribute_ReturnsNull()
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

        await VerifyOriginTagHelperBindingAsync(content);
    }

    [Fact]
    public async Task GetOriginTagHelperBindingAsync_TagHelper_AttributeValue_ReturnsNull()
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

        await VerifyOriginTagHelperBindingAsync(content);
    }

    [Fact]
    public async Task GetOriginTagHelperBindingAsync_TagHelper_AfterAttributeEquals_ReturnsNull()
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

        await VerifyOriginTagHelperBindingAsync(content);
    }

    [Fact]
    public async Task GetOriginTagHelperBindingAsync_TagHelper_AttributeEnd_ReturnsNull()
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

        await VerifyOriginTagHelperBindingAsync(content);
    }

    [Fact]
    public async Task GetOriginTagHelperBindingAsync_TagHelper_MultipleAttributes()
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

        await VerifyOriginTagHelperBindingAsync(content, "Component1TagHelper");
    }

    [Fact]
    public async Task GetOriginTagHelperBindingAsync_TagHelper_MalformedElement()
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

        await VerifyOriginTagHelperBindingAsync(content, "Component1TagHelper");
    }

    [Fact]
    public async Task GetOriginTagHelperBindingAsync_TagHelper_MalformedAttribute()
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

        await VerifyOriginTagHelperBindingAsync(content, "Component1TagHelper");
    }

    [Fact]
    public async Task GetOriginTagHelperBindingAsync_HTML_MarkupElement()
    {
        var content = """
                @addTagHelper *, TestAssembly
                <p>
                    <str$$ong></strong>
                </p>
                """;

        await VerifyOriginTagHelperBindingAsync(content);
    }

    [Fact]
    public async Task GetOriginTagHelperBindingAsync_IgnoreAttribute_PropertyAttribute()
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

        await VerifyOriginTagHelperBindingAsync(content, ignoreAttributes: true, tagHelperDescriptorName: null, attributeDescriptorPropertyName: null);
    }

    [Fact]
    public async Task GetOriginTagHelperBindingAsync_TagHelper_PropertyAttribute()
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

        await VerifyOriginTagHelperBindingAsync(content, "Component1TagHelper", "BoolVal");
    }

    [Fact]
    public async Task GetOriginTagHelperBindingAsync_TagHelper_MinimizedPropertyAttribute()
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

        await VerifyOriginTagHelperBindingAsync(content, "Component1TagHelper", "BoolVal");
    }

    [Fact]
    public async Task GetOriginTagHelperBindingAsync_TagHelper_MinimizedPropertyAttributeEdge1()
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

        await VerifyOriginTagHelperBindingAsync(content, "Component1TagHelper", "BoolVal");
    }

    [Fact]
    public async Task GetOriginTagHelperBindingAsync_TagHelper_MinimizedPropertyAttributeEdge2()
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

        await VerifyOriginTagHelperBindingAsync(content, "Component1TagHelper", "BoolVal");
    }

    [Fact, WorkItem("https://github.com/dotnet/razor-tooling/issues/6775")]
    public async Task GetOriginTagHelperBindingAsync_TagHelper_PropertyAttributeEdge()
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

        await VerifyOriginTagHelperBindingAsync(content, "Component1TagHelper", "BoolVal");
    }

    [Fact]
    public async Task GetNavigatePositionAsync_TagHelperProperty_CorrectRange1()
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

        await VerifyNavigatePositionAsync(content, "Title");
    }

    [Fact]
    public async Task GetNavigatePositionAsync_TagHelperProperty_CorrectRange2()
    {
        var content = """
                <div>@Title</div>

                @code
                {
                    [Microsoft.AspNetCore.Components.Parameter]
                    public string [|Title|] { get; set; }
                }
                """;

        await VerifyNavigatePositionAsync(content, "Title");
    }

    [Fact]
    public async Task GetNavigatePositionAsync_TagHelperProperty_CorrectRange3()
    {
        var content = """
                <div>@Title</div>

                @code
                {
                    [Components.ParameterAttribute]
                    public string [|Title|] { get; set; }
                }
                """;

        await VerifyNavigatePositionAsync(content, "Title");
    }

    [Fact]
    public async Task GetNavigatePositionAsync_TagHelperProperty_IgnoreInnerProperty()
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

        await VerifyNavigatePositionAsync(content, "Title");
    }

    #region Helpers
    private async Task VerifyOriginTagHelperBindingAsync(string content, string tagHelperDescriptorName = null, string attributeDescriptorPropertyName = null, bool isRazorFile = true, bool ignoreAttributes = false)
    {
        TestFileMarkupParser.GetPosition(content, out content, out var position);

        SetupDocument(out _, out var documentSnapshot, content, isRazorFile);
        var documentContext = CreateDocumentContext(new Uri(@"C:\file.razor"), documentSnapshot);

        var (descriptor, attributeDescriptor) = await DefinitionEndpoint.GetOriginTagHelperBindingAsync(
            documentContext, position, ignoreAttributes, LoggerFactory.CreateLogger("RazorDefinitionEndpoint"), DisposalToken);

        if (tagHelperDescriptorName is null)
        {
            Assert.Null(descriptor);
        }
        else
        {
            Assert.NotNull(descriptor);
            Assert.Equal(tagHelperDescriptorName, descriptor!.Name);
        }

        if (attributeDescriptorPropertyName is null)
        {
            Assert.Null(attributeDescriptor);
        }
        else
        {
            Assert.NotNull(attributeDescriptor);
            Assert.Equal(attributeDescriptorPropertyName, attributeDescriptor.GetPropertyName());
        }
    }

    private async Task VerifyNavigatePositionAsync(string content, string propertyName)
    {
        TestFileMarkupParser.GetSpan(content, out content, out var selection);

        SetupDocument(out var codeDocument, out _, content);
        var expectedRange = selection.ToRange(codeDocument.GetSourceText());

        var mappingService = new RazorDocumentMappingService(FilePathService, new TestDocumentContextFactory(), LoggerFactory);

        var range = await DefinitionEndpoint.TryGetPropertyRangeAsync(codeDocument, propertyName, mappingService, Logger, DisposalToken);
        Assert.NotNull(range);
        Assert.Equal(expectedRange, range);
    }

    private void SetupDocument(out RazorCodeDocument codeDocument, out IDocumentSnapshot documentSnapshot, string content, bool isRazorFile = true)
    {
        var sourceText = SourceText.From(content);
        codeDocument = CreateCodeDocument(content, isRazorFile, DefaultTagHelpers);
        var outDoc = codeDocument;
        documentSnapshot = Mock.Of<IDocumentSnapshot>(
            d => d.GetTextAsync() == Task.FromResult(sourceText),
            MockBehavior.Strict);
        Mock.Get(documentSnapshot)
            .Setup(s => s.GetGeneratedOutputAsync())
            .ReturnsAsync(outDoc);
    }
    #endregion
}
