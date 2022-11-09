// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.Client.Execution;
using Microsoft.VisualStudio.TestPlatform.Common.Hosting;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.Client.UnitTests;

[TestClass]
public class TestPlatformTests
{
    private readonly Mock<ITestEngine> _testEngine;
    private readonly Mock<IProxyDiscoveryManager> _discoveryManager;
    private readonly Mock<ITestExtensionManager> _extensionManager;
    private readonly Mock<ITestRuntimeProvider> _hostManager;
    private readonly Mock<IProxyExecutionManager> _executionManager;
    private readonly Mock<ITestLoggerManager> _loggerManager;
    private readonly Mock<IFileHelper> _mockFileHelper;
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IMetricsCollection> _mockMetricsCollection;

    public TestPlatformTests()
    {
        _testEngine = new Mock<ITestEngine>();
        _discoveryManager = new Mock<IProxyDiscoveryManager>();
        _extensionManager = new Mock<ITestExtensionManager>();
        _executionManager = new Mock<IProxyExecutionManager>();
        _loggerManager = new Mock<ITestLoggerManager>();
        _hostManager = new Mock<ITestRuntimeProvider>();
        _mockFileHelper = new Mock<IFileHelper>();
        _mockMetricsCollection = new Mock<IMetricsCollection>();
        _mockRequestData = new Mock<IRequestData>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(_mockMetricsCollection.Object);
    }

    [TestMethod]
    public void CreateDiscoveryRequestShouldInitializeDiscoveryManagerAndCreateDiscoveryRequestWithGivenCriteriaAndReturnIt()
    {
        _discoveryManager.Setup(dm => dm.Initialize(false)).Verifiable();
        var discoveryCriteria = new DiscoveryCriteria(new List<string> { "foo" }, 1, null);
        _hostManager.Setup(hm => hm.GetTestSources(discoveryCriteria.Sources))
            .Returns(discoveryCriteria.Sources);

        _testEngine.Setup(te => te.GetDiscoveryManager(_mockRequestData.Object, It.IsAny<DiscoveryCriteria>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(_discoveryManager.Object);
        _testEngine.Setup(te => te.GetExtensionManager()).Returns(_extensionManager.Object);
        _testEngine.Setup(te => te.GetLoggerManager(_mockRequestData.Object)).Returns(_loggerManager.Object);
        var tp = new TestableTestPlatform(_testEngine.Object, _hostManager.Object);

        var discoveryRequest = tp.CreateDiscoveryRequest(_mockRequestData.Object, discoveryCriteria, new TestPlatformOptions(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>());

        _discoveryManager.Verify(dm => dm.Initialize(false), Times.Once);
        Assert.AreEqual(discoveryCriteria, discoveryRequest.DiscoveryCriteria);
    }

    [TestMethod]
    public void CreateDiscoveryRequestShouldInitializeManagersWithFalseFlagWhenSkipDefaultAdaptersIsFalse()
    {
        var options = new TestPlatformOptions()
        {
            SkipDefaultAdapters = false
        };

        InvokeCreateDiscoveryRequest(options);

        _discoveryManager.Verify(dm => dm.Initialize(false), Times.Once);
    }

    [TestMethod]
    public void CreateDiscoveryRequestShouldInitializeManagersWithTrueFlagWhenSkipDefaultAdaptersIsTrue()
    {
        var options = new TestPlatformOptions()
        {
            SkipDefaultAdapters = true
        };

        InvokeCreateDiscoveryRequest(options);

        _discoveryManager.Verify(dm => dm.Initialize(true), Times.Once);
    }

    [TestMethod]
    public void CreateDiscoveryRequestShouldInitializeManagersWithFalseFlagWhenTestPlatformOptionsIsNull()
    {
        InvokeCreateDiscoveryRequest();

        _discoveryManager.Verify(dm => dm.Initialize(false), Times.Once);
    }

    [TestMethod]
    public void CreateDiscoveryRequestThrowsIfDiscoveryCriteriaIsNull()
    {
        TestPlatform tp = new();

        Assert.ThrowsException<ArgumentNullException>(() => tp.CreateDiscoveryRequest(_mockRequestData.Object, null!, new TestPlatformOptions(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()));
    }

    [TestMethod]
    public void UpdateExtensionsShouldUpdateTheEngineWithAdditionalExtensions()
    {
        _testEngine.Setup(te => te.GetExtensionManager()).Returns(_extensionManager.Object);
        var tp = new TestableTestPlatform(_testEngine.Object, _hostManager.Object);
        var additionalExtensions = new List<string> { "e1.dll", "e2.dll" };

        tp.UpdateExtensions(additionalExtensions, skipExtensionFilters: true);

        _extensionManager.Verify(em => em.UseAdditionalExtensions(additionalExtensions, true));
    }

    [TestMethod]
    public void ClearExtensionsShouldClearTheExtensionsCachedInEngine()
    {
        _testEngine.Setup(te => te.GetExtensionManager()).Returns(_extensionManager.Object);
        var tp = new TestableTestPlatform(_testEngine.Object, _hostManager.Object);

        tp.ClearExtensions();

        _extensionManager.Verify(em => em.ClearExtensions());
    }

    [TestMethod]
    public void CreateTestRunRequestShouldUpdateLoggerExtensionWhenDesingModeIsFalseForRunAll()
    {
        var additionalExtensions = new List<string> { "foo.TestLogger.dll", "Joo.TestLogger.dll" };
        _mockFileHelper.Setup(fh => fh.DirectoryExists(It.IsAny<string>())).Returns(true);
        _mockFileHelper.Setup(fh => fh.EnumerateFiles(It.IsAny<string>(), SearchOption.TopDirectoryOnly, It.IsAny<string[]>())).Returns(additionalExtensions);
        _executionManager.Setup(dm => dm.Initialize(false)).Verifiable();

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>false</DesignMode>
                     </RunConfiguration>
                </RunSettings>";

        var temp = Path.GetTempPath();
        var testRunCriteria = new TestRunCriteria(new List<string> { $@"{temp}foo.dll" }, 10, false, settingsXml, TimeSpan.Zero);
        _hostManager.Setup(hm => hm.GetTestSources(testRunCriteria.Sources!))
            .Returns(testRunCriteria.Sources!);

        _testEngine.Setup(te => te.GetExecutionManager(_mockRequestData.Object, It.IsAny<TestRunCriteria>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(_executionManager.Object);
        _testEngine.Setup(te => te.GetExtensionManager()).Returns(_extensionManager.Object);
        _testEngine.Setup(te => te.GetLoggerManager(_mockRequestData.Object)).Returns(_loggerManager.Object);

        var tp = new TestableTestPlatform(_testEngine.Object, _mockFileHelper.Object, _hostManager.Object);

        var testRunRequest = tp.CreateTestRunRequest(_mockRequestData.Object, testRunCriteria, new TestPlatformOptions(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>());
        _extensionManager.Verify(em => em.UseAdditionalExtensions(additionalExtensions, false));
    }

    [TestMethod]
    public void CreateTestRunRequestShouldUpdateLoggerExtensionWhenDesignModeIsFalseForRunSelected()
    {
        var additionalExtensions = new List<string> { "foo.TestLogger.dll", "Joo.TestLogger.dll" };
        _mockFileHelper.Setup(fh => fh.DirectoryExists(It.IsAny<string>())).Returns(true);
        _mockFileHelper.Setup(fh => fh.EnumerateFiles(It.IsAny<string>(), SearchOption.TopDirectoryOnly, It.IsAny<string[]>())).Returns(additionalExtensions);

        _executionManager.Setup(dm => dm.Initialize(false)).Verifiable();

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>false</DesignMode>
                     </RunConfiguration>
                </RunSettings>";

        var testRunCriteria = new TestRunCriteria(new List<TestCase> { new TestCase("dll1.class1.test1", new Uri("hello://x/"), $"xyz{Path.DirectorySeparatorChar}1.dll") }, 10, false, settingsXml);

        _testEngine.Setup(te => te.GetExecutionManager(_mockRequestData.Object, It.IsAny<TestRunCriteria>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(_executionManager.Object);
        _testEngine.Setup(te => te.GetExtensionManager()).Returns(_extensionManager.Object);
        _testEngine.Setup(te => te.GetLoggerManager(_mockRequestData.Object)).Returns(_loggerManager.Object);

        var tp = new TestableTestPlatform(_testEngine.Object, _mockFileHelper.Object, _hostManager.Object);

        var testRunRequest = tp.CreateTestRunRequest(_mockRequestData.Object, testRunCriteria, new TestPlatformOptions(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>());
        _extensionManager.Verify(em => em.UseAdditionalExtensions(additionalExtensions, false));
    }

    [TestMethod]
    public void CreateTestRunRequestShouldNotUpdateTestSourcesIfSelectedTestAreRun()
    {
        var additionalExtensions = new List<string> { "foo.TestLogger.dll", "Joo.TestLogger.dll" };
        _mockFileHelper.Setup(fh => fh.DirectoryExists(It.IsAny<string>())).Returns(true);
        _mockFileHelper.Setup(fh => fh.EnumerateFiles(It.IsAny<string>(), SearchOption.TopDirectoryOnly, It.IsAny<string[]>())).Returns(additionalExtensions);

        _executionManager.Setup(dm => dm.Initialize(false)).Verifiable();

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>false</DesignMode>
                     </RunConfiguration>
                </RunSettings>";

        var testRunCriteria = new TestRunCriteria(new List<TestCase> { new TestCase("dll1.class1.test1", new Uri("hello://x/"), $"xyz{Path.DirectorySeparatorChar}1.dll") }, 10, false, settingsXml);
        _hostManager.Setup(hm => hm.GetTestSources(It.IsAny<IEnumerable<string>>()))
            .Returns(new List<string> { $"xyz{Path.DirectorySeparatorChar}1.dll" });

        _testEngine.Setup(te => te.GetExecutionManager(_mockRequestData.Object, It.IsAny<TestRunCriteria>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(_executionManager.Object);
        _testEngine.Setup(te => te.GetExtensionManager()).Returns(_extensionManager.Object);
        _testEngine.Setup(te => te.GetLoggerManager(_mockRequestData.Object)).Returns(_loggerManager.Object);

        var tp = new TestableTestPlatform(_testEngine.Object, _mockFileHelper.Object, _hostManager.Object);

        tp.CreateTestRunRequest(_mockRequestData.Object, testRunCriteria, new TestPlatformOptions(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>());
        _extensionManager.Verify(em => em.UseAdditionalExtensions(additionalExtensions, false));
        _hostManager.Verify(hm => hm.GetTestSources(It.IsAny<IEnumerable<string>>()), Times.Never);
    }

    [TestMethod]
    public void CreateTestRunRequestShouldInitializeManagersAndCreateTestRunRequestWithSpecifiedCriteria()
    {
        _executionManager.Setup(dm => dm.Initialize(false)).Verifiable();
        _testEngine.Setup(te => te.GetExecutionManager(_mockRequestData.Object, It.IsAny<TestRunCriteria>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(_executionManager.Object);
        _testEngine.Setup(te => te.GetExtensionManager()).Returns(_extensionManager.Object);
        _testEngine.Setup(te => te.GetLoggerManager(_mockRequestData.Object)).Returns(_loggerManager.Object);

        var tp = new TestableTestPlatform(_testEngine.Object, _hostManager.Object);
        var testRunCriteria = new TestRunCriteria(new List<string> { "foo" }, 10);
        _hostManager.Setup(hm => hm.GetTestSources(testRunCriteria.Sources!))
            .Returns(testRunCriteria.Sources!);

        var testRunRequest = tp.CreateTestRunRequest(_mockRequestData.Object, testRunCriteria, new TestPlatformOptions(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>());

        var actualTestRunRequest = testRunRequest as TestRunRequest;

        _executionManager.Verify(em => em.Initialize(false), Times.Once);
        Assert.AreEqual(testRunCriteria, actualTestRunRequest?.TestRunCriteria);
    }

    [TestMethod]
    public void CreateTestRunRequestShouldInitializeManagersWithFalseFlagWhenSkipDefaultAdaptersIsFalse()
    {
        var options = new TestPlatformOptions()
        {
            SkipDefaultAdapters = false
        };

        InvokeCreateTestRunRequest(options);

        _executionManager.Verify(dm => dm.Initialize(false), Times.Once);
    }

    [TestMethod]
    public void CreateTestRunRequestShouldInitializeManagersWithTrueFlagWhenSkipDefaultAdaptersIsTrue()
    {
        var options = new TestPlatformOptions()
        {
            SkipDefaultAdapters = true
        };

        InvokeCreateTestRunRequest(options);

        _executionManager.Verify(dm => dm.Initialize(true), Times.Once);
    }

    [TestMethod]
    public void CreateTestRunRequestShouldInitializeManagersWithFalseFlagWhenTestPlatformOptionsIsNull()
    {
        InvokeCreateTestRunRequest();

        _executionManager.Verify(dm => dm.Initialize(false), Times.Once);
    }

    [TestMethod]
    public void CreateTestRunRequestThrowsIfTestRunCriteriaIsNull()
    {
        var tp = new TestPlatform();

        Assert.ThrowsException<ArgumentNullException>(() => tp.CreateTestRunRequest(_mockRequestData.Object, null!, new TestPlatformOptions(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()));
    }

    /// <summary>
    /// Logger extensions should be updated when design mode is false.
    /// </summary>
    [TestMethod]
    public void CreateDiscoveryRequestShouldUpdateLoggerExtensionWhenDesignModeIsFalse()
    {
        var additionalExtensions = new List<string> { "foo.TestLogger.dll", "Joo.TestLogger.dll" };
        _mockFileHelper.Setup(fh => fh.DirectoryExists(It.IsAny<string>())).Returns(true);
        _mockFileHelper.Setup(fh => fh.EnumerateFiles(It.IsAny<string>(), SearchOption.TopDirectoryOnly, It.IsAny<string[]>())).Returns(additionalExtensions);

        _discoveryManager.Setup(dm => dm.Initialize(false)).Verifiable();

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>false</DesignMode>
                     </RunConfiguration>
                </RunSettings>";

        var temp = Path.GetTempPath();
        var discoveryCriteria = new DiscoveryCriteria(new List<string> { $@"{temp}foo.dll" }, 1, settingsXml);
        _hostManager.Setup(hm => hm.GetTestSources(discoveryCriteria.Sources))
            .Returns(discoveryCriteria.Sources);

        _testEngine.Setup(te => te.GetDiscoveryManager(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(_discoveryManager.Object);
        _testEngine.Setup(te => te.GetExtensionManager()).Returns(_extensionManager.Object);
        _testEngine.Setup(te => te.GetLoggerManager(_mockRequestData.Object)).Returns(_loggerManager.Object);
        var tp = new TestableTestPlatform(_testEngine.Object, _mockFileHelper.Object, _hostManager.Object);

        // Action
        var discoveryRequest = tp.CreateDiscoveryRequest(_mockRequestData.Object, discoveryCriteria, new TestPlatformOptions(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>());

        // Verify
        _extensionManager.Verify(em => em.UseAdditionalExtensions(additionalExtensions, false));
    }

    /// <summary>
    /// Create test run request should initialize logger manager for design mode.
    /// </summary>
    [TestMethod]
    public void CreateTestRunRequestShouldInitializeLoggerManagerForDesignMode()
    {
        _testEngine.Setup(te => te.GetExecutionManager(_mockRequestData.Object, It.IsAny<TestRunCriteria>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(_executionManager.Object);
        _testEngine.Setup(te => te.GetLoggerManager(_mockRequestData.Object)).Returns(_loggerManager.Object);

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>True</DesignMode>
                     </RunConfiguration>
                </RunSettings>";
        var testRunCriteria = new TestRunCriteria(new List<string> { @"x:dummy\foo.dll" }, 10, false, settingsXml);

        var tp = new TestableTestPlatform(_testEngine.Object, _hostManager.Object);
        tp.CreateTestRunRequest(_mockRequestData.Object, testRunCriteria, new TestPlatformOptions(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>());

        _loggerManager.Verify(lm => lm.Initialize(settingsXml));
    }

    /// <summary>
    /// Create discovery request should initialize logger manager for design mode.
    /// </summary>
    [TestMethod]
    public void CreateDiscoveryRequestShouldInitializeLoggerManagerForDesignMode()
    {
        _testEngine.Setup(te => te.GetDiscoveryManager(_mockRequestData.Object, It.IsAny<DiscoveryCriteria>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(_discoveryManager.Object);
        _testEngine.Setup(te => te.GetLoggerManager(_mockRequestData.Object)).Returns(_loggerManager.Object);

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>True</DesignMode>
                     </RunConfiguration>
                </RunSettings>";
        var discoveryCriteria = new DiscoveryCriteria(new List<string> { @"x:dummy\foo.dll" }, 10, settingsXml);

        var tp = new TestableTestPlatform(_testEngine.Object, _hostManager.Object);
        tp.CreateDiscoveryRequest(_mockRequestData.Object, discoveryCriteria, new TestPlatformOptions(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>());

        _loggerManager.Verify(lm => lm.Initialize(settingsXml));
    }

    /// <summary>
    /// Create test run request should initialize logger manager for design mode.
    /// </summary>
    [TestMethod]
    public void CreateTestRunRequestShouldInitializeLoggerManagerForNonDesignMode()
    {
        _testEngine.Setup(te => te.GetExecutionManager(_mockRequestData.Object, It.IsAny<TestRunCriteria>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(_executionManager.Object);
        _testEngine.Setup(te => te.GetLoggerManager(_mockRequestData.Object)).Returns(_loggerManager.Object);

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>False</DesignMode>
                     </RunConfiguration>
                </RunSettings>";
        var testRunCriteria = new TestRunCriteria(new List<string> { "foo" }, 10, false, settingsXml);

        var tp = new TestableTestPlatform(_testEngine.Object, _hostManager.Object);
        tp.CreateTestRunRequest(_mockRequestData.Object, testRunCriteria, new TestPlatformOptions(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>());

        _loggerManager.Verify(lm => lm.Initialize(settingsXml));
    }

    /// <summary>
    /// Create discovery request should initialize logger manager for design mode.
    /// </summary>
    [TestMethod]
    public void CreateDiscoveryRequestShouldInitializeLoggerManagerForNonDesignMode()
    {
        _testEngine.Setup(te => te.GetDiscoveryManager(_mockRequestData.Object, It.IsAny<DiscoveryCriteria>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(_discoveryManager.Object);
        _testEngine.Setup(te => te.GetLoggerManager(_mockRequestData.Object)).Returns(_loggerManager.Object);

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>False</DesignMode>
                     </RunConfiguration>
                </RunSettings>";
        var discoveryCriteria = new DiscoveryCriteria(new List<string> { "foo" }, 10, settingsXml);

        var tp = new TestableTestPlatform(_testEngine.Object, _hostManager.Object);
        tp.CreateDiscoveryRequest(_mockRequestData.Object, discoveryCriteria, new TestPlatformOptions(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>());

        _loggerManager.Verify(lm => lm.Initialize(settingsXml));
    }

    [TestMethod]
    public void StartTestSessionShouldThrowExceptionIfTestSessionCriteriaIsNull()
    {
        var tp = new TestableTestPlatform(_testEngine.Object, _hostManager.Object);

        Assert.ThrowsException<ArgumentNullException>(() =>
            tp.StartTestSession(
                new Mock<IRequestData>().Object,
                null!,
                new Mock<ITestSessionEventsHandler>().Object,
                new Dictionary<string, SourceDetail>(),
                new Mock<IWarningLogger>().Object));
    }

    [TestMethod]
    public void StartTestSessionShouldReturnFalseIfDesignModeIsDisabled()
    {
        var tp = new TestableTestPlatform(_testEngine.Object, _hostManager.Object);

        var testSessionCriteria = new StartTestSessionCriteria()
        {
            RunSettings = @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <RunSettings>
                         <RunConfiguration>
                           <DesignMode>false</DesignMode>
                         </RunConfiguration>
                    </RunSettings>"
        };

        Assert.IsFalse(
            tp.StartTestSession(
                new Mock<IRequestData>().Object,
                testSessionCriteria,
                new Mock<ITestSessionEventsHandler>().Object,
                It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()));
    }

    [TestMethod]
    public void StartTestSessionShouldReturnFalseIfTestSessionManagerIsNull()
    {
        _testEngine.Setup(
                te => te.GetTestSessionManager(
                    It.IsAny<IRequestData>(),
                    It.IsAny<StartTestSessionCriteria>(),
                    It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()))
            .Returns((IProxyTestSessionManager)null!);

        var tp = new TestableTestPlatform(_testEngine.Object, _hostManager.Object);
        var mockEventsHandler = new Mock<ITestSessionEventsHandler>();

        mockEventsHandler.Setup(
            eh => eh.HandleStartTestSessionComplete(
                It.IsAny<StartTestSessionCompleteEventArgs>()))
            .Callback((StartTestSessionCompleteEventArgs eventArgs) =>
            {
                Assert.IsNull(eventArgs.TestSessionInfo);
                Assert.IsNull(eventArgs.Metrics);
            });

        var testSessionCriteria = new StartTestSessionCriteria()
        {
            RunSettings = @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <RunSettings>
                         <RunConfiguration>
                           <DesignMode>true</DesignMode>
                         </RunConfiguration>
                    </RunSettings>"
        };

        Assert.IsFalse(
            tp.StartTestSession(
                new Mock<IRequestData>().Object,
                testSessionCriteria,
                mockEventsHandler.Object,
                It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()));

        mockEventsHandler.Verify(
            eh => eh.HandleStartTestSessionComplete(It.IsAny<StartTestSessionCompleteEventArgs>()),
            Times.Once);
    }

    [TestMethod]
    public void StartTestSessionShouldReturnTrueIfTestSessionManagerStartSessionReturnsTrue()
    {
        var tp = new TestableTestPlatform(_testEngine.Object, _hostManager.Object);

        var testSessionCriteria = new StartTestSessionCriteria()
        {
            RunSettings = @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <RunSettings>
                         <RunConfiguration>
                           <DesignMode>true</DesignMode>
                         </RunConfiguration>
                    </RunSettings>"
        };

        var mockEventsHandler = new Mock<ITestSessionEventsHandler>();
        var mockRequestData = new Mock<IRequestData>();
        var mockTestSessionManager = new Mock<IProxyTestSessionManager>();
        mockTestSessionManager.Setup(
                tsm => tsm.StartSession(
                    It.IsAny<ITestSessionEventsHandler>(),
                    It.IsAny<IRequestData>()))
            .Returns(true);
        _testEngine.Setup(
                te => te.GetTestSessionManager(
                    It.IsAny<IRequestData>(),
                    It.IsAny<StartTestSessionCriteria>(),
                    It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()))
            .Returns(mockTestSessionManager.Object);

        Assert.IsTrue(
            tp.StartTestSession(
                new Mock<IRequestData>().Object,
                testSessionCriteria,
                mockEventsHandler.Object,
                It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()));

        mockTestSessionManager.Verify(
            tsm => tsm.StartSession(mockEventsHandler.Object, It.IsAny<IRequestData>()),
            Times.Once);
    }

    [TestMethod]
    public void StartTestSessionShouldReturnFalseIfTestSessionManagerStartSessionReturnsFalse()
    {
        var tp = new TestableTestPlatform(_testEngine.Object, _hostManager.Object);

        var testSessionCriteria = new StartTestSessionCriteria()
        {
            RunSettings = @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <RunSettings>
                         <RunConfiguration>
                           <DesignMode>true</DesignMode>
                         </RunConfiguration>
                    </RunSettings>"
        };

        var mockEventsHandler = new Mock<ITestSessionEventsHandler>();
        var mockRequestData = new Mock<IRequestData>();
        var mockTestSessionManager = new Mock<IProxyTestSessionManager>();
        mockTestSessionManager.Setup(
                tsm => tsm.StartSession(
                    It.IsAny<ITestSessionEventsHandler>(),
                    It.IsAny<IRequestData>()))
            .Returns(false);
        _testEngine.Setup(
                te => te.GetTestSessionManager(
                    It.IsAny<IRequestData>(),
                    It.IsAny<StartTestSessionCriteria>(),
                    It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()))
            .Returns(mockTestSessionManager.Object);

        Assert.IsFalse(
            tp.StartTestSession(
                mockRequestData.Object,
                testSessionCriteria,
                mockEventsHandler.Object,
                It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()));

        mockTestSessionManager.Verify(
            tsm => tsm.StartSession(mockEventsHandler.Object, mockRequestData.Object),
            Times.Once);
    }

    private void InvokeCreateDiscoveryRequest(TestPlatformOptions? options = null)
    {
        _discoveryManager.Setup(dm => dm.Initialize(false)).Verifiable();
        var discoveryCriteria = new DiscoveryCriteria(new List<string> { "foo" }, 1, null);
        _hostManager.Setup(hm => hm.GetTestSources(discoveryCriteria.Sources))
            .Returns(discoveryCriteria.Sources);

        _testEngine.Setup(te => te.GetDiscoveryManager(_mockRequestData.Object, It.IsAny<DiscoveryCriteria>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(_discoveryManager.Object);
        _testEngine.Setup(te => te.GetExtensionManager()).Returns(_extensionManager.Object);
        _testEngine.Setup(te => te.GetLoggerManager(_mockRequestData.Object)).Returns(_loggerManager.Object);
        var tp = new TestableTestPlatform(_testEngine.Object, _hostManager.Object);

        tp.CreateDiscoveryRequest(_mockRequestData.Object, discoveryCriteria, options, new Dictionary<string, SourceDetail>(), new Mock<IWarningLogger>().Object);
    }

    private void InvokeCreateTestRunRequest(TestPlatformOptions? options = null)
    {
        _executionManager.Setup(dm => dm.Initialize(false)).Verifiable();
        _testEngine.Setup(te => te.GetExecutionManager(_mockRequestData.Object, It.IsAny<TestRunCriteria>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(_executionManager.Object);
        _testEngine.Setup(te => te.GetExtensionManager()).Returns(_extensionManager.Object);
        _testEngine.Setup(te => te.GetLoggerManager(_mockRequestData.Object)).Returns(_loggerManager.Object);

        var tp = new TestableTestPlatform(_testEngine.Object, _hostManager.Object);
        var testRunCriteria = new TestRunCriteria(new List<string> { "foo" }, 10);
        _hostManager.Setup(hm => hm.GetTestSources(testRunCriteria.Sources!))
            .Returns(testRunCriteria.Sources!);

        tp.CreateTestRunRequest(_mockRequestData.Object, testRunCriteria, options, new Dictionary<string, SourceDetail>(), new Mock<IWarningLogger>().Object);
    }

    private class TestableTestPlatform : TestPlatform
    {
        public TestableTestPlatform(ITestEngine testEngine, ITestRuntimeProvider hostProvider)
            : base(testEngine, new FileHelper(), new TestableTestRuntimeProviderManager(hostProvider))
        {
        }

        public TestableTestPlatform(ITestEngine testEngine, IFileHelper fileHelper, ITestRuntimeProvider hostProvider)
            : base(testEngine, fileHelper, new TestableTestRuntimeProviderManager(hostProvider))
        {
        }
    }

    private class TestableTestRuntimeProviderManager : TestRuntimeProviderManager
    {
        private readonly ITestRuntimeProvider _hostProvider;

        public TestableTestRuntimeProviderManager(ITestRuntimeProvider hostProvider)
            : base(TestSessionMessageLogger.Instance)
        {
            _hostProvider = hostProvider;
        }

        public override ITestRuntimeProvider GetTestHostManagerByRunConfiguration(string? runConfiguration, List<string>? _)
        {
            return _hostProvider;
        }
    }
}
