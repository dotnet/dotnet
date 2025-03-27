// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using MessagePack;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal sealed class TagHelperDeltaResultFormatter : TopLevelFormatter<TagHelperDeltaResult>
{
    public static readonly TopLevelFormatter<TagHelperDeltaResult> Instance = new TagHelperDeltaResultFormatter();

    private TagHelperDeltaResultFormatter()
    {
    }

    public override TagHelperDeltaResult Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(4);

        var isDelta = reader.ReadBoolean();
        var resultId = reader.ReadInt32();
        var added = reader.Deserialize<ImmutableArray<Checksum>>(options);
        var removed = reader.Deserialize<ImmutableArray<Checksum>>(options);

        return new(isDelta, resultId, added, removed);
    }

    public override void Serialize(ref MessagePackWriter writer, TagHelperDeltaResult value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(4);

        writer.Write(value.IsDelta);
        writer.Write(value.ResultId);
        writer.Serialize(value.Added, options);
        writer.Serialize(value.Removed, options);
    }
}
