// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MessagePack;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language.Components;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class EventHandlerMetadataFormatter : ValueFormatter<EventHandlerMetadata>
{
    private const int PropertyCount = 1;

    public static readonly ValueFormatter<EventHandlerMetadata> Instance = new EventHandlerMetadataFormatter();

    public override EventHandlerMetadata Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        var builder = new EventHandlerMetadata.Builder
        {
            EventArgsType = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull()
        };

        return builder.Build();
    }

    public override void Serialize(ref MessagePackWriter writer, EventHandlerMetadata value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(PropertyCount);

        CachedStringFormatter.Instance.Serialize(ref writer, value.EventArgsType, options);
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        CachedStringFormatter.Instance.Skim(ref reader, options); // EventArgsType
    }
}
