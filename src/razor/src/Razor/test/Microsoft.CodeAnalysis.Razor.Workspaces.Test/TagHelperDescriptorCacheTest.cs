// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Utilities;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Test;

public class TagHelperDescriptorCacheTest(ITestOutputHelper testOutput) : TestBase(testOutput)
{
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
        TagHelperDescriptorCache.Set(TagHelperDescriptorCache.GetTagHelperDescriptorCacheId(intTagHelper), intTagHelper);

        // Assert
        Assert.False(TagHelperDescriptorCache.TryGetDescriptor(TagHelperDescriptorCache.GetTagHelperDescriptorCacheId(stringTagHelper), out var descriptor));
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
            var tagHelpersBatch = ReadTagHelpers();
            tagHelpers.AddRange(tagHelpersBatch);
            tagHelpersPerBatch = tagHelpersBatch.Length;
        }

        // Act
        var hashes = new HashSet<int>(tagHelpers.Select(TagHelperDescriptorCache.GetTagHelperDescriptorCacheId));

        // Assert
        // Only 1 batch of taghelpers should remain after we filter by cache id
        Assert.Equal(hashes.Count, tagHelpersPerBatch);
    }

    [Fact]
    public void GetHashCode_AllTagHelpers_NoCacheIdCollisions()
    {
        // Arrange
        var tagHelpers = ReadTagHelpers();

        // Act
        var hashes = new HashSet<int>(tagHelpers.Select(TagHelperDescriptorCache.GetTagHelperDescriptorCacheId));

        // Assert
        Assert.Equal(hashes.Count, tagHelpers.Length);
    }

    private static ImmutableArray<TagHelperDescriptor> ReadTagHelpers()
    {
        var bytes = RazorTestResources.GetResourceBytes(RazorTestResources.BlazorServerAppTagHelpersJson);

        using var stream = new MemoryStream(bytes);
        using var reader = new StreamReader(stream);

        return JsonDataConvert.DeserializeData(reader,
            static r => r.ReadImmutableArray(
                static r => ObjectReaders.ReadTagHelper(r, useCache: false)));
    }
}
