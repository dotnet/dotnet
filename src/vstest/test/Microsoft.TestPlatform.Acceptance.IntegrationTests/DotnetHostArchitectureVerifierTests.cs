// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
// On Linux/Mac we don't download the same .NET SDK bundles
[TestCategory("Windows-Review")]
public class DotnetHostArchitectureVerifierTests : IntegrationTestBase
{
    [TestMethod]
    [DataRow("X64")]
    // TODO: this test fails with " Determining projects to restore... .error MSB4062: The "CheckIfPackageReferenceShouldBeFrameworkReference" task could not be loaded from the assembly Microsoft.NET.Build.Tasks.dll.Could not load file or assembly. Format of the executable(.exe) or library(.dll) is invalid.Confirm that the<UsingTask> declaration is correct, that the assembly and all its dependencies are available,"
    // [DataRow("X86")]
    public void VerifyHostArchitecture(string architecture)
    {
        _testEnvironment.RunnerFramework = "net8.0";
        string dotnetPath = GetDownloadedDotnetMuxerFromTools(architecture);
        var vstestConsolePath = GetDotnetRunnerPath();
        var dotnetRunnerPath = TempDirectory.CreateDirectory("dotnetrunner");
        TempDirectory.CopyDirectory(new DirectoryInfo(Path.GetDirectoryName(vstestConsolePath)!), dotnetRunnerPath);

        // Patch the runner
        string sdkVersion = GetLatestSdkVersion(dotnetPath);
        string runtimeConfigFile = Path.Combine(dotnetRunnerPath.FullName, "vstest.console.runtimeconfig.json");
        JObject patchRuntimeConfig = JObject.Parse(File.ReadAllText(runtimeConfigFile));
        patchRuntimeConfig!["runtimeOptions"]!["framework"]!["version"] = sdkVersion;
        File.WriteAllText(runtimeConfigFile, patchRuntimeConfig.ToString());

        var environmentVariables = new Dictionary<string, string?>
        {
            ["DOTNET_MULTILEVEL_LOOKUP"] = "0",
            ["ExpectedArchitecture"] = architecture
        };

        ExecuteApplication(dotnetPath, "new mstest", out _, out _, out _, environmentVariables, TempDirectory.Path);

        // Patch test file
        File.WriteAllText(Path.Combine(TempDirectory.Path, "UnitTest1.cs"),
@"
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace cfebbc5339cf4c22854e79824e938c74;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        Assert.AreEqual(Environment.GetEnvironmentVariable(""ExpectedArchitecture""), System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString());
    }
}");

        ExecuteApplication(dotnetPath, $"test -p:VsTestConsolePath=\"{Path.Combine(dotnetRunnerPath.FullName, Path.GetFileName(vstestConsolePath))}\"", out string stdOut, out _, out int exitCode, environmentVariables, TempDirectory.Path);
        Assert.AreEqual(0, exitCode, stdOut);
    }

    private static string GetLatestSdkVersion(string dotnetPath)
        => Path.GetFileName(Directory.GetDirectories(Path.Combine(Path.GetDirectoryName(dotnetPath)!, @"shared/Microsoft.NETCore.App")).OrderByDescending(x => x).First());
}
