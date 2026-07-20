// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Microsoft.Arcade.Test.Common;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Templating.Tests
{
    public class GenerateFileFromTemplateTests
    {
        [Fact]
        public void GenerateFileFromTemplate_SubstitutesValidProperties()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            string filePath = Path.Combine(tempDir, "Directory.Build.props");

            try
            {
                GenerateFileFromTemplate task = new();
                task.TemplateFile = GetFullPath("Directory.Build.props.in");
                task.OutputPath = filePath;
                task.Properties = new[] { "DefaultNetCoreTargetFramework=net6.0" };

                Assert.True(task.Execute());
                Assert.Equal(ReadAllText("Directory.Build.props.in").Replace("${DefaultNetCoreTargetFramework}", "net6.0"), File.ReadAllText(filePath));
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Theory]
        [InlineData("DefaultNetCoreTargetFramework=")]
        [InlineData("=net6.0")]
        [InlineData("net6.0")]
        [InlineData("DefaultNetCoreTargetFramework:net6.0")]
        [InlineData("Default_NetCore_Target_Framework=net6.0")]
        public void GenerateFileFromTemplate_RemovesInvalidProperties(string invalidProperty)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            string filePath = Path.Combine(tempDir, "Directory.Build.props");

            try
            {
                GenerateFileFromTemplate task = new();
                task.BuildEngine = new MockBuildEngine();
                task.TemplateFile = GetFullPath("Directory.Build.props.in");
                task.OutputPath = filePath;
                task.Properties = new[] { invalidProperty };

                Assert.True(task.Execute());
                Assert.Equal(ReadAllText("Directory.Build.props.in").Replace("${DefaultNetCoreTargetFramework}", string.Empty), File.ReadAllText(filePath));
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Theory]
        [InlineData("Directory.Build.props.malformedbraces.in")]
        [InlineData("Directory.Build.props.nobraces.in")]
        public void GenerateFileFromTemplate_IgnoresMalformedTemplate(string filename)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            string filePath = Path.Combine(tempDir, "Directory.Build.props");

            try
            {
                GenerateFileFromTemplate task = new();
                task.BuildEngine = new MockBuildEngine();
                task.TemplateFile = GetFullPath(filename);
                task.OutputPath = filePath;
                task.Properties = new[] { "DefaultNetCoreTargetFramework=net6.0" };

                Assert.True(task.Execute());
                Assert.Equal(ReadAllText(filename), File.ReadAllText(filePath));
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void GenerateFileFromTemplate_SkipUnchanged_DoesNotRewriteUnchangedFile()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            string filePath = Path.Combine(tempDir, "Directory.Build.props");

            try
            {
                GenerateFileFromTemplate task = new();
                task.BuildEngine = new MockBuildEngine();
                task.TemplateFile = GetFullPath("Directory.Build.props.in");
                task.OutputPath = filePath;
                task.Properties = new[] { "DefaultNetCoreTargetFramework=net6.0" };
                task.SkipUnchanged = true;

                Assert.True(task.Execute());

                // Move the timestamp into the past so a rewrite would be observable.
                // Capture the on-disk value after setting it, since the filesystem may round it.
                File.SetLastWriteTimeUtc(filePath, DateTime.UtcNow.AddDays(-1));
                DateTime originalWriteTime = File.GetLastWriteTimeUtc(filePath);
                long originalLength = new FileInfo(filePath).Length;

                Assert.True(task.Execute());

                Assert.Equal(originalWriteTime, File.GetLastWriteTimeUtc(filePath));
                Assert.Equal(originalLength, new FileInfo(filePath).Length);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void GenerateFileFromTemplate_SkipUnchanged_RewritesWhenContentsDiffer()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            string filePath = Path.Combine(tempDir, "Directory.Build.props");

            try
            {
                Directory.CreateDirectory(tempDir);
                File.WriteAllText(filePath, "stale contents");

                GenerateFileFromTemplate task = new();
                task.BuildEngine = new MockBuildEngine();
                task.TemplateFile = GetFullPath("Directory.Build.props.in");
                task.OutputPath = filePath;
                task.Properties = new[] { "DefaultNetCoreTargetFramework=net6.0" };
                task.SkipUnchanged = true;

                Assert.True(task.Execute());
                Assert.Equal(ReadAllText("Directory.Build.props.in").Replace("${DefaultNetCoreTargetFramework}", "net6.0"), File.ReadAllText(filePath));
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void GenerateFileFromTemplate_SkipUnchanged_WritesWhenFileMissing()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            string filePath = Path.Combine(tempDir, "Directory.Build.props");

            try
            {
                GenerateFileFromTemplate task = new();
                task.BuildEngine = new MockBuildEngine();
                task.TemplateFile = GetFullPath("Directory.Build.props.in");
                task.OutputPath = filePath;
                task.Properties = new[] { "DefaultNetCoreTargetFramework=net6.0" };
                task.SkipUnchanged = true;

                Assert.True(task.Execute());
                Assert.True(File.Exists(filePath));
                Assert.Equal(ReadAllText("Directory.Build.props.in").Replace("${DefaultNetCoreTargetFramework}", "net6.0"), File.ReadAllText(filePath));
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        public static string GetFullPath(string relativeTestInputPath)
        {
            return Path.Combine(
                Path.GetDirectoryName(typeof(GenerateFileFromTemplateTests).Assembly.Location),
                "testassets",
                relativeTestInputPath);
        }

        public static string ReadAllText(string relativeTestInputPath)
        {
            string path = GetFullPath(relativeTestInputPath);
            return File.ReadAllText(path);
        }
    }
}
