// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Hosting;
using Microsoft.VisualStudio.TestPlatform.DesktopTestHostRuntimeProvider;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.TestHostProvider.Hosting.UnitTests;

[TestClass]
public class DefaultTestHostManagerTests
{
    private readonly TestProcessStartInfo _startInfo;
    private readonly Mock<IMessageLogger> _mockMessageLogger;
    private readonly Mock<IProcessHelper> _mockProcessHelper;
    private readonly Mock<IFileHelper> _mockFileHelper;
    private readonly Mock<IDotnetHostHelper> _mockDotnetHostHelper;
    private readonly Mock<IEnvironment> _mockEnvironment;
    private readonly Mock<IEnvironmentVariableHelper> _mockEnvironmentVariable;
    private readonly DefaultTestHostManager _testHostManager;

    private TestableTestHostManager? _testableTestHostManager;
    private string? _errorMessage;
    private int _exitCode;
    private int _testHostId;

    public DefaultTestHostManagerTests()
    {
        _mockProcessHelper = new Mock<IProcessHelper>();
        _mockFileHelper = new Mock<IFileHelper>();
        _mockProcessHelper.Setup(ph => ph.GetCurrentProcessFileName()).Returns("vstest.console.exe");
        _mockDotnetHostHelper = new Mock<IDotnetHostHelper>();
        _mockEnvironment = new Mock<IEnvironment>();
        _mockEnvironmentVariable = new Mock<IEnvironmentVariableHelper>();

        _mockMessageLogger = new Mock<IMessageLogger>();

        _testHostManager = new DefaultTestHostManager(_mockProcessHelper.Object, _mockFileHelper.Object, _mockDotnetHostHelper.Object, _mockEnvironment.Object, _mockEnvironmentVariable.Object);
        _testHostManager.Initialize(_mockMessageLogger.Object, $"<?xml version=\"1.0\" encoding=\"utf-8\"?><RunSettings> <RunConfiguration> <TargetPlatform>{Architecture.X64}</TargetPlatform> <TargetFrameworkVersion>{Framework.DefaultFramework}</TargetFrameworkVersion> <DisableAppDomain>{false}</DisableAppDomain> </RunConfiguration> </RunSettings>");
        _startInfo = _testHostManager.GetTestHostProcessStartInfo(Enumerable.Empty<string>(), null, default);
    }

    [TestMethod]
    public void ConstructorShouldSetX86ProcessForX86Architecture()
    {
        _testHostManager.Initialize(_mockMessageLogger.Object, $"<?xml version=\"1.0\" encoding=\"utf-8\"?><RunSettings> <RunConfiguration> <TargetPlatform>{Architecture.X86}</TargetPlatform> <TargetFrameworkVersion>{Framework.DefaultFramework}</TargetFrameworkVersion> <DisableAppDomain>{false}</DisableAppDomain> </RunConfiguration> </RunSettings>");

        var info = _testHostManager.GetTestHostProcessStartInfo(Enumerable.Empty<string>(), null, default);

        StringAssert.EndsWith(info.FileName, "testhost.x86.exe");
    }

    [TestMethod]
    public void ConstructorShouldSetX64ProcessForX64Architecture()
    {
        StringAssert.EndsWith(_startInfo.FileName, "testhost.exe");
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldIncludeFileNameFromSubFolderTestHostWhenCurrentProcessIsDotnet()
    {
        _mockProcessHelper.Setup(ph => ph.GetCurrentProcessFileName()).Returns("dotnet.exe");
        _mockFileHelper.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
        var startInfo = _testHostManager.GetTestHostProcessStartInfo(Enumerable.Empty<string>(), null, default);

        Assert.IsTrue(startInfo.FileName!.EndsWith(Path.Combine("TestHostNetFramework", "testhost.exe")));
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldNotIncludeFileNameFromSubFolderTestHostWhenCurrentProcessIsDotnet()
    {
        _mockProcessHelper.Setup(ph => ph.GetCurrentProcessFileName()).Returns("dotnet.exe");
        _mockFileHelper.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
        var startInfo = _testHostManager.GetTestHostProcessStartInfo(Enumerable.Empty<string>(), null, default);

        Assert.IsFalse(startInfo.FileName!.EndsWith(Path.Combine("TestHost", "testhost.exe")));
        Assert.IsTrue(startInfo.FileName!.EndsWith("testhost.exe"));
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldNotIncludeFileNameFromSubFolderTestHostWhenCurrentProcessIsIde()
    {
        _mockProcessHelper.Setup(ph => ph.GetCurrentProcessFileName()).Returns("devenv.exe");
        var startInfo = _testHostManager.GetTestHostProcessStartInfo(Enumerable.Empty<string>(), null, default);

        Assert.IsFalse(startInfo.FileName!.EndsWith(Path.Combine("TestHost", "testhost.exe")));
        Assert.IsTrue(startInfo.FileName!.EndsWith("testhost.exe"));
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldIncludeConnectionInfo()
    {
        var connectionInfo = new TestRunnerConnectionInfo { Port = 123, ConnectionInfo = new TestHostConnectionInfo { Endpoint = "127.0.0.0:123", Role = ConnectionRole.Client, Transport = Transport.Sockets }, RunnerProcessId = 101 };
        var info = _testHostManager.GetTestHostProcessStartInfo(
            Enumerable.Empty<string>(),
            null,
            connectionInfo);

        Assert.AreEqual(" --port 123 --endpoint 127.0.0.0:123 --role client --parentprocessid 101", info.Arguments);
    }

    [TestMethod]
    public void GetTestHostConnectionInfoShouldIncludeEndpointRoleAndChannelType()
    {
        var connectionInfo = new TestHostConnectionInfo
        {
            Endpoint = "127.0.0.1:0",
            Role = ConnectionRole.Client,
            Transport = Transport.Sockets
        };

        var info = _testHostManager.GetTestHostConnectionInfo();

        Assert.AreEqual(connectionInfo, info);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldIncludeEmptyEnvironmentVariables()
    {
        Assert.AreEqual(0, _startInfo.EnvironmentVariables!.Count);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldIncludeEnvironmentVariables()
    {
        var environmentVariables = new Dictionary<string, string?> { { "k1", "v1" } };

        var info = _testHostManager.GetTestHostProcessStartInfo(Enumerable.Empty<string>(), environmentVariables, default);

        Assert.AreEqual(environmentVariables, info.EnvironmentVariables);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldIncludeCurrentWorkingDirectory()
    {
        Assert.AreEqual(Directory.GetCurrentDirectory(), _startInfo.WorkingDirectory);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldIncludeTestSourcePathInArgumentsIfNonShared()
    {
        _testHostManager.Initialize(_mockMessageLogger.Object, $"<?xml version=\"1.0\" encoding=\"utf-8\"?><RunSettings> <RunConfiguration> <TargetPlatform>{Architecture.X86}</TargetPlatform> <TargetFrameworkVersion>{Framework.DefaultFramework}</TargetFrameworkVersion> <DisableAppDomain>{true}</DisableAppDomain> </RunConfiguration> </RunSettings>");
        var connectionInfo = new TestRunnerConnectionInfo { Port = 123, ConnectionInfo = new TestHostConnectionInfo { Endpoint = "127.0.0.0:123", Role = ConnectionRole.Client, Transport = Transport.Sockets }, RunnerProcessId = 101 };
        var source = @"C:\temp\a.dll";

        var info = _testHostManager.GetTestHostProcessStartInfo(
            new List<string>() { source },
            null,
            connectionInfo);

        Assert.AreEqual(" --port 123 --endpoint 127.0.0.0:123 --role client --parentprocessid 101 --testsourcepath " + source.AddDoubleQuote(), info.Arguments);
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldUseMonoAsHostOnNonWindowsIfNotStartedWithMono()
    {
        _mockProcessHelper.Setup(p => p.GetCurrentProcessFileName()).Returns("/usr/bin/dotnet");
        _mockEnvironment.Setup(e => e.OperatingSystem).Returns(PlatformOperatingSystem.Unix);
        _mockDotnetHostHelper.Setup(d => d.GetMonoPath()).Returns("/usr/bin/mono");
        var source = @"C:\temp\a.dll";

        var info = _testHostManager.GetTestHostProcessStartInfo(
            new List<string>() { source },
            null,
            default);

        Assert.AreEqual("/usr/bin/mono", info.FileName);
        StringAssert.Contains(info.Arguments, Path.Combine("TestHostNetFramework", "testhost.exe"));
    }

    [TestMethod]
    public void GetTestHostProcessStartInfoShouldNotUseMonoAsHostOnNonWindowsIfStartedWithMono()
    {
        _mockProcessHelper.Setup(p => p.GetCurrentProcessFileName()).Returns("/usr/bin/mono");
        _mockEnvironment.Setup(e => e.OperatingSystem).Returns(PlatformOperatingSystem.Unix);
        _mockDotnetHostHelper.Setup(d => d.GetMonoPath()).Returns("/usr/bin/mono");
        var source = @"C:\temp\a.dll";

        var info = _testHostManager.GetTestHostProcessStartInfo(
            new List<string>() { source },
            null,
            default);

        var testHostPath = Path.Combine("TestHostNetFramework", "testhost.exe");
        StringAssert.EndsWith(info.FileName, testHostPath);
        Assert.IsFalse(info.Arguments!.Contains(testHostPath));
    }

    [TestMethod]
    public void GetTestPlatformExtensionsShouldReturnExtensionsListAsIsIfSourcesListIsEmpty()
    {
        _testHostManager.Initialize(_mockMessageLogger.Object, $"<?xml version=\"1.0\" encoding=\"utf-8\"?><RunSettings> <RunConfiguration></RunConfiguration> </RunSettings>");
        List<string> currentList = new() { @"FooExtension.dll" };

        // Act
        var resultExtensions = _testHostManager.GetTestPlatformExtensions(new List<string>(), currentList).ToList();

        // Verify
        CollectionAssert.AreEqual(currentList, resultExtensions);
    }

    [TestMethod]
    public void GetTestPlatformExtensionsShouldReturnExtensionsListAsIsIfSourcesListIsNull()
    {
        _testHostManager.Initialize(_mockMessageLogger.Object, $"<?xml version=\"1.0\" encoding=\"utf-8\"?><RunSettings> <RunConfiguration></RunConfiguration> </RunSettings>");
        List<string> currentList = new() { @"FooExtension.dll" };

        // Act
        var resultExtensions = _testHostManager.GetTestPlatformExtensions(null, currentList).ToList();

        // Verify
        CollectionAssert.AreEqual(currentList, resultExtensions);
    }

    [TestMethod]
    public void GetTestPlatformExtensionsShouldNotExcludeOutputDirectoryExtensionsIfTestAdapterPathIsSet()
    {
        List<string> sourcesDir = new() { @"C:\Source1" };
        List<string> sources = new() { @"C:\Source1\source1.dll" };

        List<string> extensionsList1 = new() { @"C:\Source1\ext1.TestAdapter.dll", @"C:\Source1\ext2.TestAdapter.dll" };
        _mockFileHelper.Setup(fh => fh.EnumerateFiles(sourcesDir[0], SearchOption.TopDirectoryOnly, "TestAdapter.dll")).Returns(extensionsList1);

        _mockFileHelper.Setup(fh => fh.GetFileVersion(extensionsList1[0])).Returns(new Version(2, 0));
        _mockFileHelper.Setup(fh => fh.GetFileVersion(extensionsList1[1])).Returns(new Version(5, 5));

        _testHostManager.Initialize(_mockMessageLogger.Object, $"<?xml version=\"1.0\" encoding=\"utf-8\"?><RunSettings> <RunConfiguration><TestAdaptersPaths>C:\\Foo</TestAdaptersPaths></RunConfiguration> </RunSettings>");
        List<string> currentList = new() { @"FooExtension.dll", @"C:\Source1\ext1.TestAdapter.dll", @"C:\Source1\ext2.TestAdapter.dll" };

        // Act
        var resultExtensions = _testHostManager.GetTestPlatformExtensions(sources, currentList).ToList();

        // Verify
        CollectionAssert.AreEqual(currentList, resultExtensions);
    }

    [TestMethod]
    [TestCategory("Windows")]
    public void GetTestPlatformExtensionsShouldIncludeOutputDirectoryExtensionsIfTestAdapterPathIsNotSet()
    {
        List<string> sourcesDir = new() { "C:\\Source1", "C:\\Source2" };
        List<string> sources = new() { @"C:\Source1\source1.dll", @"C:\Source2\source2.dll" };

        List<string> extensionsList1 = new() { @"C:\Source1\ext1.TestAdapter.dll", @"C:\Source1\ext2.TestAdapter.dll" };
        List<string> extensionsList2 = new() { @"C:\Source2\ext1.TestAdapter.dll", @"C:\Source2\ext2.TestAdapter.dll" };

        _mockFileHelper.Setup(fh => fh.GetFileVersion(extensionsList1[0])).Returns(new Version(2, 0));
        _mockFileHelper.Setup(fh => fh.GetFileVersion(extensionsList1[1])).Returns(new Version(5, 5));
        _mockFileHelper.Setup(fh => fh.GetFileVersion(extensionsList2[0])).Returns(new Version(2, 2));
        _mockFileHelper.Setup(fh => fh.GetFileVersion(extensionsList2[1])).Returns(new Version(5, 0));

        _mockFileHelper.Setup(fh => fh.EnumerateFiles(sourcesDir[0], SearchOption.TopDirectoryOnly, "TestAdapter.dll")).Returns(extensionsList1);
        _mockFileHelper.Setup(fh => fh.EnumerateFiles(sourcesDir[1], SearchOption.TopDirectoryOnly, "TestAdapter.dll")).Returns(extensionsList2);

        _testHostManager.Initialize(_mockMessageLogger.Object, $"<?xml version=\"1.0\" encoding=\"utf-8\"?><RunSettings> <RunConfiguration><TargetFrameworkVersion>{Framework.DefaultFramework}</TargetFrameworkVersion></RunConfiguration> </RunSettings>");

        // Act
        var resultExtensions = _testHostManager.GetTestPlatformExtensions(sources, new List<string>()).ToList();

        // Verify
        List<string> expectedList = new() { @"C:\Source2\ext1.TestAdapter.dll", @"C:\Source1\ext2.TestAdapter.dll" };
        CollectionAssert.AreEqual(expectedList, resultExtensions);
        _mockMessageLogger.Verify(ml => ml.SendMessage(TestMessageLevel.Warning, "Multiple versions of same extension found. Selecting the highest version." + Environment.NewLine + "  ext1.TestAdapter : 2.2\n  ext2.TestAdapter : 5.5"), Times.Once);
    }

    [TestMethod]
    [TestCategory("Windows")]
    public void GetTestPlatformExtensionsShouldReturnPathTheHigherVersionedFileExtensions()
    {
        List<string> sourcesDir = new() { "C:\\Source1", "C:\\Source2" };
        List<string> sources = new() { @"C:\Source1\source1.dll", @"C:\Source2\source2.dll" };

        List<string> extensionsList1 = new() { @"C:\Source1\ext1.TestAdapter.dll" };
        List<string> extensionsList2 = new() { @"C:\Source2\ext1.TestAdapter.dll" };

        _mockFileHelper.Setup(fh => fh.GetFileVersion(extensionsList1[0])).Returns(new Version(2, 0));
        _mockFileHelper.Setup(fh => fh.GetFileVersion(extensionsList2[0])).Returns(new Version(2, 2));

        _mockFileHelper.Setup(fh => fh.EnumerateFiles(sourcesDir[0], SearchOption.TopDirectoryOnly, "TestAdapter.dll")).Returns(extensionsList1);
        _mockFileHelper.Setup(fh => fh.EnumerateFiles(sourcesDir[1], SearchOption.TopDirectoryOnly, "TestAdapter.dll")).Returns(extensionsList2);

        _testHostManager.Initialize(_mockMessageLogger.Object, $"<?xml version=\"1.0\" encoding=\"utf-8\"?><RunSettings> <RunConfiguration><TargetFrameworkVersion>{Framework.DefaultFramework}</TargetFrameworkVersion></RunConfiguration> </RunSettings>");

        // Act
        var resultExtensions = _testHostManager.GetTestPlatformExtensions(sources, new List<string>()).ToList();

        // Verify
        CollectionAssert.AreEqual(extensionsList2, resultExtensions);
        _mockMessageLogger.Verify(ml => ml.SendMessage(TestMessageLevel.Warning, "Multiple versions of same extension found. Selecting the highest version." + Environment.NewLine + "  ext1.TestAdapter : 2.2"), Times.Once);
    }

    [TestMethod]
    [TestCategory("Windows")]
    public void GetTestPlatformExtensionsShouldReturnPathToSingleFileExtensionOfATypeIfVersionsAreSame()
    {
        List<string> sourcesDir = new() { "C:\\Source1", "C:\\Source2" };
        List<string> sources = new() { @"C:\Source1\source1.dll", @"C:\Source2\source2.dll" };

        List<string> extensionsList1 = new() { @"C:\Source1\ext1.TestAdapter.dll" };
        List<string> extensionsList2 = new() { @"C:\Source2\ext1.TestAdapter.dll" };

        _mockFileHelper.Setup(fh => fh.GetFileVersion(extensionsList1[0])).Returns(new Version(2, 0));
        _mockFileHelper.Setup(fh => fh.GetFileVersion(extensionsList2[0])).Returns(new Version(2, 0));

        _mockFileHelper.Setup(fh => fh.EnumerateFiles(sourcesDir[0], SearchOption.TopDirectoryOnly, "TestAdapter.dll")).Returns(extensionsList1);
        _mockFileHelper.Setup(fh => fh.EnumerateFiles(sourcesDir[1], SearchOption.TopDirectoryOnly, "TestAdapter.dll")).Returns(extensionsList2);

        _testHostManager.Initialize(_mockMessageLogger.Object, $"<?xml version=\"1.0\" encoding=\"utf-8\"?><RunSettings> <RunConfiguration><TargetFrameworkVersion>{Framework.DefaultFramework}</TargetFrameworkVersion></RunConfiguration> </RunSettings>");

        // Act
        var resultExtensions = _testHostManager.GetTestPlatformExtensions(sources, new List<string>()).ToList();

        // Verify
        CollectionAssert.AreEqual(extensionsList1, resultExtensions);
        _mockMessageLogger.Verify(ml => ml.SendMessage(TestMessageLevel.Warning, It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void LaunchTestHostShouldReturnTestHostProcessId()
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
                    It.IsAny<Action<object?, string?>>())).Returns(Process.GetCurrentProcess());

        _testHostManager.Initialize(_mockMessageLogger.Object, $"<?xml version=\"1.0\" encoding=\"utf-8\"?><RunSettings> <RunConfiguration> <TargetPlatform>{Architecture.X64}</TargetPlatform> <TargetFrameworkVersion>{Framework.DefaultFramework}</TargetFrameworkVersion> <DisableAppDomain>{false}</DisableAppDomain> </RunConfiguration> </RunSettings>");
        var startInfo = _testHostManager.GetTestHostProcessStartInfo(Enumerable.Empty<string>(), null, default);

        _testHostManager.HostLaunched += TestHostManagerHostLaunched;

        Task<bool> processId = _testHostManager.LaunchTestHostAsync(startInfo, CancellationToken.None);
        processId.Wait();

        Assert.IsTrue(processId.Result);

#if NET5_0_OR_GREATER
        var pid = Environment.ProcessId;
#else
        int pid;
        using (var p = Process.GetCurrentProcess())
            pid = p.Id;
#endif
        Assert.AreEqual(pid, _testHostId);
    }

    [TestMethod]
    public void LaunchTestHostAsyncShouldNotStartHostProcessIfCancellationTokenIsSet()
    {
        _testableTestHostManager = new TestableTestHostManager(
            Architecture.X64,
            Framework.DefaultFramework,
            _mockProcessHelper.Object,
            true,
            _mockMessageLogger.Object);

        CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.Cancel();

        Assert.ThrowsException<OperationCanceledException>(() => _testableTestHostManager.LaunchTestHostAsync(GetDefaultStartInfo(), cancellationTokenSource.Token).Wait());
    }

    [TestMethod]
    public void PropertiesShouldReturnEmptyDictionary()
    {
        Assert.AreEqual(0, _testHostManager.Properties.Count);
    }

    [TestMethod]
    public void DefaultTestHostManagerShouldBeShared()
    {
        Assert.IsTrue(_testHostManager.Shared);
    }

    [TestMethod]
    public void LaunchTestHostShouldUseCustomHostIfSet()
    {
        var mockCustomLauncher = new Mock<ITestHostLauncher>();
        _testHostManager.SetCustomLauncher(mockCustomLauncher.Object);
        var currentProcess = Process.GetCurrentProcess();
        mockCustomLauncher.Setup(mc => mc.LaunchTestHost(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(currentProcess.Id);

        _testHostManager.HostLaunched += TestHostManagerHostLaunched;

        Task<bool> pid = _testHostManager.LaunchTestHostAsync(_startInfo, CancellationToken.None);
        pid.Wait();
        mockCustomLauncher.Verify(mc => mc.LaunchTestHost(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>()), Times.Once);

        Assert.IsTrue(pid.Result);
        Assert.AreEqual(currentProcess.Id, _testHostId);
    }

    [TestMethod]
    public void LaunchTestHostShouldSetExitCallbackInCaseCustomHost()
    {
        var mockCustomLauncher = new Mock<ITestHostLauncher>();
        _testHostManager.SetCustomLauncher(mockCustomLauncher.Object);
        var currentProcess = Process.GetCurrentProcess();
        mockCustomLauncher.Setup(mc => mc.LaunchTestHost(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(currentProcess.Id);
        _testHostManager.LaunchTestHostAsync(_startInfo, CancellationToken.None).Wait();

        _mockProcessHelper.Verify(ph => ph.SetExitCallback(currentProcess.Id, It.IsAny<Action<object?>>()));
    }

    [TestMethod]
    [TestCategory("Windows")]
    public void GetTestSourcesShouldReturnAppropriateSourceIfAppxRecipeIsProvided()
    {
        var sourcePath = Path.Combine(Path.GetDirectoryName(typeof(TestableTestHostManager).Assembly.GetAssemblyLocation())!, @"..\..\..\..\..\test\TestAssets\UWPTestAssets\UnitTestApp8.build.appxrecipe");
        IEnumerable<string> sources = _testHostManager.GetTestSources(new List<string> { sourcePath });
        Assert.IsTrue(sources.Any());
        Assert.IsTrue(sources.First().EndsWith(".exe", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    [TestCategory("Windows")]
    public void AppxManifestFileShouldReturnAppropriateSourceIfAppxManifestIsProvided()
    {
        var appxManifestPath = Path.Combine(Path.GetDirectoryName(typeof(TestableTestHostManager).Assembly.GetAssemblyLocation())!, @"..\..\..\..\..\test\TestAssets\UWPTestAssets\AppxManifest.xml");
        string? source = AppxManifestFile.GetApplicationExecutableName(appxManifestPath);
        Assert.AreEqual("UnitTestApp8.exe", source);
    }

    [TestMethod]
    public async Task ErrorMessageShouldBeReadAsynchronously()
    {
        string errorData = "Custom Error Strings";
        ErrorCallBackTestHelper(errorData, -1);

        await _testableTestHostManager.LaunchTestHostAsync(GetDefaultStartInfo(), CancellationToken.None);

        Assert.AreEqual(errorData, _errorMessage);
    }

    [TestMethod]
    public async Task NoErrorMessageIfExitCodeZero()
    {
        string errorData = string.Empty;
        ErrorCallBackTestHelper(errorData, 0);

        await _testableTestHostManager.LaunchTestHostAsync(GetDefaultStartInfo(), CancellationToken.None);

        Assert.IsNull(_errorMessage);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public async Task ErrorReceivedCallbackShouldNotLogNullOrEmptyData(string errorData)
    {
        ErrorCallBackTestHelper(errorData, -1);

        await _testableTestHostManager.LaunchTestHostAsync(GetDefaultStartInfo(), CancellationToken.None);

        Assert.AreEqual(_errorMessage, string.Empty);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public async Task ProcessExitedButNoErrorMessageIfNoDataWritten(int exitCode)
    {
        ExitCallBackTestHelper(exitCode);

        await _testableTestHostManager.LaunchTestHostAsync(GetDefaultStartInfo(), CancellationToken.None);

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
        await _testableTestHostManager.LaunchTestHostAsync(GetDefaultStartInfo(), CancellationToken.None);
        await _testableTestHostManager.CleanTestHostAsync(CancellationToken.None);

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
        await _testableTestHostManager.LaunchTestHostAsync(GetDefaultStartInfo(), CancellationToken.None);
        await _testableTestHostManager.CleanTestHostAsync(CancellationToken.None);

        Assert.IsTrue(isVerified);
    }

    private void TestableTestHostManagerHostExited(object? sender, HostProviderEventArgs e)
    {
        _errorMessage = e.Data.TrimEnd(Environment.NewLine.ToCharArray());
        _exitCode = e.ErrroCode;
    }

    private void TestHostManagerHostExited(object? sender, HostProviderEventArgs e)
    {
        if (e.ErrroCode != 0)
        {
            _errorMessage = e.Data.TrimEnd(Environment.NewLine.ToCharArray());
        }
    }

    private void TestHostManagerHostLaunched(object? sender, HostProviderEventArgs e)
    {
        _testHostId = e.ProcessId;
    }

    [MemberNotNull(nameof(_testableTestHostManager))]
    private void ErrorCallBackTestHelper(string errorMessage, int exitCode)
    {
        _testableTestHostManager = new TestableTestHostManager(
            Architecture.X64,
            Framework.DefaultFramework,
            _mockProcessHelper.Object,
            true,
            _mockMessageLogger.Object);

        _testableTestHostManager.HostExited += TestHostManagerHostExited;

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

    [MemberNotNull(nameof(_testableTestHostManager))]
    private void ExitCallBackTestHelper(int exitCode)
    {
        _testableTestHostManager = new TestableTestHostManager(
            Architecture.X64,
            Framework.DefaultFramework,
            _mockProcessHelper.Object,
            true,
            _mockMessageLogger.Object);

        _testableTestHostManager.HostExited += TestableTestHostManagerHostExited;

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

    private static TestProcessStartInfo GetDefaultStartInfo()
    {
        return new TestProcessStartInfo();
    }

    private class TestableTestHostManager : DefaultTestHostManager
    {
        public TestableTestHostManager(
            Architecture architecture,
            Framework framework,
            IProcessHelper processHelper,
            bool shared,
            IMessageLogger logger)
            : base(processHelper, new FileHelper(), new DotnetHostHelper(), new PlatformEnvironment(), new EnvironmentVariableHelper())
        {
            Initialize(logger, $"<?xml version=\"1.0\" encoding=\"utf-8\"?><RunSettings> <RunConfiguration> <TargetPlatform>{architecture}</TargetPlatform> <TargetFrameworkVersion>{framework}</TargetFrameworkVersion> <DisableAppDomain>{!shared}</DisableAppDomain> </RunConfiguration> </RunSettings>");
        }
    }
}
