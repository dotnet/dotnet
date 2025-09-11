// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MessagePack;
using Microsoft.AspNetCore.Razor.Language.Components;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class BindMetadataFormatter : ValueFormatter<BindMetadata>
{
    private const int PropertyCount = 7;

    public static readonly ValueFormatter<BindMetadata> Instance = new BindMetadataFormatter();

    public override BindMetadata Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        var builder = new BindMetadata.Builder
        {
            IsFallback = reader.ReadBoolean(),
            ValueAttribute = CachedStringFormatter.Instance.Deserialize(ref reader, options),
            ChangeAttribute = CachedStringFormatter.Instance.Deserialize(ref reader, options),
            ExpressionAttribute = CachedStringFormatter.Instance.Deserialize(ref reader, options),
            TypeAttribute = CachedStringFormatter.Instance.Deserialize(ref reader, options),
            IsInvariantCulture = reader.ReadBoolean(),
            Format = CachedStringFormatter.Instance.Deserialize(ref reader, options)
        };

        return builder.Build();
    }

    public override void Serialize(ref MessagePackWriter writer, BindMetadata value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(PropertyCount);

        writer.Write(value.IsFallback);
        CachedStringFormatter.Instance.Serialize(ref writer, value.ValueAttribute, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.ChangeAttribute, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.ExpressionAttribute, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.TypeAttribute, options);
        writer.Write(value.IsInvariantCulture);
        CachedStringFormatter.Instance.Serialize(ref writer, value.Format, options);
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        reader.Skip(); // IsFallback
        CachedStringFormatter.Instance.Skim(ref reader, options); // ValueAttribute
        CachedStringFormatter.Instance.Skim(ref reader, options); // ChangeAttribute
        CachedStringFormatter.Instance.Skim(ref reader, options); // ExpressionAttribute
        CachedStringFormatter.Instance.Skim(ref reader, options); // TypeAttribute
        reader.Skip(); // IsInvariantCulture
        CachedStringFormatter.Instance.Skim(ref reader, options); // Format
    }
}
