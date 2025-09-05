// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Xunit;

namespace Microsoft.CodeAnalysis.Razor;

public class DefaultTagHelperDescriptorProviderTest : TagHelperDescriptorProviderTestBase
{
    [Fact]
    public void Execute_DoesNotAddEditorBrowsableNeverDescriptorsAtDesignTime()
    {
        // Arrange
        var editorBrowsableTypeName = "TestNamespace.EditorBrowsableTagHelper";
        var compilation = BaseCompilation;
        var descriptorProvider = new DefaultTagHelperDescriptorProvider();

        var context = new TagHelperDescriptorProviderContext(compilation)
        {
            ExcludeHidden = true
        };

        // Act
        descriptorProvider.Execute(context);

        // Assert
        Assert.NotNull(compilation.GetTypeByMetadataName(editorBrowsableTypeName));
        var nullDescriptors = context.Results.Where(descriptor => descriptor == null);
        Assert.Empty(nullDescriptors);
        var editorBrowsableDescriptor = context.Results.Where(descriptor => descriptor.TypeName == editorBrowsableTypeName);
        Assert.Empty(editorBrowsableDescriptor);
    }

    [Fact]
    public void Execute_WithDefaultDiscoversTagHelpersFromAssemblyAndReference()
    {
        // Arrange
        var testTagHelper = "TestAssembly.TestTagHelper";
        var enumTagHelper = "TestNamespace.EnumTagHelper";
        var csharp = @"
using Microsoft.AspNetCore.Razor.TagHelpers;
namespace TestAssembly
{
    public class TestTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output) {}
    }
}";
        var compilation = BaseCompilation.AddSyntaxTrees(Parse(csharp));
        var descriptorProvider = new DefaultTagHelperDescriptorProvider();

        var context = new TagHelperDescriptorProviderContext(compilation);

        // Act
        descriptorProvider.Execute(context);

        // Assert
        Assert.NotNull(compilation.GetTypeByMetadataName(testTagHelper));
        Assert.NotEmpty(context.Results);
        Assert.NotEmpty(context.Results.Where(f => f.TypeName == testTagHelper));
        Assert.NotEmpty(context.Results.Where(f => f.TypeName == enumTagHelper));
    }

    [Fact]
    public void Execute_WithTargetAssembly_Works()
    {
        // Arrange
        var testTagHelper = "TestAssembly.TestTagHelper";
        var enumTagHelper = "TestNamespace.EnumTagHelper";
        var csharp = @"
using Microsoft.AspNetCore.Razor.TagHelpers;
namespace TestAssembly
{
    public class TestTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output) {}
    }
}";
        var compilation = BaseCompilation.AddSyntaxTrees(Parse(csharp));
        var descriptorProvider = new DefaultTagHelperDescriptorProvider();

        var targetSymbol = (IAssemblySymbol)compilation.GetAssemblyOrModuleSymbol(
            compilation.References.First(static r => r.Display.Contains("Microsoft.CodeAnalysis.Razor.Test")));

        var context = new TagHelperDescriptorProviderContext(compilation, targetSymbol);

        // Act
        descriptorProvider.Execute(context);

        // Assert
        Assert.NotNull(compilation.GetTypeByMetadataName(testTagHelper));
        Assert.NotEmpty(context.Results);
        Assert.Empty(context.Results.Where(f => f.TypeName == testTagHelper));
        Assert.NotEmpty(context.Results.Where(f => f.TypeName == enumTagHelper));
    }
}
