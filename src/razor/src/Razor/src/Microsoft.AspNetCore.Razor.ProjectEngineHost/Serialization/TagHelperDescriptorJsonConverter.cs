// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

internal class TagHelperDescriptorJsonConverter : ObjectJsonConverter<TagHelperDescriptor>
{
    public static readonly TagHelperDescriptorJsonConverter Instance = new();

    public static bool DisableCachingForTesting { private get; set; } = false;

    private TagHelperDescriptorJsonConverter()
    {
    }

    protected override TagHelperDescriptor ReadFromProperties(JsonDataReader reader)
        => ObjectReaders.ReadTagHelperFromProperties(reader, !DisableCachingForTesting);

    protected override void WriteProperties(JsonDataWriter writer, TagHelperDescriptor value)
        => ObjectWriters.WriteProperties(writer, value);
}
