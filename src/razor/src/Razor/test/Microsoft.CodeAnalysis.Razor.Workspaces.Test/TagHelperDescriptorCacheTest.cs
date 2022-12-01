// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Test;

public class TagHelperDescriptorCacheTest : TestBase
{
    private static readonly TestFile s_tagHelpersTestFile = TestFile.Create("taghelpers.json", typeof(TagHelperDescriptorCacheTest));

    public TagHelperDescriptorCacheTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void TagHelperDescriptorCache_TypeNameAffectsHash()
    {
        // Arrange
        var expectedPropertyName = "PropertyName";

        var intTagHelperBuilder = new DefaultTagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, "TestTagHelper", "Test");
        _ = intTagHelperBuilder.TypeName("TestTagHelper");
        intTagHelperBuilder.BoundAttributeDescriptor(intBuilder =>
            intBuilder
                .Name("test")
                .PropertyName(expectedPropertyName)
                .TypeName(typeof(int).FullName)
        );
        var intTagHelper = intTagHelperBuilder.Build();

        var stringTagHelperBuilder = new DefaultTagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, "TestTagHelper", "Test");
        _ = stringTagHelperBuilder.TypeName("TestTagHelper");
        stringTagHelperBuilder.BoundAttributeDescriptor(stringBuilder =>
            stringBuilder
                .Name("test")
                .PropertyName(expectedPropertyName)
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
            var tagHelpersBatch = ReadTagHelpers(s_tagHelpersTestFile.OpenRead());
            tagHelpers.AddRange(tagHelpersBatch);
            tagHelpersPerBatch = tagHelpersBatch.Count;
        }

        // Act
        var hashes = new HashSet<int>(tagHelpers.Select(t => TagHelperDescriptorCache.GetTagHelperDescriptorCacheId(t)));

        // Assert
        // Only 1 batch of taghelpers should remain after we filter by cache id
        Assert.Equal(hashes.Count, tagHelpersPerBatch);
    }

    [Fact]
    public void GetHashCode_AllTagHelpers_NoCacheIdCollisions()
    {
        // Arrange
        var tagHelpers = ReadTagHelpers(s_tagHelpersTestFile.OpenRead());

        // Act
        var hashes = new HashSet<int>(tagHelpers.Select(t => TagHelperDescriptorCache.GetTagHelperDescriptorCacheId(t)));

        // Assert
        Assert.Equal(hashes.Count, tagHelpers.Count);
    }

    private IReadOnlyList<TagHelperDescriptor> ReadTagHelpers(Stream stream)
    {
        var serializer = new JsonSerializer();
        serializer.Converters.Add(new RazorDiagnosticJsonConverter());
        serializer.Converters.Add(new TagHelperDescriptorJsonConverter());

        IReadOnlyList<TagHelperDescriptor> result;

        using var streamReader = new StreamReader(stream);
        using (var reader = new JsonTextReader(streamReader))
        {
            result = serializer.Deserialize<IReadOnlyList<TagHelperDescriptor>>(reader);
        }

        stream.Dispose();

        return result;
    }
}
