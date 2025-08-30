// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using MessagePack;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class TagHelperFormatter : ValueFormatter<TagHelperDescriptor>
{
    private const int PropertyCount = 14;

    public static readonly ValueFormatter<TagHelperDescriptor> Instance = new TagHelperFormatter();

    private TagHelperFormatter()
    {
    }

    public override TagHelperDescriptor Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        var flags = (TagHelperFlags)reader.ReadByte();
        var kind = (TagHelperKind)reader.ReadByte();
        var runtimeKind = (RuntimeKind)reader.ReadByte();
        var name = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var assemblyName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();

        var displayName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var typeNameObject = reader.Deserialize<TypeNameObject>(options);
        var documentationObject = reader.Deserialize<DocumentationObject>(options);
        var tagOutputHint = CachedStringFormatter.Instance.Deserialize(ref reader, options);

        var tagMatchingRules = reader.Deserialize<ImmutableArray<TagMatchingRuleDescriptor>>(options);
        var boundAttributes = reader.Deserialize<ImmutableArray<BoundAttributeDescriptor>>(options);
        var allowedChildTags = reader.Deserialize<ImmutableArray<AllowedChildTagDescriptor>>(options);

        var metadata = reader.Deserialize<MetadataObject>(options);
        var diagnostics = reader.Deserialize<ImmutableArray<RazorDiagnostic>>(options);

        return new TagHelperDescriptor(
            flags, kind, runtimeKind, name, assemblyName,
            displayName, typeNameObject, documentationObject, tagOutputHint,
            tagMatchingRules, boundAttributes, allowedChildTags,
            metadata, diagnostics);
    }

    public override void Serialize(ref MessagePackWriter writer, TagHelperDescriptor value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(PropertyCount);

        writer.Write((byte)value.Flags);
        writer.Write((byte)value.Kind);
        writer.Write((byte)value.RuntimeKind);
        CachedStringFormatter.Instance.Serialize(ref writer, value.Name, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.AssemblyName, options);

        CachedStringFormatter.Instance.Serialize(ref writer, value.DisplayName, options);
        writer.Serialize(value.TypeNameObject, options);
        writer.Serialize(value.DocumentationObject, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.TagOutputHint, options);

        writer.Serialize(value.TagMatchingRules, options);
        writer.Serialize(value.BoundAttributes, options);
        writer.Serialize(value.AllowedChildTags, options);

        writer.Serialize(value.Metadata, options);
        writer.Serialize(value.Diagnostics, options);
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        reader.Skip(); // Flags
        reader.Skip(); // Kind
        reader.Skip(); // RuntimeKind
        CachedStringFormatter.Instance.Skim(ref reader, options); // Name
        CachedStringFormatter.Instance.Skim(ref reader, options); // AssemblyName

        CachedStringFormatter.Instance.Skim(ref reader, options); // DisplayName
        TypeNameObjectFormatter.Instance.Skim(ref reader, options); // TypeNameObject
        DocumentationObjectFormatter.Instance.Skim(ref reader, options); // DocumentationObject
        CachedStringFormatter.Instance.Skim(ref reader, options); // TagOutputHint

        TagMatchingRuleFormatter.Instance.SkimArray(ref reader, options); // TagMatchingRules
        BoundAttributeFormatter.Instance.SkimArray(ref reader, options); // BoundAttributes
        AllowedChildTagFormatter.Instance.SkimArray(ref reader, options); // AllowedChildTags

        MetadataObjectFormatter.Instance.Skim(ref reader, options); // Metadata
        RazorDiagnosticFormatter.Instance.SkimArray(ref reader, options); // Diagnostics
    }
}
