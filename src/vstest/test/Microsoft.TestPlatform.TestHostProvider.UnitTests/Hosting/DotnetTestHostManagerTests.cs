// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
#if NET
using System.Reflection;
#endif
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Hosting;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.TestHostProvider.UnitTests.Hosting;

[TestClass]
public class DotnetTestHostManagerTests
{
    private const string DefaultDotnetPath = "c:\\tmp\\dotnet.exe";

    private readonly Mock<ITestHostLauncher> _mockTestHostLauncher;
    private readonly Mock<IProcessHelper> _mockProcessHelper;
    private readonly Mock<IFileHelper> _mockFileHelper;
    private readonly Mock<IWindowsRegistryHelper> _mockWindowsRegistry;
    private readonly Mock<IMessageLogger> _mockMessageLogger;
    private readonly Mock<IEnvironment> _mockEnvironment;
    private readonly Mock<IRunSettingsHelper> _mockRunsettingHelper;
    private readonly TestRunnerConnectionInfo _defaultConnectionInfo;
    private readonly string[] _testSource = ["test.dll"];
    private readonly string _defaultTestHostPath;
    private readonly TestProcessStartInfo _defaultTestProcessStartInfo;
    private readonly TestableDotnetTestHostManager _dotnetHostManager;
    private readonly Mock<IEnvironmentVariableHelper> _mockEnvironmentVariable;

    private string? _errorMessage;
    private int _exitCode;
    private int _testHostId;

    private readonly string _temp = Path.GetTempPath();

    public DotnetTestHostManagerTests()
    {
        _mockTestHostLauncher = new Mock<ITestHostLauncher>();
        _mockProcessHelper = new Mock<IProcessHelper>();
        _mockFileHelper = new Mock<IFileHelper>();
        _mockMessageLogger = new Mock<IMessageLogger>();
        _mockEnvironment = new Mock<IEnvironment>();
        _mockWindowsRegistry = new Mock<IWindowsRegistryHelper>();
        _mockEnvironmentVariable = new Mock<IEnvironmentVariableHelper>();
        _mockRunsettingHelper = new Mock<IRunSettingsHelper>();
        _defaultConnectionInfo = new TestRunnerConnectionInfo { Port = 123, ConnectionInfo = new TestHostConnectionInfo { Endpoint = "127.0.0.1:123", Role = ConnectionRole.Client }, RunnerProcessId = 0 };

        _mockEnvironment.SetupGet(e => e.Architecture).Returns((PlatformArchitecture)Enum.Parse(typeof(PlatformArchitecture), Constants.DefaultPlatform.ToString()));
        _mockRunsettingHelper.SetupGet(r => r.IsDefaultTargetArchitecture).Returns(true);
        string defaultSourcePath = Path.Combine(_temp, "test.dll");
        _defaultTestHostPath = Path.Combine(_temp, "testhost.dll");
        _dotnetHostManager = new TestableDotnetTestHostManager(
            _mockProcessHelper.Object,
            _mockFileHelper.Object,
            new DotnetHostHelper(_mockFileHelper.Object, _mockEnvironment.Object, _mockWindowsRegistry.Object, _mockEnvironmentVariable.Object, _mockProcessHelper.Object),
            _mockEnvironment.Object,
            _mockRunsettingHelper.Object,
            _mockWindowsRegistry.Object,
            _mockEnvironmentVariable.Object);
        _dotnetHostManager.Initialize(_mockMessageLogger.Object, string.Empty);

        _dotnetHostManager.HostExited += DotnetHostManagerHostExited;

        // Setup a dummy current process for tests
        _mockProcessHelper.Setup(ph => ph.GetCurrentProcessFileName()).Returns(DefaultDotnetPath);
        _mockProcessHelper.Setup(ph => ph.GetTestEngineDirectory()).Returns(DefaultDotnetPath);
        _mockProcessHelper.Setup(ph => ph.GetCurrentProcessArchitecture()).Returns(PlatformArchitecture.X64);
        _mockEnvironmentVariable.Setup(ev => ev.GetEnvironmentVariable(It.IsAny<string>())).Returns(Path.GetDirectoryName(DefaultDotnetPath)!);
        _mockFileHelper.Setup(ph => ph.Exists(_defaultTestHostPath)).Returns(true);
        _mockFileHelper.Setup(ph => ph.Exists(DefaultDotnetPath)).Returns(true);

#if NET5_0_OR_GREATER
        var pid = Environment.ProcessId;
#else
        int pid;
        using (var p = Process.GetCurrentProcess())
            pid = p.Id;
#endif

        _mockTestHostLauncher
            .Setup(th => th.LaunchTestHost(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>()))
            .Returns(pid);

        _mockTestHostLauncher
            .Setup(th => th.LaunchTestHost(It.IsAny<TestProcessStartInfo>()))
            .Returns(pid);

        _defaultTestProcessStartInfo = _dotnetHostManager.GetTestHostProcessStartInfo(new[] { defaultSourcePath }, null, _defaultConnectionInfo);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldThrowIfSourceIsNull()
    {
        Action action = () => _dotnetHostManager.GetTestHostProcessStartInfo(null!, null, _defaultConnectionInfo);

        Assert.ThrowsException<ArgumentNullException>(action);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldThrowIfMultipleSourcesAreProvided()
    {
        var sources = new[] { "test1.dll", "test2.dll" };
        Action action = () => _dotnetHostManager.GetTestHostProcessStartInfo(sources, null, _defaultConnectionInfo);

        Assert.ThrowsException<InvalidOperationException>(action);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldInvokeDotnetCommandline()
    {
        _mockProcessHelper.Setup(ph => ph.GetCurrentProcessFileName()).Returns(DefaultDotnetPath);
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);

        var startInfo = GetDefaultStartInfo();

        Assert.AreEqual(DefaultDotnetPath, startInfo.FileName);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldInvokeDotnetXPlatOnLinux()
    {
        _mockProcessHelper.Setup(ph => ph.GetCurrentProcessFileName()).Returns("/tmp/dotnet");
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);

        var startInfo = GetDefaultStartInfo();

        Assert.AreEqual("/tmp/dotnet", startInfo.FileName);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldInvokeDotnetExec()
    {
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);
        var startInfo = GetDefaultStartInfo();

        StringAssert.StartsWith(startInfo.Arguments, "exec");
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldAddRuntimeConfigJsonIfExists()
    {
        _mockFileHelper.Setup(fh => fh.Exists("test.runtimeconfig.json")).Returns(true);
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);

        var startInfo = GetDefaultStartInfo();

        StringAssert.Contains(startInfo.Arguments, "--runtimeconfig \"test.runtimeconfig.json\"");
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldNotAddRuntimeConfigJsonIfNotExists()
    {
        _mockFileHelper.Setup(fh => fh.Exists("test.runtimeconfig.json")).Returns(false);
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);

        var startInfo = GetDefaultStartInfo();

        Assert.IsFalse(startInfo.Arguments!.Contains("--runtimeconfig \"test.runtimeconfig.json\""));
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldAddDepsFileJsonIfExists()
    {
        _mockFileHelper.Setup(fh => fh.Exists("test.deps.json")).Returns(true);
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);

        var startInfo = GetDefaultStartInfo();

        StringAssert.Contains(startInfo.Arguments, "--depsfile \"test.deps.json\"");
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldNotAddDepsFileJsonIfNotExists()
    {
        _mockFileHelper.Setup(fh => fh.Exists("test.deps.json")).Returns(false);
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);

        var startInfo = GetDefaultStartInfo();

        Assert.IsFalse(startInfo.Arguments!.Contains("--depsfile \"test.deps.json\""));
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldIncludeConnectionInfo()
    {
        var connectionInfo = new TestRunnerConnectionInfo { Port = 123, ConnectionInfo = new TestHostConnectionInfo { Endpoint = "127.0.0.0:123", Role = ConnectionRole.Client, Transport = Transport.Sockets }, RunnerProcessId = 101 };
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);

        var startInfo = _dotnetHostManager.GetTestHostProcessStartInfo(_testSource, null, connectionInfo);

        StringAssert.Contains(startInfo.Arguments, "--port " + connectionInfo.Port + " --endpoint " + connectionInfo.ConnectionInfo.Endpoint + " --role client --parentprocessid 101");
    }

    [TestMethod]
    public void GetTestHostConnectionInfoShouldIncludeEndpointRoleAndChannelType()
    {
        var connectionInfo = new TestHostConnectionInfo { Endpoint = "127.0.0.1:0", Role = ConnectionRole.Client, Transport = Transport.Sockets };

        var info = _dotnetHostManager.GetTestHostConnectionInfo();

        Assert.AreEqual(connectionInfo, info);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldIncludeEnvironmentVariables()
    {
        var environmentVariables = new Dictionary<string, string?> { { "k1", "v1" }, { "k2", "v2" } };
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);

        var startInfo = _dotnetHostManager.GetTestHostProcessStartInfo(_testSource, environmentVariables, _defaultConnectionInfo);

        Assert.AreEqual(environmentVariables, startInfo.EnvironmentVariables);
    }

    [TestMethod]
    public void GetTestHostProcessStartIfDepsFileNotFoundAndTestHostFoundShouldNotThrowException()
    {
        _mockFileHelper.Setup(fh => fh.Exists("test.deps.json")).Returns(false);
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);

        var startInfo = GetDefaultStartInfo();
        StringAssert.Contains(startInfo.Arguments, "testhost.dll");
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldUseTestHostX64ExePresentOnWindows()
    {
        var testhostExePath = "testhost.exe";
        _mockFileHelper.Setup(ph => ph.Exists(testhostExePath)).Returns(true);
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);
        _mockEnvironment.Setup(ev => ev.OperatingSystem).Returns(PlatformOperatingSystem.Windows);

        var startInfo = GetDefaultStartInfo();

        StringAssert.Contains(startInfo.FileName, testhostExePath);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldUseDotnetExeOnUnixWithTestHostDllPath()
    {
        _mockFileHelper.Setup(ph => ph.Exists("testhost.x86.exe")).Returns(true);
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);
        _mockEnvironment.Setup(ev => ev.OperatingSystem).Returns(PlatformOperatingSystem.Unix);

        var startInfo = GetDefaultStartInfo();

        StringAssert.Contains(startInfo.FileName, "dotnet");
        StringAssert.Contains(startInfo.Arguments, "testhost.dll");
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldUseTestHostExeIfPresentOnWindows()
    {
        var testhostExePath = "testhost.exe";
        _mockFileHelper.Setup(ph => ph.Exists(testhostExePath)).Returns(true);
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);
        _mockEnvironment.Setup(ev => ev.OperatingSystem).Returns(PlatformOperatingSystem.Windows);

        _dotnetHostManager.Initialize(_mockMessageLogger.Object, "<RunSettings><RunConfiguration><TargetPlatform>x64</TargetPlatform></RunConfiguration></RunSettings>");
        var startInfo = GetDefaultStartInfo();

        StringAssert.Contains(startInfo.FileName, testhostExePath);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldUseDotnetHostPathFromRunsettings()
    {
        var dotnetHostPath = @"C:\dotnet.exe";
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);
        _mockEnvironment.Setup(ev => ev.OperatingSystem).Returns(PlatformOperatingSystem.Windows);
        _dotnetHostManager.Initialize(_mockMessageLogger.Object, $"<RunSettings><RunConfiguration><DotNetHostPath>{dotnetHostPath}</DotNetHostPath></RunConfiguration></RunSettings>");
        var startInfo = GetDefaultStartInfo();

        StringAssert.Contains(startInfo.FileName, dotnetHostPath);
    }

    [TestMethod]
    [TestCategory("Windows")]
    public void GetTestHostProcessStartInfoShouldUseTestHostExeFromNugetIfNotFoundInSourceLocation()
    {
        var testhostExePath = "testhost.exe";
        _dotnetHostManager.Initialize(_mockMessageLogger.Object, "<RunSettings><RunConfiguration><TargetPlatform>x64</TargetPlatform></RunConfiguration></RunSettings>");
        _mockFileHelper.Setup(ph => ph.Exists(testhostExePath)).Returns(false);
        _mockFileHelper.Setup(ph => ph.Exists("C:\\packages\\microsoft.testplatform.testhost\\15.0.0-Dev\\build\\net8.0\\x64\\testhost.exe")).Returns(true);
        _mockEnvironment.Setup(ev => ev.OperatingSystem).Returns(PlatformOperatingSystem.Windows);
        var sourcePath = Path.Combine(_temp, "test.dll");

        string runtimeConfigFileContent =
            @"{
    ""runtimeOptions"": {
        ""additionalProbingPaths"": [
            ""C:\\packages""
            ]
        }
}";

        string depsFileContent =
            @"{
    ""runtimeTarget"": {
        ""name"": "".NETCoreApp,Version=v1.0"",
        ""signature"": ""8f25843f8e35a3e80ef4ae98b95117ea5c468b3f""
    },
    ""compilationOptions"": {},
    ""targets"": {
        "".NETCoreApp,Version=v1.0"": {
            ""microsoft.testplatform.testhost/15.0.0-Dev"": {
                ""dependencies"": {
                    ""Microsoft.TestPlatform.ObjectModel"": ""15.0.0-Dev"",
                    ""Newtonsoft.Json"": ""13.0.3""
                },
                ""runtime"": {
                    ""lib/netstandard1.5/Microsoft.TestPlatform.CommunicationUtilities.dll"": { },
                    ""lib/netstandard1.5/Microsoft.TestPlatform.CrossPlatEngine.dll"": { },
                    ""lib/netstandard1.5/Microsoft.VisualStudio.TestPlatform.Common.dll"": { },
                    ""lib/netstandard1.5/testhost.dll"": { }
                }
            }
        }
    },
    ""libraries"": {
        ""microsoft.testplatform.testhost/15.0.0-Dev"": {
        ""type"": ""package"",
        ""serviceable"": true,
        ""sha512"": ""sha512-enO8sZmjbhXOfiZ6hV2ncaknaHnQbrGVsHUJzzu2Dmoh4fHFro4BF1Y4+sb4LOQhu4b3DFYPRj1ncd1RQK6HmQ=="",
        ""path"": ""microsoft.testplatform.testhost/15.0.0-Dev"",
        ""hashPath"": ""microsoft.testplatform.testhost.15.0.0-Dev""
        }
    }
}";

        MemoryStream runtimeConfigStream = new(Encoding.UTF8.GetBytes(runtimeConfigFileContent));
        _mockFileHelper.Setup(ph => ph.GetStream(Path.Combine(_temp, "test.runtimeconfig.dev.json"), FileMode.Open, FileAccess.Read)).Returns(runtimeConfigStream);
        _mockFileHelper.Setup(ph => ph.Exists(Path.Combine(_temp, "test.runtimeconfig.dev.json"))).Returns(true);

        MemoryStream depsFileStream = new(Encoding.UTF8.GetBytes(depsFileContent));
        _mockFileHelper.Setup(ph => ph.GetStream(Path.Combine(_temp, "test.deps.json"), FileMode.Open, FileAccess.Read)).Returns(depsFileStream);
        _mockFileHelper.Setup(ph => ph.Exists(Path.Combine(_temp, "test.deps.json"))).Returns(true);

        string testHostFullPath = @"C:\packages\microsoft.testplatform.testhost/15.0.0-Dev\lib/netstandard1.5/testhost.dll";
        _mockFileHelper.Setup(ph => ph.Exists(testHostFullPath)).Returns(true);

        var startInfo = _dotnetHostManager.GetTestHostProcessStartInfo(new[] { sourcePath }, null, _defaultConnectionInfo);


        // If this starts failing after updating TFMs of packakges, the GetTestHostProcessStartInfo defines the default version
        // to use in GetTestHostProcessStartInfo, change that to the lowest supported netcore version, and pass this test.
        StringAssert.Contains(startInfo.FileName, "C:\\packages\\microsoft.testplatform.testhost\\15.0.0-Dev\\build\\net8.0\\x64\\testhost.exe");
    }

    [TestMethod]
    [TestCategory("Windows")]
    public void GetTestHostProcessStartInfoShouldUseTestHostX86ExeFromNugetIfNotFoundInSourceLocation()
    {
        var testhostExePath = "testhost.x86.exe";
        _dotnetHostManager.Initialize(_mockMessageLogger.Object, "<RunSettings><RunConfiguration><TargetPlatform>x86</TargetPlatform></RunConfiguration></RunSettings>");
        _mockFileHelper.Setup(ph => ph.Exists(testhostExePath)).Returns(false);
        _mockFileHelper.Setup(ph => ph.Exists($"C:\\packages{Path.DirectorySeparatorChar}microsoft.testplatform.testhost\\15.0.0-Dev{Path.DirectorySeparatorChar}build\\net8.0\\x86\\testhost.x86.exe")).Returns(true);
        _mockEnvironment.Setup(ev => ev.OperatingSystem).Returns(PlatformOperatingSystem.Windows);
        var sourcePath = Path.Combine(_temp, "test.dll");

        string runtimeConfigFileContent =
            @"{
    ""runtimeOptions"": {
        ""additionalProbingPaths"": [
            ""C:\\packages""
            ]
        }
}";

        string depsFileContent =
            @"{
    ""runtimeTarget"": {
        ""name"": "".NETCoreApp,Version=v1.0"",
        ""signature"": ""8f25843f8e35a3e80ef4ae98b95117ea5c468b3f""
    },
    ""compilationOptions"": {},
    ""targets"": {
        "".NETCoreApp,Version=v1.0"": {
            ""microsoft.testplatform.testhost/15.0.0-Dev"": {
                ""dependencies"": {
                    ""Microsoft.TestPlatform.ObjectModel"": ""15.0.0-Dev"",
                    ""Newtonsoft.Json"": ""13.0.3""
                },
                ""runtime"": {
                    ""lib/netstandard1.5/Microsoft.TestPlatform.CommunicationUtilities.dll"": { },
                    ""lib/netstandard1.5/Microsoft.TestPlatform.CrossPlatEngine.dll"": { },
                    ""lib/netstandard1.5/Microsoft.VisualStudio.TestPlatform.Common.dll"": { },
                    ""lib/netstandard1.5/testhost.dll"": { }
                }
            }
        }
    },
    ""libraries"": {
        ""microsoft.testplatform.testhost/15.0.0-Dev"": {
        ""type"": ""package"",
        ""serviceable"": true,
        ""sha512"": ""sha512-enO8sZmjbhXOfiZ6hV2ncaknaHnQbrGVsHUJzzu2Dmoh4fHFro4BF1Y4+sb4LOQhu4b3DFYPRj1ncd1RQK6HmQ=="",
        ""path"": ""microsoft.testplatform.testhost/15.0.0-Dev"",
        ""hashPath"": ""microsoft.testplatform.testhost.15.0.0-Dev""
        }
    }
}";

        MemoryStream runtimeConfigStream = new(Encoding.UTF8.GetBytes(runtimeConfigFileContent));
        _mockFileHelper.Setup(ph => ph.GetStream(Path.Combine(_temp, "test.runtimeconfig.dev.json"), FileMode.Open, FileAccess.Read)).Returns(runtimeConfigStream);
        _mockFileHelper.Setup(ph => ph.Exists(Path.Combine(_temp, "test.runtimeconfig.dev.json"))).Returns(true);

        MemoryStream depsFileStream = new(Encoding.UTF8.GetBytes(depsFileContent));
        _mockFileHelper.Setup(ph => ph.GetStream(Path.Combine(_temp, "test.deps.json"), FileMode.Open, FileAccess.Read)).Returns(depsFileStream);
        _mockFileHelper.Setup(ph => ph.Exists(Path.Combine(_temp, "test.deps.json"))).Returns(true);

        string testHostFullPath = $@"C:\packages{Path.DirectorySeparatorChar}microsoft.testplatform.testhost/15.0.0-Dev{Path.DirectorySeparatorChar}lib/netstandard1.5/testhost.dll";
        _mockFileHelper.Setup(ph => ph.Exists(testHostFullPath)).Returns(true);

        var startInfo = _dotnetHostManager.GetTestHostProcessStartInfo(new[] { sourcePath }, null, _defaultConnectionInfo);

        // If this starts failing after updating TFMs of packakges, the GetTestHostProcessStartInfo defines the default version
        // to use in GetTestHostProcessStartInfo, change that to the lowest supported netcore version, and pass this test.
        StringAssert.Contains(startInfo.FileName, "C:\\packages\\microsoft.testplatform.testhost\\15.0.0-Dev\\build\\net8.0\\x86\\testhost.x86.exe");
    }

    [TestMethod]
    public void LaunchTestHostShouldLaunchProcessWithNullEnvironmentVariablesOrArgs()
    {
#if NET5_0_OR_GREATER
        var expectedProcessId = Environment.ProcessId;
#else
        int expectedProcessId;
        using (var p = Process.GetCurrentProcess())
            expectedProcessId = p.Id;
#endif
        _mockTestHostLauncher.Setup(thl => thl.LaunchTestHost(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(expectedProcessId);
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);
        var startInfo = GetDefaultStartInfo();
        _dotnetHostManager.SetCustomLauncher(_mockTestHostLauncher.Object);

        _dotnetHostManager.HostLaunched += DotnetHostManagerHostLaunched;

        Task<bool> processId = _dotnetHostManager.LaunchTestHostAsync(startInfo, CancellationToken.None);
        processId.Wait();

        Assert.IsTrue(processId.Result);
        Assert.AreEqual(expectedProcessId, _testHostId);
    }

    [TestMethod]
    public void LaunchTestHostAsyncShouldNotStartHostProcessIfCancellationTokenIsSet()
    {
#if NET5_0_OR_GREATER
        var expectedProcessId = Environment.ProcessId;
#else
        int expectedProcessId;
        using (var p = Process.GetCurrentProcess())
            expectedProcessId = p.Id;
#endif
        _mockTestHostLauncher.Setup(thl => thl.LaunchTestHost(It.IsAny<TestProcessStartInfo>())).Returns(expectedProcessId);
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);
        var startInfo = GetDefaultStartInfo();

        CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.Cancel();

        Assert.ThrowsException<OperationCanceledException>(() => _dotnetHostManager.LaunchTestHostAsync(startInfo, cancellationTokenSource.Token).Wait());
    }

    [TestMethod]
    public void LaunchTestHostShouldLaunchProcessWithEnvironmentVariables()
    {
        var variables = new Dictionary<string, string?> { { "k1", "v1" }, { "k2", "v2" } };
        var startInfo = new TestProcessStartInfo { EnvironmentVariables = variables };
        _dotnetHostManager.SetCustomLauncher(_mockTestHostLauncher.Object);

        _dotnetHostManager.HostLaunched += DotnetHostManagerHostLaunched;

        Task<bool> processId = _dotnetHostManager.LaunchTestHostAsync(startInfo, CancellationToken.None);
        processId.Wait();

        Assert.IsTrue(processId.Result);
        _mockTestHostLauncher.Verify(thl => thl.LaunchTestHost(It.Is<TestProcessStartInfo>(x => x.EnvironmentVariables!.Equals(variables)), It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public void DotnetTestHostManagerShouldNotBeShared()
    {
        Assert.IsFalse(_dotnetHostManager.Shared);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldThrowExceptionWhenDotnetIsNotInstalled()
    {
        // To validate the else part, set current process to exe other than dotnet
        _mockProcessHelper.Setup(ph => ph.GetCurrentProcessFileName()).Returns("vstest.console.exe");

        // Reset the muxer
        _mockEnvironmentVariable.Setup(ev => ev.GetEnvironmentVariable(It.IsAny<string>())).Returns(string.Empty);

        char separator = ';';
        var dotnetExeName = "dotnet.exe";
        if (!Environment.OSVersion.Platform.ToString().StartsWith("Win"))
        {
            separator = ':';
            dotnetExeName = "dotnet";
        }

        var paths = Environment.GetEnvironmentVariable("PATH")!.Split(separator);

        foreach (string path in paths)
        {
            string dotnetExeFullPath = Path.Combine(path.Trim(), dotnetExeName);
            _mockFileHelper.Setup(fh => fh.Exists(dotnetExeFullPath)).Returns(false);
        }

        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);

        Action action = () => GetDefaultStartInfo();

        Assert.ThrowsException<TestPlatformException>(action);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldIncludeSourceDirectoryAsWorkingDirectory()
    {
        // Absolute path to the source directory
        var sourcePath = Path.Combine(_temp, "test.dll");
        _mockFileHelper.Setup(ph => ph.Exists(Path.Combine(_temp, "testhost.dll"))).Returns(true);
        var startInfo = _dotnetHostManager.GetTestHostProcessStartInfo(new[] { sourcePath }, null, _defaultConnectionInfo);

        Assert.AreEqual(Path.GetDirectoryName(_temp), startInfo.WorkingDirectory);
    }

    [TestMethod]
    public void GetTestPlatformExtensionsShouldReturnEmptySetIfSourceDirectoryDoesNotExist()
    {
        _mockFileHelper.Setup(fh => fh.DirectoryExists(It.IsAny<string>())).Returns(false);
        var extensions = _dotnetHostManager.GetTestPlatformExtensions(new[] { $".{Path.DirectorySeparatorChar}foo.dll" }, []);

        Assert.AreEqual(0, extensions.Count());
    }

    [TestMethod]
    public void GetTestPlatformExtensionsShouldReturnLibrariesFromSourceDirectory()
    {
        _mockFileHelper.Setup(fh => fh.DirectoryExists(It.IsAny<string>())).Returns(true);
        _mockFileHelper.Setup(fh => fh.EnumerateFiles(It.IsAny<string>(), SearchOption.TopDirectoryOnly, It.IsAny<string[]>())).Returns(new[] { "foo.TestAdapter.dll" });
        var extensions = _dotnetHostManager.GetTestPlatformExtensions(new[] { $".{Path.DirectorySeparatorChar}foo.dll" }, []);

        CollectionAssert.AreEqual(new[] { "foo.TestAdapter.dll" }, extensions.ToArray());
    }

    [TestMethod]
    public void GetTestPlatformExtensionsShouldReturnEmptySetIfSourceDirectoryIsEmpty()
    {
        // Parent directory is empty since the input source is file "test.dll"
        _mockFileHelper.Setup(fh => fh.DirectoryExists(It.IsAny<string>())).Returns(true);
        _mockFileHelper.Setup(fh => fh.EnumerateFiles(It.IsAny<string>(), SearchOption.TopDirectoryOnly, It.IsAny<string[]>())).Returns(new[] { "foo.dll" });
        var extensions = _dotnetHostManager.GetTestPlatformExtensions(_testSource, []);

        Assert.AreEqual(0, extensions.Count());
    }

    [TestMethod]
    public void GetTestPlatformExtensionsShouldNotAddNonCoverletDataCollectorsExtensionsIfPresent()
    {
        _mockFileHelper.Setup(fh => fh.DirectoryExists(It.IsAny<string>())).Returns(true);
        _mockFileHelper.Setup(fh => fh.EnumerateFiles(It.IsAny<string>(), SearchOption.TopDirectoryOnly, It.IsAny<string[]>())).Returns(new[] { "foo.dll" });
        var extensions = _dotnetHostManager.GetTestPlatformExtensions(_testSource, ["abc.dataollector.dll"]);

        Assert.AreEqual(0, extensions.Count());
    }

    [TestMethod]
    public async Task LaunchTestHostShouldLaunchProcessWithConnectionInfo()
    {
        var expectedArgs = "exec \"" + _defaultTestHostPath + "\""
#if NET
            + " --roll-forward Major"
#endif
            + " --port 123 --endpoint 127.0.0.1:123 --role client --parentprocessid 0";
        _dotnetHostManager.SetCustomLauncher(_mockTestHostLauncher.Object);
        await _dotnetHostManager.LaunchTestHostAsync(_defaultTestProcessStartInfo, CancellationToken.None);

        _mockTestHostLauncher.Verify(thl => thl.LaunchTestHost(It.Is<TestProcessStartInfo>(x => x.Arguments!.Equals(expectedArgs)), It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public void LaunchTestHostShouldSetExitCallBackInCaseCustomHost()
    {
#if NET5_0_OR_GREATER
        var expectedProcessId = Environment.ProcessId;
#else
        int expectedProcessId;
        using (var p = Process.GetCurrentProcess())
            expectedProcessId = p.Id;
#endif
        _mockTestHostLauncher.Setup(thl => thl.LaunchTestHost(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(expectedProcessId);
        _mockFileHelper.Setup(ph => ph.Exists("testhost.dll")).Returns(true);

        var startInfo = GetDefaultStartInfo();
        _dotnetHostManager.SetCustomLauncher(_mockTestHostLauncher.Object);
        _dotnetHostManager.LaunchTestHostAsync(startInfo, CancellationToken.None).Wait();

        _mockProcessHelper.Verify(ph => ph.SetExitCallback(expectedProcessId, It.IsAny<Action<object?>>()));
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldIncludeTestHostPathFromSourceDirectoryIfDepsFileNotFound()
    {
        // Absolute path to the source directory
        var sourcePath = Path.Combine(_temp, "test.dll");
        string expectedTestHostPath = Path.Combine(_temp, "testhost.dll");
        _mockFileHelper.Setup(ph => ph.Exists(expectedTestHostPath)).Returns(true);
        _mockFileHelper.Setup(ph => ph.Exists(Path.Combine(_temp, "test.runtimeconfig.dev.json"))).Returns(true);

        var startInfo = _dotnetHostManager.GetTestHostProcessStartInfo(new[] { sourcePath }, null, _defaultConnectionInfo);

        StringAssert.Contains(startInfo.Arguments, expectedTestHostPath);
    }

    // TODO: This assembly was previously compiled as net472 and so it was skipped and only ran as net8.0. This fails in test, but works in code that is not isolated in appdomain. Might be worth fixing because we get one null here, and another in DotnetTestHostManager.
    // Assembly.GetEntryAssembly().Location is null because of running in app domain.
#if NET
    [TestMethod]
    public void GetTestHostProcessStartInfoShouldIncludeTestHostPathNextToTestRunnerIfTesthostDllIsNoFoundAndDepsFileNotFound()
    {
        // Absolute path to the source directory
        var sourcePath = Path.Combine(_temp, "test.dll");
        string testhostNextToTestDll = Path.Combine(_temp, "testhost.dll");
        _mockFileHelper.Setup(ph => ph.Exists(testhostNextToTestDll)).Returns(false);

        var here = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
        var expectedTestHostPath = Path.Combine(here, "testhost.dll");
        _mockFileHelper.Setup(ph => ph.Exists(expectedTestHostPath)).Returns(true);

        var startInfo = _dotnetHostManager.GetTestHostProcessStartInfo(new[] { sourcePath }, null, _defaultConnectionInfo);

        StringAssert.Contains(startInfo.Arguments, expectedTestHostPath);
        var expectedAdditionalDepsPath = Path.Combine(here, "testhost.deps.json");
        StringAssert.Contains(startInfo.Arguments, $"--additional-deps \"{expectedAdditionalDepsPath}\"");
        var expectedAdditionalProbingPath = here;
        StringAssert.Contains(startInfo.Arguments, $"--additionalprobingpath \"{expectedAdditionalProbingPath}\"");
        var expectedRuntimeConfigPath = Path.Combine(here, "testhost-latest.runtimeconfig.json");
        StringAssert.Contains(startInfo.Arguments, $"--runtimeconfig \"{expectedRuntimeConfigPath}\"");
    }

#endif

    // TODO: This assembly was previously compiled as net472 and so it was skipped and only ran as net8.0. This fails in test, but works in code that is not isolated in appdomain. Might be worth fixing because we get one null here, and another in DotnetTestHostManager.
    // Assembly.GetEntryAssembly().Location is null because of running in app domain.
#if NET

    [TestMethod]

    // we can't put in a "default" value, and we don't have other way to determine if this provided value is the
    // runtime default or the actual value that user provided, so right now the default will use the latest, instead
    // or the more correct 1.0, it should be okay, as that version is not supported anymore anyway
    [DataRow("net8.0", "8.0", true)]

    // net9.0 is currently the latest released version, but it still has it's own runtime config, it is not the same as
    // "latest" which means the latest you have on system. So if you have only 5.0 SDK then net8.0 will fail because it can't find net8.0,
    // but latest would use net9.0 because that is the latest one on your system.
    [DataRow("net9.0", "9.0", true)]
    [DataRow("net9.0", "latest", false)]
    public void GetTestHostProcessStartInfoShouldIncludeTestHostPathNextToTestRunnerIfTesthostDllIsNoFoundAndDepsFileNotFoundWithTheCorrectTfm(string tfm, string suffix, bool runtimeConfigExists)
    {
        // Absolute path to the source directory
        var sourcePath = Path.Combine(_temp, "test.dll");
        string testhostNextToTestDll = Path.Combine(_temp, "testhost.dll");
        _mockFileHelper.Setup(ph => ph.Exists(testhostNextToTestDll)).Returns(false);

        var here = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
        var testhostNextToRunner = Path.Combine(here, "testhost.dll");
        _mockFileHelper.Setup(ph => ph.Exists(testhostNextToRunner)).Returns(true);

        _mockFileHelper.Setup(ph => ph.Exists(It.Is<string>(s => s.Contains($"{suffix}.runtimeconfig.json")))).Returns(runtimeConfigExists);

        _dotnetHostManager.Initialize(_mockMessageLogger.Object, $"<RunSettings><RunConfiguration><TargetFrameworkVersion>{tfm}</TargetFrameworkVersion></RunConfiguration></RunSettings>");
        var startInfo = _dotnetHostManager.GetTestHostProcessStartInfo(new[] { sourcePath }, null, _defaultConnectionInfo);

        var expectedRuntimeConfigPath = Path.Combine(here, $"testhost-{suffix}.runtimeconfig.json");
        StringAssert.Contains(startInfo.Arguments, $"--runtimeconfig \"{expectedRuntimeConfigPath}\"");
    }

#endif

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldIncludeTestHostPathFromSourceDirectoryIfRunConfigDevFileNotFound()
    {
        // Absolute path to the source directory
        var sourcePath = Path.Combine(_temp, "test.dll");
        string expectedTestHostPath = Path.Combine(_temp, "testhost.dll");
        _mockFileHelper.Setup(ph => ph.Exists(expectedTestHostPath)).Returns(true);
        _mockFileHelper.Setup(ph => ph.Exists(Path.Combine(_temp, "test.deps.json"))).Returns(true);

        var startInfo = _dotnetHostManager.GetTestHostProcessStartInfo(new[] { sourcePath }, null, _defaultConnectionInfo);

        Assert.IsTrue(startInfo.Arguments!.Contains(expectedTestHostPath));
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldIncludeTestHostPathFromDepsFile()
    {
        // Absolute path to the source directory
        var sourcePath = Path.Combine(_temp, "test.dll");

        string runtimeConfigFileContent =
            @"{
    ""runtimeOptions"": {
        ""additionalProbingPaths"": [
            ""C:\\packages""
            ]
        }
}";

        string depsFileContent =
            @"{
    ""runtimeTarget"": {
        ""name"": "".NETCoreApp,Version=v1.0"",
        ""signature"": ""8f25843f8e35a3e80ef4ae98b95117ea5c468b3f""
    },
    ""compilationOptions"": {},
    ""targets"": {
        "".NETCoreApp,Version=v1.0"": {
            ""microsoft.testplatform.testhost/15.0.0-Dev"": {
                ""dependencies"": {
                    ""Microsoft.TestPlatform.ObjectModel"": ""15.0.0-Dev"",
                    ""Newtonsoft.Json"": ""13.0.3""
                },
                ""runtime"": {
                    ""lib/netstandard1.5/Microsoft.TestPlatform.CommunicationUtilities.dll"": { },
                    ""lib/netstandard1.5/Microsoft.TestPlatform.CrossPlatEngine.dll"": { },
                    ""lib/netstandard1.5/Microsoft.VisualStudio.TestPlatform.Common.dll"": { },
                    ""lib/netstandard1.5/testhost.dll"": { }
                }
            }
        }
    },
    ""libraries"": {
        ""microsoft.testplatform.testhost/15.0.0-Dev"": {
        ""type"": ""package"",
        ""serviceable"": true,
        ""sha512"": ""sha512-enO8sZmjbhXOfiZ6hV2ncaknaHnQbrGVsHUJzzu2Dmoh4fHFro4BF1Y4+sb4LOQhu4b3DFYPRj1ncd1RQK6HmQ=="",
        ""path"": ""microsoft.testplatform.testhost/15.0.0-Dev"",
        ""hashPath"": ""microsoft.testplatform.testhost.15.0.0-Dev""
        }
    }
}";

        MemoryStream runtimeConfigStream = new(Encoding.UTF8.GetBytes(runtimeConfigFileContent));
        _mockFileHelper.Setup(ph => ph.GetStream(Path.Combine(_temp, "test.runtimeconfig.dev.json"), FileMode.Open, FileAccess.Read)).Returns(runtimeConfigStream);
        _mockFileHelper.Setup(ph => ph.Exists(Path.Combine(_temp, "test.runtimeconfig.dev.json"))).Returns(true);

        MemoryStream depsFileStream = new(Encoding.UTF8.GetBytes(depsFileContent));
        _mockFileHelper.Setup(ph => ph.GetStream(Path.Combine(_temp, "test.deps.json"), FileMode.Open, FileAccess.Read)).Returns(depsFileStream);
        _mockFileHelper.Setup(ph => ph.Exists(Path.Combine(_temp, "test.deps.json"))).Returns(true);

        string testHostFullPath = $@"C:\packages{Path.DirectorySeparatorChar}microsoft.testplatform.testhost/15.0.0-Dev{Path.DirectorySeparatorChar}lib/netstandard1.5/testhost.dll";
        _mockFileHelper.Setup(ph => ph.Exists(testHostFullPath)).Returns(true);

        var startInfo = _dotnetHostManager.GetTestHostProcessStartInfo(new[] { sourcePath }, null, _defaultConnectionInfo);

        Assert.IsTrue(startInfo.Arguments!.Contains(testHostFullPath));
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldIncludeTestHostPathFromSourceDirectoryIfNugetpathDoesntExist()
    {
        // Absolute path to the source directory
        var sourcePath = Path.Combine(_temp, "test.dll");

        string runtimeConfigFileContent =
            @"{
    ""runtimeOptions"": {
        ""additionalProbingPaths"": [
            ""C:\\packages""
            ]
        }
}";

        string depsFileContent =
            @"{
    ""runtimeTarget"": {
        ""name"": "".NETCoreApp,Version=v1.0"",
        ""signature"": ""8f25843f8e35a3e80ef4ae98b95117ea5c468b3f""
    },
    ""compilationOptions"": {},
    ""targets"": {
        "".NETCoreApp,Version=v1.0"": {
            ""microsoft.testplatform.testhost/15.0.0-Dev"": {
                ""dependencies"": {
                    ""Microsoft.TestPlatform.ObjectModel"": ""15.0.0-Dev"",
                    ""Newtonsoft.Json"": ""13.0.3""
                },
                ""runtime"": {
                    ""lib/netstandard1.5/Microsoft.TestPlatform.CommunicationUtilities.dll"": { },
                    ""lib/netstandard1.5/Microsoft.TestPlatform.CrossPlatEngine.dll"": { },
                    ""lib/netstandard1.5/Microsoft.VisualStudio.TestPlatform.Common.dll"": { },
                    ""lib/netstandard1.5/testhost.dll"": { }
                }
            }
        }
    },
    ""libraries"": {
        ""microsoft.testplatform.testhost/15.0.0-Dev"": {
        ""type"": ""package"",
        ""serviceable"": true,
        ""sha512"": ""sha512-enO8sZmjbhXOfiZ6hV2ncaknaHnQbrGVsHUJzzu2Dmoh4fHFro4BF1Y4+sb4LOQhu4b3DFYPRj1ncd1RQK6HmQ=="",
        ""path"": ""microsoft.testplatform.testhost/15.0.0-Dev"",
        ""hashPath"": ""microsoft.testplatform.testhost.15.0.0-Dev""
        }
    }
}";

        MemoryStream runtimeConfigStream = new(Encoding.UTF8.GetBytes(runtimeConfigFileContent));
        _mockFileHelper.Setup(ph => ph.GetStream(Path.Combine(_temp, "test.runtimeconfig.dev.json"), FileMode.Open, FileAccess.Read)).Returns(runtimeConfigStream);
        _mockFileHelper.Setup(ph => ph.Exists(Path.Combine(_temp, "test.runtimeconfig.dev.json"))).Returns(true);

        MemoryStream depsFileStream = new(Encoding.UTF8.GetBytes(depsFileContent));
        _mockFileHelper.Setup(ph => ph.GetStream(Path.Combine(_temp, "test.deps.json"), FileMode.Open, FileAccess.Read)).Returns(depsFileStream);
        _mockFileHelper.Setup(ph => ph.Exists(Path.Combine(_temp, "test.deps.json"))).Returns(true);

        string testHostFullPath = $@"C:\packages{Path.DirectorySeparatorChar}microsoft.testplatform.testhost/15.0.0-Dev{Path.DirectorySeparatorChar}lib/netstandard1.5/testhost.dll";
        _mockFileHelper.Setup(ph => ph.Exists(testHostFullPath)).Returns(false);

        string testHostPath = Path.Combine(_temp, "testhost.dll");

        var startInfo = _dotnetHostManager.GetTestHostProcessStartInfo(new[] { sourcePath }, null, _defaultConnectionInfo);

        Assert.IsTrue(startInfo.Arguments!.Contains(testHostPath));
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldSkipInvalidAdditionalProbingPaths()
    {
        // Absolute path to the source directory
        var sourcePath = Path.Combine(_temp, "test.dll");

        string runtimeConfigFileContent =
            @"{
    ""runtimeOptions"": {
        ""additionalProbingPaths"": [
            ""C:\\Users\\bob\\.dotnet\\store\\|arch|\\|tfm|"",
            ""C:\\packages""
            ]
        }
}";

        string depsFileContent =
            @"{
    ""runtimeTarget"": {
        ""name"": "".NETCoreApp,Version=v1.0"",
        ""signature"": ""8f25843f8e35a3e80ef4ae98b95117ea5c468b3f""
    },
    ""compilationOptions"": {},
    ""targets"": {
        "".NETCoreApp,Version=v1.0"": {
            ""microsoft.testplatform.testhost/15.0.0-Dev"": {
                ""dependencies"": {
                    ""Microsoft.TestPlatform.ObjectModel"": ""15.0.0-Dev"",
                    ""Newtonsoft.Json"": ""13.0.3""
                },
                ""runtime"": {
                    ""lib/netstandard1.5/Microsoft.TestPlatform.CommunicationUtilities.dll"": { },
                    ""lib/netstandard1.5/Microsoft.TestPlatform.CrossPlatEngine.dll"": { },
                    ""lib/netstandard1.5/Microsoft.VisualStudio.TestPlatform.Common.dll"": { },
                    ""lib/netstandard1.5/testhost.dll"": { }
                }
            }
        }
    },
    ""libraries"": {
        ""microsoft.testplatform.testhost/15.0.0-Dev"": {
        ""type"": ""package"",
        ""serviceable"": true,
        ""sha512"": ""sha512-enO8sZmjbhXOfiZ6hV2ncaknaHnQbrGVsHUJzzu2Dmoh4fHFro4BF1Y4+sb4LOQhu4b3DFYPRj1ncd1RQK6HmQ=="",
        ""path"": ""microsoft.testplatform.testhost/15.0.0-Dev"",
        ""hashPath"": ""microsoft.testplatform.testhost.15.0.0-Dev""
        }
    }
}";

        MemoryStream runtimeConfigStream = new(Encoding.UTF8.GetBytes(runtimeConfigFileContent));
        _mockFileHelper.Setup(ph => ph.GetStream(Path.Combine(_temp, "test.runtimeconfig.dev.json"), FileMode.Open, FileAccess.Read)).Returns(runtimeConfigStream);
        _mockFileHelper.Setup(ph => ph.Exists(Path.Combine(_temp, "test.runtimeconfig.dev.json"))).Returns(true);

        MemoryStream depsFileStream = new(Encoding.UTF8.GetBytes(depsFileContent));
        _mockFileHelper.Setup(ph => ph.GetStream(Path.Combine(_temp, "test.deps.json"), FileMode.Open, FileAccess.Read)).Returns(depsFileStream);
        _mockFileHelper.Setup(ph => ph.Exists(Path.Combine(_temp, "test.deps.json"))).Returns(true);

        string testHostFullPath = $@"C:\packages{Path.DirectorySeparatorChar}microsoft.testplatform.testhost/15.0.0-Dev{Path.DirectorySeparatorChar}lib/netstandard1.5/testhost.dll";
        _mockFileHelper.Setup(ph => ph.Exists(testHostFullPath)).Returns(true);

        var startInfo = _dotnetHostManager.GetTestHostProcessStartInfo(new[] { sourcePath }, null, _defaultConnectionInfo);

        Assert.IsTrue(startInfo.Arguments!.Contains(testHostFullPath));
    }

    [TestMethod]
    [DataRow("DOTNET_ROOT(x86)", "x86")]
    [DataRow("DOTNET_ROOT", "x64")]
    [DataRow("DOTNET_ROOT_WRONG", "")]
    [TestCategory("Windows")]
    public void GetTestHostProcessStartInfoShouldForwardDOTNET_ROOTEnvVarsForAppHost(string envVar, string expectedValue)
    {
        _mockFileHelper.Setup(ph => ph.Exists("testhost.exe")).Returns(true);
        _mockEnvironment.Setup(ev => ev.OperatingSystem).Returns(PlatformOperatingSystem.Windows);
        _mockEnvironmentVariable.Reset();
        _mockEnvironmentVariable.Setup(x => x.GetEnvironmentVariable($"VSTEST_WINAPPHOST_{envVar}")).Returns(expectedValue);

        var startInfo = _dotnetHostManager.GetTestHostProcessStartInfo(_testSource, null, _defaultConnectionInfo);
        if (!string.IsNullOrEmpty(expectedValue))
        {
            Assert.AreEqual(1, startInfo.EnvironmentVariables!.Count);
            Assert.IsNotNull(startInfo.EnvironmentVariables[envVar]);
            Assert.AreEqual(startInfo.EnvironmentVariables[envVar], expectedValue);
        }
        else
        {
            Assert.AreEqual(0, startInfo.EnvironmentVariables!.Count);
        }
    }

    [TestMethod]
    public async Task DotNetCoreErrorMessageShouldBeReadAsynchronouslyAsync()
    {
        var errorData = "Custom Error Strings";
        ErrorCallBackTestHelper(errorData, -1);

        await _dotnetHostManager.LaunchTestHostAsync(_defaultTestProcessStartInfo, CancellationToken.None);

        Assert.AreEqual(errorData, _errorMessage);
    }

    [TestMethod]
    public async Task DotNetCoreNoErrorMessageIfExitCodeZero()
    {
        string errorData = string.Empty;
        ErrorCallBackTestHelper(errorData, 0);

        await _dotnetHostManager.LaunchTestHostAsync(_defaultTestProcessStartInfo, CancellationToken.None);

        Assert.IsNull(_errorMessage);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public async Task DotNetCoreErrorReceivedCallbackShouldNotLogNullOrEmptyData(string errorData)
    {
        ErrorCallBackTestHelper(errorData, -1);

        await _dotnetHostManager.LaunchTestHostAsync(_defaultTestProcessStartInfo, CancellationToken.None);

        Assert.AreEqual(_errorMessage, string.Empty);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public async Task DotNetCoreProcessExitedButNoErrorMessageIfNoDataWritten(int exitCode)
    {
        var errorData = string.Empty;
        ExitCallBackTestHelper(exitCode);

        // override event listener
        _dotnetHostManager.HostExited += DotnetHostManagerExitCodeTesterHostExited;

        await _dotnetHostManager.LaunchTestHostAsync(_defaultTestProcessStartInfo, CancellationToken.None);

        Assert.AreEqual(_errorMessage, string.Empty);
        Assert.AreEqual(_exitCode, exitCode);
    }

    [TestMethod]
    public async Task CleanTestHostAsyncShouldKillTestHostProcess()
    {
#if NET5_0_OR_GREATER
        var pid = Environment.ProcessId;
#else
        int pid;
        using (var p = Process.GetCurrentProcess())
            pid = p.Id;
#endif
        bool isVerified = false;
        _mockProcessHelper.Setup(ph => ph.TerminateProcess(It.IsAny<Process>()))
            .Callback<object>(p => isVerified = ((Process)p).Id == pid);

        ExitCallBackTestHelper(0);
        await _dotnetHostManager.LaunchTestHostAsync(_defaultTestProcessStartInfo, CancellationToken.None);

        await _dotnetHostManager.CleanTestHostAsync(CancellationToken.None);

        Assert.IsTrue(isVerified);
    }

    [TestMethod]
    public async Task CleanTestHostAsyncShouldNotThrowIfTestHostIsNotStarted()
    {
#if NET5_0_OR_GREATER
        var pid = Environment.ProcessId;
#else
        int pid;
        using (var p = Process.GetCurrentProcess())
            pid = p.Id;
#endif
        bool isVerified = false;
        _mockProcessHelper.Setup(ph => ph.TerminateProcess(It.IsAny<Process>())).Callback<object>(p => isVerified = ((Process)p).Id == pid).Throws<Exception>();

        ExitCallBackTestHelper(0);
        await _dotnetHostManager.LaunchTestHostAsync(_defaultTestProcessStartInfo, CancellationToken.None);

        await _dotnetHostManager.CleanTestHostAsync(CancellationToken.None);

        Assert.IsTrue(isVerified);
    }

    private void DotnetHostManagerExitCodeTesterHostExited(object? sender, HostProviderEventArgs e)
    {
        _errorMessage = e.Data.TrimEnd(Environment.NewLine.ToCharArray());
        _exitCode = e.ErrroCode;
    }

    private void DotnetHostManagerHostExited(object? sender, HostProviderEventArgs e)
    {
        if (e.ErrroCode != 0)
        {
            _errorMessage = e.Data.TrimEnd(Environment.NewLine.ToCharArray());
        }
    }

    private void DotnetHostManagerHostLaunched(object? sender, HostProviderEventArgs e)
    {
        _testHostId = e.ProcessId;
    }

    private void ErrorCallBackTestHelper(string errorMessage, int exitCode)
    {
        _mockProcessHelper.Setup(
                ph =>
                    ph.LaunchProcess(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<IDictionary<string, string?>>(),
                        It.IsAny<Action<object?, string?>>(),
                        It.IsAny<Action<object?>>(),
                        It.IsAny<Action<object?, string?>>()))
            .Callback<string, string, string, IDictionary<string, string>, Action<object, string>, Action<object>, Action<object, string>>(
                (var1, var2, var3, dictionary, errorCallback, exitCallback, outputCallback) =>
                {
                    var process = Process.GetCurrentProcess();

                    errorCallback(process, errorMessage);
                    exitCallback(process);
                }).Returns(Process.GetCurrentProcess());

        _mockProcessHelper.Setup(ph => ph.TryGetExitCode(It.IsAny<object>(), out exitCode)).Returns(true);
    }

    private void ExitCallBackTestHelper(int exitCode)
    {
        _mockProcessHelper.Setup(
                ph =>
                    ph.LaunchProcess(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<IDictionary<string, string?>>(),
                        It.IsAny<Action<object?, string?>>(),
                        It.IsAny<Action<object?>>(),
                        It.IsAny<Action<object?, string?>>()))
            .Callback<string, string, string, IDictionary<string, string>, Action<object, string>, Action<object>, Action<object, string>>(
                (var1, var2, var3, dictionary, errorCallback, exitCallback, outputCallback) =>
                {
                    var process = Process.GetCurrentProcess();
                    exitCallback(process);
                }).Returns(Process.GetCurrentProcess());

        _mockProcessHelper.Setup(ph => ph.TryGetExitCode(It.IsAny<object>(), out exitCode)).Returns(true);
    }

    private TestProcessStartInfo GetDefaultStartInfo()
    {
        var startInfo = _dotnetHostManager.GetTestHostProcessStartInfo(
            _testSource,
            null,
            _defaultConnectionInfo);
        return startInfo;
    }

    internal class TestableDotnetTestHostManager : DotnetTestHostManager
    {
        public TestableDotnetTestHostManager(
            IProcessHelper processHelper,
            IFileHelper fileHelper,
            IDotnetHostHelper dotnetTestHostHelper,
            IEnvironment environment,
            IRunSettingsHelper runsettingsHelper,
            IWindowsRegistryHelper windowsRegistryHelper,
            IEnvironmentVariableHelper environmentVariableHelper)
            : base(processHelper, fileHelper, dotnetTestHostHelper, environment, runsettingsHelper, windowsRegistryHelper, environmentVariableHelper)
        {
        }
    }
}
