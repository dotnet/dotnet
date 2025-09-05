// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MessagePack;
using Microsoft.AspNetCore.Razor.Language.Components;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class ComponentMetadataFormatter : ValueFormatter<ComponentMetadata>
{
    private const int PropertyCount = 2;

    public static readonly ValueFormatter<ComponentMetadata> Instance = new ComponentMetadataFormatter();

    public override ComponentMetadata Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        var builder = new ComponentMetadata.Builder
        {
            IsGeneric = reader.ReadBoolean(),
            HasRenderModeDirective = reader.ReadBoolean()
        };

        return builder.Build();
    }

    public override void Serialize(ref MessagePackWriter writer, ComponentMetadata value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(PropertyCount);

        writer.Write(value.IsGeneric);
        writer.Write(value.HasRenderModeDirective);
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        reader.Skip(); // IsGeneric
        reader.Skip(); // HasRenderModeDirective
    }
}
