// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Execution;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Adapter;

/// <summary>
/// The test execution recorder used for recording test results and test messages.
/// </summary>
internal class TestExecutionRecorder : TestSessionMessageLogger, ITestExecutionRecorder
{
    private readonly List<AttachmentSet> _attachmentSets;
    private readonly ITestRunCache _testRunCache;
    private readonly ITestCaseEventsHandler? _testCaseEventsHandler;

    /// <summary>
    /// Contains TestCase Ids for test cases that are in progress
    /// Start has been recorded but End has not yet been recorded.
    /// </summary>
    private readonly HashSet<Guid> _testCaseInProgressMap;

    private readonly object _testCaseInProgressSyncObject = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="TestExecutionRecorder"/> class.
    /// </summary>
    /// <param name="testCaseEventsHandler"> The test Case Events Handler. </param>
    /// <param name="testRunCache"> The test run cache.  </param>
    public TestExecutionRecorder(ITestCaseEventsHandler? testCaseEventsHandler, ITestRunCache testRunCache)
    {
        _testRunCache = testRunCache;
        _testCaseEventsHandler = testCaseEventsHandler;
        _attachmentSets = new List<AttachmentSet>();

        // As a framework guideline, we should get events in this order:
        // 1. Test Case Start.
        // 2. Test Case End.
        // 3. Test Case Result.
        // If that is not that case.
        // If Test Adapters don't send the events in the above order, Test Case Results are cached till the Test Case End event is received.
        _testCaseInProgressMap = new HashSet<Guid>();
    }

    /// <summary>
    /// Gets the attachments received from adapters.
    /// </summary>
    internal Collection<AttachmentSet> Attachments
    {
        get
        {
            return new Collection<AttachmentSet>(_attachmentSets);
        }
    }

    /// <summary>
    /// Notify the framework about starting of the test case.
    /// Framework sends this event to data collectors enabled in the run. If no data collector is enabled, then the event is ignored.
    /// </summary>
    /// <param name="testCase">test case which will be started.</param>
    public void RecordStart(TestCase testCase)
    {
        EqtTrace.Verbose("TestExecutionRecorder.RecordStart: Starting test: {0}.", testCase.FullyQualifiedName);
        _testRunCache.OnTestStarted(testCase);

        if (_testCaseEventsHandler != null)
        {
            lock (_testCaseInProgressSyncObject)
            {
                // Do not send TestCaseStart for a test in progress
                if (!_testCaseInProgressMap.Contains(testCase.Id))
                {
                    _testCaseInProgressMap.Add(testCase.Id);
                    _testCaseEventsHandler.SendTestCaseStart(testCase);
                }
            }
        }
    }

    /// <summary>
    /// Notify the framework about the test result.
    /// </summary>
    /// <param name="testResult">Test Result to be sent to the framework.</param>
    /// <exception cref="TestCanceledException">Exception thrown by the framework when an executor attempts to send
    /// test result to the framework when the test(s) is canceled. </exception>
    public void RecordResult(TestResult testResult)
    {
        EqtTrace.Verbose("TestExecutionRecorder.RecordResult: Received result for test: {0}.", testResult.TestCase.FullyQualifiedName);
        if (_testCaseEventsHandler != null)
        {
            // Send TestCaseEnd in case RecordEnd was not called.
            SendTestCaseEnd(testResult.TestCase, testResult.Outcome);
            _testCaseEventsHandler.SendTestResult(testResult);
        }

        // Test Result should always be flushed, even if datacollecter attachment is missing
        _testRunCache.OnNewTestResult(testResult);
    }

    /// <summary>
    /// Notify the framework about completion of the test case.
    /// Framework sends this event to data collectors enabled in the run. If no data collector is enabled, then the event is ignored.
    /// </summary>
    /// <param name="testCase">test case which has completed.</param>
    /// <param name="outcome">outcome of the test case.</param>
    public void RecordEnd(TestCase testCase, TestOutcome outcome)
    {
        EqtTrace.Verbose("TestExecutionRecorder.RecordEnd: test: {0} execution completed.", testCase.FullyQualifiedName);
        _testRunCache.OnTestCompletion(testCase);
        SendTestCaseEnd(testCase, outcome);
    }

    /// <summary>
    /// Send TestCaseEnd event for given testCase if not sent already
    /// </summary>
    /// <param name="testCase"></param>
    /// <param name="outcome"></param>
    private void SendTestCaseEnd(TestCase testCase, TestOutcome outcome)
    {
        if (_testCaseEventsHandler != null)
        {
            lock (_testCaseInProgressSyncObject)
            {
                // TestCaseEnd must always be preceded by TestCaseStart for a given test case id
                if (_testCaseInProgressMap.Contains(testCase.Id))
                {
                    // Send test case end event to handler.
                    _testCaseEventsHandler.SendTestCaseEnd(testCase, outcome);

                    // Remove it from map so that we send only one TestCaseEnd for every TestCaseStart.
                    _testCaseInProgressMap.Remove(testCase.Id);
                }
            }
        }
    }

    /// <summary>
    /// Notify the framework about run level attachments.
    /// </summary>
    /// <param name="attachments"> The attachment sets. </param>
    public void RecordAttachments(IList<AttachmentSet> attachments)
    {
        _attachmentSets.AddRange(attachments);
    }
}
