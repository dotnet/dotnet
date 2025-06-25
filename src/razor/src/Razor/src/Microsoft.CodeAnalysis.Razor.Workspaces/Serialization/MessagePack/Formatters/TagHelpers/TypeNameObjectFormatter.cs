// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MessagePack;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class TypeNameObjectFormatter : ValueFormatter<TypeNameObject>
{
    private enum TypeNameKind : byte
    {
        Index,
        String
    }

    private const int PropertyCount = 2;

    public static readonly ValueFormatter<TypeNameObject> Instance = new TypeNameObjectFormatter();

    private TypeNameObjectFormatter()
    {
    }

    public override TypeNameObject Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        if (reader.TryReadNil())
        {
            return default;
        }

        reader.ReadArrayHeaderAndVerify(PropertyCount);

        var typeNameKind = (TypeNameKind)reader.ReadByte();

        switch (typeNameKind)
        {
            case TypeNameKind.Index:
                var index = reader.ReadByte();
                return new(index);
            case TypeNameKind.String:
                var fullName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
                return new(fullName);

            default:
                return Assumed.Unreachable<TypeNameObject>();
        }
    }

    public override void Serialize(ref MessagePackWriter writer, TypeNameObject value, SerializerCachingOptions options)
    {
        if (value.IsNull)
        {
            writer.WriteNil();
            return;
        }

        writer.WriteArrayHeader(PropertyCount);

        if (value.Index is byte index)
        {
            writer.Write((byte)TypeNameKind.Index);
            writer.Write(index);
        }
        else if (value.StringValue is string stringValue)
        {
            writer.Write((byte)TypeNameKind.String);
            CachedStringFormatter.Instance.Serialize(ref writer, stringValue, options);
        }
        else
        {
            Assumed.Unreachable();
        }
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        if (reader.TryReadNil())
        {
            return;
        }

        reader.ReadArrayHeaderAndVerify(PropertyCount);

        var typeNameKind = (TypeNameKind)reader.ReadByte();
        switch (typeNameKind)
        {
            case TypeNameKind.Index:
                reader.Skip(); // Index
                break;

            case TypeNameKind.String:
                CachedStringFormatter.Instance.Skim(ref reader, options); // StringValue
                break;
        }
    }
}
