// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Serialization;

public class ProjectRazorJsonSerializationBenchmark
{
    [ParamsAllValues]
    public ResourceSet ResourceSet { get; set; }

    private ProjectRazorJson ProjectRazorJson
        => ResourceSet switch
        {
            ResourceSet.Telerik => CommonResources.TelerikProjectRazorJson,
            _ => CommonResources.LegacyProjectRazorJson
        };

    private byte[] ProjectRazorJsonBytes
        => ResourceSet switch
        {
            ResourceSet.Telerik => CommonResources.TelerikProjectRazorJsonBytes,
            _ => CommonResources.LegacyProjectRazorJsonBytes
        };

    private static ProjectRazorJson DeserializeProjectRazorJson(TextReader reader)
    {
        return JsonDataConvert.DeserializeData(reader,
            static r => r.ReadNonNullObject(ObjectReaders.ReadProjectRazorJsonFromProperties));
    }

    private static void SerializeProjectRazorJson(TextWriter writer, ProjectRazorJson projectRazorJson)
    {
        JsonDataConvert.SerializeObject(writer, projectRazorJson, ObjectWriters.WriteProperties);
    }

    [Benchmark(Description = "Serialize ProjectRazorJson")]
    public void Serialize()
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 4096);

        SerializeProjectRazorJson(writer, ProjectRazorJson);
    }

    [Benchmark(Description = "Deserialize ProjectRazorJson")]
    public void Deserialize()
    {
        using var stream = new MemoryStream(ProjectRazorJsonBytes);
        using var reader = new StreamReader(stream);

        var projectRazorJson = DeserializeProjectRazorJson(reader);

        if (projectRazorJson.ProjectWorkspaceState is null ||
            projectRazorJson.ProjectWorkspaceState.TagHelpers.Length != ProjectRazorJson.ProjectWorkspaceState?.TagHelpers.Length)
        {
            throw new InvalidDataException();
        }
    }

    [Benchmark(Description = "RoundTrip ProjectRazorJson")]
    public void RoundTrip()
    {
        using var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 4096, leaveOpen: true))
        {
            SerializeProjectRazorJson(writer, ProjectRazorJson);
        }

        stream.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(stream);

        var projectRazorJson = DeserializeProjectRazorJson(reader);

        if (projectRazorJson.ProjectWorkspaceState is null ||
            projectRazorJson.ProjectWorkspaceState.TagHelpers.Length != ProjectRazorJson.ProjectWorkspaceState?.TagHelpers.Length)
        {
            throw new InvalidDataException();
        }
    }
}
