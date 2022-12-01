// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces.Serialization;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

public class ProjectRazorJsonSerializationBenchmark
{
    // Hardcoded expectations from `ProjectSystem\project.razor.json`
    private const string ExpectedFilePath = "C:\\Users\\admin\\location\\blazorserver\\blazorserver.csproj";
    private const int ExpectedTagHelperCount = 228;

    private JsonSerializer Serializer { get; set; }
    private JsonReader Reader { get; set; }
    private byte[] ProjectRazorJsonBytes { get; set; }

    [IterationSetup]
    public void Setup()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current != null && !File.Exists(Path.Combine(current.FullName, "ProjectSystem", "project.razor.json")))
        {
            current = current.Parent;
        }

        var projectRazorJsonFilePath = Path.Combine(current.FullName, "ProjectSystem", "project.razor.json");
        ProjectRazorJsonBytes = File.ReadAllBytes(projectRazorJsonFilePath);

        Serializer = new JsonSerializer();
        Serializer.Converters.RegisterRazorConverters();
        Serializer.Converters.Add(ProjectRazorJsonJsonConverter.Instance);
    }

    [Benchmark(Description = "Razor FullProjectSnapshotHandle Roundtrip JsonConverter Serialization")]
    public void TagHelper_JsonConvert_Serialization_RoundTrip()
    {
        var stream = new MemoryStream(ProjectRazorJsonBytes);
        Reader = new JsonTextReader(new StreamReader(stream));

        Reader.Read();

        var res = ProjectRazorJsonJsonConverter.Instance.ReadJson(Reader, typeof(ProjectRazorJson), null, Serializer) as ProjectRazorJson;

        if (res.FilePath != ExpectedFilePath ||
            res.ProjectWorkspaceState.TagHelpers.Count != ExpectedTagHelperCount)
        {
            throw new InvalidDataException();
        }
    }
}
