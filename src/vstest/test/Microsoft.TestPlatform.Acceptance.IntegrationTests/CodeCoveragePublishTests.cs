// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public sealed class CodeCoveragePublishTests : AcceptanceTestBase
{
    [TestMethod]
    [DataRow(-1)]
    [DataRow(1)]
    public void PublishKeepsPrivateDependenciesIsolatedRegardlessOfTimestamp(int coverageTimestampOffset)
    {
        var packageDirectory = TempDirectory.CreateDirectory("coverage-package").FullName;
        var applicationDirectory = TempDirectory.CreateDirectory("application").FullName;
        using var deployment = new TempDirectory();
        string[] dependencies =
        [
            "System.Memory.dll", "System.Buffers.dll", "System.Runtime.CompilerServices.Unsafe.dll",
            "System.Threading.Tasks.Extensions.dll", "System.Text.Json.dll", "System.Text.Encodings.Web.dll",
            "Microsoft.Bcl.AsyncInterfaces.dll", "Microsoft.CodeCoverage.targets"
        ];
        var timestamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        foreach (var dependency in dependencies)
        {
            var applicationFile = Path.Combine(applicationDirectory, dependency);
            File.WriteAllText(applicationFile, "application");
            File.SetLastWriteTimeUtc(applicationFile, timestamp);
            var coverageFile = Path.Combine(packageDirectory, dependency);
            File.WriteAllText(coverageFile, "coverage");
            File.SetLastWriteTimeUtc(coverageFile, timestamp.AddDays(coverageTimestampOffset));
        }

        File.Copy(
            Path.Combine(IntegrationTestEnvironment.RepoRootDirectory, "src", "package", "Microsoft.CodeCoverage", "Microsoft.CodeCoverage.targets"),
            Path.Combine(packageDirectory, "Microsoft.CodeCoverage.targets"),
            overwrite: true);
        Directory.CreateDirectory(Path.Combine(packageDirectory, "x64"));
        File.WriteAllText(Path.Combine(packageDirectory, "x64", "instrumentation.dll"), "native");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "fr"));
        File.WriteAllText(Path.Combine(packageDirectory, "fr", "collector.resources.dll"), "resource");

        var projectPath = Path.Combine(TempDirectory.Path, "Publish.csproj");
        File.WriteAllText(Path.Combine(TempDirectory.Path, "Directory.Build.props"), "<Project />");
        File.WriteAllText(Path.Combine(TempDirectory.Path, "Directory.Build.targets"), "<Project />");
        File.WriteAllText(projectPath, $"""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>{Core11TargetFramework}</TargetFramework>
                <EnableDefaultItems>false</EnableDefaultItems>
                <DisableMsCoverageReferencedPathMaps>true</DisableMsCoverageReferencedPathMaps>
              </PropertyGroup>
              <ItemGroup>
                <Content Include="application/*" CopyToPublishDirectory="PreserveNewest" TargetPath="%(Filename)%(Extension)" />
              </ItemGroup>
              <Import Project="coverage-package/Microsoft.CodeCoverage.targets" />
              <Target Name="_GetDefaultWasmAssembliesToBundle">
                <ItemGroup>
                  <WasmAssembliesToBundle Include="$(PublishDir)/**/*.dll" />
                </ItemGroup>
              </Target>
              <Target Name="CheckWasmBundle" DependsOnTargets="_GetDefaultWasmAssembliesToBundle">
                <WriteLinesToFile File="$(PublishDir)/wasm-inputs.txt" Lines="@(WasmAssembliesToBundle)" Overwrite="true" />
              </Target>
            </Project>
            """);

        for (var publish = 0; publish < 2; publish++)
        {
            RunDotnet($"""publish "{projectPath}" -c Release -o "{deployment.Path}" -bl:"{Path.Combine(TempDirectory.Path, $"publish-{publish}.binlog")}" """);
            foreach (var dependency in dependencies)
            {
                Assert.AreEqual("application", File.ReadAllText(Path.Combine(deployment.Path, dependency)), dependency);
                Assert.AreEqual(
                    File.ReadAllText(Path.Combine(packageDirectory, dependency)),
                    File.ReadAllText(Path.Combine(deployment.Path, "Microsoft.CodeCoverage", dependency)),
                    dependency);
            }

            Assert.AreEqual("native", File.ReadAllText(Path.Combine(deployment.Path, "Microsoft.CodeCoverage", "x64", "instrumentation.dll")));
            Assert.AreEqual("resource", File.ReadAllText(Path.Combine(deployment.Path, "Microsoft.CodeCoverage", "fr", "collector.resources.dll")));
            var publishDirectory = deployment.Path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            // A trailing backslash would escape the closing quote in Windows process arguments.
            foreach (var directory in new[] { publishDirectory, publishDirectory + Path.AltDirectorySeparatorChar })
            {
                RunDotnet($"""msbuild "{projectPath}" -t:CheckWasmBundle -p:PublishDir="{directory}" """);
                var bundledFiles = File.ReadAllLines(Path.Combine(deployment.Path, "wasm-inputs.txt"));
                Assert.Contains(path => path.EndsWith("System.Memory.dll", StringComparison.Ordinal), bundledFiles);
                Assert.DoesNotContain(path => path.Contains($"Microsoft.CodeCoverage{Path.DirectorySeparatorChar}", StringComparison.Ordinal), bundledFiles);
            }
        }
    }

    [TestMethod]
    [TestMatrix(console: Net)]
    public void PublishedTestsDiscoverCoverageWithoutPackageAdapterPath(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var projectPath = GetIsolatedTestAsset("SimpleTestProject.csproj", runnerInfo.TargetFramework);
        using var deployment = new TempDirectory();
        RunDotnet($"""publish "{projectPath}" -c Release -o "{deployment.Path}" -p:PackageVersion={IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion} -bl:"{Path.Combine(TempDirectory.Path, "publish.binlog")}" """);
        var collectorPath = Path.Combine(deployment.Path, "Microsoft.CodeCoverage", "Microsoft.VisualStudio.TraceDataCollector.dll");
        Assert.IsTrue(File.Exists(collectorPath), collectorPath);
        Assert.IsFalse(File.Exists(Path.Combine(deployment.Path, "Microsoft.VisualStudio.TraceDataCollector.dll")));

        // Use the SDK's existing runner without TestAdapterPath. DataCollectionRequestHandler.AddExtensionAssemblies
        // searches the test source directories recursively, independently of the test host's adapter discovery.
        Directory.CreateDirectory(DiagLogsDirectory);
        var output = RunDotnet(
            $@"vstest ""{Path.Combine(deployment.Path, "SimpleTestProject.dll")}"" /collect:""Code Coverage;Format=cobertura"" /ResultsDirectory:""{deployment.Path}"" /logger:""console;verbosity=normal"" /Diag:""{Path.Combine(DiagLogsDirectory, "coverage.log")}""",
            expectedExitCode: 1,
            packagesDirectory: Path.Combine(deployment.Path, "empty-cache"));
        Assert.Contains("Passed: 1", output);
        Assert.Contains("Failed: 1", output);
        Assert.Contains("Skipped: 1", output);
        var report = Directory.GetFiles(deployment.Path, "*.cobertura.xml", SearchOption.AllDirectories).Single();
        var coverage = new XmlDocument();
        coverage.Load(report);
        Assert.IsGreaterThan(0, int.Parse(coverage.DocumentElement!.GetAttribute("lines-covered"), System.Globalization.CultureInfo.InvariantCulture), report);

        var diagnostics = string.Join(Environment.NewLine, Directory.GetFiles(DiagLogsDirectory, "*.log").Select(File.ReadAllText));
        Assert.Contains(collectorPath, diagnostics);
        Assert.Contains(Path.Combine(deployment.Path, "Microsoft.CodeCoverage", "Microsoft.CodeCoverage.Core.dll"), diagnostics);
    }

    private static string RunDotnet(string arguments, int expectedExitCode = 0, string? packagesDirectory = null)
    {
        var dotnet = Path.Combine(IntegrationTestEnvironment.RepoRootDirectory, ".dotnet", OSUtils.IsWindows ? "dotnet.exe" : "dotnet");
        ExecuteApplication(dotnet, arguments, out var output, out var error, out var exitCode,
            new Dictionary<string, string?> { ["NUGET_PACKAGES"] = packagesDirectory ?? IntegrationTestEnvironment.TestAssetsNuGetCacheDirectory });
        Assert.AreEqual(expectedExitCode, exitCode, output + Environment.NewLine + error);

        return output;
    }
}
