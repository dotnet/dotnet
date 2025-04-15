// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.CSharp;
using Moq;
using Xunit;

namespace Microsoft.CodeAnalysis.Razor;

public class CompilationTagHelperFeatureTest
{
    [Fact]
    public void IsValidCompilation_ReturnsTrueIfTagHelperInterfaceCannotBeFound()
    {
        // Arrange
        var references = new[]
        {
            ReferenceUtil.NetLatestSystemRuntime,
        };
        var compilation = CSharpCompilation.Create("Test", references: references);

        // Act
        var result = CompilationTagHelperFeature.IsValidCompilation(compilation);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidCompilation_ReturnsFalseIfSystemStringCannotBeFound()
    {
        // Arrange
        var references = new[]
        {
            ReferenceUtil.AspNetLatestRazor,
        };
        var compilation = CSharpCompilation.Create("Test", references: references);

        // Act
        var result = CompilationTagHelperFeature.IsValidCompilation(compilation);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidCompilation_ReturnsTrueIfWellKnownTypesAreFound()
    {
        // Arrange
        var references = new[]
        {
            ReferenceUtil.NetLatestSystemRuntime,
            ReferenceUtil.AspNetLatestRazor,
        };
        var compilation = CSharpCompilation.Create("Test", references: references);

        // Act
        var result = CompilationTagHelperFeature.IsValidCompilation(compilation);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetDescriptors_DoesNotSetCompilation_IfCompilationIsInvalid()
    {
        // Arrange
        var provider = new Mock<ITagHelperDescriptorProvider>();
        provider.Setup(c => c.Execute(It.IsAny<TagHelperDescriptorProviderContext>()));

        var engine = RazorProjectEngine.Create(
            configure =>
            {
                configure.ConfigureParserOptions(builder =>
                {
                    builder.UseRoslynTokenizer = true;
                });

                configure.Features.Add(new DefaultMetadataReferenceFeature());
                configure.Features.Add(provider.Object);
                configure.Features.Add(new CompilationTagHelperFeature());
            });

        var feature = engine.Engine.GetFeatures<CompilationTagHelperFeature>().First();

        // Act
        var result = feature.GetDescriptors();

        // Assert
        Assert.Empty(result);
        provider.Verify(c => c.Execute(It.IsAny<TagHelperDescriptorProviderContext>()), Times.Never);
    }

    [Fact]
    public void GetDescriptors_SetsCompilation_IfCompilationIsValid()
    {
        // Arrange
        Compilation compilation = null;
        var provider = new Mock<ITagHelperDescriptorProvider>();
        provider
            .Setup(c => c.Execute(It.IsAny<TagHelperDescriptorProviderContext>()))
            .Callback<TagHelperDescriptorProviderContext>(c => compilation = c.Compilation)
            .Verifiable();

        var references = new[]
        {
            ReferenceUtil.NetLatestSystemRuntime,
            ReferenceUtil.AspNetLatestRazor,
        };

        var engine = RazorProjectEngine.Create(
            configure =>
            {
                configure.ConfigureParserOptions(builder =>
                {
                    builder.UseRoslynTokenizer = true;
                });

                configure.Features.Add(new DefaultMetadataReferenceFeature { References = references });
                configure.Features.Add(provider.Object);
                configure.Features.Add(new CompilationTagHelperFeature());
            });

        var feature = engine.Engine.GetFeatures<CompilationTagHelperFeature>().First();

        // Act
        var result = feature.GetDescriptors();

        // Assert
        Assert.Empty(result);
        provider.Verify();
        Assert.NotNull(compilation);
    }
}
