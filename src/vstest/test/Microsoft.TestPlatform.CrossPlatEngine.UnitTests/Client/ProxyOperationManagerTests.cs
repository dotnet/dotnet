// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
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
    private readonly Mock<IConnectionValidationConfigurable> _mockConnectionValidator;
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
        _mockConnectionValidator = _mockRequestSender.As<IConnectionValidationConfigurable>();
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
                th => th.GetTestHostProcessStartInfo(Array.Empty<string>(), It.IsAny<Dictionary<string, string?>>(), It.IsAny<TestRunnerConnectionInfo>()))
            .Returns(expectedStartInfo);

        _testOperationManager.SetupChannel([], DefaultRunSettings);

        _mockTestHostManager.Verify(thl => thl.LaunchTestHostAsync(It.Is<TestProcessStartInfo>(si => si == expectedStartInfo), It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public void SetupChannelShouldCreateTimestampedLogFileForHost()
    {
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(123);
        EqtTrace.InitializeTrace("log.txt", PlatformTraceLevel.Verbose);

        _testOperationManager.SetupChannel([], DefaultRunSettings);

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
        Assert.IsTrue(testOperationManager.SetupChannel([], DefaultRunSettings) == expectedSetupResult);
    }

    [TestMethod]
    public void SetupChannelShouldAddRunnerProcessIdForTestHost()
    {
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(123);

        _testOperationManager.SetupChannel([], DefaultRunSettings);

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
    public void SetupChannelShouldInjectConnectionSecretsWhenProviderSupportsConnectionSecrets()
    {
        var startInfo = new TestProcessStartInfo();
        var mockProvider = CreateProviderMock(supportsConnectionSecrets: true, startInfo);
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(123);

        var operationManager = new TestableProxyOperationManager(_mockRequestData.Object, _mockRequestSender.Object, mockProvider.Object);
        operationManager.SetupChannel([], DefaultRunSettings);

        Assert.IsNotNull(startInfo.EnvironmentVariables);
        Assert.IsTrue(startInfo.EnvironmentVariables.TryGetValue(EnvironmentHelper.VstestConnectionSecrets, out var secrets));
        // A pair is two 256-bit values rendered as hex, joined by one separator: 64 + 1 + 64.
        Assert.IsFalse(string.IsNullOrEmpty(secrets));
        Assert.AreEqual(129, secrets!.Length);
        _mockConnectionValidator.Verify(x => x.RequireConnectionValidation(), Times.Once);
        _mockConnectionValidator.Verify(x => x.UseConnectionSecrets(secrets), Times.Once);
    }

    [TestMethod]
    public void SetupChannelShouldCanonicalizeReservedSecretsEnvironmentVariables()
    {
        const string lowerConnectionSecrets = "vstest_connection_secrets";
        const string lowerDataCollectionSecrets = "vstest_datacollection_connection_secrets";
        const string lowerDesignModeSecrets = "vstest_designmode_connection_secrets";
        var startInfo = new TestProcessStartInfo
        {
            EnvironmentVariables = new Dictionary<string, string?>(StringComparer.Ordinal)
            {
                [lowerConnectionSecrets] = "stale",
                [lowerDataCollectionSecrets] = "stale",
                [lowerDesignModeSecrets] = "stale",
            },
        };
        var mockProvider = CreateProviderMock(supportsConnectionSecrets: true, startInfo);
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(123);

        var operationManager = new TestableProxyOperationManager(
            _mockRequestData.Object,
            _mockRequestSender.Object,
            mockProvider.Object);
        operationManager.SetupChannel([], DefaultRunSettings);

        Assert.IsNotNull(startInfo.EnvironmentVariables);
        Assert.IsFalse(startInfo.EnvironmentVariables.ContainsKey(lowerConnectionSecrets));
        Assert.IsFalse(startInfo.EnvironmentVariables.ContainsKey(lowerDataCollectionSecrets));
        Assert.IsFalse(startInfo.EnvironmentVariables.ContainsKey(lowerDesignModeSecrets));
        Assert.IsFalse(string.IsNullOrEmpty(startInfo.EnvironmentVariables[EnvironmentHelper.VstestConnectionSecrets]));
        Assert.AreEqual("stale", startInfo.EnvironmentVariables[EnvironmentHelper.VstestDataCollectionConnectionSecrets]);
        Assert.IsNull(startInfo.EnvironmentVariables[EnvironmentHelper.VstestDesignModeConnectionSecrets]);
    }

    /// <summary>
    /// There is no unauthenticated mode, so a test host that does not perform the identity check is
    /// refused before it is launched, and the reason names the component and the version to move to.
    /// </summary>
    [TestMethod]
    public void SetupChannelShouldRefuseATestHostThatDoesNotSupportConnectionSecrets()
    {
        var startInfo = new TestProcessStartInfo();
        var mockProvider = CreateProviderMock(supportsConnectionSecrets: false, startInfo);
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(123);

        var operationManager = new TestableProxyOperationManager(_mockRequestData.Object, _mockRequestSender.Object, mockProvider.Object);

        var error = Assert.ThrowsException<TestPlatformException>(
            () => operationManager.SetupChannel([], DefaultRunSettings)).Message;

        // The message must say where to read about the fix, so the user is not left with a
        // timeout whose cause they have to guess.
        StringAssert.Contains(error, "https://aka.ms/vstest/negotiation-error");
        _mockConnectionValidator.Verify(x => x.UseConnectionSecrets(It.IsAny<string>()), Times.Never);
        _mockRequestSender.Verify(rs => rs.Close(), Times.Once, "A refused run must close the channel it already opened.");
        _mockConnectionValidator.Verify(x => x.CannotValidateConnection(error), Times.Once);
        mockProvider.Verify(
            p => p.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>()),
            Times.Never,
            "A test host that cannot be validated must not be launched at all.");
    }

    /// <summary>
    /// A provider that only implements the older interfaces cannot report which test host it
    /// launches, so it is treated the same as one that reports no support.
    /// </summary>
    [TestMethod]
    public void SetupChannelShouldRefuseAProviderThatCannotReportSupport()
    {
        var legacyProvider = new Mock<ITestRuntimeProvider>();
        legacyProvider.SetupGet(th => th.Shared).Returns(true);
        legacyProvider.Setup(
                m => m.GetTestHostProcessStartInfo(
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<IDictionary<string, string?>>(),
                    It.IsAny<TestRunnerConnectionInfo>()))
            .Returns(new TestProcessStartInfo());
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(123);
        var operationManager = new TestableProxyOperationManager(
            _mockRequestData.Object,
            _mockRequestSender.Object,
            legacyProvider.Object);

        var error = Assert.ThrowsException<TestPlatformException>(
            () => operationManager.SetupChannel([], DefaultRunSettings)).Message;

        StringAssert.Contains(error, "https://aka.ms/vstest/negotiation-error");
        _mockConnectionValidator.Verify(x => x.UseConnectionSecrets(It.IsAny<string>()), Times.Never);
        _mockRequestSender.Verify(rs => rs.Close(), Times.Once, "A refused run must close the channel it already opened.");
        legacyProvider.Verify(
            p => p.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static Mock<ITestRuntimeProvider3> CreateProviderMock(
        bool supportsConnectionSecrets,
        TestProcessStartInfo startInfo,
        int? launchedProcessId = null)
    {
        var mockProvider = new Mock<ITestRuntimeProvider3>();
        mockProvider.SetupGet(th => th.Shared).Returns(true);
        mockProvider.SetupGet(th => th.SupportsConnectionSecrets).Returns(supportsConnectionSecrets);
        mockProvider.Setup(
                m => m.GetTestHostProcessStartInfo(
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<IDictionary<string, string?>>(),
                    It.IsAny<TestRunnerConnectionInfo>()))
            .Returns(startInfo);
        mockProvider.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>()))
            .Callback(
                () =>
                {
                    using var process = Process.GetCurrentProcess();
                    mockProvider.Raise(
                        thm => thm.HostLaunched += null,
                        new HostProviderEventArgs(
                            string.Empty,
                            0,
                            launchedProcessId ?? process.Id));
                })
            .Returns(Task.FromResult(true));
        return mockProvider;
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
        _testOperationManager.SetupChannel([], DefaultRunSettings);

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
        _testOperationManager.SetupChannel([], DefaultRunSettings);

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
        mockCommunicationServer.As<IConnectionValidationConfigurable>();

        mockCommunicationServer.Setup(mc => mc.Start(connectionInfo.Endpoint)).Returns(IPAddress.Loopback + ":123").Callback(
            () => mockCommunicationServer.Raise(s => s.Connected += null, mockCommunicationServer.Object, new ConnectedEventArgs(_mockChannel.Object)));

        var testRequestSender = new TestRequestSender(mockCommunicationServer.Object, connectionInfo, _mockDataSerializer.Object, protocolConfig, Clientprocessexitwait);
        SetupChannelMessage(MessageType.VersionCheck, MessageType.VersionCheck, protocolConfig.Version);

        _mockTestHostManager.Setup(thm => thm.GetTestHostConnectionInfo()).Returns(connectionInfo);

        var localTestOperationManager = new TestableProxyOperationManager(_mockRequestData.Object, testRequestSender, _mockTestHostManager.Object);

        localTestOperationManager.SetupChannel([], DefaultRunSettings);

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
        mockCommunicationEndpoint.As<IConnectionValidationConfigurable>();
        mockCommunicationEndpoint.Setup(mc => mc.Start(connectionInfo.Endpoint)).Returns(connectionInfo.Endpoint).Callback(() => mockCommunicationEndpoint.Raise(
            s => s.Connected += null,
            mockCommunicationEndpoint.Object,
            new ConnectedEventArgs(_mockChannel.Object)));

        SetupChannelMessage(MessageType.VersionCheck, MessageType.VersionCheck, protocolConfig.Version);
        var testRequestSender = new TestRequestSender(mockCommunicationEndpoint.Object, connectionInfo, _mockDataSerializer.Object, new ProtocolConfig { Version = 2 }, Clientprocessexitwait);

        _mockTestHostManager.Setup(thm => thm.GetTestHostConnectionInfo()).Returns(connectionInfo);

        var localTestOperationManager = new TestableProxyOperationManager(_mockRequestData.Object, testRequestSender, _mockTestHostManager.Object);

        localTestOperationManager.SetupChannel([], DefaultRunSettings);

        mockCommunicationEndpoint.Verify(s => s.Start(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void SetupChannelShouldNotInitializeIfConnectionIsAlreadyInitialized()
    {
        _testOperationManager.SetupChannel([], DefaultRunSettings);
        _testOperationManager.SetupChannel([], DefaultRunSettings);

        _mockRequestSender.Verify(s => s.InitializeCommunication(), Times.Once);
    }

    [TestMethod]
    public void SetupChannelShouldWaitForTestHostConnection()
    {
        _testOperationManager.SetupChannel([], DefaultRunSettings);

        _mockRequestSender.Verify(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public void SetupChannelShouldNotWaitForTestHostConnectionIfConnectionIsInitialized()
    {
        _testOperationManager.SetupChannel([], DefaultRunSettings);
        _testOperationManager.SetupChannel([], DefaultRunSettings);

        _mockRequestSender.Verify(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>()), Times.Exactly(1));
    }

    [TestMethod]
    public void SetupChannelShouldHonorTimeOutSetByUser()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, "100");

        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(100000, It.IsAny<CancellationToken>())).Returns(true);
        _testOperationManager.SetupChannel([], DefaultRunSettings);

        _mockRequestSender.Verify(rs => rs.WaitForRequestHandlerConnection(100000, It.IsAny<CancellationToken>()), Times.Exactly(1));
    }

    [TestMethod]
    public void SetupChannelShouldThrowIfWaitForTestHostConnectionTimesOut()
    {
        SetupTestHostLaunched(true);
        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>())).Returns(false);

        var operationManager = new TestableProxyOperationManager(_mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object);

        var message = Assert.ThrowsException<TestPlatformException>(() => operationManager.SetupChannel([], DefaultRunSettings)).Message;
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
        var message = Assert.ThrowsException<TestPlatformException>(() => operationManager.SetupChannel([], DefaultRunSettings)).Message;
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

        var message = Assert.ThrowsException<TestPlatformException>(() => operationManager.SetupChannel([], DefaultRunSettings)).Message;
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

        var message = Assert.ThrowsException<TestPlatformException>(() => operationManager.SetupChannel([], DefaultRunSettings)).Message;
        Equals("Canceling the operation as requested.", message);
    }

    [TestMethod]
    public void SetupChannelShouldThrowIfLaunchTestHostFails()
    {
        SetupTestHostLaunched(false);
        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>())).Returns(true);

        var operationManager = new TestableProxyOperationManager(_mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object);

        var message = Assert.ThrowsException<TestPlatformException>(() => operationManager.SetupChannel([], DefaultRunSettings)).Message;
        Assert.AreEqual(message, Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Resources.Resources.InitializationFailed);
    }

    [TestMethod]
    public void SetupChannelShouldCheckVersionWithTestHost()
    {
        _testOperationManager.SetupChannel([], DefaultRunSettings);
        _mockRequestSender.Verify(rs => rs.CheckVersionWithTestHost(), Times.Once);
    }

    [TestMethod]
    public void SetupChannelShouldThrowExceptionIfVersionCheckFails()
    {
        // Make the version check fail
        _mockRequestSender.Setup(rs => rs.CheckVersionWithTestHost()).Throws(new TestPlatformException("Version check failed"));
        Assert.ThrowsException<TestPlatformException>(() => _testOperationManager.SetupChannel([], DefaultRunSettings));
    }

    [TestMethod]
    public void SetupChannelForDotnetHostManagerWithIsVersionCheckRequiredFalseShouldNotCheckVersionWithTestHost()
    {
        SetUpMocksForDotNetTestHost();
        var testHostManager = new TestableDotnetTestHostManager(false, _mockProcessHelper.Object, _mockFileHelper.Object, _mockEnvironment.Object, _mockRunsettingHelper.Object, _mockWindowsRegistry.Object, _mockEnvironmentVariableHelper.Object);
        testHostManager.Initialize(new NullMessageLogger(), DefaultRunSettings);

        var operationManager = new TestableProxyOperationManager(_mockRequestData.Object, _mockRequestSender.Object, testHostManager);

        operationManager.SetupChannel([], DefaultRunSettings);

        _mockRequestSender.Verify(rs => rs.CheckVersionWithTestHost(), Times.Never);
    }

    [TestMethod]
    public void SetupChannelForDotnetHostManagerWithIsVersionCheckRequiredTrueShouldCheckVersionWithTestHost()
    {
        SetUpMocksForDotNetTestHost();
        var testHostManager = new TestableDotnetTestHostManager(true, _mockProcessHelper.Object, _mockFileHelper.Object, _mockEnvironment.Object, _mockRunsettingHelper.Object, _mockWindowsRegistry.Object, _mockEnvironmentVariableHelper.Object);
        testHostManager.Initialize(new NullMessageLogger(), DefaultRunSettings);
        var operationManager = new TestableProxyOperationManager(_mockRequestData.Object, _mockRequestSender.Object, testHostManager);

        operationManager.SetupChannel([], DefaultRunSettings);

        _mockRequestSender.Verify(rs => rs.CheckVersionWithTestHost(), Times.Once);
    }

    [TestMethod]
    public void CloseShouldEndSessionIfHostWasLaunched()
    {
        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>())).Returns(true);
        _testOperationManager.SetupChannel([], DefaultRunSettings);

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
        _testOperationManager.SetupChannel([], DefaultRunSettings);

        _testOperationManager.Close();

        _testOperationManager.SetupChannel([], DefaultRunSettings);
        _mockTestHostManager.Verify(th => th.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [TestMethod]
    public void CloseShouldTerminateTesthostProcessIfWaitTimesout()
    {
        // Ensure testhost start returns a dummy process id
        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(ConnectionTimeout, It.IsAny<CancellationToken>())).Returns(true);
        _testOperationManager.SetupChannel([], DefaultRunSettings);

        _testOperationManager.Close();

        _mockTestHostManager.Verify(th => th.CleanTestHostAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public void CloseShouldNotThrowIfEndSessionFails()
    {
        _mockRequestSender.Setup(rs => rs.EndSession()).Throws<Exception>();

        _testOperationManager.Close();
    }

    private static HostProviderEventArgs CreateCurrentHostLaunchedArgs()
    {
        using var process = Process.GetCurrentProcess();
        return new HostProviderEventArgs(string.Empty, 0, process.Id);
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
            .Callback(() => _mockTestHostManager.Raise(t => t.HostLaunched += null, CreateCurrentHostLaunchedArgs()))
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
        testOperationManager.SetupChannel([], DefaultRunSettings);

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
        testOperationManager.SetupChannel([], DefaultRunSettings);

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
            Framework testHostManagerFramework)
            : base(requestData, requestSender, testHostManager, testHostManagerFramework)
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

        // These tests are about the version check, and they override
        // GetTestHostProcessStartInfo, which is where the real manager resolves the testhost and its
        // version. Report support so the run is not refused before it gets that far.
        public override bool SupportsConnectionSecrets => true;

        public override TestProcessStartInfo GetTestHostProcessStartInfo(
            IEnumerable<string> sources,
            IDictionary<string, string?>? environmentVariables,
            TestRunnerConnectionInfo connectionInfo)
        {
            return new TestProcessStartInfo();
        }
    }
}

internal class NullMessageLogger : IMessageLogger
{
    public void SendMessage(TestMessageLevel testMessageLevel, string message)
    {
    }
}
