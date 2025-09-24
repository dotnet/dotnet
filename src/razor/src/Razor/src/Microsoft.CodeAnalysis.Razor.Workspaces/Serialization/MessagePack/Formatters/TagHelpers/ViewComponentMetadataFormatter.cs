// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MessagePack;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class ViewComponentMetadataFormatter : ValueFormatter<ViewComponentMetadata>
{
    private const int PropertyCount = 2;

    public static readonly ValueFormatter<ViewComponentMetadata> Instance = new ViewComponentMetadataFormatter();

    public override ViewComponentMetadata Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        var builder = new ViewComponentMetadata.Builder
        {
            Name = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull(),
            OriginalTypeNameObject = reader.Deserialize<TypeNameObject>(options)
        };

        return builder.Build();
    }

    public override void Serialize(ref MessagePackWriter writer, ViewComponentMetadata value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(PropertyCount);

        CachedStringFormatter.Instance.Serialize(ref writer, value.Name, options);
        writer.Serialize(value.OriginalTypeNameObject, options);
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        CachedStringFormatter.Instance.Skim(ref reader, options); // Name
        TypeNameObjectFormatter.Instance.Skim(ref reader, options); // OriginalTypeName
    }
}
