// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System;
using NuGet.CommandLine.Xplat.Tests;
using NuGet.Test.Utility;
using Xunit;
using System.IO;
using System.Reflection;

namespace Dotnet.Integration.Test
{
    [Collection(DotnetIntegrationCollection.Name)]
    public class DotnetPackageSearchTests : IClassFixture<PackageSearchRunnerFixture>
    {
        private readonly DotnetIntegrationTestFixture _testFixture;
        private readonly PackageSearchRunnerFixture _packageSearchRunnerFixture;

        public DotnetPackageSearchTests(DotnetIntegrationTestFixture testFixture, PackageSearchRunnerFixture packageSearchRunnerFixture)
        {
            _testFixture = testFixture;
            _packageSearchRunnerFixture = packageSearchRunnerFixture;
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
                var args = new string[] { "package", "search", "json", "--source", $"{_packageSearchRunnerFixture.ServerWithMultipleEndpoints.Uri}v3/index.json", "--format", "json" };

                // Act
                var result = _testFixture.RunDotnetExpectSuccess(pathContext.PackageSource, string.Join(" ", args));

                // Assert
                Assert.Equal(0, result.ExitCode);
                Assert.Contains("\"total downloads\": 531607259,", result.AllOutput);
                Assert.Contains("\"owners\": \"James Newton-King\",", result.AllOutput);
                Assert.Contains("\"total downloads\": 531607259,", result.AllOutput);
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
                Dictionary<string, string> finalEnvironmentVariables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["MSBuildSDKsPath"] = _testFixture.MsBuildSdksPath,
                    ["UseSharedCompilation"] = bool.FalseString,
                    ["DOTNET_MULTILEVEL_LOOKUP"] = "0",
                    ["DOTNET_ROOT"] = TestDotnetCLiUtility.CopyAndPatchLatestDotnetCli(Path.GetFullPath(Assembly.GetExecutingAssembly().Location)),
                    ["MSBUILDDISABLENODEREUSE"] = bool.TrueString,
                    ["NUGET_SHOW_STACK"] = bool.TrueString
                };

                string error = "is invalid. Provide a valid source.";
                string help = "dotnet package search [<SearchTerm>] [options]";
                // Act
                var result = CommandRunner.Run(_testFixture.TestDotnetCli, pathContext.PackageSource, string.Join(" ", args), environmentVariables: finalEnvironmentVariables);

                // Assert
                Assert.Contains(error, result.AllOutput);
                Assert.DoesNotContain(help, result.AllOutput);
            }
        }
    }
}
