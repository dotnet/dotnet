// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace Msbuild.Integration.Test
{
    public class NuGetAuditDefaultsTests : IClassFixture<MsbuildIntegrationTestFixture>
    {
        private MsbuildIntegrationTestFixture _fixture;
        private ITestOutputHelper _output;

        public NuGetAuditDefaultsTests(MsbuildIntegrationTestFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
        }

        [Theory]
        // .NET 9 SDK
        [InlineData("9.0.100", "true", "all")]
        // .NET 8 SDK
        [InlineData(null, "true", "direct")]
        // non-SDK style project
        [InlineData(null, null, "all")]
        // non-SDK style project explicit opt-out
        [InlineData("8.0", null, "direct")]
        public void NuGetAuditModeDefaults(string SdkAnalysisLevel, string UsingMicrosoftNETSdk, string expected)
        {
            // Arrange
            using var testDirectory = TestDirectory.Create();

            string projectText = @"<Project>
    <Import Project=""$(NuGetRestoreTargets)"" />
</Project>";
            var projectFilePath = Path.Combine(testDirectory, "my.proj");
            File.WriteAllText(projectFilePath, projectText);

            string args = $"{projectFilePath} -getProperty:NuGetAuditMode";
            if (!string.IsNullOrEmpty(SdkAnalysisLevel)) args += $" -p:SdkAnalysisLevel={SdkAnalysisLevel}";
            if (!string.IsNullOrEmpty(UsingMicrosoftNETSdk)) args += $" -p:UsingMicrosoftNETSdk={UsingMicrosoftNETSdk}";

            // Act
            var result = _fixture.RunMsBuild(testDirectory, args);
            var resultText = result.Output.Trim();

            // Assert
            resultText.Should().Be(expected);
        }

        [Fact]
        // Some customers run the latest Nuget.exe with old versions of MSBuild. MSBuild only added intrinsic functions
        // needed for SdkAnalysisVersion comparisons in dev16, so NuGet.exe detects the MSBuild version and adds a
        // global property when invoking MSBuild, so that our MSBuild script can avoid calling intrinstic functions
        // that don't exist. In order to test this with a "real" integration test, we'd need an old version of MSBuild
        // installed, which is not worth the effort, assuming it's even feasible. Therefore, while this test can't
        // validate that the intrinstic functions are not called, it can set the same global property and validate that
        // the default values are what we expect.
        public void NuGetAuditModeDefaults_NuGetExeWithOldMSBuildEmulation()
        {
            // Arrange
            using var testDirectory = TestDirectory.Create();

            string projectText = @"<Project>
    <Import Project=""$(NuGetRestoreTargets)"" />
</Project>";
            var projectFilePath = Path.Combine(testDirectory, "my.proj");
            File.WriteAllText(projectFilePath, projectText);

            string args = $"{projectFilePath} -getProperty:NuGetAuditMode -p:NuGetExeSkipSdkAnalysisLevelCheck=true";

            // Act
            var result = _fixture.RunMsBuild(testDirectory, args);
            var resultText = result.Output.Trim();

            // Assert
            resultText.Should().Be("all");
        }
    }
}
