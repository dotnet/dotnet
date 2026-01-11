// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MessagePack;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters;

internal sealed class FetchTagHelpersResultFormatter : TopLevelFormatter<FetchTagHelpersResult>
{
    public static readonly TopLevelFormatter<FetchTagHelpersResult> Instance = new FetchTagHelpersResultFormatter();

    private FetchTagHelpersResultFormatter()
    {
    }

    public override FetchTagHelpersResult Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        var tagHelpers = reader.Deserialize<TagHelperCollection>(options);

        return new(tagHelpers);
    }

    public override void Serialize(ref MessagePackWriter writer, FetchTagHelpersResult value, SerializerCachingOptions options)
    {
        writer.Serialize(value.TagHelpers, options);
    }
}
