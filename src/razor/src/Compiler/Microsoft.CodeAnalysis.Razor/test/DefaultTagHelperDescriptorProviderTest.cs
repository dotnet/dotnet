// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Microsoft.CodeAnalysis.Razor;

public class DefaultTagHelperDescriptorProviderTest
{
    private static readonly Assembly _assembly = typeof(DefaultTagHelperDescriptorProviderTest).GetTypeInfo().Assembly;

    [Fact]
    public void Execute_DoesNotAddEditorBrowsableNeverDescriptorsAtDesignTime()
    {
        // Arrange
        var editorBrowsableTypeName = "Microsoft.CodeAnalysis.Razor.Workspaces.Test.EditorBrowsableTagHelper";
        var compilation = TestCompilation.Create(_assembly);
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
        var editorBrowsableDescriptor = context.Results.Where(descriptor => descriptor.GetTypeName() == editorBrowsableTypeName);
        Assert.Empty(editorBrowsableDescriptor);
    }

    [Fact]
    public void Execute_WithDefaultDiscoversTagHelpersFromAssemblyAndReference()
    {
        // Arrange
        var testTagHelper = "TestAssembly.TestTagHelper";
        var enumTagHelper = "Microsoft.CodeAnalysis.Razor.Workspaces.Test.EnumTagHelper";
        var csharp = @"
using Microsoft.AspNetCore.Razor.TagHelpers;
namespace TestAssembly
{
    public class TestTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output) {}
    }
}";
        var compilation = TestCompilation.Create(_assembly, CSharpSyntaxTree.ParseText(csharp));
        var descriptorProvider = new DefaultTagHelperDescriptorProvider();

        var context = new TagHelperDescriptorProviderContext(compilation);

        // Act
        descriptorProvider.Execute(context);

        // Assert
        Assert.NotNull(compilation.GetTypeByMetadataName(testTagHelper));
        Assert.NotEmpty(context.Results);
        Assert.NotEmpty(context.Results.Where(f => f.GetTypeName() == testTagHelper));
        Assert.NotEmpty(context.Results.Where(f => f.GetTypeName() == enumTagHelper));
    }

    [Fact]
    public void Execute_WithTargetAssembly_Works()
    {
        // Arrange
        var testTagHelper = "TestAssembly.TestTagHelper";
        var enumTagHelper = "Microsoft.CodeAnalysis.Razor.Workspaces.Test.EnumTagHelper";
        var csharp = @"
using Microsoft.AspNetCore.Razor.TagHelpers;
namespace TestAssembly
{
    public class TestTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output) {}
    }
}";
        var compilation = TestCompilation.Create(_assembly, CSharpSyntaxTree.ParseText(csharp));
        var descriptorProvider = new DefaultTagHelperDescriptorProvider();

        var targetSymbol = (IAssemblySymbol)compilation.GetAssemblyOrModuleSymbol(
            compilation.References.First(static r => r.Display.Contains("Microsoft.CodeAnalysis.Razor.Test.dll")));

        var context = new TagHelperDescriptorProviderContext(compilation, targetSymbol);

        // Act
        descriptorProvider.Execute(context);

        // Assert
        Assert.NotNull(compilation.GetTypeByMetadataName(testTagHelper));
        Assert.NotEmpty(context.Results);
        Assert.Empty(context.Results.Where(f => f.GetTypeName() == testTagHelper));
        Assert.NotEmpty(context.Results.Where(f => f.GetTypeName() == enumTagHelper));
    }
}
