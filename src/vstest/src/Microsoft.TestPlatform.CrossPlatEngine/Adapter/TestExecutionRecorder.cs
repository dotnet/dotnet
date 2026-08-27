// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Execution;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.Utilities;

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
    /// Tracks the number of in-progress starts per test case ID.
    /// Multiple data-driven test executions sharing the same ID are each counted.
    /// </summary>
    private readonly Dictionary<Guid, int> _testCaseInProgressMap;

    /// <summary>
    /// Tracks explicit end events that have not yet been paired with a result.
    /// This prevents <see cref="RecordResult"/> from sending a duplicate end event while
    /// allowing another execution with the same ID to use the result safety net.
    /// </summary>
    /// <remarks>
    /// Pairing uses reference equality, so <see cref="RecordEnd"/> and <see cref="RecordResult"/>
    /// must receive the same <see cref="TestCase"/> instance for an execution.
    /// <para>
    /// An adapter may also reuse one instance for several executions that share an id. Start and end
    /// counts stay balanced in that case, because a pending end is consumed by the next result for that
    /// instance. The executions are no longer told apart though, so an end event can carry the outcome
    /// of the other execution when results arrive in a different order than the executions ran. Pass a
    /// distinct instance per execution to keep outcomes attributed.
    /// </para>
    /// </remarks>
    private readonly Dictionary<TestCase, int> _testCaseEndCalledMap;

    private readonly object _testCaseInProgressSyncObject = new();
    private readonly bool _disableMultipleTestCaseEvents;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestExecutionRecorder"/> class.
    /// </summary>
    /// <param name="testCaseEventsHandler"> The test Case Events Handler. </param>
    /// <param name="testRunCache"> The test run cache.  </param>
    public TestExecutionRecorder(ITestCaseEventsHandler? testCaseEventsHandler, ITestRunCache testRunCache)
        : this(testCaseEventsHandler, testRunCache, FeatureFlag.Instance)
    {
    }

    internal TestExecutionRecorder(ITestCaseEventsHandler? testCaseEventsHandler, ITestRunCache testRunCache, IFeatureFlag featureFlag)
    {
        _testRunCache = testRunCache;
        _testCaseEventsHandler = testCaseEventsHandler;
        _attachmentSets = new List<AttachmentSet>();
        _disableMultipleTestCaseEvents = featureFlag.IsSet(FeatureFlag.VSTEST_DISABLE_MULTIPLE_TESTCASE_EVENTS);

        // As a framework guideline, we should get events in this order:
        // 1. Test Case Start.
        // 2. Test Case End.
        // 3. Test Case Result.
        // If that is not that case.
        // If Test Adapters don't send the events in the above order, Test Case Results are cached till the Test Case End event is received.
        _testCaseInProgressMap = new Dictionary<Guid, int>();
        _testCaseEndCalledMap = new Dictionary<TestCase, int>(TestCaseReferenceComparer.Instance);
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
                bool isAlreadyInProgress = _testCaseInProgressMap.TryGetValue(testCase.Id, out int count);
                if (_disableMultipleTestCaseEvents && isAlreadyInProgress)
                {
                    return;
                }

                _testCaseInProgressMap[testCase.Id] = isAlreadyInProgress ? count + 1 : 1;
                _testCaseEventsHandler.SendTestCaseStart(testCase);
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
            lock (_testCaseInProgressSyncObject)
            {
                if (_testCaseEndCalledMap.TryGetValue(testResult.TestCase, out int endCount))
                {
                    if (endCount == 1)
                    {
                        _testCaseEndCalledMap.Remove(testResult.TestCase);
                    }
                    else
                    {
                        _testCaseEndCalledMap[testResult.TestCase] = endCount - 1;
                    }
                }
                else
                {
                    SendTestCaseEnd(testResult.TestCase, testResult.Outcome, explicitEnd: false);
                }
            }

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

        if (_testCaseEventsHandler != null)
        {
            lock (_testCaseInProgressSyncObject)
            {
                SendTestCaseEnd(testCase, outcome, explicitEnd: true);
            }
        }
    }

    private void SendTestCaseEnd(TestCase testCase, TestOutcome outcome, bool explicitEnd)
    {
        if (!_testCaseInProgressMap.TryGetValue(testCase.Id, out int count))
        {
            return;
        }

        _testCaseEventsHandler!.SendTestCaseEnd(testCase, outcome);

        if (explicitEnd)
        {
            _testCaseEndCalledMap[testCase] = _testCaseEndCalledMap.TryGetValue(testCase, out int endCount) ? endCount + 1 : 1;
        }

        if (count == 1)
        {
            _testCaseInProgressMap.Remove(testCase.Id);
        }
        else
        {
            _testCaseInProgressMap[testCase.Id] = count - 1;
        }
    }

    private sealed class TestCaseReferenceComparer : IEqualityComparer<TestCase>
    {
        public static readonly TestCaseReferenceComparer Instance = new();

        public bool Equals(TestCase? x, TestCase? y) => ReferenceEquals(x, y);

        public int GetHashCode(TestCase obj) => RuntimeHelpers.GetHashCode(obj);
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
