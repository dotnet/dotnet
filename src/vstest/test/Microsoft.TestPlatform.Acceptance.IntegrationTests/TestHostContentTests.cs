// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class TestHostContentTests : AcceptanceTestBase
{
    [TestMethod]
    [DataRow("AnyCPU", "x86", "Windows_NT")]
    [DataRow("AnyCPU", "x64", "Windows_NT")]
    [DataRow("x86", "", "Windows_NT")]
    [DataRow("AnyCPU", "", "Windows_NT")]
    [DataRow("AnyCPU", "x64", "Unix")]
    public void TestHostContentIsExcludedOnlyForAndroid(string platform, string platformTarget, string os)
    {
        var propsPath = Path.Combine(
            IntegrationTestEnvironment.RepoRootDirectory,
            "src", "package", "Microsoft.TestPlatform.TestHost", "Microsoft.TestPlatform.TestHost.props");
        var projectPath = GetIsolatedTestAsset("TestHostContent.csproj", Core11TargetFramework);
        var intermediatePath = Directory.CreateDirectory(Path.Combine(TempDirectory.Path, "obj")).FullName;

        // Import at the NuGet props hook, before the project properties and SDK framework inference.
        File.Copy(
            Path.Combine(TempDirectory.Path, "TestHostContent.csproj.nuget.g.props"),
            Path.Combine(intermediatePath, "TestHostContent.csproj.nuget.g.props"));

        var dotnetPath = Path.Combine(
            IntegrationTestEnvironment.RepoRootDirectory, ".dotnet", OSUtils.IsWindows ? "dotnet.exe" : "dotnet");
        var arguments = $"""
            msbuild "{projectPath}" -t:GetTestHostContent -nologo -nr:false -p:Configuration=Release -p:Platform={platform} -p:PlatformTarget={platformTarget} -p:OS={os} -p:MSBuildEnableWorkloadResolver=false -p:MSBuildProjectExtensionsPath="{intermediatePath}{Path.AltDirectorySeparatorChar}" -p:TestHostPropsPath="{propsPath}"
            """;

        // Check single-target evaluation as well as MSBuild's dispatch to each inner build.
        foreach (var framework in new[] { "net11.0", "net11.0-android", "net11.0-windows" })
        {
            ExecuteApplication(
                dotnetPath, $"{arguments} -p:SingleTargetFramework={framework}",
                out var output, out var error, out var exitCode, workingDirectory: TempDirectory.Path);

            Assert.AreEqual(0, exitCode, $"{output}\n{error}");
            AssertContent(framework);
        }

        ExecuteApplication(
            dotnetPath, arguments,
            out var multiTargetOutput, out var multiTargetError, out var multiTargetExitCode, workingDirectory: TempDirectory.Path);

        Assert.AreEqual(0, multiTargetExitCode, $"{multiTargetOutput}\n{multiTargetError}");
        foreach (var framework in new[] { "net11.0", "net11.0-android", "net11.0-windows" })
        {
            AssertContent(framework);
        }

        void AssertContent(string framework)
        {
            var hostName = platform == "x86" || platformTarget == "x86" ? "testhost.x86" : "testhost";
            var expected = os == "Windows_NT" && framework != "net11.0-android"
                ? new[] { $"{hostName}.exe|PreserveNewest|False", $"{hostName}.dll|PreserveNewest|False" }
                : Array.Empty<string>();
            var contentPath = Path.Combine(TempDirectory.Path, $"{framework}.content");

            CollectionAssert.AreEqual(expected, File.ReadAllLines(contentPath), framework);
            File.Delete(contentPath);
        }
    }
}
