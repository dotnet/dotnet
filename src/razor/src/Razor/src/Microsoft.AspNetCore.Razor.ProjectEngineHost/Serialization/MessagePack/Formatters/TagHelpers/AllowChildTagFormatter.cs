// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using MessagePack;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class AllowedChildTagFormatter : ValueFormatter<AllowedChildTagDescriptor>
{
    public static readonly ValueFormatter<AllowedChildTagDescriptor> Instance = new AllowedChildTagFormatter();

    private AllowedChildTagFormatter()
    {
    }

    public override AllowedChildTagDescriptor Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(3);

        var name = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var displayName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var diagnostics = reader.Deserialize<ImmutableArray<RazorDiagnostic>>(options);

        return new AllowedChildTagDescriptor(name, displayName, diagnostics);
    }

    public override void Serialize(ref MessagePackWriter writer, AllowedChildTagDescriptor value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(3);

        CachedStringFormatter.Instance.Serialize(ref writer, value.Name, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.DisplayName, options);
        writer.Serialize(value.Diagnostics, options);
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(3);

        CachedStringFormatter.Instance.Skim(ref reader, options); // Name
        CachedStringFormatter.Instance.Skim(ref reader, options); // DisplayName
        RazorDiagnosticFormatter.Instance.SkimArray(ref reader, options); // Diagnostics
    }
}
