// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Arcade.Sdk.Tests
{
    [Collection(TestProjectCollection.Name)]
    public class ParallelTestTfmsTests
    {
        private readonly ITestOutputHelper _output;
        private readonly TestProjectFixture _fixture;

        public ParallelTestTfmsTests(ITestOutputHelper output, TestProjectFixture fixture)
        {
            _output = output;
            _fixture = fixture;
        }

        [Fact]
        public void TargetFrameworksRunInParallel()
        {
            var app = _fixture.CreateTestApp("ParallelTestTfms");
            var startInfo = new ProcessStartInfo
            {
                FileName = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet",
                WorkingDirectory = app.WorkingDirectory,
            };
            startInfo.ArgumentList.Add("msbuild");
            startInfo.ArgumentList.Add("ParallelTestTfms.proj");
            startInfo.ArgumentList.Add("/t:Test");
            startInfo.ArgumentList.Add("/m:4");
            startInfo.ArgumentList.Add("/nr:false");

            var exitCode = app.Run(_output, startInfo);

            Assert.Equal(0, exitCode);
            Assert.True(File.Exists(Path.Combine(app.WorkingDirectory, "artifacts", "tmp", "test-tfm-a.started")));
            Assert.True(File.Exists(Path.Combine(app.WorkingDirectory, "artifacts", "tmp", "test-tfm-b.started")));
        }

        [Fact]
        public void ExplicitTargetFrameworkRunsOnlyThatTargetFramework()
        {
            var app = _fixture.CreateTestApp("ParallelTestTfms");
            var startInfo = new ProcessStartInfo
            {
                FileName = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet",
                WorkingDirectory = app.WorkingDirectory,
            };
            startInfo.ArgumentList.Add("msbuild");
            startInfo.ArgumentList.Add("ParallelTestTfms.proj");
            startInfo.ArgumentList.Add("/t:Test");
            startInfo.ArgumentList.Add("/p:TargetFramework=test-tfm-a");
            startInfo.ArgumentList.Add("/p:ExpectedStartedTfms=test-tfm-a");
            startInfo.ArgumentList.Add("/nr:false");

            var exitCode = app.Run(_output, startInfo);

            Assert.Equal(0, exitCode);
            Assert.True(File.Exists(Path.Combine(app.WorkingDirectory, "artifacts", "tmp", "test-tfm-a.started")));
            Assert.False(File.Exists(Path.Combine(app.WorkingDirectory, "artifacts", "tmp", "test-tfm-b.started")));
        }
    }
}
