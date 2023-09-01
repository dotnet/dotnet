// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class AllowedChildTagFormatter : TagHelperObjectFormatter<AllowedChildTagDescriptor>
{
    public static readonly TagHelperObjectFormatter<AllowedChildTagDescriptor> Instance = new AllowedChildTagFormatter();

    private AllowedChildTagFormatter()
    {
    }

    public override AllowedChildTagDescriptor Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        var name = reader.ReadString(cache);
        var displayName = reader.ReadString(cache);
        var diagnostics = RazorDiagnosticFormatter.Instance.DeserializeArray(ref reader, options);

        return new DefaultAllowedChildTagDescriptor(name, displayName, diagnostics);
    }

    public override void Serialize(ref MessagePackWriter writer, AllowedChildTagDescriptor value, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        writer.Write(value.Name, cache);
        writer.Write(value.DisplayName, cache);

        RazorDiagnosticFormatter.Instance.SerializeArray(ref writer, value.Diagnostics, options);
    }
}
