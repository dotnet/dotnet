// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Razor.Serialization.Json;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Resolvers;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Serialization;

public class RazorProjectInfoSerializationBenchmark
{
    [AllowNull]
    private ArrayBufferWriter<byte> _buffer;
    private ReadOnlyMemory<byte> _projectInfoMessagePackBytes;

    private static readonly MessagePackSerializerOptions s_options = MessagePackSerializerOptions.Standard
        .WithResolver(CompositeResolver.Create(
            RazorProjectInfoResolver.Instance,
            StandardResolver.Instance));

    [ParamsAllValues]
    public ResourceSet ResourceSet { get; set; }

    private RazorProjectInfo ProjectInfo
        => ResourceSet switch
        {
            ResourceSet.Telerik => CommonResources.TelerikProjectInfo,
            _ => CommonResources.LegacyProjectInfo
        };

    private byte[] ProjectInfoJsonBytes
        => ResourceSet switch
        {
            ResourceSet.Telerik => CommonResources.TelerikProjectInfoJsonBytes,
            _ => CommonResources.LegacyProjectInfoJsonBytes
        };

    private static RazorProjectInfo DeserializeProjectInfo_Json(TextReader reader)
        => JsonDataConvert.DeserializeProjectInfo(reader);

    private static void SerializeProjectInfo_Json(TextWriter writer, RazorProjectInfo projectInfo)
        => JsonDataConvert.Serialize(projectInfo, writer);

    [Benchmark(Description = "Serialize RazorProjectInfo (JSON)")]
    public void Serialize_Json()
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 4096);

        SerializeProjectInfo_Json(writer, ProjectInfo);
    }

    [Benchmark(Description = "Deserialize RazorProjectInfo (JSON)")]
    public void Deserialize_Json()
    {
        using var stream = new MemoryStream(ProjectInfoJsonBytes);
        using var reader = new StreamReader(stream);

        var projectInfo = DeserializeProjectInfo_Json(reader);

        if (projectInfo.ProjectWorkspaceState is null ||
            projectInfo.ProjectWorkspaceState.TagHelpers.Count != ProjectInfo.ProjectWorkspaceState.TagHelpers.Count)
        {
            throw new InvalidDataException();
        }
    }

    [Benchmark(Description = "RoundTrip RazorProjectInfo (JSON)")]
    public void RoundTrip_Json()
    {
        using var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 4096, leaveOpen: true))
        {
            SerializeProjectInfo_Json(writer, ProjectInfo);
        }

        stream.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(stream);

        var projectInfo = DeserializeProjectInfo_Json(reader);

        if (projectInfo.ProjectWorkspaceState is null ||
            projectInfo.ProjectWorkspaceState.TagHelpers.Count != ProjectInfo.ProjectWorkspaceState.TagHelpers.Count)
        {
            throw new InvalidDataException();
        }
    }

    [GlobalSetup(Targets = [nameof(Serialize_MessagePack), nameof(Deserialize_MessagePack), nameof(RoundTrip_MessagePack)])]
    public void GlobalSetup_MessagePack()
    {
        _buffer = new ArrayBufferWriter<byte>(initialCapacity: 1024 * 1024);
        _projectInfoMessagePackBytes = SerializeProjectInfo_MessagePack(ProjectInfo);
    }

    private static RazorProjectInfo DeserializeProjectInfo_MessagePack(ReadOnlyMemory<byte> bytes)
    {
        return MessagePackSerializer.Deserialize<RazorProjectInfo>(bytes, s_options);
    }

    private ReadOnlyMemory<byte> SerializeProjectInfo_MessagePack(RazorProjectInfo projectInfo)
    {
        MessagePackSerializer.Serialize(_buffer, projectInfo, s_options);

        return _buffer.WrittenMemory;
    }

    [Benchmark(Description = "Serialize ProjectRazorJson (MessagePack)")]
    public void Serialize_MessagePack()
    {
        SerializeProjectInfo_MessagePack(ProjectInfo);
        _buffer.Clear();

    }

    [Benchmark(Description = "Deserialize ProjectRazorJson (MessagePack)")]
    public void Deserialize_MessagePack()
    {
        var projectInfo = DeserializeProjectInfo_MessagePack(_projectInfoMessagePackBytes);

        if (projectInfo.ProjectWorkspaceState is null ||
            projectInfo.ProjectWorkspaceState.TagHelpers.Count != ProjectInfo.ProjectWorkspaceState.TagHelpers.Count)
        {
            throw new InvalidDataException();
        }
    }

    [Benchmark(Description = "RoundTrip ProjectRazorJson (MessagePack)")]
    public void RoundTrip_MessagePack()
    {
        var bytes = SerializeProjectInfo_MessagePack(ProjectInfo);
        var projectInfo = DeserializeProjectInfo_MessagePack(bytes);
        _buffer.Clear();

        if (projectInfo.ProjectWorkspaceState is null ||
            projectInfo.ProjectWorkspaceState.TagHelpers.Count != ProjectInfo.ProjectWorkspaceState.TagHelpers.Count)
        {
            throw new InvalidDataException();
        }
    }
}
