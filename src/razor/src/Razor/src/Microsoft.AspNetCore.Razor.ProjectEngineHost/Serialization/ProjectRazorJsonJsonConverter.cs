// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

internal class ProjectRazorJsonJsonConverter : ObjectJsonConverter<ProjectRazorJson>
{
    public static readonly ProjectRazorJsonJsonConverter Instance = new();

    private ProjectRazorJsonJsonConverter()
    {
    }

    protected override ProjectRazorJson ReadFromProperties(JsonDataReader reader)
        => ObjectReaders.ReadProjectRazorJsonFromProperties(reader);

    protected override void WriteProperties(JsonDataWriter writer, ProjectRazorJson value)
        => ObjectWriters.WriteProperties(writer, value);
}
