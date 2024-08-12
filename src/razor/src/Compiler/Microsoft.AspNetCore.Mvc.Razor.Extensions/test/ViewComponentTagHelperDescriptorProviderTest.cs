// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions;

// This is just a basic integration test. There are detailed tests for the VCTH visitor and descriptor factory.
public class ViewComponentTagHelperDescriptorProviderTest
{
    [Fact]
    public void DescriptorProvider_FindsVCTH()
    {
        // Arrange
        var code = @"
        public class StringParameterViewComponent
        {
            public string Invoke(string foo, string bar) => null;
        }
";

        var compilation = MvcShim.BaseCompilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(code));

        var context = new TagHelperDescriptorProviderContext(compilation);

        var provider = new ViewComponentTagHelperDescriptorProvider()
        {
            Engine = RazorProjectEngine.CreateEmpty().Engine,
        };

        var expectedDescriptor = TagHelperDescriptorBuilder.Create(
            ViewComponentTagHelperConventions.Kind,
            "__Generated__StringParameterViewComponentTagHelper",
            TestCompilation.AssemblyName)
            .Metadata(
                TypeName("__Generated__StringParameterViewComponentTagHelper"),
                new(ViewComponentTagHelperMetadata.Name, "StringParameter"))
            .DisplayName("StringParameterViewComponentTagHelper")
            .TagMatchingRuleDescriptor(rule =>
                rule
                .RequireTagName("vc:string-parameter")
                .RequireAttributeDescriptor(attribute => attribute.Name("foo"))
                .RequireAttributeDescriptor(attribute => attribute.Name("bar")))
            .BoundAttributeDescriptor(attribute =>
                attribute
                .Name("foo")
                .Metadata(PropertyName("foo"))
                .TypeName(typeof(string).FullName)
                .DisplayName("string StringParameterViewComponentTagHelper.foo"))
            .BoundAttributeDescriptor(attribute =>
                attribute
                .Name("bar")
                .Metadata(PropertyName("bar"))
                .TypeName(typeof(string).FullName)
                .DisplayName("string StringParameterViewComponentTagHelper.bar"))
            .Build();

        // Act
        provider.Execute(context);

        // Assert
        Assert.Single(context.Results, d => d.Equals(expectedDescriptor));
    }
}
