// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MessagePack;
using Microsoft.AspNetCore.Razor.Language.Components;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class PropertyMetadataFormatter : ValueFormatter<PropertyMetadata>
{
    private const int PropertyCount = 7;

    public static readonly ValueFormatter<PropertyMetadata> Instance = new PropertyMetadataFormatter();

    public override PropertyMetadata Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        var builder = new PropertyMetadata.Builder
        {
            GloballyQualifiedTypeName = CachedStringFormatter.Instance.Deserialize(ref reader, options),
            IsChildContent = reader.ReadBoolean(),
            IsEventCallback = reader.ReadBoolean(),
            IsDelegateSignature = reader.ReadBoolean(),
            IsDelegateWithAwaitableResult = reader.ReadBoolean(),
            IsGenericTyped = reader.ReadBoolean(),
            IsInitOnlyProperty = reader.ReadBoolean()
        };

        return builder.Build();
    }

    public override void Serialize(ref MessagePackWriter writer, PropertyMetadata value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(PropertyCount);

        CachedStringFormatter.Instance.Serialize(ref writer, value.GloballyQualifiedTypeName, options);
        writer.Write(value.IsChildContent);
        writer.Write(value.IsEventCallback);
        writer.Write(value.IsDelegateSignature);
        writer.Write(value.IsDelegateWithAwaitableResult);
        writer.Write(value.IsGenericTyped);
        writer.Write(value.IsInitOnlyProperty);
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        CachedStringFormatter.Instance.Skim(ref reader, options); // GloballyQualifiedTypeName
        reader.Skip(); // IsChildContent
        reader.Skip(); // IsEventCallback
        reader.Skip(); // IsDelegateSignature
        reader.Skip(); // IsDelegateWithAwaitableResult
        reader.Skip(); // IsGenericTyped
        reader.Skip(); // IsInitOnlyProperty
    }
}
