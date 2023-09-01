// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class BoundAttributeParameterFormatter : TagHelperObjectFormatter<BoundAttributeParameterDescriptor>
{
    public static readonly TagHelperObjectFormatter<BoundAttributeParameterDescriptor> Instance = new BoundAttributeParameterFormatter();

    private BoundAttributeParameterFormatter()
    {
    }

    public override BoundAttributeParameterDescriptor Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        var kind = reader.ReadString(cache).AssumeNotNull();
        var name = reader.ReadString(cache);
        var typeName = reader.ReadString(cache);
        var isEnum = reader.ReadBoolean();
        var displayName = reader.ReadString(cache);
        var documentationObject = DocumentationObjectFormatter.Instance.Deserialize(ref reader, options, cache);
        var caseSensitive = reader.ReadBoolean();

        var metadata = MetadataCollectionFormatter.Instance.Deserialize(ref reader, options, cache);
        var diagnostics = RazorDiagnosticFormatter.Instance.DeserializeArray(ref reader, options);

        return new DefaultBoundAttributeParameterDescriptor(
            kind, name, typeName,
            isEnum, documentationObject, displayName, caseSensitive,
            metadata, diagnostics);
    }

    public override void Serialize(ref MessagePackWriter writer, BoundAttributeParameterDescriptor value, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        writer.Write(value.Kind, cache);
        writer.Write(value.Name, cache);
        writer.Write(value.TypeName, cache);
        writer.Write(value.IsEnum);
        writer.Write(value.DisplayName, cache);
        DocumentationObjectFormatter.Instance.Serialize(ref writer, value.DocumentationObject, options, cache);
        writer.Write(value.CaseSensitive);

        MetadataCollectionFormatter.Instance.Serialize(ref writer, (MetadataCollection)value.Metadata, options, cache);
        RazorDiagnosticFormatter.Instance.SerializeArray(ref writer, value.Diagnostics, options);
    }
}
