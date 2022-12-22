// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;

public class AutoClosingTagOnAutoInsertProviderTest : RazorOnAutoInsertProviderTestBase
{
    public AutoClosingTagOnAutoInsertProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    private RazorLSPOptions Options { get; set; } = RazorLSPOptions.Default;

    private static TagHelperDescriptor CatchAllTagHelper
    {
        get
        {
            var descriptor = TagHelperDescriptorBuilder.Create("CatchAllTagHelper", "TestAssembly");
            descriptor.SetTypeName("TestNamespace.CatchAllTagHelper");
            descriptor.TagMatchingRule(builder => builder.RequireTagName("*").RequireTagStructure(TagStructure.Unspecified));

            return descriptor.Build();
        }
    }

    private static TagHelperDescriptor UnspecifiedInputMirroringTagHelper
    {
        get
        {
            var descriptor = TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly");
            descriptor.SetTypeName("TestNamespace.TestTagHelper");
            descriptor.TagMatchingRule(builder => builder.RequireTagName("Input").RequireTagStructure(TagStructure.Unspecified));

            return descriptor.Build();
        }
    }

    private static TagHelperDescriptor UnspecifiedTagHelper
    {
        get
        {
            var descriptor = TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly");
            descriptor.SetTypeName("TestNamespace.TestTagHelper");
            descriptor.TagMatchingRule(builder => builder.RequireTagName("test").RequireTagStructure(TagStructure.Unspecified));

            return descriptor.Build();
        }
    }

    private static TagHelperDescriptor UnspecifiedInputTagHelper
    {
        get
        {
            var descriptor = TagHelperDescriptorBuilder.Create("TestInputTagHelper", "TestAssembly");
            descriptor.SetTypeName("TestNamespace.TestInputTagHelper");
            descriptor.TagMatchingRule(builder => builder.RequireTagName("input").RequireTagStructure(TagStructure.Unspecified));

            return descriptor.Build();
        }
    }

    private static TagHelperDescriptor NormalOrSelfclosingInputTagHelper
    {
        get
        {
            var descriptor = TagHelperDescriptorBuilder.Create("TestInputTagHelper", "TestAssembly");
            descriptor.SetTypeName("TestNamespace.TestInputTagHelper");
            descriptor.TagMatchingRule(builder => builder.RequireTagName("input").RequireTagStructure(TagStructure.NormalOrSelfClosing));

            return descriptor.Build();
        }
    }

    private static TagHelperDescriptor NormalOrSelfClosingTagHelper
    {
        get
        {
            var descriptor = TagHelperDescriptorBuilder.Create("TestTagHelper2", "TestAssembly");
            descriptor.SetTypeName("TestNamespace.TestTagHelper2");
            descriptor.TagMatchingRule(builder => builder.RequireTagName("test").RequireTagStructure(TagStructure.NormalOrSelfClosing));

            return descriptor.Build();
        }
    }

    private static TagHelperDescriptor WithoutEndTagTagHelper
    {
        get
        {
            var descriptor = TagHelperDescriptorBuilder.Create("TestTagHelper3", "TestAssembly");
            descriptor.SetTypeName("TestNamespace.TestTagHelper3");
            descriptor.TagMatchingRule(builder => builder.RequireTagName("test").RequireTagStructure(TagStructure.WithoutEndTag));

            return descriptor.Build();
        }
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6217")]
    public void OnTypeCloseAngle_ConflictingAutoClosingBehaviorsChoosesMostSpecific()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

<test>$$
",
expected: @"
@addTagHelper *, TestAssembly

<test />
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { WithoutEndTagTagHelper, CatchAllTagHelper });

    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/36125")]
    public void OnTypeCloseAngle_TagHelperAlreadyHasEndTag()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

<test>$$<test></test></test>
",
expected: @"
@addTagHelper *, TestAssembly

<test><test></test></test>
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfClosingTagHelper });
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/36125")]
    public void OnTypeCloseAngle_VoidTagHelperHasEndTag_ShouldStillAutoClose()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

<input>$$<input></input></input>
",
expected: @"
@addTagHelper *, TestAssembly

<input /><input></input></input>
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { UnspecifiedInputTagHelper });
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/36125")]
    public void OnTypeCloseAngle_TagAlreadyHasEndTag()
    {
        RunAutoInsertTest(
input: @"
<div>$$<div></div></div>
",
expected: @"
<div><div></div></div>
");
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/36125")]
    public void OnTypeCloseAngle_TagDoesNotAutoCloseOutOfScope()
    {
        RunAutoInsertTest(
input: @"
<div>
    @if (true)
    {
        <div>$$</div>
    }
",
expected: @"
<div>
    @if (true)
    {
        <div></div>
    }
");
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/36125")]
    public void OnTypeCloseAngle_VoidTagHasEndTag_ShouldStillAutoClose()
    {
        RunAutoInsertTest(
input: @"
<input>$$<input></input></input>
",
expected: @"
<input /><input></input></input>
");
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/36568")]
    public void OnTypeCloseAngle_VoidElementMirroringTagHelper()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

<Input>$$
",
expected: @"
@addTagHelper *, TestAssembly

<Input>$0</Input>
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { UnspecifiedInputMirroringTagHelper });
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/36568")]
    public void OnTypeCloseAngle_VoidHtmlElementCapitalized_SelfCloses()
    {
        RunAutoInsertTest(
input: "<Input>$$",
expected: "<Input />",
fileKind: FileKinds.Legacy,
tagHelpers: Array.Empty<TagHelperDescriptor>());
    }

    [Fact]
    public void OnTypeCloseAngle_NormalOrSelfClosingStructureOverridesVoidTagBehavior()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

<input>$$
",
expected: @"
@addTagHelper *, TestAssembly

<input>$0</input>
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfclosingInputTagHelper });
    }

    [Fact]
    public void OnTypeCloseAngle_UnspeccifiedStructureInheritsVoidTagBehavior()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

<input>$$
",
expected: @"
@addTagHelper *, TestAssembly

<input />
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { UnspecifiedInputTagHelper });
    }

    [Fact]
    public void OnTypeCloseAngle_UnspeccifiedTagHelperTagStructure()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

<test>$$
",
expected: @"
@addTagHelper *, TestAssembly

<test>$0</test>
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { UnspecifiedTagHelper });
    }

    [Fact]
    public void OnTypeCloseAngle_NormalOrSelfClosingTagHelperTagStructure()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

<test>$$
",
expected: @"
@addTagHelper *, TestAssembly

<test>$0</test>
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfClosingTagHelper });
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/33930")]
    public void OnTypeCloseAngle_TagHelperInHtml_NestedStatement()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><test>$$</div>
}
",
expected: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><test>$0</test></div>
}
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfClosingTagHelper });
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/5694")]
    public void OnTypeCloseAngle_HtmlTagInHtml_NestedStatement_WithAttribute()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><a target=""_blank"">$$</div>
}
",
expected: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><a target=""_blank"">$0</a></div>
}
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfClosingTagHelper });
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/5694")]
    public void OnTypeCloseAngle_HtmlTagInHtml_NestedStatement_WithAttribute_SpaceBetweenClosingAngleAndAttributeClosingQuote()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><a target=""_blank"" >$$</div>
}
",
expected: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><a target=""_blank"" >$0</a></div>
}
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfClosingTagHelper });
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/5694")]
    public void OnTypeCloseAngle_HtmlTagInHtml_NestedStatement_WithMinimalizedAttribute()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><form novalidate>$$</div>
}
",
expected: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><form novalidate>$0</form></div>
}
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfClosingTagHelper });
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/5694")]
    public void OnTypeCloseAngle_HtmlTagInHtml_NestedStatement_WithMinimalizedAttribute_SpaceBetweenClosingAngleAndAttributeClosingQuote()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><form novalidate >$$</div>
}
",
expected: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><form novalidate >$0</form></div>
}
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfClosingTagHelper });
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/5694")]
    public void OnTypeCloseAngle_TagHelperInHtml_NestedStatement_WithAttribute()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><test attribute=""value"">$$</div>
}
",
expected: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><test attribute=""value"">$0</test></div>
}
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfClosingTagHelper });
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/5694")]
    public void OnTypeCloseAngle_TagHelperInHtml_NestedStatement_WithAttribute_SpaceBetweenClosingAngleAndAttributeClosingQuote()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><test attribute=""value"" >$$</div>
}
",
expected: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><test attribute=""value"" >$0</test></div>
}
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfClosingTagHelper });
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/5694")]
    public void OnTypeCloseAngle_TagHelperInHtml_NestedStatement_WithMinimalizedAttribute()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><test bool-val>$$</div>
}
",
expected: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><test bool-val>$0</test></div>
}
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfClosingTagHelper });
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/5694")]
    public void OnTypeCloseAngle_TagHelperInHtml_NestedStatement_WithMinimalizedAttribute_SpaceBetweenClosingAngleAndAttributeClosingQuote()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><test bool-val >$$</div>
}
",
expected: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><test bool-val >$0</test></div>
}
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfClosingTagHelper });
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/33930")]
    public void OnTypeCloseAngle_TagHelperInTagHelper_NestedStatement()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

@if (true)
{
<test><input>$$</test>
}
",
expected: @"
@addTagHelper *, TestAssembly

@if (true)
{
<test><input /></test>
}
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfClosingTagHelper, UnspecifiedInputTagHelper });
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/36906")]
    public void OnTypeCloseAngle_TagHelperNextToVoidTagHelper_NestedStatement()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

@if (true)
{
<test>$$<input />
}
",
expected: @"
@addTagHelper *, TestAssembly

@if (true)
{
<test>$0</test><input />
}
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfClosingTagHelper, UnspecifiedInputTagHelper });
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/36906")]
    public void OnTypeCloseAngle_TagHelperNextToTagHelper_NestedStatement()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

@if (true)
{
<test>$$<input></input>
}
",
expected: @"
@addTagHelper *, TestAssembly

@if (true)
{
<test>$0</test><input></input>
}
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfClosingTagHelper, NormalOrSelfclosingInputTagHelper });
    }

    [Fact]
    public void OnTypeCloseAngle_NormalOrSelfClosingTagHelperTagStructure_CodeBlock()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

@{
    <test>$$
}
",
expected: @"
@addTagHelper *, TestAssembly

@{
    <test>$0</test>
}
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfClosingTagHelper });
    }

    [Fact]
    public void OnTypeCloseAngle_WithSlash_WithoutEndTagTagHelperTagStructure()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

<test />$$
",
expected: @"
@addTagHelper *, TestAssembly

<test />
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { WithoutEndTagTagHelper });
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/33930")]
    public void OnTypeCloseAngle_NestedStatement()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><test />$$</div>
}
",
expected: @"
@addTagHelper *, TestAssembly

@if (true)
{
<div><test /></div>
}
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { WithoutEndTagTagHelper });
    }

    [Fact]
    public void OnTypeCloseAngle_WithSpace_WithoutEndTagTagHelperTagStructure()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

<test >$$
",
expected: @"
@addTagHelper *, TestAssembly

<test />
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { WithoutEndTagTagHelper });
    }

    [Fact]
    public void OnTypeCloseAngle_WithoutEndTagTagHelperTagStructure()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

<test>$$
",
expected: @"
@addTagHelper *, TestAssembly

<test />
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { WithoutEndTagTagHelper });
    }

    [Fact]
    public void OnTypeCloseAngle_WithoutEndTagTagHelperTagStructure_CodeBlock()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

@{
    <test>$$
}
",
expected: @"
@addTagHelper *, TestAssembly

@{
    <test>
}
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { WithoutEndTagTagHelper });
    }

    [Fact]
    public void OnTypeCloseAngle_MultipleApplicableTagHelperTagStructures()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

<test>$$
",
expected: @"
@addTagHelper *, TestAssembly

<test>$0</test>
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { UnspecifiedTagHelper, NormalOrSelfClosingTagHelper, WithoutEndTagTagHelper });
    }

    [Fact]
    public void OnTypeCloseAngle_EscapedTagTagHelperAutoCompletesWithEscape()
    {
        RunAutoInsertTest(
input: @"
@addTagHelper *, TestAssembly

<!test>$$
",
expected: @"
@addTagHelper *, TestAssembly

<!test>$0</!test>
",
fileKind: FileKinds.Legacy,
tagHelpers: new[] { NormalOrSelfClosingTagHelper });
    }

    [Fact]
    public void OnTypeCloseAngle_AlwaysClosesStandardHTMLTag()
    {
        RunAutoInsertTest(
input: @"
    <div><div>$$</div>
",
expected: @"
    <div><div>$0</div></div>
");
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/33930")]
    public void OnTypeCloseAngle_ClosesStandardHTMLTag_NestedStatement()
    {
        RunAutoInsertTest(
input: @"
@if (true)
{
    <div><p>$$</div>
}
",
expected: @"
@if (true)
{
    <div><p>$0</p></div>
}
");
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/36906")]
    public void OnTypeCloseAngle_TagNextToTag_NestedStatement()
    {
        RunAutoInsertTest(
input: @"
@if (true)
{
    <p>$$<div></div>
}
",
expected: @"
@if (true)
{
    <p>$0</p><div></div>
}
");
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/36906")]
    public void OnTypeCloseAngle_TagNextToVoidTag_NestedStatement()
    {
        RunAutoInsertTest(
input: @"
@if (true)
{
    <p>$$<input />
}
",
expected: @"
@if (true)
{
    <p>$0</p><input />
}
");
    }

    [Fact]
    public void OnTypeCloseAngle_ClosesStandardHTMLTag()
    {
        RunAutoInsertTest(
input: @"
    <div>$$
",
expected: @"
    <div>$0</div>
");
    }

    [Fact]
    public void OnTypeCloseAngle_ClosesStandardHTMLTag_CodeBlock()
    {
        RunAutoInsertTest(
input: @"
@{
    <div>$$
}
",
expected: @"
@{
    <div>$0</div>
}
");
    }

    [Fact]
    public void OnTypeCloseAngle_ClosesVoidHTMLTag()
    {
        RunAutoInsertTest(
input: @"
    <input>$$
",
expected: @"
    <input />
");
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/33930")]
    public void OnTypeCloseAngle_ClosesVoidHTMLTag_NestedStatement()
    {
        RunAutoInsertTest(
input: @"
@if (true)
{
    <strong><input>$$</strong>
}
",
expected: @"
@if (true)
{
    <strong><input /></strong>
}
");
    }

    [Fact]
    public void OnTypeCloseAngle_ClosesVoidHTMLTag_CodeBlock()
    {
        RunAutoInsertTest(
input: @"
@{
    <input>$$
}
",
expected: @"
@{
    <input />
}
");
    }

    [Fact]
    public void OnTypeCloseAngle_WithSlash_ClosesVoidHTMLTag()
    {
        RunAutoInsertTest(
input: @"
    <input />$$
",
expected: @"
    <input />
");
    }

    [Fact]
    public void OnTypeCloseAngle_WithSpace_ClosesVoidHTMLTag()
    {
        RunAutoInsertTest(
input: @"
    <input >$$
",
expected: @"
    <input />
");
    }

    [Fact]
    public void OnTypeCloseAngle_AutoInsertDisabled_Noops()
    {
        Options = new RazorLSPOptions(Trace.Off, enableFormatting: true, autoClosingTags: false, insertSpaces: true, tabSize: 4);
        RunAutoInsertTest(
input: @"
    <div>$$
",
expected: @"
    <div>
");
    }

    internal override RazorOnAutoInsertProvider CreateProvider()
    {
        var optionsMonitor = new Mock<IOptionsMonitor<RazorLSPOptions>>(MockBehavior.Strict);
        optionsMonitor.SetupGet(o => o.CurrentValue).Returns(Options);

        var provider = new AutoClosingTagOnAutoInsertProvider(optionsMonitor.Object, LoggerFactory);
        return provider;
    }
}
