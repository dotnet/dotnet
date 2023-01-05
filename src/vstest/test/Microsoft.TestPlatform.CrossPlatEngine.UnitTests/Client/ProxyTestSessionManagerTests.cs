// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.CrossPlatEngine.UnitTests.Client;

[TestClass]
public class ProxyTestSessionManagerTests
{
    private readonly IList<string> _fakeTestSources = new List<string>() { @"C:\temp\FakeTestAsset.dll" };
    private readonly Dictionary<string, TestRuntimeProviderInfo> _fakeTestSourcesToRuntimeProviderMap;
    private readonly IList<string> _fakeTestMultipleSources = new List<string>() {
        @"C:\temp\FakeTestAsset1.dll",
        @"C:\temp\FakeTestAsset2.dll",
        @"C:\temp\FakeTestAsset3.dll",
        @"C:\temp\FakeTestAsset4.dll",
        @"C:\temp\FakeTestAsset5.dll",
        @"C:\temp\FakeTestAsset6.dll",
        @"C:\temp\FakeTestAsset7.dll",
        @"C:\temp\FakeTestAsset8.dll",
    };
    private readonly string _runSettingsNoEnvVars = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <RunSettings>
                <RunConfiguration>
                </RunConfiguration>
            </RunSettings>";
    private readonly string _runSettingsOneEnvVar = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <RunSettings>
                <RunConfiguration>
                    <EnvironmentVariables>
                        <AAA>Test1</AAA>
                    </EnvironmentVariables>
                </RunConfiguration>
            </RunSettings>";
    private readonly string _runSettingsTwoEnvVars = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <RunSettings>
                <RunConfiguration>
                    <EnvironmentVariables>
                        <AAA>Test1</AAA>
                        <BBB>Test2</BBB>
                    </EnvironmentVariables>
                </RunConfiguration>
            </RunSettings>";
    private readonly string _runSettingsTwoEnvVarsAndDataCollectors = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <RunSettings>
                <RunConfiguration>
                    <EnvironmentVariables>
                        <AAA>Test1</AAA>
                        <BBB>Test2</BBB>
                    </EnvironmentVariables>
                </RunConfiguration>
                <DataCollectionRunSettings>
                    <DataCollectors>
                        <DataCollector friendlyName=""blame"" enabled=""True""></DataCollector>
                    </DataCollectors>
                </DataCollectionRunSettings>
            </RunSettings>";
    private readonly string _fakeRunSettings = "FakeRunSettings";
    private readonly ProtocolConfig _protocolConfig = new() { Version = 1 };
    private readonly Mock<ITestSessionEventsHandler> _mockEventsHandler;
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IMetricsCollection> _mockMetricsCollection;

    public ProxyTestSessionManagerTests()
    {
        TestSessionPool.Instance = null;

        var metrics = new Dictionary<string, object>();

        _mockEventsHandler = new Mock<ITestSessionEventsHandler>();
        _mockRequestData = new Mock<IRequestData>();
        _mockMetricsCollection = new Mock<IMetricsCollection>();

        _mockEventsHandler.Setup(
            e => e.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()))
            .Callback((StartTestSessionCompleteEventArgs eventArgs) =>
             {
                 Assert.IsNotNull(eventArgs.TestSessionInfo);
                 Assert.IsNotNull(eventArgs.Metrics);

                 Assert.IsTrue(eventArgs.Metrics.ContainsKey(TelemetryDataConstants.TestSessionId));
                 Assert.IsTrue(eventArgs.Metrics.ContainsKey(TelemetryDataConstants.TestSessionState));
                 Assert.IsTrue(
                     eventArgs.Metrics.ContainsKey(TelemetryDataConstants.TestSessionSpawnedTesthostCount)
                     && (int)eventArgs.Metrics[TelemetryDataConstants.TestSessionSpawnedTesthostCount] > 0);
                 Assert.IsTrue(eventArgs.Metrics.ContainsKey(TelemetryDataConstants.TestSessionTesthostSpawnTimeInSec));
             });

        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(_mockMetricsCollection.Object);
        _mockRequestData.Setup(rd => rd.ProtocolConfig).Returns(_protocolConfig);
        _mockMetricsCollection.Setup(mc => mc.Metrics).Returns(metrics);
        _mockMetricsCollection.Setup(mc => mc.Add(It.IsAny<string>(), It.IsAny<object>()))
            .Callback((string metric, object value) => metrics.Add(metric, value));

        _fakeTestSourcesToRuntimeProviderMap = new Dictionary<string, TestRuntimeProviderInfo>
        {
            [_fakeTestSources[0]] = new TestRuntimeProviderInfo(typeof(ITestRuntimeProvider), false, _fakeRunSettings, new List<SourceDetail>
            {
                new SourceDetail {
                    Source = _fakeTestSources[0],
                    Architecture = Architecture.X86,
                    Framework = Framework.DefaultFramework
                }
            })
        };
    }

    [TestMethod]
    public void StartSessionShouldSucceedIfCalledOnlyOnce()
    {
        var mockProxyOperationManager = new Mock<ProxyOperationManager>(null, null, null, null);
        mockProxyOperationManager.Setup(pom => pom.SetupChannel(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Returns(true);

        var testSessionCriteria = CreateTestSession(_fakeTestSources, _fakeRunSettings);
        var proxyManager = CreateProxy(testSessionCriteria, mockProxyOperationManager.Object);

        // First call to StartSession should succeed.
        Assert.IsTrue(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        mockProxyOperationManager.Verify(pom => pom.SetupChannel(
                testSessionCriteria.Sources!,
                testSessionCriteria.RunSettings),
            Times.Once);
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Once);

        // Second call to StartSession should fail.
        Assert.IsFalse(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        mockProxyOperationManager.Verify(pom => pom.SetupChannel(
                testSessionCriteria.Sources!,
                testSessionCriteria.RunSettings),
            Times.Once);
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Once);
    }

    [TestMethod]
    public void StartSessionShouldSucceedWhenCalledWithMultipleSources()
    {
        var mockProxyOperationManager = new Mock<ProxyOperationManager>(null, null, null, null);
        mockProxyOperationManager.Setup(pom => pom.SetupChannel(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Returns(true);

        var testSessionCriteria = CreateTestSession(_fakeTestMultipleSources, _fakeRunSettings);
        var proxyManager = CreateProxy(testSessionCriteria, mockProxyOperationManager.Object);

        // First call to StartSession should succeed.
        Assert.IsTrue(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        mockProxyOperationManager.Verify(pom => pom.SetupChannel(
                It.IsAny<IEnumerable<string>>(),
                testSessionCriteria.RunSettings),
            Times.Exactly(_fakeTestMultipleSources.Count));
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Once);
    }

    [TestMethod]
    public void StartSessionShouldFailIfProxyCreatorIsNull()
    {
        var testSessionCriteria = CreateTestSession(_fakeTestSources, _fakeRunSettings);
        var proxyManager = CreateProxy(testSessionCriteria, null!);

        Assert.IsFalse(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Never);
    }

    [TestMethod]
    public void StartSessionShouldFailIfSetupChannelReturnsFalse()
    {
        var mockProxyOperationManager = new Mock<ProxyOperationManager>(null, null, null, null);
        mockProxyOperationManager.Setup(pom => pom.SetupChannel(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Returns(false);
        mockProxyOperationManager.Setup(pom => pom.Close()).Callback(() => { });

        var testSessionCriteria = CreateTestSession(_fakeTestSources, _fakeRunSettings);
        var proxyManager = CreateProxy(testSessionCriteria, mockProxyOperationManager.Object);

        // Call fails because SetupChannel returns false.
        Assert.IsFalse(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        mockProxyOperationManager.Verify(pom => pom.SetupChannel(
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<string>()),
            Times.Once);
        mockProxyOperationManager.Verify(pom => pom.Close(), Times.Never);
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Never);
    }

    [TestMethod]
    public void StartSessionShouldNotFailIfSetupChannelReturnsFalseButTheProxyDisposalPolicyAllowsFailures()
    {
        var mockProxyOperationManager = new Mock<ProxyOperationManager>(null, null, null, null);
        mockProxyOperationManager.SetupSequence(pom => pom.SetupChannel(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Returns(true)
            .Returns(false)
            .Returns(false)
            .Returns(false)
            .Returns(false)
            .Returns(false)
            .Returns(false)
            .Returns(false);
        mockProxyOperationManager.Setup(pom => pom.Close()).Callback(() => { });

        var testSessionCriteria = CreateTestSession(_fakeTestMultipleSources, _fakeRunSettings);
        var proxyManager = CreateProxy(testSessionCriteria, mockProxyOperationManager.Object);
        proxyManager.DisposalPolicy = ProxyDisposalOnCreationFailPolicy.AllowProxySetupFailures;

        // Call fails because SetupChannel returns false.
        Assert.IsTrue(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        mockProxyOperationManager.Verify(pom => pom.SetupChannel(
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<string>()),
            Times.Exactly(_fakeTestMultipleSources.Count));
        mockProxyOperationManager.Verify(pom => pom.Close(), Times.Never);
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Once);
    }

    [TestMethod]
    public void StartSessionShouldStillFailIfSetupChannelReturnsFalseAndTheProxyDisposalPolicyAllowsFailuresButNoTesthostIsSpawned()
    {
        var mockProxyOperationManager = new Mock<ProxyOperationManager>(null, null, null, null);
        mockProxyOperationManager.Setup(pom => pom.SetupChannel(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Returns(false);
        mockProxyOperationManager.Setup(pom => pom.Close()).Callback(() => { });

        var testSessionCriteria = CreateTestSession(_fakeTestSources, _fakeRunSettings);
        var proxyManager = CreateProxy(testSessionCriteria, mockProxyOperationManager.Object);
        proxyManager.DisposalPolicy = ProxyDisposalOnCreationFailPolicy.AllowProxySetupFailures;

        // Call fails because SetupChannel returns false.
        Assert.IsFalse(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        mockProxyOperationManager.Verify(pom => pom.SetupChannel(
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<string>()),
            Times.Exactly(_fakeTestSources.Count));
        mockProxyOperationManager.Verify(pom => pom.Close(), Times.Never);
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Never);
    }

    [TestMethod]
    public void StartSessionShouldFailIfSetupChannelThrowsException()
    {
        var mockProxyOperationManager = new Mock<ProxyOperationManager>(null, null, null, null);
        mockProxyOperationManager.Setup(pom => pom.SetupChannel(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Throws(new TestPlatformException("Dummy exception."));
        mockProxyOperationManager.Setup(pom => pom.Close()).Callback(() => { });

        var testSessionCriteria = CreateTestSession(_fakeTestSources, _fakeRunSettings);
        var proxyManager = CreateProxy(testSessionCriteria, mockProxyOperationManager.Object);

        // Call fails because SetupChannel returns false.
        Assert.IsFalse(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        mockProxyOperationManager.Verify(pom => pom.SetupChannel(
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<string>()),
            Times.Once);
        mockProxyOperationManager.Verify(pom => pom.Close(), Times.Never);
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Never);
    }

    [TestMethod]
    public void StartSessionShouldFailIfAddSessionFails()
    {
        var mockTestSessionPool = new Mock<TestSessionPool>();
        mockTestSessionPool.Setup(tsp => tsp.AddSession(It.IsAny<TestSessionInfo>(), It.IsAny<ProxyTestSessionManager>()))
            .Returns(false);
        TestSessionPool.Instance = mockTestSessionPool.Object;

        var mockProxyOperationManager = new Mock<ProxyOperationManager>(null, null, null, null);
        mockProxyOperationManager.Setup(pom => pom.SetupChannel(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Returns(true);
        mockProxyOperationManager.Setup(pom => pom.Close()).Callback(() => { });

        var testSessionCriteria = CreateTestSession(_fakeTestSources, _fakeRunSettings);
        var proxyManager = CreateProxy(testSessionCriteria, mockProxyOperationManager.Object);

        // Call to StartSession should fail because AddSession fails.
        Assert.IsFalse(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        mockProxyOperationManager.Verify(pom => pom.SetupChannel(
                testSessionCriteria.Sources!,
                testSessionCriteria.RunSettings),
            Times.Once);
        mockProxyOperationManager.Verify(pom => pom.Close(), Times.Once);
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Never);
    }

    [TestMethod]
    public void StopSessionShouldSucceedIfCalledOnlyOnce()
    {
        var mockProxyOperationManager = new Mock<ProxyOperationManager>(null, null, null, null);
        mockProxyOperationManager.Setup(pom => pom.SetupChannel(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Returns(true);
        mockProxyOperationManager.Setup(pom => pom.Close()).Callback(() => { });

        var testSessionCriteria = CreateTestSession(_fakeTestSources, _fakeRunSettings);
        var proxyManager = CreateProxy(testSessionCriteria, mockProxyOperationManager.Object);

        // StartSession should succeed.
        Assert.IsTrue(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        mockProxyOperationManager.Verify(pom => pom.SetupChannel(
                testSessionCriteria.Sources!,
                testSessionCriteria.RunSettings),
            Times.Once);
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Once);

        // First call to StopSession should succeed.
        _mockMetricsCollection.Object.Metrics.Clear();
        Assert.IsTrue(proxyManager.StopSession(_mockRequestData.Object));

        mockProxyOperationManager.Verify(pom => pom.Close(), Times.Once);
        CheckStopSessionTelemetry(true);

        // Second call to StopSession should fail.
        _mockMetricsCollection.Object.Metrics.Clear();
        Assert.IsFalse(proxyManager.StopSession(_mockRequestData.Object));

        mockProxyOperationManager.Verify(pom => pom.Close(), Times.Once);
        CheckStopSessionTelemetry(false);
    }

    [TestMethod]
    public void StopSessionShouldSucceedWhenCalledWithMultipleSources()
    {
        var mockProxyOperationManager = new Mock<ProxyOperationManager>(null, null, null, null);
        mockProxyOperationManager.Setup(pom => pom.SetupChannel(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Returns(true);
        mockProxyOperationManager.Setup(pom => pom.Close()).Callback(() => { });

        var testSessionCriteria = CreateTestSession(_fakeTestMultipleSources, _fakeRunSettings);
        var proxyManager = CreateProxy(testSessionCriteria, mockProxyOperationManager.Object);

        // StartSession should succeed.
        Assert.IsTrue(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        mockProxyOperationManager.Verify(pom => pom.SetupChannel(
                It.IsAny<IEnumerable<string>>(),
                testSessionCriteria.RunSettings),
            Times.Exactly(testSessionCriteria.Sources!.Count));
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Once);

        // First call to StopSession should succeed.
        _mockMetricsCollection.Object.Metrics.Clear();
        Assert.IsTrue(proxyManager.StopSession(_mockRequestData.Object));

        mockProxyOperationManager.Verify(pom => pom.Close(), Times.Exactly(testSessionCriteria.Sources.Count));
        CheckStopSessionTelemetry(true);
    }

    [TestMethod]
    public void DequeueProxyShouldSucceedIfIdentificationCriteriaAreMet()
    {
        var mockProxyOperationManager = new Mock<ProxyOperationManager>(null, null, null, null);
        mockProxyOperationManager.Setup(pom => pom.SetupChannel(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Returns(true);

        var testSessionCriteria = CreateTestSession(_fakeTestSources, _runSettingsNoEnvVars);
        var proxyManager = CreateProxy(testSessionCriteria, mockProxyOperationManager.Object);

        // StartSession should succeed.
        Assert.IsTrue(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        mockProxyOperationManager.Verify(pom => pom.SetupChannel(
                It.IsAny<IEnumerable<string>>(),
                testSessionCriteria.RunSettings),
            Times.Exactly(testSessionCriteria.Sources!.Count));
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Once);

        // First call to DequeueProxy fails because of source mismatch.
        Assert.ThrowsException<InvalidOperationException>(() => proxyManager.DequeueProxy(
            @"C:\temp\FakeTestAsset2.dll",
            testSessionCriteria.RunSettings));

        // Second call to DequeueProxy fails because of runsettings mismatch.
        Assert.ThrowsException<InvalidOperationException>(() => proxyManager.DequeueProxy(
            testSessionCriteria.Sources[0],
            _runSettingsOneEnvVar));

        // Third call to DequeueProxy succeeds.
        Assert.AreEqual(proxyManager.DequeueProxy(
                testSessionCriteria.Sources[0],
                testSessionCriteria.RunSettings),
            mockProxyOperationManager.Object);

        // Fourth call to DequeueProxy fails because proxy became unavailable following successful deque.
        Assert.ThrowsException<InvalidOperationException>(() => proxyManager.DequeueProxy(
            testSessionCriteria.Sources[0],
            testSessionCriteria.RunSettings));
    }

    [TestMethod]
    public void DequeueProxyTwoConsecutiveTimesWithEnqueueShouldBeSuccessful()
    {
        var mockProxyOperationManager = new Mock<ProxyOperationManager>(null, null, null, null);
        mockProxyOperationManager.Setup(pom => pom.SetupChannel(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Returns(true);

        var testSessionCriteria = CreateTestSession(_fakeTestSources, _runSettingsTwoEnvVars);
        var proxyManager = CreateProxy(testSessionCriteria, mockProxyOperationManager.Object);

        // StartSession should succeed.
        Assert.IsTrue(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        mockProxyOperationManager.Verify(pom => pom.SetupChannel(
                It.IsAny<IEnumerable<string>>(),
                testSessionCriteria.RunSettings),
            Times.Exactly(testSessionCriteria.Sources!.Count));
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Once);

        // Call to DequeueProxy succeeds.
        Assert.AreEqual(proxyManager.DequeueProxy(
                testSessionCriteria.Sources[0],
                testSessionCriteria.RunSettings),
            mockProxyOperationManager.Object);

        Assert.AreEqual(proxyManager.EnqueueProxy(mockProxyOperationManager.Object.Id), true);

        // Call to DequeueProxy succeeds when called with the same runsettings as before.
        Assert.AreEqual(proxyManager.DequeueProxy(
                testSessionCriteria.Sources[0],
                testSessionCriteria.RunSettings),
            mockProxyOperationManager.Object);
    }

    [TestMethod]
    public void DequeueProxyShouldFailIfRunSettingsMatchingFails()
    {
        var mockProxyOperationManager = new Mock<ProxyOperationManager>(null, null, null, null);
        mockProxyOperationManager.Setup(pom => pom.SetupChannel(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Returns(true);

        var testSessionCriteria = CreateTestSession(_fakeTestSources, _runSettingsOneEnvVar);
        var proxyManager = CreateProxy(testSessionCriteria, mockProxyOperationManager.Object);

        // StartSession should succeed.
        Assert.IsTrue(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        mockProxyOperationManager.Verify(pom => pom.SetupChannel(
                It.IsAny<IEnumerable<string>>(),
                testSessionCriteria.RunSettings),
            Times.Exactly(testSessionCriteria.Sources!.Count));
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Once);

        // This call to DequeueProxy fails because of runsettings mismatch.
        Assert.ThrowsException<InvalidOperationException>(() => proxyManager.DequeueProxy(
            testSessionCriteria.Sources[0],
            _runSettingsTwoEnvVars));
    }

    [TestMethod]
    public void DequeueProxyShouldFailIfRunSettingsMatchingFailsFor2EnvVariables()
    {
        var mockProxyOperationManager = new Mock<ProxyOperationManager>(null, null, null, null);
        mockProxyOperationManager.Setup(pom => pom.SetupChannel(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Returns(true);

        var testSessionCriteria = CreateTestSession(_fakeTestSources, _runSettingsTwoEnvVars);
        var proxyManager = CreateProxy(testSessionCriteria, mockProxyOperationManager.Object);

        // StartSession should succeed.
        Assert.IsTrue(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        mockProxyOperationManager.Verify(pom => pom.SetupChannel(
                It.IsAny<IEnumerable<string>>(),
                testSessionCriteria.RunSettings),
            Times.Exactly(testSessionCriteria.Sources!.Count));
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Once);

        // This call to DequeueProxy fails because of runsettings mismatch.
        Assert.ThrowsException<InvalidOperationException>(() => proxyManager.DequeueProxy(
            testSessionCriteria.Sources[0],
            _runSettingsOneEnvVar));
    }

    [TestMethod]
    public void DequeueProxyShouldFailIfRunSettingsMatchingFailsForDataCollectors()
    {
        var mockProxyOperationManager = new Mock<ProxyOperationManager>(null, null, null, null);
        mockProxyOperationManager.Setup(pom => pom.SetupChannel(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Returns(true);

        var testSessionCriteria = CreateTestSession(_fakeTestSources, _runSettingsTwoEnvVars);
        var proxyManager = CreateProxy(testSessionCriteria, mockProxyOperationManager.Object);

        // StartSession should succeed.
        Assert.IsTrue(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        mockProxyOperationManager.Verify(pom => pom.SetupChannel(
                It.IsAny<IEnumerable<string>>(),
                testSessionCriteria.RunSettings),
            Times.Exactly(testSessionCriteria.Sources!.Count));
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Once);

        // This call to DequeueProxy fails because of runsettings mismatch.
        Assert.ThrowsException<InvalidOperationException>(() => proxyManager.DequeueProxy(
            testSessionCriteria.Sources[0],
            _runSettingsTwoEnvVarsAndDataCollectors));
    }

    [TestMethod]
    public void EnqueueProxyShouldSucceedIfIdentificationCriteriaAreMet()
    {
        var mockProxyOperationManager = new Mock<ProxyOperationManager>(null, null, null, null);
        mockProxyOperationManager.Setup(pom => pom.SetupChannel(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Returns(true);

        var testSessionCriteria = CreateTestSession(_fakeTestSources, _runSettingsNoEnvVars);
        var proxyManager = CreateProxy(testSessionCriteria, mockProxyOperationManager.Object);

        // Validate sanity checks.
        Assert.ThrowsException<ArgumentException>(() => proxyManager.EnqueueProxy(-1));
        Assert.ThrowsException<ArgumentException>(() => proxyManager.EnqueueProxy(1));

        // StartSession should succeed.
        Assert.IsTrue(proxyManager.StartSession(_mockEventsHandler.Object, _mockRequestData.Object));
        mockProxyOperationManager.Verify(pom => pom.SetupChannel(
                It.IsAny<IEnumerable<string>>(),
                testSessionCriteria.RunSettings),
            Times.Exactly(testSessionCriteria.Sources!.Count));
        _mockEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Once);

        // Call throws exception because proxy is already available.
        Assert.ThrowsException<InvalidOperationException>(() => proxyManager.EnqueueProxy(0));

        // Call succeeds.
        Assert.AreEqual(proxyManager.DequeueProxy(
                testSessionCriteria.Sources[0],
                testSessionCriteria.RunSettings),
            mockProxyOperationManager.Object);
        Assert.IsTrue(proxyManager.EnqueueProxy(0));
    }

    private static StartTestSessionCriteria CreateTestSession(IList<string> sources, string runSettings)
    {
        return new StartTestSessionCriteria()
        {
            Sources = sources,
            RunSettings = runSettings
        };
    }

    private ProxyTestSessionManager CreateProxy(
        StartTestSessionCriteria testSessionCriteria,
        ProxyOperationManager proxyOperationManager)
    {
        var runSettings = testSessionCriteria.RunSettings ?? _fakeRunSettings;
        var runtimeProviderInfo = new TestRuntimeProviderInfo
        (
            typeof(ITestRuntimeProvider),
            shared: false,
            runSettings,
            testSessionCriteria.Sources!.Select(s => new SourceDetail
            {
                Source = s,
                Architecture = Architecture.X86,
                Framework = Framework.DefaultFramework
            }).ToList()
        );

        var runtimeProviders = new List<TestRuntimeProviderInfo> { runtimeProviderInfo };
        return new ProxyTestSessionManager(
            testSessionCriteria,
            testSessionCriteria.Sources!.Count,
            _ => proxyOperationManager,
            runtimeProviders
            );
    }

    private void CheckStopSessionTelemetry(bool exists)
    {
        Assert.AreEqual(_mockMetricsCollection.Object.Metrics.ContainsKey(TelemetryDataConstants.TestSessionId), exists);
        Assert.AreEqual(_mockMetricsCollection.Object.Metrics.ContainsKey(TelemetryDataConstants.TestSessionState), exists);
        Assert.AreEqual(_mockMetricsCollection.Object.Metrics.ContainsKey(TelemetryDataConstants.TestSessionTotalSessionTimeInSec), exists);
    }
}
