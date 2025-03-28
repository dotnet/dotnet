// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using MessagePack;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class BoundAttributeParameterFormatter : ValueFormatter<BoundAttributeParameterDescriptor>
{
    public static readonly ValueFormatter<BoundAttributeParameterDescriptor> Instance = new BoundAttributeParameterFormatter();

    private BoundAttributeParameterFormatter()
    {
    }

    public override BoundAttributeParameterDescriptor Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(9);

        var kind = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var name = CachedStringFormatter.Instance.Deserialize(ref reader, options);
        var typeName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var isEnum = reader.ReadBoolean();
        var displayName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var documentationObject = reader.Deserialize<DocumentationObject>(options);
        var caseSensitive = reader.ReadBoolean();

        var metadata = reader.Deserialize<MetadataCollection>(options);
        var diagnostics = reader.Deserialize<ImmutableArray<RazorDiagnostic>>(options);

        return new BoundAttributeParameterDescriptor(
            kind, name!, typeName,
            isEnum, documentationObject, displayName, caseSensitive,
            metadata, diagnostics);
    }

    public override void Serialize(ref MessagePackWriter writer, BoundAttributeParameterDescriptor value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(9);

        CachedStringFormatter.Instance.Serialize(ref writer, value.Kind, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.Name, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.TypeName, options);
        writer.Write(value.IsEnum);
        CachedStringFormatter.Instance.Serialize(ref writer, value.DisplayName, options);
        writer.Serialize(value.DocumentationObject, options);
        writer.Write(value.CaseSensitive);

        writer.Serialize(value.Metadata, options);
        writer.Serialize(value.Diagnostics, options);
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(9);

        CachedStringFormatter.Instance.Skim(ref reader, options); // Kind
        CachedStringFormatter.Instance.Skim(ref reader, options); // Name
        CachedStringFormatter.Instance.Skim(ref reader, options); // TypeName
        reader.Skip(); // IsEnum
        CachedStringFormatter.Instance.Skim(ref reader, options); // DisplayName
        DocumentationObjectFormatter.Instance.Skim(ref reader, options); // DocumentationObject
        reader.Skip(); // CaseSensitive

        MetadataCollectionFormatter.Instance.Skim(ref reader, options); // Metadata
        RazorDiagnosticFormatter.Instance.SkimArray(ref reader, options); // Diagnostics
    }
}
