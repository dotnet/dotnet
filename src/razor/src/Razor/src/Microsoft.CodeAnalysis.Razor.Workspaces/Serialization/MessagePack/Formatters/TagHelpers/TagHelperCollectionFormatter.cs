// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using MessagePack;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.Utilities;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class TagHelperCollectionFormatter : ValueFormatter<TagHelperCollection>
{
    public static readonly ValueFormatter<TagHelperCollection> Instance = new TagHelperCollectionFormatter();

    private TagHelperCollectionFormatter()
    {
    }

    public override TagHelperCollection Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        var count = reader.ReadArrayHeader();
        if (count == 0)
        {
            return TagHelperCollection.Empty;
        }

        // The array is structured as [Checksum, TagHelperDescriptor, Checksum, TagHelperDescriptor, ...]
        Debug.Assert(count % 2 == 0, "Expected array to have an even number of elements.");
        count /= 2;

        using var builder = new TagHelperCollection.RefBuilder(initialCapacity: count);

        var cache = TagHelperCache.Default;
        var checksumFormatter = ChecksumFormatter.Instance;
        var tagHelperFormatter = TagHelperFormatter.Instance;

        for (var i = 0; i < count; i++)
        {
            var checksum = checksumFormatter.Deserialize(ref reader, options);

            if (!cache.TryGet(checksum, out var tagHelper))
            {
                tagHelper = tagHelperFormatter.Deserialize(ref reader, options);
                cache.TryAdd(checksum, tagHelper);
            }
            else
            {
                tagHelperFormatter.Skim(ref reader, options);
            }

            builder.Add(tagHelper);
        }

        return builder.ToCollection();
    }

    public override void Serialize(ref MessagePackWriter writer, TagHelperCollection value, SerializerCachingOptions options)
    {
        if (value.Count == 0)
        {
            writer.WriteArrayHeader(0);
            return;
        }

        // Write an array of [Checksum, TagHelperDescriptor, Checksum, TagHelperDescriptor, ...]
        writer.WriteArrayHeader(value.Count * 2);

        var checksumFormatter = ChecksumFormatter.Instance;
        var tagHelperFormatter = TagHelperFormatter.Instance;

        foreach (var tagHelper in value)
        {
            checksumFormatter.Serialize(ref writer, tagHelper.Checksum, options);
            tagHelperFormatter.Serialize(ref writer, tagHelper, options);
        }
    }
}
