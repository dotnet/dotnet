// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class TagMatchingRuleFormatter : TagHelperObjectFormatter<TagMatchingRuleDescriptor>
{
    public static readonly TagHelperObjectFormatter<TagMatchingRuleDescriptor> Instance = new TagMatchingRuleFormatter();

    private TagMatchingRuleFormatter()
    {
    }

    public override TagMatchingRuleDescriptor Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        var tagName = reader.ReadString(cache).AssumeNotNull();
        var parentTag = reader.ReadString(cache);
        var tagStructure = (TagStructure)reader.ReadInt32();
        var caseSensitive = reader.ReadBoolean();
        var attributes = RequiredAttributeFormatter.Instance.DeserializeArray(ref reader, options, cache);
        var diagnostics = RazorDiagnosticFormatter.Instance.DeserializeArray(ref reader, options);

        return new DefaultTagMatchingRuleDescriptor(
            tagName, parentTag,
            tagStructure, caseSensitive,
            attributes, diagnostics);
    }

    public override void Serialize(ref MessagePackWriter writer, TagMatchingRuleDescriptor value, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        writer.Write(value.TagName, cache);
        writer.Write(value.ParentTag, cache);
        writer.Write((int)value.TagStructure);
        writer.Write(value.CaseSensitive);
        RequiredAttributeFormatter.Instance.SerializeArray(ref writer, value.Attributes, options, cache);
        RazorDiagnosticFormatter.Instance.SerializeArray(ref writer, value.Diagnostics, options);
    }
}
