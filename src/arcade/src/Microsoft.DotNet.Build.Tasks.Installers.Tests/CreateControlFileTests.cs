// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using AwesomeAssertions;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Installers.Tests
{
    public class CreateControlFileTests : IDisposable
    {
        private readonly string _tempDir;

        public CreateControlFileTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "debcontroltests-" + Guid.NewGuid().ToString("n"));
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            try
            {
                Directory.Delete(_tempDir, recursive: true);
            }
            catch
            {
                // Best-effort cleanup.
            }
        }

        private CreateControlFile CreateTask(string outputPath) => new()
        {
            BuildEngine = new MockBuildEngine(),
            PackageName = "dotnet-host",
            PackageVersion = "11.0.0",
            PackageArchitecture = "amd64",
            Maintainer = "Test Maintainer",
            Description = "Test description",
            InstalledSize = "1024",
            Depends = Array.Empty<ITaskItem>(),
            Section = "devel",
            ControlFileOutputPath = outputPath,
        };

        [Fact]
        public void ReplacesAdditionalProperty_IsEmittedVerbatim()
        {
            // The DEB half of the host dnx fix expresses a bounded Replaces on the released
            // dotnet-sdk-10.0 package via a DebControlProperty. Prove the control file emits
            // the exact bounded constraint (including the version relation) unmodified.
            string outputPath = Path.Combine(_tempDir, "control");
            CreateControlFile task = CreateTask(outputPath);

            ITaskItem replaces = new TaskItem("Replaces");
            replaces.SetMetadata("Value", "dotnet-sdk-10.0 (<< 10.1.0)");
            task.AdditionalProperties = [replaces];

            task.Execute().Should().BeTrue();

            string content = File.ReadAllText(outputPath);
            content.Should().Contain("Replaces: dotnet-sdk-10.0 (<< 10.1.0)");
        }
    }
}
