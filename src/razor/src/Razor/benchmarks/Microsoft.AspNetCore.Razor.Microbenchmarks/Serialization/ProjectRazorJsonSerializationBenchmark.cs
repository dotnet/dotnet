// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Serialization;

public class ProjectRazorJsonSerializationBenchmark
{
    // Hardcoded expectations from `Resources\project.razor.json`
    private const string ExpectedFilePath = @"C:\Users\admin\location\blazorserver\blazorserver.csproj";
    private const int ExpectedTagHelperCount = 228;

    private byte[]? _projectRazorJsonBytes;

    private byte[] ProjectRazorJsonBytes => _projectRazorJsonBytes.AssumeNotNull();

    [GlobalSetup]
    public void Setup()
    {
        _projectRazorJsonBytes = Resources.GetResourceBytes("project.razor.json");
    }

    [Benchmark(Description = "Razor ProjectRazorJson Roundtrip JsonConverter Serialization")]
    public void ProjectRazorJson_Serialization_RoundTrip()
    {
        using var stream = new MemoryStream(ProjectRazorJsonBytes);
        using var jsonReader = new JsonTextReader(new StreamReader(stream));

        jsonReader.Read();

        var dataReader = JsonDataReader.Get(jsonReader);
        try
        {
            var result = dataReader.ReadObject(ObjectReaders.ReadProjectRazorJsonFromProperties);

            if (result is null ||
                result.FilePath != ExpectedFilePath ||
                result.ProjectWorkspaceState?.TagHelpers.Count != ExpectedTagHelperCount)
            {
                throw new InvalidDataException();
            }
        }
        finally
        {
            JsonDataReader.Return(dataReader);
            jsonReader.Close();
            stream.Close();
        }
    }
}
