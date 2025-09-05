// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MessagePack;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class MetadataObjectFormatter : ValueFormatter<MetadataObject>
{
    private const int PropertyCount = 2;

    public static readonly ValueFormatter<MetadataObject> Instance = new MetadataObjectFormatter();

    public override MetadataObject Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        var kind = (MetadataKind)reader.ReadByte();

        switch (kind)
        {
            case MetadataKind.None:
                reader.ReadNil();
                return MetadataObject.None;

            case MetadataKind.TypeParameter:
                return TypeParameterMetadataFormatter.Instance.Deserialize(ref reader, options);

            case MetadataKind.Property:
                return PropertyMetadataFormatter.Instance.Deserialize(ref reader, options);

            case MetadataKind.ChildContentParameter:
                reader.ReadNil();
                return ChildContentParameterMetadata.Default;

            case MetadataKind.Bind:
                return BindMetadataFormatter.Instance.Deserialize(ref reader, options);

            case MetadataKind.Component:
                return ComponentMetadataFormatter.Instance.Deserialize(ref reader, options);

            case MetadataKind.EventHandler:
                return EventHandlerMetadataFormatter.Instance.Deserialize(ref reader, options);

            case MetadataKind.ViewComponent:
                return ViewComponentMetadataFormatter.Instance.Deserialize(ref reader, options);

            default:
                return Assumed.Unreachable<MetadataObject>();
        }
    }

    public override void Serialize(ref MessagePackWriter writer, MetadataObject value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(PropertyCount);

        var kind = value?.Kind ?? MetadataKind.None;
        writer.Write((byte)kind);

        switch (kind)
        {
            case MetadataKind.None:
                writer.WriteNil();
                break;

            case MetadataKind.TypeParameter:
                TypeParameterMetadataFormatter.Instance.Serialize(ref writer, (TypeParameterMetadata)value.AssumeNotNull(), options);
                break;

            case MetadataKind.Property:
                PropertyMetadataFormatter.Instance.Serialize(ref writer, (PropertyMetadata)value.AssumeNotNull(), options);
                break;

            case MetadataKind.ChildContentParameter:
                writer.WriteNil();
                break;

            case MetadataKind.Bind:
                BindMetadataFormatter.Instance.Serialize(ref writer, (BindMetadata)value.AssumeNotNull(), options);
                break;

            case MetadataKind.Component:
                ComponentMetadataFormatter.Instance.Serialize(ref writer, (ComponentMetadata)value.AssumeNotNull(), options);
                break;

            case MetadataKind.EventHandler:
                EventHandlerMetadataFormatter.Instance.Serialize(ref writer, (EventHandlerMetadata)value.AssumeNotNull(), options);
                break;

            case MetadataKind.ViewComponent:
                ViewComponentMetadataFormatter.Instance.Serialize(ref writer, (ViewComponentMetadata)value.AssumeNotNull(), options);
                break;
        }
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        var kind = (MetadataKind)reader.ReadByte();

        switch (kind)
        {
            case MetadataKind.None:
                reader.ReadNil();
                break;

            case MetadataKind.TypeParameter:
                TypeParameterMetadataFormatter.Instance.Skim(ref reader, options);
                break;

            case MetadataKind.Property:
                PropertyMetadataFormatter.Instance.Skim(ref reader, options);
                break;

            case MetadataKind.ChildContentParameter:
                reader.ReadNil();
                break;

            case MetadataKind.Bind:
                BindMetadataFormatter.Instance.Skim(ref reader, options);
                break;

            case MetadataKind.Component:
                ComponentMetadataFormatter.Instance.Skim(ref reader, options);
                break;

            case MetadataKind.EventHandler:
                EventHandlerMetadataFormatter.Instance.Skim(ref reader, options);
                break;

            case MetadataKind.ViewComponent:
                ViewComponentMetadataFormatter.Instance.Skim(ref reader, options);
                break;

            default:
                Assumed.Unreachable();
                break;
        }
    }
}
