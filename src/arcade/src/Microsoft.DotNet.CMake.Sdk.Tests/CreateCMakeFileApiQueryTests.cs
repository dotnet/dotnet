// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Microsoft.Arcade.Test.Common;
using Xunit;

namespace Microsoft.DotNet.CMake.Sdk.Tests;

public class CreateCMakeFileApiQueryTests : IDisposable
{
    private readonly string _outputDirectory = Directory.CreateTempSubdirectory("CMakeQuery-").FullName;

    [Fact]
    public void CreatesEmptyQueryWhileAnotherWriterIsOpen()
    {
        string queryDirectory = Path.Combine(_outputDirectory, ".cmake", "api", "v1", "query", "client-Microsoft.DotNet.CMake.Sdk");
        Directory.CreateDirectory(queryDirectory);
        string queryFile = Path.Combine(queryDirectory, "codemodel-v2");
        using var otherWriter = new FileStream(queryFile, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite);
        otherWriter.WriteByte(42);
        otherWriter.Flush();

        var engine = new MockBuildEngine();
        var task = new CreateCMakeFileApiQuery { BuildEngine = engine, CMakeOutputDir = _outputDirectory };

        Assert.True(task.Execute(), engine.ErrorSummary);
        Assert.Equal(0, new FileInfo(queryFile).Length);
    }

    public void Dispose() => Directory.Delete(_outputDirectory, recursive: true);
}
