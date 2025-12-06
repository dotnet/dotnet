// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using System.IO;
using FluentAssertions;
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
        public void DotnetPackageSearch_WithJsonOutput_Succeed()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Arrange
                string source = $"{_packageSearchRunnerFixture.ServerWithMultipleEndpoints.Uri}v3/index.json";
                pathContext.Settings.AddSource(source, source, allowInsecureConnectionsValue: "true");
                var args = $"package search json --take 10 --prerelease --source {_packageSearchRunnerFixture.ServerWithMultipleEndpoints.Uri}v3/index.json --format json";

                // Act
                var result = _testFixture.RunDotnetExpectSuccess(pathContext.PackageSource, args, testOutputHelper: _testOutputHelper);

                // Assert
                Assert.Equal(0, result.ExitCode);
                Assert.Contains("\"id\": \"Fake.Newtonsoft.Json\",", result.AllOutput);
                Assert.Contains("\"owners\": \"James Newton-King\"", result.AllOutput);
                Assert.Contains("\"totalDownloads\": 531607259,", result.AllOutput);
                Assert.Contains("\"latestVersion\": \"12.0.3\"", result.AllOutput);
            }
        }

        [Fact]
        public void DotnetPackageSearch_WithConsoleOutput_Succeed()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Arrange
                string source = $"{_packageSearchRunnerFixture.ServerWithMultipleEndpoints.Uri}v3/index.json";
                pathContext.Settings.AddSource(source, source, allowInsecureConnectionsValue: "true");
                var args = $"package search json --take 10 --prerelease --source {_packageSearchRunnerFixture.ServerWithMultipleEndpoints.Uri}v3/index.json";

                // Act
                var result = _testFixture.RunDotnetExpectSuccess(pathContext.PackageSource, args, testOutputHelper: _testOutputHelper);

                // Assert
                result.ExitCode.Should().Be(0, result.AllOutput);
                List<string> table = new();
                using (var stringReader = new StringReader(result.AllOutput))
                {
                    string line;
                    while ((line = stringReader.ReadLine()) != null)
                    {
                        if (line.Length > 0 && line[0] == '|')
                        {
                            table.Add(line);
                        }
                    }
                }

                table.Count.Should().Be(4, $"Unexpected table in output: {string.Join("\n", table)}");
                table[0].Should().Be("| Package ID           | Latest Version | Owners            | Total Downloads |");
                table[1].Should().Be("| -------------------- | -------------- | ----------------- | --------------- |");
                table[2].Should().Be("| Fake.Newtonsoft.Json | 12.0.3         | James Newton-King | 531,607,259     |");
                table[3].Should().Be("| -------------------- | -------------- | ----------------- | --------------- |");
            }
        }

        [Fact]
        public void DotnetPackageSearch_WithInvalidSource_FailWithNoHelpOutput()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Arrange
                string source = "invalid-source";
                var args = $"package search json --source {source} --format json";

                string error = "is invalid. Provide a valid source.";
                string help = "dotnet package search [<SearchTerm>] [options]";

                // Act
                var result = _testFixture.RunDotnetExpectFailure(pathContext.SolutionRoot, args, testOutputHelper: _testOutputHelper);

                // Assert
                Assert.Contains(error, result.AllOutput);
                Assert.DoesNotContain(help, result.AllOutput);
            }
        }
    }
}
