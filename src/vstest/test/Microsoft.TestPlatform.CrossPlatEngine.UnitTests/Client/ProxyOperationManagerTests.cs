// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Hosting;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests.Client;

[TestClass]
public class ProxyOperationManagerTests : ProxyBaseManagerTests
{
    private const int Clientprocessexitwait = 10 * 1000;

    private static readonly int ConnectionTimeout = EnvironmentHelper.DefaultConnectionTimeout * 1000;
    private static readonly string DefaultRunSettings = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<RunSettings>\r\n  <DataCollectionRunSettings>\r\n    <DataCollectors >{0}</DataCollectors>\r\n  </DataCollectionRunSettings>\r\n</RunSettings>";
    private static readonly string TimoutErrorMessage =
        "vstest.console process failed to connect to testhost process after 90 seconds. This may occur due to machine slowness, please set environment variable VSTEST_CONNECTION_TIMEOUT to increase timeout.";

    private readonly ProxyOperationManager _testOperationManager;
    private readonly Mock<ITestRequestSender> _mockRequestSender;
    private readonly Mock<IRequestData> _mockRequestData;

    private Mock<IProcessHelper>? _mockProcessHelper;
    private Mock<IRunSettingsHelper>? _mockRunsettingHelper;
    private Mock<IWindowsRegistryHelper>? _mockWindowsRegistry;
    private Mock<IEnvironmentVariableHelper>? _mockEnvironmentVariableHelper;
    private Mock<IFileHelper>? _mockFileHelper;
    private Mock<IEnvironment>? _mockEnvironment;

    public ProxyOperationManagerTests()
    {
        _mockRequestSender = new Mock<ITestRequestSender>();
        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>())).Returns(true);
        _mockRequestData = new Mock<IRequestData>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(new Mock<IMetricsCollection>().Object);
        _testOperationManager = new TestableProxyOperationManager(_mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object);
    }

    [TestCleanup]
    public void Cleanup()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, string.Empty);
    }

    [TestMethod]
    public void SetupChannelShouldLaunchTestHost()
    {
        var expectedStartInfo = new TestProcessStartInfo();
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(123);
        _mockTestHostManager.Setup(
                th => th.GetTestHostProcessStartInfo(Enumerable.Empty<string>(), It.IsAny<Dictionary<string, string?>>(), It.IsAny<TestRunnerConnectionInfo>()))
            .Returns(expectedStartInfo);

        _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        _mockTestHostManager.Verify(thl => thl.LaunchTestHostAsync(It.Is<TestProcessStartInfo>(si => si == expectedStartInfo), It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public void SetupChannelShouldCreateTimestampedLogFileForHost()
    {
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(123);
        EqtTrace.InitializeTrace("log.txt", PlatformTraceLevel.Verbose);

        _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        _mockTestHostManager.Verify(
            th =>
                th.GetTestHostProcessStartInfo(
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<Dictionary<string, string?>>(),
                    It.Is<TestRunnerConnectionInfo>(
                        t => t.LogFile!.Contains("log.host." + DateTime.Now.ToString("yy-MM-dd", CultureInfo.CurrentCulture))
                             && t.LogFile.Contains("_" + Environment.CurrentManagedThreadId + ".txt"))));
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Off;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Off;
#endif
    }

    [TestMethod]
    [DataRow("Dummy", true, false, false)]
    [DataRow(ProxyOperationManager.DefaultTesthostFriendlyName, true, true, true)]
    [DataRow(ProxyOperationManager.DotnetTesthostFriendlyName, true, true, true)]
    public void SetupChannelOutcomeShouldTakeTesthostSessionSupportIntoAccount(
        string testhostFriendlyName,
        bool isTestSessionEnabled,
        bool expectedCompatibilityCheckResult,
        bool expectedSetupResult)
    {
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(123);

        var testOperationManager = new TestableProxyOperationManager(
            _mockRequestData.Object,
            _mockRequestSender.Object,
            _mockTestHostManager.Object)
        {
            IsTestSessionEnabled = isTestSessionEnabled,
            TesthostFriendlyName = testhostFriendlyName
        };

        Assert.IsTrue(testOperationManager.IsTesthostCompatibleWithTestSessions() == expectedCompatibilityCheckResult);
        Assert.IsTrue(testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings) == expectedSetupResult);
    }

    [TestMethod]
    public void SetupChannelShouldAddRunnerProcessIdForTestHost()
    {
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(123);

        _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

#if NET5_0_OR_GREATER
        var pid = Environment.ProcessId;
#else
        int pid;
        using (var p = Process.GetCurrentProcess())
            pid = p.Id;
#endif

        _mockTestHostManager.Verify(
            th =>
                th.GetTestHostProcessStartInfo(
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<Dictionary<string, string?>>(),
                    It.Is<TestRunnerConnectionInfo>(t => t.RunnerProcessId.Equals(pid))));
    }

    [TestMethod]
    public void SetupChannelShouldAddCorrectTraceLevelForTestHost()
    {
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Info;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Info;
#endif

        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(123);
        _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        _mockTestHostManager.Verify(
            th =>
                th.GetTestHostProcessStartInfo(
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<Dictionary<string, string?>>(),
                    It.Is<TestRunnerConnectionInfo>(t => t.TraceLevel == (int)PlatformTraceLevel.Info)));
    }

    [TestMethod]
    public void SetupChannelShouldSetupServerForCommunication()
    {
        _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        _mockRequestSender.Verify(s => s.InitializeCommunication(), Times.Once);
    }

    [TestMethod]
    public void SetupChannelShouldCallHostServerIfRunnerIsServer()
    {
        var connectionInfo = new TestHostConnectionInfo
        {
            Endpoint = IPAddress.Loopback + ":0",
            Role = ConnectionRole.Host,
            Transport = Transport.Sockets
        };
        ProtocolConfig protocolConfig = new() { Version = 2 };
        var mockCommunicationServer = new Mock<ICommunicationEndPoint>();

        mockCommunicationServer.Setup(mc => mc.Start(connectionInfo.Endpoint)).Returns(IPAddress.Loopback + ":123").Callback(
            () => mockCommunicationServer.Raise(s => s.Connected += null, mockCommunicationServer.Object, new ConnectedEventArgs(_mockChannel.Object)));

        var testRequestSender = new TestRequestSender(mockCommunicationServer.Object, connectionInfo, _mockDataSerializer.Object, protocolConfig, Clientprocessexitwait);
        SetupChannelMessage(MessageType.VersionCheck, MessageType.VersionCheck, protocolConfig.Version);

        _mockTestHostManager.Setup(thm => thm.GetTestHostConnectionInfo()).Returns(connectionInfo);

        var localTestOperationManager = new TestableProxyOperationManager(_mockRequestData.Object, testRequestSender, _mockTestHostManager.Object);

        localTestOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        mockCommunicationServer.Verify(s => s.Start(IPAddress.Loopback.ToString() + ":0"), Times.Once);
    }

    [TestMethod]
    public void SetupChannelShouldCallSetupClientIfRunnerIsClient()
    {
        var connectionInfo = new TestHostConnectionInfo
        {
            Endpoint = IPAddress.Loopback + ":124",
            Role = ConnectionRole.Host,
            Transport = Transport.Sockets
        };
        ProtocolConfig protocolConfig = new() { Version = 2 };
        var mockCommunicationEndpoint = new Mock<ICommunicationEndPoint>();
        mockCommunicationEndpoint.Setup(mc => mc.Start(connectionInfo.Endpoint)).Returns(connectionInfo.Endpoint).Callback(() => mockCommunicationEndpoint.Raise(
            s => s.Connected += null,
            mockCommunicationEndpoint.Object,
            new ConnectedEventArgs(_mockChannel.Object)));

        SetupChannelMessage(MessageType.VersionCheck, MessageType.VersionCheck, protocolConfig.Version);
        var testRequestSender = new TestRequestSender(mockCommunicationEndpoint.Object, connectionInfo, _mockDataSerializer.Object, new ProtocolConfig { Version = 2 }, Clientprocessexitwait);

        _mockTestHostManager.Setup(thm => thm.GetTestHostConnectionInfo()).Returns(connectionInfo);

        var localTestOperationManager = new TestableProxyOperationManager(_mockRequestData.Object, testRequestSender, _mockTestHostManager.Object);

        localTestOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        mockCommunicationEndpoint.Verify(s => s.Start(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void SetupChannelShouldNotInitializeIfConnectionIsAlreadyInitialized()
    {
        _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);
        _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        _mockRequestSender.Verify(s => s.InitializeCommunication(), Times.Once);
    }

    [TestMethod]
    public void SetupChannelShouldWaitForTestHostConnection()
    {
        _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        _mockRequestSender.Verify(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public void SetupChannelShouldNotWaitForTestHostConnectionIfConnectionIsInitialized()
    {
        _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);
        _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        _mockRequestSender.Verify(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>()), Times.Exactly(1));
    }

    [TestMethod]
    public void SetupChannelShouldHonorTimeOutSetByUser()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, "100");

        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(100000, It.IsAny<CancellationToken>())).Returns(true);
        _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        _mockRequestSender.Verify(rs => rs.WaitForRequestHandlerConnection(100000, It.IsAny<CancellationToken>()), Times.Exactly(1));
    }

    [TestMethod]
    public void SetupChannelShouldThrowIfWaitForTestHostConnectionTimesOut()
    {
        SetupTestHostLaunched(true);
        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>())).Returns(false);

        var operationManager = new TestableProxyOperationManager(_mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object);

        var message = Assert.ThrowsException<TestPlatformException>(() => operationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings)).Message;
        Assert.AreEqual(message, TimoutErrorMessage);
    }

    [TestMethod]
    public void SetupChannelShouldThrowTestPlatformExceptionIfRequestCancelled()
    {
        SetupTestHostLaunched(true);
        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>())).Returns(false);

        var cancellationTokenSource = new CancellationTokenSource();
        var operationManager = new TestableProxyOperationManager(_mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object, cancellationTokenSource);

        cancellationTokenSource.Cancel();
        var message = Assert.ThrowsException<TestPlatformException>(() => operationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings)).Message;
        Equals("Canceling the operation as requested.", message);
    }

    [TestMethod]
    public void SetupChannelShouldThrowTestPlatformExceptionIfRequestCancelledDuringLaunchOfTestHost()
    {
        SetupTestHostLaunched(true);
        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>())).Returns(false);

        _mockTestHostManager.Setup(rs => rs.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Callback(() => Task.Run(() => throw new OperationCanceledException()));

        var cancellationTokenSource = new CancellationTokenSource();
        var operationManager = new TestableProxyOperationManager(_mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object, cancellationTokenSource);

        var message = Assert.ThrowsException<TestPlatformException>(() => operationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings)).Message;
        Equals("Canceling the operation as requested.", message);
    }

    [TestMethod]
    public void SetupChannelShouldThrowTestPlatformExceptionIfRequestCancelledPostHostLaunchDuringWaitForHandlerConnection()
    {
        SetupTestHostLaunched(true);
        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>())).Returns(false);

        var cancellationTokenSource = new CancellationTokenSource();
        _mockTestHostManager.Setup(rs => rs.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Callback(() => cancellationTokenSource.Cancel());
        var operationManager = new TestableProxyOperationManager(_mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object, cancellationTokenSource);

        var message = Assert.ThrowsException<TestPlatformException>(() => operationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings)).Message;
        Equals("Canceling the operation as requested.", message);
    }

    [TestMethod]
    public void SetupChannelShouldThrowIfLaunchTestHostFails()
    {
        SetupTestHostLaunched(false);
        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>())).Returns(true);

        var operationManager = new TestableProxyOperationManager(_mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object);

        var message = Assert.ThrowsException<TestPlatformException>(() => operationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings)).Message;
        Assert.AreEqual(message, Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Resources.Resources.InitializationFailed);
    }

    [TestMethod]
    public void SetupChannelShouldCheckVersionWithTestHost()
    {
        _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);
        _mockRequestSender.Verify(rs => rs.CheckVersionWithTestHost(), Times.Once);
    }

    [TestMethod]
    public void SetupChannelShouldThrowExceptionIfVersionCheckFails()
    {
        // Make the version check fail
        _mockRequestSender.Setup(rs => rs.CheckVersionWithTestHost()).Throws(new TestPlatformException("Version check failed"));
        Assert.ThrowsException<TestPlatformException>(() => _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings));
    }

    [TestMethod]
    public void SetupChannelForDotnetHostManagerWithIsVersionCheckRequiredFalseShouldNotCheckVersionWithTestHost()
    {
        SetUpMocksForDotNetTestHost();
        var testHostManager = new TestableDotnetTestHostManager(false, _mockProcessHelper.Object, _mockFileHelper.Object, _mockEnvironment.Object, _mockRunsettingHelper.Object, _mockWindowsRegistry.Object, _mockEnvironmentVariableHelper.Object);
        var operationManager = new TestableProxyOperationManager(_mockRequestData.Object, _mockRequestSender.Object, testHostManager);

        operationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        _mockRequestSender.Verify(rs => rs.CheckVersionWithTestHost(), Times.Never);
    }

    [TestMethod]
    public void SetupChannelForDotnetHostManagerWithIsVersionCheckRequiredTrueShouldCheckVersionWithTestHost()
    {
        SetUpMocksForDotNetTestHost();
        var testHostManager = new TestableDotnetTestHostManager(true, _mockProcessHelper.Object, _mockFileHelper.Object, _mockEnvironment.Object, _mockRunsettingHelper.Object, _mockWindowsRegistry.Object, _mockEnvironmentVariableHelper.Object);
        var operationManager = new TestableProxyOperationManager(_mockRequestData.Object, _mockRequestSender.Object, testHostManager);

        operationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        _mockRequestSender.Verify(rs => rs.CheckVersionWithTestHost(), Times.Once);
    }

    [TestMethod]
    public void CloseShouldEndSessionIfHostWasLaunched()
    {
        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>())).Returns(true);
        _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        _testOperationManager.Close();

        _mockRequestSender.Verify(rs => rs.EndSession(), Times.Once);
    }

    [TestMethod]
    public void CloseShouldNotEndSessionIfHostLaucnhedFailed()
    {
        _testOperationManager.Close();

        _mockRequestSender.Verify(rs => rs.EndSession(), Times.Never);
    }

    [TestMethod]
    public void CloseShouldAlwaysCleanTestHost()
    {
        _testOperationManager.Close();

        _mockTestHostManager.Verify(th => th.CleanTestHostAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public void CloseShouldResetChannelInitialization()
    {
        SetupWaitForTestHostExit();
        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>())).Returns(true);
        _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        _testOperationManager.Close();

        _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);
        _mockTestHostManager.Verify(th => th.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [TestMethod]
    public void CloseShouldTerminateTesthostProcessIfWaitTimesout()
    {
        // Ensure testhost start returns a dummy process id
        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>())).Returns(true);
        _testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        _testOperationManager.Close();

        _mockTestHostManager.Verify(th => th.CleanTestHostAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public void CloseShouldNotThrowIfEndSessionFails()
    {
        _mockRequestSender.Setup(rs => rs.EndSession()).Throws<Exception>();

        _testOperationManager.Close();
    }

    private void SetupWaitForTestHostExit()
    {
        // Raise host exited when end session is called
        _mockRequestSender.Setup(rs => rs.EndSession())
            .Callback(() => _mockTestHostManager.Raise(t => t.HostExited += null, new HostProviderEventArgs(string.Empty)));
    }

    private void SetupTestHostLaunched(bool launchStatus)
    {
        // Raise host exited when end session is called
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>()))
            .Callback(() => _mockTestHostManager.Raise(t => t.HostLaunched += null, new HostProviderEventArgs(string.Empty)))
            .Returns(Task.FromResult(launchStatus));
    }

    [TestMethod]
    public void UpdateTestProcessStartInfoShouldUpdateTelemetryOptedInArgTrueIfTelemetryOptedIn()
    {
        TestProcessStartInfo receivedTestProcessInfo = new();
        var mockRequestData = new Mock<IRequestData>();

        mockRequestData.Setup(rd => rd.IsTelemetryOptedIn).Returns(true);

        var testOperationManager = new TestableProxyOperationManager(mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object);

        _mockTestHostManager
            .Setup(tm => tm.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>()))
            .Callback<TestProcessStartInfo, CancellationToken>(
                (testProcessStartInfo, cancellationToken) => receivedTestProcessInfo = testProcessStartInfo)
            .Returns(Task.FromResult(true));

        // Act.
        testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        // Verify.
        Assert.IsTrue(receivedTestProcessInfo.Arguments!.Contains("--telemetryoptedin true"));
    }

    [TestMethod]
    public void UpdateTestProcessStartInfoShouldUpdateTelemetryOptedInArgFalseIfTelemetryOptedOut()
    {
        TestProcessStartInfo receivedTestProcessInfo = new();
        var mockRequestData = new Mock<IRequestData>();

        mockRequestData.Setup(rd => rd.IsTelemetryOptedIn).Returns(false);

        var testOperationManager = new TestableProxyOperationManager(mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object);

        _mockTestHostManager
            .Setup(tm => tm.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>()))
            .Callback<TestProcessStartInfo, CancellationToken>(
                (testProcessStartInfo, cancellationToken) => receivedTestProcessInfo = testProcessStartInfo)
            .Returns(Task.FromResult(true));

        // Act.
        testOperationManager.SetupChannel(Enumerable.Empty<string>(), DefaultRunSettings);

        // Verify.
        Assert.IsTrue(receivedTestProcessInfo.Arguments!.Contains("--telemetryoptedin false"));
    }

    [MemberNotNull(nameof(_mockProcessHelper), nameof(_mockFileHelper), nameof(_mockEnvironment), nameof(_mockRunsettingHelper), nameof(_mockWindowsRegistry), nameof(_mockEnvironmentVariableHelper))]
    private void SetUpMocksForDotNetTestHost()
    {
        _mockProcessHelper = new Mock<IProcessHelper>();
        _mockFileHelper = new Mock<IFileHelper>();
        _mockEnvironment = new Mock<IEnvironment>();
        _mockRunsettingHelper = new Mock<IRunSettingsHelper>();
        _mockWindowsRegistry = new Mock<IWindowsRegistryHelper>();
        _mockEnvironmentVariableHelper = new Mock<IEnvironmentVariableHelper>();

        _mockRunsettingHelper.SetupGet(r => r.IsDefaultTargetArchitecture).Returns(true);
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

                    errorCallback(process, string.Empty);
                    exitCallback(process);
                }).Returns(Process.GetCurrentProcess());
    }

    private class TestableProxyOperationManager : ProxyOperationManager
    {
        public TestableProxyOperationManager(IRequestData requestData,
            ITestRequestSender requestSender,
            ITestRuntimeProvider testHostManager) : base(requestData, requestSender, testHostManager, Framework.DefaultFramework)
        {
        }

        public TestableProxyOperationManager(
            IRequestData requestData,
            ITestRequestSender requestSender,
            ITestRuntimeProvider testHostManager,
            CancellationTokenSource cancellationTokenSource) : base(requestData, requestSender, testHostManager, Framework.DefaultFramework)
        {
            CancellationTokenSource = cancellationTokenSource;
        }

        public string TesthostFriendlyName { get; set; } = "Dummy";

        internal override string ReadTesthostFriendlyName()
        {
            return TesthostFriendlyName;
        }
    }

    private class TestableDotnetTestHostManager : DotnetTestHostManager
    {
        private readonly bool _isVersionCheckRequired;

        public TestableDotnetTestHostManager(
            bool checkRequired,
            IProcessHelper processHelper,
            IFileHelper fileHelper,
            IEnvironment environment,
            IRunSettingsHelper runsettingHelper,
            IWindowsRegistryHelper windowsRegistryHelper,
            IEnvironmentVariableHelper environmentVariableHelper) : base(
            processHelper,
            fileHelper,
            new DotnetHostHelper(fileHelper, environment, windowsRegistryHelper, environmentVariableHelper, processHelper),
            environment,
            runsettingHelper,
            windowsRegistryHelper,
            environmentVariableHelper)
        {
            _isVersionCheckRequired = checkRequired;
        }

        internal override bool IsVersionCheckRequired => _isVersionCheckRequired;

        public override TestProcessStartInfo GetTestHostProcessStartInfo(
            IEnumerable<string> sources,
            IDictionary<string, string?>? environmentVariables,
            TestRunnerConnectionInfo connectionInfo)
        {
            return new TestProcessStartInfo();
        }
    }
}
