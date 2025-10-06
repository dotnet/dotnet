// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using MessagePack;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class TagMatchingRuleFormatter : ValueFormatter<TagMatchingRuleDescriptor>
{
    public static readonly ValueFormatter<TagMatchingRuleDescriptor> Instance = new TagMatchingRuleFormatter();

    private TagMatchingRuleFormatter()
    {
    }

    public override TagMatchingRuleDescriptor Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(6);

        var tagName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var parentTag = CachedStringFormatter.Instance.Deserialize(ref reader, options);
        var tagStructure = (TagStructure)reader.ReadInt32();
        var caseSensitive = reader.ReadBoolean();
        var attributes = reader.Deserialize<ImmutableArray<RequiredAttributeDescriptor>>(options);
        var diagnostics = reader.Deserialize<ImmutableArray<RazorDiagnostic>>(options);

        return new TagMatchingRuleDescriptor(
            tagName, parentTag,
            tagStructure, caseSensitive,
            attributes, diagnostics);
    }

    public override void Serialize(ref MessagePackWriter writer, TagMatchingRuleDescriptor value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(6);

        CachedStringFormatter.Instance.Serialize(ref writer, value.TagName, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.ParentTag, options);
        writer.Write((int)value.TagStructure);
        writer.Write(value.CaseSensitive);
        writer.Serialize(value.Attributes, options);
        writer.Serialize(value.Diagnostics, options);
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(6);

        CachedStringFormatter.Instance.Skim(ref reader, options); // TagName
        CachedStringFormatter.Instance.Skim(ref reader, options); // ParentTag
        reader.Skip(); // TagStructure
        reader.Skip(); // CaseSensitive
        RequiredAttributeFormatter.Instance.SkimArray(ref reader, options); // Attributes
        RazorDiagnosticFormatter.Instance.SkimArray(ref reader, options); // Diagnostics
    }
}
