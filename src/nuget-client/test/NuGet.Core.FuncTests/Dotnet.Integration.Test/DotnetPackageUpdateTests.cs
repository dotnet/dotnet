// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using FluentAssertions;
using NuGet.CommandLine.Xplat.Tests;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;
using Xunit.Abstractions;

namespace Dotnet.Integration.Test
{
    [Collection(DotnetIntegrationCollection.Name)]
    public class DotnetPackageUpdateTests : IClassFixture<PackageSearchRunnerFixture>
    {
        private readonly DotnetIntegrationTestFixture _testFixture;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly string _xplatCli;
        private readonly IReadOnlyDictionary<string, string> _envVars;

        // The .NET SDK downloads reference assembly packages for target frameworks it can't find ref assemblies for
        // locally. Ideally this should be solved, so it's not necessary to download the packages every time the test
        // runs. At a minimum, this should be changed to use package source mapping, once update package supports PSM.
        private const string NugetConfigFormat = """
            <configuration>
              <packageSources>
                <clear />
                <add key="test" value="{0}" />
                <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
              </packageSources>
              <packageSourceMapping>
                <clear />
              </packageSourceMapping>
            </configuration>
            """;

        public DotnetPackageUpdateTests(DotnetIntegrationTestFixture testFixture, ITestOutputHelper testOutputHelper)
        {
            _testFixture = testFixture;
            _testOutputHelper = testOutputHelper;

            _xplatCli = Path.Combine(_testFixture.SdkDirectory.FullName, "NuGet.CommandLine.XPlat.dll");
            _envVars = new Dictionary<string, string>()
            {
                ["DOTNET_HOST_PATH"] = _testFixture.TestDotnetCli
            };
        }

        [Fact]
        public async Task SingleTfmProject_PackageVersionUpdated()
        {
            // Arrange
            using var testContext = new SimpleTestPathContext();
            File.WriteAllText(testContext.NuGetConfig, string.Format(NugetConfigFormat, testContext.PackageSource));

            var a1 = new SimpleTestPackageContext("NuGet.Internal.Test.a", "1.0.0");
            var a2 = new SimpleTestPackageContext("NuGet.Internal.Test.a", "2.0.0");

            SimpleTestPackageContext[] packages = [a1, a2];
            await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, packages);

            var csprojContents = """
                <Project Sdk="Microsoft.NET.Sdk">
                    <PropertyGroup>
                        <TargetFramework>net9.0</TargetFramework>
                    </PropertyGroup>
                    <ItemGroup>
                        <PackageReference Include="NuGet.Internal.Test.a" Version="1.0.0" />
                    </ItemGroup>
                </Project>
                """;
            var csprojPath = Path.Combine(testContext.SolutionRoot, "my.csproj");
            File.WriteAllText(csprojPath, csprojContents);

            // Act
            var result = _testFixture.RunDotnetExpectSuccess(
                workingDirectory: testContext.SolutionRoot,
                args: $"{_xplatCli} package update NuGet.Internal.Test.a",
                testOutputHelper: _testOutputHelper,
                environmentVariables: _envVars);

            // Assert
            result.ExitCode.Should().Be(0);

            XDocument csproj = XDocument.Load(csprojPath);
            var packageReferenceA = csproj.XPathSelectElements("//PackageReference[@Include='NuGet.Internal.Test.a']").ToList();
            packageReferenceA.Count.Should().Be(1);
            packageReferenceA[0].Attribute("Version").Value.Should().Be("2.0.0");
        }

        [Fact]
        public async Task MultiTfmProject_PackageVersionUpdated()
        {
            // Arrange
            using var testContext = new SimpleTestPathContext();
            File.WriteAllText(testContext.NuGetConfig, string.Format(NugetConfigFormat, testContext.PackageSource));

            var a1 = new SimpleTestPackageContext("NuGet.Internal.Test.a", "1.0.0");
            var a2 = new SimpleTestPackageContext("NuGet.Internal.Test.a", "2.0.0");

            SimpleTestPackageContext[] packages = [a1, a2];
            await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, packages);

            var csprojContents = """
                <Project Sdk="Microsoft.NET.Sdk">
                    <PropertyGroup>
                        <TargetFrameworks>net9.0;net8.0</TargetFrameworks>
                    </PropertyGroup>
                    <ItemGroup>
                        <PackageReference Include="NuGet.Internal.Test.a" Version="1.0.0" />
                    </ItemGroup>
                </Project>
                """;
            var csprojPath = Path.Combine(testContext.SolutionRoot, "my.csproj");
            File.WriteAllText(csprojPath, csprojContents);

            // Act
            var result = _testFixture.RunDotnetExpectSuccess(
                workingDirectory: testContext.SolutionRoot,
                args: $"{_xplatCli} package update NuGet.Internal.Test.a",
                testOutputHelper: _testOutputHelper,
                environmentVariables: _envVars);

            // Assert
            result.ExitCode.Should().Be(0);

            XDocument csproj = XDocument.Load(csprojPath);
            var packageReferenceA = csproj.XPathSelectElements("//PackageReference[@Include='NuGet.Internal.Test.a']").ToList();
            packageReferenceA.Count.Should().Be(1);
            packageReferenceA[0].Attribute("Version").Value.Should().Be("2.0.0");
        }

        [Fact]
        public async Task MultiTfmProjectWithConditionalPackageRef_PackageVersionUpdated()
        {
            // Arrange
            using var testContext = new SimpleTestPathContext();
            File.WriteAllText(testContext.NuGetConfig, string.Format(NugetConfigFormat, testContext.PackageSource));

            var a1 = new SimpleTestPackageContext("NuGet.Internal.Test.a", "1.0.0");
            var a2 = new SimpleTestPackageContext("NuGet.Internal.Test.a", "2.0.0");

            SimpleTestPackageContext[] packages = [a1, a2];
            await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, packages);

            var csprojContents = """
                <Project Sdk="Microsoft.NET.Sdk">
                    <PropertyGroup>
                        <TargetFrameworks>net9.0;net8.0</TargetFrameworks>
                    </PropertyGroup>
                    <ItemGroup>
                        <PackageReference Include="NuGet.Internal.Test.a" Version="1.0.0" Condition=" '$(TargetFramework)' == 'net8.0' " />
                    </ItemGroup>
                </Project>
                """;
            var csprojPath = Path.Combine(testContext.SolutionRoot, "my.csproj");
            File.WriteAllText(csprojPath, csprojContents);

            // Act
            var result = _testFixture.RunDotnetExpectSuccess(
                workingDirectory: testContext.SolutionRoot,
                args: $"{_xplatCli} package update NuGet.Internal.Test.a",
                testOutputHelper: _testOutputHelper,
                environmentVariables: _envVars);

            // Assert
            result.ExitCode.Should().Be(0);

            XDocument csproj = XDocument.Load(csprojPath);
            var packageReferenceA = csproj.XPathSelectElements("//PackageReference[@Include='NuGet.Internal.Test.a']").ToList();
            packageReferenceA.Count.Should().Be(1);
            packageReferenceA[0].Attribute("Version").Value.Should().Be("2.0.0");
            packageReferenceA[0].Attribute("Condition").Value.Should().Be(" '$(TargetFramework)' == 'net8.0' ");
        }

        [Fact]
        public async Task SingleTfmProject_CommitsRestore()
        {
            // Arrange
            using var testContext = new SimpleTestPathContext();
            File.WriteAllText(testContext.NuGetConfig, string.Format(NugetConfigFormat, testContext.PackageSource));

            var a1 = new SimpleTestPackageContext("NuGet.Internal.Test.a", "1.0.0");
            var a2 = new SimpleTestPackageContext("NuGet.Internal.Test.a", "2.0.0");

            SimpleTestPackageContext[] packages = [a1, a2];
            await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, packages);

            var csprojContents = """
                <Project Sdk="Microsoft.NET.Sdk">
                    <PropertyGroup>
                        <TargetFramework>net8.0</TargetFramework>
                    </PropertyGroup>
                    <ItemGroup>
                        <PackageReference Include="NuGet.Internal.Test.a" Version="1.0.0" />
                    </ItemGroup>
                </Project>
                """;
            var csprojPath = Path.Combine(testContext.SolutionRoot, "my.csproj");
            File.WriteAllText(csprojPath, csprojContents);

            // Act
            var result = _testFixture.RunDotnetExpectSuccess(
                workingDirectory: testContext.SolutionRoot,
                args: $"{_xplatCli} package update NuGet.Internal.Test.a",
                testOutputHelper: _testOutputHelper,
                environmentVariables: _envVars);

            // Assert
            result.ExitCode.Should().Be(0);

            string assetsFilePath = Path.Combine(testContext.SolutionRoot, "obj", "project.assets.json");
            LockFile assetsFile = new LockFileFormat().Read(assetsFilePath);
            assetsFile.Libraries[0].Name.Should().Be("NuGet.Internal.Test.a");
            assetsFile.Libraries[0].Version.Should().Be(new NuGetVersion("2.0.0"));
        }

        [Fact]
        public void InvalidProjectFile_OutputsMeaningfulError()
        {
            // Arrange
            using var testContext = new SimpleTestPathContext();

            var csprojContents = "<Invalid Xml";
            var csprojPath = Path.Combine(testContext.SolutionRoot, "my.csproj");
            File.WriteAllText(csprojPath, csprojContents);

            // Act
            var result = _testFixture.RunDotnetExpectFailure(
                workingDirectory: testContext.SolutionRoot,
                args: $"{_xplatCli} package update NuGet.Internal.Test.a",
                testOutputHelper: _testOutputHelper,
                environmentVariables: _envVars);

            // Assert
            result.Output.Should().Contain("MSB4025").And.Contain(csprojPath);
        }
    }
}
