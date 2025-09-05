// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using MessagePack;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class TypeNameObjectFormatter : ValueFormatter<TypeNameObject>
{
    private enum TypeNameKind : byte
    {
        Index,
        Strings
    }

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

        var propertyCount = reader.ReadArrayHeader();
        Debug.Assert(propertyCount >= 2);

        var typeNameKind = (TypeNameKind)reader.ReadByte();

        switch (typeNameKind)
        {
            case TypeNameKind.Index:
                Debug.Assert(propertyCount == 2);
                var index = reader.ReadByte();
                return new(index);

            case TypeNameKind.Strings:
                Debug.Assert(propertyCount == 4);

                var fullName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
                var namespaceName = CachedStringFormatter.Instance.Deserialize(ref reader, options);
                var name = CachedStringFormatter.Instance.Deserialize(ref reader, options);

                return TypeNameObject.From(fullName, namespaceName, name);

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

        if (value.Index is byte index)
        {
            writer.WriteArrayHeader(2);

            writer.Write((byte)TypeNameKind.Index);
            writer.Write(index);
        }
        else
        {
            writer.WriteArrayHeader(4);

            writer.Write((byte)TypeNameKind.Strings);

            CachedStringFormatter.Instance.Serialize(ref writer, value.FullName, options);
            CachedStringFormatter.Instance.Serialize(ref writer, value.Namespace, options);
            CachedStringFormatter.Instance.Serialize(ref writer, value.Name, options);
        }
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        if (reader.TryReadNil())
        {
            return;
        }

        var propertyCount = reader.ReadArrayHeader();

        var typeNameKind = (TypeNameKind)reader.ReadByte();
        switch (typeNameKind)
        {
            case TypeNameKind.Index:
                Debug.Assert(propertyCount == 2);
                reader.Skip(); // Index
                break;

            case TypeNameKind.Strings:
                Debug.Assert(propertyCount == 4);
                CachedStringFormatter.Instance.Skim(ref reader, options); // FullName
                CachedStringFormatter.Instance.Skim(ref reader, options); // Namespace
                CachedStringFormatter.Instance.Skim(ref reader, options); // Name
                break;
        }
    }
}
