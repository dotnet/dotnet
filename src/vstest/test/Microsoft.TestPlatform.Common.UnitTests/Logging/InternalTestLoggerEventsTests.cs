// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;

namespace Microsoft.TestPlatform.Common.UnitTests.Logging;

[TestClass]
public class InternalTestLoggerEventsBehaviors
{
    private readonly TestSessionMessageLogger _testSessionMessageLogger;
    private readonly InternalTestLoggerEvents _loggerEvents;

    public InternalTestLoggerEventsBehaviors()
    {
        _testSessionMessageLogger = TestSessionMessageLogger.Instance;
        _loggerEvents = new InternalTestLoggerEvents(_testSessionMessageLogger);
    }

    [TestCleanup]
    public void Dispose()
    {
        _loggerEvents.Dispose();
        TestSessionMessageLogger.Instance = null;
    }

    [TestMethod]
    public void RaiseTestRunMessageShouldNotThrowExceptionIfNoEventHandlersAreRegistered()
    {
        // Send the test message event.
        _loggerEvents.RaiseTestRunMessage(new TestRunMessageEventArgs(TestMessageLevel.Informational, "This is a string."));
    }

    [TestMethod]
    public void RaiseTestRunMessageShouldInvokeRegisteredEventHandlerIfTestRunMessageEventArgsIsPassed()
    {
        EventWaitHandle waitHandle = new AutoResetEvent(false);
        bool testMessageReceived = false;
        TestRunMessageEventArgs? eventArgs = null;
        var message = "This is the test message";

        // Register for the test message event.
        _loggerEvents.TestRunMessage += (sender, e) =>
        {
            testMessageReceived = true;
            eventArgs = e;
            waitHandle.Set();
        };

        _loggerEvents.EnableEvents();
        // Send the test message event.
        _loggerEvents.RaiseTestRunMessage(new TestRunMessageEventArgs(TestMessageLevel.Informational, message));

        var waitSuccess = waitHandle.WaitOne(500);
        Assert.IsTrue(waitSuccess, "Event must be raised within timeout.");

        Assert.IsTrue(testMessageReceived);
        Assert.IsNotNull(eventArgs);
        Assert.AreEqual(message, eventArgs.Message);
        Assert.AreEqual(TestMessageLevel.Informational, eventArgs.Level);
    }

    [TestMethod]
    public void RaiseTestResultShouldInvokeRegisteredEventHandlerIfTestResultEventArgsIsPassed()
    {
        EventWaitHandle waitHandle = new AutoResetEvent(false);
        bool testResultReceived = false;
        TestResultEventArgs? eventArgs = null;
        var result = new TestResult(new TestCase("This is a string.", new Uri("some://uri"), "DummySourceFileName"));

        // Register for the test result event.
        _loggerEvents.TestResult += (sender, e) =>
        {
            testResultReceived = true;
            eventArgs = e;
            waitHandle.Set();
        };

        _loggerEvents.EnableEvents();
        // Send the test result event.
        _loggerEvents.RaiseTestResult(new TestResultEventArgs(result));

        var waitSuccess = waitHandle.WaitOne(500);
        Assert.IsTrue(waitSuccess, "Event must be raised within timeout.");

        Assert.IsTrue(testResultReceived);
        Assert.IsNotNull(eventArgs);
        Assert.AreEqual(result, eventArgs.Result);
    }

    [TestMethod]
    public void RaiseTestResultShouldThrowExceptionIfNullTestResultEventArgsIsPassed()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _loggerEvents.RaiseTestResult(null!));
    }

    [TestMethod]
    public void RaiseTestRunMessageShouldThrowExceptioIfNullTestRunMessageEventArgsIsPassed()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _loggerEvents.RaiseTestRunMessage(null!));
    }

    [TestMethod]
    public void CompleteTestRunShouldInvokeRegisteredEventHandler()
    {
        bool testRunCompleteReceived = false;
        TestRunCompleteEventArgs? eventArgs = null;

        EventWaitHandle waitHandle = new AutoResetEvent(false);

        // Register for the test run complete event.
        _loggerEvents.TestRunComplete += (sender, e) =>
        {
            testRunCompleteReceived = true;
            eventArgs = e;
            waitHandle.Set();
        };

        _loggerEvents.EnableEvents();
        // Send the test run complete event.
        _loggerEvents.CompleteTestRun(null, false, false, null, null, null, new TimeSpan());

        var waitSuccess = waitHandle.WaitOne(500);
        Assert.IsTrue(waitSuccess, "Event must be raised within timeout.");
        Assert.IsTrue(testRunCompleteReceived);
        Assert.IsNotNull(eventArgs);
    }

    [TestMethod]
    public void EnableEventsShouldSendEventsAlreadyPresentInQueueToRegisteredEventHandlers()
    {
        bool testResultReceived = false;
        bool testMessageReceived = false;

        // Send the events.
        _loggerEvents.RaiseTestRunMessage(new TestRunMessageEventArgs(TestMessageLevel.Error, "This is a string."));
        _loggerEvents.RaiseTestResult(new TestResultEventArgs(new TestResult(new TestCase("This is a string.", new Uri("some://uri"), "DummySourceFileName"))));

        // Register for the events.
        _loggerEvents.TestResult += (sender, e) => testResultReceived = true;

        _loggerEvents.TestRunMessage += (sender, e) => testMessageReceived = true;

        // Enable events and verify that the events are received.
        _loggerEvents.EnableEvents();

        Assert.IsTrue(testResultReceived);
        Assert.IsTrue(testMessageReceived);
    }

    [TestMethod]
    public void DisposeShouldNotThrowExceptionIfCalledMultipleTimes()
    {
        var loggerEvents = GetDisposedLoggerEvents();
        loggerEvents.Dispose();
    }

    [TestMethod]
    public void RaiseTestResultShouldThrowExceptionIfDisposedIsAlreadyCalled()
    {
        var loggerEvents = GetDisposedLoggerEvents();

        Assert.ThrowsException<ObjectDisposedException>(() => loggerEvents.RaiseTestResult(new TestResultEventArgs(new TestResult(new TestCase("This is a string.", new Uri("some://uri"), "DummySourceFileName")))));
    }

    [TestMethod]
    public void RaiseTestRunMessageShouldThrowExceptionIfDisposeIsAlreadyCalled()
    {
        var loggerEvents = GetDisposedLoggerEvents();

        Assert.ThrowsException<ObjectDisposedException>(() => loggerEvents.RaiseTestRunMessage(new TestRunMessageEventArgs(TestMessageLevel.Error, "This is a string.")));
    }

    [TestMethod]
    public void CompleteTestRunShouldThrowExceptionIfAlreadyDisposed()
    {
        var loggerEvents = GetDisposedLoggerEvents();

        Assert.ThrowsException<ObjectDisposedException>(() => loggerEvents.CompleteTestRun(null, true, false, null, null, null, new TimeSpan()));
    }

    [TestMethod]
    public void EnableEventsShouldThrowExceptionIfAlreadyDisposed()
    {
        var loggerEvents = GetDisposedLoggerEvents();

        Assert.ThrowsException<ObjectDisposedException>(() => loggerEvents.EnableEvents());
    }

    [TestMethod]
    public void TestRunMessageLoggerProxySendMessageShouldInvokeRegisteredEventHandler()
    {
        var receivedRunMessage = false;
        using (_loggerEvents)
        {
            _loggerEvents.TestRunMessage += (sender, e) => receivedRunMessage = true;

            _testSessionMessageLogger.SendMessage(TestMessageLevel.Error, "This is a string.");
        }

        Assert.IsTrue(receivedRunMessage);
    }

    [TestMethod]
    public void TestLoggerProxySendMessageShouldNotInvokeRegisterdEventHandlerIfAlreadyDisposed()
    {
        var receivedRunMessage = false;
        _loggerEvents.TestRunMessage += (sender, e) => receivedRunMessage = true;

        // Dispose the logger events, send a message, and verify it is not received.
        _loggerEvents.Dispose();
        _testSessionMessageLogger.SendMessage(TestMessageLevel.Error, "This is a string.");

        Assert.IsFalse(receivedRunMessage);
    }

    /// <summary>
    /// Exception should be thrown if event args passed is null.
    /// </summary>
    [TestMethod]
    public void RaiseDiscoveryStartShouldThrowExceptionIfNullDiscoveryStartEventArgsIsPassed()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _loggerEvents.RaiseDiscoveryStart(null!));
    }

    /// <summary>
    /// Exception should be thrown if discovered tests event args is null.
    /// </summary>
    [TestMethod]
    public void RaiseDiscoveredTestsShouldThrowExceptionIfNullDiscoveredTestsEventArgsIsPassed()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _loggerEvents.RaiseDiscoveredTests(null!));
    }

    /// <summary>
    /// Exception should be thrown if logger events are already disposed.
    /// </summary>
    [TestMethod]
    public void RaiseDiscoveredTestsShouldThrowExceptionIfAlreadyDisposed()
    {
        var loggerEvents = GetDisposedLoggerEvents();
        List<TestCase> testCases = new() { new TestCase("This is a string.", new Uri("some://uri"), "DummySourceFileName") };
        DiscoveredTestsEventArgs discoveredTestsEventArgs = new(testCases);

        Assert.ThrowsException<ObjectDisposedException>(() => loggerEvents.RaiseDiscoveredTests(discoveredTestsEventArgs));
    }

    /// <summary>
    /// Check for invocation to registered event handlers.
    /// </summary>
    [TestMethod]
    public void RaiseDiscoveredTestsShouldInvokeRegisteredEventHandler()
    {
        bool discoveredTestsReceived = false;
        DiscoveredTestsEventArgs? receivedEventArgs = null;
        EventWaitHandle waitHandle = new AutoResetEvent(false);

        List<TestCase> testCases = new() { new TestCase("This is a string.", new Uri("some://uri"), "DummySourceFileName") };
        DiscoveredTestsEventArgs discoveredTestsEventArgs = new(testCases);

        // Register for the discovered tests event.
        _loggerEvents.DiscoveredTests += (sender, e) =>
        {
            discoveredTestsReceived = true;
            receivedEventArgs = e;
            waitHandle.Set();
        };

        _loggerEvents.EnableEvents();
        // Send the discovered tests event.
        _loggerEvents.RaiseDiscoveredTests(discoveredTestsEventArgs);

        var waitSuccess = waitHandle.WaitOne(500);
        Assert.IsTrue(waitSuccess, "Event must be raised within timeout.");
        Assert.IsTrue(discoveredTestsReceived);
        Assert.IsNotNull(receivedEventArgs);
        Assert.AreEqual(receivedEventArgs, discoveredTestsEventArgs);
    }

    /// <summary>
    /// Exception should be thrown if event args passed is null.
    /// </summary>
    [TestMethod]
    public void RaiseDiscoveryCompleteShouldThrowExceptionIfNullDiscoveryCompleteEventArgsIsPassed()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _loggerEvents.RaiseDiscoveryComplete(null!));
    }

    /// <summary>
    /// Exception should be thrown if logger events are already disposed.
    /// </summary>
    [TestMethod]
    public void RaiseDiscoveryStartShouldThrowExceptionIfAlreadyDisposed()
    {
        var loggerEvents = GetDisposedLoggerEvents();
        DiscoveryCriteria discoveryCriteria = new() { TestCaseFilter = "Name=Test1" };
        DiscoveryStartEventArgs discoveryStartEventArgs = new(discoveryCriteria);

        Assert.ThrowsException<ObjectDisposedException>(() => loggerEvents.RaiseDiscoveryStart(discoveryStartEventArgs));
    }

    /// <summary>
    /// Exception should be thrown if logger events are already disposed.
    /// </summary>
    [TestMethod]
    public void RaiseDiscoveryCompleteShouldThrowExceptionIfAlreadyDisposed()
    {
        var loggerEvents = GetDisposedLoggerEvents();
        DiscoveryCompleteEventArgs discoveryCompleteEventArgs = new(2, false);

        Assert.ThrowsException<ObjectDisposedException>(() => loggerEvents.RaiseDiscoveryComplete(discoveryCompleteEventArgs));
    }

    /// <summary>
    /// Check for invocation to registered event handlers.
    /// </summary>
    [TestMethod]
    public void RaiseDiscoveryStartShouldInvokeRegisteredEventHandler()
    {
        bool discoveryStartReceived = false;
        DiscoveryStartEventArgs? receivedEventArgs = null;
        EventWaitHandle waitHandle = new AutoResetEvent(false);

        DiscoveryCriteria discoveryCriteria = new() { TestCaseFilter = "Name=Test1" };
        DiscoveryStartEventArgs discoveryStartEventArgs = new(discoveryCriteria);

        // Register for the discovery start event.
        _loggerEvents.DiscoveryStart += (sender, e) =>
        {
            discoveryStartReceived = true;
            receivedEventArgs = e;
            waitHandle.Set();
        };

        _loggerEvents.EnableEvents();
        // Send the discovery start event.
        _loggerEvents.RaiseDiscoveryStart(discoveryStartEventArgs);

        var waitSuccess = waitHandle.WaitOne(500);
        Assert.IsTrue(waitSuccess, "Event must be raised within timeout.");
        Assert.IsTrue(discoveryStartReceived);
        Assert.IsNotNull(receivedEventArgs);
        Assert.AreEqual(receivedEventArgs, discoveryStartEventArgs);
        Assert.AreEqual("Name=Test1", receivedEventArgs.DiscoveryCriteria.TestCaseFilter);
    }

    /// <summary>
    /// Check for invocation to registered event handlers.
    /// </summary>
    [TestMethod]
    public void RaiseDiscoveryCompleteShouldInvokeRegisteredEventHandler()
    {
        bool discoveryCompleteReceived = false;
        DiscoveryCompleteEventArgs? receivedEventArgs = null;
        EventWaitHandle waitHandle = new AutoResetEvent(false);

        DiscoveryCompleteEventArgs discoveryCompleteEventArgs = new(2, false);

        // Register for the discovery complete event.
        _loggerEvents.DiscoveryComplete += (sender, e) =>
        {
            discoveryCompleteReceived = true;
            receivedEventArgs = e;
            waitHandle.Set();
        };

        _loggerEvents.EnableEvents();
        // Send the discovery complete event.
        _loggerEvents.RaiseDiscoveryComplete(discoveryCompleteEventArgs);

        var waitSuccess = waitHandle.WaitOne(500);
        Assert.IsTrue(waitSuccess, "Event must be raised within timeout.");
        Assert.IsTrue(discoveryCompleteReceived);
        Assert.IsNotNull(receivedEventArgs);
        Assert.AreEqual(receivedEventArgs, discoveryCompleteEventArgs);
    }

    /// <summary>
    /// Exception should be thrown if event args passed is null.
    /// </summary>
    [TestMethod]
    public void RaiseTestRunStartShouldThrowExceptionIfNullTestRunStartEventArgsIsPassed()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _loggerEvents.RaiseTestRunStart(null!));
    }

    /// <summary>
    /// Exception should be thrown if event args passed is null.
    /// </summary>
    [TestMethod]
    public void RaiseDiscoveryMessageShouldThrowExceptionIfNullTestRunMessageEventArgsIsPassed()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _loggerEvents.RaiseDiscoveryMessage(null!));
    }

    /// <summary>
    /// Exception should be thrown if logger events are already disposed.
    /// </summary>
    [TestMethod]
    public void RaiseTestRunStartShouldThrowExceptionIfAlreadyDisposed()
    {
        var loggerEvents = GetDisposedLoggerEvents();
        TestRunCriteria testRunCriteria = new(new List<string> { @"x:dummy\foo.dll" }, 10, false, string.Empty, TimeSpan.MaxValue, null, "Name=Test1", null);
        TestRunStartEventArgs testRunStartEventArgs = new(testRunCriteria);

        Assert.ThrowsException<ObjectDisposedException>(() => loggerEvents.RaiseTestRunStart(testRunStartEventArgs));
    }

    /// <summary>
    /// Exception should be thrown if logger events are already disposed.
    /// </summary>
    [TestMethod]
    public void RaiseDiscoveryMessageShouldThrowExceptionIfAlreadyDisposed()
    {
        var loggerEvents = GetDisposedLoggerEvents();
        string message = "This is the test message";
        TestRunMessageEventArgs testRunMessageEventArgs = new(TestMessageLevel.Informational, message);

        Assert.ThrowsException<ObjectDisposedException>(() => loggerEvents.RaiseDiscoveryMessage(testRunMessageEventArgs));
    }

    /// <summary>
    /// Check for invocation to registered event handlers.
    /// </summary>
    [TestMethod]
    public void RaiseTestRunStartShouldInvokeRegisteredEventHandler()
    {
        bool testRunStartReceived = false;
        TestRunStartEventArgs? receivedEventArgs = null;
        EventWaitHandle waitHandle = new AutoResetEvent(false);

        TestRunCriteria testRunCriteria = new(new List<string> { @"x:dummy\foo.dll" }, 10, false, string.Empty, TimeSpan.MaxValue, null, "Name=Test1", null);
        TestRunStartEventArgs testRunStartEventArgs = new(testRunCriteria);

        // Register for the test run start event.
        _loggerEvents.TestRunStart += (sender, e) =>
        {
            testRunStartReceived = true;
            receivedEventArgs = e;
            waitHandle.Set();
        };

        _loggerEvents.EnableEvents();
        // Send the test run start event.
        _loggerEvents.RaiseTestRunStart(testRunStartEventArgs);

        var waitSuccess = waitHandle.WaitOne(500);
        Assert.IsTrue(waitSuccess, "Event must be raised within timeout.");
        Assert.IsTrue(testRunStartReceived);
        Assert.IsNotNull(receivedEventArgs);
        Assert.AreEqual(receivedEventArgs, testRunStartEventArgs);
        Assert.AreEqual("Name=Test1", receivedEventArgs.TestRunCriteria.TestCaseFilter);
    }

    /// <summary>
    /// Check for invocation to registered event handlers.
    /// </summary>
    [TestMethod]
    public void RaiseDiscoveryMessageShouldInvokeRegisteredEventHandler()
    {
        bool discoveryMessageReceived = false;
        TestRunMessageEventArgs? receivedEventArgs = null;
        EventWaitHandle waitHandle = new AutoResetEvent(false);

        string message = "This is the test message";
        TestRunMessageEventArgs testRunMessageEventArgs = new(TestMessageLevel.Informational, message);

        // Register for the discovery message event.
        _loggerEvents.DiscoveryMessage += (sender, e) =>
        {
            discoveryMessageReceived = true;
            receivedEventArgs = e;
            waitHandle.Set();
        };

        _loggerEvents.EnableEvents();
        // Send the discovery message event.
        _loggerEvents.RaiseDiscoveryMessage(testRunMessageEventArgs);

        var waitSuccess = waitHandle.WaitOne(500);
        Assert.IsTrue(waitSuccess, "Event must be raised within timeout.");
        Assert.IsTrue(discoveryMessageReceived);
        Assert.IsNotNull(receivedEventArgs);
        Assert.AreEqual(receivedEventArgs, testRunMessageEventArgs);
        Assert.AreEqual(message, receivedEventArgs.Message);
        Assert.AreEqual(TestMessageLevel.Informational, receivedEventArgs.Level);
    }

    /// <summary>
    /// Gets a disposed instance of the logger events.
    /// </summary>
    /// <returns>Disposed instance.</returns>
    private InternalTestLoggerEvents GetDisposedLoggerEvents()
    {
        var loggerEvents = new InternalTestLoggerEvents(_testSessionMessageLogger);
        loggerEvents.Dispose();

        return loggerEvents;
    }
}
