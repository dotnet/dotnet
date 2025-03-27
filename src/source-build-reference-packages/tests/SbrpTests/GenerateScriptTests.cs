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
    public enum PackageType
    {
        Reference,
        Text
    }

    public static IEnumerable<object[]> Data => new List<object[]>
    {
        new object[] { "Microsoft.Build.NoTargets", "3.7.0", PackageType.Text },
        new object[] { "Microsoft.Extensions.Logging.Abstractions", "6.0.4", PackageType.Reference },
        new object[] { "NuGet.Packaging", "6.12.1", PackageType.Reference },
        new object[] { "System.Buffers", "4.6.0", PackageType.Reference },
        new object[] { "System.Security.Cryptography.ProtectedData", "8.0.0", PackageType.Reference },
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
        string arguments = $"-p {package},{version} -x -d {SandboxDirectory}";
        string pkgSrcDirectory;
        string pkgSandboxDirectory = Path.Combine(SandboxDirectory, package.ToLower(), version);

        switch (type)
        {
            case PackageType.Reference:
                pkgSrcDirectory = Path.Combine(PathUtilities.GetRepoRoot(), "src", "referencePackages", "src", package.ToLower(), version);
                break;
            case PackageType.Text:
                arguments += " -t text";
                pkgSrcDirectory = Path.Combine(PathUtilities.GetRepoRoot(), "src", "textOnlyPackages", "src", package.ToLower(), version);
                break;
            default:
                throw new ArgumentException($"Unknown package type '{type}'");
        }

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
