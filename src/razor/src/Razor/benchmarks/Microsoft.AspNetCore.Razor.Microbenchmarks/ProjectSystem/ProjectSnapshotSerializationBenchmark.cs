// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Diagnostics;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Serialization.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.ProjectSystem;

public class ProjectSnapshotSerializationBenchmark : ProjectSnapshotManagerBenchmarkBase
{
    public ProjectSnapshotSerializationBenchmark()
    {
        // Deserialize from json file.
        Serializer = new JsonSerializer();
        Serializer.Converters.RegisterRazorConverters();

        var snapshotManager = CreateProjectSnapshotManager();
        snapshotManager.ProjectAdded(HostProject);
        var projectSnapshot = snapshotManager.GetLoadedProject(HostProject.Key);
        Debug.Assert(projectSnapshot != null);
        ProjectSnapshotHandle = new ProjectSnapshotHandle(ProjectId.CreateNewId(), projectSnapshot.Configuration, projectSnapshot.RootNamespace);
    }

    public JsonSerializer Serializer { get; set; }
    private ProjectSnapshotHandle ProjectSnapshotHandle { get; }

    [Benchmark(Description = "Razor ProjectSnapshot Roundtrip JsonConverter Serialization")]
    public void TagHelper_JsonConvert_Serialization_RoundTrip()
    {
        MemoryStream originalStream;
        using (originalStream = new MemoryStream())
        using (var writer = new StreamWriter(originalStream, Encoding.UTF8, bufferSize: 4096))
        {
            Serializer.Serialize(writer, ProjectSnapshotHandle);
        }

        ProjectSnapshotHandle deserializedResult;
        var stream = new MemoryStream(originalStream.GetBuffer());
        using (stream)
        using (var reader = new JsonTextReader(new StreamReader(stream)))
        {
            deserializedResult = Serializer.Deserialize<ProjectSnapshotHandle>(reader);
        }
    }
}
