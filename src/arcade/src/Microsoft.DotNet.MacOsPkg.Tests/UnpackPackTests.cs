// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AwesomeAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.MacOsPkg.Tests;

public class UnpackPackTests : IDisposable
{
    private readonly ITestOutputHelper output;
    private readonly string testRoot;
    private static readonly string simplePkg = GetResourceFilePath("Simple.pkg");
    private static readonly string withAppPkg = GetResourceFilePath("WithApp.pkg");
    private static readonly string simpleInstallerPkg = GetResourceFilePath("SimpleInstaller.pkg");
    private static readonly string withAppInstallerPkg = GetResourceFilePath("WithAppInstaller.pkg");

    private static readonly string pkgToolPath = Path.Combine(
        Path.GetDirectoryName(typeof(UnpackPackTests).Assembly.Location)!,
        "tools",
        "macospkg",
        "Microsoft.DotNet.MacOsPkg.Cli.dll");

    const UnixFileMode nonExecutableFileMode = UnixFileMode.OtherRead | 
                                               UnixFileMode.GroupRead |
                                               UnixFileMode.UserWrite |
                                               UnixFileMode.UserRead;
    const UnixFileMode executableFileMode = UnixFileMode.OtherExecute |
                                            UnixFileMode.OtherRead |
                                            UnixFileMode.GroupExecute |
                                            UnixFileMode.GroupRead |
                                            UnixFileMode.UserExecute |
                                            UnixFileMode.UserWrite |
                                            UnixFileMode.UserRead;
    private static readonly (string file, UnixFileMode mode)[] simplePkgFiles =
    [
        ("Bom", nonExecutableFileMode ),
        ("PackageInfo", nonExecutableFileMode),
        (Path.Combine("Payload", "Sample.txt"), nonExecutableFileMode),
    ];

    private static readonly (string file, UnixFileMode mode)[] withAppPkgFiles =
    [
        ("Bom", nonExecutableFileMode),
        ("PackageInfo", nonExecutableFileMode),
        (Path.Combine("Payload", "test.app"), nonExecutableFileMode),
    ];

    private static readonly (string file, UnixFileMode mode)[] appFiles =
    [
        (Path.Combine("Contents", "Info.plist"), nonExecutableFileMode),
        (Path.Combine("Contents", "MacOS", "main"), executableFileMode),
        (Path.Combine("Contents", "Resources", "libexample.dylib"), executableFileMode)
    ];

    private static readonly (string file, UnixFileMode mode)[] simpleInstallerFiles =
    [
        ("Distribution", nonExecutableFileMode),
        ("Simple.pkg", nonExecutableFileMode),
    ];

    private static readonly (string file, UnixFileMode mode)[] withAppInstallerFiles =
    [
        ("Distribution", nonExecutableFileMode),
        ("WithApp.pkg", nonExecutableFileMode),
    ];

    public UnpackPackTests(ITestOutputHelper output)
    {
        this.output = output;
        // These tests repeatedly archive executable app bundles, so keep their workspace with the build artifacts.
        testRoot = Path.Combine(AppContext.BaseDirectory, "test-artifacts", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(testRoot);
    }

    public void Dispose()
    {
        try
        {
            if (Directory.Exists(testRoot))
            {
                Directory.Delete(testRoot, true);
            }
        }
        catch (Exception ex)
        {
            output.WriteLine($"Failed to delete test workspace '{testRoot}': {ex}");
        }
    }

    [MacOSOnlyFact]
    public void UnpackPackSimplePkg()
    {
        string unpackPath = GetTestPath();
        string packPath = GetTempPkgPath();

        ExecuteWithCleanup(() =>
        {
            Unpack(simplePkg, unpackPath, simplePkgFiles);
            Pack(unpackPath, packPath, simplePkgFiles);
        }, [ unpackPath, packPath ]);
    }

    [MacOSOnlyFact]
    public void UnpackPackWithAppPkg()
    {
        string unpackPath = GetTestPath();
        string packPath = GetTempPkgPath();

        ExecuteWithCleanup(() =>
        {
            Unpack(withAppPkg, unpackPath, withAppPkgFiles);
            Pack(unpackPath, packPath, withAppPkgFiles);
        }, [ unpackPath, packPath ]);
    }

    [MacOSOnlyFact]
    public void UnpackPackAppBundle()
    {
        string unpackPkgPath = GetTestPath();
        string unpackAppPath = GetTestPath();
        string packAppPath = GetTempAppPath();

        ExecuteWithCleanup(() =>
        {
            Unpack(withAppPkg, unpackPkgPath, withAppPkgFiles);
            Unpack(Path.Combine(unpackPkgPath, "Payload", "test.app"), unpackAppPath, appFiles);
            Pack(unpackAppPath, packAppPath, appFiles);
        }, [ unpackPkgPath, unpackAppPath ]);
    }

    [MacOSOnlyFact]
    public void UnpackPackSimpleInstallerPkg()
    {
        string unpackPath = GetTestPath();
        string packPath = GetTempPkgPath();

        ExecuteWithCleanup(() =>
        {
            Unpack(simpleInstallerPkg, unpackPath, simpleInstallerFiles);
            Pack(unpackPath, packPath, simpleInstallerFiles);
        }, [ unpackPath, packPath ]);
    }

    [MacOSOnlyFact]
    public void UnpackPackSimplePkgInSimpleInstallerPkg()
    {
        string unpackInstallerPath = GetTestPath();
        string unpackComponentPath = GetTestPath();
        string packInstallerPath = GetTempPkgPath();

        string componentPkgPath = Path.Combine(unpackInstallerPath, "Simple.pkg");

        ExecuteWithCleanup(() =>
        {
            Unpack(simpleInstallerPkg, unpackInstallerPath, simpleInstallerFiles);
            Unpack(componentPkgPath, unpackComponentPath, simplePkgFiles);
            Pack(unpackComponentPath, componentPkgPath, simplePkgFiles);
            Pack(unpackInstallerPath, packInstallerPath, simpleInstallerFiles);
        }, [ unpackInstallerPath, unpackComponentPath, packInstallerPath ]);
    }

    [MacOSOnlyFact]
    public void UnpackPackAppBundleAndWithAppPkgInWithAppInstallerPkg()
    {
        string unpackInstallerPath = GetTestPath();
        string unpackComponentPath = GetTestPath();
        string unpackAppPath = GetTestPath();
        string packInstallerPath = GetTempPkgPath();
        
        string componentPkgPath = Path.Combine(unpackInstallerPath, "WithApp.pkg");
        string appPath = Path.Combine(unpackComponentPath, "Payload", "test.app");

        ExecuteWithCleanup(() =>
            {
                Unpack(withAppInstallerPkg, unpackInstallerPath, withAppInstallerFiles);
                Unpack(componentPkgPath, unpackComponentPath, withAppPkgFiles);
                Unpack(appPath, unpackAppPath, appFiles);
                Pack(unpackAppPath, appPath, appFiles);
                Pack(unpackComponentPath, componentPkgPath, withAppPkgFiles);
                Pack(unpackInstallerPath, packInstallerPath, withAppInstallerFiles);
            }, [ unpackInstallerPath, unpackComponentPath, unpackAppPath, packInstallerPath ]);
    }

    private static void ExecuteWithCleanup(Action action, List<string> cleanupPaths)
    {
        try
        {
            action();
        }
        finally
        {
            foreach (string path in cleanupPaths)
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                else if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }

    private void Unpack(string srcPath, string dstPath, (string, UnixFileMode)[] expectedFiles)
    {
        RunPkgProcess(srcPath, dstPath, "unpack").Should().BeTrue();

        Directory.Exists(dstPath).Should().BeTrue();
        
        CompareContent(dstPath, expectedFiles);
    }

    private void Pack(string srcPath, string dstPath, (string, UnixFileMode)[] expectedFiles)
    {
        RunPkgProcess(srcPath, dstPath, "pack").Should().BeTrue();

        File.Exists(dstPath).Should().BeTrue();

        // Unpack the packed pkg and verify the content
        string unpackPath = GetTestPath();
        ExecuteWithCleanup(() =>
        {
            Unpack(dstPath, unpackPath, expectedFiles);
        }, [ unpackPath ]);
    }

    private bool RunPkgProcess(string inputPath, string outputPath, string action)
    {
        var process = Process.Start(new ProcessStartInfo()
        {
            FileName = "dotnet",
            Arguments = $@"exec ""{pkgToolPath}"" {action} ""{inputPath}"" ""{outputPath}""",
            UseShellExecute = false,
            RedirectStandardError = true,
        });

        process!.WaitForExit(60000); // 60 seconds
        bool success = process.ExitCode == 0;
        if (!success)
        {
            output.WriteLine($"Error: {process.StandardError.ReadToEnd()}");
        }
        return success;
    }

    private static string GetResourceFilePath(string resourceName)
    {
        return Path.Combine(
            Path.GetDirectoryName(typeof(UnpackPackTests).Assembly.Location)!,
            "Resources",
            resourceName);
    }

    private string GetTestPath(string extension = "") =>
        Path.Combine(testRoot, $"{Path.GetRandomFileName()}{extension}");

    private string GetTempPkgPath() => GetTestPath(".pkg");

    private string GetTempAppPath() => GetTestPath(".app");

#pragma warning disable CA1416
    private static void CompareContent(string basePath, (string file, UnixFileMode mode)[] expectedFiles)
    {
        (string, UnixFileMode)[] actualFiles = Directory.GetFiles(basePath, "*.*", SearchOption.AllDirectories)
            .Select(f => (f.Substring(basePath.Length + 1), File.GetUnixFileMode(f)))
            .ToArray();
        actualFiles.Should().BeEquivalentTo(expectedFiles);
    }
}
