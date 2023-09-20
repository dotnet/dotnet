// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.Serialization.Json.Converters;

internal partial class TagHelperDeltaResultJsonConverter : ObjectJsonConverter<TagHelperDeltaResult>
{
    public static readonly TagHelperDeltaResultJsonConverter Instance = new();

    private TagHelperDeltaResultJsonConverter()
    {
    }

    protected override TagHelperDeltaResult ReadFromProperties(JsonDataReader reader)
    {
        var delta = reader.ReadBooleanOrTrue(nameof(TagHelperDeltaResult.Delta));
        var resultId = reader.ReadInt32OrZero(nameof(TagHelperDeltaResult.ResultId));
        var added = reader.ReadImmutableArrayOrEmpty(nameof(TagHelperDeltaResult.Added),
            static r => ObjectReaders.ReadTagHelper(r, useCache: true));
        var removed = reader.ReadImmutableArrayOrEmpty(nameof(TagHelperDeltaResult.Removed),
            static r => ObjectReaders.ReadTagHelper(r, useCache: true));

        return new(delta, resultId, added, removed);
    }

    protected override void WriteProperties(JsonDataWriter writer, TagHelperDeltaResult value)
    {
        writer.WriteIfNotTrue(nameof(value.Delta), value.Delta);
        writer.WriteIfNotZero(nameof(value.ResultId), value.ResultId);
        writer.WriteArrayIfNotNullOrEmpty(nameof(value.Added), value.Added, ObjectWriters.Write);
        writer.WriteArrayIfNotNullOrEmpty(nameof(value.Removed), value.Removed, ObjectWriters.Write);
    }
}
