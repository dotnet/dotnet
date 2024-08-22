// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.CommandLine.Xplat.Tests;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace Dotnet.Integration.Test
{
    [Collection(DotnetIntegrationCollection.Name)]
    public class DotnetPackageSearchTests : IClassFixture<PackageSearchRunnerFixture>
    {
        private readonly DotnetIntegrationTestFixture _testFixture;
        private readonly PackageSearchRunnerFixture _packageSearchRunnerFixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public DotnetPackageSearchTests(DotnetIntegrationTestFixture testFixture, PackageSearchRunnerFixture packageSearchRunnerFixture, ITestOutputHelper testOutputHelper)
        {
            _testFixture = testFixture;
            _packageSearchRunnerFixture = packageSearchRunnerFixture;
            _testOutputHelper = testOutputHelper;
        }

        internal string NormalizeNewlines(string input)
        {
            return input.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        [Fact]
        public void DotnetPackageSearch_Succeed()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Arrange
                var args = new string[] { "package", "search", "json", "--take", "10", "--prerelease", "--source", $"{_packageSearchRunnerFixture.ServerWithMultipleEndpoints.Uri}v3/index.json", "--format", "json" };

                // Act
                var result = _testFixture.RunDotnetExpectSuccess(pathContext.PackageSource, string.Join(" ", args), testOutputHelper: _testOutputHelper);

                // Assert
                Assert.Equal(0, result.ExitCode);
                Assert.Contains("\"id\": \"Fake.Newtonsoft.Json\",", result.AllOutput);
                Assert.Contains("\"owners\": \"James Newton-King\"", result.AllOutput);
                Assert.Contains("\"totalDownloads\": 531607259,", result.AllOutput);
                Assert.Contains("\"latestVersion\": \"12.0.3\"", result.AllOutput);
            }
        }

        [Fact]
        public void DotnetPackageSearch_WithInvalidSource_FailWithNoHelpOutput()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Arrange
                string source = "invalid-source";
                var args = new string[] { "package", "search", "json", "--source", source, "--format", "json" };

                string error = "is invalid. Provide a valid source.";
                string help = "dotnet package search [<SearchTerm>] [options]";

                // Act
                var result = _testFixture.RunDotnetExpectFailure(pathContext.SolutionRoot, string.Join(" ", args), testOutputHelper: _testOutputHelper);

                // Assert
                Assert.Contains(error, result.AllOutput);
                Assert.DoesNotContain(help, result.AllOutput);
            }
        }
    }
}
