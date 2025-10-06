// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using MessagePack;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class BoundAttributeFormatter : ValueFormatter<BoundAttributeDescriptor>
{
    private const int PropertyCount = 12;

    public static readonly ValueFormatter<BoundAttributeDescriptor> Instance = new BoundAttributeFormatter();

    private BoundAttributeFormatter()
    {
    }

    public override BoundAttributeDescriptor Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        var flags = (BoundAttributeFlags)reader.ReadByte();
        var name = CachedStringFormatter.Instance.Deserialize(ref reader, options);
        var propertyName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var typeNameObject = reader.Deserialize<TypeNameObject>(options);
        var indexerNamePrefix = CachedStringFormatter.Instance.Deserialize(ref reader, options);
        var indexerTypeNameObject = reader.Deserialize<TypeNameObject>(options);
        var displayName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var containingType = CachedStringFormatter.Instance.Deserialize(ref reader, options);
        var documentationObject = reader.Deserialize<DocumentationObject>(options);
        var parameters = reader.Deserialize<ImmutableArray<BoundAttributeParameterDescriptor>>(options);

        var metadata = reader.Deserialize<MetadataObject>(options);
        var diagnostics = reader.Deserialize<ImmutableArray<RazorDiagnostic>>(options);

        return new BoundAttributeDescriptor(
            flags, name!, propertyName, typeNameObject,
            indexerNamePrefix, indexerTypeNameObject,
            documentationObject, displayName, containingType,
            parameters, metadata, diagnostics);
    }

    public override void Serialize(ref MessagePackWriter writer, BoundAttributeDescriptor value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(PropertyCount);

        writer.Write((byte)value.Flags);
        CachedStringFormatter.Instance.Serialize(ref writer, value.Name, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.PropertyName, options);
        writer.Serialize(value.TypeNameObject, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.IndexerNamePrefix, options);
        writer.Serialize(value.IndexerTypeNameObject, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.DisplayName, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.ContainingType, options);
        writer.Serialize(value.DocumentationObject, options);
        writer.Serialize(value.Parameters, options);

        writer.Serialize(value.Metadata, options);
        writer.Serialize(value.Diagnostics, options);
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        reader.Skip(); // Flags
        CachedStringFormatter.Instance.Skim(ref reader, options); // Name
        CachedStringFormatter.Instance.Skim(ref reader, options); // PropertyName
        TypeNameObjectFormatter.Instance.Skim(ref reader, options); // TypeName
        CachedStringFormatter.Instance.Skim(ref reader, options); // IndexerNamePrefix
        TypeNameObjectFormatter.Instance.Skim(ref reader, options); // IndexerTypeName
        CachedStringFormatter.Instance.Skim(ref reader, options); // DisplayName
        CachedStringFormatter.Instance.Skim(ref reader, options); // ContainingType
        DocumentationObjectFormatter.Instance.Skim(ref reader, options); // DocumentationObject
        BoundAttributeParameterFormatter.Instance.SkimArray(ref reader, options); // BoundAttributeParameters

        MetadataObjectFormatter.Instance.Skim(ref reader, options); // MetadataObject
        RazorDiagnosticFormatter.Instance.SkimArray(ref reader, options); // Diagnostics
    }
}
