// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using Microsoft.Arcade.Test.Common;
using Xunit;

namespace Microsoft.DotNet.CMake.Sdk.Tests;

public class GetCMakeArtifactsFromFileApiTests : IDisposable
{
    private readonly string _outputDirectory = Directory.CreateTempSubdirectory("CMakeArtifacts-").FullName;

    public GetCMakeArtifactsFromFileApiTests()
    {
        string replyDirectory = Path.Combine(_outputDirectory, ".cmake", "api", "v1", "reply");
        Directory.CreateDirectory(replyDirectory);
        File.WriteAllText(Path.Combine(replyDirectory, "index-1.json"),
            """{"reply":{"client-Microsoft.DotNet.CMake.Sdk":{"codemodel-v2":{"jsonFile":"codemodel.json"}}}}""");
        File.WriteAllText(Path.Combine(replyDirectory, "codemodel.json"),
            """{"configurations":[{"name":"Debug","targets":[{"name":"Widget","jsonFile":"upper.json"},{"name":"widget","jsonFile":"lower.json"}]}]}""");
        File.WriteAllText(Path.Combine(replyDirectory, "upper.json"),
            """{"artifacts":[{"path":"upper.bin"}]}""");
        File.WriteAllText(Path.Combine(replyDirectory, "lower.json"),
            """{"artifacts":[{"path":"lower.bin"}]}""");
    }

    [Fact]
    public void SelectsOnlyTheTargetWithTheRequestedCase()
    {
        var engine = new MockBuildEngine();
        var task = CreateTask(engine, "widget");

        Assert.True(task.Execute(), engine.ErrorSummary);
        Assert.Equal(Path.Combine(_outputDirectory, "lower.bin"), Assert.Single(task.Artifacts).ItemSpec);
    }

    [Fact]
    public void DoesNotSelectTargetsWithDifferentCase()
    {
        var engine = new MockBuildEngine();
        var task = CreateTask(engine, "WIDGET");

        Assert.True(task.Execute(), engine.ErrorSummary);
        Assert.Empty(task.Artifacts);
        Assert.Contains("WIDGET", Assert.Single(engine.BuildWarningEvents).Message, StringComparison.Ordinal);
    }

    [Fact]
    public void SelectsArtifactsWhenAnotherTargetIsNotInTheCodeModel()
    {
        var engine = new MockBuildEngine();
        var task = CreateTask(engine, "Widget;install");

        Assert.True(task.Execute(), engine.ErrorSummary);
        Assert.Equal(Path.Combine(_outputDirectory, "upper.bin"), Assert.Single(task.Artifacts).ItemSpec);
    }

    [Fact]
    public void ReadingPrebuiltOutputDoesNotModifyTheBuildTree()
    {
        string[] filesBefore = Directory.GetFiles(_outputDirectory, "*", SearchOption.AllDirectories).Order().ToArray();
        var engine = new MockBuildEngine();

        Assert.True(CreateTask(engine, "Widget").Execute(), engine.ErrorSummary);
        Assert.Equal(filesBefore, Directory.GetFiles(_outputDirectory, "*", SearchOption.AllDirectories).Order().ToArray());
    }

    private GetCMakeArtifactsFromFileApi CreateTask(MockBuildEngine engine, string targets) => new()
    {
        BuildEngine = engine,
        CMakeOutputDir = _outputDirectory,
        CMakeTargets = targets,
        Configuration = "Debug"
    };

    public void Dispose() => Directory.Delete(_outputDirectory, recursive: true);
}
