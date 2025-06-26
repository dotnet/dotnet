// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using MessagePack;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class BoundAttributeParameterFormatter : ValueFormatter<BoundAttributeParameterDescriptor>
{
    private const int PropertyCount = 6;

    public static readonly ValueFormatter<BoundAttributeParameterDescriptor> Instance = new BoundAttributeParameterFormatter();

    private BoundAttributeParameterFormatter()
    {
    }

    public override BoundAttributeParameterDescriptor Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        var flags = (BoundAttributeParameterFlags)reader.ReadByte();
        var name = CachedStringFormatter.Instance.Deserialize(ref reader, options);
        var propertyName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var typeNameObject = reader.Deserialize<TypeNameObject>(options);
        var documentationObject = reader.Deserialize<DocumentationObject>(options);

        var diagnostics = reader.Deserialize<ImmutableArray<RazorDiagnostic>>(options);

        return new BoundAttributeParameterDescriptor(
            flags, name!, propertyName, typeNameObject, documentationObject, diagnostics);
    }

    public override void Serialize(ref MessagePackWriter writer, BoundAttributeParameterDescriptor value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(PropertyCount);

        writer.Write((byte)value.Flags);
        CachedStringFormatter.Instance.Serialize(ref writer, value.Name, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.PropertyName, options);
        writer.Serialize(value.TypeNameObject, options);
        writer.Serialize(value.DocumentationObject, options);

        writer.Serialize(value.Diagnostics, options);
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        reader.Skip(); // Flags
        CachedStringFormatter.Instance.Skim(ref reader, options); // Name
        CachedStringFormatter.Instance.Skim(ref reader, options); // PropertyName
        TypeNameObjectFormatter.Instance.Skim(ref reader, options); // TypeNameObject
        DocumentationObjectFormatter.Instance.Skim(ref reader, options); // DocumentationObject

        RazorDiagnosticFormatter.Instance.SkimArray(ref reader, options); // Diagnostics
    }
}
