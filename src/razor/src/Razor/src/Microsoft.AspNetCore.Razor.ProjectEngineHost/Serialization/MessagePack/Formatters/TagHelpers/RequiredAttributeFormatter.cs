// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;
using Microsoft.AspNetCore.Razor.Language;
using static Microsoft.AspNetCore.Razor.Language.RequiredAttributeDescriptor;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class RequiredAttributeFormatter : TagHelperObjectFormatter<RequiredAttributeDescriptor>
{
    public static readonly TagHelperObjectFormatter<RequiredAttributeDescriptor> Instance = new RequiredAttributeFormatter();

    private RequiredAttributeFormatter()
    {
    }

    public override RequiredAttributeDescriptor Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        var name = reader.ReadString(cache);
        var nameComparison = (NameComparisonMode)reader.ReadInt32();
        var caseSensitive = reader.ReadBoolean();
        var value = reader.ReadString(cache);
        var valueComparison = (ValueComparisonMode)reader.ReadInt32();
        var displayName = reader.ReadString(cache);

        var metadata = MetadataCollectionFormatter.Instance.Deserialize(ref reader, options, cache);
        var diagnostics = RazorDiagnosticFormatter.Instance.DeserializeArray(ref reader, options);

        return new DefaultRequiredAttributeDescriptor(
            name, nameComparison,
            caseSensitive,
            value, valueComparison,
            displayName!, diagnostics, metadata);
    }

    public override void Serialize(ref MessagePackWriter writer, RequiredAttributeDescriptor value, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        writer.Write(value.Name, cache);
        writer.Write((int)value.NameComparison);
        writer.Write(value.CaseSensitive);
        writer.Write(value.Value, cache);
        writer.Write((int)value.ValueComparison);
        writer.Write(value.DisplayName, cache);

        MetadataCollectionFormatter.Instance.Serialize(ref writer, (MetadataCollection)value.Metadata, options, cache);
        RazorDiagnosticFormatter.Instance.SerializeArray(ref writer, value.Diagnostics, options);
    }
}
