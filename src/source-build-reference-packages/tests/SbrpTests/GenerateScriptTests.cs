// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace SbrpTests;

public class GenerateScriptTests
{
    public static IEnumerable<object[]> Data => new List<object[]>
    {
        new object[] { "Microsoft.Build.NoTargets", "3.7.0", PackageType.Text }, // Text only package
        new object[] { "Microsoft.CodeAnalysis.PooledObjects", "5.0.0-1.25277.114", PackageType.Text }, // Text only package w/reference package dependencies
        new object[] { "System.Memory", "4.6.3", PackageType.Reference }, // Simple reference package w/o customizations
        new object[] { "System.Threading.Channels", "8.0.0", PackageType.Reference }, // Reference package w/numerous TFMs
        new object[] { "NuGet.Packaging", "6.13.2", PackageType.Reference }, // Package w/Customizations.props
        new object[] { "System.Collections.Immutable", "8.0.0", PackageType.Reference }, // Package w/Customizations.cs
    };

    public string SandboxDirectory { get; set; }
    public ITestOutputHelper Output { get; set; }

    public GenerateScriptTests(ITestOutputHelper output)
    {
        Output = output;
        SandboxDirectory = Path.Combine(Environment.CurrentDirectory, $"GenerateTests-{DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()}");
        Directory.CreateDirectory(SandboxDirectory);
    }

    [Theory]
    [MemberData(nameof(GenerateScriptTests.Data), MemberType = typeof(GenerateScriptTests))]
    public void VerifyGenerateScript(string package, string version, PackageType type)
    {
        string command = Path.Combine(PathUtilities.GetRepoRoot(), RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "generate.cmd" : "generate.sh");
        string arguments = $"-p {package},{version} -x -d {SandboxDirectory}"
            + (type == PackageType.Text ? " -t text" : string.Empty);
        string pkgDirectory = Path.Combine(PathUtilities.GetPackageTypeDir(type), "src", package.ToLower(), version);
        string pkgSrcDirectory = Path.Combine(PathUtilities.GetRepoRoot(), "src", pkgDirectory);
        string pkgSandboxDirectory = Path.Combine(SandboxDirectory, pkgDirectory);

        Assert.True(Directory.Exists(pkgSrcDirectory), $"Source directory '{pkgSrcDirectory}' does not exist.");

        ExecuteHelper.ExecuteProcessValidateExitCode(command, arguments, Output);

        // Copy any customization files from the source directory to the sandbox directory.
        // This is necessary because git diff doesn't support exclusions when comparing files outside of the repository.
        string[] customFiles = { "Customizations.cs", "Customizations.props" };
        foreach (string customFile in customFiles)
        {
            string srcFile = Path.Combine(pkgSrcDirectory, customFile);
            if (File.Exists(srcFile))
            {
                string destFile = Path.Combine(pkgSandboxDirectory, customFile);
                File.Copy(srcFile, destFile);
            }
        }

        (Process Process, string StdOut, string StdErr) result =
            ExecuteHelper.ExecuteProcess("git", $"diff --no-index {pkgSrcDirectory} {pkgSandboxDirectory}", Output, true);

        string diff = result.StdOut;
        if (diff != string.Empty)
        {
            Assert.Fail($"Regenerated package '{package}, {version}' does not match the checked-in content.  {Environment.NewLine}"
                + $"{diff}{Environment.NewLine}");
        }
        else if (result.Process.ExitCode != 0)
        {
            Assert.Fail($"Unexpected git diff failure on '{package}, {version}'.  {Environment.NewLine}{result.StdErr}{Environment.NewLine}");
        }
    }
}
