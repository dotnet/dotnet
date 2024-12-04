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
        [InlineData("9.0.100", "true", "direct")]
        // .NET 8 SDK
        [InlineData(null, "true", "direct")]
        // non-SDK style project
        [InlineData(null, null, "direct")]
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
    }
}
