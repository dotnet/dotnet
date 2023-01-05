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

public class RazorDefinitionEndpointTest : TagHelperServiceTestBase
{
    public RazorDefinitionEndpointTest(ITestOutputHelper testOutput)
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

    #region Helpers
    private async Task VerifyOriginTagHelperBindingAsync(string content, string tagHelperDescriptorName = null, bool isRazorFile = true)
    {
        TestFileMarkupParser.GetPosition(content, out content, out var position);

        SetupDocument(out _, out var documentSnapshot, content, isRazorFile);
        var documentContext = CreateDocumentContext(new Uri(@"C:\file.razor"), documentSnapshot);

        var descriptor = await RazorDefinitionEndpoint.GetOriginTagHelperBindingAsync(documentContext, position, LoggerFactory.CreateLogger("RazorDefinitionEndpoint"), DisposalToken);

        if (tagHelperDescriptorName is null)
        {
            Assert.Null(descriptor);
        }
        else
        {
            Assert.NotNull(descriptor);
            Assert.Equal(tagHelperDescriptorName, descriptor!.Name);
        }
    }

    private void SetupDocument(out RazorCodeDocument codeDocument, out DocumentSnapshot documentSnapshot, string content, bool isRazorFile = true)
    {
        var sourceText = SourceText.From(content);
        codeDocument = CreateCodeDocument(content, isRazorFile, DefaultTagHelpers);
        var outDoc = codeDocument;
        documentSnapshot = Mock.Of<DocumentSnapshot>(
            d => d.GetTextAsync() == Task.FromResult(sourceText),
            MockBehavior.Strict);
        Mock.Get(documentSnapshot)
            .Setup(s => s.GetGeneratedOutputAsync())
            .ReturnsAsync(outDoc);
    }
    #endregion
}
