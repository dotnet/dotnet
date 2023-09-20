// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class TagHelperFormatter : TagHelperObjectFormatter<TagHelperDescriptor>
{
    public static readonly TagHelperObjectFormatter<TagHelperDescriptor> Instance = new TagHelperFormatter();

    private TagHelperFormatter()
    {
    }

    public override TagHelperDescriptor Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        var kind = reader.ReadString(cache).AssumeNotNull();
        var name = reader.ReadString(cache).AssumeNotNull();
        var assemblyName = reader.ReadString(cache).AssumeNotNull();

        var displayName = reader.ReadString(cache);
        var documentationObject = DocumentationObjectFormatter.Instance.Deserialize(ref reader, options, cache);
        var tagOutputHint = reader.ReadString(cache);
        var caseSensitive = reader.ReadBoolean();

        var tagMatchingRules = TagMatchingRuleFormatter.Instance.DeserializeArray(ref reader, options, cache);
        var boundAttributes = BoundAttributeFormatter.Instance.DeserializeArray(ref reader, options, cache);
        var allowedChildTags = AllowedChildTagFormatter.Instance.DeserializeArray(ref reader, options, cache);

        var metadata = MetadataCollectionFormatter.Instance.Deserialize(ref reader, options, cache);
        var diagnostics = RazorDiagnosticFormatter.Instance.DeserializeArray(ref reader, options);

        return new DefaultTagHelperDescriptor(
            kind, name, assemblyName,
            displayName!, documentationObject,
            tagOutputHint, caseSensitive,
            tagMatchingRules, boundAttributes, allowedChildTags,
            metadata, diagnostics);
    }

    public override void Serialize(ref MessagePackWriter writer, TagHelperDescriptor value, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        writer.Write(value.Kind, cache);
        writer.Write(value.Name, cache);
        writer.Write(value.AssemblyName, cache);
        writer.Write(value.DisplayName, cache);
        DocumentationObjectFormatter.Instance.Serialize(ref writer, value.DocumentationObject, options, cache);
        writer.Write(value.TagOutputHint, cache);
        writer.Write(value.CaseSensitive);
        TagMatchingRuleFormatter.Instance.SerializeArray(ref writer, value.TagMatchingRules, options, cache);
        BoundAttributeFormatter.Instance.SerializeArray(ref writer, value.BoundAttributes, options, cache);
        AllowedChildTagFormatter.Instance.SerializeArray(ref writer, value.AllowedChildTags, options, cache);
        MetadataCollectionFormatter.Instance.Serialize(ref writer, (MetadataCollection)value.Metadata, options, cache);
        RazorDiagnosticFormatter.Instance.SerializeArray(ref writer, value.Diagnostics, options);
    }
}
