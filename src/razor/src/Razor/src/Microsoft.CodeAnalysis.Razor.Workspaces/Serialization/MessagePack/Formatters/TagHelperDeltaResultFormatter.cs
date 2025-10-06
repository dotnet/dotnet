// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using MessagePack;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters;

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
