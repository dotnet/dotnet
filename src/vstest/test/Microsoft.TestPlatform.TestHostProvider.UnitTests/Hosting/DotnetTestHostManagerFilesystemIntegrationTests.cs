// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// These integration tests verify that DotnetTestHostManager correctly reads real deps.json and
// runtimeconfig.dev.json files from disk (without mocking the file system) to locate testhost.dll.
// They guard against regressions in the DepsJsonParser and runtimeconfig.dev.json parsing logic.

using System;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Hosting;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.TestHostProvider.UnitTests.Hosting;

/// <summary>
/// Integration tests for DotnetTestHostManager that use real file system I/O (no IFileHelper mock).
/// These tests ensure that deps.json and runtimeconfig.dev.json parsing correctly locates testhost.dll
/// through the actual parsing code paths (DepsJsonParser on .NET Framework, DependencyContextJsonReader
/// on .NET Core, and Jsonite-based runtimeconfig.dev.json parsing).
/// </summary>
[TestClass]
public class DotnetTestHostManagerFilesystemIntegrationTests
{
    private string _tempDir = null!;
    private readonly Mock<IProcessHelper> _mockProcessHelper = new();
    private readonly Mock<IEnvironment> _mockEnvironment = new();
    private readonly Mock<IMessageLogger> _mockMessageLogger = new();
    private readonly Mock<IWindowsRegistryHelper> _mockWindowsRegistry = new();
    private readonly Mock<IEnvironmentVariableHelper> _mockEnvironmentVariable = new();
    private readonly TestRunnerConnectionInfo _connectionInfo = new()
    {
        Port = 123,
        ConnectionInfo = new() { Endpoint = "127.0.0.1:123", Role = ConnectionRole.Client },
        RunnerProcessId = 0,
    };

    [TestInitialize]
    public void SetUp()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), "vstest-integration-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempDir);

        // Simulate non-Windows so the code goes to testhost.dll discovery (not testhost.exe).
        _mockEnvironment.Setup(e => e.OperatingSystem).Returns(PlatformOperatingSystem.Unix);
        _mockEnvironment.SetupGet(e => e.Architecture).Returns(PlatformArchitecture.X64);

        // Current process is a dotnet muxer so it is used as-is for FileName.
        _mockProcessHelper.Setup(p => p.GetCurrentProcessFileName()).Returns("/usr/bin/dotnet");
        _mockProcessHelper.Setup(p => p.GetTestEngineDirectory()).Returns("/usr/bin");
        _mockProcessHelper.Setup(p => p.GetCurrentProcessArchitecture()).Returns(PlatformArchitecture.X64);

        // No special environment variables.
        _mockEnvironmentVariable.Setup(e => e.GetEnvironmentVariable(It.IsAny<string>())).Returns((string?)null);
    }

    [TestCleanup]
    public void TearDown()
    {
        try
        {
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, recursive: true);
            }
        }
        catch
        {
            // Best-effort cleanup; do not fail the test on a leaked temp dir.
        }
    }

    [TestMethod]
    public void GetTestHostProcessStartInfo_FindsTestHostDllViaRealDepsJsonAndRuntimeConfigDevJson()
    {
        // Arrange — create real deps.json and runtimeconfig.dev.json files and a fake testhost.dll.
        string packagesDir = Path.Combine(_tempDir, "packages");
        string packageRelativePath = Path.Combine("microsoft.testplatform.testhost", "18.0.0-dev");
        string testhostFullPath = Path.Combine(packagesDir, packageRelativePath, "lib", "net8.0", "testhost.dll");

        Directory.CreateDirectory(Path.GetDirectoryName(testhostFullPath)!);
        File.WriteAllText(testhostFullPath, "fake testhost");

        string depsJson = $$"""
            {
                "runtimeTarget": {
                    "name": ".NETCoreApp,Version=v8.0",
                    "signature": "abc123"
                },
                "compilationOptions": {},
                "targets": {
                    ".NETCoreApp,Version=v8.0": {
                        "microsoft.testplatform.testhost/18.0.0-dev": {
                            "dependencies": {},
                            "runtime": {
                                "lib/net8.0/testhost.dll": {}
                            }
                        }
                    }
                },
                "libraries": {
                    "microsoft.testplatform.testhost/18.0.0-dev": {
                        "type": "package",
                        "serviceable": true,
                        "sha512": "",
                        "path": "{{packageRelativePath.Replace("\\", "/")}}",
                        "hashPath": ""
                    }
                }
            }
            """;

        string runtimeConfigDevJson = $$"""
            {
                "runtimeOptions": {
                    "additionalProbingPaths": [
                        "{{packagesDir.Replace("\\", "\\\\")}}"
                    ]
                }
            }
            """;

        string sourceDll = Path.Combine(_tempDir, "TestProject.dll");
        File.WriteAllText(sourceDll, "fake test dll");
        File.WriteAllText(Path.Combine(_tempDir, "TestProject.deps.json"), depsJson);
        File.WriteAllText(Path.Combine(_tempDir, "TestProject.runtimeconfig.dev.json"), runtimeConfigDevJson);

        var manager = CreateManager(new FileHelper());
        manager.Initialize(_mockMessageLogger.Object, "<RunSettings><RunConfiguration><TargetFrameworkVersion>.NETCoreApp,Version=v8.0</TargetFrameworkVersion></RunConfiguration></RunSettings>");

        // Act
        var startInfo = manager.GetTestHostProcessStartInfo(new[] { sourceDll }, null, _connectionInfo);

        // Assert — testhost.dll is resolved via probing path from runtimeconfig.dev.json
        Assert.IsNotNull(startInfo.Arguments);
        // Normalize path separators: deps.json paths use forward slashes while Path.Combine may produce
        // backslashes on Windows, resulting in mixed separators in the resolved path.
        Assert.Contains(
            testhostFullPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar),
            startInfo.Arguments.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar));
    }

    [TestMethod]
    public void GetTestHostProcessStartInfo_DoesNotThrowWhenRuntimeConfigDevJsonHasNoAdditionalProbingPaths()
    {
        // Arrange — a runtimeconfig.dev.json that exists but only carries runtime options other than
        // additionalProbingPaths (for example Hot Reload switches generated by the SDK for net6.0+ Debug builds).
        // The manager must not fail reading it; it should fall back to finding testhost.dll next to the source.
        string sourceDll = Path.Combine(_tempDir, "TestProject.dll");
        string depsJsonPath = Path.Combine(_tempDir, "TestProject.deps.json");

        File.WriteAllText(sourceDll, "fake test dll");

        // Minimal deps.json — testhost not in it, so resolution falls through to the source directory.
        string depsJson = """
            {
                "runtimeTarget": { "name": ".NETCoreApp,Version=v8.0" },
                "compilationOptions": {},
                "targets": { ".NETCoreApp,Version=v8.0": {} },
                "libraries": {}
            }
            """;
        File.WriteAllText(depsJsonPath, depsJson);

        // runtimeconfig.dev.json without an "additionalProbingPaths" property.
        string runtimeConfigDevJson = """
            {
                "runtimeOptions": {
                    "configProperties": {
                        "System.Reflection.Metadata.MetadataUpdater.IsSupported": true,
                        "System.StartupHookProvider.IsSupported": true
                    }
                }
            }
            """;
        File.WriteAllText(Path.Combine(_tempDir, "TestProject.runtimeconfig.dev.json"), runtimeConfigDevJson);

        // Place testhost.dll next to source so the manager can resolve it without probing paths.
        string testhostNextToSource = Path.Combine(_tempDir, "testhost.dll");
        File.WriteAllText(testhostNextToSource, "fake testhost");

        var manager = CreateManager(new FileHelper());
        manager.Initialize(_mockMessageLogger.Object, "<RunSettings><RunConfiguration><TargetFrameworkVersion>.NETCoreApp,Version=v8.0</TargetFrameworkVersion></RunConfiguration></RunSettings>");

        // Act — this previously threw KeyNotFoundException when additionalProbingPaths was missing.
        var startInfo = manager.GetTestHostProcessStartInfo(new[] { sourceDll }, null, _connectionInfo);

        // Assert — resolution succeeds and falls back to testhost.dll next to the source.
        Assert.IsNotNull(startInfo.Arguments);
        Assert.Contains("testhost.dll", startInfo.Arguments);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfo_PassesDepsFileArgWhenDepsJsonExists()
    {
        // Arrange — when a .deps.json file exists next to the test dll, --depsfile should appear in args.
        string sourceDll = Path.Combine(_tempDir, "TestProject.dll");
        string depsJsonPath = Path.Combine(_tempDir, "TestProject.deps.json");

        File.WriteAllText(sourceDll, "fake test dll");

        // Minimal deps.json — testhost not in it, so it will fall through to other fallbacks.
        string depsJson = """
            {
                "runtimeTarget": { "name": ".NETCoreApp,Version=v8.0" },
                "compilationOptions": {},
                "targets": { ".NETCoreApp,Version=v8.0": {} },
                "libraries": {}
            }
            """;
        File.WriteAllText(depsJsonPath, depsJson);

        // Place testhost.dll next to source so the manager doesn't throw "not found".
        string testhostNextToSource = Path.Combine(_tempDir, "testhost.dll");
        File.WriteAllText(testhostNextToSource, "fake testhost");

        var manager = CreateManager(new FileHelper());
        manager.Initialize(_mockMessageLogger.Object, "<RunSettings><RunConfiguration><TargetFrameworkVersion>.NETCoreApp,Version=v8.0</TargetFrameworkVersion></RunConfiguration></RunSettings>");

        // Act
        var startInfo = manager.GetTestHostProcessStartInfo(new[] { sourceDll }, null, _connectionInfo);

        // Assert — --depsfile argument must reference the real deps.json path
        Assert.IsNotNull(startInfo.Arguments);
        Assert.Contains("--depsfile", startInfo.Arguments);
        Assert.Contains("TestProject.deps.json", startInfo.Arguments);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfo_PassesRuntimeConfigArgWhenRuntimeConfigExists()
    {
        // Arrange — when a .runtimeconfig.json file exists next to the test dll, --runtimeconfig should appear in args.
        string sourceDll = Path.Combine(_tempDir, "TestProject.dll");
        string runtimeConfigPath = Path.Combine(_tempDir, "TestProject.runtimeconfig.json");

        File.WriteAllText(sourceDll, "fake test dll");
        File.WriteAllText(runtimeConfigPath, """{"runtimeOptions":{"tfm":"net8.0"}}""");

        // Place testhost.dll next to source so the manager doesn't throw.
        string testhostNextToSource = Path.Combine(_tempDir, "testhost.dll");
        File.WriteAllText(testhostNextToSource, "fake testhost");

        var manager = CreateManager(new FileHelper());
        manager.Initialize(_mockMessageLogger.Object, "<RunSettings><RunConfiguration><TargetFrameworkVersion>.NETCoreApp,Version=v8.0</TargetFrameworkVersion></RunConfiguration></RunSettings>");

        // Act
        var startInfo = manager.GetTestHostProcessStartInfo(new[] { sourceDll }, null, _connectionInfo);

        // Assert — --runtimeconfig argument must reference the real runtimeconfig.json path
        Assert.IsNotNull(startInfo.Arguments);
        Assert.Contains("--runtimeconfig", startInfo.Arguments);
        Assert.Contains("TestProject.runtimeconfig.json", startInfo.Arguments);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfo_FindsTestHostDllNextToSourceWhenNoDepsJson()
    {
        // Arrange — no runtimeconfig.dev.json, testhost.dll is placed next to the source dll.
        string sourceDll = Path.Combine(_tempDir, "TestProject.dll");
        string testhostNextToSource = Path.Combine(_tempDir, "testhost.dll");

        File.WriteAllText(sourceDll, "fake test dll");
        File.WriteAllText(testhostNextToSource, "fake testhost");

        var manager = CreateManager(new FileHelper());
        manager.Initialize(_mockMessageLogger.Object, "<RunSettings><RunConfiguration><TargetFrameworkVersion>.NETCoreApp,Version=v8.0</TargetFrameworkVersion></RunConfiguration></RunSettings>");

        // Act
        var startInfo = manager.GetTestHostProcessStartInfo(new[] { sourceDll }, null, _connectionInfo);

        // Assert — testhost.dll next to source is found
        Assert.IsNotNull(startInfo.Arguments);
        Assert.Contains("testhost.dll", startInfo.Arguments);
    }

    private DotnetTestHostManager CreateManager(IFileHelper fileHelper)
        => new(
            _mockProcessHelper.Object,
            fileHelper,
            new DotnetHostHelper(fileHelper, _mockEnvironment.Object, _mockWindowsRegistry.Object, _mockEnvironmentVariable.Object, _mockProcessHelper.Object),
            _mockEnvironment.Object,
            _mockWindowsRegistry.Object,
            _mockEnvironmentVariable.Object);
}
