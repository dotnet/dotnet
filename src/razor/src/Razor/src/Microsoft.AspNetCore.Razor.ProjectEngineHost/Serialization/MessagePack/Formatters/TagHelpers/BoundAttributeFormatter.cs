// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class BoundAttributeFormatter : TagHelperObjectFormatter<BoundAttributeDescriptor>
{
    public static readonly TagHelperObjectFormatter<BoundAttributeDescriptor> Instance = new BoundAttributeFormatter();

    private BoundAttributeFormatter()
    {
    }

    public override BoundAttributeDescriptor Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        var kind = reader.ReadString(cache).AssumeNotNull();
        var name = reader.ReadString(cache);
        var typeName = reader.ReadString(cache);
        var isEnum = reader.ReadBoolean();
        var hasIndexer = reader.ReadBoolean();
        var indexerNamePrefix = reader.ReadString(cache);
        var indexerTypeName = reader.ReadString(cache);
        var displayName = reader.ReadString(cache);
        var documentationObject = DocumentationObjectFormatter.Instance.Deserialize(ref reader, options, cache);
        var caseSensitive = reader.ReadBoolean();
        var isEditorRequired = reader.ReadBoolean();
        var parameters = BoundAttributeParameterFormatter.Instance.DeserializeArray(ref reader, options, cache);

        var metadata = MetadataCollectionFormatter.Instance.Deserialize(ref reader, options, cache);
        var diagnostics = RazorDiagnosticFormatter.Instance.DeserializeArray(ref reader, options);

        return new DefaultBoundAttributeDescriptor(
            kind, name, typeName, isEnum,
            hasIndexer, indexerNamePrefix, indexerTypeName,
            documentationObject, displayName, caseSensitive, isEditorRequired,
            parameters, metadata, diagnostics);
    }

    public override void Serialize(ref MessagePackWriter writer, BoundAttributeDescriptor value, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        writer.Write(value.Kind, cache);
        writer.Write(value.Name, cache);
        writer.Write(value.TypeName, cache);
        writer.Write(value.IsEnum);
        writer.Write(value.HasIndexer);
        writer.Write(value.IndexerNamePrefix, cache);
        writer.Write(value.IndexerTypeName, cache);
        writer.Write(value.DisplayName, cache);
        DocumentationObjectFormatter.Instance.Serialize(ref writer, value.DocumentationObject, options, cache);
        writer.Write(value.CaseSensitive);
        writer.Write(value.IsEditorRequired);
        BoundAttributeParameterFormatter.Instance.SerializeArray(ref writer, value.BoundAttributeParameters, options, cache);

        MetadataCollectionFormatter.Instance.Serialize(ref writer, (MetadataCollection)value.Metadata, options, cache);
        RazorDiagnosticFormatter.Instance.SerializeArray(ref writer, value.Diagnostics, options);
    }
}
