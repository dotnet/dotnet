// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Remote.Razor.Test;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Remote.Razor;

public class TagHelperDeltaResultTest : TagHelperDescriptorTestBase
{
    public TagHelperDeltaResultTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void Apply_Noop()
    {
        // Arrange
        var delta = new TagHelperDeltaResult(Delta: true, ResultId: 1337, Array.Empty<TagHelperDescriptor>(), Array.Empty<TagHelperDescriptor>());

        // Act
        var tagHelpers = delta.Apply(Project1TagHelpers);

        // Assert
        Assert.Equal(Project1TagHelpers, tagHelpers);
    }

    [Fact]
    public void Apply_Added()
    {
        // Arrange
        var delta = new TagHelperDeltaResult(Delta: true, ResultId: 1337, Project1TagHelpers, Array.Empty<TagHelperDescriptor>());

        // Act
        var tagHelpers = delta.Apply(Project2TagHelpers);

        // Assert
        Assert.Equal(Project1AndProject2TagHelpers, tagHelpers);
    }

    [Fact]
    public void Apply_Removed()
    {
        // Arrange
        var delta = new TagHelperDeltaResult(Delta: true, ResultId: 1337, Array.Empty<TagHelperDescriptor>(), Project1TagHelpers);

        // Act
        var tagHelpers = delta.Apply(Project1AndProject2TagHelpers);

        // Assert
        Assert.Equal(Project2TagHelpers, tagHelpers);
    }

    [Fact]
    public void Apply_AddAndRemoved()
    {
        // Arrange
        var delta = new TagHelperDeltaResult(Delta: true, ResultId: 1337, Project1TagHelpers, Project2TagHelpers);

        // Act
        var tagHelpers = delta.Apply(Project2TagHelpers);

        // Assert
        Assert.Equal(Project1TagHelpers, tagHelpers);
    }
}
