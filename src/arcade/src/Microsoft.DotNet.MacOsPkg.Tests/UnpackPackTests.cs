// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AwesomeAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
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

    [MacOSOnlyTheory]
    [InlineData(null, false, false)]
    [InlineData("12.0", false, false)]
    [InlineData("14.0", true, true)]
    public void UnpackPackPreservesMinimumOSVersionAndCompression(string? minimumOSVersion, bool withApp, bool inInstaller)
    {
        string sourcePath = GetTestPath();
        string originalPkgPath = GetTempPkgPath();
        string unpackPath = GetTestPath();
        string repackedPkgPath = GetTempPkgPath();
        string roundTripPath = GetTestPath();
        string expandedPath = GetTestPath();
        var expectedFiles = withApp ? withAppPkgFiles : simplePkgFiles;

        RunCommand("pkgutil", "--expand-full", withApp ? withAppPkg : simplePkg, sourcePath);

        List<string> arguments =
        [
            "--root", Path.Combine(sourcePath, "Payload"),
            "--identifier", "com.example.compression",
            "--version", "1.0",
            "--install-location", "/usr/local/share/compression",
        ];
        if (minimumOSVersion is not null)
        {
            arguments.AddRange(["--compression", "latest", "--min-os-version", minimumOSVersion]);
        }
        arguments.Add(originalPkgPath);
        RunCommand("pkgbuild", arguments.ToArray());

        string? installerPath = null;
        string? unpackedInstallerPath = null;
        if (inInstaller)
        {
            installerPath = GetTempPkgPath();
            unpackedInstallerPath = GetTestPath();
            RunCommand("productbuild", "--package", originalPkgPath, installerPath);
            RunPkgProcess(installerPath, unpackedInstallerPath, "unpack").Should().BeTrue();
            originalPkgPath = Directory.GetFiles(unpackedInstallerPath, "*.pkg").Single();
        }

        Unpack(originalPkgPath, unpackPath, expectedFiles);
        Pack(unpackPath, repackedPkgPath, expectedFiles);
        if (inInstaller)
        {
            File.Copy(repackedPkgPath, originalPkgPath, overwrite: true);
            RunPkgProcess(unpackedInstallerPath!, installerPath!, "pack").Should().BeTrue();
            string expandedInstallerPath = GetTestPath();
            RunPkgProcess(installerPath!, expandedInstallerPath, "unpack").Should().BeTrue();
            repackedPkgPath = Directory.GetFiles(expandedInstallerPath, "*.pkg").Single();
        }
        Unpack(repackedPkgPath, roundTripPath, expectedFiles);
        RunCommand("pkgutil", "--expand", repackedPkgPath, expandedPath);

        XElement packageInfo = XElement.Load(Path.Combine(expandedPath, "PackageInfo"));
        (packageInfo.Attribute("minimumSystemVersion")?.Value).Should().Be(minimumOSVersion);
        packageInfo.Attribute("identifier")!.Value.Should().Be("com.example.compression");
        packageInfo.Attribute("version")!.Value.Should().Be("1.0");
        packageInfo.Attribute("install-location")!.Value.Should().Be("/usr/local/share/compression");

        using FileStream payload = File.OpenRead(Path.Combine(expandedPath, "Payload"));
        byte[] header = new byte[4];
        payload.ReadExactly(header);
        if (minimumOSVersion is null)
        {
            header.Take(2).Should().Equal(0x1f, 0x8b);
        }
        else
        {
            header.Should().Equal((byte)'p', (byte)'b', (byte)'z', (byte)'x');
        }

        string payloadPath = Path.Combine(roundTripPath, "Payload");
        if (withApp)
        {
            string appPath = GetTestPath();
            Unpack(Path.Combine(payloadPath, "test.app"), appPath, appFiles);
            foreach (var (file, _) in appFiles)
            {
                File.ReadAllBytes(Path.Combine(appPath, file))
                    .Should().Equal(File.ReadAllBytes(Path.Combine(sourcePath, "Payload", "test.app", file)));
            }
        }
        else
        {
            File.ReadAllBytes(Path.Combine(payloadPath, "Sample.txt"))
                .Should().Equal(File.ReadAllBytes(Path.Combine(sourcePath, "Payload", "Sample.txt")));
        }

        void RunCommand(string command, params string[] arguments)
        {
            ProcessTextOutput result = Process.RunAndCaptureText(command, arguments, timeout: TimeSpan.FromMinutes(1));

            output.WriteLine(result.StandardOutput);
            result.ExitStatus.ExitCode.Should().Be(0, result.StandardError);
        }
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
