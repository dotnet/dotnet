// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal sealed class TagHelperDeltaResultFormatter : MessagePackFormatter<TagHelperDeltaResult>
{
    public static readonly MessagePackFormatter<TagHelperDeltaResult> Instance = new TagHelperDeltaResultFormatter();

    private TagHelperDeltaResultFormatter()
    {
    }

    public override TagHelperDeltaResult Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        using var _ = TagHelperSerializationCache.Pool.GetPooledObject(out var cache);

        var delta = reader.ReadBoolean();
        var resultId = reader.ReadInt32();
        var added = TagHelperFormatter.Instance.DeserializeImmutableArray(ref reader, options, cache);
        var removed = TagHelperFormatter.Instance.DeserializeImmutableArray(ref reader, options, cache);

        return new(delta, resultId, added, removed);
    }

    public override void Serialize(ref MessagePackWriter writer, TagHelperDeltaResult value, MessagePackSerializerOptions options)
    {
        using var _ = TagHelperSerializationCache.Pool.GetPooledObject(out var cache);

        writer.Write(value.Delta);
        writer.Write(value.ResultId);
        TagHelperFormatter.Instance.SerializeArray(ref writer, value.Added, options, cache);
        TagHelperFormatter.Instance.SerializeArray(ref writer, value.Removed, options, cache);
    }
}
