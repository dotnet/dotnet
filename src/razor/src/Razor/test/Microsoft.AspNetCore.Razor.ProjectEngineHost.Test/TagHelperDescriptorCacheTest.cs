// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Utilities;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;
using Checksum = Microsoft.AspNetCore.Razor.Utilities.Checksum;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Test;

public class TagHelperDescriptorCacheTest(ITestOutputHelper testOutput) : TestBase(testOutput)
{
    private readonly TagHelperCache _tagHelperCache = new();

    [Fact]
    public void TagHelperDescriptorCache_TypeNameAffectsHash()
    {
        // Arrange
        var expectedPropertyName = "PropertyName";

        var intTagHelperBuilder = new DefaultTagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, "TestTagHelper", "Test");
        _ = intTagHelperBuilder.Metadata(TypeName("TestTagHelper"));
        intTagHelperBuilder.BoundAttributeDescriptor(intBuilder =>
            intBuilder
                .Name("test")
                .Metadata(PropertyName(expectedPropertyName))
                .TypeName(typeof(int).FullName)
        );
        var intTagHelper = intTagHelperBuilder.Build();

        var stringTagHelperBuilder = new DefaultTagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, "TestTagHelper", "Test");
        _ = stringTagHelperBuilder.Metadata(TypeName("TestTagHelper"));
        stringTagHelperBuilder.BoundAttributeDescriptor(stringBuilder =>
            stringBuilder
                .Name("test")
                .Metadata(PropertyName(expectedPropertyName))
                .TypeName(typeof(string).FullName)
        );
        var stringTagHelper = stringTagHelperBuilder.Build();

        // Act
        _tagHelperCache.TryAdd(intTagHelper.GetChecksum(), intTagHelper);

        // Assert
        Assert.False(_tagHelperCache.TryGet(stringTagHelper.GetChecksum(), out _));
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
        var hashes = new HashSet<Checksum>(tagHelpers.Select(t => t.GetChecksum()));

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
        var hashes = new HashSet<Checksum>(tagHelpers.Select(t => t.GetChecksum()));

        // Assert
        Assert.Equal(hashes.Count, tagHelpers.Length);
    }
}
