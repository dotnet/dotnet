// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
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
        new object[] { "System.Xml.ReaderWriter", "4.0.11", PackageType.Reference },
        new object[] { "Microsoft.Extensions.Logging.Abstractions", "7.0.1", PackageType.Reference },
        new object[] { "Microsoft.CodeAnalysis.CSharp", "3.11.0", PackageType.Reference },
        new object[] { "System.Security.Cryptography.Pkcs", "7.0.2", PackageType.Reference },
        new object[] { "Microsoft.Build.NoTargets", "3.7.0", PackageType.Text },
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
        string pkgSrcDirectory = string.Empty;
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
        }

        ExecuteHelper.ExecuteProcessValidateExitCode(command, arguments, Output);

        string diff = ExecuteHelper.ExecuteProcess("git", $"diff --no-index {pkgSrcDirectory} {pkgSandboxDirectory}", Output, true).StdOut;
        if (diff != string.Empty)
        {
            Assert.Fail($"Regenerated package '{package}' does not match the checked-in content.  {Environment.NewLine}"
                    + $"{diff}{Environment.NewLine}");
        }
    }
}
