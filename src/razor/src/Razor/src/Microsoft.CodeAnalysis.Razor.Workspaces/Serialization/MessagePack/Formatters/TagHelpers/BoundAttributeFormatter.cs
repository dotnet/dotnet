// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using MessagePack;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class BoundAttributeFormatter : ValueFormatter<BoundAttributeDescriptor>
{
    private const int PropertyCount = 14;

    public static readonly ValueFormatter<BoundAttributeDescriptor> Instance = new BoundAttributeFormatter();

    private BoundAttributeFormatter()
    {
    }

    public override BoundAttributeDescriptor Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        var name = CachedStringFormatter.Instance.Deserialize(ref reader, options);
        var typeName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var isEnum = reader.ReadBoolean();
        var hasIndexer = reader.ReadBoolean();
        var indexerNamePrefix = CachedStringFormatter.Instance.Deserialize(ref reader, options);
        var indexerTypeName = CachedStringFormatter.Instance.Deserialize(ref reader, options);
        var displayName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var containingType = CachedStringFormatter.Instance.Deserialize(ref reader, options);
        var documentationObject = reader.Deserialize<DocumentationObject>(options);
        var caseSensitive = reader.ReadBoolean();
        var isEditorRequired = reader.ReadBoolean();
        var parameters = reader.Deserialize<ImmutableArray<BoundAttributeParameterDescriptor>>(options);

        var metadata = reader.Deserialize<MetadataCollection>(options);
        var diagnostics = reader.Deserialize<ImmutableArray<RazorDiagnostic>>(options);

        return new BoundAttributeDescriptor(
            name!, typeName, isEnum,
            hasIndexer, indexerNamePrefix, indexerTypeName,
            documentationObject, displayName, containingType, caseSensitive, isEditorRequired,
            parameters, metadata, diagnostics);
    }

    public override void Serialize(ref MessagePackWriter writer, BoundAttributeDescriptor value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(PropertyCount);

        CachedStringFormatter.Instance.Serialize(ref writer, value.Name, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.TypeName, options);
        writer.Write(value.IsEnum);
        writer.Write(value.HasIndexer);
        CachedStringFormatter.Instance.Serialize(ref writer, value.IndexerNamePrefix, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.IndexerTypeName, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.DisplayName, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.ContainingType, options);
        writer.Serialize(value.DocumentationObject, options);
        writer.Write(value.CaseSensitive);
        writer.Write(value.IsEditorRequired);
        writer.Serialize(value.Parameters, options);

        writer.Serialize(value.Metadata, options);
        writer.Serialize(value.Diagnostics, options);
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        CachedStringFormatter.Instance.Skim(ref reader, options); // Name
        CachedStringFormatter.Instance.Skim(ref reader, options); // TypeName
        reader.Skip(); // IsEnum
        reader.Skip(); // HasIndexer
        CachedStringFormatter.Instance.Skim(ref reader, options); // IndexerNamePrefix
        CachedStringFormatter.Instance.Skim(ref reader, options); // IndexerTypeName
        CachedStringFormatter.Instance.Skim(ref reader, options); // DisplayName
        CachedStringFormatter.Instance.Skim(ref reader, options); // ContainingType
        DocumentationObjectFormatter.Instance.Skim(ref reader, options); // DocumentationObject
        reader.Skip(); // CaseSensitive
        reader.Skip(); // IsEditorRequired
        BoundAttributeParameterFormatter.Instance.SkimArray(ref reader, options); // BoundAttributeParameters

        MetadataCollectionFormatter.Instance.Skim(ref reader, options); // Metadata
        RazorDiagnosticFormatter.Instance.SkimArray(ref reader, options); // Diagnostics
    }
}
