// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MessagePack;
using Microsoft.AspNetCore.Razor.Language.Components;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class TypeParameterMetadataFormatter : ValueFormatter<TypeParameterMetadata>
{
    private const int PropertyCount = 3;

    public static readonly ValueFormatter<TypeParameterMetadata> Instance = new TypeParameterMetadataFormatter();

    public override TypeParameterMetadata Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        var builder = new TypeParameterMetadata.Builder
        {
            IsCascading = reader.ReadBoolean(),
            Constraints = CachedStringFormatter.Instance.Deserialize(ref reader, options),
            NameWithAttributes = CachedStringFormatter.Instance.Deserialize(ref reader, options)
        };

        return builder.Build();
    }

    public override void Serialize(ref MessagePackWriter writer, TypeParameterMetadata value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(PropertyCount);

        writer.Write(value.IsCascading);
        CachedStringFormatter.Instance.Serialize(ref writer, value.Constraints, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.NameWithAttributes, options);
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        reader.Skip(); // IsCascading
        CachedStringFormatter.Instance.Skim(ref reader, options); // Constraints
        CachedStringFormatter.Instance.Skim(ref reader, options); // NameWithAttributes
    }
}
