// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if !NETFRAMEWORK

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
[TestCategory("Windows-Review")]
public class DotnetArchitectureSwitchTestsWindowsOnly : AcceptanceTestBase
{
    [TestMethod]
    [DataRow("X64", "X86")]
    // TODO: This test does not work on server, it occasionally fails with cryptic message around not being able to load MSBuild.Tasks.
    // [DataRow("X86", "X64")]
    public void Use_EnvironmentVariables(string architectureFrom, string architectureTo)
    {
        SetTestEnvironment(_testEnvironment, new RunnerInfo { RunnerFramework = DEFAULT_RUNNER_NETCORE });
        string dotnetPath = GetDownloadedDotnetMuxerFromTools(architectureFrom);
        string dotnetPathTo = GetDownloadedDotnetMuxerFromTools(architectureTo);
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
            [$"DOTNET_ROOT_{architectureTo}"] = Path.GetDirectoryName(dotnetPathTo)!,
            ["ExpectedArchitecture"] = architectureTo
        };

        if (architectureTo == "X86")
        {
            environmentVariables.Add("DOTNET_ROOT(x86)", Path.GetDirectoryName(dotnetPathTo)!);
        }
        ExecuteApplication(dotnetPath, "new mstest", out _, out string _, out _, environmentVariables, TempDirectory.Path);

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

        ExecuteApplication(dotnetPath, $"test -p:VsTestConsolePath=\"{Path.Combine(dotnetRunnerPath.FullName, Path.GetFileName(vstestConsolePath))}\" --arch {architectureTo.ToLower(CultureInfo.InvariantCulture)} --diag:log.txt", out string stdOut, out _, out int exitCode, environmentVariables, TempDirectory.Path);
        Assert.AreEqual(0, exitCode, stdOut);

        environmentVariables = new Dictionary<string, string?>
        {
            ["DOTNET_MULTILEVEL_LOOKUP"] = "0",
            ["DOTNET_ROOT"] = Path.GetDirectoryName(dotnetPathTo),
            ["ExpectedArchitecture"] = architectureTo
        };

        if (architectureTo == "X86")
        {
            environmentVariables.Add("DOTNET_ROOT(x86)", Path.GetDirectoryName(dotnetPathTo)!);
        }
        ExecuteApplication(dotnetPath, $"test -p:VsTestConsolePath=\"{Path.Combine(dotnetRunnerPath.FullName, Path.GetFileName(vstestConsolePath))}\" --arch {architectureTo.ToLower(CultureInfo.InvariantCulture)} --diag:log.txt", out stdOut, out _, out exitCode, environmentVariables, TempDirectory.Path);
        Assert.AreEqual(0, exitCode, stdOut);

        environmentVariables = new Dictionary<string, string?>
        {
            ["DOTNET_MULTILEVEL_LOOKUP"] = "0",
            [$"DOTNET_ROOT_{architectureTo}"] = Path.GetDirectoryName(dotnetPathTo),
            ["DOTNET_ROOT"] = "WE SHOULD PICK THE ABOVE ONE BEFORE FALLBACK TO DOTNET_ROOT",
            ["ExpectedArchitecture"] = architectureTo
        };

        if (architectureTo == "X86")
        {
            environmentVariables.Add("DOTNET_ROOT(x86)", Path.GetDirectoryName(dotnetPathTo)!);
        }
        ExecuteApplication(dotnetPath, $"test -p:VsTestConsolePath=\"{Path.Combine(dotnetRunnerPath.FullName, Path.GetFileName(vstestConsolePath))}\" --arch {architectureTo.ToLower(CultureInfo.InvariantCulture)} --diag:log.txt", out stdOut, out _, out exitCode, environmentVariables, TempDirectory.Path);
        Assert.AreEqual(0, exitCode, stdOut);
    }

    private static string GetLatestSdkVersion(string dotnetPath)
        => Path.GetFileName(Directory.GetDirectories(Path.Combine(Path.GetDirectoryName(dotnetPath)!, @"shared/Microsoft.NETCore.App")).OrderByDescending(x => x).First());
}

#endif
