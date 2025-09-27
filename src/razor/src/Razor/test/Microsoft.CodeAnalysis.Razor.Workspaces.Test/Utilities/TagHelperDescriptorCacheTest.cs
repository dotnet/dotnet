// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.Utilities;

public class TagHelperDescriptorCacheTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    private readonly TagHelperCache _tagHelperCache = new();

    [Fact]
    public void TagHelperDescriptorCache_TypeNameAffectsHash()
    {
        // Arrange
        var expectedPropertyName = "PropertyName";

        var intTagHelperBuilder = new TagHelperDescriptorBuilder(TagHelperKind.ITagHelper, "TestTagHelper", "Test")
        {
            TypeName = "TestTagHelper"
        };

        intTagHelperBuilder.BindAttribute(b =>
        {
            b.Name = "test";
            b.PropertyName = expectedPropertyName;
            b.TypeName = typeof(int).FullName;
        });

        var intTagHelper = intTagHelperBuilder.Build();

        var stringTagHelperBuilder = new TagHelperDescriptorBuilder(TagHelperKind.ITagHelper, "TestTagHelper", "Test")
        {
            TypeName = "TestTagHelper"
        };

        stringTagHelperBuilder.BindAttribute(b =>
        {
            b.Name = "test";
            b.PropertyName = expectedPropertyName;
            b.TypeName = typeof(string).FullName;
        });

        var stringTagHelper = stringTagHelperBuilder.Build();

        // Act
        _tagHelperCache.TryAdd(intTagHelper.Checksum, intTagHelper);

        // Assert
        Assert.False(_tagHelperCache.TryGet(stringTagHelper.Checksum, out _));
    }

    [Fact]
    public void GetHashCode_DuplicateTagHelpers_NoCacheIdCollisions()
    {
        // Arrange
        var tagHelpers = new List<TagHelperDescriptor>();
        var tagHelpersPerBatch = -1;

        // Reads 5 copies of the TagHelpers (with 5x references)
        for (var i = 0; i < 5; ++i)
        {
            var tagHelpersBatch = RazorTestResources.BlazorServerAppTagHelpers;
            tagHelpers.AddRange(tagHelpersBatch);
            tagHelpersPerBatch = tagHelpersBatch.Length;
        }

        // Act
        var hashes = new HashSet<Checksum>(tagHelpers.Select(t => t.Checksum));

        // Assert
        // Only 1 batch of taghelpers should remain after we filter by cache id
        Assert.Equal(hashes.Count, tagHelpersPerBatch);
    }

    [Fact]
    public void GetHashCode_AllTagHelpers_NoCacheIdCollisions()
    {
        // Arrange
        var tagHelpers = RazorTestResources.BlazorServerAppTagHelpers;

        // Act
        var hashes = new HashSet<Checksum>(tagHelpers.Select(t => t.Checksum));

        // Assert
        Assert.Equal(hashes.Count, tagHelpers.Length);
    }
}
