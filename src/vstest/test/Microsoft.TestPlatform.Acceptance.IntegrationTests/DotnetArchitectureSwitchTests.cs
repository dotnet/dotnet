// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if !NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;
// This tests need specific sdks to be installed on arm machine
// >= ARM 6.0.2xx
// >= x64 6.0.2xx
// x64 5.0.4xx for Mac
// x64 3.1.4XX for Win
// Manual test './tools/.../dotnet test ./test/Microsoft.TestPlatform.AcceptanceTests/bin/Debug/net8.0/Microsoft.TestPlatform.AcceptanceTests.dll --testcasefilter:"DotnetArchitectureSwitchTests"'
[TestClass]
[Ignore("Manual tests(for now). Tests in this class need some .NET SDK global installations")]
public class DotnetArchitectureSwitchTests : AcceptanceTestBase
{
    private static string s_privateX64Installation = string.Empty;

    [ClassInitialize]
    public static void ClassInitialize(TestContext _)
    {
        s_privateX64Installation = Path.Combine(new TempDirectory().Path, "x64");
        CopyAll(new DirectoryInfo(GetX64InstallationFolder), new DirectoryInfo(s_privateX64Installation));
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        // Remove one level up because we are targeting a sub-folder of the temp directory.
        TempDirectory.TryRemoveDirectory(new DirectoryInfo(s_privateX64Installation).Parent!.FullName);
    }

    [TestMethod]
    public void GlobalInstallation()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return;
        }

        var projectName = "ArchitectureSwitch.csproj";
        var projectPath = GetProjectFullPath(projectName);
        var projectDirectory = Path.GetDirectoryName(projectPath);

        var env = new Dictionary<string, string?>
            {
                { "DOTNET_ROOT", null },
                { "DOTNET_MULTILEVEL_LOOKUP", "0" }
            };

        // Verify native architecture
        ExecuteApplication(GetDefaultDotnetMuxerLocation, $"test {projectPath} --framework net8.0", out string stdOut, out _, out _, env, projectDirectory);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Assert.IsTrue(stdOut.Contains("Runtime location: /usr/local/share/dotnet/shared/Microsoft.NETCore.App"));
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Assert.IsTrue(stdOut.Contains($@"Runtime location: {Environment.ExpandEnvironmentVariables("%ProgramFiles%")}\dotnet\shared\Microsoft.NETCore.App"));
        }
        Assert.IsTrue(stdOut.Contains("OSArchitecture: ARM64"));
        Assert.IsTrue(stdOut.Contains("ProcessArchitecture: ARM64"));


        // Verify switch using csproj
        ExecuteApplication(GetDefaultDotnetMuxerLocation, $"test {projectPath} --framework net8.0 --arch x64", out stdOut, out _, out _, env, projectDirectory);
        AssertSwitch(stdOut);

        // Verify switch using test container
        var buildAssemblyPath = GetTestDllForFramework("ArchitectureSwitch.dll", "net8.0");
        ExecuteApplication(GetDefaultDotnetMuxerLocation, $"test {buildAssemblyPath} --arch x64", out stdOut, out _, out _, env, projectDirectory);
        AssertSwitch(stdOut);

        static void AssertSwitch(string output)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Assert.IsTrue(output.Contains("Runtime location: /usr/local/share/dotnet/x64/shared/Microsoft.NETCore.App"));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.IsTrue(output.Contains($@"Runtime location: {Environment.ExpandEnvironmentVariables("%ProgramFiles%")}\dotnet\x64\shared\Microsoft.NETCore.App"));
            }
            Assert.IsTrue(output.Contains("OSArchitecture: X64"));
            Assert.IsTrue(output.Contains("ProcessArchitecture: X64"));
        }
    }

    [TestMethod]
    [DataRow(true, false)]
    [DataRow(false, true)]
    public void DOTNET_ROOTS_EnvironmentVariables(bool dotnetRoot, bool dotnetRootX64)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return;
        }

        var env = new Dictionary<string, string?>
        {
            ["DOTNET_ROOT"] = null,
            ["DOTNET_MULTILEVEL_LOOKUP"] = "0"
        };

        var projectName = "ArchitectureSwitch.csproj";
        var projectPath = GetProjectFullPath(projectName);
        var projectDirectory = Path.GetDirectoryName(projectPath);

        // Verify native architecture
        ExecuteApplication(GetDefaultDotnetMuxerLocation, $"test {projectPath} --framework net8.0", out string stdOut, out _, out _, env, projectDirectory);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Assert.IsTrue(stdOut.Contains("Runtime location: /usr/local/share/dotnet/shared/Microsoft.NETCore.App"), "Unexpected runtime location");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Assert.IsTrue(stdOut.Contains($@"Runtime location: {Environment.ExpandEnvironmentVariables("%ProgramFiles%")}\dotnet\shared\Microsoft.NETCore.App"));
        }
        Assert.IsTrue(stdOut.Contains("OSArchitecture: ARM64"), "Unexpected OSArchitecture");
        Assert.IsTrue(stdOut.Contains("ProcessArchitecture: ARM64"), "Unexpected ProcessArchitecture");

        env.Clear();
        env["DOTNET_ROOT"] = null;
        env["DOTNET_MULTILEVEL_LOOKUP"] = "0";
        if (dotnetRoot)
        {
            env["DOTNET_ROOT"] = s_privateX64Installation;
        }

        if (dotnetRootX64)
        {
            env.Add("DOTNET_ROOT_X64", s_privateX64Installation);
        }

        // Verify switch using csproj
        ExecuteApplication($"{s_privateX64Installation}/{GetMuxerName}", $"test {projectPath} --framework net8.0 --arch x64", out stdOut, out _, out _, env, projectDirectory);
        AssertSwitch(stdOut);

        // Verify switch using test container
        var buildAssemblyPath = GetTestDllForFramework("ArchitectureSwitch.dll", "net8.0");
        ExecuteApplication($"{s_privateX64Installation}/{GetMuxerName}", $"test {buildAssemblyPath} --framework net8.0 --arch x64", out stdOut, out _, out _, env, projectDirectory);
        AssertSwitch(stdOut);

        void AssertSwitch(string output)
        {
            Assert.IsTrue(Regex.IsMatch(output.Replace(@"\", "/"), $"Runtime location: .*{s_privateX64Installation.Replace(@"\", "/")}.*shared.*Microsoft.NETCore.App"), "Unexpected runtime location");
            Assert.IsTrue(output.Contains("OSArchitecture: X64"), "Unexpected OSArchitecture");
            Assert.IsTrue(output.Contains("ProcessArchitecture: X64"), "Unexpected ProcessArchitecture");
            Assert.IsTrue(!dotnetRoot || output.Contains($"DOTNET_ROOT: {s_privateX64Installation}"), "Unexpected DOTNET_ROOT var");
            Assert.IsTrue(!dotnetRootX64 || output.Contains($"DOTNET_ROOT_X64: {s_privateX64Installation}"), "Unexpected DOTNET_ROOT_X64 var");
        }
    }

    [TestMethod]
    public void PrivateX64BuildToGlobalArmInstallation()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return;
        }

        var env = new Dictionary<string, string?>
        {
            ["DOTNET_ROOT"] = null,
            ["DOTNET_MULTILEVEL_LOOKUP"] = "0"
        };
        string privateInstallationMuxer = Path.Combine(s_privateX64Installation, GetMuxerName);

        var projectName = "ArchitectureSwitch.csproj";
        var projectPath = GetProjectFullPath(projectName);
        var projectDirectory = Path.GetDirectoryName(projectPath);

        // Verify native architecture
        ExecuteApplication(privateInstallationMuxer, $"test {projectPath} --framework net8.0", out string stdOut, out _, out _, env, projectDirectory);
        Assert.IsTrue(Regex.IsMatch(stdOut.Replace(@"\", "/"), $"Runtime location: .*{s_privateX64Installation.Replace(@"\", "/")}.*shared.*Microsoft.NETCore.App"), "Unexpected runtime location");
        Assert.IsTrue(stdOut.Contains("OSArchitecture: X64"), "Unexpected OSArchitecture");
        Assert.IsTrue(stdOut.Contains("ProcessArchitecture: X64"), "Unexpected ProcessArchitecture");

        // Verify switch using csproj
        ExecuteApplication($"{s_privateX64Installation}/{GetMuxerName}", $"test {projectPath} --framework net8.0 --arch arm64", out stdOut, out _, out _, env, projectDirectory);
        AssertSwitch(stdOut);

        // Verify switch using test container
        var buildAssemblyPath = GetTestDllForFramework("ArchitectureSwitch.dll", "net8.0");
        ExecuteApplication($"{s_privateX64Installation}/{GetMuxerName}", $"test {buildAssemblyPath} --framework net8.0 --arch arm64", out stdOut, out _, out _, env, projectDirectory);
        AssertSwitch(stdOut);

        static void AssertSwitch(string output)
        {
            Assert.IsTrue(Regex.IsMatch(output.Replace(@"\", "/"), $"Runtime location: .*{GetDefaultLocation.Replace(@"\", "/")}.*shared.*Microsoft.NETCore.App"), "Unexpected runtime location");
            Assert.IsTrue(output.Contains("OSArchitecture: ARM64"), "Unexpected OSArchitecture");
            Assert.IsTrue(output.Contains("ProcessArchitecture: ARM64"), "Unexpected ProcessArchitecture");
        }
    }

    [TestMethod]
    [DataRow(true, false)]
    [DataRow(false, true)]
    public void PrivateX64BuildToDOTNET_ROOTS_EnvironmentVariables(bool dotnetRoot, bool dotnetRootArm64)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return;
        }

        var env = new Dictionary<string, string?>
        {
            ["DOTNET_ROOT"] = null,
            ["DOTNET_MULTILEVEL_LOOKUP"] = "0"
        };
        string privateInstallationMuxer = Path.Combine(s_privateX64Installation, GetMuxerName);

        var projectName = "ArchitectureSwitch.csproj";
        var projectPath = GetProjectFullPath(projectName);
        var projectDirectory = Path.GetDirectoryName(projectPath);

        // Verify native architecture
        ExecuteApplication(privateInstallationMuxer, $"test {projectPath} --framework net8.0", out string stdOut, out _, out _, env, projectDirectory);
        Assert.IsTrue(Regex.IsMatch(stdOut.Replace(@"\", "/"), $"Runtime location: .*{s_privateX64Installation.Replace(@"\", "/")}.*shared.*Microsoft.NETCore.App"), "Unexpected runtime location");
        Assert.IsTrue(stdOut.Contains("OSArchitecture: X64"), "Unexpected OSArchitecture");
        Assert.IsTrue(stdOut.Contains("ProcessArchitecture: X64"), "Unexpected ProcessArchitecture");

        env.Clear();
        env["DOTNET_ROOT"] = null;
        env["DOTNET_MULTILEVEL_LOOKUP"] = "0";
        if (dotnetRoot)
        {
            env["DOTNET_ROOT"] = GetDefaultLocation;
        }

        if (dotnetRootArm64)
        {
            env["DOTNET_ROOT_ARM64"] = GetDefaultLocation;
        }

        // Verify switch using csproj
        ExecuteApplication($"{s_privateX64Installation}/{GetMuxerName}", $"test {projectPath} --framework net8.0 --arch arm64", out stdOut, out _, out _, env, projectDirectory);
        AssertSwitch(stdOut);

        // Verify switch using test container
        var buildAssemblyPath = GetTestDllForFramework("ArchitectureSwitch.dll", "net8.0");
        ExecuteApplication($"{s_privateX64Installation}/{GetMuxerName}", $"test {buildAssemblyPath} --framework net8.0 --arch arm64", out stdOut, out _, out _, env, projectDirectory);
        AssertSwitch(stdOut);

        void AssertSwitch(string output)
        {
            Assert.IsTrue(Regex.IsMatch(output.Replace(@"\", "/"), $"Runtime location: .*{GetDefaultLocation.Replace(@"\", "/")}.*shared.*Microsoft.NETCore.App"), "Unexpected runtime location");
            Assert.IsTrue(output.Contains("OSArchitecture: ARM64"), "Unexpected OSArchitecture");
            Assert.IsTrue(output.Contains("ProcessArchitecture: ARM64"), "Unexpected ProcessArchitecture");
            Assert.IsTrue(!dotnetRoot || output.Contains($"DOTNET_ROOT: {GetDefaultLocation}"), "Unexpected DOTNET_ROOT var");
            Assert.IsTrue(!dotnetRootArm64 || output.Contains($"DOTNET_ROOT_ARM64: {GetDefaultLocation}"), "Unexpected DOTNET_ROOT_ARM64 var");
        }
    }

    [TestMethod]
    public void SilentlyForceX64()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return;
        }

        var projectName = "ArchitectureSwitch.csproj";
        var projectPath = GetProjectFullPath(projectName);
        var projectDirectory = Path.GetDirectoryName(projectPath);

        var env = new Dictionary<string, string?>
        {
            ["DOTNET_ROOT"] = null,
            ["DOTNET_MULTILEVEL_LOOKUP"] = "0"
        };
        ExecuteApplication(GetDefaultDotnetMuxerLocation, $"test {projectPath} --framework {GetFrameworkVersionToForceToX64}", out string stdOut, out _, out _, env, projectDirectory);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Assert.IsTrue(stdOut.Contains("Runtime location: /usr/local/share/dotnet/x64/shared/Microsoft.NETCore.App"));
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Assert.IsTrue(stdOut.Contains($@"Runtime location: {Environment.ExpandEnvironmentVariables("%ProgramFiles%")}\dotnet\x64\shared\Microsoft.NETCore.App"));
        }
        Assert.IsTrue(stdOut.Contains("OSArchitecture: X64"));
        Assert.IsTrue(stdOut.Contains("ProcessArchitecture: X64"));
    }

    private static string GetMuxerName => RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "dotnet" : "dotnet.exe";

    private static string GetX64InstallationFolder => RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ?
        "/usr/local/share/dotnet/x64" :
        $@"{Environment.ExpandEnvironmentVariables("%ProgramFiles%")}\dotnet\x64";

    private static string GetFrameworkVersionToForceToX64 => RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ?
        "net9.0" :
        "net8.0";

    private static string GetDefaultLocation => RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ?
        $"/usr/local/share/dotnet" :
        $@"{Environment.ExpandEnvironmentVariables("%ProgramFiles%")}\dotnet";

    private static string GetDefaultDotnetMuxerLocation => RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ?
        $"{GetDefaultLocation}/{GetMuxerName}" :
        $@"{GetDefaultLocation}\{GetMuxerName}";

    private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
        Directory.CreateDirectory(target.FullName);

        // Copy each file into the new directory.
        foreach (FileInfo fi in source.GetFiles())
        {
            fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
        }

        // Copy each subdirectory using recursion.
        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir =
                target.CreateSubdirectory(diSourceSubDir.Name);
            CopyAll(diSourceSubDir, nextTargetSubDir);
        }
    }
}

#endif
