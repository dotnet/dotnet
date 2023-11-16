// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Xunit;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions.Version2_X;

public class InjectTargetExtensionTest
{
    [Fact]
    public void InjectDirectiveTargetExtension_WritesProperty()
    {
        // Arrange
        using var context = TestCodeRenderingContext.CreateRuntime();
        var target = new InjectTargetExtension();
        var node = new InjectIntermediateNode()
        {
            TypeName = "PropertyType",
            MemberName = "PropertyName",
        };

        // Act
        target.WriteInjectProperty(context, node);

        // Assert
        Assert.Equal("""
            [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
            public PropertyType PropertyName { get; private set; }

            """,
            context.CodeWriter.GenerateCode());
    }

    [Fact]
    public void InjectDirectiveTargetExtension_WritesPropertyWithLinePragma_WhenSourceIsSet()
    {
        // Arrange
        using var context = TestCodeRenderingContext.CreateRuntime();
        var target = new InjectTargetExtension();
        var node = new InjectIntermediateNode()
        {
            TypeName = "PropertyType<ModelType>",
            MemberName = "PropertyName",
            Source = new SourceSpan(
                filePath: "test-path",
                absoluteIndex: 0,
                lineIndex: 1,
                characterIndex: 1,
                length: 10)
        };

        // Act
        target.WriteInjectProperty(context, node);

        // Assert
        Assert.Equal("""

            #nullable restore
            #line 2 "test-path"
            [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
            public PropertyType<ModelType> PropertyName { get; private set; }

            #line default
            #line hidden
            #nullable disable

            """,
            context.CodeWriter.GenerateCode());
    }
}
