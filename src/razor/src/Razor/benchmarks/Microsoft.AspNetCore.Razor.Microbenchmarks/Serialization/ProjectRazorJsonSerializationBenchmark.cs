// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Microsoft.CodeAnalysis.Razor;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Serialization;

public class ProjectRazorJsonSerializationBenchmark
{
    // Hardcoded expectations from `Resources\project.razor.json`
    private const string ExpectedFilePath = @"C:\Users\admin\location\blazorserver\blazorserver.csproj";
    private const int ExpectedTagHelperCount = 228;

    private JsonSerializer? _serializer;
    private byte[]? _projectRazorJsonBytes;

    private JsonSerializer Serializer => _serializer.AssumeNotNull();
    private byte[] ProjectRazorJsonBytes => _projectRazorJsonBytes.AssumeNotNull();

    [GlobalSetup]
    public void Setup()
    {
        _projectRazorJsonBytes = Resources.GetResourceBytes("project.razor.json");

        _serializer = new JsonSerializer();
        _serializer.Converters.RegisterRazorConverters();
        _serializer.Converters.Add(ProjectRazorJsonJsonConverter.Instance);
    }

    [Benchmark(Description = "Razor ProjectRazorJson Roundtrip JsonConverter Serialization")]
    public void ProjectRazorJson_JsonConverter_Serialization_RoundTrip()
    {
        using var stream = new MemoryStream(ProjectRazorJsonBytes);
        using var reader = new JsonTextReader(new StreamReader(stream));

        reader.Read();

        var result = ProjectRazorJsonJsonConverter.Instance.ReadJson(reader, typeof(ProjectRazorJson), null, Serializer) as ProjectRazorJson;

        if (result is null ||
            result.FilePath != ExpectedFilePath ||
            result.ProjectWorkspaceState?.TagHelpers.Count != ExpectedTagHelperCount)
        {
            throw new InvalidDataException();
        }
    }
}
