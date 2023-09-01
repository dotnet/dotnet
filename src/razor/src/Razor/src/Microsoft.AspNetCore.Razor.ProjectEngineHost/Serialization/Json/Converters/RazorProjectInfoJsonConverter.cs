// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Serialization.Json.Converters;

internal class RazorProjectInfoJsonConverter : ObjectJsonConverter<RazorProjectInfo>
{
    public static readonly RazorProjectInfoJsonConverter Instance = new();

    private RazorProjectInfoJsonConverter()
    {
    }

    protected override RazorProjectInfo ReadFromProperties(JsonDataReader reader)
        => ObjectReaders.ReadRazorProjectInfoFromProperties(reader);

    protected override void WriteProperties(JsonDataWriter writer, RazorProjectInfo value)
        => ObjectWriters.WriteProperties(writer, value);
}
