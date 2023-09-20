// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using MessagePack;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal sealed class FetchTagHelpersResultFormatter : TopLevelFormatter<FetchTagHelpersResult>
{
    public static readonly TopLevelFormatter<FetchTagHelpersResult> Instance = new FetchTagHelpersResultFormatter();

    private FetchTagHelpersResultFormatter()
    {
    }

    public override FetchTagHelpersResult Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        var tagHelpers = reader.Deserialize<ImmutableArray<TagHelperDescriptor>>(options);

        return new(tagHelpers);
    }

    public override void Serialize(ref MessagePackWriter writer, FetchTagHelpersResult value, SerializerCachingOptions options)
    {
        writer.Serialize(value.TagHelpers, options);
    }
}
