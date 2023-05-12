// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

internal partial class TagHelperResolutionResultJsonConverter : ObjectJsonConverter<TagHelperResolutionResult>
{
    public static readonly TagHelperResolutionResultJsonConverter Instance = new();

    public TagHelperResolutionResultJsonConverter()
    {
    }

    protected override TagHelperResolutionResult ReadFromProperties(JsonDataReader reader)
    {
        Data data = default;
        reader.ReadProperties(ref data, Data.PropertyMap);

        return new(data.Descriptors, data.Diagnostics);
    }

    protected override void WriteProperties(JsonDataWriter writer, TagHelperResolutionResult value)
    {
        writer.WriteArray(nameof(value.Descriptors), value.Descriptors, ObjectWriters.Write);
        writer.WriteArray(nameof(value.Diagnostics), value.Diagnostics, ObjectWriters.Write);
    }
}
