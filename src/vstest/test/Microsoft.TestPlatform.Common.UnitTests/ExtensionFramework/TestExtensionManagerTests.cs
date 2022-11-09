// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.Common.UnitTests.ExtensionFramework;

[TestClass]
public class TestExtensionManagerTests
{
    private readonly IMessageLogger _messageLogger;
    private TestExtensionManager<ITestLogger, ITestLoggerCapabilities>? _testExtensionManager;
    private readonly IEnumerable<LazyExtension<ITestLogger, ITestLoggerCapabilities>> _filteredTestExtensions;
    private readonly IEnumerable<LazyExtension<ITestLogger, Dictionary<string, object>>> _unfilteredTestExtensions;

    public TestExtensionManagerTests()
    {
        TestPluginCacheHelper.SetupMockExtensions(typeof(TestExtensionManagerTests));
        _messageLogger = TestSessionMessageLogger.Instance;
        TestPluginManager.GetSpecificTestExtensions<TestLoggerPluginInformation, ITestLogger, ITestLoggerCapabilities, TestLoggerMetadata>
            (TestPlatformConstants.TestLoggerEndsWithPattern, out _unfilteredTestExtensions, out _filteredTestExtensions);
    }

    [TestCleanup]
    public void Cleanup()
    {
        TestSessionMessageLogger.Instance = null;
    }

    [TestMethod]
    public void TestExtensionManagerConstructorShouldThrowExceptionIfMessageLoggerIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _testExtensionManager = new DummyTestExtensionManager(_unfilteredTestExtensions, _filteredTestExtensions, null!));
    }

    [TestMethod]
    public void TryGetTestExtensionShouldReturnExtensionWithCorrectUri()
    {
        _testExtensionManager = new DummyTestExtensionManager(_unfilteredTestExtensions, _filteredTestExtensions, _messageLogger);
        var result = _testExtensionManager.TryGetTestExtension(new Uri("testlogger://logger"));

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Value, typeof(ITestLogger));
    }

    [TestMethod]
    public void TryGetTestExtensionShouldThrowExceptionWithNullUri()
    {
        _testExtensionManager = new DummyTestExtensionManager(_unfilteredTestExtensions, _filteredTestExtensions, _messageLogger);
        TestPluginCacheHelper.SetupMockAdditionalPathExtensions(typeof(TestExtensionManagerTests));
        Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var result = _testExtensionManager.TryGetTestExtension(default(Uri)!);
            }
        );
    }

    [TestMethod]
    public void TryGetTestExtensionShouldNotReturnExtensionWithIncorrectlUri()
    {
        _testExtensionManager = new DummyTestExtensionManager(_unfilteredTestExtensions, _filteredTestExtensions, _messageLogger);
        var result = _testExtensionManager.TryGetTestExtension("");
        Assert.IsNull(result);
    }

    [TestMethod]
    public void TryGetTestExtensionWithStringUriUnitTest()
    {
        _testExtensionManager = new DummyTestExtensionManager(_unfilteredTestExtensions, _filteredTestExtensions, _messageLogger);
        var result = _testExtensionManager.TryGetTestExtension(new Uri("testlogger://logger").AbsoluteUri);

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Value, typeof(ITestLogger));
    }

    [ExtensionUri("testlogger://logger")]
    [FriendlyName("TestLoggerExtension")]
    private class ValidLogger3 : ITestLogger
    {
        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {
            events.TestRunMessage += TestMessageHandler;
            events.TestRunComplete += Events_TestRunComplete;
            events.TestResult += Events_TestResult;
        }

        private void Events_TestResult(object? sender, TestResultEventArgs e)
        {
        }

        private void Events_TestRunComplete(object? sender, TestRunCompleteEventArgs e)
        {
        }

        private void TestMessageHandler(object? sender, TestRunMessageEventArgs e)
        {
        }
    }
}

internal class DummyTestExtensionManager : TestExtensionManager<ITestLogger, ITestLoggerCapabilities>
{
    public DummyTestExtensionManager(IEnumerable<LazyExtension<ITestLogger, Dictionary<string, object>>> unfilteredTestExtensions, IEnumerable<LazyExtension<ITestLogger, ITestLoggerCapabilities>> testExtensions, IMessageLogger logger) : base(unfilteredTestExtensions, testExtensions, logger)
    {
    }
}
