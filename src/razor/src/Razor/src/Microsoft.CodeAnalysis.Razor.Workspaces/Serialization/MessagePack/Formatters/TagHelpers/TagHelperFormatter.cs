// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using MessagePack;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class TagHelperFormatter : ValueFormatter<TagHelperDescriptor>
{
    public static readonly ValueFormatter<TagHelperDescriptor> Instance = new TagHelperFormatter();

    private TagHelperFormatter()
    {
    }

    public override TagHelperDescriptor Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(12);

        var kind = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var name = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var assemblyName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();

        var displayName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var documentationObject = reader.Deserialize<DocumentationObject>(options);
        var tagOutputHint = CachedStringFormatter.Instance.Deserialize(ref reader, options);
        var caseSensitive = reader.ReadBoolean();

        var tagMatchingRules = reader.Deserialize<ImmutableArray<TagMatchingRuleDescriptor>>(options);
        var boundAttributes = reader.Deserialize<ImmutableArray<BoundAttributeDescriptor>>(options);
        var allowedChildTags = reader.Deserialize<ImmutableArray<AllowedChildTagDescriptor>>(options);

        var metadata = reader.Deserialize<MetadataCollection>(options);
        var diagnostics = reader.Deserialize<ImmutableArray<RazorDiagnostic>>(options);

        return new TagHelperDescriptor(
            kind, name, assemblyName,
            displayName, documentationObject,
            tagOutputHint, caseSensitive,
            tagMatchingRules, boundAttributes, allowedChildTags,
            metadata, diagnostics);
    }

    public override void Serialize(ref MessagePackWriter writer, TagHelperDescriptor value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(12);

        CachedStringFormatter.Instance.Serialize(ref writer, value.Kind, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.Name, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.AssemblyName, options);

        CachedStringFormatter.Instance.Serialize(ref writer, value.DisplayName, options);
        writer.Serialize(value.DocumentationObject, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.TagOutputHint, options);
        writer.Write(value.CaseSensitive);

        writer.Serialize(value.TagMatchingRules, options);
        writer.Serialize(value.BoundAttributes, options);
        writer.Serialize(value.AllowedChildTags, options);

        writer.Serialize(value.Metadata, options);
        writer.Serialize(value.Diagnostics, options);
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(12);

        CachedStringFormatter.Instance.Skim(ref reader, options); // Kind
        CachedStringFormatter.Instance.Skim(ref reader, options); // Name
        CachedStringFormatter.Instance.Skim(ref reader, options); // AssemblyName

        CachedStringFormatter.Instance.Skim(ref reader, options); // DisplayName
        DocumentationObjectFormatter.Instance.Skim(ref reader, options); // DocumentationObject
        CachedStringFormatter.Instance.Skim(ref reader, options); // TagOutputHint
        reader.Skip(); // CaseSensitive

        TagMatchingRuleFormatter.Instance.SkimArray(ref reader, options); // TagMatchingRules
        BoundAttributeFormatter.Instance.SkimArray(ref reader, options); // BoundAttributes
        AllowedChildTagFormatter.Instance.SkimArray(ref reader, options); // AllowedChildTags

        MetadataCollectionFormatter.Instance.Skim(ref reader, options); // Metadata
        RazorDiagnosticFormatter.Instance.SkimArray(ref reader, options); // Diagnostics
    }
}
